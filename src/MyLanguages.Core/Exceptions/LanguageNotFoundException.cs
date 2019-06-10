using System;

namespace MyLanguages.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when is trying to add a language that does not exists
    /// </summary>
    public class LanguageNotFoundException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="LanguageNotFoundException"/> with an error message
        /// </summary>
        /// <param name="message">The error message</param>
        public LanguageNotFoundException(string message) : base(message) { }
    }
}
