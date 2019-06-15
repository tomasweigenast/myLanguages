using MyLanguages.Core.Engine;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyLanguages.Wpf.Translator
{
    /// <summary>
    /// Provides a model that is passed into the XAML code to change the translations dinamically
    /// </summary>
    public class Translation : INotifyPropertyChanged
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Translation()
        {
            LocalizationEngine.Instance.LanguageChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(Text));
            };
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The text to show
        /// </summary>
        public string Text => LocalizationEngine.Localizator[Key];

        /// <summary>
        /// The key to use to localize an string
        /// </summary>
        public string Key { get; set; }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion
    }
}
