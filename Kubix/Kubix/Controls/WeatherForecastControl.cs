using CommunityToolkit.Mvvm.DependencyInjection;
using Kubix.Helpers;
using Kubix.Model;
using Kubix.Services.Interfaces;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Kubix.Controls
{
    public class WeatherForecastControl : Control
    {
        #region Constants

        private const string LOADING_STATE = "LoadingState";
        private const string WEATHER_STATE = "WeatherState";

        private const int MinTextSizeToSearch = 3;
        private readonly string WeatherApiKey = "b1f1a549064b4081a33152427241611";

        #endregion

        #region Fields & Properties

        private readonly ILogger _logger;
        private readonly IExcelService _excelService;

        private Control mainControl;
        private AutoSuggestBox searchBox;
        private TextBlock temperatureText;
        private TextBlock cityText;
        private TextBlock populationText;
        private ProgressRing loadingWeather;

        public string CurrentState { get; set; } = LOADING_STATE;

        private CityModel ActualCity { get; set; }

        #endregion

        #region Constructor

        public WeatherForecastControl()
        {
            _logger = Ioc.Default.GetService<ILogger>();
            _excelService = Ioc.Default.GetService<IExcelService>();
        }

        #endregion

        #region Methods       

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
                ActualCity.Temperature = (float)await GetCurrentTemperatureAsync(ActualCity.City);

                ShowInfoOnScreen();
            }
        }

        public async Task<float?> GetCurrentTemperatureAsync(string city)
        {
            city = TextWithoutAccent(city);

            string url = $"https://api.weatherapi.com/v1/current.json?key={WeatherApiKey}&q={city}&aqi=no";

            using HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var jsonParsed = JObject.Parse(jsonResponse);
                    
                    float temperature = float.Parse(jsonParsed["current"]["temp_c"]?.ToString());

                    return temperature;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }

            return null;
        }

        private void ShowInfoOnScreen()
        {
            searchBox.Text = string.Empty;
            cityText.Text = $"{ActualCity.City}, {ActualCity.Country}";
            temperatureText.Text = ActualCity.Temperature.ToString() + " º";
            populationText.Text = Stringer.GetString("KB_HasPopulationText", ActualCity.City, ActualCity.Population);

            CurrentState = WEATHER_STATE;
            loadingWeather.IsActive = false;
            VisualStateManager.GoToState(mainControl, CurrentState, true);

            _logger.InfoLog($"Country: {ActualCity.Country}\n" +
                            $"City: {ActualCity.City}\n" +
                            $"Temperature: {ActualCity.Temperature}\n" +
                            $"Population: {ActualCity.Population}");
        }

        private string TextWithoutAccent(string city)
        {
            string normalizedText = city.Normalize(NormalizationForm.FormD);

            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        private bool CanFindCity()
        {
            return searchBox.Text.Length >= MinTextSizeToSearch;
        }

        #endregion

        #region Event Handlers

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (CanFindCity())
            {
                List<CityModel> cities = _excelService.GetTypedCities(searchBox.Text);
                searchBox.ItemsSource = cities;
            }
        }

        private async void SearchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            CurrentState = LOADING_STATE;
            loadingWeather.IsActive = true;
            VisualStateManager.GoToState(mainControl, CurrentState, true);

            ActualCity = args.SelectedItem as CityModel;
            ActualCity.Temperature = (float)await GetCurrentTemperatureAsync(ActualCity.City);

            ShowInfoOnScreen();
        }

        private async void MainControl_Loaded(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(mainControl, CurrentState, true);

            await Task.Run(async () =>
            {
                _excelService.InitializeExcelFile();
                await GetUserLocation();
            });
        }

        #endregion

        #region OnApplyTemplate

        protected override async void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            mainControl = GetTemplateChild("MainControl") as Control;
            searchBox = GetTemplateChild("SearchCity") as AutoSuggestBox;
            temperatureText = GetTemplateChild("TemperatureText") as TextBlock;
            cityText = GetTemplateChild("CityText") as TextBlock;
            populationText = GetTemplateChild("PopulationText") as TextBlock;
            loadingWeather = GetTemplateChild("WeatherProgressRing") as ProgressRing;

            if (mainControl != null)
            {
                mainControl.Loaded += MainControl_Loaded;
            }

            if (searchBox != null)
            {
                searchBox.TextChanged += SearchBox_TextChanged;
                searchBox.SuggestionChosen += SearchBox_SuggestionChosen;
            }

            //await GetUserLocation();
        }

        #endregion
    }
}
