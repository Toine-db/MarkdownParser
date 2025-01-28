using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CommonMark.Syntax;
using MarkdownParser.Models;
using MarkdownParser.Models.Segments;
using MarkdownParser.Models.Segments.Indicators;

namespace MarkdownParser.Writer
{
    public class ViewWriter<T>
    {
        private IViewSupplier<T> ViewSupplier { get; }
        private List<T> WrittenViews { get; set; } = new List<T>();
        private Stack<ViewWriterCache<T>> Workbench { get; } = new Stack<ViewWriterCache<T>>();
        private Dictionary<string, Reference> _referenceDefinitions;

        public ViewWriter(IViewSupplier<T> viewSupplier)
        {
            ViewSupplier = viewSupplier;
        }

        private ViewWriterCache<T> GetWorkbenchItem()
        {
            if (Workbench.Count == 0)
            {
                return null;
            }

            return Workbench.Peek();
        }

        public List<T> Flush()
        {
            var collectedViews = WrittenViews;
            WrittenViews = new List<T>();

            return collectedViews;
        }

        public void RegisterReferenceDefinitions(Dictionary<string, Reference> referenceDefinitions)
        {
            _referenceDefinitions = referenceDefinitions;

            if (_referenceDefinitions == null || _referenceDefinitions.Count == 0)
            {
                return;
            }

            var markdownReferenceDefinition = new List<MarkdownReferenceDefinition>();
            foreach (var referenceDefinition in _referenceDefinitions)
            {
                if (referenceDefinition.Value == null)
                {
                    continue;
                }

                markdownReferenceDefinition.Add(new MarkdownReferenceDefinition()
                {
                    IsPlaceholder = referenceDefinition.Value.IsPlaceholder,
                    Label = referenceDefinition.Value.Label,
                    Title = referenceDefinition.Value.Title,
                    Url = referenceDefinition.Value.Url
                });
            }

            ViewSupplier.OnReferenceDefinitionsPublished(markdownReferenceDefinition);
        }

        public void StartBlock(BlockTag blockType)
        {
            Workbench.Push(new ViewWriterCache<T> { ComponentType = blockType });
        }
        
        public void FinalizeParagraphBlock()
        {
            var wbi = GetWorkbenchItem();
            if (wbi.ComponentType != BlockTag.Paragraph)
            {
                Debug.WriteLine($"Finalizing Paragraph can not finalize {wbi.ComponentType}");
                return;
            }

            var views = new List<T>();

            var topWorkbenchItem = Workbench.Pop();
            var itemsCache = topWorkbenchItem.FlushCache();

            foreach (var itemsCacheTuple in itemsCache)
            {
                T view;
                if (itemsCacheTuple.TextBlock != null)
                {
                    itemsCacheTuple.TextBlock.AncestorsTree = GetAncestorsTreeFromWorkbench(BlockType.Paragraph);
                    view = ViewSupplier.CreateTextView(itemsCacheTuple.TextBlock);
                }
                else
                {
                    view = itemsCacheTuple.Value;
                }

                if (view != null)
                {
                    views.Add(view);
                }
            }

            StoreView(StackViews(views));
        }

        public void FinalizeBlockquoteBlock()
        {
            var wbi = GetWorkbenchItem();
            if (wbi.ComponentType != BlockTag.BlockQuote)
            {
                Debug.WriteLine($"Finalizing BlockQuote can not finalize {wbi.ComponentType}");
                return;
            }

            var topWorkbenchItem = Workbench.Pop();
            var itemsCache = topWorkbenchItem.FlushCache();

            var childViews = itemsCache.Select(itemsCacheTuple => itemsCacheTuple.Item2).ToList();
            var childView = StackViews(childViews);

            var blockView = ViewSupplier.CreateBlockquotesView(childView);

            StoreView(blockView);
        }

        public void FinalizeHeaderBlock(int headerLevel)
        {
            var wbi = GetWorkbenchItem();
            if (wbi.ComponentType != BlockTag.AtxHeading
                && wbi.ComponentType != BlockTag.SetextHeading)
            {
                Debug.WriteLine($"Finalizing Header can not finalize {wbi.ComponentType}");
                return;
            }

            var views = new List<T>();

            var topWorkbenchItem = Workbench.Pop();
            var itemsCache = topWorkbenchItem.FlushCache();

            foreach (var itemsCacheTuple in itemsCache)
            {
                T view;
                if (itemsCacheTuple.TextBlock != null)
                {
                    itemsCacheTuple.TextBlock.AncestorsTree = GetAncestorsTreeFromWorkbench(BlockType.Heading);
                    view = ViewSupplier.CreateHeaderView(itemsCacheTuple.TextBlock, headerLevel);
                }
                else
                {
                    view = itemsCacheTuple.Value;
                }

                views.Add(view);
            }

            StoreView(StackViews(views));
        }

