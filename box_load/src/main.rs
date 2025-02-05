use std::{
    env::var_os,
    ffi::{c_void, CStr},
    io::{stdin, Read},
    path::{Path, PathBuf},
    process::exit,
    sync::LazyLock,
    thread::sleep,
    time::Duration,
};
use windows_targets::link;

mod install;
mod unstall;

link!("kernel32.dll" "system" fn Process32Next(hsnapshot: *mut c_void, lppe: *mut PROCESSENTRY32) -> i32);
link!("kernel32.dll" "system" fn Process32First(hsnapshot: *mut c_void, lppe: *mut PROCESSENTRY32) -> i32);
link!("kernel32.dll" "system" fn CreateToolhelp32Snapshot(dwflags: u32, th32processid: u32) -> *mut c_void);

#[allow(non_snake_case)]
#[repr(C)]
struct PROCESSENTRY32 {
    dwSize: u32,
    cntUsage: u32,
    th32ProcessID: u32,
    th32DefaultHeapID: usize,
    th32ModuleID: u32,
    cntThreads: u32,
    th32ParentProcessID: u32,
    pcPriClassBase: i32,
    dwFlags: u32,
    szExeFile: [i8; 260],
}

struct Process {
    hsnapshot: *mut c_void,
    lppe: *mut PROCESSENTRY32,
}

impl Default for Process {
    fn default() -> Self {
        Self {
            hsnapshot: unsafe { CreateToolhelp32Snapshot(2, 0) },
            lppe: Box::into_raw(Box::new(PROCESSENTRY32 {
                dwSize: size_of::<PROCESSENTRY32>() as u32,
                cntUsage: 0,
                th32ProcessID: 0,
                th32DefaultHeapID: 0,
                th32ModuleID: 0,
                cntThreads: 0,
                th32ParentProcessID: 0,
                pcPriClassBase: 0,
                dwFlags: 0,
                szExeFile: [0; 260],
            })),
        }
    }
}

impl Process {
    fn next(&mut self) -> bool {
        unsafe { Process32Next(self.hsnapshot, self.lppe) != 0 }
    }

    fn first(&mut self) -> bool {
        unsafe { Process32First(self.hsnapshot, self.lppe) != 0 }
    }

    fn get_name<'a>(&self) -> &'a str {
        unsafe {
            CStr::from_ptr((*self.lppe).szExeFile.as_ptr())
                .to_str()
                .unwrap()
        }
    }
}

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
    Path::new(match var_os("PROGRAMDATA") {
        Some(ref path) => path.to_str().unwrap(),
        None => {
            println!("\t获取失败 -> 获取路径失败, 尝试使用默认路径'C:\\ProgramData'");
            "C:\\ProgramData"
        }
    })
    .join("DataBox")
});

// 检测目标程序是否正在运行(Excel, WPS ET)
// Check if the target(Excel, WPS ET) program is running
fn is_run() {
    let mut process = Process::default();
    if process.first() {
        while process.next() {
            let process_name = process.get_name();
            if process_name == "EXCEL.EXE"
                || process_name == "et.exe"
                || process_name == "EXCEL"
                || process_name == "et"
            {
                println!("程序将在15秒后退出:\n\t检测到当前电脑正在运行WPS或Excel, 请关闭WPS或Excel后再次运行此程序");
                sleep(Duration::from_secs(15));
                exit(0)
            }
        }
    }
}

fn main() {
    is_run();
    let mut buf = [0; 1];
    println!("{}", "*".repeat(64));
    println!("DataBox基本信息:");
    println!("\tDataBox当前版本-> {}", {
        let mut version = env!("CARGO_PKG_VERSION").as_bytes().to_vec();
        version.push(0);
        if let [.., dot, end] = &mut version[..] {
            *end = *dot;
            *dot = 46;
        };
        unsafe { String::from_utf8_unchecked(version) }
    });
    println!("\tDataBox安装路径-> {}", INSTALL_DIR.display());
    loop {
        println!("{}", "*".repeat(64));
        println!("请选择要进行的操作, 输入数字后按回车键确认");
        println!("\t1.安装DataBox");
        println!("\t2.卸载DataBox");
        _ = stdin().read_exact(&mut buf);
        if buf[0] != b'1' && buf[0] != b'2' {
            _ = stdin().read_line(&mut String::new());
            println!("请输入数字1或2");
        } else {
            _ = stdin().read_line(&mut String::new());
            break;
        }
    }
    if buf[0] == b'1' {
        install()
    } else {
        unstall()
    }
}

fn install() {
    install::write_resources()
}

fn unstall() {
    unstall::remome_resources()
}
