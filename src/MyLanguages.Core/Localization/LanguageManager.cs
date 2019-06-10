using MyLanguages.Core.Exceptions;
using MyLanguages.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

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
        public static void LoadFromAssemblyDetection()
        {
            // Get languages resource
            var resources = Assembly.GetEntryAssembly().GetManifestResourceNames().Where(x => x.EndsWith(".lang")).ToArray();

            foreach (var resource in resources)
            {
                // Get language name
                var resourceSplit = resource.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                var langName = resourceSplit[resourceSplit.Length - 2];

                // Add the language
                AddLanguage(langName, resource);
            }
        }

        /// <summary>
        /// Adds a new language
        /// </summary>
        /// <param name="languageCode">The code of the language</param>
        /// <param name="resource">The resource name of the language file</param>
        public static bool AddLanguage(string languageCode, string resource)
        {
            // Duplicated language
            if (mLanguages.Any(x => x.Code == languageCode))
                return false;

            // Create language
            Language lang = new Language(languageCode, resource);

            // Add language
            mLanguages.Add(lang);
            return true;
        }

        /// <summary>
        /// Returns the current language of the application
        /// </summary>
        public static Language GetCurrentLanguage() => mCurrentLanguage;

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

            // Get entry assembly
            Assembly entry = Assembly.GetEntryAssembly();

            // Check if the language resource exists
            if (!entry.GetManifestResourceNames().Any(x => x == language.Location))
                throw new LanguageNotFoundException("Language file not found.");

            // Parse lines
            foreach (var line in entry.GetManifestResourceStream(language.Location).ReadAllLines())
            {
                string[] split = line.Split('=');
                string key = split[0]; // Get the key
                string value = split[1].Remove(0, 1).Remove(split[1].Length - 2); // Get the value
                mEntries.Add(key, value); // Add entry
            }
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
        /// Gets all languages
        /// </summary>
        public static IReadOnlyList<Language> GetLangs() => new ReadOnlyCollection<Language>(mLanguages);

        #endregion
    }
}
