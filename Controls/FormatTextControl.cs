using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;

namespace KanBoard.Controls
{
    public class FormatTextControl : Control
    {
        #region Fields & Properties

        ComboBox comboBox;

        public event EventHandler<FontFamily> KFamilyFontChanged;
        public event EventHandler KFontSizeChanged;
        public event EventHandler KForegroundChanged;
        public event EventHandler KBoldChanged;
        public event EventHandler KItalicChanged;
        public event EventHandler KUnderlineChanged;
        public event EventHandler KStrikethroughChanged;

        #endregion

        #region Dependency Properties

        public FontFamily KFontFamily
        {
            get { return (FontFamily)GetValue(KFontFamilyProperty); }
            set { SetValue(KFontFamilyProperty, value); }
        }

        public static readonly DependencyProperty KFontFamilyProperty =
            DependencyProperty.Register(nameof(KFontFamily), typeof(FontFamily), typeof(FormatTextControl), new PropertyMetadata(FontFamily.XamlAutoFontFamily, OnFamilyFontChanged));

        private static void OnFamilyFontChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FormatTextControl self)
            {
                //self.FontFamily = new FontFamily("Comic Sans MS");
                self.KFamilyFontChanged?.Invoke(self, e.NewValue as FontFamily);
            }
        }

        public string KFontSize
        {
            get { return (string)GetValue(KFontSizeProperty); }
            set { SetValue(KFontSizeProperty, value); }
        }

        public static readonly DependencyProperty KFontSizeProperty =
            DependencyProperty.Register(nameof(KFontSize), typeof(string), typeof(FormatTextControl), new PropertyMetadata("12"));

        public Brush KForeground
        {
            get { return (Brush)GetValue(KForegroundProperty); }
            set { SetValue(KForegroundProperty, value); }
        }

        public static readonly DependencyProperty KForegroundProperty =
            DependencyProperty.Register(nameof(KForeground), typeof(Brush), typeof(FormatTextControl), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public bool KBold
        {
            get { return (bool)GetValue(KBoldProperty); }
            set { SetValue(KBoldProperty, value); }
        }

        public static readonly DependencyProperty KBoldProperty =
            DependencyProperty.Register(nameof(KBold), typeof(bool), typeof(FormatTextControl), new PropertyMetadata(false));

        public bool KItalic
        {
            get { return (bool)GetValue(KItalicProperty); }
            set { SetValue(KItalicProperty, value); }
        }

        public static readonly DependencyProperty KItalicProperty =
            DependencyProperty.Register(nameof(KItalic), typeof(bool), typeof(FormatTextControl), new PropertyMetadata(false));

        public bool KUnderline
        {
            get { return (bool)GetValue(KUnderlineProperty); }
            set { SetValue(KUnderlineProperty, value); }
        }

        public static readonly DependencyProperty KUnderlineProperty =
            DependencyProperty.Register(nameof(KUnderline), typeof(bool), typeof(FormatTextControl), new PropertyMetadata(false));

        public bool KStrikethrough
        {
            get { return (bool)GetValue(KStrikethroughProperty); }
            set { SetValue(KStrikethroughProperty, value); }
        }

        public static readonly DependencyProperty KStrikethroughProperty =
            DependencyProperty.Register(nameof(KStrikethrough), typeof(bool), typeof(FormatTextControl), new PropertyMetadata(false));


        #endregion

        #region Event Handlers



        #endregion

        #region OnApplyTemplate

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            comboBox = GetTemplateChild("KComboBox") as ComboBox;

            if (comboBox != null)
            {
                comboBox.SelectionChanged += ComboBox_SelectionChanged;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string fontName = (string)(e.AddedItems[0] as ComboBoxItem).Content;
            KFontFamily = new FontFamily(fontName);
        }

        #endregion
    }
}
