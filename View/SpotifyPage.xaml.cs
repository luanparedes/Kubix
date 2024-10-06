using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using KanBoard.ViewModel;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace KanBoard.View
{
    public sealed partial class SpotifyPage : Page
    {
        SpotifyViewModel ViewModel { get; }
        public SpotifyPage()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetService<SpotifyViewModel>();
        }
    }
}
