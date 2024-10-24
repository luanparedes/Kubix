using KanBoard.View;
using Microsoft.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using System;
using Windows.UI;

namespace KanBoard.Controls
{
    public class FormatTextControl : Control
    {
        #region Fields & Properties

        public RichEditBox EditBox = new RichEditBox();

        private ComboBox fontFamilyComboBox;
        private ComboBox fontSizeComboBox;
        private Button kForegroundButton;
        private ToggleButton kBoldButton;
        private ToggleButton kItalicButton;
        private ToggleButton kUnderlineButton;
        private ToggleButton kStrikethroughButton;

        public bool HasChanges => !IsInitialText();
        public string InitialText { get; set; }

        public event EventHandler<FontFamily> KFamilyFontChanged;
        public event EventHandler<int> KFontSizeChanged;
        public event EventHandler<SolidColorBrush> KForegroundChanged;
        public event EventHandler<ToggleButton> KBoldChanged;
        public event EventHandler<bool> KItalicChanged;
        public event EventHandler<bool> KUnderlineChanged;
        public event EventHandler<bool> KStrikethroughChanged;

        #endregion

        #region Dependency Properties

        public FontFamily KFontFamily
        {
            get { return (FontFamily)GetValue(KFontFamilyProperty); }
            set { SetValue(KFontFamilyProperty, value); }
        }

        public static readonly DependencyProperty KFontFamilyProperty =
            DependencyProperty.Register(nameof(KFontFamily), typeof(FontFamily), typeof(FormatTextControl), new PropertyMetadata(FontFamily.XamlAutoFontFamily, OnFamilyFontChanged));

        public int KFontSize
        {
            get { return (int)GetValue(KFontSizeProperty); }
            set { SetValue(KFontSizeProperty, value); }
        }

        public static readonly DependencyProperty KFontSizeProperty =
            DependencyProperty.Register(nameof(KFontSize), typeof(int), typeof(FormatTextControl), new PropertyMetadata(12, OnFontSizeChanged));

        public SolidColorBrush KForeground
        {
            get { return (SolidColorBrush)GetValue(KForegroundProperty); }
            set { SetValue(KForegroundProperty, value); }
        }

        public static readonly DependencyProperty KForegroundProperty =
            DependencyProperty.Register(nameof(KForeground), typeof(SolidColorBrush), typeof(FormatTextControl), new PropertyMetadata(Colors.Black, OnForegroundChanged));

        public bool KBold
        {
            get { return (bool)GetValue(KBoldProperty); }
            set { SetValue(KBoldProperty, value); }
        }

        public static readonly DependencyProperty KBoldProperty =
            DependencyProperty.Register(nameof(KBold), typeof(bool), typeof(FormatTextControl), new PropertyMetadata(false, OnBoldChanged));

        public bool KItalic
        {
            get { return (bool)GetValue(KItalicProperty); }
            set { SetValue(KItalicProperty, value); }
        }

        public static readonly DependencyProperty KItalicProperty =
            DependencyProperty.Register(nameof(KItalic), typeof(bool), typeof(FormatTextControl), new PropertyMetadata(false, OnItalicChanged));

        public bool KUnderline
        {
            get { return (bool)GetValue(KUnderlineProperty); }
            set { SetValue(KUnderlineProperty, value); }
        }

        public static readonly DependencyProperty KUnderlineProperty =
            DependencyProperty.Register(nameof(KUnderline), typeof(bool), typeof(FormatTextControl), new PropertyMetadata(false, OnUnderlineChanged));

        public bool KStrikethrough
        {
            get { return (bool)GetValue(KStrikethroughProperty); }
            set { SetValue(KStrikethroughProperty, value); }
        }

        public static readonly DependencyProperty KStrikethroughProperty =
            DependencyProperty.Register(nameof(KStrikethrough), typeof(bool), typeof(FormatTextControl), new PropertyMetadata(false, OnStrikethroughChanged));

        #endregion

        #region Methods

        private bool IsInitialText()
        {
            string initialText = InitialText;
            string text;
            EditBox.Document.GetText(TextGetOptions.None, out text);

            initialText = initialText.Replace("\r", "").Trim();
            text = text.Replace("\r", "").Trim();

            bool teste = initialText.Equals(text);
            return teste;
        }

