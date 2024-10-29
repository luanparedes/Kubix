using Kubix.Services.Interfaces;
using System;
using System.Diagnostics;

namespace Kubix.Services.Classes
{
    public class LogService : ILogger
    {
        public void ErrorLog(string message)
        {
            Debug.WriteLine($"Error >>>>> \n{message}");
        }

        public void InfoLog(string message)
        {
            Debug.WriteLine($"Info > \n{message}");
        }

        public void WarnLog(string message)
        {
            Debug.WriteLine($"Warning >>> \n{message}");
        }
    }
}
