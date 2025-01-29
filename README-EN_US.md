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

| Function name                                                | Function description                                         | Parameter description                                        |
| :----------------------------------------------------------- | :----------------------------------------------------------- | ------------------------------------------------------------ |
| AdjustTime(Time, <u>Year</u>, <u>Month</u>, <u>Day</u>, <u>Hour</u>, <u>Minute</u>, <u>Second</u>) | Adjust the given time                                        | Time:Given time<br/><u>Year</u>:Year<br/><u>Month</u>:Month<br/><u>Day</u>:Day<br/><u>Hour</u>:Hour<br/><u>Minute</u>:Minute<br/><u>Second</u>:Second |
| **SecToTime**(**Sec**)                                       | The time to convert seconds to [hh]:mm:ss format             | **Sec**:The number of seconds to convert                         |
| **TimeSub**(**TimeI**, **TimeN**)                            | Calculate the difference between the two times<br/>The output is in seconds | **TimeI**:The first time<br/>**TimeN**:The second time               |
| **TimeSub2**(**TimeI**, **TimeN**, Includ)                   | Calculate the difference between two times in the calculated includ time<br/>The output is in seconds        | **TimeI**:The first time<br/>**TimeN**:The second time<br/>Includ:Calculated includ time |

### 2 Coordinate Calculation

| Function name                    | Function description                                         | Parameter description                                        |
| :------------------------------- | :----------------------------------------------------------- | ------------------------------------------------------------ |
| Distance(Lon1, Lat1, Lon2, Lat2) | Calculate the distance between two WG S84 coordinates latitude and longitude<br/>The output is in meters | Lon1:The first longitude<br/>Lat1:The first latitude<br/>Lon2:The second longitude<br/>Lat2:The second latitude |

### 3 Text Processing

| Function name                             | Function description                                         | Parameter description                                        |
| :---------------------------------------- | :----------------------------------------------------------- | ------------------------------------------------------------ |
| **IsMatch**(**Input**, Pattern)           | Check if the entered string can be matched by a regular expression<br/>If the match is successful, it will return True, and if it fails, it will return False | **Input**:The string entered<br/>Pattern:regular expression                |
| **Matches**(Input, Pattern, <u>Label</u>) | Returns all (or label) content of the entered string that matches in the regular expression         | Input:The string entered<br/>Pattern:regular expression<br/><u>Label</u>:label, optional parameter, label with "$" sign |
| **Replaces**(**Input**, Pattern, Replace) | The part of the input string that is matched by the regular expression is the specified string       | **Input**:The string entered<br/>Pattern:regular expression<br/>Replace:Specify a string with a "$" sign in the label |

### 4 Scope Processing

| Function name                             | Function description                                         | Parameter description                                        |
| ----------------------------------------- | ------------------------------------------------------------ | ------------------------------------------------------------ |
| **VLOOKUP2**(Value, **Array**, **Finds**) | An upgraded version of the VLOOKUP function<br/>This function returns the results of multiple lookups<br/>Return error #VALUE! when the search result is empty<br/>If there is a lot of data, we recommend that you use the `Data Quick Find` | Value:Specify a value<br/>**Array**:Specify the scope<br/>**Finds**:Return range |
| **ANDS**(**Lhs**, **Rhs**)                | AND the **Lhs** and **Rhs** in order                          | **Lhs**:Lvalue range<br/>**Rhs**:Rvalue range                |
| **ORS**(**Lhs**, **Rhs**)                 | OR the **Lhs** and **Rhs** in order                           | **Lhs**:Lvalue range<br/>**Rhs**:Rvalue range                |
| **XORS**(**Lhs**, **Rhs**)                | XOR the **Lhs** and **Rhs** in order                          | **Lhs**:Lvalue range<br/>**Rhs**:Rvalue range                |
| **NOTS**(**Array**)                       | Perform the NOT operation with the `**Array**`                                       | **Array**:Given range                                           |

### 5 Data Quick Find

> Before using the following functions, bind data in the ribbon

