using AngleSharp.Io;
using CommunityToolkit.Mvvm.Input;
using Kubix.Model;
using Kubix.Services.Interfaces;
using Microsoft.VisualBasic;
using MonoTorrent;
using MonoTorrent.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;
using Path = System.IO.Path;

namespace Kubix.Services.Classes
{
    public class DownloadService : IDownloadService
    {
        public event EventHandler<Tuple<DownloadModel, DownloadModel>> DownloadProgressChangedEvent;
        public event EventHandler<Tuple<DownloadModel, DownloadModel>> ConfigureCommandsChangedEvent;

        private ClientEngine _engine;
        private ConcurrentDictionary<string, TorrentManager> _downloads;
        private readonly ILogger _logger;

        public DownloadService(ILogger logger)
        {
            _logger = logger;
            _downloads = new ConcurrentDictionary<string, TorrentManager>();

            StartEngine();
        }

        public async Task DownloadTorrentAsync(DownloadModel download)
        {
            TorrentManager _manager = await _engine.AddAsync(MagnetLink.Parse(download.Url.OriginalString), download.DownloadPath);
            _downloads.TryAdd(GetTorrentHash(download.Url.OriginalString), _manager);

            await _manager.StartAsync();
            _ = MonitorProgressAsync(_manager, download);
        }

        public async Task DownloadYoutubeAsync(DownloadModel download)
        {
            string ytdlpPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Executables", "yt-dlp.exe");

            if (!File.Exists(ytdlpPath))
            {
                throw new FileNotFoundException("yt-dlp.exe não encontrado!", ytdlpPath);
            }

            Dictionary<string, string> videoInfo = await GetJsonProperties(download.Url.AbsoluteUri);
            string packageName = videoInfo["title"];
            string packageThumbnail = videoInfo["thumbnail"];

            var formatCode = await GetBestCombinedFormat(download.Url.AbsoluteUri);

            if (formatCode != null)
            {
                string arguments = $"-f {formatCode} -o \"{download.DownloadPath}\\%(title)s.%(ext)s\" \"{download.Url.AbsoluteUri}\"";

                var startInfo = new ProcessStartInfo
                {
                    FileName = ytdlpPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                };

                using var process = new Process { StartInfo = startInfo };

                DownloadModel update = new(download.Url.AbsoluteUri);
                update.PackageName = packageName;
                update.Thumbnail = packageThumbnail;

                ActiveActions(download, update);

                process.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                        return;

                    if (e.Data.Contains("[download]"))
                    {
                        var match = Regex.Match(e.Data, @"\[download\]\s+(?<percent>[\d\.]+)%\s+of\s+(?<totalSizeNum>[\d\.]+)(?<totalSizeUnit>\w+)\s+at\s+(?<speed>[\d\.]+\w+/s)\s+ETA\s+(?<eta>[\d:]+)", RegexOptions.IgnoreCase);

                        if (match.Success)
                        {
                            _logger.InfoLog($"Download {download.PackageName} started.");
                            update.State = DownloadState.Downloading;
                            update.DownloadSize = double.Parse(match.Groups["totalSizeNum"].Value, CultureInfo.InvariantCulture);
                            update.PercentCompleted = (int)double.Parse(match.Groups["percent"].Value, CultureInfo.InvariantCulture);
                            update.BytesDownloaded = ((double)update.PercentCompleted / 100) * update.DownloadSize;
                            update.DownloadSpeed = match.Groups["speed"].Value;

                            if (update.PercentCompleted == 100)
                            {
                                update.State = DownloadState.Completed;
                                update.DownloadSpeed = "0 Mib/s";
                            }

                            if (update.PercentCompleted != download.PercentCompleted)
                            {
                                update.IsDownloading = false;
                                _logger.InfoLog($"Download {download.PackageName} {update.PercentCompleted}%");
                                InvokeEvent(EventEnum.DownloadProgress, download, update);
                            }
                        }
                    }
                };

                process.Start();
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();

                await process.WaitForExitAsync();

                if (process.ExitCode != 0)
                {
                    throw new Exception($"yt-dlp failed with code {process.ExitCode}");
                }
            }
        }

        private void StartEngine()
        {
            EngineSettings settings = new EngineSettingsBuilder
            {
                AllowPortForwarding = true,
                AutoSaveLoadDhtCache = true,
                AutoSaveLoadFastResume = true,
                CacheDirectory = ApplicationData.Current.LocalCacheFolder.Path,
                DhtEndPoint = new IPEndPoint(IPAddress.Any, 0),
                ListenEndPoints = new Dictionary<string, IPEndPoint>
                {
                    ["ipv4"] = new IPEndPoint(IPAddress.Any, 0),
                    ["ipv6"] = new IPEndPoint(IPAddress.IPv6Any, 0)
                },
            }.ToSettings();

            _engine = new ClientEngine(settings);
        }

