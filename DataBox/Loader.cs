using System;
using System.Windows;
using DataBox.RustFun;
using ExcelDna.Integration;
using ExcelDna.IntelliSense;
using Excel = Microsoft.Office.Interop.Excel;

namespace DataBox
{
    public class IntelliSenseAddIn : IExcelAddIn
    {
        public static readonly Excel.Application MainApplication = (Excel.Application)
            ExcelDnaUtil.Application;

        public void AutoOpen()
        {
            Link.Install();
            WPSReg.WPSRegistry();
            //RegMenu();
            MainApplication.SheetSelectionChange += SelectCell;
            //MainApplication.OnKey("%{.}", "AdaptiveCell");
            IntelliSenseServer.Install();
        }

        public void AutoClose()
        {
            IntelliSenseServer.Uninstall();
            //MainApplication.OnKey("%{.}", "");
            MainApplication.SheetSelectionChange -= SelectCell;
            //UnRegMenu();
            WPSReg.WPSUnRegistry();
            Link.UnInstall();
            GC.Collect();
        }

        private static void SelectCell(object Sh, Excel.Range Target)
        {
            MainApplication.StatusBar = ((Excel.Worksheet)Sh).Name + "!" + Target.Address;
        }

        //WPS不兼容
        //WPS Office is not compatible
        //private static void RegMenu()
        //{
        //    Office.CommandBar Bar = MainApplication.CommandBars["cell"];
        //    UnRegMenu();
        //    Office.CommandBarControl Control = Bar
        //        .Controls.Cast<Office.CommandBarControl>()
        //        .FirstOrDefault(BarControl => BarControl.Caption == Loader.AdaptiveCellSize);
        //    if (Control == null)
        //    {
        //        Control = Bar.Controls.Add(
        //            Type: Office.MsoControlType.msoControlButton,
        //            Before: 1,
        //            Temporary: false
        //        );
        //        Control.Caption = Loader.AdaptiveCellSize;
        //        Control.OnAction = "AdaptiveCell";
        //    }
        //}

        //private static void UnRegMenu()
        //{
        //    Office.CommandBar Bar = MainApplication.CommandBars["cell"];
        //    while (true)
        //    {
        //        try
        //        {
        //            Bar.Controls[Loader.AdaptiveCellSize].Delete();
        //        }
        //        catch
        //        {
        //            break;
        //        }
        //    }
        //}

        //[ExcelCommand(MenuName ="AdaptiveCell", MenuText = Loader.AdaptiveCellSize)]
        //public static void AdaptiveCell()
        //{
        //    if (MainApplication != null)
        //    {
        //        MainApplication.EnableEvents = false;
        //        Excel.Range Selection = (Excel.Range)MainApplication.Selection;
        //        if (Selection != null)
        //        {
        //            Selection.Rows.AutoFit();
        //            Selection.Columns.AutoFit();
        //        }
        //        MainApplication.EnableEvents = true;
        //    }
        //}
    }

    public static class Operation
    {
        public static object SelectCell<T>(string Title, string Prompt, object Type, string Msg)
        {
            Retry:
            try
            {
                T t = (T)
                    IntelliSenseAddIn.MainApplication.InputBox(
                        Prompt: Prompt,
                        Title: Title,
                        Type: Type
                    );
                return t;
            }
            catch
            {
                if (
                    MessageBox.Show(
                        Msg,
                        Title,
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Information
                    ) == MessageBoxResult.Yes
                )
                    goto Retry;
                return null;
            }
        }
    }
}
