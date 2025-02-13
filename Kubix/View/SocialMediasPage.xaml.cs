
using CommunityToolkit.Mvvm.DependencyInjection;
using Kubix.ViewModel;
using Microsoft.UI.Xaml.Controls;

namespace Kubix.View
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
