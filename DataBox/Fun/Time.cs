using System;
using System.Threading.Tasks;
using DataBox.LANGUAGE.Fun;
using DataBox.RustFun;
using ExcelDna.Integration;

public static class TimeCS
{
    [ExcelFunction(Category = Time.Category, Description = Time.SecToTime.Description)]
    public static object SecToTime(
        [ExcelArgument(Description = Time.SecToTime.Arg1Description)] object[] Sec
    )
    {
        if (Sec[0] is ExcelMissing)
        {
            return ExcelError.ExcelErrorValue;
        }
        object[,] Arr = new object[Sec.Length, 1];
        Parallel.For(
            0,
            Sec.Length,
            delegate(int Index)
            {
                try
                {
                    Arr[Index, 0] = TimeRS
                        .SecToTime(
                            Sec[Index] is double Second
                                ? (long)Second
                                : Convert.ToInt64(Sec[Index].ToString())
                        )
                        .ToString();
                }
                catch
                {
                    Arr[Index, 0] = ExcelError.ExcelErrorValue;
                }
            }
        );
        return Arr;
    }

    [ExcelFunction(Category = Time.Category, Description = Time.TimeSub.Description)]
    public static object TimeSub(
        [ExcelArgument(Description = Time.TimeSub.Arg1Description)] object[] TimeI,
        [ExcelArgument(Description = Time.TimeSub.Arg2Description)] object[] TimeN
    )
    {
        if (TimeI.Length != TimeN.Length || TimeI[0] is ExcelMissing)
        {
            return ExcelError.ExcelErrorValue;
        }
        object[,] Arr = new object[TimeI.Length, 1];
        Parallel.For(
            0,
            TimeI.Length,
            delegate(int Index)
            {
                object ItemI = TimeI[Index];
                object ItemN = TimeN[Index];
                try
                {
                    TimeRS.SimpleTime TI = ItemI is double SI
                        ? new TimeRS.SimpleTime(DateTime.FromOADate(SI))
                        : TimeRS.ParseTime(new RefObj(ItemI.ToString()));
                    TimeRS.SimpleTime TN = ItemN is double SN
                        ? new TimeRS.SimpleTime(DateTime.FromOADate(SN))
                        : TimeRS.ParseTime(new RefObj(ItemN.ToString()));
                    Arr[Index, 0] = TimeRS.TimeSub(TI, TN);
                }
                catch
                {
                    Arr[Index, 0] = ExcelError.ExcelErrorValue;
                }
            }
        );
        return Arr;
    }

    [ExcelFunction(Category = Time.Category, Description = Time.TimeSub2.Description)]
    public static object TimeSub2(
        [ExcelArgument(Description = Time.TimeSub2.Arg1Description)] object[] TimeI,
        [ExcelArgument(Description = Time.TimeSub2.Arg2Description)] object[] TimeN,
        [ExcelArgument(Description = Time.TimeSub2.Arg3Description)] string Include
    )
    {
        if (TimeI.Length != TimeN.Length || TimeI[0] is ExcelMissing)
        {
            return ExcelError.ExcelErrorValue;
        }
        IntPtr IncludePtr = TimeRS.TimeSubCreate(new RefObj(Include));
        object[,] Arr = new object[TimeI.Length, 1];
        try
        {
            Parallel.For(
                0,
                TimeI.Length,
                delegate(int Index)
                {
                    object ItemI = TimeI[Index];
                    object ItemN = TimeN[Index];
                    try
                    {
                        TimeRS.SimpleTime TI = ItemI is double SI
                            ? new TimeRS.SimpleTime(DateTime.FromOADate(SI))
                            : TimeRS.ParseTime(new RefObj(ItemI.ToString()));
                        TimeRS.SimpleTime TN = ItemN is double SN
                            ? new TimeRS.SimpleTime(DateTime.FromOADate(SN))
                            : TimeRS.ParseTime(new RefObj(ItemN.ToString()));
                        Arr[Index, 0] = TimeRS.TimeSubCompute(IncludePtr, TI, TN);
                    }
                    catch
                    {
                        Arr[Index, 0] = ExcelError.ExcelErrorValue;
                    }
                }
            );
        }
        finally
        {
            TimeRS.TimeSubDispose(IncludePtr);
        }
        return Arr;
    }

    [ExcelFunction(Category = Time.Category, Description = Time.AdjustTime.Description)]
    public static object AdjustTime(
        [ExcelArgument(Description = Time.AdjustTime.Arg1Description)] object Time,
        [ExcelArgument(Description = Time.AdjustTime.Arg2Description)] int Year = 0,
        [ExcelArgument(Description = Time.AdjustTime.Arg3Description)] int Month = 0,
        [ExcelArgument(Description = Time.AdjustTime.Arg4Description)] int Day = 0,
        [ExcelArgument(Description = Time.AdjustTime.Arg5Description)] int Hour = 0,
        [ExcelArgument(Description = Time.AdjustTime.Arg6Description)] int Minute = 0,
        [ExcelArgument(Description = Time.AdjustTime.Arg7Description)] int Second = 0
    )
    {
        DateTime ArgTime;
        if (Time is double DT)
        {
            ArgTime = DateTime.FromOADate(DT);
        }
        else if (Time is string ST)
        {
            ArgTime = TimeRS.ParseTime(new RefObj(ST)).ToDateTime();
        }
        else
        {
            return ExcelError.ExcelErrorValue;
        }
        return ArgTime
            .AddYears(Year)
            .AddMonths(Month)
            .AddDays(Day)
            .AddHours(Hour)
            .AddMinutes(Minute)
            .AddSeconds(Second)
            .ToString("yyyy-MM-dd HH:mm:ss");
    }
}
