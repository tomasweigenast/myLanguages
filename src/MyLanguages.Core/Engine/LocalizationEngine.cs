using MyLanguages.Core.Localization;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MyLanguages.Core.Engine
{
    /// <summary>
    /// Localization engine used to add new languages, get values, etc
    /// </summary>
    public class LocalizationEngine : INotifyPropertyChanged
    {
        #region Singleton

        private static LocalizationEngine mInstance;

        /// <summary>
        /// A single instance of the <see cref="LocalizationEngine"/>
        /// </summary>
        public static LocalizationEngine Instance
        {
            get
            {
                // Throw exception is null
                if (mInstance == null)
                    throw new Exception("The Localization Engine is not initialized. Set it up by using MakeNew().");

                // Return singleton
                return mInstance;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Makes sure its truly lazy
        /// </summary>
        static LocalizationEngine() { }

        /// <summary>
        /// Prevent instantiation outside
        /// </summary>
        private LocalizationEngine()
        {
            // Initialize localizator
            Localizator = new Localize();
        }

        #endregion

        #region Property Changed Interface

        /// <summary>
        /// Event raised when a property changes its value
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Method to be executed when a property changes
        /// </summary>
        /// <param name="propertyName">The name of the property to alert of his change</param>
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
            => PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));

        #endregion

        #region Constants

        /// <summary>
        /// The default extension for any language file
        /// </summary>
        internal const string LANG_FILE_EXTENSION = ".lang";

        #endregion

        #region Public Properties

        /// <summary>
        /// The current language of the application
        /// </summary>
        public Language CurrentLanguage => LanguageManager.GetCurrentLanguage();

        /// <summary>
        /// Used to get localization entries
        /// </summary>
        public static Localize Localizator { get; internal set; }

        #endregion

        #region Methods

        #region Static

        /// <summary>
        /// Initialize the <see cref="LocalizationEngine"/> and returns an unique instance
        /// </summary>
        public static LocalizationEngine MakeNew()
        {
            // Setup singleton
            if (mInstance == null)
                mInstance = new LocalizationEngine();
            else
                throw new InvalidOperationException("There are already another instance of the Localization Engine");

            return mInstance;
        }

        #endregion

        /// <summary>
        /// Loads the localization engine and install all the detected languages in the assembly.
        /// Returns the amount of loaded languages
        /// </summary>
        /// <param name="defaultLanguage">The default language to use. If its null, the system language will be used.</param>
        public int DetectLanguages(string defaultLanguage = null)
        {
            // Load languages
            LanguageManager.LoadFromAssemblyDetection();

            // Use system language
            if (string.IsNullOrWhiteSpace(defaultLanguage))
                ChangeLanguage(CultureInfo.CurrentCulture.Name);

            // Use specified language
            else
                ChangeLanguage(defaultLanguage);

            return GetInstalledLanguages().Length;
        }

        /// <summary>
        /// Changes the current language of the application
        /// </summary>
        /// <param name="languageCode">The new language</param>
        public bool ChangeLanguage(string languageCode)
        {
            Language newLang = null;
            // Try get language
            try { newLang = LanguageManager.GetLanguage(languageCode); } catch { return false; }
            LanguageManager.SetLanguage(newLang); // Set the language

            // Update culture
            OnPropertyChanged(nameof(CurrentLanguage));

            return true;
        }

        /// <summary>
        /// Returns a readonly list containing all the installed languages
        /// </summary>
        public Language[] GetInstalledLanguages()
            => LanguageManager.GetLangs().ToArray();

        #endregion
    }
}
