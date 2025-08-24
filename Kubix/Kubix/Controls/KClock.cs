using Kubix.Model;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Kubix.Controls
{
    public class KClock : Control
    {
        #region Fields & Properties

        private TextBlock _clockText;

        public CityModel ModelCity
        {
            get { return (CityModel)GetValue(CityModelProperty); }
            set { SetValue(CityModelProperty, value); }
        }

        public static readonly DependencyProperty CityModelProperty =
            DependencyProperty.Register(nameof(ModelCity), typeof(CityModel), typeof(KClock), new PropertyMetadata(null, OnCityModelChanged));

        public DispatcherTimer Timer
        {
            get { return (DispatcherTimer)GetValue(TimerProperty); }
            set { SetValue(TimerProperty, value); }
        }

        public static readonly DependencyProperty TimerProperty =
            DependencyProperty.Register(nameof(Timer), typeof(DispatcherTimer), typeof(KClock), new PropertyMetadata(new DispatcherTimer()));

        public ClockType Clock
        {
            get { return (ClockType)GetValue(ClockProperty); }
            set { SetValue(ClockProperty, value); }
        }

        public static readonly DependencyProperty ClockProperty =
            DependencyProperty.Register(nameof(Clock), typeof(ClockType), typeof(KClock), new PropertyMetadata(ClockType.Main));

        #endregion

        #region Constructor

        public KClock()
        {
            Loaded += KClock_Loaded;
        }

        #endregion

        #region Methods

        private void StartClock()
        {
            if (Timer != null)
            {
                StopTimer();
            }

            StartTimer();
        }

        private void StartTimer()
        {
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        private void StopTimer()
        {
            Timer.Tick -= Timer_Tick;
            Timer.Stop();
        }

        #endregion

        #region Event Handlers

        private void KClock_Loaded(object sender, RoutedEventArgs e)
        {
            if (Clock == ClockType.Main)
            {
                StartClock();
            }
        }

        private void Timer_Tick(object sender, object e)
        {
            string hour = Clock == ClockType.Main ? DateTime.Now.ToString("HH") : ModelCity.ActualTime.Split(':')[0];
            string minute = DateTime.Now.ToString("mm");
            string completeTime = $"{hour}:{minute}";

            _clockText.Text = completeTime;
        }

        #endregion

        #region Callbacks

        private static void OnCityModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is KClock clock)
            {
                clock.StartClock();
            }
        }

        #endregion

        #region OnApplyTemplate

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _clockText = GetTemplateChild("ClockRun") as TextBlock;
        }

        #endregion
    }

    public enum ClockType
    {
        Main,
        City
    }
}
