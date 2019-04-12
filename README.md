# Alumis.Text.Unicode

This library’s main goal is to treat Unicode strings as a series of grapheme clusters, as opposed to a series of UTF-16 code units (char).

This is implemented via the class GraphemeString.

## Grapheme Clusters

The most basic unit in Unicode is the code point (a 32-bit value). However, more than one code point can be used to represent a single user-perceived character.

For example, the user-perceived character g̈ is made up of two code points:

0067 ( g ) LATIN SMALL LETTER G and
0308 ( ◌̈ ) COMBINING DIAERESIS

This is called a grapheme cluster.

## Examples

```
var utf16String = "g̈";
var graphemeString = new Alumis.Text.Unicode.GraphemeString(utf16String);

Console.WriteLine(utf16String.Length); // 2
Console.WriteLine(graphemeString.Length); // 1

utf16String = "g̈test";
graphemeString = new Alumis.Text.Unicode.GraphemeString(utf16String);

Console.WriteLine(utf16String.Substring(0, 5)); // g̈tes
Console.WriteLine(graphemeString.Substring(0, 5)); // g̈test
```

## Installation

### Package Manager

Install-Package Alumis.Text.Unicode -Version 1.0.8

### .NET CLI

dotnet add package Alumis.Text.Unicode --version 1.0.8

### Paket CLI

paket add Alumis.Text.Unicode --version 1.0.8
