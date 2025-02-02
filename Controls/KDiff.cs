using DiffPlex.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Kubix.Controls
{
    public class KDiff : Control
    {
        #region Fields & Properties

        private RichEditBox _leftBox;
        private RichEditBox _rightBox;
        private DiffTextView _diffTextView;

        public string LeftText
        {
            get { return (string)GetValue(LeftTextProperty); }
            set { SetValue(LeftTextProperty, value); }
        }

        public static readonly DependencyProperty LeftTextProperty =
            DependencyProperty.Register(nameof(LeftText), typeof(string), typeof(KDiff), new PropertyMetadata(string.Empty, LeftTextChanged));

        public string RightText
        {
            get { return (string)GetValue(RightTextProperty); }
            set { SetValue(RightTextProperty, value); }
        }

        public static readonly DependencyProperty RightTextProperty =
            DependencyProperty.Register(nameof(RightText), typeof(string), typeof(KDiff), new PropertyMetadata(string.Empty, RightTextChanged));


        #endregion

        #region Event Handlers

        private void RichEditBox_TextChanged(object sender, RoutedEventArgs e)
        {
            string text;
            RichEditBox editBox = sender as RichEditBox;

            editBox.Document.GetText(TextGetOptions.UseCrlf, out text);

            switch (editBox.Tag)
            {
                case "LeftBox":
                    LeftText = text;
                    break;
                case "RightBox":
                    RightText = text;
                    break;
            }
        }

        #endregion

        #region Callbacks

        private static void RightTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is KDiff self)
            {
                self._diffTextView.NewText = (string)e.NewValue;
            }
        }

        private static void LeftTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is KDiff self)
            {
                self._diffTextView.OldText = (string)e.NewValue;
            }
        }

        #endregion

        #region OnApplyTemplate

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _leftBox = GetTemplateChild("LeftText") as RichEditBox;
            _rightBox = GetTemplateChild("RightText") as RichEditBox;
            _diffTextView = GetTemplateChild("DiffView") as DiffTextView;

            if (_leftBox != null)
            {
                _leftBox.TextChanged += RichEditBox_TextChanged;
            }

            if (_rightBox != null)
            {
                _rightBox.TextChanged += RichEditBox_TextChanged;
            }
        }

        #endregion
    }
}
