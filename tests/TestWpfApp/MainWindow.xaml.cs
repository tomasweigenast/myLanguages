using MyLanguages.Core.Engine;
using MyLanguages.Core.Localization;
using System;
using System.Linq;
using System.Windows;

namespace TestWpfApp
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LocalizationEngine.Instance.ChangeLanguage(GetRandomLang().Code);
        }

        private Language GetRandomLang()
        {
            var installedLangs = LocalizationEngine.Instance.GetInstalledLanguages().ToArray();
            Language randomLang = installedLangs[new Random().Next(0, installedLangs.Length)];
            if (randomLang == LocalizationEngine.Instance.CurrentLanguage)
                return GetRandomLang();

            return randomLang;
        }
    }
}