        private async Task MonitorProgressAsync(TorrentManager manager, DownloadModel download)
        {
            DownloadModel update = new DownloadModel(download.Url.AbsoluteUri) { PackageName = manager.Name };
            update.DownloadPath = download.DownloadPath;
            update.State = DownloadState.Downloading;

            ActiveActions(download, update);

            do
            {
                if (manager.State == TorrentState.Downloading)
                {
                    Torrent torrent = await GetTorrentInfo(download);
                    update.DownloadSize = torrent.Size;
                }

                if (update.State == DownloadState.ContinueDownloading)
                {
                    await manager.StartAsync();
                    update.State = DownloadState.Downloading;
                    InvokeEvent(EventEnum.DownloadProgress, download, update);
                }
                else if (update.State == DownloadState.PrePaused)
                {
                    await manager.PauseAsync();
                    update.State = DownloadState.Paused;
                    InvokeEvent(EventEnum.DownloadProgress, download, update);
                }
                else if (update.State == DownloadState.Cancelled)
                {
                    ResetValues(update);
                    InvokeEvent(EventEnum.DownloadProgress, download, update);
                    await manager.StopAsync();
                    return;
                }

                update.PercentCompleted = (int)manager.Progress;
                update.BytesDownloaded = manager.Monitor.DataBytesReceived;
                update.DownloadSpeed = $"{manager.Monitor.DownloadRate / 1024d:0.##} KiB/s";

                InvokeEvent(EventEnum.DownloadProgress, download, update);
                await Task.Delay(1000);
            }
            while (manager.State != TorrentState.Seeding);

            update.BytesDownloaded = update.DownloadSize;
            update.PercentCompleted = 100;
            update.DownloadSpeed = string.Empty;
            update.IsDownloading = false;

            InvokeEvent(EventEnum.DownloadProgress, download, update);

            await manager.StopAsync();
        }

        private async Task<Torrent> GetTorrentInfo(DownloadModel download)
        {
            string torrentFile = Directory.GetFiles(ApplicationData.Current.LocalCacheFolder.Path + "\\metadata", "*.torrent")
                .FirstOrDefault(x => x.Contains(GetTorrentHash(download.Url.OriginalString)));

            if (torrentFile == null)
                return null;

            Torrent torrent = Torrent.Load(torrentFile);

            return torrent;
        }

        private string GetTorrentHash(string magnetLinkString)
        {
            var magnet = MagnetLink.Parse(magnetLinkString);
            string infoHash = magnet.InfoHashes.V1OrV2.ToHex();

            return infoHash;
        }
        private async Task<string?> GetBestCombinedFormat(string videoUrl)
        {
            string ytdlpPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Executables", "yt-dlp.exe");
            var formats = new List<(string code, string resolution)>();

            var psi = new ProcessStartInfo
            {
                FileName = ytdlpPath,
                Arguments = $"--list-formats \"{videoUrl}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = new Process { StartInfo = psi };
            process.OutputDataReceived += (s, e) =>
            {
                if (e.Data == null) return;

                var regex = new Regex(@"(?<code>\d+)\s+mp4\s+(?<res>\d{3,4}x\d{3,4}).*?mp4a", RegexOptions.IgnoreCase);
                var match = regex.Match(e.Data);
                if (match.Success)
                {
                    formats.Add((match.Groups["code"].Value, match.Groups["res"].Value));
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            await process.WaitForExitAsync();

            if (formats.Count == 0)
                return null;

            var best = formats
                .OrderByDescending(f => int.Parse(f.resolution.Split('x')[1]))
                .First();

            return best.code;
        }

        private async Task<Dictionary<string, string>> GetJsonProperties(string videoUrl)
        {
            Dictionary<string, string> jsonData = new();

            string ytdlpPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Executables", "yt-dlp.exe");

            var psi = new ProcessStartInfo
            {
                FileName = ytdlpPath,
                Arguments = $"--dump-json --no-check-certificates --no-playlist -f best \"{videoUrl}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = psi };

            var outputBuilder = new System.Text.StringBuilder();
            var errorBuilder = new System.Text.StringBuilder();

            process.OutputDataReceived += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    outputBuilder.AppendLine(e.Data);
            };

            process.Start();
            var outputTask = process.StandardOutput.ReadToEndAsync();

            await Task.WhenAll(outputTask, process.WaitForExitAsync());

            string output = await outputTask;

            if (!string.IsNullOrWhiteSpace(output))
            {
                var json = JsonDocument.Parse(output).RootElement;

                jsonData["title"] = json.GetProperty("title").GetString();
                jsonData["thumbnail"] = json.GetProperty("thumbnail").GetString();
            }

            return jsonData;
        }

        private void ResetValues(DownloadModel update)
        {
            update.PercentCompleted = 0;
            update.BytesDownloaded = 0;
            update.DownloadSize = 0;
            update.DownloadSpeed = string.Empty;
            update.State = DownloadState.Cancelled;
            update.IsDownloading = false;
        }

        private void ActiveActions(DownloadModel download, DownloadModel update)
        {
            update.PlayDownload = new AsyncRelayCommand(async () => await PlayDownloadAction(update));
            update.PauseDownload = new AsyncRelayCommand(async () => await PauseDownloadAction(update));
            update.CancelDownload = new AsyncRelayCommand(async () => await CancelDownloadAction(update));

            InvokeEvent(EventEnum.ConfigureCommands, download, update);
        }

        private void InvokeEvent(EventEnum eventType, DownloadModel download, DownloadModel update)
        {
            Tuple<DownloadModel, DownloadModel> downloadedFiles = new Tuple<DownloadModel, DownloadModel>(download, update);

            switch (eventType)
            {
                case EventEnum.ConfigureCommands:
                    ConfigureCommandsChangedEvent?.Invoke(this, downloadedFiles);
                    break;
                case EventEnum.DownloadProgress:
                    DownloadProgressChangedEvent?.Invoke(this, downloadedFiles);
                    break;
            }
        }

        #region Actions

        private async Task CancelDownloadAction(DownloadModel update)
        {
            update.State = DownloadState.Cancelled;
        }

        private async Task PlayDownloadAction(DownloadModel update)
        {
            update.State = DownloadState.ContinueDownloading;
        }

        private async Task PauseDownloadAction(DownloadModel update)
        {
            update.State = DownloadState.PrePaused;
        }

        #endregion
    }

    public enum EventEnum
    {
        ConfigureCommands,
        DownloadProgress
    }
}
