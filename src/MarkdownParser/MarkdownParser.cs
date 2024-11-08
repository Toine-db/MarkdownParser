using System.Collections.Generic;
using System.IO;
using CommonMark;
using CommonMark.Syntax;
using MarkdownParser.Writer;

namespace MarkdownParser
{
    public class MarkdownParser<T>
    {
        private readonly IViewSupplier<T> _viewSupplier;

        public MarkdownParser(IViewSupplier<T> viewSupplier)
        {
            _viewSupplier = viewSupplier;
        }

        /// <summary>
        /// Format\Convert markdown into UI Components
        /// </summary>
        /// <param name="markdownSource"></param>
        /// <returns></returns>
        public List<T> Parse(string markdownSource)
        {
            using (var reader = new StringReader(markdownSource))
            {
                return Parse(reader);
            }
        }

        /// <summary>
        /// Format\Convert markdown into UI Components
        /// </summary>
        /// <param name="markdownSource"></param>
        /// <returns></returns>
        public List<T> Parse(TextReader markdownSource)
        {
            // Parse to usable c# objects
            var markdownDocument = GetMarkdownDocument(markdownSource);

            // Format\Convert c# objects into <T> UI Components
            var formatter = new ViewFormatter<T>(_viewSupplier);
            var uiComponents = formatter.Format(markdownDocument);

            return uiComponents;
        }

        /// <summary>
        /// Get usable c# objects from 
        /// </summary>s
        /// <param name="markdownSource"></param>
        /// <returns></returns>
        internal static Block GetMarkdownDocument(TextReader markdownSource)
        {
            // Parse to usable c# objects
            var settings = CommonMarkSettings.Default.Clone();
            settings.AdditionalFeatures |= CommonMarkAdditionalFeatures.PlaceholderBracket;
            settings.AdditionalFeatures |= CommonMarkAdditionalFeatures.StrikethroughTilde;
            
            return CommonMarkConverter.Parse(markdownSource, settings);
        }
    }
}
