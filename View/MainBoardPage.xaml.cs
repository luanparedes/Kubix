using CommunityToolkit.Mvvm.DependencyInjection;
using KanBoard.ViewModel;
using Microsoft.UI.Xaml;

namespace KanBoard
{
    public sealed partial class MainWindow : Window
    {
        public MainBoardViewModel ViewModel { get; }

        public MainWindow()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetService<MainBoardViewModel>();
        }
    }
}
