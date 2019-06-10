using MyLanguages.Core.Exceptions;
using System.Globalization;

namespace MyLanguages.Core.Localization
{
    /// <summary>
    /// Represents an installed language
    /// </summary>
    public class Language
    {
        #region Public Properties

        /// <summary>
        /// The language's code
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Represents the TwoLetterISOLanguageName of the language
        /// </summary>
        public string UniversalCode { get; }

        /// <summary>
        /// Represents the subculture of the language. 
        /// For example, in the culture: es-AR, the subculture is AR
        /// </summary>
        public string SubCulture { get; }

        /// <summary>
        /// The name of the resource where the language is
        /// </summary>
        public string Location { get; }

        /// <summary>
        /// Indicates if the language is a universal language, without sublanguages
        /// For example: Español = es
        /// </summary>
        public bool IsUniversal => string.IsNullOrEmpty(SubCulture);

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="Language"/>
        /// </summary>
        /// <param name="code">The code name of the language</param>
        /// <param name="location">The resource location</param>
        public Language(string code, string location)
        {
            try
            {
                var culture = CultureInfo.GetCultureInfo(code);
                Code = code;
                UniversalCode = culture.Parent.Name;
                Location = location;

                if(culture.Name.Contains("-"))
                    SubCulture = culture.Name.Split('-')[1];
            }
            catch
            {
                throw new LanguageNotFoundException("Invalid language code.");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the <see cref="CultureInfo"/> that represents the current language
        /// </summary>
        public CultureInfo GetCulture() => CultureInfo.GetCultureInfo(Code);

        /// <summary>
        /// Returns the native name of the language
        /// </summary>
        public override string ToString() => CultureInfo.GetCultureInfo(Code).NativeName;

        #endregion
    }
}
