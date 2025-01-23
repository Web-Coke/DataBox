use crate::RefObj;
use reqwest::blocking::{Client, Response};
use serde::Deserialize;
use std::time::Duration;

const MAX_ITERATIONS: u64 = 200;
const SEMI_MAJOR_AXIS: f64 = 6378137.0;
const FLATTENING: f64 = 1.0 / 298.257223563;
const CONVERGENCE_THRESHOLD: f64 = 0.000000000001;
const SEMI_MINOR_AXIS: f64 = 6_356_752.314_245_179;

struct Point {
    longitude: f64,
    latitude: f64,
}

impl Point {
    fn new(longitude: f64, latitude: f64) -> Self {
        Point {
            longitude,
            latitude,
        }
    }

    fn distance(self, other: Self) -> f64 {
        let (point_i, point_n) = (self, other);
        let u1 = f64::atan((1.0 - FLATTENING) * f64::tan(f64::to_radians(point_i.latitude)));
        let u2 = f64::atan((1.0 - FLATTENING) * f64::tan(f64::to_radians(point_n.latitude)));
        let l = f64::to_radians(point_i.longitude - point_n.longitude);
        let mut lambda = l;
        let mut cos_sqalpha = 0.0;
        let mut sin_sigma = 0.0;
        let mut cos2_sigma_m = 0.0;
        let mut cos_sigma = 0.0;
        let mut sigma = 0.0;

        let sin_u1 = f64::sin(u1);
        let cos_u1 = f64::cos(u1);
        let sin_u2 = f64::sin(u2);
        let cos_u2 = f64::cos(u2);

        for _ in 0..MAX_ITERATIONS {
            let sin_lambda = f64::sin(lambda);
            let cos_lambda = f64::cos(lambda);
            sin_sigma = f64::sqrt(
                (cos_u2 * sin_lambda).powf(2.0)
                    + (cos_u1 * sin_u2 - sin_u1 * cos_u2 * cos_lambda).powf(2.0),
            );
            if sin_sigma == 0.0 {
                return 0.0;
            };
            cos_sigma = sin_u1 * sin_u2 + cos_u1 * cos_u2 * cos_lambda;
            sigma = f64::atan2(sin_sigma, cos_sigma);
            let sin_alpha = cos_u1 * cos_u2 * sin_lambda / sin_sigma;
            cos_sqalpha = 1.0 - sin_alpha.powf(2.0);
            cos2_sigma_m = if cos_sigma == 0.0 {
                0.0
            } else {
                cos_sigma - 2.0 * sin_u1 * sin_u2 / cos_sqalpha
            };
            let c =
                FLATTENING / 16.0 * cos_sqalpha * (4.0 + FLATTENING * (4.0 - 3.0 * cos_sqalpha));
            let lambda_prev = lambda;
            lambda = l
                + (1.0 - c)
                    * FLATTENING
                    * sin_alpha
                    * (sigma
                        + c * sin_sigma
                            * (cos2_sigma_m
                                + c * cos_sigma * (-1.0 + 2.0 * cos2_sigma_m.powf(2.0))));
            if f64::abs(lambda - lambda_prev) < CONVERGENCE_THRESHOLD {
                break;
            }
        }

        let usq = cos_sqalpha * (SEMI_MAJOR_AXIS.powf(2.0) - SEMI_MINOR_AXIS.powf(2.0))
            / (SEMI_MINOR_AXIS.powf(2.0));
        let a = 1.0 + usq / 16384.0 * (4096.0 + usq * (-768.0 + usq * (320.0 - 175.0 * usq)));
        let b = usq / 1024.0 * (256.0 + usq * (-128.0 + usq * (74.0 - 47.0 * usq)));
        let delta_sigma = b
            * sin_sigma
            * (cos2_sigma_m
                + b / 4.0
                    * (cos_sigma * (-1.0 + 2.0 * cos2_sigma_m.powf(2.0))
                        - b / 6.0
                            * cos2_sigma_m
                            * (-3.0 + 4.0 * sin_sigma.powf(2.0))
                            * (-3.0 + 4.0 * cos2_sigma_m.powf(2.0))));
        SEMI_MINOR_AXIS * a * (sigma - delta_sigma)
    }
}

#[derive(Deserialize)]
struct JsonPOI {
    info: Option<String>,
    regeocode: Option<JsonRegeocode>,
}

