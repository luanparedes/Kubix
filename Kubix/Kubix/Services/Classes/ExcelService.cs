using CommunityToolkit.Mvvm.DependencyInjection;
using Kubix.Model;
using Kubix.Services.Interfaces;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace Kubix.Services.Classes
{
    public class ExcelService : IExcelService
    {
        #region Constants

        private const string EXCEL_NAME = "worldcities.xlsx";

        #endregion

        #region Fields & Properties

        private readonly ILogger _logger;
        private string filePath;
        #endregion

        #region Constructor

        public ExcelService()
        {
            _logger = Ioc.Default.GetService<ILogger>();
        }

        #endregion

        #region Methods

        public void InitializeExcelFile()
        {
            CreateExcelFile();
        }

        private async void CreateExcelFile()
        {
            try
            {
                StorageFile sourceFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/{EXCEL_NAME}"));
                StorageFile destinationFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(EXCEL_NAME, CreationCollisionOption.ReplaceExisting);
                await sourceFile.CopyAndReplaceAsync(destinationFile);

                string localFilePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, EXCEL_NAME);
                StorageFile file = await StorageFile.GetFileFromPathAsync(localFilePath);

                filePath = file.Path;

                _logger.InfoLog($"File copied succeffuly {filePath}");
            }
            catch (Exception ex)
            {
                _logger.ErrorLog($"Error while creating excel file: {ex.Message}");
            }
        }

        public List<CityModel> GetAllCities()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];

                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                List<CityModel> cities = new List<CityModel>();

                for (int row = 2; row <= rowCount; row++)
                {

                    cities.Add(new CityModel()
                    {
                        City = worksheet.Cells[row, 1].Text,
                        Latitude = Double.Parse(worksheet.Cells[row, 3].Text),
                        Longitude = Double.Parse(worksheet.Cells[row, 4].Text),
                        Country = worksheet.Cells[row, 5].Text,
                        State = worksheet.Cells[row, 8].Text,
                        Population = worksheet.Cells[row, 10].Text == string.Empty ? "0" : (int.Parse(worksheet.Cells[row, 10].Text)).ToString("N0"),
                    });

                }

                return cities;
            }
        }

        public List<CityModel> GetTypedCities(string text)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];

                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                List<CityModel> cities = new List<CityModel>();

                for (int row = 2; row <= rowCount; row++)
                {
                    if (worksheet.Cells[row, 1].Text.ToLower().Contains(text.ToLower()))
                    {
                        cities.Add(new CityModel()
                        {
                            City = worksheet.Cells[row, 1].Text,
                            Latitude = Double.Parse(worksheet.Cells[row, 3].Text),
                            Longitude = Double.Parse(worksheet.Cells[row, 4].Text),
                            Country = worksheet.Cells[row, 5].Text,
                            State = worksheet.Cells[row, 8].Text,
                            Population = worksheet.Cells[row, 10].Text == string.Empty ? "0" : (int.Parse(worksheet.Cells[row, 10].Text)).ToString("N0"),
                        });
                    }
                }

                return cities;
            }
        }

        public CityModel GetCityByPosition(double latitude, double longitude)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];

                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                List<CityModel> cities = new List<CityModel>();

                for (int row = 2; row <= rowCount; row++)
                {
                    if ((int)double.Parse(worksheet.Cells[row, 3].Text) == (int)latitude
                        && (int)double.Parse(worksheet.Cells[row, 4].Text) == (int)longitude)
                    {
                        cities.Add(new CityModel()
                        {
                            City = worksheet.Cells[row, 1].Text,
                            Latitude = Double.Parse(worksheet.Cells[row, 3].Text),
                            Longitude = Double.Parse(worksheet.Cells[row, 4].Text),
                            Country = worksheet.Cells[row, 5].Text,
                            State = worksheet.Cells[row, 8].Text,
                            Population = worksheet.Cells[row, 10].Text == string.Empty ? "0" : (int.Parse(worksheet.Cells[row, 10].Text)).ToString("N0")
                        });
                    }
                }

                return cities.First();
            }
        }

        #endregion
    }
}
