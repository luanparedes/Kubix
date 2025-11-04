using Kubix.Helpers;
using Kubix.Services.Interfaces;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace Kubix.Services.Classes
{
    public class NavigationService : INavigationService
    {
        private Frame _frame;
        private Frame _navigationViewFrame;
        private readonly ILogger _logger;

        public bool CanGoBack => _frame?.CanGoBack ?? false;

        public bool CanGoForward => _frame?.CanGoForward ?? false;

        public NavigationService(ILogger logger)
        {
            _logger = logger;
        }

        public void SetFrame(Frame frame, FrameTypeEnum frameType)
        {
            switch (frameType)
            {
                case FrameTypeEnum.MainFrame:
                    _frame = frame;
                    _logger.InfoLog($"Main frame set for navigation service {frame.Name}.");
                    break;
                case FrameTypeEnum.NavigationViewFrame:
                    _navigationViewFrame = frame;
                    _logger.InfoLog($"Navigation view frame set for navigation service {frame.Name}.");
                    break;
            }
        }

        public void BackPrevious()
        {
            if (CanGoBack)
            {
                _frame.GoBack();
            }
        }

        public void BackToBoard()
        {
            GoToPage(typeof(MainBoardPage));
        }

        public void GoForward()
        {
            if (CanGoForward)
            {
                _frame.GoForward();
            }
        }

        public void GoToPage(Type sourcePageType, object parameter = null)
        {

            try
            {
                _frame.Navigate(sourcePageType, parameter);
            }
            catch (NullReferenceException ex)
            {
                _logger.ErrorLog($"Error on navigating: {ex.Message}");
            }
        }

        public void GoToNavigationView(Type sourcePageType, object parameter = null)
        {
            try
            {
                _navigationViewFrame.Navigate(sourcePageType, parameter);
            }
            catch (NullReferenceException ex)
            {
                _logger.ErrorLog($"Error on navigating navigation view: {ex.Message}");
            }
        }
    }
}
