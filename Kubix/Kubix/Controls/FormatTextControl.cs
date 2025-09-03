using Kubix.Helpers;
using Kubix.View;
using Microsoft.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace Kubix.Controls
{
    public class FormatTextControl : Control
    {
        #region Fields & Properties

        private ComboBox fontFamilyComboBox;
        private ComboBox fontSizeComboBox;
        private ToggleButton kForegroundButton;
        private ToggleButton kBoldButton;
        private ToggleButton kItalicButton;
        private ToggleButton kUnderlineButton;
        private ToggleButton kStrikethroughButton;
        private CreateFileEnum fileEnum;
        private bool _hasFormatChange = false;

        public event EventHandler<string> KTextChanged;
        public event EventHandler<bool> HasChanged;
        public event EventHandler<bool> KColorPickerOpenChanged;

        public string ActualTextDocument;
        public string InitialText { get; set; } = string.Empty;
        public string FileType { get; set; }

        private bool _hasChanges = false;
        public bool HasChanges
        { 
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    HasChanged?.Invoke(this, value);
                }
            }
        }

        #endregion

        #region Dependency Properties

        public RichEditBox EditBox
        {
            get { return (RichEditBox)GetValue(RichDocumentProperty); }
            set { SetValue(RichDocumentProperty, value); }
        }

        public static readonly DependencyProperty RichDocumentProperty =
            DependencyProperty.Register(nameof(EditBox), typeof(RichEditBox), typeof(FormatTextControl), new PropertyMetadata(null));

        public StorageFile TabFile
        {
            get { return (StorageFile)GetValue(TabFileProperty); }
            set { SetValue(TabFileProperty, value); }
        }

        public static readonly DependencyProperty TabFileProperty =
            DependencyProperty.Register(nameof(TabFile), typeof(StorageFile), typeof(FormatTextControl), new PropertyMetadata(null));

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
            DependencyProperty.Register(nameof(KFontSize), typeof(int), typeof(FormatTextControl), new PropertyMetadata(10));

        public SolidColorBrush KForeground
        {
            get { return (SolidColorBrush)GetValue(KForegroundProperty); }
            set { SetValue(KForegroundProperty, value); }
        }

        public static readonly DependencyProperty KForegroundProperty =
            DependencyProperty.Register(nameof(KForeground), typeof(SolidColorBrush), typeof(FormatTextControl), new PropertyMetadata(new SolidColorBrush(Colors.AliceBlue), OnKForegroundChanged));

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
            this.ActualTextDocument = text;
        }

        private void FormatTextControl_Loaded(object sender, RoutedEventArgs e)
        {
            CreateNote();
            InitializeFormat(TextConstants.MaxUnitCount);
            ShowInitialValues();

            this.kForegroundButton.Background = KForeground;

            HasChanges = false;
            Loaded -= FormatTextControl_Loaded;
        }

        #endregion

        #region Methods

        private void CreateNote()
        {
            if (EditBox != null)
            {
                switch (fileEnum)
                {
                    case CreateFileEnum.NewFile:
                        EditBox.Document.GetText(TextGetOptions.None, out ActualTextDocument);
                        break;
                    case CreateFileEnum.OpenFile:
                        EditBox.Document.SetText(TextSetOptions.None, ActualTextDocument);
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
            HasChanges = false;

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
                        EditBox.Document.GetText(TextGetOptions.None, out text);
                        break;
                    case ".rtf":
                        EditBox.Document.GetText(TextGetOptions.FormatRtf, out text);
                        break;
                }

                await FileIO.WriteTextAsync(TabFile, text);
            }
            else
            {
                var savePicker = new FileSavePicker();

                savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                savePicker.FileTypeChoices.Add("Text File", new List<string>() { ".txt" });
                savePicker.FileTypeChoices.Add("Text Formated File", new List<string>() { ".rtf" });
                savePicker.SuggestedFileName = Stringer.GetString("KB_NewDocumentText");

                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Instance.MainWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hwnd);

                TabFile = await savePicker.PickSaveFileAsync();

                if (TabFile != null)
                {
                    switch (TabFile.FileType)
                    {
                        case ".txt":
                            EditBox.Document.GetText(TextGetOptions.None, out text);
                            break;
                        case ".rtf":
                            EditBox.Document.GetText(TextGetOptions.FormatRtf, out text);
                            break;
                    }

                    await FileIO.WriteTextAsync(TabFile, text);
                }
            }

            if (TabFile == null)
                return TabFile;

            _hasFormatChange = false;
            InitialText = EditBox.Document.ToString();

            return TabFile;
        }

        private async Task<bool> IsInitialText()
        {
            if (EditBox != null)
            {
                string initialText = InitialText;
                string text;

                EditBox.Document.GetText(TextGetOptions.None, out text);

                if (FileType == ".rtf")
                {
                    initialText = await ConvertRtfStringToPlainText(InitialText);
                }

                initialText = initialText.Replace("\r", "");
                initialText = initialText.Replace("\n", "");
                text = text.Replace("\r", "");
                text = text.Replace("\n", "");

                return initialText.Equals(text);
            }

            return false;
        }

        private async Task<string> ConvertRtfStringToPlainText(string rtfContent)
        {
            if (string.IsNullOrEmpty(rtfContent))
            {
                return string.Empty;
            }

            var tempRichEditBox = new RichEditBox();
            var richTextDocument = tempRichEditBox.Document;

            using (var stream = new InMemoryRandomAccessStream())
            {
                using (var dataWriter = new DataWriter(stream))
                {
                    dataWriter.WriteString(rtfContent);
                    await dataWriter.StoreAsync();
                    await dataWriter.FlushAsync();
                    stream.Seek(0);

                    richTextDocument.LoadFromStream(TextSetOptions.FormatRtf, stream);
                }
            }

            string plainText;
            richTextDocument.GetText(TextGetOptions.None, out plainText);

            return plainText;
        }

        private void InitializeFormat(int count = 100)
        {
            var format = EditBox.Document.Selection;
            var selectedText = EditBox.Document.GetRange(0, count);

            selectedText.CharacterFormat.Name = KFontFamily.Source;
            selectedText.CharacterFormat.Size = KFontSize;
            selectedText.CharacterFormat.ForegroundColor = KForeground.Color;
            selectedText.CharacterFormat.Bold = KBold ? FormatEffect.On : FormatEffect.Off;
            selectedText.CharacterFormat.Italic = KItalic ? FormatEffect.On : FormatEffect.Off;
            selectedText.CharacterFormat.Underline = KUnderline ? UnderlineType.Single : UnderlineType.None;
            selectedText.CharacterFormat.Strikethrough = KStrikethrough ? FormatEffect.On : FormatEffect.Off;

            EditBox.Document.Selection.SetRange(0, count);

            HasChanges = false;
        }

        private void ShowInitialValues()
        {
            foreach (ComboBoxItem item in fontFamilyComboBox.Items)
            {
                if (KFontFamily.Source.Contains(item.Content.ToString()))
                    fontFamilyComboBox.SelectedItem = item;
            }

            foreach (ComboBoxItem item in fontSizeComboBox.Items)
            {
                if (item.Content.ToString().Equals(KFontSize.ToString()))
                    fontSizeComboBox.SelectedItem = item;
            }
        }

        private void FormatText(ToggleButton sender)
        {
            var selectedText = EditBox.Document.Selection;

            if (selectedText != null)
            {
                switch (sender.Tag)
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

                _hasFormatChange = true;
            }
        }

        private List<ComboBoxItem> LoadFonts()
        {
            InstalledFontCollection fonts = new InstalledFontCollection();

            List<ComboBoxItem> comboItems = new List<ComboBoxItem>();

            var fontNames = fonts.Families
                         .Select(f => f.Name)
                         .OrderBy(name => name)
                         .ToList();

            foreach (var font in fontNames)
            {
                comboItems.Add(new ComboBoxItem() { Content = font });
            }

            return comboItems;
        }

        #endregion

        #region Event Handlers

        private void FamilyFont_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string fontName = (string)(e.AddedItems[0] as ComboBoxItem).Content;
            KFontFamily = new FontFamily(fontName);

            var selectedText = EditBox.Document.Selection;

            if (!selectedText.Equals(""))
            {
                selectedText.CharacterFormat.Name = KFontFamily.Source;

                if (selectedText.Text.Count() > 0)
                    HasChanges = true;
            }
        }

        private void FontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string fontSize = (string)(e.AddedItems[0] as ComboBoxItem).Content;
            KFontSize = int.Parse(fontSize);

            var selectedText = EditBox.Document.Selection;

            if (!selectedText.Equals(""))
            {
                selectedText.CharacterFormat.Size = KFontSize;
                HasChanges = true;
            }
        }

        private void KForeground_Click(object sender, RoutedEventArgs e)
        {
            KColorPickerOpenChanged?.Invoke(kForegroundButton, (bool)kForegroundButton.IsChecked);
        }

        private void ToogleBtn_Click(object sender, RoutedEventArgs e)
        {
            FormatText(sender as ToggleButton);
        }

        private async void EditBox_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!await IsInitialText() || _hasFormatChange)
                HasChanges = true;
            else
                HasChanges = false;

            KTextChanged?.Invoke(this, ActualTextDocument);
        }

        private void EditBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ITextSelection selectedText = EditBox.Document.Selection;

            if (selectedText != null)
            {
                var format = selectedText.CharacterFormat;

                kBoldButton.IsChecked = format.Bold == FormatEffect.On;
                kItalicButton.IsChecked = format.Italic == FormatEffect.On;
                kUnderlineButton.IsChecked = format.Underline == UnderlineType.Single;
                kStrikethroughButton.IsChecked = format.Strikethrough == FormatEffect.On;
            }
        }

        private static void OnKForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FormatTextControl format)
            {
                format.kForegroundButton.Background = format.KForeground;
                var selectedText = format.EditBox.Document.Selection;

                selectedText.CharacterFormat.ForegroundColor = format.KForeground.Color;

            }
        }

        #endregion

        #region OnApplyTemplate

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            EditBox = GetTemplateChild("EditBox") as RichEditBox;
            fontFamilyComboBox = GetTemplateChild("KComboBox") as ComboBox;
            fontSizeComboBox = GetTemplateChild("KFontSize") as ComboBox;
            kForegroundButton = GetTemplateChild("KForeground") as ToggleButton;
            kBoldButton = GetTemplateChild("KBold") as ToggleButton;
            kItalicButton = GetTemplateChild("KItalic") as ToggleButton;
            kUnderlineButton = GetTemplateChild("KUnderline") as ToggleButton;
            kStrikethroughButton = GetTemplateChild("KStrikethrough") as ToggleButton;

            if (EditBox != null)
            {
                EditBox.TextChanged += EditBox_TextChanged;
                EditBox.SelectionChanged += EditBox_SelectionChanged;
            }

            if (fontFamilyComboBox != null)
            {
                fontFamilyComboBox.SelectionChanged += FamilyFont_SelectionChanged;
                fontFamilyComboBox.ItemsSource = LoadFonts();
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
                kBoldButton.Click += ToogleBtn_Click;
            }

            if (kItalicButton != null)
            {
                kItalicButton.Click += ToogleBtn_Click;
            }

            if (kUnderlineButton != null)
            {
                kUnderlineButton.Click += ToogleBtn_Click;
            }

            if (kStrikethroughButton != null)
            {
                kStrikethroughButton.Click += ToogleBtn_Click;
            }
        }

        #endregion
    }
}
