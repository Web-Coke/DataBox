use crate::RefObj;
use regex::Regex;
use std::ffi::c_void;

#[no_mangle]
pub unsafe extern "C-unwind" fn RegexCreate(pattern: RefObj) -> *mut Regex {
    Box::into_raw(Box::new(Regex::new(&pattern.into_str()).unwrap()))
}

#[no_mangle]
pub unsafe extern "C-unwind" fn IsMatch(input: RefObj, pattern: *mut Regex) -> bool {
    (*pattern).is_match(&input.into_str())
}

#[no_mangle]
pub unsafe extern "C-unwind" fn Matches(input: RefObj, pattern: RefObj, name: RefObj) -> RefObj {
    let name = if name.len == 0 { "" } else { &name.into_str() };
    let ret = if name.is_empty() {
        Regex::new(&pattern.into_str())
            .unwrap()
            .find_iter(&input.into_str())
            .map(|caps| RefObj::from_str(caps.as_str()))
            .collect::<Vec<RefObj>>()
    } else {
        Regex::new(&pattern.into_str())
            .unwrap()
            .captures_iter(&input.into_str())
            .filter_map(|caps| caps.name(name).map(|cap| RefObj::from_str(cap.as_str())))
            .collect::<Vec<RefObj>>()
    };
    RefObj {
        len: i32::try_from(ret.len()).unwrap(),
        rty: 0,
        ptr: Box::into_raw(ret.into_boxed_slice()) as *mut c_void,
    }
}

#[no_mangle]
pub unsafe extern "C-unwind" fn Replaces(
    input: RefObj,
    pattern: *mut Regex,
    replace: *mut String,
) -> RefObj {
    RefObj::from_str((*pattern).replace_all(&input.into_str(), &*replace))
}

#[no_mangle]
pub unsafe extern "C-unwind" fn RegexDispose(pattern: *mut Regex) {
    drop(Box::from_raw(pattern))
}

#[no_mangle]
pub unsafe extern "C-unwind" fn fixed_str(input: RefObj) -> *mut String {
    Box::into_raw(Box::new(input.into_str()))
}

#[no_mangle]
pub unsafe extern "C-unwind" fn drop_str(input: *mut String) {
    drop(Box::from_raw(input))
}

#[cfg(test)]
mod test {
    use super::*;
    macro_rules! get_test_time {
        ($expr:expr) => {
            let times = std::time::Instant::now();
            $expr
            let end = times.elapsed();
            println!("{:?}", end);
        };
    }

    #[test]
    fn is_match_test() {
        get_test_time!({
            unsafe {
                assert!(IsMatch(
                    RefObj::from_str(r"2024/11/01 08:00:00"),
                    RegexCreate(RefObj::from_str(r"^[[:ascii:]]*$"))
                ));
            }
        });
        get_test_time!({
            unsafe {
                assert!(IsMatch(
                    RefObj::from_str(r"I categorically deny having triskaidekaphobia."),
                    RegexCreate(RefObj::from_str(r"\b\w{13}\b"))
                ));
            }
        });
    }

    #[test]
    fn matches_test() {
        unsafe {
            let ret = Matches(
                RefObj::from_str(
                    "匹配电子邮箱的示例:web-chang@foxmail.com火狐邮箱\nGoogle@gmail.com谷歌邮箱",
                ),
                RefObj::from_str(r"[A-Za-z0-9\-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+"),
                RefObj::from_str(""),
            )
            .into_str_arr();
            for i in ret {
                println!("{}", i);
            }

            let ret = Matches(
                RefObj::from_str(
                    "根据身份证提取生日的示例:\n510000200005109876\n460000199612116789",
                ),
                RefObj::from_str(r"[1-9]\d{5}(?<Num>\d{8})\d{3}[0-9Xx]"),
                RefObj::from_str("Num"),
            )
            .into_str_arr();
            for i in ret {
                println!("{}", i);
            }
        }
    }
}
