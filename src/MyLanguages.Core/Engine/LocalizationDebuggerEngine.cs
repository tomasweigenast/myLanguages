using MyLanguages.Core.Decoder;
using MyLanguages.Core.Extensions;
using MyLanguages.Core.Localization;
using System.Collections.Generic;
using System.Linq;

namespace MyLanguages.Core.Engine
{
    /// <summary>
    /// Represents a localization engine used to debug
    /// </summary>
    public class LocalizationDebuggerEngine
    {
        #region Private Members

        private ILanguageDecoder mDecoder; // The language decoder
        private Dictionary<Language, Dictionary<string, string>> mLangs;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="decoder">The decoder used to get languages</param>
        public LocalizationDebuggerEngine(ILanguageDecoder decoder)
        {
            mDecoder = decoder;
            mLangs = new Dictionary<Language, Dictionary<string, string>>();

            // Load languages
            foreach(var lang in mDecoder.Decode())
            {
                mLangs.Add(lang, new Dictionary<string, string>());

                // Parse lines
                foreach (var line in decoder.GetStream(lang).ReadAllLines())
                {
                    string[] split = line.Split('=');
                    string key = split[0]; // Get the key
                    string value = split[1].Remove(0, 1).Remove(split[1].Length - 2); // Get the value
                    mLangs[lang].Add(key, value); // Add entries
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a string value
        /// </summary>
        public string GetValue(string language, string key)
        {
            var lang = mLangs.FirstOrDefault(x => x.Key.Code == language);

            if (!mLangs.ContainsKey(lang.Key))
                return "Language not found.";

            if (!mLangs[lang.Key].ContainsKey(key))
                return "Key not found.";

            return mLangs[lang.Key][key];
        }

        #endregion
    }
}
