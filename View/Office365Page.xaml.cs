
using CommunityToolkit.Mvvm.DependencyInjection;
using Kubix.ViewModel;
using Microsoft.UI.Xaml.Controls;

namespace Kubix.View
{
    public sealed partial class Office365Page : Page
    {
        Office365ViewModel ViewModel { get; }

        public Office365Page()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetService<Office365ViewModel>();
        }
    }
}
