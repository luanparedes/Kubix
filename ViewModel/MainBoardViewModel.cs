
using CommunityToolkit.Mvvm.DependencyInjection;
using KanBoard.Services.Interfaces;
using Windows.Security.Cryptography.Core;

namespace KanBoard.ViewModel
{
    public class MainBoardViewModel
    {
        private readonly ILogger _logger = Ioc.Default.GetService<ILogger>();

        public MainBoardViewModel()
        {
            _logger.InfoLog("Entered Constructor ViewModel!");
        }
    }
}
