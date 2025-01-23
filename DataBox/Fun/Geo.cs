using System.Threading.Tasks;
using DataBox.LANGUAGE.Fun;
using DataBox.RustFun;
using ExcelDna.Integration;

public static class GeoCS
{
    [ExcelFunction(Category = Geo.Category, Description = Geo.Distance.Description)]
    public static double Distance(
        [ExcelArgument(Description = Geo.Distance.Arg1Description)] double Lon1,
        [ExcelArgument(Description = Geo.Distance.Arg2Description)] double Lat1,
        [ExcelArgument(Description = Geo.Distance.Arg3Description)] double Lon2,
        [ExcelArgument(Description = Geo.Distance.Arg4Description)] double Lat2
    )
    {
        return GeoRS.Distance(Lon1, Lat1, Lon2, Lat2);
    }

#if ZH_CN
    //此函数非中国地区不可用
    //This function is not available outside of China
    [ExcelFunction(
        Category = "坐标计算",
        Description = "获取WGS84坐标的基本POI信息\nKey需在高德开放平台申请"
    )]
    public static object GetPOI(
        [ExcelArgument(Description = "经度")] double Lon,
        [ExcelArgument(Description = "纬度")] double Lat,
        [ExcelArgument(Description = "服务Key")] string Key
    )
    {
        string Uri =
            $"/v3/geocode/regeo?key={Key}&location={Lon:#.######},{Lat:#.######}&radius=50&extensions=base";
        return ExcelAsyncUtil.RunTask(
            "GetPOI",
            new string[1] { Uri },
            async () => await Task.Run(() => GeoRS.GetPOI(new RefObj(Uri)).ToStringArr())
        );
    }
#endif

#if ZH_CN
    //此函数非中国地区不可用
    //This function is not available outside of China
    [ExcelFunction(
        Category = "坐标计算",
        Description = "将WGS84坐标转国测局坐标\nKey需在高德开放平台申请"
    )]
    public static object ToGCJ02(
        [ExcelArgument(Description = "经度")] double Lon,
        [ExcelArgument(Description = "纬度")] double Lat,
        [ExcelArgument(Description = "服务Key")] string Key
    )
    {
        string Uri =
            $"/v3/assistant/coordinate/convert?key={Key}&locations={Lon:#.######},{Lat:#.######}&coordsys=gps";
        return ExcelAsyncUtil.RunTask(
            "ToGCJ02",
            new string[1] { Uri },
            async () => await Task.Run(() => GeoRS.ToGCJ02(new RefObj(Uri)).ToStringArr())
        );
    }
#endif
}
