using CommunityToolkit.Mvvm.ComponentModel;
using Kubix.Services.Interfaces;

namespace Kubix.ViewModel
{
    public class BrowserViewModel : ObservableObject
    {
        #region Fields & Properties
        private readonly ILogger _logger;
        #endregion

        #region Constructor
        public BrowserViewModel(ILogger logger)
        {
            _logger = logger;
            _logger.InfoLog("BrowserViewModel initialized.");
        }
        #endregion
    }
}
