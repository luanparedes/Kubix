using CommunityToolkit.Mvvm.ComponentModel;
using Kubix.Services.Interfaces;
using System.Reflection.Metadata;
using System.Xml.Linq;
using Windows.Storage;

namespace Kubix.Services.Classes
{
    public class DataInitial : ObservableObject, IDataInitial
    {
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

        private bool _isDefaultThemeChecked;
        public bool IsDefaultThemeChecked
        {
            get { return _isDefaultThemeChecked; }
            set
            {
                SetProperty(ref _isDefaultThemeChecked, value);
                SaveBool(nameof(IsDefaultThemeChecked), value);
            }
        }

        public DataInitial()
        {
            SetInitialValues();
        }

        private void SetInitialValues()
        {
            IsFirstTimeOpening = RecoverBool(nameof(IsFirstTimeOpening));
            IsDarkThemeChecked = RecoverBool(nameof(IsDarkThemeChecked));
            IsLightThemeChecked = RecoverBool(nameof(IsLightThemeChecked));
            IsDefaultThemeChecked = RecoverBool(nameof(IsDefaultThemeChecked));
        }

        public void SaveBool(string chave, bool valor)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[chave] = valor;
        }

        public bool RecoverBool(string name, bool defaultValue = false)
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            if (localSettings.Values.ContainsKey(name))
            {
                return (bool)localSettings.Values[name];
            }

            return defaultValue;
        }
    }
}
