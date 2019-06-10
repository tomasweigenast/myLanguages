using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyLanguages.Core.Extensions
{
    /// <summary>
    /// Extension methods to work with <see cref="Stream"/>
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Reads all lines from a <see cref="Stream"/>
        /// </summary>
        /// <param name="stream">The stream that contains the lines to read</param>
        public static IEnumerable<string> ReadAllLines(this Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.Unicode))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    yield return line;
            }

            stream.Close();
        }
    }
}
