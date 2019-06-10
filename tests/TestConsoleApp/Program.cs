using MyLanguages.Core.Engine;
using System;
using System.IO;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            LocalizationEngine.MakeNew();

            LocalizationEngine.Instance.DetectLanguages();


            string value = LocalizationEngine.Localizator["Titles.awdawd"];
            LocalizationEngine.Instance.GetInstalledLanguages();
            Console.ReadLine();
        }
    }
}
