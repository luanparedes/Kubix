using KanBoard.Helpers;
using KanBoard.View;
using Microsoft.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using System;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace KanBoard.Controls
{
    public class FormatTextControl : Control
    {
        #region Fields & Properties

        private RichEditBox editBox;
        private ComboBox fontFamilyComboBox;
        private ComboBox fontSizeComboBox;
        private Button kForegroundButton;
        private ToggleButton kBoldButton;
        private ToggleButton kItalicButton;
        private ToggleButton kUnderlineButton;
        private ToggleButton kStrikethroughButton;

        private CreateFileEnum fileEnum;
        private string ActualText;

        public event EventHandler<string> KTextChanged; 

        public bool HasChanges => !IsInitialText();
        public string InitialText { get; set; } = string.Empty;
        public StorageFile TabFile { get; set; }

        #endregion

        #region Dependency Properties

        public FontFamily KFontFamily
        {
            get { return (FontFamily)GetValue(KFontFamilyProperty); }
            set { SetValue(KFontFamilyProperty, value); }
        }

        public static readonly DependencyProperty KFontFamilyProperty =
            DependencyProperty.Register(nameof(KFontFamily), typeof(FontFamily), typeof(FormatTextControl), new PropertyMetadata(FontFamily.XamlAutoFontFamily));

        public int KFontSize
        {
            get { return (int)GetValue(KFontSizeProperty); }
            set { SetValue(KFontSizeProperty, value); }
        }

        public static readonly DependencyProperty KFontSizeProperty =
            DependencyProperty.Register(nameof(KFontSize), typeof(int), typeof(FormatTextControl), new PropertyMetadata(12));

        public SolidColorBrush KForeground
        {
            get { return (SolidColorBrush)GetValue(KForegroundProperty); }
            set { SetValue(KForegroundProperty, value); }
        }

        public static readonly DependencyProperty KForegroundProperty =
            DependencyProperty.Register(nameof(KForeground), typeof(SolidColorBrush), typeof(FormatTextControl), new PropertyMetadata(new SolidColorBrush(Colors.AliceBlue)));

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

        #region Constructor

        public FormatTextControl(CreateFileEnum fileEnum, string text)
        {
            Loaded += FormatTextControl_Loaded;

            this.fileEnum = fileEnum;
            this.ActualText = text;
        }

        private void FormatTextControl_Loaded(object sender, RoutedEventArgs e)
        {
            CreateNote();
            this.kForegroundButton.Background = KForeground;
        }

        #endregion

        #region Methods

        private void CreateNote()
        {
            if (editBox != null)
            {
                switch (fileEnum)
                {
                    case CreateFileEnum.NewFile:
                        editBox.Document.GetText(TextGetOptions.None, out ActualText);
                        break;
                    case CreateFileEnum.OpenFile:
                        editBox.Document.SetText(TextSetOptions.None, ActualText);
                        break;
                }
            }
        }

        public async Task<StorageFile> OpenFile()
        {
            var openPicker = new FileOpenPicker();
            openPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            openPicker.FileTypeFilter.Add(".txt");
            openPicker.FileTypeFilter.Add(".rtf");

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Instance.MainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hwnd);

            StorageFile file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                switch (file.FileType)
                {
                    case ".txt":
                        string text = await Windows.Storage.FileIO.ReadTextAsync(file);
                        editBox.Document.SetText(TextSetOptions.None, text);
                        break;

                    case ".rtf":
                        using (var stream = await file.OpenAsync(FileAccessMode.Read))
                        {
                            editBox.Document.LoadFromStream(TextSetOptions.FormatRtf, stream);
                        }
                        break;
                }
            }

            return file;
        }

        public async Task<StorageFile> SaveFile()
        {
            string text = string.Empty;

            if (TabFile != null)
            {
                switch (TabFile.FileType)
                {
                    case ".txt":
                        editBox.Document.GetText(TextGetOptions.None, out text);
                        break;
                    case ".rtf":
                        editBox.Document.GetText(TextGetOptions.FormatRtf, out text);
                        break;
                }

                await FileIO.WriteTextAsync(TabFile, text);
            }
            else
            {
                var savePicker = new FileSavePicker();

                savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                savePicker.FileTypeChoices.Add("Text File", new List<string>() { ".txt", ".rtf" });
                savePicker.SuggestedFileName = Stringer.GetString("KB_NewDocumentText");

                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Instance.MainWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hwnd);

                TabFile = await savePicker.PickSaveFileAsync();

                if (TabFile != null)
                {
                    switch (TabFile.FileType)
                    {
                        case ".txt":
                            editBox.Document.GetText(TextGetOptions.None, out text);
                            break;
                        case ".rtf":
                            editBox.Document.GetText(TextGetOptions.FormatRtf, out text);
                            break;
                    }

                    await FileIO.WriteTextAsync(TabFile, text);
                }
            }

            if (TabFile == null)
                return TabFile;

            InitialText = editBox.Document.ToString();

            return TabFile;
        }

        private bool IsInitialText()
        {
            if (editBox != null)
            {
                string initialText = InitialText;
                string text;
                editBox.Document.GetText(TextGetOptions.None, out text);

                initialText = initialText.Replace("\r", "").Trim();
                text = text.Replace("\r", "").Trim();

                return initialText.Equals(text);
            }

            return false;
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

            var selectedText = editBox.Document.Selection;

            if (!selectedText.Equals(""))
            {
                selectedText.CharacterFormat.Name = KFontFamily.Source;
            }
        }

        private void FontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string fontSize = (string)(e.AddedItems[0] as ComboBoxItem).Content;
            KFontSize = int.Parse(fontSize);

            var selectedText = editBox.Document.Selection;

            if (!selectedText.Equals(""))
                selectedText.CharacterFormat.Size = KFontSize;
        }

        private void KForeground_Click(object sender, RoutedEventArgs e)
        {
            InitializeColorPickerWindow();
        }

        private void ToggleBtn_Checked(object sender, RoutedEventArgs e)
        {
            var selectedText = editBox.Document.Selection;

            if (selectedText != null)
            {
                switch ((sender as ToggleButton).Tag)
                {
                    case "BoldBtn":
                        KBold = (bool)kBoldButton.IsChecked;
                        selectedText.CharacterFormat.Bold = KBold ? FormatEffect.On : FormatEffect.Off;
                        break;
                    case "ItalicBtn":
                        KItalic = (bool)kItalicButton.IsChecked;
                        selectedText.CharacterFormat.Italic = KItalic ? FormatEffect.On : FormatEffect.Off;
                        break;
                    case "UnderlineBtn":
                        KUnderline = (bool)kUnderlineButton.IsChecked;
                        selectedText.CharacterFormat.Underline = KUnderline ? UnderlineType.Single : UnderlineType.None;
                        break;
                    case "StrikethroughBtn":
                        KStrikethrough = (bool)kStrikethroughButton.IsChecked;
                        selectedText.CharacterFormat.Strikethrough = KStrikethrough ? FormatEffect.On : FormatEffect.Off;
                        break;
                }
            }
        }

        private void ViewModel_KColorChanged(object sender, ColorChangedEventArgs e)
        {
            kForegroundButton.Background = new SolidColorBrush(e.NewColor);
            KForeground = new SolidColorBrush(e.NewColor);

            var selectedText = editBox.Document.Selection;

            if (!selectedText.Equals(""))
            {
                selectedText.CharacterFormat.ForegroundColor = KForeground.Color;
            }
        }

        private void EditBox_TextChanged(object sender, RoutedEventArgs e)
        {
            KTextChanged?.Invoke(this, ActualText);
        }

        private void EditBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ITextSelection selectedText = editBox.Document.Selection;

        }

        #endregion

        #region OnApplyTemplate

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            editBox = GetTemplateChild("EditBox") as RichEditBox;
            fontFamilyComboBox = GetTemplateChild("KComboBox") as ComboBox;
            fontSizeComboBox = GetTemplateChild("KFontSize") as ComboBox;
            kForegroundButton = GetTemplateChild("KForeground") as Button;
            kBoldButton = GetTemplateChild("KBold") as ToggleButton;
            kItalicButton = GetTemplateChild("KItalic") as ToggleButton;
            kUnderlineButton = GetTemplateChild("KUnderline") as ToggleButton;
            kStrikethroughButton = GetTemplateChild("KStrikethrough") as ToggleButton;

            if (editBox != null)
            {
                editBox.TextChanged += EditBox_TextChanged;
                editBox.SelectionChanged += EditBox_SelectionChanged;
            }

            if (fontFamilyComboBox != null)
            {
                fontFamilyComboBox.SelectionChanged += FamilyFont_SelectionChanged;
            }

            if (fontSizeComboBox != null)
            {
                fontSizeComboBox.SelectionChanged += FontSize_SelectionChanged;
            }

            if (kForegroundButton != null)
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
