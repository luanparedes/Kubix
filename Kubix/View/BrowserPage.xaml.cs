using Kubix.ViewModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace Kubix.View
{
    public sealed partial class BrowserPage : Page
    {
        BrowserViewModel ViewModel { get; }

        public BrowserPage()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetService<BrowserViewModel>();
        }
    }
}
