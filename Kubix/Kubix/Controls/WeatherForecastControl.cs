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
using Windows.Media.Core;

namespace Kubix.Controls
{
    public class WeatherForecastControl : Control
    {
        #region Constants

        private const string LOADING_STATE = "LoadingState";
        private const string WEATHER_STATE = "WeatherState";

        private const int MinTextSizeToSearch = 3;
        private readonly string WeatherApiKey = "255519a40d4d423596a174654252008";

        #endregion

        #region Fields & Properties

        private readonly ILogger _logger;
        private readonly IExcelService _excelService;

        private Control mainControl;
        private AutoSuggestBox searchBox;
        private TextBlock temperatureText;
        private TextBlock cityText;
        private KClock kClock;
        private TextBlock dateText;
        private TextBlock populationText;
        private ProgressRing loadingWeather;
        private BitmapIcon weatherBitmap;

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

        private async Task<CityModel> GetCityInfoAsync(CityModel cityModel = null)
        {
            if (cityModel == null)
            {
                var accessStatus = await Geolocator.RequestAccessAsync();

                if (accessStatus == GeolocationAccessStatus.Allowed)
                {

                    var geolocator = new Geolocator { DesiredAccuracyInMeters = 50 };
                    var position = await geolocator.GetGeopositionAsync();

                    double latitude = position.Coordinate.Point.Position.Latitude;
                    double longitude = position.Coordinate.Point.Position.Longitude;

                    cityModel = _excelService.GetCityByPosition(latitude, longitude);
                }
            }

            cityModel.City = TextWithoutAccent(cityModel.City);

            string url = $"https://api.weatherapi.com/v1/current.json?key={WeatherApiKey}&q={cityModel.City}&aqi=no";

            using HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JObject jsonParsed = JObject.Parse(jsonResponse);

                    cityModel.Temperature = await GetCurrentTemperatureAsync(jsonParsed);
                    cityModel.ActualTime = await GetCurrentTimeAsync(jsonParsed);
                    cityModel.ActualDate = await GetCurrentDateAsync(jsonParsed);
                    cityModel.WeatherIcon = await GetWeatherIcon(jsonParsed);
                }
            }
            catch (WeatherApiException ex)
            {
                _logger.ErrorLog($"Error fetching city info: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.ErrorLog($"Unexpected error: {ex.Message}");
            }

            return cityModel;
        }

        private async Task<float> GetCurrentTemperatureAsync(JObject jsonParsed)
        {
            return float.Parse(jsonParsed["current"]["temp_c"]?.ToString());
        }

        private async Task<string> GetCurrentTimeAsync(JObject jsonParsed)
        {
            string time = jsonParsed["location"]["localtime"]?.ToString();
            DateTime dateTime = DateTime.ParseExact(time, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            
            return dateTime.ToString("HH:mm");
        }

        private async Task<string> GetCurrentDateAsync(JObject jsonParsed)
        {
            string time = jsonParsed["location"]["localtime"]?.ToString();
            DateTime dateTime = DateTime.ParseExact(time, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            
            return dateTime.ToString("dd/MM/yyyy");
        }

        private async Task<BitmapIcon> GetWeatherIcon(JObject jsonParsed)
        {
            BitmapIcon bitmap = new BitmapIcon();
            string source = "http:" + jsonParsed["current"]["condition"]["icon"]?.ToString();
            bitmap.UriSource = new Uri(source);

            return bitmap;
        }

        private void ShowInfoOnScreen()
        {
            this.DispatcherQueue.TryEnqueue(() =>
            {
                searchBox.Text = string.Empty;
                cityText.Text = $"{ActualCity.City}, {ActualCity.Country}";
                dateText.Text = $"{ActualCity.ActualDate}";
                temperatureText.Text = ActualCity.Temperature.ToString() + " º";
                weatherBitmap.UriSource = ActualCity.WeatherIcon.UriSource;
                populationText.Text = Stringer.GetString("KB_HasPopulationText", ActualCity.City, ActualCity.Population);
                loadingWeather.IsActive = false;
            });

            CurrentState = WEATHER_STATE;
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

            ActualCity = await GetCityInfoAsync(args.SelectedItem as CityModel);
            kClock.ModelCity = ActualCity;
            ShowInfoOnScreen();
        }

        private async void MainControl_Loaded(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(mainControl, CurrentState, true);

            await Task.Run(() =>
            {
                _excelService.InitializeExcelFile();
            });

            ActualCity = await GetCityInfoAsync();
            kClock.ModelCity = ActualCity;
            ShowInfoOnScreen();
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
            kClock = GetTemplateChild("KClock") as KClock;
            dateText = GetTemplateChild("DateText") as TextBlock;
            populationText = GetTemplateChild("PopulationText") as TextBlock;
            loadingWeather = GetTemplateChild("WeatherProgressRing") as ProgressRing;
            weatherBitmap = GetTemplateChild("WeatherBitmap") as BitmapIcon;

            if (mainControl != null)
            {
                mainControl.Loaded += MainControl_Loaded;
            }

            if (searchBox != null)
            {
                searchBox.TextChanged += SearchBox_TextChanged;
                searchBox.SuggestionChosen += SearchBox_SuggestionChosen;
            }
        }

        #endregion
    }

    #region WeatherApiException

    public class WeatherApiException : Exception
    {
        public WeatherApiException() {}

        public WeatherApiException(string message)
            : base(message) {}

        public WeatherApiException(string message, Exception innerException)
            : base(message, innerException) {}
    }

    #endregion
}
