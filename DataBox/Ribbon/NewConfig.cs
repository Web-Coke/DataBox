using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using DataBox.LANGUAGE.Ribbon;
using DataBox.LANGUAGE.Ribbon.Fun;
using DataBox.RustFun;
using DataBox.RustFun.Ribbon;
using Excel = Microsoft.Office.Interop.Excel;

namespace DataBox.Ribbon
{
    public partial class NewConfig : Form
    {
        public const int MaxLine = 255;

        public NewConfig()
        {
            InitializeComponent();
            SheetNames.HeaderText = NewConfigText.SheetName;
            DataAddrs.HeaderText = NewConfigText.DataAddr;
            DataNames.HeaderText = NewConfigText.DataName;
            FileOperation.Text = NewConfigText.SavedState;
            NewFile.Text = NewConfigText.NewFile;
            OpenFile.Text = NewConfigText.OpenFile;
            SaveFile.Text = NewConfigText.SaveFile;
            SaveAs.Text = NewConfigText.SaveAs;
            SelectOpenFile.Filter = NewConfigText.Filter;
            SelectOpenFile.Title = NewConfigText.SelectOpenFile;
            SelectSaveFile.Filter = NewConfigText.Filter;
            SelectSaveFile.Title = NewConfigText.SelectSaveFile;
            SelectFolder.Description = NewConfigText.SelectFolder;
            Text = NewConfigText.NewConfig;
        }

        private static string SetTheString(IntPtr Ptr, int Len)
        {
            unsafe
            {
                return new string((char*)Ptr, 0, Len);
            }
        }

        private void SetDataGridView(string SheetName, string DataAddr, string DataName)
        {
            if (DataGridView.Rows.Count < MaxLine)
            {
                DataGridView.Rows.Add(SheetName, DataAddr, DataName);
            }
        }

        private void BECRead()
        {
            BatchExtractionRS.BECRead(new RefObj(ConfigFilePath), SetTheString, SetDataGridView);
        }

        private RefObj GetDataGridView(int Index)
        {
            return new RefObj(
                new string[]
                {
                    (string)DataGridView[0, Index].Value,
                    (string)DataGridView[1, Index].Value,
                    (string)DataGridView[2, Index].Value,
                }
            );
        }

        private void BECWrite()
        {
            BatchExtractionRS.BECWrite(
                new RefObj(ConfigFilePath),
                DataGridView.Rows.Count,
                GetDataGridView
            );
        }

        private string ConfigFilePath = string.Empty;
        private bool ConfigChanges = false;

        private void DataGridView_CellClick(object Sender, DataGridViewCellEventArgs CellClickEvent)
        {
            if (
                CellClickEvent.RowIndex == -1
                && CellClickEvent.ColumnIndex != -1
                && DataGridView.Rows.Count < MaxLine
            )
            {
                if (
                    Operation.SelectCell<Excel.Range>(
                        BatchExtraction.Category,
                        NewConfigText.SelectDataCell,
                        8,
                        NewConfigText.SelectDataCellErr
                    )
                        is Excel.Range Range
                    && Operation.SelectCell<string>(
                        BatchExtraction.Category,
                        NewConfigText.InputDataName,
                        2,
                        NewConfigText.InputDataNameErr
                    )
                        is string DataName
                )
                {
                    DataGridView.Rows.Add(
                        Range.Worksheet.Name,
                        Range.Address.Split(':')[0],
                        DataName
                    );
                }
            }
        }

        private void DataGridView_RowsAdded(object Sender, DataGridViewRowsAddedEventArgs RowEvent)
        {
            DataGridView.Rows[RowEvent.RowIndex].HeaderCell.Style.Alignment =
                DataGridViewContentAlignment.MiddleRight;
            DataGridView.Rows[RowEvent.RowIndex].HeaderCell.Value = BatchExtractionRS
                .Conversion(RowEvent.RowIndex + 1)
                .ToString();
            if (!ConfigChanges)
            {
                ConfigChanges = true;
                FileOperation.Text = NewConfigText.NotSavedState;
            }
        }

        private void DataGridView_RowsRemoved(
            object Sender,
            DataGridViewRowsRemovedEventArgs RowEvent
        )
        {
            if (RowEvent.RowIndex <= DataGridView.Rows.Count)
            {
                Parallel.For(
                    RowEvent.RowIndex,
                    DataGridView.Rows.Count,
                    delegate(int Index)
                    {
                        DataGridView.Rows[Index].HeaderCell.Value = BatchExtractionRS
                            .Conversion(Index + 1)
                            .ToString();
                    }
                );
                if (!ConfigChanges)
                {
                    ConfigChanges = true;
                    FileOperation.Text = NewConfigText.NotSavedState;
                }
            }
        }

        private void NewFile_Click(object Sender, EventArgs Event)
        {
            new NewConfig().Show();
        }

        private void OpenFile_Click(object Sender, EventArgs Event)
        {
            if (SelectOpenFile.ShowDialog() == DialogResult.OK)
            {
                ConfigFilePath = SelectOpenFile.FileName;
            }
            else
            {
                System.Windows.MessageBox.Show(
                    NewConfigText.CancelOperation,
                    BatchExtraction.Category,
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
                return;
            }
            if (ConfigChanges)
            {
                SaveFile_Click(Sender, Event);
            }
            if (!ConfigChanges)
            {
                DataGridView.Rows.Clear();
                try
                {
                    BECRead();
                }
                catch
                {
                    System.Windows.MessageBox.Show(
                        NewConfigText.ReadConfigFileErr,
                        BatchExtraction.Category,
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                    DataGridView.Rows.Clear();
                }
            }
        }

        private void SaveFile_Click(object Sender, EventArgs Event)
        {
            if (ConfigFilePath == string.Empty)
            {
                if (SelectSaveFile.ShowDialog() == DialogResult.OK)
                {
                    ConfigFilePath = SelectSaveFile.FileName;
                }
                else
                {
                    System.Windows.MessageBox.Show(
                        NewConfigText.UnsaveConfigFile,
                        BatchExtraction.Category,
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                    return;
                }
            }
            try
            {
                BECWrite();
                if (ConfigChanges)
                {
                    ConfigChanges = false;
                    FileOperation.Text = NewConfigText.SavedState;
                }
            }
            catch
            {
                System.Windows.MessageBox.Show(
                    NewConfigText.SaveConfigFileErr,
                    BatchExtraction.Category,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void SaveAs_Click(object Sender, EventArgs Event)
        {
            ConfigFilePath = string.Empty;
            SaveFile_Click(Sender, Event);
        }
    }
}
