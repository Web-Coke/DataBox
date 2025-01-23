using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using DataBox.LANGUAGE.Ribbon;
using DataBox.LANGUAGE.Ribbon.Fun;
using DataBox.RustFun;
using DataBox.RustFun.Ribbon;
using Excel = Microsoft.Office.Interop.Excel;

namespace DataBox.Ribbon.Fun
{
    public static class BatchExtractionCS
    {
        private struct Config
        {
            public string DataAddr;
            public string DataColumn;

            public Config(string DA, string DC)
            {
                DataAddr = DA;
                DataColumn = DC;
            }
        }

        private static readonly int[] Index = new int[] { 1, 2 };
        private static readonly Dictionary<string, List<Config>> Configs = new Dictionary<
            string,
            List<Config>
        >(NewConfig.MaxLine);
        private static Excel.Worksheet InfoSheet;
        private static Excel.Worksheet ResultsSheet;

        private static string SetTheString(IntPtr Ptr, int Len)
        {
            unsafe
            {
                return new string((char*)Ptr, 0, Len);
            }
        }

        private static void AddConfig(string SheetName, string DataAddr, string DataName)
        {
            if (Index[0] < NewConfig.MaxLine)
            {
                string DataColumn = BatchExtractionRS.Conversion(Index[0]).ToString();
                if (Configs.ContainsKey(SheetName))
                {
                    Configs[SheetName].Add(new Config(DataAddr, DataColumn));
                }
                else
                {
                    var Vector = new List<Config> { Capacity = NewConfig.MaxLine };
                    Vector.Add(new Config(DataAddr, DataColumn));
                    Configs.Add(SheetName, Vector);
                }
                ResultsSheet.Range[$"{DataColumn}1"].Value = DataName;
                Index[0] += 1;
            }
        }

        private static void BECRead(string ConfigFilePath)
        {
            BatchExtractionRS.BECRead(new RefObj(ConfigFilePath), SetTheString, AddConfig);
        }

        private static object SelectConfig()
        {
            NewConfig Select = new NewConfig();
            return Select.SelectOpenFile.ShowDialog() == DialogResult.OK
                ? Select.SelectOpenFile.FileName
                : null;
        }

        private static object SelectFolder()
        {
            NewConfig Select = new NewConfig();
            return Select.SelectFolder.ShowDialog() == DialogResult.OK
                ? Select.SelectFolder.SelectedPath
                : null;
        }

        private static void Initialization(string ConfigFilePath)
        {
            Excel.Sheets Sheets = IntelliSenseAddIn.MainApplication.Workbooks.Add().Sheets;
            ResultsSheet = (Excel.Worksheet)Sheets["Sheet1"];
            ResultsSheet.Name = BatchExtraction.ResultsSheetName;
            ResultsSheet.Cells.NumberFormatLocal = "@";
            InfoSheet = (Excel.Worksheet)Sheets.Add(After: ResultsSheet);
            InfoSheet.Name = BatchExtraction.InfoSheetName;
            InfoSheet.Range["A1"].Value = BatchExtraction.InfoSheetA1Value;
            InfoSheet.Range["B1"].Value = BatchExtraction.InfoSheetB1Value;
            InfoSheet.Range["C1"].Value = BatchExtraction.InfoSheetC1Value;
            InfoSheet.Range["D1"].Value = BatchExtraction.InfoSheetD1Value;
            BECRead(ConfigFilePath);
            Index[0] = 2;
        }

        private static void WriteInfo(string File)
        {
            InfoSheet.Range[$"A{Index[1]}"].Value = BatchExtraction.Successes;
            InfoSheet.Range[$"B{Index[1]}"].Value = File;
            Index[1] += 1;
        }

        private static void WriteInfo(string File, string Description, Exception Err)
        {
            InfoSheet.Range[$"A{Index[1]}"].Value = BatchExtraction.Failures;
            InfoSheet.Range[$"B{Index[1]}"].Value = File;
            InfoSheet.Range[$"C{Index[1]}"].Value = Description;
            InfoSheet.Range[$"D{Index[1]}"].Value = $"{Err}";
            Index[1] += 1;
        }

        private static void Extraction(string[] AllExcelFile)
        {
            Excel.Application ExcelApplication = new Excel.Application() { Visible = false };
            foreach (string File in AllExcelFile)
            {
                Excel.Workbook WorkBook;
                Excel.Worksheet CurrentSheet;
                try
                {
                    WorkBook = ExcelApplication.Workbooks.Open(File, ReadOnly: true);
                }
                catch (Exception Err)
                {
                    WriteInfo(File, BatchExtraction.OpenFileFailures, Err);
                    continue;
                }
                foreach (KeyValuePair<string, List<Config>> KV in Configs)
                {
                    try
                    {
                        CurrentSheet = (Excel.Worksheet)WorkBook.Sheets[KV.Key];
                    }
                    catch (Exception Err)
                    {
                        WriteInfo(File, BatchExtraction.OpenSheetFailures(KV.Key), Err);
                        continue;
                    }
                    for (int RowIdx = 0; RowIdx < KV.Value.Count; RowIdx++)
                    {
                        try
                        {
                            ResultsSheet.Range[$"{KV.Value[RowIdx].DataColumn}{Index[0]}"].Value =
                                CurrentSheet.Range[KV.Value[RowIdx].DataAddr].Value;
                        }
                        catch (Exception Err)
                        {
                            WriteInfo(File, BatchExtraction.CellRWFailures, Err);
                        }
                    }
                    WriteInfo(File);
                    Index[0] += 1;
                }
                WorkBook.Close(SaveChanges: false);
            }
            ExcelApplication.Quit();
        }

        public static void Start()
        {
            if (SelectConfig() is string ConfigFilePath && SelectFolder() is string FolderPath)
            {
                Initialization(ConfigFilePath);
                Extraction(Directory.GetFiles(FolderPath, "*.xls"));
            }
            else
            {
                System.Windows.MessageBox.Show(
                    NewConfigText.CancelOperation,
                    BatchExtraction.Category,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
        }
    }
}