        public void FinalizeListBlock()
        {
            var wbi = GetWorkbenchItem();
            if (wbi.ComponentType != BlockTag.List)
            {
                Debug.WriteLine($"Finalizing List can not finalize {wbi.ComponentType}");
                return;
            }

            var topWorkbenchItem = Workbench.Pop();
            var itemsCache = topWorkbenchItem.FlushCache();

            var listItems = itemsCache.Select(itemsCacheTuple => itemsCacheTuple.Item2).ToList();
            var listView = ViewSupplier.CreateListView(listItems);

            StoreView(listView);
        }

        public void FinalizeListItemBlock(ListData listData)
        {
            var wbi = GetWorkbenchItem();
            if (wbi.ComponentType != BlockTag.ListItem)
            {
                Debug.WriteLine($"Finalizing ListItem can not finalize {wbi.ComponentType}");
                return;
            }

            var views = new List<T>();

            var isOrderedList = listData.ListType == ListType.Ordered;
            var sequenceNumber = listData.Start;
            var depthLevel = Workbench.Count(wbItem => wbItem.ComponentType == BlockTag.List);

            var topWorkbenchItem = Workbench.Pop();
            var itemsCache = topWorkbenchItem.FlushCache();

            foreach (var itemsCacheTuple in itemsCache)
            {
                T view;
                if (itemsCacheTuple.TextBlock != null)
                {
                    itemsCacheTuple.TextBlock.AncestorsTree = GetAncestorsTreeFromWorkbench(BlockType.List);
                    view = ViewSupplier.CreateTextView(itemsCacheTuple.TextBlock);
                }
                else
                {
                    view = itemsCacheTuple.Value;
                } 

                if (view != null)
                {
                    views.Add(view);
                }
            }

            var flattenedView = StackViews(views);

            var listItemView = ViewSupplier.CreateListItemView(flattenedView, isOrderedList, sequenceNumber, depthLevel);

            StoreView(listItemView);
        }

        public void AddText(string content, int firstCharacterPosition)
        {
            GetWorkbenchItem().Add(content, firstCharacterPosition);
        }

        public void AddLink(Inline inline, int firstCharacterPosition, int length, string url, string urlTitle)
        {
            GetWorkbenchItem().AddLink(firstCharacterPosition, length, url, urlTitle);
        }

        public void AddEmphasis(Inline inline, int firstCharacterPosition, int length)
        {
            SegmentIndicator indicator;
            switch (inline.Tag)
            {
                case InlineTag.Strikethrough:
                    indicator = SegmentIndicator.Strikethrough;
                    break;
                case InlineTag.Strong:
                    indicator = SegmentIndicator.Strong;
                    break;
                case InlineTag.Code:
                    indicator = SegmentIndicator.Code;
                    break;
                case InlineTag.Emphasis:
                    indicator = SegmentIndicator.Italic;
                    break;
                case InlineTag.LineBreak:
                case InlineTag.SoftBreak:
                    indicator = SegmentIndicator.LineBreak;
                    break;
                default:
                    indicator = SegmentIndicator.NotSupported;
                    break;
            }

            if (indicator == SegmentIndicator.NotSupported)
            {
                return;
            }

            GetWorkbenchItem().Add(indicator, firstCharacterPosition, length);
        }

        public void StartAndFinalizeImageBlock(string targetUrl, string subscription, string imageId)
        {
            var imageView = ViewSupplier.CreateImageView(targetUrl, subscription, imageId);
            StoreView(imageView);
        }

        public void StartAndFinalizeFencedCodeBlock(StringContent content, string blockInfo)
        {
            var blocks = StringContentToBlocks(content);
            blocks.AncestorsTree = GetAncestorsTreeFromWorkbench(BlockType.FencedCode);

            var blockView = ViewSupplier.CreateFencedCodeBlock(blocks, blockInfo);
            StoreView(blockView);
        }

