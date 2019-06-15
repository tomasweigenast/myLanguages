using MyLanguages.Core.Localization;
using System;

namespace MyLanguages.Core.Events
{
    /// <summary>
    /// Provides the arguments to fire the LanguageChanged event
    /// </summary>
    public class LanguageChangedEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// The language that changed
        /// </summary>
        public Language OldLanguage { get; }

        /// <summary>
        /// The new language of the application
        /// </summary>
        public Language NewLanguage { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="oldLanguage">The language that changed</param>
        /// <param name="newLanguage">The new language of the application</param>
        public LanguageChangedEventArgs(Language oldLanguage, Language newLanguage)
        {
            OldLanguage = oldLanguage;
            NewLanguage = newLanguage;
        }

        #endregion
    }
}
