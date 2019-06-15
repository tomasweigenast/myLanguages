using MyLanguages.Core.Decoder;
using MyLanguages.Core.Events;
using MyLanguages.Core.Exceptions;
using MyLanguages.Core.Localization;
using MyLanguages.Core.OnlineTranslator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
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
        private LocalizationEngine(ILanguageDecoder decoder)
        {
            // Initialize localizator
            Localizator = new Localize();

            // Set decoder
            if (decoder == null)
                Decoder = new EmbbededLanguageDecoder();
            else
                Decoder = decoder;
        }

        #endregion

        #region Events

        #region Delegates

        /// <summary>
        /// Language changed event handler
        /// </summary>
        public delegate void LanguageChangedHandler(object sender, LanguageChangedEventArgs args);

        #endregion

        /// <summary>
        /// Event raised when a property changes its value
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Event raised when the current language of the application changes
        /// </summary>
        public event LanguageChangedHandler LanguageChanged;

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
        /// Used to get localization entries
        /// </summary>
        public static Localize Localizator { get; internal set; }

        /// <summary>
        /// Gets or sets the language debugger engine
        /// </summary>
        public LocalizationDebuggerEngine DebuggerEngine { get; set; }

        /// <summary>
        /// The current language of the application
        /// </summary>
        public Language CurrentLanguage => LanguageManager.GetCurrentLanguage();

        #endregion

        #region Internal Members

        /// <summary>
        /// The language decoder to use. By default is Embbeded Decoder
        /// </summary>
        internal ILanguageDecoder Decoder { get; } 

        #endregion

        #region Methods

        #region Static

        /// <summary>
        /// Initialize the <see cref="LocalizationEngine"/> and returns an unique instance
        /// </summary>
        /// <param name="decoder">The decoder to use. By default, it will use the <see cref="EmbbededLanguageDecoder"/></param>
        public static LocalizationEngine MakeNew(ILanguageDecoder decoder = null)
        {
            // Setup singleton
            if (mInstance == null)
                mInstance = new LocalizationEngine(decoder);
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
            // Decode languages
            LanguageManager.LoadLanguages(Decoder);

            // Use system language
            if (string.IsNullOrWhiteSpace(defaultLanguage))
                ChangeLanguage(CultureInfo.CurrentCulture.Name);

            // Use specified language
            else
                ChangeLanguage(defaultLanguage);

            return GetInstalledLanguages().Count();
        }

        /// <summary>
        /// Returns true if a key exists
        /// </summary>
        /// <param name="key">The key to check if exists</param>
        public bool KeyExists(string key)
            => LanguageManager.EntryExists(key);

        /// <summary>
        /// Changes the current language of the application
        /// </summary>
        /// <param name="languageCode">The new language</param>
        public bool ChangeLanguage(string languageCode)
        {
            Language oldLang = LanguageManager.GetCurrentLanguage();
            Language newLang;
            // Try get language
            try { newLang = LanguageManager.GetLanguage(languageCode); } catch { return false; }
            LanguageManager.SetLanguage(newLang); // Set the language

            // Fire language changed
            LanguageChanged?.Invoke(this, new LanguageChangedEventArgs(oldLang, newLang));

            return true;
        }

        /// <summary>
        /// Gets an entry from a language for debugging
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="language">The language</param>
        public string DebuggingLanguageEntry(string key, string language)
        {
            // Load language
            var langEntries = LanguageManager.LoadLanguageEntries(language, Decoder);

            return langEntries[key];
        }

        /// <summary>
        /// Returns a readonly list containing all the installed languages
        /// </summary>
        public IEnumerable<Language> GetInstalledLanguages()
            => LanguageManager.GetLangs().ToArray();

        /// <summary>
        /// Translates a local language to another language
        /// </summary>
        /// <param name="sourceLanguage">The language to be translated</param>
        /// <param name="toLanguage">The language to translate to</param>
        /// <param name="translator">The translator to use</param>
        public string[] AddOnlineLanguage(Language sourceLanguage, CultureInfo toLanguage, IOnlineTranslator translator)
        {
            // If the decoder selected is embbeded, throw exception
            if (Decoder is EmbbededLanguageDecoder)
                throw new Exception("This feature is not available using the EmbbededLanguage decoder.");

            // If the source language is not installed, throw exception
            if (!GetInstalledLanguages().Contains(sourceLanguage))
                throw new LanguageNotFoundException($"Language {sourceLanguage.Code} not found.");

            // Get the language file
            if (!File.Exists(sourceLanguage.Location))
                throw new FileNotFoundException("Language file not found.");

            // Get entries
            var entries = LanguageManager.LoadLanguageEntries(sourceLanguage.Code, Decoder);
            var translatedEntries = new Dictionary<string, string>(); // Create a dictionary to store translated entries
            var translationFailed = new List<string>(); // List to store the keys that could not be translated

            // Foreach entry...
            foreach(var entry in entries)
            {
                // Translate the text
                string translated = translator.TranslateText(entry.Value, sourceLanguage.GetCulture(), toLanguage);

                // Add if failed
                if (string.IsNullOrWhiteSpace(translated))
                    translationFailed.Add(entry.Key);

                // Add to translated entries
                translatedEntries.Add(entry.Key, translated.Replace('"', '\''));
            }

            // Write to a file
            string path = Path.Combine((Decoder as PhysicalFileDecoder).Path, $"{toLanguage.Name}{LANG_FILE_EXTENSION}");
            using (var fs = new FileStream(path, FileMode.Create))
            using (var writer = new StreamWriter(fs))
            {
                // Write each entry into the file
                foreach(var entry in translatedEntries)
                    writer.WriteLine($"{entry.Key}={entry.Value}");
            }

            // Create a new language
            Language lang = new Language(toLanguage.Name, path);

            // Add it
            LanguageManager.AddLanguage(lang);

            // Return failed translations as array
            return translationFailed.ToArray();
        }

        /// <summary>
        /// Translates a local language to another language using Google Translation services
        /// </summary>
        /// <param name="sourceLanguage">The language to be translated</param>
        /// <param name="toLanguage">The language to translate to</param>
        public string[] AddOnlineLanguage(Language sourceLanguage, CultureInfo toLanguage)
            => AddOnlineLanguage(sourceLanguage, toLanguage, new GoogleTranslator());

        /// <summary>
        /// Translates a local language to another languages using Google Translation services
        /// </summary>
        /// <param name="sourceLanguage">The language to be translated</param>
        /// <param name="toLanguages">The language to translate to</param>
        public Dictionary<string, string[]> AddOnlineLanguages(Language sourceLanguage, params string[] toLanguages)
        {
            Dictionary<string, string[]> errors = new Dictionary<string, string[]>();

            // Foreach lang...
            foreach(var lang in toLanguages)
            {
                // Check if the language exists
                try { CultureInfo.GetCultureInfo(lang); } catch { throw; }

                // Translate to language
                var result = AddOnlineLanguage(sourceLanguage, CultureInfo.GetCultureInfo(lang));

                // Add errors if any
                errors.Add(lang, result);
            }

            return errors;
        }

        /// <summary>
        /// Returns a language based on its code
        /// </summary>
        /// <param name="code">The code of the language to search</param>
        public Language GetLanguage(string code) => LanguageManager.GetLangs().FirstOrDefault(x => x.Code == code);

        #endregion
    }
}
