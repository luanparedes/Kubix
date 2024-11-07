using CommunityToolkit.Mvvm.ComponentModel;
using Kubix.Helpers;
using Kubix.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kubix.Services.Classes
{
    public class AppInfo : ObservableObject, IAppInfo
    {
        private string _appName = Stringer.GetString("KB_AppNameText");
        public string AppName
        {
            get { return _appName; }
        }

        private string _appVersion = "2.1.1.0";
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