        private void InitializeColorPickerWindow()
        {
            var colorPickerWindow = new ColorPickerWindow();
            colorPickerWindow.Activate();

            colorPickerWindow.ViewModel.KColorChanged += ViewModel_KColorChanged;
        }

        #endregion

        #region Event Handlers

        private void FamilyFont_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string fontName = (string)(e.AddedItems[0] as ComboBoxItem).Content;
            KFontFamily = new FontFamily(fontName);
        }

        private void FontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string fontSize = (string)(e.AddedItems[0] as ComboBoxItem).Content;
            KFontSize = int.Parse(fontSize);
        }

        private void KForeground_Click(object sender, RoutedEventArgs e)
        {
            InitializeColorPickerWindow();
        }

        private void ToggleBtn_Checked(object sender, RoutedEventArgs e)
        {
            switch ((sender as ToggleButton).Tag)
            {
                case "BoldBtn":
                    KBold = (bool)kBoldButton.IsChecked;
                    break;
                case "ItalicBtn":
                    KItalic = (bool)kItalicButton.IsChecked;
                    break;
                case "UnderlineBtn":
                    KUnderline = (bool)kUnderlineButton.IsChecked;
                    break;
                case "StrikethroughBtn":
                    KStrikethrough = (bool)kStrikethroughButton.IsChecked;
                    break;
            }
        }

        private void ViewModel_KColorChanged(object sender, ColorChangedEventArgs e)
        {
            kForegroundButton.Background = new SolidColorBrush(e.NewColor);
            KForeground = new SolidColorBrush(e.NewColor);
        }

        #endregion

        #region Callbacks

        private static void OnFamilyFontChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FormatTextControl self)
            {
                self.KFamilyFontChanged?.Invoke(self, e.NewValue as FontFamily);
            }
        }

        private static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FormatTextControl self)
            {
                self.KFontSizeChanged?.Invoke(self, (int)e.NewValue);
            }
        }

        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FormatTextControl self)
            {
                self.KForegroundChanged?.Invoke(self, (SolidColorBrush)e.NewValue);
            }
        }

        private static void OnBoldChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FormatTextControl self)
            {
                self.KBoldChanged?.Invoke(self, self.kBoldButton);
            }
        }

        private static void OnItalicChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FormatTextControl self)
            {
                self.KItalicChanged?.Invoke(self, (bool)e.NewValue);
            }
        }

        private static void OnUnderlineChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FormatTextControl self)
            {
                self.KUnderlineChanged?.Invoke(self, (bool)e.NewValue);
            }
        }

        private static void OnStrikethroughChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FormatTextControl self)
            {
                self.KStrikethroughChanged?.Invoke(self, (bool)e.NewValue);
            }
        }

        #endregion

        #region OnApplyTemplate

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            fontFamilyComboBox = GetTemplateChild("KComboBox") as ComboBox;
            fontSizeComboBox = GetTemplateChild("KFontSize") as ComboBox;
            kForegroundButton = GetTemplateChild("KForeground") as Button;
            kBoldButton = GetTemplateChild("KBold") as ToggleButton;
            kItalicButton = GetTemplateChild("KItalic") as ToggleButton;
            kUnderlineButton = GetTemplateChild("KUnderline") as ToggleButton;
            kStrikethroughButton = GetTemplateChild("KStrikethrough") as ToggleButton;

            if (fontFamilyComboBox != null)
            {
                fontFamilyComboBox.SelectionChanged += FamilyFont_SelectionChanged;
            }

            if (fontSizeComboBox != null)
            {
                fontSizeComboBox.SelectionChanged += FontSize_SelectionChanged;
            }

            if(kForegroundButton != null)
            {
                kForegroundButton.Click += KForeground_Click;
            }

            if (kBoldButton != null)
            {
                kBoldButton.Checked += ToggleBtn_Checked;
            }

            if (kItalicButton != null)
            {
                kItalicButton.Checked += ToggleBtn_Checked;
            }

            if (kUnderlineButton != null)
            {
                kUnderlineButton.Checked += ToggleBtn_Checked;
            }

            if (kStrikethroughButton != null)
            {
                kStrikethroughButton.Checked += ToggleBtn_Checked;
            }
        }

        #endregion
    }
}
