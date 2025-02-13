using Kubix.Model;
using Kubix.Services.Interfaces;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Storage;

namespace Kubix.Services.Classes
{
    public class ExcelService : IExcelService
    {
        #region Fields & Properties

        private const string EXCEL_NAME = "worldcities.xlsx";
        private string filePath;
        #endregion

        #region Constructor

        public ExcelService()
        {
            CreateExcelFile();
        }

        #endregion

        #region Methods

        private async void CreateExcelFile()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/{EXCEL_NAME}"));
            StorageFile copiedFile = await file.CopyAsync(ApplicationData.Current.LocalFolder, EXCEL_NAME, NameCollisionOption.ReplaceExisting);
            filePath = copiedFile.Path;
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
