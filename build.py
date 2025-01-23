from os import remove, system
from os.path import dirname, exists, join
from re import findall
from shutil import move

UTF8 = "utf-8"

with open("Cargo.toml", "r", encoding=UTF8) as toml:
    [toml.readline() for _ in range(5)]
    Version = ".".join(list(findall(r"(?<=\").*(?=\")", toml.readline())[0].replace(".", "")))
    print(f"Core版本: {Version}")
    with open(r"DataBox\Properties\AssemblyInfo.cs", "r", encoding=UTF8) as AssemblyInfo:
        Lines = AssemblyInfo.readlines()
        Lines[31] = f'[assembly: AssemblyVersion("{Version}")]\n'
    with open(r"DataBox\Properties\AssemblyInfo.cs", "w", encoding=UTF8) as AssemblyInfo:
        AssemblyInfo.writelines(Lines)
input("是否更新依赖库?(Y/-):\n") == "Y" and system("cargo update")
RootPath = join(dirname(__file__), "target")
[
    print(f"编译{E[0]}中...")
    or system(f"cargo build --release -p tool_core --target {E[1]}")
    or exists(Destination := join(RootPath, "release", E[0]))
    and remove(Destination)
    or move(join(RootPath, E[1], "release", "core.dll"), Destination)
    for E in [["Core32.dll", "i686-pc-windows-msvc"], ["Core64.dll", "x86_64-pc-windows-msvc"]]
]
