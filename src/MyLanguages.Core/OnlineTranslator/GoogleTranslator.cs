using System;
using System.Globalization;
using System.Net;
using System.Text;
using System.Web;

namespace MyLanguages.Core.OnlineTranslator
{
    /// <summary>
    /// Uses Google services to translate text
    /// </summary>
    public class GoogleTranslator : IOnlineTranslator
    {
        /// <summary>
        /// Translates a text
        /// </summary>
        /// <param name="text">The text to translate</param>
        /// <param name="from">The source language</param>
        /// <param name="to">The language to translate the text</param>
        public string TranslateText(string text, CultureInfo from, CultureInfo to)
        {
            // Do null checks
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(text));
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (to == null) throw new ArgumentNullException(nameof(to));

            // Get langs ISO names
            string fromLang = from.TwoLetterISOLanguageName;
            string toLang = to.TwoLetterISOLanguageName;

            // Encode request url
            string requestUrl = string.Format(@"http://translate.google.com/translate_a/t?client=j&text={0}&hl=en&sl={1}&tl={2}",
                                HttpUtility.UrlEncode(text), fromLang, toLang);

            //Make web request
            using (WebClient webClient = new WebClient())
            {
                // Setup
                webClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0");
                webClient.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
                webClient.Encoding = Encoding.UTF8;

                // Execute
                return webClient.DownloadString(requestUrl);
            }
        }
    }
}
