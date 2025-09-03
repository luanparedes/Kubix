using CommunityToolkit.Mvvm.ComponentModel;
using Kubix.Helpers;
using Kubix.Services.Interfaces;

namespace Kubix.Services.Classes
{
    public class AppInfo : ObservableObject, IAppInfo
    {
        private string _appName = Stringer.GetString("KB_AppNameText");
        public string AppName
        {
            get { return _appName; }
        }

        private string _appVersion = "v2.7.3";
        public string AppVersion
        {
            get { return _appVersion; }
        }

        public string GetAppFullNameVersion()
        {
            return $"{AppName} {AppVersion}";
        }
    }
}
