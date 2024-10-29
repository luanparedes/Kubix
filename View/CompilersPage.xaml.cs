
using CommunityToolkit.Mvvm.DependencyInjection;
using KanBoard.ViewModel;
using Microsoft.UI.Xaml.Controls;

namespace KanBoard.View
{
    public sealed partial class CompilersPage : Page
    {
        CompilersViewModel ViewModel { get; }

        public CompilersPage()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetService<CompilersViewModel>();
        }
    }
}
