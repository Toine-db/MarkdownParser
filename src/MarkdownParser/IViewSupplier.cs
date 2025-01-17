using System.Collections.Generic;
using MarkdownParser.Models;

namespace MarkdownParser
{
    public interface IViewSupplier<T>
    {
        /// <summary>
        /// reference definitions are known, triggered before first view is created (reference definitions are sometimes placed at the end of the document or used for creating links)
        /// </summary>
        /// <param name="markdownReferenceDefinitions">collection of Reference Definitions</param>
        /// <returns></returns>
        void OnReferenceDefinitionsPublished(IEnumerable<MarkdownReferenceDefinition> markdownReferenceDefinitions);

        /// <summary>
        /// a default text view
        /// </summary>
        /// <param name="textBlock">textBlock with a collection of text segments</param>
        /// <returns></returns>
        T CreateTextView(TextBlock textBlock);

        /// <summary>
        /// a block quote view, where other views can be inserted
        /// </summary>
        /// <param name="childView"></param>
        /// <returns></returns>
        T CreateBlockquotesView(T childView);

        /// <summary>
        /// a title, subtitle or header view
        /// </summary>
        /// <param name="textBlock">textBlock with a collection of text segments</param>
        /// <param name="headerLevel">header level</param>
        /// <returns></returns>
        T CreateHeaderView(TextBlock textBlock, int headerLevel);

        /// <summary>
        /// a image view with an optional subscription text view
        /// </summary>
        /// <param name="url">image location</param>
        /// <param name="subscription">(optional) null or empty when not used</param>
        /// <param name="imageId">(optional) id for image</param>
        /// <returns></returns>
        T CreateImageView(string url, string subscription, string imageId);

        /// <summary>
        /// a view that shows a list of list-items 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        T CreateListView(List<T> items);

        /// <summary>
        /// a view that shows a single item for a ListView (return View can be used in CreateListView)
        /// </summary>
        /// <param name="childView">view as child-view (or use the content parameter)</param>
        /// <param name="isOrderedList">does the item belong to a ordered (numbered) list</param>
        /// <param name="sequenceNumber">number of sequence</param>
        /// <param name="listLevel">level depth of the list, root level starting at 1</param>
        /// <returns></returns>
        T CreateListItemView(T childView, bool isOrderedList, int sequenceNumber, int listLevel);

        /// <summary>
        /// a layout that shows a collection of views
        /// </summary>
        /// <param name="childViews">collection of views</param>
        /// <returns></returns>
        T CreateStackLayoutView(List<T> childViews);

        /// <summary>
        /// an image view that separates content 
        /// </summary>
        /// <returns></returns>
        T CreateThematicBreak();

        /// <summary>
        /// a placeholder for views or other objects
        /// </summary>
        /// <param name="placeholderName">placeholder string</param>
        /// <returns></returns>
        T CreatePlaceholder(string placeholderName);

        /// <summary>
        /// a view that shows fenced code (found in MD blocks starting with ```cs )
        /// </summary>
        /// <returns></returns>
        T CreateFencedCodeBlock(TextBlock textBlock, string codeInfo);

        /// <summary>
        /// a view that shows indented code (found in MD lines starting with at least 4 spaces)
        /// </summary>
        /// <returns></returns>
        T CreateIndentedCodeBlock(TextBlock textBlock);

        /// <summary>
        /// a view that shows html content
        /// </summary>
        /// <returns></returns>
        T CreateHtmlBlock(TextBlock textBlock);

        /// <summary>
        /// clear all stored values to reuse the IViewSupplier
        /// </summary>
        void Clear();
    }
}
