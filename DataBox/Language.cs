//将所有语言进行and以防止冲突
//All languages are and, to prevent conflicts
//#if ZH_CN && EN_US
# if ZH_CN
namespace DataBox
{
    namespace LANGUAGE
    {
        public class Loader
        {
            public const string AdaptiveCellSize = "自适应单元格";
        }

        public class Link
        {
            public const string LoadDllErr = "加载Dll时间发生错误";
            public const string LoadDllErrFile =
                "由于找不到 Core.dll, 无法继续加载程序. 重新运行Dll加载程序可能会解决此问题";
            public const string LoadDllErrNull = "Dll加载失败! 错误代码:";
            public const string RefObjPointerObsolete =
                "获取此值时请做好程序抛出异常和内存泄露的准备, 除非你知道你要干什么";
            public const string RefObjInterleavedArrayObsolete =
                "非交错数组不建议使用, 建议重新手搓实现, 重写索引器";
        }

        namespace Fun
        {
            public class Geo
            {
                public const string Category = "坐标计算";

                public class Distance
                {
                    public const string Description =
                        "计算两个WGS84坐标经纬度之间的距离\n输出以米为单位";
                    public const string Arg1Description = "第一个经度";
                    public const string Arg2Description = "第一个纬度";
                    public const string Arg3Description = "第二个经度";
                    public const string Arg4Description = "第二个纬度";
                }
            }

            public class Range
            {
                public const string Category = "范围处理";

                public class VLOOKUP2
                {
                    public const string Description =
                        "VLOOKUP函数的升级版\n该函数返回多个查找的结果\n当未找到结果时返回#VALUE!错误";
                    public const string Arg1Description = "指定值";
                    public const string Arg2Description = "指定范围";
                    public const string Arg3Description = "返回范围";
                }

                public class ANDS
                {
                    public const string Description = "将Lhs与Rhs按顺序进行AND操作";
                    public const string Arg1Description = "左值范围";
                    public const string Arg2Description = "右值范围";
                }

                public class ORS
                {
                    public const string Description = "将Lhs与Rhs按顺序进行OR操作";
                    public const string Arg1Description = "左值范围";
                    public const string Arg2Description = "右值范围";
                }

                public class XORS
                {
                    public const string Description = "将Lhs与Rhs按顺序进行XOR操作";
                    public const string Arg1Description = "左值范围";
                    public const string Arg2Description = "右值范围";
                }

                public class NOTS
                {
                    public const string Description = "将Array进行NOT操作";
                    public const string Arg1Description = "指定范围";
                }
            }

            public class Text
            {
                public const string Category = "文本处理";

                public class IsMatch
                {
                    public const string Description =
                        "检查输入的字符串是否可被正则表达式匹配\n匹配成功返回True  失败则返回False";
                    public const string Arg1Description = "输入的字符串(范围)";
                    public const string Arg2Description = "正则表达式";
                }

                public class Matches
                {
                    public const string Description =
                        "返回输入的字符串在正则表达式中匹配到的所有(标签)内容";
                    public const string Arg1Description = "输入的字符串";
                    public const string Arg2Description = "正则表达式";
                    public const string Arg3Description = "标签, 可选参数, 标签需带'$'号";
                }

                public class Replaces
                {
                    public const string Description =
                        "替换输入的字符串中被正则表达式匹配到的部分为指定字符串";
                    public const string Arg1Description = "输入的字符串(范围)";
                    public const string Arg2Description = "正则表达式";
                    public const string Arg3Description = "指定字符串, 标签需带'$'号";
                }
            }

            public class Time
            {
                public const string Category = "时间计算";

                public class SecToTime
                {
                    public const string Description = "将秒转换为[hh]:mm:ss格式的时间";
                    public const string Arg1Description = "需转换的秒数";
                }

                public class TimeSub
                {
                    public const string Description = "计算两个时间的差值\n输出以秒为单位";
                    public const string Arg1Description = "第一个时间(范围)";
                    public const string Arg2Description = "第二个时间(范围)";
                }

                public class TimeSub2
                {
                    public const string Description =
                        "计算两个时间在计算在内时间中的差值\n输出以秒为单位";
                    public const string Arg1Description = "第一个时间(范围)";
                    public const string Arg2Description = "第二个时间(范围)";
                    public const string Arg3Description = "计算在内的时间";
                }

                public class AdjustTime
                {
                    public const string Description = "调整给定时间";
                    public const string Arg1Description = "给定时间";
                    public const string Arg2Description = "年";
                    public const string Arg3Description = "月";
                    public const string Arg4Description = "日";
                    public const string Arg5Description = "时";
                    public const string Arg6Description = "分";
                    public const string Arg7Description = "秒";
                }
            }
        }

