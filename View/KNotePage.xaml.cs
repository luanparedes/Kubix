using Microsoft.UI.Xaml.Controls;
using KanBoard.ViewModel;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace KanBoard.View
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
