using MyLanguages.Core.Localization;

namespace MyLanguages.Core.Engine
{
    /// <summary>
    /// Used to retrieve localization values
    /// </summary>
    public class Localize
    {
        /// <summary>
        /// Gets a value from a key
        /// </summary>
        /// <param name="key">The key to get its value</param>
        /// <param name="defaultValue">A default value when key is not found</param>
        public string this[string key, string defaultValue = null] => LanguageManager.GetValue(key, defaultValue);
    }
}
