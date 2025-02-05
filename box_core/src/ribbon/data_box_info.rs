use crate::{RefObj, VERSION};
use reqwest::blocking::Client;
use serde::{Deserialize, Serialize};
use std::time::Duration;

const UPDATE_URL_ZH_CN: &str = "https://gitee.com/api/v5/repos/Web-Coke/DataBox/releases/latest";
const UPDATE_URL_EN_US: &str = "https://api.github.com/repos/Web-Coke/DataBox/releases/latest";

#[derive(Deserialize, Serialize)]
struct Releases {
    tag_name: Option<String>,
}

#[no_mangle]
pub unsafe extern "C-unwind" fn CoreVersion() -> RefObj {
    RefObj::from_str(&*VERSION)
}

#[no_mangle]
pub unsafe extern "C-unwind" fn IsThereNewVersion_ZH_CN() -> bool {
    Client::builder()
        .timeout(Duration::from_secs(15))
        .build()
        .unwrap()
        .get(UPDATE_URL_ZH_CN)
        .send()
        .unwrap()
        .json::<Releases>()
        .unwrap()
        .tag_name
        .unwrap()
        == *VERSION
}

#[no_mangle]
pub unsafe extern "C-unwind" fn IsThereNewVersion_EN_US() -> bool {
    Client::builder()
        .timeout(Duration::from_secs(15))
        .build()
        .unwrap()
        .get(UPDATE_URL_EN_US)
        .send()
        .unwrap()
        .json::<Releases>()
        .unwrap()
        .tag_name
        .unwrap()
        == *VERSION
}
