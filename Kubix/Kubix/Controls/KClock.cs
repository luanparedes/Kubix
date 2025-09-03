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
        public event EventHandler<bool> DayChanged;

        public string ActualTime
        {
            get { return (string)GetValue(ActualTimeProperty); }
            set { SetValue(ActualTimeProperty, value); }
        }

        public static readonly DependencyProperty ActualTimeProperty =
            DependencyProperty.Register(nameof(ActualTime), typeof(string), typeof(KClock), new PropertyMetadata("00:00"));

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

        private bool IsDayChanged(string completeHour)
        {
            return completeHour.Equals("00:00:00");
        }

        private bool IsHourChanged(string minute, string second)
        {
            return minute.Equals("00") && second.Equals("00");
        }

        #endregion

        #region Event Handlers

        private void KClock_Loaded(object sender, RoutedEventArgs e)
        {
            StartClock();
        }

        private void Timer_Tick(object sender, object e)
        {
            string hour = Clock == ClockType.Main ? DateTime.Now.ToString("HH") : ActualTime.Split(':')[0];
            string minute = DateTime.Now.ToString("mm");
            string second = DateTime.Now.ToString("ss");
            string timeWithSeconds = $"{hour}:{minute}:{second}";
            string completeTime = $"{hour}:{minute}";

            if (IsHourChanged(minute, second))
            {
                DateTime newHour = DateTime.Parse(timeWithSeconds);
                newHour = newHour.AddHours(1);
                completeTime = newHour.ToString("HH:mm");
                timeWithSeconds = newHour.ToString("HH:mm:ss");
            }

            if (IsDayChanged(timeWithSeconds))
            {
                DayChanged?.Invoke(this, true);
            }

            _clockText.Text = ActualTime = completeTime;
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
