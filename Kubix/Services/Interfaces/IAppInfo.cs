using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kubix.Services.Interfaces
{
    public interface IAppInfo
    {
        public string AppName { get; }
        public string AppVersion { get; }

        string GetAppFullNameVersion();
    }
}
