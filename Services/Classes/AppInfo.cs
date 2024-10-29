using Kubix.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kubix.Services.Classes
{
    public class AppInfo : IAppInfo
    {
        public readonly string AppName = "Kubix";
        public readonly string AppVersion = "0.0.5";

        public string GetAppFullNameVersion()
        {
            return $"{AppName} {AppVersion}";
        }
    }
}
