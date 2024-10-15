
using Microsoft.UI.Xaml.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using KanBoard.ViewModel;

namespace KanBoard.View
{
    public sealed partial class GoogleMapsPage : Page
    {
        GoogleMapsViewModel ViewModel { get; }

        public GoogleMapsPage()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetService<GoogleMapsViewModel>();
        }
    }
}
