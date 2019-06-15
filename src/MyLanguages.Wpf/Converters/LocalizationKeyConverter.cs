using MyLanguages.Core.Engine;
using System;
using System.Globalization;
using System.Windows.Data;

namespace MyLanguages.Wpf.Converters
{
    public class LocalizationKeyConverter : IMultiValueConverter
    {
        #region Members

        private string mKey;
        private string mDefaultValue;

        #endregion

        #region Constructors

        public LocalizationKeyConverter(string key, string defaultValue)
        {
            mKey = key;
            mDefaultValue = defaultValue;
        }

        #endregion

        #region Methods

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            => string.IsNullOrWhiteSpace(mKey) == true ? mDefaultValue ?? "Key not found." : LocalizationEngine.Localizator[mKey];

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class SingleLocalizationKeyConverter : IValueConverter
    {
        #region Members

        private string mKey;
        private string mDefaultValue;

        #endregion

        #region Constructors

        public SingleLocalizationKeyConverter(string key, string defaultValue)
        {
            mKey = key;
            mDefaultValue = defaultValue;
        }

        #endregion

        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => string.IsNullOrWhiteSpace(mKey) == true ? mDefaultValue ?? "Key not found." : LocalizationEngine.Localizator[mKey];

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
