using KanBoard.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanBoard.Services.Classes
{
    public class AppInfo : IAppInfo
    {
        public readonly string AppName = "KanBoard";
        public readonly string AppVersion = "0.0.5";

        public string GetAppFullNameVersion()
        {
            return $"{AppName} {AppVersion}";
        }
    }
}
