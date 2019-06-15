using MyLanguages.Core.Exceptions;
using System;
using System.Globalization;
using System.Linq;

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

        /// <summary>
        /// An id that identifies the language in the current instance
        /// </summary>
        public string UniqueId { get; }

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
                UniversalCode = culture.TwoLetterISOLanguageName;
                Location = location;
                UniqueId = Guid.NewGuid().ToString();

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
        public override string ToString()
        {
            var culture = GetCulture();
            string finalStr = "";

            if (culture.Parent == null || string.IsNullOrWhiteSpace(culture.Parent.Name))
                finalStr = $"{culture.NativeName.First().ToString().ToUpper()}{string.Join("", culture.NativeName.Skip(1))} ({culture.DisplayName})";
            else
            {
                // Nativename culture
                string[] nativeAlfabetization = culture.NativeName.Split('(');
                string originalName = nativeAlfabetization[0];
                string subCulture = nativeAlfabetization[1].Replace("(", "").Replace(")", "");

                // Displayname alfabetization
                string[] displayAlfabetization = culture.DisplayName.Split('(');
                string displayName = displayAlfabetization[0];
                string displayNameSubCulture = displayAlfabetization[1].Replace("(", "").Replace(")", "");

                finalStr = $"{originalName.First().ToString().ToUpper()}{string.Join("", originalName.Skip(1))}- {subCulture} ({displayName}- {displayNameSubCulture})";

            }

            return finalStr;
        }

        #endregion
    }
}
