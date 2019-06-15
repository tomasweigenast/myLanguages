using MyLanguages.Core.Decoder;
using MyLanguages.Core.Engine;
using System;
using System.Globalization;
using System.IO;
using System.Windows;

namespace TestWpfApp
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Make a new localization engine
            LocalizationEngine.MakeNew(new PhysicalFileDecoder(Path.Combine(Directory.GetCurrentDirectory(), "langs")));
            int foundLangs = LocalizationEngine.Instance.DetectLanguages(); // Detect the languages
            LocalizationEngine.Instance.AddOnlineLanguage(LocalizationEngine.Instance.GetLanguage("en-US"), CultureInfo.GetCultureInfo("de"));

            Console.WriteLine($"Loaded languages: {foundLangs} [{string.Join(",", LocalizationEngine.Instance.GetInstalledLanguages())}]");
        }
    }
}
