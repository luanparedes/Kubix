using CommunityToolkit.Mvvm.DependencyInjection;
using KanBoard.ViewModel;
using Microsoft.UI.Xaml.Controls;

namespace KanBoard.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StreamingsPage : Page
    {
        StreamingsViewModel ViewModel { get; }

        public StreamingsPage()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetService<StreamingsViewModel>();
        }
    }
}
