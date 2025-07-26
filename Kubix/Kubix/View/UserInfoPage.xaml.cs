using Microsoft.UI.Xaml.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using Kubix.ViewModel;

namespace Kubix.View
{
    public sealed partial class UserInfoPage : Page
    {
        public UserInfoViewModel ViewModel { get; }

        public UserInfoPage()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetService<UserInfoViewModel>();
        }
    }
}
