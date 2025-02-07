using Kubix.Helpers;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kubix.Services.Interfaces
{
    interface INavigationService
    {
        bool CanGoBack { get; }
        bool CanGoForward { get; }

        void BackPrevious();
        void BackToBoard();
        void GoForward();
        void GoToPage(Type sourcePageType, object parameter = null);
        void GoToNavigationView(Type sourcePageType, object parameter = null);
        void SetFrame(Frame frame, FrameTypeEnum frameType);
    }
}
