using CommunityToolkit.Mvvm.DependencyInjection;
using Kubix.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace Kubix.View
{
    public sealed partial class DownloaderPage : Page
    {
        DownloaderViewModel ViewModel { get; }

        public DownloaderPage()
        {
            InitializeComponent();
            ViewModel = Ioc.Default.GetService<DownloaderViewModel>();
            ViewModel.Dispatcher = DispatcherQueue;
        }
    }
}
