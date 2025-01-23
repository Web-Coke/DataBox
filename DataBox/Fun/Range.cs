using System.Collections;
using System.Threading.Tasks;
using DataBox.LANGUAGE.Fun;
using ExcelDna.Integration;

public static class RangeCS
{
    [ExcelFunction(Category = Range.Category, Description = Range.VLOOKUP2.Description)]
    public static object VLOOKUP2(
        [ExcelArgument(Description = Range.VLOOKUP2.Arg1Description)] object Value,
        [ExcelArgument(Description = Range.VLOOKUP2.Arg2Description)] object[] Array,
        [ExcelArgument(Description = Range.VLOOKUP2.Arg3Description)] object[] Finds
    )
    {
        if (Array.Length != Finds.Length)
        {
            return ExcelError.ExcelErrorNA;
        }
        ArrayList Vector = new ArrayList { Capacity = 512 };
        for (int i = 0; i < Array.Length; i++)
        {
            if (Array[i].Equals(Value))
            {
                Vector.Add(Finds[i]);
            }
        }
        return Vector.ToArray();
    }

    [ExcelFunction(Category = Range.Category, Description = Range.ANDS.Description)]
    public static object ANDS(
        [ExcelArgument(Description = Range.ANDS.Arg1Description)] object[] Lhs,
        [ExcelArgument(Description = Range.ANDS.Arg2Description)] object[] Rhs
    )
    {
        if (Lhs.Length != Rhs.Length)
        {
            return ExcelError.ExcelErrorNA;
        }
        object[,] Arr = new object[Lhs.Length, 1];
        Parallel.For(
            0,
            Lhs.Length,
            delegate(int Index)
            {
                Arr[Index, 0] = (bool)Lhs[Index] & (bool)Rhs[Index];
            }
        );
        return Arr;
    }

    [ExcelFunction(Category = Range.Category, Description = Range.ORS.Description)]
    public static object ORS(
        [ExcelArgument(Description = Range.ORS.Arg1Description)] object[] Lhs,
        [ExcelArgument(Description = Range.ORS.Arg2Description)] object[] Rhs
    )
    {
        if (Lhs.Length != Rhs.Length)
        {
            return ExcelError.ExcelErrorNA;
        }
        object[,] Arr = new object[Lhs.Length, 1];
        Parallel.For(
            0,
            Lhs.Length,
            delegate(int Index)
            {
                Arr[Index, 0] = (bool)Lhs[Index] | (bool)Rhs[Index];
            }
        );
        return Arr;
    }

    [ExcelFunction(Category = Range.Category, Description = Range.XORS.Description)]
    public static object XORS(
        [ExcelArgument(Description = Range.XORS.Arg1Description)] object[] Lhs,
        [ExcelArgument(Description = Range.XORS.Arg2Description)] object[] Rhs
    )
    {
        if (Lhs.Length != Rhs.Length)
        {
            return ExcelError.ExcelErrorNA;
        }
        object[,] Arr = new object[Lhs.Length, 1];
        Parallel.For(
            0,
            Lhs.Length,
            delegate(int Index)
            {
                Arr[Index, 0] = (bool)Lhs[Index] ^ (bool)Rhs[Index];
            }
        );
        return Arr;
    }

    [ExcelFunction(Category = Range.Category, Description = Range.NOTS.Description)]
    public static object NOTS(
        [ExcelArgument(Description = Range.NOTS.Arg1Description)] object[] Array
    )
    {
        object[,] Arr = new object[Array.Length, 1];
        Parallel.For(
            0,
            Array.Length,
            delegate(int Index)
            {
                Arr[Index, 0] = !(bool)Array[Index];
            }
        );
        return Arr;
    }
}
