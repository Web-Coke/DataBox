#![cfg(test)]
use std::ffi::c_void;

use crate::RefObj;

#[no_mangle]
pub unsafe extern "C-unwind" fn MemoryLeak(test_str: RefObj) -> RefObj {
    let test_str = test_str.into_str();
    let obj: Vec<u16> = test_str.encode_utf16().collect::<Vec<u16>>();
    let mut big_obj: Vec<RefObj> = Vec::with_capacity(1024);
    for _ in 0..1024 {
        let o = obj.clone();
        big_obj.push(RefObj {
            len: i32::try_from(o.len()).unwrap(),
            rty: 2,
            ptr: Box::into_raw(o.into_boxed_slice()) as *mut c_void,
        });
    }
    RefObj {
        len: 1024,
        rty: 0,
        ptr: Box::into_raw(big_obj.into_boxed_slice()) as *mut c_void,
    }
}
