using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using AddInReg;
using Microsoft.Win32;

class Program
{
    static readonly string BoxPath =
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\DataBox\\";

    static void ProcessesISRun()
    {
        if (
            Process.GetProcessesByName("EXCEL.EXE").Length > 0
            || Process.GetProcessesByName("et.exe").Length > 0
            || Process.GetProcessesByName("EXCEL").Length > 0
            || Process.GetProcessesByName("et").Length > 0
        )
        {
            Console.WriteLine(
                "程序将在15秒后退出:\n  检测到当前电脑正在运行WPS或Excel, 请关闭WPS或Excel后再次运行此程序"
            );
            Thread.Sleep(15 * 1000);
            Environment.Exit(0);
            Process.GetCurrentProcess().Kill();
        }
        //30-43行代码在下版中删除
        string _Path = string.Empty;
        try
        {
            _Path =
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
                + "\\TowerTool";
            if (Directory.Exists(_Path))
                Directory.Delete(_Path, true);
        }
        catch
        {
            Console.WriteLine($"  删除失败 -> 路径\n{_Path}\n 删除失败, 请尝试手动删除");
        }
        Console.WriteLine("卸载旧版加载项中...");
        Excel.UnInstall();
        WPSET.UnInstall();
        Console.WriteLine("旧版加载项卸载完毕!\n");
        Console.WriteLine($"加载项信息");
        Console.WriteLine($"  加载项版本 -> {Assembly.GetExecutingAssembly().GetName().Version}");
        Console.WriteLine($"  加载项目录 -> {BoxPath}\n");
    }

    static void DllXllInstall()
    {
        Console.WriteLine("开始解压资源");
        string[] Resource = new string[]
        {
            "Core32.dll",
            "Core64.dll",
            "DataBox-AddIn-packed.xll",
            "DataBox-AddIn64-packed.xll",
        };
        Assembly Application = Assembly.GetExecutingAssembly();
        foreach (string Item in Resource)
        {
            try
            {
                using (
                    Stream DllStream =
                        Application.GetManifestResourceStream(
                            $"{new AssemblyName(Application.FullName ?? string.Empty).Name}.File.{Item}"
                        ) ?? throw new ApplicationException("无法获取嵌入的资源")
                )
                {
                    DllStream.CopyTo(File.Create($"{BoxPath}{Item}"));
                }
                Console.WriteLine($"  解压成功 -> 资源 {Item} 解压成功!");
            }
            catch (Exception Err)
            {
                Console.WriteLine($"  解压失败 -> 资源 {Item} 解压失败:\n  {Err}");
            }
        }
        Console.WriteLine("资源解压完成\n");
    }

    static void ExcelInstall()
    {
        Console.WriteLine("  自动安装 -> 尝试自动安装Excel加载项中...");
        try
        {
            RegistryKey Key = Registry.CurrentUser.OpenSubKey(
                @"SOFTWARE\Microsoft\Office\16.0\Excel\",
                true
            );
            if (Key?.GetValue("ExcelName") != null)
            {
                AddInReg.Excel.Install(BoxPath + "DataBox-AddIn64-packed.xll");
                Console.WriteLine($"  安装成功 -> Excel安装加载项成功");
            }
            else
            {
                Console.WriteLine("  安装失败 -> 未在此电脑上发现Excel, 如存在请手动安装");
            }
            Key?.Close();
        }
        catch (Exception Err)
        {
            Console.WriteLine($"  安装错误 -> 为Excel安装加载项时发生了一个错误:\n  {Err}");
        }
    }

    static void WPSETInstall()
    {
        Console.WriteLine("  自动安装 -> 尝试自动安装WPS加载项中...");
        try
        {
            RegistryKey Key = Registry.CurrentUser.OpenSubKey(
                @"SOFTWARE\kingsoft\Office\6.0\Common",
                true
            );
            object WPSVersion = Key?.GetValue("Version");
            if (WPSVersion != null)
            {
                (string XllName, string CPUArchitecture) = (
                    (Key?.GetValue("Architecture") ?? "X86").ToString() ?? string.Empty
                ).Equals("X86", StringComparison.OrdinalIgnoreCase)
                    ? ("DataBox-AddIn-packed.xll", "32位")
                    : ("DataBox-AddIn64-packed.xll", "64位");
                Key = Registry.CurrentUser.OpenSubKey(
                    @"SOFTWARE\kingsoft\Office\6.0\et\LoadMacros",
                    true
                );
                Key?.SetValue(BoxPath + XllName, 1, RegistryValueKind.String);
                WPSET.Install(BoxPath + XllName);
                Console.WriteLine(
                    $"  安装成功 -> WPS安装加载项成功, WPS版本: {WPSVersion}, {CPUArchitecture}"
                );
            }
            else
            {
                Console.WriteLine("  安装失败 -> 未在此电脑上发现WPS, 如存在请手动安装");
            }
            Key?.Close();
        }
        catch (Exception Err)
        {
            Console.WriteLine($"  安装错误 -> 为WPS安装加载项时发生了一个错误:\n  {Err}");
        }
    }

    static void Main()
    {
        if (!Directory.Exists(BoxPath))
            Directory.CreateDirectory(BoxPath);
        ProcessesISRun();
        DllXllInstall();
        Console.WriteLine("开始自动安装");
        ExcelInstall();
        WPSETInstall();
        Console.WriteLine("自动安装结束\n");
        Console.WriteLine("操作完成 -> 操作完成, 输入任意键退出");
        Console.ReadLine();
    }
}
