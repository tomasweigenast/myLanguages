using MyLanguages.Core.Localization;

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
    }
}