        namespace Ribbon
        {
            public class Basics
            {
                public const string DataBox = "数据盒子";
                public const string DataQuickFind = "快速查找";
                public const string Binding = "绑定数据";
                public const string UnBinding = "解绑数据";
                public const string BatchExtraction = "批量提取";
                public const string Launch = "开始提取";
                public const string NewConfig = "新建配置";
                public const string DataBoxInfo = "盒子信息";
                public const string Updates = "检查更新";
                public const string BoxInfo = "查看信息";
            }

            public class DataBoxInfoText
            {
                public const string AliPay = "AliPay";
                public const string WeChat = "WeChat";
                public const string DonateAuthor = "捐赠作者";
                public const string CoreVersion = "Core版本";
                public const string AddInVersion = "加载项版本";
                public const string Feedback = "问题反馈:web-chang@foxmail.com";
                public const string DataBoxInfo = "盒子信息";
            }

            public class NewConfigText
            {
                public const string CancelOperation = "您已取消此次操作";
                public const string SelectDataCell = "请选择要提取数据所在的单元格";
                public const string SelectDataCellErr = "获取单元格信息异常, 是否重试?";
                public const string InputDataName = "请输入提取数据的名称";
                public const string InputDataNameErr = "获取数据名称异常, 是否重试?";
                public const string ReadConfigFileErr = "配置文件读取失败, 请选择正确的配置文件";
                public const string UnsaveConfigFile = "已取消保存配置文件";
                public const string SaveConfigFileErr = "配置文件保存失败, 已放弃更改";
                public const string SheetName = "工作表名";
                public const string DataAddr = "数据位置";
                public const string DataName = "数据名称";
                public const string SavedState = "文件(&F)";
                public const string NotSavedState = "文件(&F)*";
                public const string NewFile = "新建(&N)";
                public const string OpenFile = "打开(&O)";
                public const string SaveFile = "保存(&S)";
                public const string SaveAs = "另存为(&A)";
                public const string Filter = "配置文件|*.json";
                public const string SelectOpenFile = "选择配置文件";
                public const string SelectSaveFile = "保存配置文件";
                public const string SelectFolder = "选择待提取文件夹";
                public const string NewConfig = "配置文件编辑器";
            }

            namespace Fun
            {
                public class BatchExtraction
                {
                    public const string Category = "批量提取";
                    public const string ResultsSheetName = "提取结果";
                    public const string InfoSheetName = "提取信息";
                    public const string InfoSheetA1Value = "提取状态";
                    public const string InfoSheetB1Value = "文件路径";
                    public const string InfoSheetC1Value = "错误描述";
                    public const string InfoSheetD1Value = "错误信息";
                    public const string Successes = "成功";
                    public const string Failures = "失败";
                    public const string OpenFileFailures = "打开文件失败";

                    public static string OpenSheetFailures(string SheetName) =>
                        $"打开Sheet`{SheetName}`失败";

                    public const string CellRWFailures = "读写单元格失败";
                }

                public class DataQuickFind
                {
                    public const string CreateBindingDataPrompt = "请选择单元格以绑定数据";
                    public const string CreateBindingDataTitle = "绑定数据";
                    public const string CreateIndexPrompt = "请输入索引所在列的序号";
                    public const string CreateIndexTitle = "建立索引";
                    public const string BindingDataException = "绑定数据异常, 是否重试?";
                    public const string GetInputException = "获取输入异常, 是否重试?";
                    public const string OnePieceData = "Emm, 只获取到一个数据, 重新绑定一下吧";
                    public const string IndexOutOfRangeException =
                        "建立索引时出现异常, 请检查索引所在列是否输入正确后重新绑定";
                    public const string OtherException =
                        "建立索引时出现异常, 这通常是由内存不足引起的, 重启一下电脑试试";
                    public const string Succeed = "绑定数据成功!!!";

                    public const string Category = "数据快查";

                    public class FLOOKUP
                    {
                        public const string Description = "在索引中检索给定值并返回对应数据";
                        public const string Arg1Description = "给定值";
                        public const string Arg2Description = "数据所在列的序号";
                    }

                    public class FCOUNTS
                    {
                        public const string Description = "统计给定值在索引中出现的次数";
                        public const string Arg1Description = "给定值";
                    }

                    public class FGETKEY
                    {
                        public const string Description = "获取去重后的索引";
                    }
                }
            }
        }
    }
}
#elif EN_US

#endif
