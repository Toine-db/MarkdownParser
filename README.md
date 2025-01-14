![nuget.png](https://raw.githubusercontent.com/Toine-db/MarkdownParser/main/nuget.png)
# MarkdownParser

`MarkdownParser` provides the ability to parse Markdown text in to a nested UI structure.

## Install Plugin

[![NuGet](https://img.shields.io/nuget/v/MarkdownParser?label=NuGet)](https://www.nuget.org/packages/MarkdownParser/)

Available on [NuGet](http://www.nuget.org/packages/MarkdownParser).

Install with the dotnet CLI: `dotnet add package MarkdownParser`
(or use your preferred NuGet Package Manager).

## Roadmap 
- [x] Original Markdown syntax support
- [ ] Replace '[CommonMark.NET](https://github.com/Knagis/CommonMark.NET)' for '[Markdig](https://github.com/xoofx/markdig)'
  - _for extended markdown syntax_

## How it works
1. CommonMark.NET is used to read Markdown and turn them into c# objects
2. MarkdownParser creates a custom formatter to work with the created c# objects
3. MarkdownParser creates a custom writer to control creation of ui components
3. MarkdownParser creates a custom writer to call the IViewSupplier and manage the creation of UI components
4. __You__ need to create/provide an UI component generator (IViewSupplier), one for every platform.

If you need any help with creating a IViewSupplier? check out [Plugin.Maui.MarkdownView](https://github.com/Toine-db/Plugin.Maui.MarkdownView).

## Supported
- :heavy_check_mark: All basic Markdown syntaxes are supported
  * _see basic Markdown syntax on the [markdownguide basic-syntax page](https://markdownguide.offshoot.io/basic-syntax/)__.


## Not (yet) Supported
The basic syntax created in the original Markdown design document provides enough elements for everyday use. However, over time, various extensions have been released to meet specific needs, sometimes using different syntaxes generating the same result. Examples markdown extended syntaxes on [markdownguide extended-syntax page](https://markdownguide.offshoot.io/extended-syntax/).

While MarkdownParser supports a small variety of extended syntaxes, it does NOT support the following widely used extended syntaxes:
- :x: Tables
- :x: Emoji Shortcodes
- :x: Highlights

The solution is replacing the package '[CommonMark.NET](https://github.com/Knagis/CommonMark.NET)' by '[Markdig](https://github.com/xoofx/markdig)' (see Roadmap).

