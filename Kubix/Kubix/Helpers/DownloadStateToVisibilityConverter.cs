using Kubix.Model;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace Kubix.Helpers
{
    public class DownloadStateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DownloadState state)
            {
                switch (state)
                {
                    case DownloadState.NotStarted:
                        return Visibility.Collapsed;
                    case DownloadState.Downloading:
                        return Visibility.Visible;
                    case DownloadState.Completed:
                        return Visibility.Collapsed;
                    case DownloadState.Paused:
                        return Visibility.Collapsed;
                    case DownloadState.Cancelled:
                        return Visibility.Collapsed;
                    case DownloadState.Failed:
                        return Visibility.Collapsed;
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible ? DownloadState.Downloading : DownloadState.NotStarted;
            }

            return DownloadState.NotStarted;
        }
    }
}
