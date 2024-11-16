using CommunityToolkit.Mvvm.DependencyInjection;
using Kubix.Model;
using Kubix.Services.Interfaces;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Kubix.Controls
{
    public class WeatherForecastControl : Control
    {
        #region Constants

        private const int MinTextSizeToSearch = 3;

        #endregion

        #region Fields & Properties

        private readonly string apiKey = "4d324dab12c5eace63c194da0f4a0ec5";
        private readonly IExcelService _excelService;

        private AutoSuggestBox searchBox;
        private ComboBox comboState;
        private TextBlock temperatureText;
        private TextBlock cityText;

        private CityModel ActualCity { get; set; }

        #endregion

        #region Constructor

        public WeatherForecastControl()
        {
            _excelService = Ioc.Default.GetService<IExcelService>();
        }

        #endregion

        #region Methods

        public async Task<List<CityModel>> GetCitiesToChoose(string cityName, string stateName)
        {
            string url = string.Empty;

            if (stateName.Equals("All"))
                url = $"https://apiadvisor.climatempo.com.br/api/v1/locale/city?name={cityName}&token={apiKey}";
            else
                url = $"https://apiadvisor.climatempo.com.br/api/v1/locale/city?name={cityName}&state={stateName}&token={apiKey}";

            using HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    List<CityModel> cities = JsonSerializer.Deserialize<List<CityModel>>(jsonResponse);

                    return cities;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro: {ex.Message}");
            }

            return null;
        }

        public async Task<float?> GetCurrentTemperatureAsync(int cityId)
        {
            string url = $"https://apiadvisor.climatempo.com.br/api/v1/weather/locale/{cityId}/current?token={apiKey}";

            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Dados climáticos para a cidade {cityId}: {jsonResponse}");

                // A partir do jsonResponse, você pode desserializar para obter os dados específicos, como a temperatura
            }

            return 0.0f;
        }

        public async Task<bool> RegisterCityAsync(int cityId)
        {
            string url = $"https://apiadvisor.climatempo.com.br/api-manager/user-token/{apiKey}/locales";

            using (HttpClient client = new HttpClient())
            {
                // Cria o corpo da requisição com o ID da cidade como array JSON
                var content = new StringContent($"[{cityId}]", Encoding.UTF8, "application/json");

                // Envia a requisição PUT
                HttpResponseMessage response = await client.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Cidade com ID {cityId} registrada com sucesso.");
                    return true;
                }
                else
                {
                    // Exibe o código do erro para diagnóstico
                    Console.WriteLine($"Erro ao registrar cidade: {response.StatusCode}");
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Erro detalhado: {errorContent}");
                    return false;
                }
            }
        }

        private bool CanFindCity()
        {
            return searchBox.Text.Length >= MinTextSizeToSearch;
        }

        #endregion

        #region OnApplyTemplate

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            searchBox = GetTemplateChild("SearchCity") as AutoSuggestBox;
            comboState = GetTemplateChild("ComboState") as ComboBox;
            temperatureText = GetTemplateChild("TemperatureText") as TextBlock;
            cityText = GetTemplateChild("CityText") as TextBlock;

            if (searchBox != null)
            {
                searchBox.TextChanged += SearchBox_TextChanged;
                searchBox.SuggestionChosen += SearchBox_SuggestionChosen;
            }

            GetUserLocation();
        }

        private async void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (CanFindCity())
            {
                //List<CityModel> cities = await GetCitiesToChoose(searchBox.Text, (string)(comboState.SelectedItem as ComboBoxItem).Content.ToString());
                List<CityModel> cities = _excelService.GetTypedCities(searchBox.Text);
                searchBox.ItemsSource = cities;
            }
        }

        private async void SearchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            //_excelService.Teste();

            ActualCity = args.SelectedItem as CityModel;

            //if (await RegisterCityAsync(ActualCity.Id))
            //{
            //    ActualCity.Temperature = await GetCurrentTemperatureAsync(ActualCity.Id);
            //}

            searchBox.Text = string.Empty;
            cityText.Text = $"{ActualCity.City}, {ActualCity.Country}";
        }

        private async Task GetUserLocation()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();

            if (accessStatus == GeolocationAccessStatus.Allowed)
            {
                var geolocator = new Geolocator { DesiredAccuracyInMeters = 50 };
                var position = await geolocator.GetGeopositionAsync();

                var latitude = position.Coordinate.Point.Position.Latitude;
                var longitude = position.Coordinate.Point.Position.Longitude;

                ActualCity = _excelService.GetCityByPosition(latitude, longitude);
                cityText.Text = $"{ActualCity.City}, {ActualCity.Country}";
            }
        }

        #endregion
    }
}
