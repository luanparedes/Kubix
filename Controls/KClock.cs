using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kubix.Controls
{
    public class KClock : Control
    {
        #region Fields & Properties

        private TextBlock _clockText;
        private DispatcherTimer _timer;

        #endregion

        #region Constructor

        public KClock()
        {
            StartClock();
        }

        #endregion

        #region Methods

        private void StartClock()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        #endregion

        #region Event Handlers

        private void Timer_Tick(object sender, object e)
        {
            _clockText.Text = DateTime.Now.ToString("H:mm");
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
}
