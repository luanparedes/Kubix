
using CommunityToolkit.Mvvm.DependencyInjection;
using KanBoard.ViewModel;
using Microsoft.UI.Xaml.Controls;

namespace KanBoard.View
{
    public sealed partial class SocialMediasPage : Page
    {
        SocialMediasViewModel ViewModel { get; }

        public SocialMediasPage()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetService<SocialMediasViewModel>();
        }
    }
}
