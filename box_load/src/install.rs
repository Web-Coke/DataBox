use std::fs::{create_dir_all, write};

use crate::{Resources, ALL_RESOURCES, INSTALL_DIR};

fn write_resources() {
    if !INSTALL_DIR.exists() && !INSTALL_DIR.is_dir() {
        match create_dir_all(&*INSTALL_DIR) {
            Ok(()) => println!("创建成功 -> 目录{}创建成功", INSTALL_DIR.display()),
            Err(err) => {
                println!(
                    "创建失败 -> 目录{}创建失败:{err}, 尝试直接写入文件",
                    INSTALL_DIR.display()
                );
            }
        }
    }
    for Resources { name, data } in ALL_RESOURCES {
        if let Err(err) = write(INSTALL_DIR.join(name), data) {
            println!("写入失败 -> 资源{name}写入失败:{err}")
        } else {
            println!("写入失败 -> 资源{name}写入成功");
        }
    }
}
