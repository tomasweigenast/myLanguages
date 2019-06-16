using MyLanguages.Core.Decoder;
using MyLanguages.Core.Engine;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = string.Format(@"http://translate.google.com/translate_a/t?client=j&text={0}&hl=en&sl={1}&tl={2}",
                               HttpUtility.UrlEncode("hello my dear"), "en", "es");

            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0");
            client.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
            client.Encoding = Encoding.UTF8;
            string result = client.DownloadString(url);

            LocalizationEngine.MakeNew(new PhysicalFileDecoder(Path.Combine(Directory.GetCurrentDirectory(), "langs")));

            LocalizationEngine.Instance.DetectLanguages("fr");

            Console.WriteLine("Installed languages:");
            foreach (var lang in LocalizationEngine.Instance.GetInstalledLanguages())
                Console.WriteLine($"\t- {lang.ToString()}");

            Console.WriteLine($"Current language: {LocalizationEngine.Instance.CurrentLanguage.ToString()}");

            LocalizationEngine.Instance.ChangeLanguage("");

            string value = LocalizationEngine.Localizator["Titles.Principal"];
            Console.WriteLine($"A value: {value}");

            // Keep console opened
            Console.ReadLine();
        }
    }
}
