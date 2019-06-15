using MyLanguages.Wpf.Converters;
using MyLanguages.Wpf.Helpers;
using MyLanguages.Wpf.Translator;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace MyLanguages.Wpf.Markup
{
    /// <summary>
    /// Provides a custom markup extension to translate
    /// XAML controls
    /// </summary>
    public class LocExtension : MarkupExtension
    {
        #region Public Properties

        /// <summary>
        /// The key provided in XAML
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// A default value to show at design-time or if the key is not found
        /// </summary>
        public string FallbackValue { get; set; }

        /// <summary>
        /// The key provided in XAML as a binding source
        /// </summary>
        public Binding KeySource { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="LocExtension"/> markup extension
        /// and translates the current control using its reference as Key
        /// </summary>
        public LocExtension() { }

        /// <summary>
        /// Creates a new instance of the <see cref="LocExtension"/> markup extension
        /// and translates the current control
        /// </summary>
        /// <param name="key">The translator key to get the translated text</param>
        public LocExtension(string key)
        {
            Key = key;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="LocExtension"/> markup extension
        /// and translates the current control
        /// </summary>
        /// <param name="key">The translator key to get the translated text</param>
        /// <param name="fallbackValue">A default value to show at design-time or if the key is not found</param>
        public LocExtension(string key, string fallbackValue)
        {
            Key = key;
            FallbackValue = fallbackValue;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="LocExtension"/> markup extension
        /// and translates the current control
        /// </summary>
        /// <param name="keySource">The translator key as binding source to get the translated text</param>
        public LocExtension(Binding keySource)
        {
            KeySource = keySource;
        }

        #endregion

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var provideTargetValue = serviceProvider as IProvideValueTarget;
            var targetObject = provideTargetValue?.TargetObject as FrameworkElement;
            var targetProperty = provideTargetValue?.TargetProperty as DependencyProperty;

            // If the key was not provided
            if(string.IsNullOrWhiteSpace(Key))
            {
                // Get the key of the element
                var lastElement = targetObject.GetLastParent();
                var type = lastElement.GetType();

                Key = $"{type.Namespace}.{type.Name}.{targetObject.Name}";
            }

            // Create a multibinding to add to the control
            var binding = new Binding("Text")
            {
                Source = new Translation { Key = KeySource == null ? Key : (string)KeySource.Source },
                Converter = new SingleLocalizationKeyConverter(Key, null)
            };

            // Provide the value
            return binding.ProvideValue(serviceProvider);
        }
    }
}
