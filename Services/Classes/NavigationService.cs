using KanBoard.Helpers;
using KanBoard.Services.Interfaces;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace KanBoard.Services.Classes
{
    public class NavigationService : INavigationService
    {
        private Frame _frame;
        private Frame _navigationViewFrame;

        public bool CanGoBack => _frame?.CanGoBack ?? false;

        public bool CanGoForward => _frame?.CanGoForward ?? false;

        public void SetFrame(Frame frame, FrameTypeEnum frameType)
        {
            switch (frameType)
            {
                case FrameTypeEnum.MainFrame:
                    _frame = frame;
                    break;
                case FrameTypeEnum.NavigationViewFrame:
                    _navigationViewFrame = frame;
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
                Debug.WriteLine($"Error on navigating: {ex.Message}");
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
                Debug.WriteLine($"Error on navigating view: {ex.Message}");
            }
        }
    }
}