| Function name                | Function description     | 参数说明                                         |
| :----------------------------------- | -------------------------------- | ------------------------------------------------ |
| **FLOOKUP**(Key, SerialNumber) | Retrieve a given value in the index and return the corresponding data | Key:Given value<br>SerialNumber:The ordinal number of the column in which the data resides |
| FCOUNTS(Key)                         | Count the number of times a given value appears in the index     | Key:Given value                                       |
| **FGETKEY**()                  | Get the deduplicated index                 |                                                  |

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
> The regular expression engine complies with the [Unicode® Technical Standard #18](https://www.unicode.org/reports/tr18/) specification.
>
> The `Characters Set` please refer to:[Regex Tutorial - Unicode Characters and Properties](https://www.regular-expressions.info/unicode.html#prop)(https://www.regular-expressions.info/unicode.html#prop)

### Matching one character

<pre>
.             any character except new line (includes new line with s flag)
[0-9]         any ASCII digit
\d            digit (\p{Nd})
\D            not digit
\pX           Unicode character class identified by a one-letter name
\p{Greek}     Unicode character class (general category or script)
\PX           Negated Unicode character class identified by a one-letter name
\P{Greek}     negated Unicode character class (general category or script)
</pre>

### Character classes

<pre>
[xyz]         A character class matching either x, y or z (union).
[^xyz]        A character class matching any character except x, y and z.
[a-z]         A character class matching any character in range a-z.
[[:alpha:]]   ASCII character class ([A-Za-z])
[[:^alpha:]]  Negated ASCII character class ([^A-Za-z])
[x[^xyz]]     Nested/grouping character class (matching any character except y and z)
[a-y&amp;&amp;xyz]    Intersection (matching x or y)
[0-9&amp;&amp;[^4]]   Subtraction using intersection and negation (matching 0-9 except 4)
[0-9--4]      Direct subtraction (matching 0-9 except 4)
[a-g~~b-h]    Symmetric difference (matching `a` and `h` only)
[\[\]]        Escaping in character classes (matching [ or ])
[a&amp;&amp;b]        An empty character class matching nothing
</pre>

### Composites

<pre>
xy    concatenation (x followed by y)
x|y   alternation (x or y, prefer x)
</pre>

### Repetitions

<pre>
x*        zero or more of x (greedy)
x+        one or more of x (greedy)
x?        zero or one of x (greedy)
x*?       zero or more of x (ungreedy/lazy)
x+?       one or more of x (ungreedy/lazy)
x??       zero or one of x (ungreedy/lazy)
x{n,m}    at least n x and at most m x (greedy)
x{n,}     at least n x (greedy)
x{n}      exactly n x
x{n,m}?   at least n x and at most m x (ungreedy/lazy)
x{n,}?    at least n x (ungreedy/lazy)
x{n}?     exactly n x
</pre>


### Empty matches

<pre>
^               the beginning of a haystack (or start-of-line with multi-line mode)
$               the end of a haystack (or end-of-line with multi-line mode)
\A              only the beginning of a haystack (even with multi-line mode enabled)
\z              only the end of a haystack (even with multi-line mode enabled)
\b              a Unicode word boundary (\w on one side and \W, \A, or \z on other)
\B              not a Unicode word boundary
\b{start}, \&lt;   a Unicode start-of-word boundary (\W|\A on the left, \w on the right)
\b{end}, \&gt;     a Unicode end-of-word boundary (\w on the left, \W|\z on the right))
\b{start-half}  half of a Unicode start-of-word boundary (\W|\A on the left)
\b{end-half}    half of a Unicode end-of-word boundary (\W|\z on the right)
</pre>

### Grouping and flags

<pre>
(exp)          numbered capture group (indexed by opening parenthesis)
(?P&lt;name&gt;exp)  named (also numbered) capture group (names must be alpha-numeric)
(?&lt;name&gt;exp)   named (also numbered) capture group (names must be alpha-numeric)
(?:exp)        non-capturing group
(?flags)       set flags within current group
(?flags:exp)   set flags for exp (non-capturing)
</pre>

### Escape sequences

<pre>
\*              literal *, applies to all ASCII except [0-9A-Za-z&lt;&gt;]
\a              bell (\x07)
\f              form feed (\x0C)
\t              horizontal tab
\n              new line
\r              carriage return
\v              vertical tab (\x0B)
\A              matches at the beginning of a haystack
\z              matches at the end of a haystack
\b              word boundary assertion
\B              negated word boundary assertion
\b{start}, \&lt;   start-of-word boundary assertion
\b{end}, \&gt;     end-of-word boundary assertion
\b{start-half}  half of a start-of-word boundary assertion
\b{end-half}    half of a end-of-word boundary assertion
\123            octal character code, up to three digits (when enabled)
\x7F            hex character code (exactly two digits)
\x{10FFFF}      any hex character code corresponding to a Unicode code point
\u007F          hex character code (exactly four digits)
\u{7F}          any hex character code corresponding to a Unicode code point
\U0000007F      hex character code (exactly eight digits)
\U{7F}          any hex character code corresponding to a Unicode code point
\p{Letter}      Unicode character class
\P{Letter}      negated Unicode character class
\d, \s, \w      Perl character class
\D, \S, \W      negated Perl character class
</pre>

### Perl character classes (Unicode friendly)

<pre>
\d     digit (\p{Nd})
\D     not digit
\s     whitespace (\p{White_Space})
\S     not whitespace
\w     word character (\p{Alphabetic} + \p{M} + \d + \p{Pc} + \p{Join_Control})
\W     not word character
</pre>

### ASCII character classes

<pre>
[[:alnum:]]    alphanumeric ([0-9A-Za-z])
[[:alpha:]]    alphabetic ([A-Za-z])
[[:ascii:]]    ASCII ([\x00-\x7F])
[[:blank:]]    blank ([\t ])
[[:cntrl:]]    control ([\x00-\x1F\x7F])
[[:digit:]]    digits ([0-9])
[[:graph:]]    graphical ([!-~])
[[:lower:]]    lower case ([a-z])
[[:print:]]    printable ([ -~])
[[:punct:]]    punctuation ([!-/:-@\[-`{-~])
[[:space:]]    whitespace ([\t\n\v\f\r ])
[[:upper:]]    upper case ([A-Z])
[[:word:]]     word characters ([0-9A-Za-z_])
[[:xdigit:]]   hex digit ([0-9A-Fa-f])
</pre>

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
