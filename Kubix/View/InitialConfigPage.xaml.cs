using CommunityToolkit.Mvvm.DependencyInjection;
using Kubix.ViewModel;
using Microsoft.UI.Xaml.Controls;

namespace Kubix.View
{
    public sealed partial class InitialConfigPage : Page
    {
        InitialConfigViewModel ViewModel { get; }

        public InitialConfigPage()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetService<InitialConfigViewModel>();
        }
    }
}
