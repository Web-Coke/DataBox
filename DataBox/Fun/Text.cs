using System;
using System.Threading.Tasks;
using DataBox.LANGUAGE.Fun;
using DataBox.RustFun;
using ExcelDna.Integration;

public static class TextCS
{
    [ExcelFunction(Category = Text.Category, Description = Text.IsMatch.Description)]
    public static object IsMatch(
        [ExcelArgument(Description = Text.IsMatch.Arg1Description)] object[] Input,
        [ExcelArgument(Description = Text.IsMatch.Arg2Description)] string Pattern
    )
    {
        if (Input[0] is ExcelMissing)
        {
            return ExcelError.ExcelErrorValue;
        }
        object[,] Arr = new object[Input.Length, 1];
        IntPtr Ptr = TextRS.RegexCreate(new RefObj(Pattern));
        Parallel.For(
            0,
            Input.Length,
            delegate(int Index)
            {
                try
                {
                    object Item = Input[Index];
                    Arr[Index, 0] = TextRS.IsMatch(
                        Item is ExcelEmpty ? new RefObj(string.Empty) : new RefObj(Item.ToString()),
                        Ptr
                    );
                }
                catch
                {
                    Arr[Index, 0] = ExcelError.ExcelErrorValue;
                }
            }
        );
        TextRS.RegexDispose(Ptr);
        return Arr;
    }

    [ExcelFunction(Category = Text.Category, Description = Text.Matches.Description)]
    public static object[] Matches(
        [ExcelArgument(Description = Text.Matches.Arg1Description)] string Input,
        [ExcelArgument(Description = Text.Matches.Arg2Description)] string Pattern,
        [ExcelArgument(Description = Text.Matches.Arg3Description)] string Label
    )
    {
        return TextRS
            .Matches(new RefObj(Input), new RefObj(Pattern), new RefObj(Label.Replace("$", "")))
            .ToStringArr();
    }

    [ExcelFunction(Category = Text.Category, Description = Text.Replaces.Description)]
    public static object Replaces(
        [ExcelArgument(Description = Text.Replaces.Arg1Description)] object[] Input,
        [ExcelArgument(Description = Text.Replaces.Arg2Description)] string Pattern,
        [ExcelArgument(Description = Text.Replaces.Arg3Description)] string Replace
    )
    {
        if (Input[0] is ExcelMissing)
        {
            return ExcelError.ExcelErrorValue;
        }
        object[,] Arr = new object[Input.Length, 1];
        IntPtr PatternPtr = TextRS.RegexCreate(new RefObj(Pattern));
        IntPtr ReplacePtr = TextRS.FixedStr(new RefObj(Replace));
        Parallel.For(
            0,
            Input.Length,
            delegate(int Index)
            {
                try
                {
                    object Item = Input[Index];
                    Arr[Index, 0] = TextRS.Replaces(
                        Item is ExcelEmpty ? new RefObj(string.Empty) : new RefObj(Item.ToString()),
                        PatternPtr,
                        ReplacePtr
                    );
                }
                catch
                {
                    Arr[Index, 0] = ExcelError.ExcelErrorValue;
                }
            }
        );
        TextRS.RegexDispose(PatternPtr);
        TextRS.DropStr(ReplacePtr);
        return Arr;
    }
}
