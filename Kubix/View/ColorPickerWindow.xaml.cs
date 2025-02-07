
using CommunityToolkit.Mvvm.DependencyInjection;
using Kubix.ViewModel;
using Microsoft.UI.Xaml;

namespace Kubix.View
{
    public sealed partial class ColorPickerWindow : Window
    {
        public ColorPickerViewModel ViewModel { get; set; }

        public ColorPickerWindow()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetService<ColorPickerViewModel>();

            this.Activated += ViewModel.ColorPickerWindow_Activated;
            this.Closed += ViewModel.ColorPickerWindow_Closed;
        }
    }
}
