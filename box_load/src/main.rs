use std::{
    mem::transmute,
    path::{Path, PathBuf},
    process::{Command, ExitStatus, Output},
    sync::LazyLock,
};

mod install;
mod unstall;

struct Resources {
    name: &'static str,
    data: &'static [u8],
}

const ALL_RESOURCES: [Resources; 4] = [
    Resources {
        name: "Core32.dll",
        data: include_bytes!("..\\..\\.\\target\\release\\Core32.dll"),
    },
    Resources {
        name: "Core64.dll",
        data: include_bytes!("..\\..\\.\\target\\release\\Core64.dll"),
    },
    Resources {
        name: "DataBox-AddIn-packed.xll",
        data: include_bytes!("..\\..\\DataBox\\bin\\Release\\DataBox-AddIn-packed.xll"),
    },
    Resources {
        name: "DataBox-AddIn64-packed.xll",
        data: include_bytes!("..\\..\\DataBox\\bin\\Release\\DataBox-AddIn64-packed.xll"),
    },
];

static INSTALL_DIR: LazyLock<PathBuf> = LazyLock::new(|| {
    let mut programdata_path = Command::new("cmd")
        .args([
            {
                let mut slice = [b'C', b':', b'\\'];
                let path = Path::new(unsafe { transmute::<&[u8], &str>(&slice) });
                for index in 0x41..=0x5A {
                    slice[0] = index;
                    if path.exists() && path.is_dir() {
                        break;
                    }
                }
                unsafe { transmute::<&[u8], &str>(&[b'\\', slice[0]]) }
            },
            "echo %PROGRAMDATA%",
        ])
        .output()
        .unwrap_or_else(|err| {
            println!("获取失败 -> 获取路径失败:{err}, 尝试使用默认路径'C:\\ProgramData'");
            Output {
                status: ExitStatus::default(),
                stdout: b"C:\\ProgramData".to_vec(),
                stderr: vec![],
            }
        })
        .stdout;
    programdata_path.append(&mut b"\\DataBox".to_vec());
    Path::new(unsafe {
        transmute::<Box<[u8]>, &str>(
            programdata_path
                .iter()
                .filter_map(|s| {
                    if s.is_ascii_whitespace() {
                        None
                    } else {
                        Some(*s)
                    }
                })
                .collect::<Vec<_>>()
                .into_boxed_slice(),
        )
    })
    .join("DataBox")
});

fn main() {
    println!("Hello, world!");
}
