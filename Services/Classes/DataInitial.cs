using CommunityToolkit.Mvvm.ComponentModel;
using Kubix.Services.Interfaces;
using System;
using System.ComponentModel;
using Windows.Storage;

namespace Kubix.Services.Classes
{
    public class DataInitial : ObservableObject, INotifyPropertyChanged, IDataInitial
    {
        #region Fields & Properties

        public event EventHandler UIUpdateChanged;

        private bool _isFirstTimeOpening = true;
        public bool IsFirstTimeOpening
        {
            get { return _isFirstTimeOpening; }
            set
            {
                SetProperty(ref _isFirstTimeOpening, value);
                SaveBool(nameof(IsFirstTimeOpening), value);
            }
        }

        private bool _isDarkThemeChecked;
        public bool IsDarkThemeChecked
        {
            get { return _isDarkThemeChecked; }
            set
            {
                SetProperty(ref _isDarkThemeChecked, value);
                SaveBool(nameof(IsDarkThemeChecked), value);
            }
        }

        private bool _isLightThemeChecked;
        public bool IsLightThemeChecked
        {
            get { return _isLightThemeChecked; }
            set
            {
                SetProperty(ref _isLightThemeChecked, value);
                SaveBool(nameof(IsLightThemeChecked), value);
            }
        }

        private bool _isDefaultThemeChecked = true;
        public bool IsDefaultThemeChecked
        {
            get { return _isDefaultThemeChecked; }
            set
            {
                SetProperty(ref _isDefaultThemeChecked, value);
                SaveBool(nameof(IsDefaultThemeChecked), value);
            }
        }

        private bool _hasWebBrowser = true;
        public bool HasWebBrowser
        {
            get { return _hasWebBrowser; }
            set
            {
                SetProperty(ref _hasWebBrowser, value);
                SaveBool(nameof(HasWebBrowser), value);
            }
        }

        private bool _hasAI = true;
        public bool HasAI
        {
            get { return _hasAI; }
            set
            {
                SetProperty(ref _hasAI, value);
                SaveBool(nameof(HasAI), value);
            }
        }

        private bool _hasMusic = true;
        public bool HasMusic
        {
            get { return _hasMusic; }
            set
            {
                SetProperty(ref _hasMusic, value);
                SaveBool(nameof(HasMusic), value);
            }
        }

        private bool _hasYoutube = true;
        public bool HasYoutube
        {
            get { return _hasYoutube; }
            set
            {
                SetProperty(ref _hasYoutube, value);
                SaveBool(nameof(HasYoutube), value);
            }
        }

        private bool _hasStreaming = true;
        public bool HasStreaming
        {
            get { return _hasStreaming; }
            set
            {
                SetProperty(ref _hasStreaming, value);
                SaveBool(nameof(HasStreaming), value);
            }
        }

        private bool _hasSocialMedia = true;
        public bool HasSocialMedia
        {
            get { return _hasSocialMedia; }
            set
            {
                SetProperty(ref _hasSocialMedia, value);
                SaveBool(nameof(HasSocialMedia), value);
            }
        }

        private bool _hasKNote = true;
        public bool HasKNote
        {
            get { return _hasKNote; }
            set
            {
                SetProperty(ref _hasKNote, value);
                SaveBool(nameof(HasKNote), value);
            }
        }

        private bool _hasOffice = true;
        public bool HasOffice
        {
            get { return _hasOffice; }
            set
            {
                SetProperty(ref _hasOffice, value);
                SaveBool(nameof(HasOffice), value);
            }
        }

        private bool _hasGoogle = true;
        public bool HasGoogle
        {
            get { return _hasGoogle; }
            set
            {
                SetProperty(ref _hasGoogle, value);
                SaveBool(nameof(HasGoogle), value);
            }
        }

        private bool _hasCompilers = true;
        public bool HasCompilers
        {
            get { return _hasCompilers; }
            set
            {
                SetProperty(ref _hasCompilers, value);
                SaveBool(nameof(HasCompilers), value);
            }
        }

        private bool _hasTerminal = true;
        public bool HasTerminal
        {
            get { return _hasTerminal; }
            set
            {
                SetProperty(ref _hasTerminal, value);
                SaveBool(nameof(HasTerminal), value);
            }
        }

        private bool _hasKDiff = true;
        public bool HasKDiff
        {
            get { return _hasKDiff; }
            set
            {
                SetProperty(ref _hasKDiff, value);
                SaveBool(nameof(HasKDiff), value);
            }
        }

        #endregion

        #region Constructor

        public DataInitial()
        {
            SetInitialValues();
        }

        #endregion

        #region Methods

        public void SaveBool(string chave, bool valor)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[chave] = valor;
        }

        public bool RecoverBool(string name, bool defaultValue = true)
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            if (localSettings.Values.ContainsKey(name))
            {
                return (bool)localSettings.Values[name];
            }

            return defaultValue;
        }

        private void SetInitialValues()
        {
            IsFirstTimeOpening = RecoverBool(nameof(IsFirstTimeOpening));
            IsDarkThemeChecked = RecoverBool(nameof(IsDarkThemeChecked));
            IsLightThemeChecked = RecoverBool(nameof(IsLightThemeChecked));
            IsDefaultThemeChecked = RecoverBool(nameof(IsDefaultThemeChecked));
            HasWebBrowser = RecoverBool(nameof(HasWebBrowser));
            HasAI = RecoverBool(nameof(HasAI));
            HasMusic = RecoverBool(nameof(HasMusic));
            HasYoutube = RecoverBool(nameof(HasYoutube));
            HasStreaming = RecoverBool(nameof(HasStreaming));
            HasSocialMedia = RecoverBool(nameof(HasSocialMedia));
            HasKNote = RecoverBool(nameof(HasKNote));
            HasOffice = RecoverBool(nameof(HasOffice));
            HasGoogle = RecoverBool(nameof(HasGoogle));
            HasCompilers = RecoverBool(nameof(HasCompilers));
            HasTerminal = RecoverBool(nameof(HasTerminal));
            HasKDiff = RecoverBool(nameof(HasKDiff));
        }

        #endregion

        #region Events

        public void OnUpdateUI()
        {
            UIUpdateChanged?.Invoke(this, null);
        }

        #endregion
    }
}
