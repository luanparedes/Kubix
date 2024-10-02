using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanBoard.Services.Interfaces
{
    public interface ILogger
    {
        void InfoLog(string message);
        void WarnLog(string message);
        void ErrorLog(string message);
    }
}