        public void StartAndFinalizeIndentedCodeBlock(StringContent content)
        {
            var blocks = StringContentToBlocks(content);
            blocks.AncestorsTree = GetAncestorsTreeFromWorkbench(BlockType.IndentedCode);

            var blockView = ViewSupplier.CreateIndentedCodeBlock(blocks);
            StoreView(blockView);
        }

        public void StartAndFinalizeHtmlBlock(StringContent content)
        {
            var blocks = StringContentToBlocks(content);
            blocks.AncestorsTree = GetAncestorsTreeFromWorkbench(BlockType.Html);

            var blockView = ViewSupplier.CreateHtmlBlock(blocks);
            StoreView(blockView);
        }

        public void StartAndFinalizeThematicBreak()
        {
            var separator = ViewSupplier.CreateThematicBreak();
            StoreView(separator);
        }

        public void StartAndFinalizePlaceholderBlock(string placeholderName)
        {
            var placeholderView = ViewSupplier.CreatePlaceholder(placeholderName);
            StoreView(placeholderView);
        }

        private BlockType[] GetAncestorsTreeFromWorkbench(BlockType currentBlockType)
        {
            var blockTypeTree = new List<BlockType>();
            foreach (var workBenchItem in Workbench.Reverse())
            {
                switch (workBenchItem.ComponentType)
                {
                    case BlockTag.BlockQuote:
                        blockTypeTree.Add(BlockType.Quote);
                        break;
                    case BlockTag.List:
                        blockTypeTree.Add(BlockType.List);
                        break;
                    case BlockTag.FencedCode:
                        blockTypeTree.Add(BlockType.FencedCode);
                        break;
                    case BlockTag.IndentedCode:
                        blockTypeTree.Add(BlockType.IndentedCode);
                        break;
                    case BlockTag.HtmlBlock:
                        blockTypeTree.Add(BlockType.Html);
                        break;
                    case BlockTag.Paragraph:
                        blockTypeTree.Add(BlockType.Paragraph);
                        break;
                    case BlockTag.AtxHeading:
                    case BlockTag.SetextHeading:
                        blockTypeTree.Add(BlockType.Heading);
                        break;
                    case BlockTag.ListItem: // this is already covered by BlockTag.List
                    case BlockTag.Document:
                    case BlockTag.ThematicBreak:
                    case BlockTag.ReferenceDefinition:
                    case null:
                    default:
                        break;
                }
            }

            // Add current BlockType as most recent Ancestor
            // but skip Lists as current to prevent double List entries
            // because current List and ListItem levels are already picked up from Workbench
            if (currentBlockType != BlockType.List) 
            {
                blockTypeTree.Add(currentBlockType);
            }

            return blockTypeTree.ToArray();
        }

        private T StackViews(List<T> views)
        {
            if (views == null
                || views.Count == 0)
            {
                return default;
            }

            // multiple views combine a single stack layout
            var viewToStore = views.Count == 1
                ? views[0]
                : ViewSupplier.CreateStackLayoutView(views);

            return viewToStore;
        }

        private void StoreView(T view)
        {
            if (view == null)
            {
                return;
            }

            // Check if Workbench has an item where it's working on
            var wbi = GetWorkbenchItem();
            if (wbi != null)  // add the new View to the WorkbenchItem
            {
                wbi.Add(view);
            }
            else // otherwise add the new View to finalized views collection
            {
                WrittenViews.Add(view);
            }
        }

        private TextBlock StringContentToBlocks(StringContent content)
        {
            var stringWriter = new StringWriter();
            content.WriteTo(stringWriter);
            var contentLines = stringWriter.ToString();

            contentLines = contentLines.Replace("\r", "");
            contentLines = contentLines.TrimEnd('\n');

            var contentParts = contentLines.Split('\n');
            var segments = new List<BaseSegment>();
            if (contentParts.Any())
            {
                segments.Add(new TextSegment(contentParts.First()));

                for (var i = 1; i < contentParts.Length; i++)
                {
                    var lineBreakSegment = new IndicatorSegment(SegmentIndicator.LineBreak, SegmentIndicatorPosition.Start);
                    segments.Add(lineBreakSegment);

                    var textSegment = new TextSegment(contentParts[i]);
                    segments.Add(textSegment);
                }
            }

            var textBlock = new TextBlock(segments.ToArray());

            return textBlock;
        }
    }
}
