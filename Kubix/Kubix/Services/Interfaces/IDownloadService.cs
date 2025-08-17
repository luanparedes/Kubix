using Kubix.Model;
using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kubix.Services.Interfaces
{
    public interface IDownloadService
    {
        event EventHandler<Tuple<DownloadModel, DownloadModel>> DownloadProgressChangedEvent;
        event EventHandler<Tuple<DownloadModel, DownloadModel>> ConfigureCommandsChangedEvent;

        Task DownloadYoutubeAsync(DownloadModel download);
        Task DownloadTorrentAsync(DownloadModel download);
    }
}
