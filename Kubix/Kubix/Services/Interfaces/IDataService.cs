using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kubix.Services.Interfaces
{
    public interface IDataService
    {
        public void CreateDatabaseIfNotExists();
        public void CreateTerminalCommandsTable();
        public void InsertIntoCommands(string command);
        public List<string> GetDBCommands();
        public string GetDBPath();
    }
}
