![nuget.png](https://raw.githubusercontent.com/Toine-db/MarkdownParser/main/nuget.png)
# MarkdownParser

`MarkdownParser` provides the ability to parse Markdown text in to a nested UI structure.

## Install Plugin

[![NuGet](https://img.shields.io/nuget/v/MarkdownParser?label=NuGet)](https://www.nuget.org/packages/MarkdownParser/)

Available on [NuGet](http://www.nuget.org/packages/MarkdownParser).

Install with the dotnet CLI: `dotnet add package MarkdownParser`, or through the NuGet Package Manager in Visual Studio.

## The Mechanism
1. CommonMark.NET is used to read Markdown and turn them into usable c# objects
2. A custom formatter is created to work with the created c# objects (formatter looks like a CommonMark.NET formatter)
3. A custom writer is created to control creation of ui components
4. You need to create: An UI component generator (IViewSupplier) must be created which supplies the ui components (one for every platform)

Need any help on creating a IViewSupplier? check out [Plugin.Maui.MarkdownView](https://github.com/Toine-db/Plugin.Maui.MarkdownView).