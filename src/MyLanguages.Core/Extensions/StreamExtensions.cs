using System.Collections.Generic;
using System.IO;

namespace MyLanguages.Core.Extensions
{
    /// <summary>
    /// Extension methods to work with <see cref="StreamReader"/>
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Reads all lines from a <see cref="StreamReader"/>
        /// </summary>
        /// <param name="stream">The stream that contains the lines to read</param>
        public static IEnumerable<string> ReadAllLines(this StreamReader stream)
        {
            if (stream == null)
                yield return "";

            string line;
            while ((line = stream.ReadLine()) != null)
                yield return line;

            stream.Close();
        }
    }
}
