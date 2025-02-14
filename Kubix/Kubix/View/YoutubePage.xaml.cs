using CommunityToolkit.Mvvm.DependencyInjection;
using Kubix.ViewModel;
using Microsoft.UI.Xaml.Controls;

namespace Kubix.View
{
    public sealed partial class YoutubePage : Page
    {
        YoutubeViewModel ViewModel { get; }

        public YoutubePage()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetService<YoutubeViewModel>();
        }
    }
}
