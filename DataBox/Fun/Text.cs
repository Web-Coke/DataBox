using DataBox.LANGUAGE.Fun;
using ExcelDna.Integration;
using DataBox.RustFun;

public static class TextCS
{
    [ExcelFunction(Category = Text.Category, Description = Text.IsMatch.Description)]
    public static object IsMatch(
        [ExcelArgument(Description = Text.IsMatch.Arg1Description)] string Input,
        [ExcelArgument(Description = Text.IsMatch.Arg2Description)] string Pattern
    )
    {
        return TextRS.IsMatch(new RefObj(Input), new RefObj(Pattern));
    }

    [ExcelFunction(Category = Text.Category, Description = Text.Matches.Description)]
    public static object[] Matches(
        [ExcelArgument(Description = Text.Matches.Arg1Description)] string Input,
        [ExcelArgument(Description = Text.Matches.Arg2Description)] string Pattern,
        [ExcelArgument(Description = Text.Matches.Arg3Description)] string Label
    )
    {
        return TextRS.Matches(
                new RefObj(Input),
                new RefObj(Pattern),
                new RefObj(Label.Replace("$", ""))
            )
            .ToStringArr();
    }

    [ExcelFunction(Category = Text.Category, Description = Text.Replaces.Description)]
    public static string Replaces(
        [ExcelArgument(Description = Text.Replaces.Arg1Description)] string Input,
        [ExcelArgument(Description = Text.Replaces.Arg2Description)] string Pattern,
        [ExcelArgument(Description = Text.Replaces.Arg3Description)] string Replace
    )
    {
        return TextRS.Replaces(new RefObj(Input), new RefObj(Pattern), new RefObj(Replace)).ToString();
    }
}
