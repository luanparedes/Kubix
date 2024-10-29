﻿
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Kubix.Helpers;
using Kubix.Services.Interfaces;
using Kubix.View;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Kubix.ViewModel
{
    public class MainBoardViewModel : ObservableObject
    {
        #region Fields & Properties

        private readonly INavigationService _navigationService = Ioc.Default.GetService<INavigationService>();
        private readonly ILogger _logger = Ioc.Default.GetService<ILogger>();

        #endregion

        #region Contructor

        public MainBoardViewModel()
        {
            _logger.InfoLog("Entered Constructor ViewModel!");
        }

        #endregion

        #region Event Handlers

        public void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            _navigationService.SetFrame((Frame)(sender as NavigationView).Content, FrameTypeEnum.NavigationViewFrame);
            _navigationService.GoToNavigationView(typeof(BrowserPage));
        }

        public void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            Frame frame = (sender.Content) as Frame;

            var selectedItem = args.SelectedItem as NavigationViewItem;
            var tag = (string)selectedItem.Tag;

            switch(tag)
            {
                case "SettingsPage":
                    _navigationService.GoToNavigationView(typeof(SettingsPage));
                    break;
                case "UserInfoPage":
                    _navigationService.GoToNavigationView(typeof(UserInfoPage));
                    break;
                case "AppMusicPage":
                    _navigationService.GoToNavigationView(typeof(AppMusicPage));
                    break;
                case "BrowserPage":
                    _navigationService.GoToNavigationView(typeof(BrowserPage));
                    break;
                case "YoutubePage":
                    _navigationService.GoToNavigationView(typeof(YoutubePage));
                    break;
                case "StreamingsPage":
                    _navigationService.GoToNavigationView(typeof(StreamingsPage));
                    break;
                case "NotepadPage":
                    _navigationService.GoToNavigationView(typeof(KNotePage));
                    break;
                case "AIPage":
                    _navigationService.GoToNavigationView(typeof(AIPage));
                    break;
                case "Office365Page":
                    _navigationService.GoToNavigationView(typeof(Office365Page));
                    break;
                case "GooglePage":
                    _navigationService.GoToNavigationView(typeof(GooglePage));
                    break;
                case "SocialMediaPage":
                    _navigationService.GoToNavigationView(typeof(SocialMediasPage));
                    break;
                case "CompilersPage":
                    _navigationService.GoToNavigationView(typeof(CompilersPage));
                    break;

            }
        }

        #endregion
    }
}

