const CROE32: &[u8] = include_bytes!(r"../.././target/release/Core32.dll");
const CROE64: &[u8] = include_bytes!(r"../.././target/release/Core64.dll");
const DATA_BOX32: &[u8] = include_bytes!(r"../../DataBox/bin/Release/DataBox-AddIn-packed.xll");
const DATA_BOX64: &[u8] = include_bytes!(r"../../DataBox/bin/Release/DataBox-AddIn64-packed.xll");

fn main() {
    println!("Hello, world!");
}
