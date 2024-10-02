using CommunityToolkit.Mvvm.DependencyInjection;
using KanBoard.Services.Classes;
using KanBoard.Services.Interfaces;
using KanBoard.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace KanBoard
{
    public partial class App : Application
    {
        #region Fields & Properties

        private static App _instance;
        public static Window MainWindow { get; private set; }

        #endregion

        #region Constructor

        public App()
        {
            this.InitializeComponent();
            ConfigureServices();

            _instance = this;
        }

        public static App Instance => _instance;

        #endregion

        #region Methods

        private void ConfigureServices()
        {
            var services = new ServiceCollection();

            services
                .AddSingleton<ILogger, LogService>()
                .AddSingleton<IThemeService, ThemeService>()
                .AddSingleton<MainBoardViewModel>();

            Ioc.Default.ConfigureServices(services.BuildServiceProvider());
        }

        public void ChangeTheme(ElementTheme theme)
        {

            if (MainWindow.Content is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = theme;
            }
        }

        #endregion

        #region Event Handlers

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            MainWindow = new MainWindow();
            MainWindow.Activate();
        }

        #endregion
    }
}
