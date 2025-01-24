<div align="center">
<p><img alt="DataBox" width="256" style="max-width:40%;min-width:60px;" src="https://github.com/Web-Coke/DataBox/blob/main/DataBox.png" /></p>
<p>
    <a href="https://github.com/Web-Coke/DataBox/README.md">简体中文</a>
    <a href="https://github.com/Web-Coke/DataBox/README-EN_US.md">English</a>
</p>
<p>

DataBox是一个基于[Excel-DNA](https://github.com/Excel-DNA)的Excel加载项

部分核心功能使用[Rust](https://www.rust-lang.org/)实现, 速度快到不可思议

如果您是普通用户, 希望DataBox可助您高效办公

如果您是程序员, 希望DataBox给您带来新的启发(注释已经抓紧再写!)

## 安装使用

### 自动安装/手动安装

[点击查看](https://github.com/Web-Coke/DataBox/releases/)

### 从源码构建

```text
1. 拉取此项目
2. 安装Rust(Version>=1.80.0)
3. 安装Python(Version>=3.8.1)
4. 运行此项目的build.py
5. 安装Visual Studio
6. 打开项目的DataBox.sln
7. 还原NuGet程序包
8. 编译DataBox
```

## 示例

为了让您快速使用DataBox, 此项目提供了一个[示例](https://github.com/Web-Coke/DataBox/blob/main/Examples/示例.xlsx)文件

## 函数介绍

> 函数名称加粗的函数为溢出型函数
>
> 参数名称加粗的参数可为范围参数
>
> 参数名称标记下划线的参数为可选参数

### 1 时间计算

| 函数名称                                                     | 函数说明                                              | 参数说明                                                     |
| :----------------------------------------------------------- | :---------------------------------------------------- | ------------------------------------------------------------ |
| AdjustTime(Time, <u>Year</u>, <u>Month</u>, <u>Day</u>, <u>Hour</u>, <u>Minute</u>, <u>Second</u>) | 调整给定时间                                          | Time:给定时间<br/><u>Year</u>:年<br/><u>Month</u>:月<br/><u>Day</u>:日<br/><u>Hour</u>:分<br/><u>Second</u>:秒 |
| **SecToTime**(**Sec**)                                       | 将秒转换为[hh]:mm:ss格式的时间                        | Sec:需转换的秒数                                             |
| **TimeSub**(**TimeI**, **TimeN**)                            | 计算两个时间的差值<br/>输出以秒为单位                 | TimeI:第一个时间<br/>TimeN:第二个时间                        |
| **TimeSub2**(**TimeI**, **TimeN**, Includ)                   | 计算两个时间在计算在内时间中的差值<br/>输出以秒为单位 | TimeI:第一个时间<br/>TimeN:第二个时间<br/>Includ:计算在内的时间 |

### 2 坐标计算

| 函数名称                         | 函数说明                                                                        | 参数说明                                                                                |
| :------------------------------- | :------------------------------------------------------------------------------ | --------------------------------------------------------------------------------------- |
| Distance(Lon1, Lat1, Lon2, Lat2) | 计算两个WGS84坐标经纬度之间的距离<br/>输出以米为单位 |Lon1:第一个经度<br/>Lat1:第一个纬度<br/>Lon2:第二个经度<br/>Lat2:第二个纬度|
| **GetPOI**(Lon, Lat, Key)  | 获取WGS84坐标的基本POI信息<br/>Key需在[高德开放平台](http://lbs.amap.com/)申请                                      |Lon:经度<br/>Lat:纬度<br/>Key:服务Key|
| **ToGCJ02**(Lon, Lat, Key) | 将WGS84坐标转国测局坐标<br/>Key需在[高德开放平台](http://lbs.amap.com/)申请                                      |Lon:经度<br/>Lat:纬度<br/>Key:服务Key|

### 3 文本处理

| 函数名称                                  | 函数说明                                                     | 参数说明                                                     |
| :---------------------------------------- | :----------------------------------------------------------- | ------------------------------------------------------------ |
| IsMatch(Input, Pattern)                   | 检查输入的字符串是否可被正则表达式匹配<br/>匹配成功返回True  失败则返回False | Input:输入的字符串<br/>Pattern:正则表达式                    |
| **Matches**(Input, Pattern, <u>Label</u>) | 返回输入的字符串在正则表达式中匹配到的所有(标签)内容         | Input:输入的字符串<br/>Pattern:正则表达式<br/><u>Label</u>:标签, 可选参数, 标签需带"$"号 |
| Replaces(Input, Pattern, Replace)         | 替换输入的字符串中被正则表达式匹配到的部分为指定字符串       | Input:输入的字符串<br/>Pattern:正则表达式<br/>Replace:指定字符串, 标签需带"$"号 |

### 4 范围处理

| 函数名称                                  | 函数说明                                                     | 参数说明                                                   |
| ----------------------------------------- | ------------------------------------------------------------ | ---------------------------------------------------------- |
| **VLOOKUP2**(Value, **Array**, **Finds**) | VLOOKUP函数的升级版<br/>该函数返回多个查找的结果<br/>当未找到结果时返回#VALUE!错误<br/>数据多时建议使用数据快查功能 | Value:指定值<br/>**Array**:指定范围<br/>**Finds**:返回范围 |
| **ANDS**(**Lhs**, **Rhs**)                | 将**Lhs**与**Rhs**按顺序进行AND操作                          | **Lhs**:左值范围<br/>**Rhs**:右值范围                      |
| **ORS**(**Lhs**, **Rhs**)                 | 将**Lhs**与**Rhs**按顺序进行OR操作                           | **Lhs**:左值范围<br/>**Rhs**:右值范围                      |
| **XORS**(**Lhs**, **Rhs**)                | 将**Lhs**与**Rhs**按顺序进行XOR操作                          | **Lhs**:左值范围<br/>**Rhs**:右值范围                      |
| **NOTS**(**Array**)                       | 将**Array**进行NOT操作                                       | **Array**:指定范围                                         |

### 5 数据快查

> 在使用以下函数前请先在功能区中绑定数据

| 函数名称                             | 函数说明                         | 参数说明                                         |
| :----------------------------------- | -------------------------------- | ------------------------------------------------ |
| **FLOOKUP**(Key, SerialNumber) | 在索引中检索给定值并返回对应数据 | Key:给定值<br>SerialNumber:数据所在列的序号 |
| FCOUNTS(Key)                         | 统计给定值在索引中出现的次数     | Key:给定值                                       |
| **FGETKEY**()                  | 获取去重后的索引                 |                                                  |

## 真值表

|           AND(与)           |           OR(或)           |       NOT(非)       |          XOR(异或)          |
| :-------------------------: | :------------------------: | :------------------: | :-------------------------: |
| False `AND` False = False | False `OR` False = False | `NOT` False = True | False `XOR` False = False |
| False `AND` True = False |  False `OR` True = True  | `NOT` True = False | False `XOR` False = True |
|  True `AND` True = False  |  False `OR` True = True  |                      | False `XOR` False = True |
|  True `AND` True = True  |  False `OR` True = True  |                      | False `XOR` False = False |

## 正则表达式

> 正则表达式在线学习or练习[网站](https://regex101.com/)(https://regex101.com/)
>
> 设置中文:点击左边设置按钮->`Language`选项选择`Chinese`
>
> 请设置`语言风格`为`Rust`
>
> 正则引擎遵守[Unicode® Technical Standard #18](https://www.unicode.org/reports/tr18/)规范(https://www.unicode.org/reports/tr18/)
>
> `字符集`可参考[Regex Tutorial - Unicode Characters and Properties](https://www.regular-expressions.info/unicode.html#prop)(https://www.regular-expressions.info/unicode.html#prop)

| 基本表达式   | 描述                                                         |
| :----------- | ------------------------------------------------------------ |
| .            | 匹配除换行符外的任何字符                                     |
| [0-9]        | 匹配任何ASCII数字                                            |
| \d           | 数字, 包括`𝟙𝟚𝟛`这样的数字                                    |
| \D           | 非数字                                                       |
| \p{`字符集`} | 匹配给定字符集                                               |
| \P{`字符集`} | 不匹配给定的字符集                                           |
| [xyz]        | 匹配所有的`xyz`                                              |
| [^xyz]       | 除`xyz`之外的都匹配                                          |
| [a-z]        | 匹配所有的小写字母`a-z`                                      |
| [[:alpha:]]  | 匹配所有的大小写字母, 等同于`[A-Za-z]`                       |
| [[:^alpha:]] | 除所有的大小写字母都匹配, 等同于`[^A-Za-z]`                  |
| [x\[^xyz]]   | 嵌套/分组模式, 匹配除`y`和`z`之外的任何字符                  |
| [a-y&&xyz]   | 交集模式, 匹配`x`或`y`                                       |
| [0-9&&\[^4]] | 使用交集和求反进行减法, 匹配`0-9`, 但不包括`4`               |
| [0-9--4]     | 直接减法, 匹配`0-9`, 但不包括`4`                             |
| [a-g~~b-h]   | 对称差值, 仅匹配`a`和`h`                                     |
| [\\[\\]]     | 转义, 匹配`[]`                                               |
| xy           | 匹配所有的`xy`                                               |
| x\|y         | 匹配`x`或`y`                                                 |
| x*           | 匹配0个或多个`x`, 该模式为贪婪匹配, 会尽可能多的匹配         |
| x+           | 匹配1个或多个`x`, 该模式为贪婪匹配, 会尽可能多的匹配         |
| x?           | 匹配0个或1个`x`, 该模式为贪婪匹配, 会尽可能多的匹配          |
| x*?          | 匹配0个或多个`x`, 该模式为非贪婪匹配, 也就是懒匹配, 会尽可能少的匹配 |
| x+?          | 匹配1个或多个`x`, 该模式为非贪婪匹配, 也就是懒匹配, 会尽可能少的匹配 |
| x??          | 匹配0个或1个`x`, 该模式为非贪婪匹配, 也就是懒匹配, 会尽可能少的匹配 |
| x{n,m}       | 匹配`n-m`之间数量的`x`, 包括`n`和`m`, 该模式为贪婪匹配, 会尽可能多的匹配 |
| x{n,}        | 匹配至少`n`个`x`, 该模式为贪婪匹配, 会尽可能多的匹配         |
| x{n}         | 匹配刚好`n`个`x`                                             |
| x{n,m}?      | 匹配`n-m`之间数量的`x`, 包括`n`和`m`,该模式为非贪婪匹配, 也就是懒匹配, 会尽可能少的匹配 |
| x{n,}?       | 匹配至少`n`个`x`, 该模式为非贪婪匹配, 也就是懒匹配, 会尽可能少的匹配 |
| x{n}?        | 匹配刚好`n`个`x`                                             |
| \b           | Unicode单词边界                                              |
| \B           | 非Unicode单词边界                                            |

| 分组模式                     | 描述                                 |
| :--------------------------- | ------------------------------------ |
| (`基本表达式`)             | 给匹配内容分组                       |
| (?\<`name`>`基本表达式`) | 给匹配的分组命名, 名称必须为字母数字 |
| (?:`基本表达式`)           | 非捕获的内容分组                     |
| (?`标志`)                  | 在当前组中设置标志                   |
| (?`标志`:`基本表达式`)   | 为匹配内容设置标志, 非捕获模式     |

| 标志 | 描述                                           |
| :--- | ---------------------------------------------- |
| i    | 不区分大小写：字母同时匹配大写和小写           |
| m    | 多行模式                                       |
| s    | 允许`.`匹配换行符                              |
| R    | 启用`CRLF`模式：启用多行模式时, 允许匹配`\r\n` |
| U    | 交换贪婪匹配和非贪婪匹配的含义                 |
| u    | 默认匹配规则                                   |
| x    | 注释模式, 忽略空格并允许行注释, 注释以`#`开头  |

## 贡献

如果您有好的建议, 请复刻Fork本仓库并且创建一个拉取请求(pull request)

当然您也可以提交一个issue, 并且添加标签(enhancement)

不要忘记给项目点一个Star, 再次感谢❗❗❗

<a href="https://github.com/Web-Coke/DataBox/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=Web-Coke/DataBox" />
</a>

## 赞助

DataBox是一个开源项目, 您毫不吝啬滴赞赏和Star是对此项目的最大肯定

<p style="text-align: center;">
<img src="https://github.com/Web-Coke/DataBox/blob/main/DataBox/Ribbon/Src/AliPay.png" alt="Alipay" width="180"/>
<img src="https://github.com/Web-Coke/DataBox/blob/main/DataBox/Ribbon/Src/WeChat.png" alt="WeChat" width="180"/>
</p>

## 开源协议

DataBox采用[MIT](LICENSE.txt)开源协议

## 现存问题说明

- 在旧版WPS中溢出型函数的计算结果不会完整显示, WPS最新版与Excel中则可以正常显示计算结果, 旧版WPS不支持溢出型函数
- `GetPOI`与`ToGCJ02`函数中所使用的`Key`需在[高德开放平台](http://lbs.amap.com/)申请`Web服务`类型的`Key`且这两个函数需要网络连接

---

> 加载项使用中有任何建议、反馈、疑问请与我联系
>
> WeChat:`C0-Coke`
>
> Email:`web-chang@foxmail.com`
