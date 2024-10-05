using Microsoft.UI.Xaml.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using KanBoard.ViewModel;

namespace KanBoard.View
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
