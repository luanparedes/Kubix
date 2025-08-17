using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.ComponentModel;
using System.Threading;

namespace Kubix.Model
{
    public partial class DownloadModel : ObservableObject
    {
        [ObservableProperty]
        private string packageName;
        [ObservableProperty]
        private string downloadPath;
        [ObservableProperty]
        private Uri url;
        [ObservableProperty]
        private int percentCompleted = 0;
        [ObservableProperty]
        private double bytesDownloaded = 0;
        [ObservableProperty]
        private double downloadSize = 0;
        [ObservableProperty]
        private string downloadSpeed;
        [ObservableProperty]
        private DownloadState state = DownloadState.NotStarted;
        [ObservableProperty]
        private bool isNotStartedDownload = true;
        [ObservableProperty]
        private bool isDownloading = true;
        [ObservableProperty]
        private string thumbnail;

        [ObservableProperty]
        public AsyncRelayCommand playDownload;
        [ObservableProperty]
        public AsyncRelayCommand pauseDownload;
        [ObservableProperty]
        public AsyncRelayCommand cancelDownload;

        public CancellationTokenSource CancellationTokenSource { get; set; }

        public DownloadModel(string url)
        {
            Url = new Uri(url);
            CancellationTokenSource = new CancellationTokenSource();
        }
    }

    public enum DownloadState
    {
        NotStarted,
        Downloading,
        ContinueDownloading,
        Completed,
        Paused,
        PrePaused,
        Cancelled,
        Failed
    }
}
