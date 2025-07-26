using Kubix.Services.Interfaces;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using Windows.Storage;
using ILog = Serilog;

namespace Kubix.Services.Classes
{
    public class LogService : Interfaces.ILogger
    {
        private ILog.ILogger _logger;

        public LogService()
        {
            LoggerConfiguration();
        }


        public void ErrorLog(string message)
        {
            _logger.Error(message);
        }

        public void InfoLog(string message)
        {
            _logger.Information(message);
        }

        public void WarnLog(string message)
        {
            _logger.Warning(message);
        }

        private void LoggerConfiguration()
        {
            string file = SetLogPath();

            _logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Debug()
                .WriteTo.File(file, rollingInterval: RollingInterval.Day, shared: true)
                .CreateLogger();
        }

        private static string SetLogPath()
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                string fileNameFormat = $"log_kubix-.txt";

                string logPath = ApplicationData.Current.LocalFolder.Path;
                string logFile = System.IO.Path.Combine(logPath, "logs", fileNameFormat);

                string logDirectory = System.IO.Path.GetDirectoryName(logFile);

                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                return logFile;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return string.Empty;
            
        }
    }
}
