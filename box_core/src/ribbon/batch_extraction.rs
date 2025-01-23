use crate::RefObj;
use serde::{Deserialize, Serialize};
use serde_json::{from_reader, to_writer};
use std::{collections::HashMap, ffi::c_void, fs::File, io::BufReader};

#[derive(Deserialize, Serialize)]
struct Config {
    pub data_addr: String,
    pub data_name: String,
}

impl Config {
    #[no_mangle]
    pub unsafe extern "C-unwind" fn BECRead(
        config_file_path: RefObj,
        set_string: extern "C-unwind" fn(*mut c_void, i32) -> *mut c_void,
        action: extern "C-unwind" fn(*mut c_void, *mut c_void, *mut c_void),
    ) {
        let buffer = BufReader::new(
            File::options()
                .read(true)
                .open(config_file_path.into_str())
                .unwrap(),
        );
        match from_reader::<BufReader<File>, Option<HashMap<String, Vec<Config>>>>(buffer).unwrap()
        {
            Some(hmap) => {
                let hmap = (hmap).iter();
                for (key, values) in hmap {
                    let sheet_name = key.encode_utf16().collect::<Vec<u16>>();
                    let ptr = sheet_name.as_ptr() as *mut c_void;
                    let len = i32::try_from(sheet_name.len()).unwrap();
                    for info in values {
                        let data_addr = info.data_addr.encode_utf16().collect::<Vec<u16>>();
                        let data_name = info.data_name.encode_utf16().collect::<Vec<u16>>();
                        action(
                            set_string(ptr, len),
                            set_string(
                                data_addr.as_ptr() as *mut c_void,
                                i32::try_from(data_addr.len()).unwrap(),
                            ),
                            set_string(
                                data_name.as_ptr() as *mut c_void,
                                i32::try_from(data_name.len()).unwrap(),
                            ),
                        )
                    }
                }
            }
            None => return,
        }
    }

    #[no_mangle]
    pub unsafe extern "C-unwind" fn BECWrite(
        config_file_path: RefObj,
        count: i32,
        action: unsafe extern "C-unwind" fn(i32) -> RefObj,
    ) {
        let file = File::options()
            .create(true)
            .write(true)
            .truncate(true)
            .open(config_file_path.into_str())
            .unwrap();
        let mut hmap: HashMap<String, Vec<Config>> = HashMap::with_capacity(16);
        for index in 0..count {
            let info = action(index).into_str_arr();
            match hmap.get_mut(info.get_unchecked(0)) {
                Some(values) => {
                    values.push(Config {
                        data_addr: info.get_unchecked(1).to_string(),
                        data_name: info.get_unchecked(2).to_string(),
                    });
                }
                None => {
                    let mut values = Vec::with_capacity(255);
                    values.push(Config {
                        data_addr: info.get_unchecked(1).to_string(),
                        data_name: info.get_unchecked(2).to_string(),
                    });
                    hmap.insert(info.get_unchecked(0).to_string(), values);
                }
            }
        }
        to_writer(file, &hmap).unwrap();
    }
}

#[no_mangle]
pub unsafe extern "C-unwind" fn Conversion(mut index: i32) -> RefObj {
    let mut chars = Vec::with_capacity(4);
    while index > 0 {
        index -= 1;
        chars.push((index % 26 + 65) as u8);
        index /= 26;
    }
    chars.reverse();
    RefObj::from_str(String::from_utf8_unchecked(chars))
}

#[cfg(test)]
mod test {
    use super::*;
    #[test]
    fn test_conversion() {
        unsafe {
            for i in 1..255 {
                println!("{}", Conversion(i).into_str());
            }
        }
    }
}
