using CommunityToolkit.Mvvm.ComponentModel;
using Kubix.Model;
using Kubix.Services.Interfaces;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace Kubix.ViewModel
{
    public partial class DownloaderViewModel : ObservableObject
    {
        private readonly IDownloadService _downloadService;
        public DispatcherQueue Dispatcher { get; set; }

        #region Observable Properties

        [ObservableProperty]
        private ObservableCollection<DownloadModel> downloadItems = new ObservableCollection<DownloadModel>();
        [ObservableProperty]
        private string url;
        [ObservableProperty]
        private string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads\\KubixDownloads");

        #endregion

        #region Constructor

        public DownloaderViewModel(IDownloadService downloadService)
        {
            _downloadService = downloadService;
            _downloadService.DownloadProgressChangedEvent += _downloadService_DownloadProgressChangedEvent;
            _downloadService.ConfigureCommandsChangedEvent += _downloadService_ConfigureCommandsEvent;

            Directory.CreateDirectory(DownloadPath);
        }

        #endregion

        #region Methods

        private Task DownloadProduct()
        {
            DownloadModel model = new DownloadModel(Url)
            {
                Url = new Uri(Url),
                DownloadPath = this.DownloadPath
            };

            DownloadItems.Add(model);

            Url = string.Empty;

            if (IsYoutubeVideo(model))
            {
                Task.Run(async () =>
                {
                    await _downloadService.DownloadYoutubeAsync(model);
                });
            }
            else
            {
                Task.Run(async () =>
                {
                    await _downloadService.DownloadTorrentAsync(model);
                });
            }

            return Task.CompletedTask;
        }

        private bool IsYoutubeVideo(DownloadModel model)
        {
            return model.Url.AbsoluteUri.Contains("youtube");
        }

        #endregion

        #region Event Handlers

        private void _downloadService_ConfigureCommandsEvent(object sender, Tuple<DownloadModel, DownloadModel> downloadTuple)
        {
            Dispatcher.TryEnqueue(() =>
            {
                downloadTuple.Item1.PlayDownload = downloadTuple.Item2.PlayDownload;
                downloadTuple.Item1.PauseDownload = downloadTuple.Item2.PauseDownload;
                downloadTuple.Item1.CancelDownload = downloadTuple.Item2.CancelDownload;
            });
        }

        private void _downloadService_DownloadProgressChangedEvent(object sender, Tuple<DownloadModel, DownloadModel> downloadTuple)
        {
            Dispatcher.TryEnqueue(() =>
            {
                if (string.IsNullOrEmpty(downloadTuple.Item1.PackageName))
                {
                    downloadTuple.Item1.PackageName = downloadTuple.Item2.PackageName;
                    downloadTuple.Item1.Thumbnail = downloadTuple.Item2.Thumbnail;
                }

                downloadTuple.Item1.PercentCompleted = downloadTuple.Item2.PercentCompleted;
                downloadTuple.Item1.BytesDownloaded = downloadTuple.Item2.BytesDownloaded;
                downloadTuple.Item1.DownloadSize = downloadTuple.Item2.DownloadSize;
                downloadTuple.Item1.DownloadSpeed = downloadTuple.Item2.DownloadSpeed;
                downloadTuple.Item1.State = downloadTuple.Item2.State;
                downloadTuple.Item1.IsDownloading = downloadTuple.Item2.IsDownloading;

                if (downloadTuple.Item1.BytesDownloaded > 0)
                    downloadTuple.Item1.IsNotStartedDownload = false;
            });
        }

        public void ButtonDownload_Click(object sender, RoutedEventArgs e)
        {
            DownloadProduct();
        }

        public async void ButtonFolderPicker_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker folderPicker = new FolderPicker
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.Downloads,
                FileTypeFilter = {"*"}
            };

            var hwnd = WindowNative.GetWindowHandle(App.Instance.MainWindow);
            InitializeWithWindow.Initialize(folderPicker, hwnd);

            DownloadPath = (await folderPicker.PickSingleFolderAsync()).Path;
        }

        #endregion
    }
}
