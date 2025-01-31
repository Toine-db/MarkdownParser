using System.Collections.Generic;
using System.Linq;
using CommonMark.Syntax;
using MarkdownParser.Models;
using MarkdownParser.Models.Segments;
using MarkdownParser.Models.Segments.Indicators;

namespace MarkdownParser.Writer
{
    public class ViewWriterCache<T>
    {
        public BlockTag? ComponentType { get; set; }

        // Stack to use as build or cache collection
        private readonly Stack<(BaseSegment Segment, T Value)> _valuesStack = new Stack<(BaseSegment, T)>();

        // SegmentIndicators that are not ended/closed yet
        private readonly List<(SegmentIndicator SegmentIndicator, int lastCharacterPosition)> _pendingSegmentIndicators = new List<(SegmentIndicator SegmentIndicator, int lastCharacterPosition)>();

        private readonly T _defaultT = default;

        public void Add(T item)
        {
            if (EqualityComparer<T>.Default.Equals(item, _defaultT))
            {
                return;
            }

            _valuesStack.Push((null, item));
        }

        public void Add(string item, int firstCharacterPosition)
        {
            FinalizeSegmentIndicator(firstCharacterPosition);

            if (string.IsNullOrEmpty(item))
            {
                return;
            }

            var segment = new TextSegment(item);
            _valuesStack.Push((segment, _defaultT));
        }

        public void AddLink(int firstCharacterPosition, int length, string url, string urlTitle)
        {
            FinalizeSegmentIndicator(firstCharacterPosition);

            _pendingSegmentIndicators.Add((SegmentIndicator.Link, firstCharacterPosition + length - 1));

            var segmentIndicator = new LinkSegment(SegmentIndicatorPosition.Start, url, urlTitle);
            _valuesStack.Push((segmentIndicator, _defaultT));
        }

        public void AddPlaceholder(int firstCharacterPosition, int length, string url, string title)
        {
            FinalizeSegmentIndicator(firstCharacterPosition);

            var segmentIndicator = new PlaceholderSegment(url, title);
            _valuesStack.Push((segmentIndicator, _defaultT));
        }

        public void Add(SegmentIndicator indicator, int firstCharacterPosition, int length)
        {
            FinalizeSegmentIndicator(firstCharacterPosition);

            if (indicator != SegmentIndicator.LineBreak)
            {
                _pendingSegmentIndicators.Add((indicator, firstCharacterPosition + length - 1));
            }

            var segmentIndicator = new IndicatorSegment(indicator, SegmentIndicatorPosition.Start);
            _valuesStack.Push((segmentIndicator, _defaultT));
        }

        /// <summary>
        /// Get cached items in order, each Tuple has a text or T value (never both in the same Tuple)
        /// </summary>
        /// <returns>collection that contain TextSegments or T (never both in the same Tuple)</returns>
        public List<(TextBlock TextBlock, T Value)> FlushCache()
        {
            FinalizeRemainingSegmentIndicators();

            var groupedCache = new List<(TextBlock, T)>();

            var textSegmentsCache = new List<BaseSegment>();
            var values = _valuesStack.Reverse().ToArray();

            foreach (var value in values)
            {
                // Check for text
                if (value.Segment != null)
                {
                    textSegmentsCache.Add(value.Segment);
                    continue;
                }

                // No text anymore: Store workbenchItemTextCache if any to groupedCache
                if (textSegmentsCache.Any())
                {
                    groupedCache.Add((new TextBlock(textSegmentsCache.ToArray()), _defaultT));
                    textSegmentsCache.Clear();
                }

                // If item2 is not null
                if (!EqualityComparer<T>.Default.Equals(value.Item2, _defaultT))
                {
                    groupedCache.Add((null, value.Item2));
                }
            }

            // Store leftovers workbenchItemTextCache 
            if (textSegmentsCache.Any())
            {
                groupedCache.Add((new TextBlock(textSegmentsCache.ToArray()), _defaultT));
                textSegmentsCache.Clear();
            }

            return groupedCache;
        }

        private void FinalizeSegmentIndicator(int toWritePosition)
        {
            if (!_pendingSegmentIndicators.Any())
            {
                return;
            }

            var pendingIndicators = _pendingSegmentIndicators.ToArray();
            foreach (var pendingIndicator in pendingIndicators)
            {
                if (toWritePosition > pendingIndicator.lastCharacterPosition)
                {
                    _pendingSegmentIndicators.Remove(pendingIndicator);

                    IndicatorSegment segmentIndicator;
                    switch (pendingIndicator.SegmentIndicator)
                    {
                        case SegmentIndicator.Link:
                            segmentIndicator = new LinkSegment(SegmentIndicatorPosition.End, string.Empty, string.Empty);
                            break;
                        default:
                            segmentIndicator = new IndicatorSegment(pendingIndicator.SegmentIndicator, SegmentIndicatorPosition.End);
                            break;
                    }

                    _valuesStack.Push((segmentIndicator, _defaultT));
                }
            }
        }

        private void FinalizeRemainingSegmentIndicators()
        {
            if (!_pendingSegmentIndicators.Any())
            {
                return;
            }

            var pendingSegmentIndicators = _pendingSegmentIndicators.OrderBy(x => x.lastCharacterPosition);
            foreach (var pendingSegmentIndicator in pendingSegmentIndicators)
            {
                var segmentIndicator = new IndicatorSegment(pendingSegmentIndicator.SegmentIndicator, SegmentIndicatorPosition.End);
                _valuesStack.Push((segmentIndicator, _defaultT));
            }

            _pendingSegmentIndicators.Clear();
        }
    }
}