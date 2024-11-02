﻿using System;
using System.Collections.Generic;
using CommonMark;
using CommonMark.Syntax;

namespace MarkdownParser
{
    public class ViewFormatter<T>
    {
        private readonly ViewWriter<T> _writer;

        public ViewFormatter(IViewSupplier<T> viewSupplier)
        {
            _writer = new ViewWriter<T>(viewSupplier);
        }

        public List<T> Format(Block markdownBlock)
        {
            WriteBlockToView(markdownBlock, _writer);
            _writer.StartAndFinalizeReferenceDefinitions();

            return _writer.Flush();
        }

        public List<T> FormatSingleBlock(Block markdownBlock)
        {
            WriteBlockToView(markdownBlock, _writer, false);

            return _writer.Flush();
        }

        private void WriteBlockToView(Block block, ViewWriter<T> writer, bool continueWithNextSibling = true)
        {
            if (block == null)
            {
                return;
            }

            switch (block.Tag)
            {
                case BlockTag.Document:
                    _writer.RegisterReferenceDefinitions(block.Document.ReferenceMap);
                    WriteBlockToView(block.FirstChild, writer);
                    break;
                case BlockTag.Paragraph:
                    writer.StartBlock(block.Tag);
                    WriteInlineToView(block.InlineContent, writer);
                    writer.FinalizeParagraphBlock();
                    break;
                case BlockTag.BlockQuote:
                    writer.StartBlock(BlockTag.BlockQuote);
                    WriteBlockToView(block.FirstChild, writer);
                    writer.FinalizeBlockquoteBlock();
                    break;
                case BlockTag.List:
                    writer.StartBlock(block.Tag);
                    WriteBlockToView(block.FirstChild, writer);
                    writer.FinalizeListBlock();
                    break;
                case BlockTag.ListItem:
                    writer.StartBlock(block.Tag);
                    WriteBlockToView(block.FirstChild, writer);
                    writer.FinalizeListItemBlock(block.ListData);
                    break;
                case BlockTag.AtxHeading:
                case BlockTag.SetextHeading:
                    writer.StartBlock(block.Tag);
                    WriteInlineToView(block.InlineContent, writer); // needs to be tested
                    WriteBlockToView(block.FirstChild, writer);
                    writer.FinalizeHeaderBlock(block.Heading.Level);
                    break;
                case BlockTag.ThematicBreak:
                    writer.StartAndFinalizeThematicBreak();
                    break;
                case BlockTag.FencedCode:
                    writer.StartAndFinalizeFencedCodeBlock(block.StringContent, block.FencedCodeData.Info);
                    break;
                case BlockTag.IndentedCode:
                    writer.StartAndFinalizeIndentedCodeBlock(block.StringContent);
                    break;
                case BlockTag.HtmlBlock:
                    writer.StartAndFinalizeHtmlBlock(block.StringContent);
                    break;
                case BlockTag.ReferenceDefinition:
                    // ignore, handled at the end of document by _writer.StartAndFinalizeReferenceDefinitions()
                    break;
                default:
                    throw new CommonMarkException("Block type " + block.Tag + " is not supported.", block);
            }

            if (continueWithNextSibling && block.NextSibling != null)
            {
                WriteBlockToView(block.NextSibling, writer);
            }
        }

        private void WriteInlineToView(Inline inline, ViewWriter<T> writer)
        {
            if (inline == null)
            {
                return;
            }

            switch (inline.Tag)
            {
                case InlineTag.String:
                case InlineTag.Code:
                case InlineTag.RawHtml:
                    writer.AddText(inline.LiteralContent);
                    break;
                case InlineTag.Link:
                    // TODO; maybe
                    WriteInlineToView(inline.FirstChild, writer);
                    break;
                case InlineTag.Image:
                    writer.StartAndFinalizeImageBlock(inline.TargetUrl, inline.LiteralContent, inline.FirstChild?.LiteralContent);
                    break;
                case InlineTag.SoftBreak:
                case InlineTag.LineBreak:
                    writer.AddText(writer.GetTextualLineBreak());
                    break;
                case InlineTag.Placeholder:
                    writer.StartAndFinalizePlaceholderBlock(inline.TargetUrl);
                    break;
                case InlineTag.Strikethrough:
                case InlineTag.Emphasis:
                case InlineTag.Strong:
                    WriteInlineToView(inline.FirstChild, writer);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (inline.NextSibling != null)
            {
                WriteInlineToView(inline.NextSibling, writer);
            }
        }

        // Helper for bug CommonMark nested listing
        private List<Block> TrimNestedListBlocks(Block listBlock)
        {
            var listBlockChildTree = listBlock.AsEnumerable();

            var cleanedChildTree = new List<Block>();
            var captureEnabled = true;
            foreach (var child in listBlockChildTree)
            {
                if (child.Block == null)
                {
                    continue;
                }

                if (child.Block != listBlock
                    && child.Block.Tag == BlockTag.List)
                {
                    captureEnabled = child.IsClosing;
                }

                if (captureEnabled
                    && child.IsOpening)
                {
                    cleanedChildTree.Add(child.Block);
                }
            }

            return cleanedChildTree;
        }
    }
}