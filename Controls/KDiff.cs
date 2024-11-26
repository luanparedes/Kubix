using Microsoft.UI.Xaml.Controls;

namespace Kubix.Controls
{
    public class KDiff : Control
    {
        #region Fields & Properties

        private RichEditBox _leftBox;
        private RichEditBox _rightBox;

        #endregion

        #region OnApplyTemplate

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _leftBox = GetTemplateChild("LeftText") as RichEditBox;
            _rightBox = GetTemplateChild("RightText") as RichEditBox;
        }

        #endregion
    }
}
