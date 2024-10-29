using Kubix.ViewModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace Kubix.View
{
    public partial class SettingsPage : Page
    {
        protected SettingsViewModel ViewModel { get; }

        public SettingsPage()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetService<SettingsViewModel>();
        }
    }
}
