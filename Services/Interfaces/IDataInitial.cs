using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public bool HasWebBrowser { get; set; }
        public bool HasAI { get; set; }
        public bool HasMusic { get; set; }
        public bool HasYoutube { get; set; }
        public bool HasStreaming { get; set; }
        public bool HasSocialMedia { get; set; }
        public bool HasKNote { get; set; }
        public bool HasOffice { get; set; }
        public bool HasGoogle { get; set; }
        public bool HasCompilers { get; set; }

        public event EventHandler UIUpdateChanged;
        public void OnUpdateUI();
    }
}