#[allow(non_snake_case)]
#[derive(Deserialize)]
struct JsonRegeocode {
    formatted_address: Option<String>, //内蒙古自治区阿拉善盟阿拉善左旗腾格里额里斯镇沙之船露营酒店
    addressComponent: Option<JsonAddressComponent>,
}

#[derive(Deserialize)]
struct JsonAddressComponent {
    province: Option<String>, //内蒙古自治区
    city: JsonDeser,          //阿拉善盟
    district: Option<String>, //阿拉善左旗
    township: Option<String>, //腾格里额里斯镇
}

#[derive(Deserialize)]
#[serde(untagged)]
enum JsonDeser {
    Array(),
    String(String),
    None,
}

impl JsonPOI {
    unsafe fn into_array(self) -> RefObj {
        let info = self.info.unwrap();
        let arr = if info.to_uppercase() == "OK" {
            let regeocode = self.regeocode.unwrap();
            let address_component = regeocode.addressComponent.unwrap();
            vec![
                address_component.province.unwrap(),
                if let JsonDeser::String(info) = address_component.city {
                    info
                } else {
                    "".to_string()
                },
                address_component.district.unwrap(),
                address_component.township.unwrap(),
                regeocode.formatted_address.unwrap(),
            ]
        } else {
            vec![format!("#Error:{}", info)]
        };
        RefObj::from_str_arr(arr)
    }
}

#[derive(Deserialize)]
struct JsonPoint {
    info: Option<String>,
    locations: Option<String>,
}

impl JsonPoint {
    unsafe fn into_array(self) -> RefObj {
        let info = self.info.unwrap();
        let arr = if info.to_uppercase() == "OK" {
            let locations = self.locations.unwrap();
            let mut locat_iter = locations.split(",");
            vec![
                locat_iter.next().unwrap_or("").to_string(),
                locat_iter.next().unwrap_or("").to_string(),
            ]
        } else {
            vec![format!("#Error:{}", info)]
        };
        RefObj::from_str_arr(arr)
    }
}

#[inline(always)]
fn amap_request(uri: String) -> Response {
    Client::builder()
        .timeout(Duration::from_secs(15))
        .build()
        .unwrap()
        .get(format!("https://restapi.amap.com{}", uri))
        .send()
        .unwrap()
}

#[no_mangle]
pub unsafe extern "C-unwind" fn Distance(lon1: f64, lat1: f64, lon2: f64, lat2: f64) -> f64 {
    Point::new(lon1, lat1).distance(Point::new(lon2, lat2))
}

#[no_mangle]
pub unsafe extern "C-unwind" fn GetPOI(uri: RefObj) -> RefObj {
    amap_request(uri.into_str())
        .json::<JsonPOI>()
        .unwrap()
        .into_array()
}

#[no_mangle]
pub unsafe extern "C-unwind" fn ToGCJ02(uri: RefObj) -> RefObj {
    amap_request(uri.into_str())
        .json::<JsonPoint>()
        .unwrap()
        .into_array()
}

#[cfg(test)]
mod test {
    use super::*;

    #[test]
    fn distance_test() {
        let point_i = Point {
            longitude: -71.0693514,
            latitude: 42.3541165,
        };
        let point_n = Point {
            longitude: -73.9680804,
            latitude: 40.7791472,
        };
        println!("Distance: {}", point_i.distance(point_n));

        let point_i = Point {
            longitude: 0.0,
            latitude: 63.0,
        };
        let point_n = Point {
            longitude: 0.0,
            latitude: 64.0,
        };
        println!("Distance: {}", point_i.distance(point_n));
    }

    #[test]
    fn get_poitest() {
        unsafe {
            let ret = GetPOI(RefObj::from_str(
                "/v3/geocode/regeo?key=&location=116.481488,39.990464&radius=50&extensions=base",
            ))
            .into_str_arr();
            for i in ret {
                println!("{}", i);
            }
        }
    }

    #[test]
    fn to_gcj02_test() {
        unsafe {
            let ret = ToGCJ02(RefObj::from_str(
                "/v3/assistant/coordinate/convert?key=&locations=116.481488,39.990464&coordsys=gps",
            ))
            .into_str_arr();
            for i in ret {
                println!("{}", i);
            }
        }
    }
}
