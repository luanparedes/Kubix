using Kubix.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kubix.Services.Interfaces
{
    public interface IExcelService
    {
        Task InitializeExcelFile();
        public List<CityModel> GetTypedCities(string text);
        public List<CityModel> GetAllCities();
        public Task<CityModel> GetCityByPosition(double latitude, double longitude);
    }
}
