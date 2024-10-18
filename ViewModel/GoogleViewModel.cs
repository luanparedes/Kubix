using KanBoard.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanBoard.ViewModel
{
    public class GoogleViewModel
    {
        #region Constants

        public const string STATE_CHOICE_APP = "ChoiceAppState";
        public const string STATE_PHOTOS_APP = "PhotosAppState";
        public const string STATE_MAPS_APP = "MapsAppState";
        public const string STATE_TRANSLATE_APP = "TranslateAppState";

        public readonly string PhotosURL = "https://photos.google.com/";
        public readonly string MapsURL = "https://google.com/maps/";
        public readonly string TranslateURL = "https://translate.google.com/";

        #endregion

        #region Fields & Properties

        private Control pageControl;

        public GoogleApp ActualGoogleApp;

        private GoogleAuthentication auth;
        public string CurrentState { get; set; } = STATE_CHOICE_APP;

        #endregion

        #region Constructors

        public GoogleViewModel()
        {
            auth = new GoogleAuthentication();
        }

        #endregion

        #region Event Handlers

        public void PageControl_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            pageControl = sender as UserControl;
            VisualStateManager.GoToState(pageControl, CurrentState, true);
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            switch (button.Tag)
            {
                case "PhotosBtn":
                    ActualGoogleApp = GoogleApp.Photos;
                    CurrentState = STATE_PHOTOS_APP;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    break;
                case "MapsBtn":
                    ActualGoogleApp = GoogleApp.Maps;
                    CurrentState = STATE_MAPS_APP;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    break;
                case "TranslateBtn":
                    ActualGoogleApp = GoogleApp.Translate;
                    CurrentState = STATE_TRANSLATE_APP;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    break;
                case "BackButton":
                    CurrentState = STATE_CHOICE_APP;
                    VisualStateManager.GoToState(pageControl, CurrentState, true);
                    break;
            }
        }

        #endregion
    }

    public enum GoogleApp
    {
        Photos,
        Maps,
        Translate
    }
}
