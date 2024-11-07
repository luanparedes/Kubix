
using CommunityToolkit.Mvvm.DependencyInjection;
using Kubix.ViewModel;
using Microsoft.UI.Xaml.Controls;

namespace Kubix.View
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
