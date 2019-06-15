using MyLanguages.Core.Decoder;
using MyLanguages.Core.Engine;
using MyLanguages.Core.Exceptions;
using MyLanguages.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyLanguages.Core.Localization
{
    /// <summary>
    /// Provides methods to work with Languages
    /// </summary>
    internal static class LanguageManager
    {
        #region Private Members

        private static readonly IList<Language> mLanguages = new List<Language>(); // All loaded languages
        private static readonly Dictionary<string, string> mEntries = new Dictionary<string, string>(); // The actual language entrie
        private static Language mCurrentLanguage; // The actual language

        #endregion

        #region Methods

        /// <summary>
        /// Searchs language files in embbeded files of the assembly
        /// </summary>
        public static void LoadLanguages(ILanguageDecoder decoder)
        {
            // Foreach decoded language
            foreach (var lang in decoder.Decode())
            {
                // If the language is duplicated, skip
                if (mLanguages.Any(x => x.Code == lang.Code))
                    continue;

                // Add it
                mLanguages.Add(lang);
            }
        }

        /// <summary>
        /// Returns the current language of the application
        /// </summary>
        public static Language GetCurrentLanguage() => mCurrentLanguage;

        /// <summary>
        /// Loads a language in a reduced environment
        /// </summary>
        public static Dictionary<string, string> LoadLanguageEntries(string langName, ILanguageDecoder decoder)
        {
            // Get lang
            var lang = decoder.Decode().FirstOrDefault(x => x.Code == langName);

            // If null, throw exception
            if (lang == null)
                throw new Exception("Language not found");

            Dictionary<string, string> entries = new Dictionary<string, string>();

            // Parse lines
            foreach (var line in decoder.GetStream(lang).ReadAllLines())
            {
                string[] split = line.Split('=');
                string key = split[0]; // Get the key
                string value = split[1].Remove(0, 1).Remove(split[1].Length - 2); // Get the value
                entries.Add(key, value); // Add entry
            }

            return entries;
        }

        /// <summary>
        /// Gets a language based on its code.
        /// </summary>
        /// <remarks>
        /// If the code represents a language with Code and Subculture
        /// tries to find the same language, otherwise, its going to search a language with the same Universal culture.
        /// </remarks>
        /// <param name="languageCode">The code to get its language</param>
        public static Language GetLanguage(string languageCode)
        {
            // Split the language
            var split = languageCode.Split('-');
            Language lang = null;

            // If the split results with a length of 1, it means the language code is only Universal culture
            if (split.Length == 1)
            {
                // Get a language with the same Universal Code
                lang = mLanguages.FirstOrDefault(x => x.UniversalCode == split[0]);
            }

            // If the split results with a length of 2, it means the language code contains the Universal and Sub cultures
            else if(split.Length == 2)
            {
                // First try to get the same language
                lang = mLanguages.FirstOrDefault(x => x.Code == languageCode);

                // If not found, get a language with the same Universal culture
                if(lang == null)
                    lang = mLanguages.FirstOrDefault(x => x.UniversalCode == split[0]);
            }

            // If lang is null, throw exception
            if (lang == null)
                throw new LanguageNotFoundException("Cannot found any language with that specific cultures.");

            // Return the language
            return lang;
        }

        /// <summary>
        /// Sets the new language
        /// </summary>
        public static void SetLanguage(Language language)
        {
            // Ignore if the same
            if (mCurrentLanguage == language)
                return;

            // Set language
            mCurrentLanguage = language;

            // Clear old entries
            mEntries.Clear();

            // Check if the language resource exists
            if(!LocalizationEngine.Instance.Decoder.LanguageFileExists(language))
                throw new LanguageNotFoundException("Language file not found.");

            // Parse lines
            foreach (var line in LocalizationEngine.Instance.Decoder.GetStream(language).ReadAllLines())
            {
                string[] split = line.Split('=');
                string key = split[0]; // Get the key
                string value = split[1].Remove(0, 1).Remove(split[1].Length - 2); // Get the value
                mEntries.Add(key, value); // Add entry
            }
        }

        /// <summary>
        /// Adds a new language to the system
        /// </summary>
        /// <param name="language">The language to add</param>
        public static void AddLanguage(Language language)
        {
            // Language duplicated
            if (mLanguages.Contains(language))
                throw new ArgumentException("This language is already added.", nameof(language));

            // Add the language
            mLanguages.Add(language);
        }

        /// <summary>
        /// Gets a value based on a key
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="defaultValue">The default value when key is not found</param>
        public static string GetValue(string key, string defaultValue = null)
        {
            // If the current lang is null...
            if (mCurrentLanguage == null)
                throw new InvalidOperationException("Current language is not set.");

            // If there are no entries...
            if (mEntries.Count <= 0)
                throw new InvalidOperationException("There are no loaded entries.");

            // If the key exists, return its value
            if (mEntries.ContainsKey(key))
                return mEntries[key];

            // Otherwise...
            else
                if (string.IsNullOrWhiteSpace(defaultValue))
                    return key; // Return key
                else
                    return defaultValue; // Return a default value
        }

        /// <summary>
        /// Indicates if an entry exists
        /// </summary>
        public static bool EntryExists(string key)
        {
            if (mCurrentLanguage == null)
                return false;

            if (mEntries.Count <= 0)
                return false;

            if (!mEntries.ContainsKey(key))
                return false;

            return true;
        }

        /// <summary>
        /// Gets all languages
        /// </summary>
        public static Language[] GetLangs() => mLanguages.ToArray();

        #endregion
    }
}
