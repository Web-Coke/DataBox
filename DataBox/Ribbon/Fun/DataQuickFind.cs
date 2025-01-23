using System;
using System.Threading.Tasks;
using System.Windows;
using DataBox.LANGUAGE.Ribbon.Fun;
using DataBox.RustFun;
using DataBox.RustFun.Ribbon;
using ExcelDna.Integration;
using Excel = Microsoft.Office.Interop.Excel;

namespace DataBox.Ribbon.Fun
{
    public static class DataQuickFindCS
    {
        private static IntPtr IndexPtr = IntPtr.Zero;
        private static object[,] DatasArr = null;
        public static bool ISNone => IndexPtr == IntPtr.Zero || DatasArr == null;

        public static void Create()
        {
            Dispose:
            if (!ISNone)
            {
                DataQuickFindRS.DQFDispose(IndexPtr);
                IndexPtr = IntPtr.Zero;
                DatasArr = null;
                return;
            }
            if (
                Operation.SelectCell<Excel.Range>(
                    DataQuickFind.CreateBindingDataTitle,
                    DataQuickFind.CreateBindingDataPrompt,
                    8,
                    DataQuickFind.BindingDataException
                )
                    is Excel.Range TableRange
                && Operation.SelectCell<int>(
                    DataQuickFind.CreateIndexTitle,
                    DataQuickFind.CreateIndexPrompt,
                    1,
                    DataQuickFind.GetInputException
                )
                    is int InputIndex
            )
            {
                if (
                    new ExcelReference(
                        TableRange.Row - 1,
                        TableRange.Row - 1 + TableRange.Rows.Count - 1,
                        TableRange.Column - 1,
                        TableRange.Column - 1 + TableRange.Columns.Count - 1,
                        TableRange.Worksheet.Name
                    ).GetValue()
                    is object[,] Arr
                )
                {
                    DatasArr = Arr;
                }
                else
                {
                    MessageBox.Show(DataQuickFind.OnePieceData, DataQuickFind.Category);
                    return;
                }
                try
                {
                    InputIndex -= 1;
                    IndexPtr = DataQuickFindRS.DQFCreate();
                    Parallel.For(
                        0,
                        DatasArr.GetLength(0),
                        delegate(int Idx)
                        {
                            object Item = DatasArr[Idx, InputIndex];
                            DataQuickFindRS.DQFWrite(
                                IndexPtr,
                                Item is ExcelEmpty
                                    ? new RefObj(string.Empty)
                                    : new RefObj(Item.ToString()),
                                Idx
                            );
                        }
                    );
                }
                catch (IndexOutOfRangeException)
                {
                    MessageBox.Show(DataQuickFind.IndexOutOfRangeException, DataQuickFind.Category);
                    goto Dispose;
                }
                catch
                {
                    MessageBox.Show(DataQuickFind.OtherException, DataQuickFind.Category);
                    goto Dispose;
                }
                MessageBox.Show(DataQuickFind.Succeed, DataQuickFind.Category);
            }
        }

        [ExcelFunction(
            Category = DataQuickFind.Category,
            Description = DataQuickFind.FLOOKUP.Description
        )]
        public static object FLOOKUP(
            [ExcelArgument(Description = DataQuickFind.FLOOKUP.Arg1Description)] string Key,
            [ExcelArgument(Description = DataQuickFind.FLOOKUP.Arg2Description)] int SerialNumber
        )
        {
            SerialNumber -= 1;
            if (ISNone)
            {
                return ExcelError.ExcelErrorNA;
            }
            int[] Addres = DataQuickFindRS.DQFFinds(IndexPtr, new RefObj(Key)).ToNumArray<int>();
            object[] Arr = new object[Addres.Length];
            Parallel.For(
                0,
                Addres.Length,
                delegate(int Idx)
                {
                    object Item = DatasArr[Addres[Idx], SerialNumber];
                    Arr[Idx] = Item is ExcelEmpty ? string.Empty : Item;
                }
            );
            return Arr;
        }

        [ExcelFunction(
            Category = DataQuickFind.Category,
            Description = DataQuickFind.FCOUNTS.Description
        )]
        public static object FCOUNTS(
            [ExcelArgument(Description = DataQuickFind.FCOUNTS.Arg1Description)] string Key
        )
        {
            return ISNone
                ? ExcelError.ExcelErrorNA
                : (object)DataQuickFindRS.DQFFinds(IndexPtr, new RefObj(Key)).Length;
        }

        [ExcelFunction(
            Category = DataQuickFind.Category,
            Description = DataQuickFind.FGETKEY.Description
        )]
        public static object FGETKEY()
        {
            if (ISNone)
            {
                return ExcelError.ExcelErrorNA;
            }
            using (RefObj Keys = DataQuickFindRS.DQFKeys(IndexPtr))
            {
                unsafe
                {
                    int Len = Keys.Length;
                    RefObj* Ptr = (RefObj*)Keys.Pointer;
                    object[,] Arr = new object[Len, 1];
                    Parallel.For(
                        0,
                        Len,
                        delegate(int Index)
                        {
                            Arr[Index, 0] = new string(
                                (char*)Ptr[Index].Pointer,
                                0,
                                Ptr[Index].Length
                            );
                        }
                    );
                    return Arr;
                }
            }
        }
    }
}
