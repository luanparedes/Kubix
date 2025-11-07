using CommunityToolkit.Mvvm.ComponentModel;
using Kubix.Services.Interfaces;

namespace Kubix.ViewModel
{
    public partial class TerminalViewModel : ObservableObject
    {
        #region Fields & Properties
        private readonly ILogger _logger;
        #endregion

        #region Constructor
        public TerminalViewModel(ILogger logger)
        {
            _logger = logger;
            _logger.InfoLog("TerminalViewModel initialized.");
        }
        #endregion
    }
}
