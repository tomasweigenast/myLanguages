using MyLanguages.Core.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MyLanguages.Core.Decoder
{
    /// <summary>
    /// Decodes languages from embbeded files in the assembly
    /// </summary>
    public class EmbbededLanguageDecoder : ILanguageDecoder
    {
        /// <summary>
        /// Decodes the languages
        /// </summary>
        public Language[] Decode()
        {
            // Get languages resource
            var resources = Assembly.GetEntryAssembly().GetManifestResourceNames().Where(x => x.EndsWith(".lang")).ToArray();

            // Create list to store languages
            IList<Language> langs = new List<Language>();

            foreach (var resource in resources)
            {
                // Get language name
                var resourceSplit = resource.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                var langName = resourceSplit[resourceSplit.Length - 2];

                // Add lang
                langs.Add(new Language(langName, resource));
            }

            // Return all the languages
            return langs.ToArray();
        }
    }
}
