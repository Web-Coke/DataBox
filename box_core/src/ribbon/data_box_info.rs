use crate::{RefObj, VERSION};
use reqwest::blocking::Client;
use serde::Deserialize;
use std::time::Duration;

const ZH_CN_UPDATE_URL: &str = "https://gitee.com/api/v5/repos/Web-Coke/DataBox/releases/latest";
const EN_US_UPDATE_URL: &str = "https://api.github.com/repos/Web-Coke/DataBox/releases/latest";

#[derive(Deserialize)]
struct Releases {
    tag_name: Option<String>,
}

#[no_mangle]
pub unsafe extern "C-unwind" fn CoreVersion() -> RefObj {
    RefObj::from_str(&*VERSION)
}

#[no_mangle]
pub unsafe extern "C-unwind" fn ZH_CN_IsThereNewVersion() -> bool {
    Client::builder()
        .timeout(Duration::from_secs(15))
        .build()
        .unwrap()
        .get(ZH_CN_UPDATE_URL)
        .send()
        .unwrap()
        .json::<Releases>()
        .unwrap()
        .tag_name
        .unwrap()
        == *VERSION
}

#[no_mangle]
pub unsafe extern "C-unwind" fn EN_US_IsThereNewVersion() -> bool {
    Client::builder()
        .timeout(Duration::from_secs(15))
        .build()
        .unwrap()
        .get(EN_US_UPDATE_URL)
        .send()
        .unwrap()
        .json::<Releases>()
        .unwrap()
        .tag_name
        .unwrap()
        == *VERSION
}
