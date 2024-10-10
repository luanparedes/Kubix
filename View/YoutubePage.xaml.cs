using CommunityToolkit.Mvvm.DependencyInjection;
using KanBoard.ViewModel;
using Microsoft.UI.Xaml.Controls;

namespace KanBoard.View
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
