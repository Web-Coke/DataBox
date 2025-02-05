use std::fs::{create_dir_all, write};
use std::mem::transmute;
use winreg::enums::HKEY_CURRENT_USER;
use winreg::RegKey;
use crate::{Resources, ALL_RESOURCES, INSTALL_DIR};

pub fn write_resources() {
    println!("写入资源 -> 开始写入资源");
    if !INSTALL_DIR.exists() && !INSTALL_DIR.is_dir() {
        match create_dir_all(&*INSTALL_DIR) {
            Ok(()) => println!("\t创建成功 -> 目录{}创建成功", INSTALL_DIR.display()),
            Err(err) => {
                println!(
                    "\t创建失败 -> 目录{}创建失败:{err}, 尝试直接写入文件",
                    INSTALL_DIR.display()
                );
            }
        }
    }
    for Resources { name, data } in ALL_RESOURCES {
        if let Err(err) = write(INSTALL_DIR.join(name), data) {
            println!("\t写入失败 -> 资源{name}写入失败:{err}")
        } else {
            println!("\t写入失败 -> 资源{name}写入成功");
        }
    }
}

pub fn excel_reg() {
    println!("开始安装 -> 为Excel安装DataBox");
    let hkcu = RegKey::predef(HKEY_CURRENT_USER);
    if let Ok(excel_option) = hkcu.open_subkey(r"SOFTWARE\Microsoft\Office\16.0\Excel\Options") {
        let key = if let Err(_) = excel_option.get_value::<String, &str>("OPEN") {
            "OPEN"
        } else {
            let mut key = "";
            for idx in 1..100 {
                key =
                    unsafe { transmute::<Box<str>, &str>(format!("OPEN{}", idx).into_boxed_str()) };
                if let Err(_) = excel_option.get_value::<String, &str>(key) {
                    break;
                }
            }
            key
        };
        let value = format!(
            r#"/R "{}\DataBox-AddIn64-packed.xll""#,
            INSTALL_DIR.to_str().unwrap()
        );
        match excel_option.set_value(key, &value) {
            Ok(()) => println!("安装成功 -> 为Excel安装DataBox成功"),
            Err(err) => {
                println!("\t写入失败 -> 写入注册表时发生了一个异常:\n\t{err}");
                println!("安装失败 -> 未能在Excel中安装DataBox, 请尝试手动安装")
            }
        };
    } else {
        println!("安装失败 -> 注册表 SOFTWARE\\Microsoft\\Office\\16.0\\Excel\\Options 为空, 此计算机可能未安装Excel, 请尝试手动安装")
    }
}

pub fn wpset_reg() {}

#[cfg(test)]
mod test {
    use super::*;
    #[test]
    fn test_reg() {
        excel_reg();
    }
}
