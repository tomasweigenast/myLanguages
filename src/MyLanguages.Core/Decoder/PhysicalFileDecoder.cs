using MyLanguages.Core.Engine;
using MyLanguages.Core.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyLanguages.Core.Decoder
{
    /// <summary>
    /// Decodes languages from a physical directory in the machine
    /// </summary>
    public class PhysicalFileDecoder : ILanguageDecoder
    {
        #region Private Members

        private string mPath; // The directory path

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="PhysicalFileDecoder"/>
        /// </summary>
        /// <param name="path">The directory where to look for language files</param>
        public PhysicalFileDecoder(string path)
        {
            try
            {
                var dirInfo = new DirectoryInfo(path);

                // Throw exception if the path is not a directory
                if (!dirInfo.Attributes.HasFlag(FileAttributes.Directory))
                    throw new ArgumentException("The path provided does not correspond to a directory location.", nameof(path));

                // Throw exception if the directory does not exists
                if(!dirInfo.Exists)
                    throw new DirectoryNotFoundException($"Directory at '{path}' not found.");

                // Set path
                mPath = path;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Decodes the languages
        /// </summary>
        public Language[] Decode()
        {
            // Get files from the directory
            string[] langFiles = Directory.GetFiles(mPath, $"*.{LocalizationEngine.LANG_FILE_EXTENSION}", SearchOption.TopDirectoryOnly);

            // List to store langs
            IList<Language> langs = new List<Language>();

            // Foreach file...
            foreach(string langFile in langFiles)
            {
                FileInfo fi = new FileInfo(langFile);

                // Get language name
                var langName = Path.GetFileNameWithoutExtension(fi.Name); 

                // Add lang
                langs.Add(new Language(langName, langFile));
            }

            // Return langs
            return langs.ToArray();
        }

        #endregion
    }
}
