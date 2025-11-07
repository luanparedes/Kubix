
using Kubix.Services.Interfaces;

namespace Kubix.ViewModel
{
    public class KDiffViewModel
    {
        #region Fields & Properties
        private readonly ILogger _logger;
        #endregion

        #region Constructor
        public KDiffViewModel(ILogger logger)
        {
            _logger = logger;
            _logger.InfoLog("KDiffViewModel initialized.");
        }
        #endregion
    }
}
