using Microsoft.UI.Xaml.Controls;
using KanBoard.ViewModel;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace KanBoard.View
{
    public sealed partial class NotepadPage : Page
    {
        NotepadViewModel ViewModel { get; }

        public NotepadPage()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetService<NotepadViewModel>();
        }
    }
}
