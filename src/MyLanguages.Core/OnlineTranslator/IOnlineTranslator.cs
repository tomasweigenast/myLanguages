using System.Globalization;

namespace MyLanguages.Core.OnlineTranslator
{
    /// <summary>
    /// Defines a base class to implement an online translator
    /// </summary>
    public interface IOnlineTranslator
    {
        /// <summary>
        /// Translates a text
        /// </summary>
        /// <param name="text">The text to translate</param>
        /// <param name="from">The source language</param>
        /// <param name="to">The language to translate the text</param>
        string TranslateText(string text, CultureInfo from, CultureInfo to);
    }
}
