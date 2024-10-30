using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kubix.Services.Interfaces
{
    public interface IDataInitial
    {
        public bool IsFirstTimeOpening {  get; set; }
        public bool IsDarkThemeChecked { get; set; }
        public bool IsLightThemeChecked { get; set; }
        public bool IsDefaultThemeChecked { get; set; }
    }
}
