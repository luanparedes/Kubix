
using CommunityToolkit.Mvvm.DependencyInjection;
using KanBoard.ViewModel;
using Microsoft.UI.Xaml.Controls;

namespace KanBoard.View
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
