using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using ExcelDna.Integration;

namespace DataBox.RustFun
{
    public static class Link
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibraryEx(
            string lpLibFileName,
            IntPtr hFile,
            uint dwFlags
        );

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr hLibModule, string lpProcName);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeLibrary(IntPtr hLibModule);

        private static IntPtr hLibModule;

        private static readonly string DllPath =
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
            + "\\DataBox\\"
            + (IntPtr.Size == 8 ? "Core64.dll" : "Core32.dll");

        private static bool Exists => File.Exists(DllPath);

        public static void Install()
        {
            if (!Exists)
            {
                MessageBox.Show(
                    LANGUAGE.Link.LoadDllErrFile,
                    LANGUAGE.Link.LoadDllErr,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }
            hLibModule = LoadLibraryEx(DllPath, IntPtr.Zero, 0x00000008);
            if (hLibModule == IntPtr.Zero)
            {
                throw new NullReferenceException(
                    $"{LANGUAGE.Link.LoadDllErrNull}{Marshal.GetLastWin32Error()}"
                );
            }
        }

        public static void UnInstall()
        {
            FreeLibrary(hLibModule);
        }

        public static DelegateFn LaodFun<DelegateFn>(string EntryPoint)
            where DelegateFn : Delegate
        {
            return Marshal.GetDelegateForFunctionPointer<DelegateFn>(
                GetProcAddress(hLibModule, EntryPoint)
            );
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct RefObj : IDisposable
    {
        private int Len;
        private int Rty;

        //此处使用void*指针主要防止进行指针运算出现溢出等问题
        //The void* pointer is used here to prevent problems such as overflow during pointer arithmetic
        //其次为减少开销
        //The second is to reduce overhead
        private void* Ptr;

        private delegate void DFn_RefObjDispose(RefObj Self);

        private static readonly DFn_RefObjDispose RFn_RefObjDispose =
            Link.LaodFun<DFn_RefObjDispose>("RefObjDispose");

        public int Length => Len;

        [Obsolete(LANGUAGE.Link.RefObjPointerObsolete, false)]
        public void* Pointer => Ptr;

        public RefObj(string val)
        {
            fixed (char* p = val)
            {
                Len = val.Length;
                Rty = -1;
                Ptr = p;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            using (this)
            {
                return new string((char*)Ptr, 0, Len);
            }
        }

        public RefObj(string[] val)
        {
            RefObj[] Arr = new RefObj[val.Length];
            for (int i = 0; i < val.Length; i++)
            {
                fixed (char* p = val[i])
                {
                    Arr[i] = new RefObj
                    {
                        Len = val[i].Length,
                        Rty = -1,
                        Ptr = p,
                    };
                }
            }
            fixed (RefObj* p = Arr)
            {
                Len = val.Length;
                Rty = -1;
                Ptr = p;
            }
        }

        public string[] ToStringArr()
        {
            using (this)
            {
                string[] Arr = new string[Len];
                RefObj* Val = (RefObj*)Ptr;
                for (int i = 0; i < Len; i++)
                {
                    Arr[i] = new string((char*)Val[i].Ptr, 0, Val[i].Len);
                }
                return Arr;
            }
        }
#pragma warning disable CS8500
        public RefObj FromNumArr<T>(T[] val)
            where T : struct
        {
            fixed (T* p = val)
            {
                Len = val.Length;
                Rty = -1;
                Ptr = p;
            }
            return this;
        }

        public T[] ToNumArray<T>()
            where T : struct
        {
            using (this)
            {
                T[] Arr = new T[Len];
                for (int i = 0; i < Len; i++)
                {
                    Arr[i] = ((T*)Ptr)[i];
                }
                return Arr;
            }
        }

        public RefObj FromNumArr<T>(T[][] val)
            where T : struct
        {
            RefObj[] Arr = new RefObj[val.GetLength(0)];
            for (int Index = 0; Index < val.GetLength(0); Index++)
            {
                T[] arr = val[Index];
                fixed (T* p = arr)
                {
                    Arr[Index] = new RefObj
                    {
                        Ptr = p,
                        Rty = -1,
                        Len = arr.Length,
                    };
                }
            }
            fixed (RefObj* p = Arr)
            {
                return new RefObj
                {
                    Len = val.Length,
                    Rty = -1,
                    Ptr = p,
                };
            }
        }

        public T[][] ToNumArr<T>()
            where T : struct
        {
            using (this)
            {
                T[][] Arr = new T[Len][];
                RefObj* Val = (RefObj*)Ptr;
                for (int i = 0; i < Len; i++)
                {
                    T[] _Arr = new T[Val[i].Len];
                    for (int n = 0; n < Val[i].Len; n++)
                    {
                        _Arr[n] = ((T*)Val[i].Ptr)[n];
                    }
                    Arr.SetValue(_Arr, i);
                }
                return Arr;
            }
        }
#pragma warning restore CS8500
        public void Dispose()
        {
            if (Rty != -1)
            {
                RFn_RefObjDispose(this);
            }
        }
    }

    public static class GeoRS
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate double _Distance(double Lon1, double Lat1, double Lon2, double Lat2);
        public static readonly _Distance Distance = Link.LaodFun<_Distance>("Distance");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate RefObj _GetPOI(RefObj Uri);
        public static readonly _GetPOI GetPOI = Link.LaodFun<_GetPOI>("GetPOI");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate RefObj _ToGCJ02(RefObj Uri);
        public static readonly _ToGCJ02 ToGCJ02 = Link.LaodFun<_ToGCJ02>("ToGCJ02");
    }

    public static class TextRS
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr _RegexCreate(RefObj Pattern);
        public static readonly _RegexCreate RegexCreate = Link.LaodFun<_RegexCreate>("RegexCreate");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool _IsMatch(RefObj Input, IntPtr Pattern);
        public static readonly _IsMatch IsMatch = Link.LaodFun<_IsMatch>("IsMatch");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate RefObj _Matches(RefObj Input, RefObj Pattern, RefObj Label);
        public static readonly _Matches Matches = Link.LaodFun<_Matches>("Matches");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate RefObj _Replaces(RefObj Input, IntPtr Pattern, IntPtr Replace);
        public static readonly _Replaces Replaces = Link.LaodFun<_Replaces>("Replaces");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void _RegexDispose(IntPtr Pattern);
        public static readonly _RegexDispose RegexDispose = Link.LaodFun<_RegexDispose>(
            "RegexDispose"
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr _FixedStr(RefObj Input);
        public static readonly _FixedStr FixedStr = Link.LaodFun<_FixedStr>("FixedStr");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate RefObj _DropStr(IntPtr Input);
        public static readonly _DropStr DropStr = Link.LaodFun<_DropStr>("DropStr");
    }

    public static class TimeRS
    {
        [StructLayout(LayoutKind.Sequential)]
        public readonly struct SimpleTime
        {
            private readonly ulong Year;
            private readonly ulong Month;
            private readonly ulong Day;
            private readonly ulong Hour;
            private readonly ulong Minute;
            private readonly ulong Second;

            public SimpleTime(DateTime Time)
            {
                Year = (ulong)Time.Year;
                Month = (ulong)Time.Month;
                Day = (ulong)Time.Day;
                Hour = (ulong)Time.Hour;
                Minute = (ulong)Time.Minute;
                Second = (ulong)Time.Second;
            }

            public DateTime ToDateTime()
            {
                return new DateTime(
                    (int)Year,
                    (int)Month,
                    (int)Day,
                    (int)Hour,
                    (int)Minute,
                    (int)Second
                );
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate RefObj _SecToTime(long Sec);
        public static readonly _SecToTime SecToTime = Link.LaodFun<_SecToTime>("SecToTime");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate long _TimeSub(SimpleTime TimeI, SimpleTime TimeN);
        public static readonly _TimeSub TimeSub = Link.LaodFun<_TimeSub>("TimeSub");

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr _TimeSubCreate(RefObj Include);
        public static readonly _TimeSubCreate TimeSubCreate = Link.LaodFun<_TimeSubCreate>(
            "TimeSubCreate"
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate long _TimeSubCompute(IntPtr Self, SimpleTime TimeI, SimpleTime TimeN);
        public static readonly _TimeSubCompute TimeSubCompute = Link.LaodFun<_TimeSubCompute>(
            "TimeSubCompute"
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void _TimeSubDispose(IntPtr Self);
        public static readonly _TimeSubDispose TimeSubDispose = Link.LaodFun<_TimeSubDispose>(
            "TimeSubDispose"
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate SimpleTime _ParseTime(RefObj Time);
        public static readonly _ParseTime ParseTime = Link.LaodFun<_ParseTime>("ParseTime");
    }

    namespace Ribbon
    {
        public static class BatchExtractionRS
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate string Callback_SetString(IntPtr Ptr, int Len);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void Callback_RAction(string Arg1, string Arg2, string Arg3);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate RefObj Callback_WAction(int Index);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void _BECRead(
                RefObj ConfigFilePath,
                Callback_SetString SetString,
                Callback_RAction Action
            );
            public static readonly _BECRead BECRead = Link.LaodFun<_BECRead>("BECRead");

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void _BECWrite(
                RefObj ConfigFilePath,
                int Count,
                Callback_WAction Action
            );
            public static readonly _BECWrite BECWrite = Link.LaodFun<_BECWrite>("BECWrite");

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate RefObj _Conversion(int Index);
            public static readonly _Conversion Conversion = Link.LaodFun<_Conversion>("Conversion");
        }

        public static class DataBoxInfoRs
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate RefObj _CoreVersion();
            public static readonly _CoreVersion CoreVersion = Link.LaodFun<_CoreVersion>(
                "CoreVersion"
            );
        }

        public static class DataQuickFindRS
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr _DQFCreate();
            public static readonly _DQFCreate DQFCreate = Link.LaodFun<_DQFCreate>("DQFCreate");

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void _DQFWrite(IntPtr Self, RefObj Key, int Idx);
            public static readonly _DQFWrite DQFWrite = Link.LaodFun<_DQFWrite>("DQFWrite");

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate RefObj _DQFFinds(IntPtr Self, RefObj Key);
            public static readonly _DQFFinds DQFFinds = Link.LaodFun<_DQFFinds>("DQFFinds");

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate int _DQFCounts(IntPtr Self, RefObj Key);
            public static readonly _DQFCounts DQFCounts = Link.LaodFun<_DQFCounts>("DQFCounts");

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate RefObj _DQFKeys(IntPtr Self);
            public static readonly _DQFKeys DQFKeys = Link.LaodFun<_DQFKeys>("DQFKeys");

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void _DQFDispose(IntPtr Self);
            public static readonly _DQFDispose DQFDispose = Link.LaodFun<_DQFDispose>("DQFDispose");
        }
    }

#if DEBUG
    public static class Test
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate RefObj _MemoryLeak(RefObj TestStr);
        private static readonly _MemoryLeak MemoryLeak = Link.LaodFun<_MemoryLeak>("MemoryLeak");

        [ExcelFunction(Category = "Test", Description = "检查Rust创建的RefObj使用后是否正常销毁")]
#pragma warning disable IDE1006
        public static object _MemoryLeakTest(string TestStr)
#pragma warning restore IDE1006
        {
            return MemoryLeak(new RefObj(TestStr)).ToStringArr();
        }
    }
#endif
}
