using MyLanguages.Core.Localization;
using System.IO;

namespace MyLanguages.Core.Decoder
{
    /// <summary>
    /// A base class that acts a language decoder
    /// </summary>
    public interface ILanguageDecoder
    {
        /// <summary>
        /// Gets the languages from the decoder
        /// </summary>
        Language[] Decode();

        /// <summary>
        /// Returns a boolean indicating if the language file exists
        /// </summary>
        bool LanguageFileExists(Language language);

        /// <summary>
        /// Returns the corresponding stream for the decoder
        /// </summary>
        StreamReader GetStream(Language language);
    }
}
