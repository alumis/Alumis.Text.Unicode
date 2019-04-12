# Alumis.Text.Unicode

One goal of this library is to treat Unicode strings as a series of grapheme clusters, as opposed to a series of UTF-16 code units (char).

This is implemented via the class GraphemeString.

Various extension methods are also available.

## Grapheme Clusters

The most basic unit in Unicode is the code point (a 32-bit value). However, more than one code point can be used to represent a single user-perceived character.

For example, the user-perceived character g̈ is made up of two code points:

0067 ( g ) LATIN SMALL LETTER G and
0308 ( ◌̈ ) COMBINING DIAERESIS

This is called a grapheme cluster.

### Examples

```
var utf16String = "g̈";
var graphemeString = new Alumis.Text.Unicode.GraphemeString(utf16String);

Console.WriteLine(utf16String.Length); // 2
Console.WriteLine(graphemeString.Length); // 1

utf16String = "g̈test";
graphemeString = new Alumis.Text.Unicode.GraphemeString(utf16String);

Console.WriteLine(utf16String.Substring(0, 5)); // g̈tes
Console.WriteLine(graphemeString.Substring(0, 5)); // g̈test

// Iterating grapheme clusers
foreach (var s in graphemeString)
  Console.WriteLine(s);
```

## Extension methods
```
void AppendCodePoint(this StringBuilder stringBuilder, uint cp);
byte GetUtf8Lo(this byte b);
bool IsUtf8Lo(this byte b);
bool IsNewlineGrapheme(this string str); // E.g. both \r\n and \n will yield true
bool IsHexGrapheme(this string str);
bool IsDecGrapheme(this string str);
uint LastCodePoint(this string str); // Returns the last code point in the string.
bool IsWhitespaceGrapheme(this string str);

// The following two methods are useful for tokenization (see http://www.unicode.org/reports/tr31/tr31-31.html#Default_Identifier_Syntax)
bool HasBinaryPropertyXidContinue(this uint cp);
bool HasBinaryPropertyXidStart(this uint cp);
```
## Installation

### Package Manager

Install-Package Alumis.Text.Unicode -Version 1.0.8

### .NET CLI

dotnet add package Alumis.Text.Unicode --version 1.0.8

### Paket CLI

paket add Alumis.Text.Unicode --version 1.0.8
