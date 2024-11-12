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
using Kubix.ViewModel;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace Kubix.View
{
    public sealed partial class GooglePage : Page
    {
        GoogleViewModel ViewModel { get; }

        public GooglePage()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetService<GoogleViewModel>();
        }
    }
}
