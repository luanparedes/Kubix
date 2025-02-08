using CommunityToolkit.Mvvm.DependencyInjection;
using Kubix.ViewModel;
using Microsoft.UI.Xaml.Controls;

namespace Kubix.View
{
    public sealed partial class AIPage : Page
    {
        AIViewModel ViewModel { get; }

        public AIPage()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetService<AIViewModel>();
        }
    }
}
