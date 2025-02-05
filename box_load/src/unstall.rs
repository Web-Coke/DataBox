use std::fs::remove_file;

use crate::{Resources, ALL_RESOURCES, INSTALL_DIR};

pub fn remome_resources() {
    if INSTALL_DIR.exists() && INSTALL_DIR.is_dir() {
        for Resources { name, .. } in ALL_RESOURCES {
            let file_path = INSTALL_DIR.join(name);
            if file_path.exists() {
                if let Err(err) = remove_file(INSTALL_DIR.join(name)) {
                    println!("删除失败 -> 资源{name}删除失败:{err}")
                } else {
                    println!("删除成功 -> 资源{name}删除成功");
                }
            } else {
                println!("删除失败 -> 资源{name}不存在或无法访问, 已跳过该资源");
            }
        }
    }
}
