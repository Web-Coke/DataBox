<div align="center">
<p><img alt="DataBox" width="256" style="max-width:40%;min-width:60px;" src="https://github.com/Web-Coke/DataBox/blob/main/DataBox.png" /></p>
<p>
    <a href="README.md">简体中文</a>
    <a href="README-EN_US.md">English</a>
</p>
<p>

DataBox is an Excel add-in based on [Excel-DNA](https://github.com/Excel-DNA).

Some of the core features are implemented in [Rust](https://www.rust-lang.org/), It's incredibly fast.

If you are a regular user, you hope that DataBox can help you work efficiently.

If you are a programmer, I hope DataBox will inspire you in a new way.

## Install and use

### Automatic/manual installation

[Click to understand the detailed steps](https://github.com/Web-Coke/DataBox/releases/)

### Build from source

```text
1. Pull this item
2. Install Rust(Version>=1.80.0)
3. Install Python(Version>=3.8.1)
4. Install Visual Studio
5. Open the project's DataBox.sln
6. Restore the NuGet package
7. build DataBox
8. Run the build.py of this project
```

## Example

To get you up and running quickly, this project provides an [Example](https://github.com/Web-Coke/DataBox/blob/main/Examples/示例.xlsx) file

## Introduction to add-on functions

> Functions with bold function names are overflow functions
>
> Parameters with the parameter name in bold can be range parameters
>
> Parameter name marked with an underscore parameter is optional

### 1 Time Calculation

| 函数名称                                                     | 函数说明                                              | 参数说明                                                     |
| :----------------------------------------------------------- | :---------------------------------------------------- | ------------------------------------------------------------ |
| AdjustTime(Time, <u>Year</u>, <u>Month</u>, <u>Day</u>, <u>Hour</u>, <u>Minute</u>, <u>Second</u>) | 调整给定时间                                          | Time:给定时间<br/><u>Year</u>:年<br/><u>Month</u>:月<br/><u>Day</u>:日<br/><u>Hour</u>:分<br/><u>Second</u>:秒 |
| **SecToTime**(**Sec**)                                       | 将秒转换为[hh]:mm:ss格式的时间                        | Sec:需转换的秒数                                             |
| **TimeSub**(**TimeI**, **TimeN**)                            | 计算两个时间的差值<br/>输出以秒为单位                 | TimeI:第一个时间<br/>TimeN:第二个时间                        |
| **TimeSub2**(**TimeI**, **TimeN**, Includ)                   | 计算两个时间在计算在内时间中的差值<br/>输出以秒为单位 | TimeI:第一个时间<br/>TimeN:第二个时间<br/>Includ:计算在内的时间 |

### 2 Coordinate Calculation

| 函数名称                         | 函数说明                                                                        | 参数说明                                                                                |
| :------------------------------- | :------------------------------------------------------------------------------ | --------------------------------------------------------------------------------------- |
| Distance(Lon1, Lat1, Lon2, Lat2) | 计算两个WGS84坐标经纬度之间的距离<br/>输出以米为单位 |Lon1:第一个经度<br/>Lat1:第一个纬度<br/>Lon2:第二个经度<br/>Lat2:第二个纬度|

### 3 Text Processing

| 函数名称                                  | 函数说明                                                     | 参数说明                                                     |
| :---------------------------------------- | :----------------------------------------------------------- | ------------------------------------------------------------ |
| **IsMatch**(**Input**, Pattern)           | 检查输入的字符串是否可被正则表达式匹配<br/>匹配成功返回True  失败则返回False | **Input**:输入的字符串<br/>Pattern:正则表达式                |
| **Matches**(Input, Pattern, <u>Label</u>) | 返回输入的字符串在正则表达式中匹配到的所有(标签)内容         | Input:输入的字符串<br/>Pattern:正则表达式<br/><u>Label</u>:标签, 可选参数, 标签需带"$"号 |
| **Replaces**(**Input**, Pattern, Replace) | 替换输入的字符串中被正则表达式匹配到的部分为指定字符串       | **Input**:输入的字符串<br/>Pattern:正则表达式<br/>Replace:指定字符串, 标签需带"$"号 |

### 4 Scope Processing

| 函数名称                                  | 函数说明                                                     | 参数说明                                                   |
| ----------------------------------------- | ------------------------------------------------------------ | ---------------------------------------------------------- |
| **VLOOKUP2**(Value, **Array**, **Finds**) | VLOOKUP函数的升级版<br/>该函数返回多个查找的结果<br/>当未找到结果时返回#VALUE!错误<br/>数据多时建议使用数据快查功能 | Value:指定值<br/>**Array**:指定范围<br/>**Finds**:返回范围 |
| **ANDS**(**Lhs**, **Rhs**)                | 将**Lhs**与**Rhs**按顺序进行AND操作                          | **Lhs**:左值范围<br/>**Rhs**:右值范围                      |
| **ORS**(**Lhs**, **Rhs**)                 | 将**Lhs**与**Rhs**按顺序进行OR操作                           | **Lhs**:左值范围<br/>**Rhs**:右值范围                      |
| **XORS**(**Lhs**, **Rhs**)                | 将**Lhs**与**Rhs**按顺序进行XOR操作                          | **Lhs**:左值范围<br/>**Rhs**:右值范围                      |
| **NOTS**(**Array**)                       | 将**Array**进行NOT操作                                       | **Array**:指定范围                                         |

### 5 Data Quick Find

> Before using the following functions, bind data in the ribbon

| 函数名称                             | 函数说明                         | 参数说明                                         |
| :----------------------------------- | -------------------------------- | ------------------------------------------------ |
| **FLOOKUP**(Key, SerialNumber) | 在索引中检索给定值并返回对应数据 | Key:给定值<br>SerialNumber:数据所在列的序号 |
| FCOUNTS(Key)                         | 统计给定值在索引中出现的次数     | Key:给定值                                       |
| **FGETKEY**()                  | 获取去重后的索引                 |                                                  |

## Truth table

|            AND            |            OR            |       NOT       |          XOR          |
| :-----------------------: | :----------------------: | :-------------: | :-------------------: |
| False `AND` False = False | False `OR` False = False | `NOT` False = True | False `XOR` False = False |
| False `AND` True = False |  False `OR` True = True  | `NOT` True = False | False `XOR` False = True |
|  True `AND` True = False  |  False `OR` True = True  |                      | False `XOR` False = True |
|  True `AND` True = True  |  False `OR` True = True  |                      | False `XOR` False = False |

## Regular Expression

> Regular Expressions e-learning or practice [website](https://regex101.com/)(https://regex101.com/)
>
> Please set `FLAVOR` to `Rust`
>
> 正则引擎遵守[Unicode® Technical Standard #18](https://www.unicode.org/reports/tr18/)规范(https://www.unicode.org/reports/tr18/)
>
> The `Characters Set` please refer to:[Regex Tutorial - Unicode Characters and Properties](https://www.regular-expressions.info/unicode.html#prop)(https://www.regular-expressions.info/unicode.html#prop)

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

## Contribute

If you have a good suggestion, fork the repository and create a pull request

Of course, you can also file an issue with an enhancement

Don't forget to give the project a Star, thanks again ❗❗❗

<a href="https://github.com/Web-Coke/DataBox/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=Web-Coke/DataBox" />
</a>

## Sponsor

DataBox is an open source project, and you don't hesitate to praise it and Star is the biggest affirmation of this project

<p style="text-align: center;">
<img src="https://github.com/Web-Coke/DataBox/blob/main/DataBox/Ribbon/Src/AliPay.png" alt="Alipay" width="180"/>
<img src="https://github.com/Web-Coke/DataBox/blob/main/DataBox/Ribbon/Src/WeChat.png" alt="WeChat" width="180"/>
</p>

## Open-Source Protocol

DataBox uses the [MIT](LICENSE) open source license

## Description of existing problems

- In the old version of WPS, the calculation results of overflow functions are not displayed completely, but in the latest version of WPS and Excel, the calculation results can be displayed normally, and the old version of WPS does not support overflow functions

---

> Please contact me if you have any suggestions, feedback, or questions about using the add-in
>
> Email:`web-chang@foxmail.com`
