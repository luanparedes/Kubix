using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Kubix.Services.Interfaces;
using Microsoft.UI.Xaml;

namespace Kubix.ViewModel
{
    public class UserInfoViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService = Ioc.Default.GetService<INavigationService>();

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.BackPrevious();
        }
    }
}
