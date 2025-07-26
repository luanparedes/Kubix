using Microsoft.UI.Xaml.Controls;
using Kubix.ViewModel;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace Kubix.View
{
    public sealed partial class KNotePage : Page
    {
        KNoteViewModel ViewModel { get; }

        public KNotePage()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetService<KNoteViewModel>();
        }
    }
}
