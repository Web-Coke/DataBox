use crate::RefObj;
use std::ffi::c_void;

const ZH_CN_UPDATE_URL: &str = "https://gitee.com/api/v5/repos/Web-Coke/DataBox/releases/latest";
const EN_US_UPDATE_URL: &str = "https://api.github.com/repos/Web-Coke/DataBox/releases/latest";

struct Releases {
    tag_name: Option<String>,
}

#[no_mangle]
pub unsafe extern "C-unwind" fn CoreVersion() -> RefObj {
    let mut version = env!("CARGO_PKG_VERSION")
        .encode_utf16()
        .collect::<Vec<u16>>();
    version.push(0);
    if let [.., dot, end] = &mut version[..] {
        *end = *dot;
        *dot = 46;
    };
    RefObj {
        len: i32::try_from(version.len()).unwrap(),
        rty: 2,
        ptr: Box::into_raw(version.into_boxed_slice()) as *mut c_void,
    }
}

fn IsThereNewVersion() -> bool {
    true
}

#[no_mangle]
pub unsafe extern "C-unwind" fn ZH_CN_IsThereNewVersion(){

}

#[no_mangle]
pub unsafe extern "C-unwind" fn EN_US_IsThereNewVersion(){

}