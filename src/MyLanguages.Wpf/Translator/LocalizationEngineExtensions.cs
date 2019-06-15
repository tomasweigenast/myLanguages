using MyLanguages.Core.Engine;
using MyLanguages.Wpf.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace MyLanguages.Wpf.Translator
{
    /// <summary>
    /// Extensions methods for the <see cref="LocalizationEngine"/>
    /// to work in WPF
    /// </summary>
    public static class LocalizationEngineExtensions
    {
        public static void DetectTranslatableControls(this LocalizationEngine engine, Application app)
        {
            // Get the type of ITranslatable
            var translatableControls = new List<Type>();

            // Add ITranslatable types
            foreach (var type in Assembly.GetEntryAssembly().GetTypes().Where(x => x.GetInterfaces().Contains(typeof(ITranslatable))))
                if (typeof(Control).IsAssignableFrom(type))
                    translatableControls.Add(type);

            // Translate every control
            foreach(var controlType in translatableControls)
            {
                // If the control type is a Window
                if (controlType.BaseType == typeof(Window))
                {
                    foreach(Window window in app.Windows)
                    {
                        if(window.GetType() == controlType)
                        {

                        }
                    }
                }
            }
        }
    }
}
