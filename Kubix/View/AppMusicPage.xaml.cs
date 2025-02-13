using Microsoft.UI.Xaml.Controls;
using Kubix.ViewModel;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace Kubix.View
{
    public sealed partial class AppMusicPage : Page
    {
        AppMusicViewModel ViewModel { get; }

        public AppMusicPage()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetService<AppMusicViewModel>();
        }
    }
}
