using Kubix.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kubix.ViewModel
{
    public class HomeViewModel
    {
        #region Fields & Properties
        private readonly ILogger _logger;
        #endregion

        #region Constructor
        public HomeViewModel(ILogger logger)
        {
            _logger = logger;
            _logger.InfoLog("HomeViewModel initialized.");
        }
        #endregion
    }
}
