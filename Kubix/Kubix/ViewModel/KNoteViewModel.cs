using CommunityToolkit.Mvvm.ComponentModel;
using Kubix.Services.Interfaces;

namespace Kubix.ViewModel
{
    public class KNoteViewModel : ObservableObject
    {
        #region Fields & Properties
        private readonly ILogger _logger;
        #endregion

        #region Constructor

        public KNoteViewModel(ILogger logger)
        {
            _logger = logger;
            _logger.InfoLog("KNoteViewModel initialized.");
        }

        #endregion
    }
}
