using MyLanguages.Core.Engine;
using MyLanguages.Core.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MyLanguages.Core.Decoder
{
    /// <summary>
    /// Decodes languages from a physical directory in the machine
    /// </summary>
    public class PhysicalFileDecoder : ILanguageDecoder
    {
        #region Public Properties

        /// <summary>
        /// The directory where the decoder will work
        /// </summary>
        public string Path { get; }

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
                Path = path;
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
            string[] langFiles = Directory.GetFiles(Path, $"*{LocalizationEngine.LANG_FILE_EXTENSION}", SearchOption.TopDirectoryOnly);

            // List to store langs
            IList<Language> langs = new List<Language>();

            // Foreach file...
            foreach(string langFile in langFiles)
            {
                FileInfo fi = new FileInfo(langFile);

                // Get language name
                var langName = System.IO.Path.GetFileNameWithoutExtension(fi.Name); 

                // Add lang
                langs.Add(new Language(langName, langFile));
            }

            // Return langs
            return langs.ToArray();
        }

        /// <summary>
        /// Returns a boolean indicating if the language file exists
        /// </summary>
        /// <param name="language">The language to get its file</param>
        public bool LanguageFileExists(Language language)
            => Directory.GetFiles(Path, $"{language.Code}{LocalizationEngine.LANG_FILE_EXTENSION}").Any();

        /// <summary>
        /// Returns the corresponding stream for the decoder
        /// </summary>
        public StreamReader GetStream(Language language)
            => new StreamReader(language.Location, Encoding.UTF8);

        #endregion
    }
}
