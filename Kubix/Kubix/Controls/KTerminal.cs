using CommunityToolkit.Mvvm.DependencyInjection;
using Kubix.Helpers;
using Kubix.Services.Interfaces;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace Kubix.Controls
{
    public class KTerminal : Control
    {
        #region Constants

        private readonly string COMMANDS_FILE = "default_commands.txt";

        #endregion

        #region Fields & Properties

        private IDataService _dataService;

        private ScrollViewer _scrollViewer;
        private TextBox _commandTextBox;
        private ListView _latestListView;
        private ListView _defaultListView;
        private Process _process;
        private int _commandIndex = 0;
        private bool isLoaded = false;

        public ObservableCollection<string> LastCommands
        {
            get { return (ObservableCollection<string>)GetValue(LastCommandsProperty); }
            set { SetValue(LastCommandsProperty, value); }
        }

        public static readonly DependencyProperty LastCommandsProperty =
           DependencyProperty.Register(nameof(LastCommands), typeof(ObservableCollection<string>), typeof(KTerminal), new PropertyMetadata(new ObservableCollection<string>()));

        public ObservableCollection<string> DefaultCommands
        {
            get { return (ObservableCollection<string>)GetValue(DefaultCommandsProperty); }
            set { SetValue(DefaultCommandsProperty, value); }
        }

        public static readonly DependencyProperty DefaultCommandsProperty =
           DependencyProperty.Register(nameof(DefaultCommands), typeof(ObservableCollection<string>), typeof(KTerminal), new PropertyMetadata(new ObservableCollection<string>()));

        public string TerminalOutput
        {
            get { return (string)GetValue(TerminalOutputProperty); }
            set { SetValue(TerminalOutputProperty, value); }
        }

        public static readonly DependencyProperty TerminalOutputProperty =
            DependencyProperty.Register(nameof(TerminalOutput), typeof(string), typeof(KTerminal), new PropertyMetadata(string.Empty));

        public string TerminalInput
        {
            get { return (string)GetValue(TerminalInputProperty); }
            set { SetValue(TerminalInputProperty, value); }
        }

        public static readonly DependencyProperty TerminalInputProperty =
            DependencyProperty.Register(nameof(TerminalInput), typeof(string), typeof(KTerminal), new PropertyMetadata(string.Empty));

        public string CurrentDirectory
        {
            get { return (string)GetValue(CurrentDirectoryProperty); }
            set { SetValue(CurrentDirectoryProperty, value); }
        }

        public static readonly DependencyProperty CurrentDirectoryProperty =
            DependencyProperty.Register(nameof(CurrentDirectory), typeof(string), typeof(KTerminal), new PropertyMetadata("Vamos ver"));

        public bool IsWaitingFinishCommand
        {
            get { return (bool)GetValue(IsWaitingFinishCommandProperty); }
            set { SetValue(IsWaitingFinishCommandProperty, value); }
        }

        public static readonly DependencyProperty IsWaitingFinishCommandProperty =
            DependencyProperty.Register(nameof(IsWaitingFinishCommand), typeof(bool), typeof(KTerminal), new PropertyMetadata(false));

        #endregion

        #region Strings

        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
        }

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register(nameof(PlaceholderText), typeof(string), typeof(KTerminal), new PropertyMetadata(Stringer.GetString("KB_TypeAndEnter")));

        public string LatestCommandsText
        {
            get { return (string)GetValue(LatestCommandsTextProperty); }
            set { SetValue(LatestCommandsTextProperty, value); }
        }

        public static readonly DependencyProperty LatestCommandsTextProperty =
            DependencyProperty.Register(nameof(LatestCommandsText), typeof(string), typeof(KTerminal), new PropertyMetadata(Stringer.GetString("KB_LastCommands")));

        public string DefaultCommandsText
        {
            get { return (string)GetValue(DefaultCommandsTextProperty); }
            set { SetValue(DefaultCommandsTextProperty, value); }
        }

        public static readonly DependencyProperty DefaultCommandsTextProperty =
            DependencyProperty.Register(nameof(DefaultCommandsText), typeof(string), typeof(KTerminal), new PropertyMetadata(Stringer.GetString("KB_DefaultCommands")));

        #endregion

        #region OnApplyTemplate

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _dataService = Ioc.Default.GetService<IDataService>();
            InitializeControl();

            _scrollViewer = GetTemplateChild("MainScrollViewer") as ScrollViewer;
            _commandTextBox = GetTemplateChild("CommandTextBox") as TextBox;
            _latestListView = GetTemplateChild("LatestListView") as ListView;
            _defaultListView = GetTemplateChild("DefaultListView") as ListView;

            if (_scrollViewer != null)
            {
                _scrollViewer.Loaded += ScrollViewer_Loaded;
            }

            if (_commandTextBox != null)
            {
                _commandTextBox.KeyDown += CommandTextBox_KeyDown;
            }

            if (_latestListView != null)
            {
                _latestListView.SelectionChanged += ListView_SelectionChanged;
                _latestListView.ItemsSource = LastCommands;
            }

            if (_defaultListView != null)
            {
                _defaultListView.SelectionChanged += ListView_SelectionChanged;
                _defaultListView.ItemsSource = DefaultCommands;
            }
        }

        #endregion

        #region Methods

        private void InitializeControl()
        {
            CopyDefaultFileIfNotExistsAsync();
            LoadLatestCommandsAsync();
            LoadCommandsAsync();

            CurrentDirectory = Environment.CurrentDirectory;
        }

        private async void InitializeTerminal(string command)
        {
            IsWaitingFinishCommand = true;
            TerminalOutput += $"\n> {TerminalInput}";

            await Task.Run(() =>
            {
                try
                {
                    ProcessStartInfo StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-NoLogo -NoProfile -Command \"[Console]::OutputEncoding = [System.Text.Encoding]::UTF8; {command}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        StandardOutputEncoding = Encoding.UTF8,
                        StandardErrorEncoding = Encoding.UTF8,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WorkingDirectory = Environment.CurrentDirectory
                    };

                    using (Process process = new Process() { StartInfo = StartInfo })
                    {
                        process.Start();

                        string output = process.StandardOutput.ReadToEnd();
                        string error = process.StandardError.ReadToEnd();
                        process.WaitForExit();

                        if (this.DispatcherQueue != null)
                        {
                            this.DispatcherQueue.TryEnqueue(() =>
                            {
                                VerifyChangeFolderCommand();
                                GetTerminalOutput(output, error, command);
                            });
                        }
                    }
                }
                catch (Exception e)
                {

                }
            });

            IsWaitingFinishCommand = false;
            TerminalInput = string.Empty;
            _commandIndex = 0;
            _scrollViewer.ChangeView(0, _scrollViewer.ScrollableHeight, null);    
        }

        private void GetTerminalOutput(string result, string error, string command)
        {
            if (!string.IsNullOrEmpty(result))
            {
                TerminalOutput += $"\n{result}";

                if (!LastCommands.Contains(command))
                {
                    LastCommands.Insert(0, command);
                    _dataService.InsertIntoCommands(command);
                }
                else
                {
                    _latestListView.SelectionChanged -= ListView_SelectionChanged;

                    LastCommands.Remove(command);
                    LastCommands.Insert(0, command);
                    _commandIndex--;
                    _latestListView.SelectedIndex = -1;
                    Task.Delay(1);

                    _latestListView.SelectionChanged += ListView_SelectionChanged;
                }
            }
            if (!string.IsNullOrEmpty(error))
            {
                TerminalOutput += $"\nError: {error}";
            }
        }

        private bool VerifyChangeFolderCommand()
        {
            if (TerminalInput.StartsWith("cd ", StringComparison.OrdinalIgnoreCase))
            {
                string newPath = TerminalInput.Substring(3).Trim();
                try
                {
                    string fullPath = Path.GetFullPath(newPath, CurrentDirectory);
                    if (Directory.Exists(fullPath))
                    {
                        CurrentDirectory = fullPath;
                        LastCommands.Insert(0, TerminalInput);
                        _dataService.InsertIntoCommands(TerminalInput);
                    }
                    else
                    {
                        TerminalOutput += $"\nThe system cannot find the path specified: {newPath}";
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    TerminalOutput += $"\nError changing directory: {ex.Message}";
                    return false;
                }
            }

            return false;
        }

        public async void CopyDefaultFileIfNotExistsAsync()
        {
            var localFolder = ApplicationData.Current.LocalFolder;

            bool fileExists = false;
            try
            {
                await localFolder.GetFileAsync(COMMANDS_FILE);
                fileExists = true;
            }
            catch
            {
                fileExists = false;
            }

            if (!fileExists)
            {
                StorageFile sourceFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/{COMMANDS_FILE}"));
                await sourceFile.CopyAsync(localFolder, COMMANDS_FILE, NameCollisionOption.ReplaceExisting);
            }
        }

        private async void LoadCommandsAsync()
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(COMMANDS_FILE);
            var lines = await FileIO.ReadLinesAsync(file);

            List<string> commands = new List<string>(lines.Where(line => !string.IsNullOrWhiteSpace(line)));

            var sorted = commands.OrderBy(c => c).ToList();
            commands.Clear();

            foreach (var item in sorted)
                DefaultCommands.Add(item);
        }

        private void LoadLatestCommandsAsync()
        {
            this.DispatcherQueue.TryEnqueue(() =>
            {
                List<string> commands = _dataService.GetDBCommands();

                foreach (var command in commands)
                {
                    LastCommands.Add(command);
                }
            });
        }

        #endregion

        #region Event Handlers

        public void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            _scrollViewer = sender as ScrollViewer;
        }

        public void CommandTextBox_KeyDown(object sender, KeyRoutedEventArgs args)
        {
            if (args.Key == Windows.System.VirtualKey.Enter)
            {
                TerminalInput = (args.OriginalSource as TextBox).Text;
                InitializeTerminal(TerminalInput);
                args.Handled = true;
            }

            if (args.Key == Windows.System.VirtualKey.Up)
            {
                try
                {
                    if (_commandIndex == -1)
                        _commandIndex = 0;

                    TerminalInput = LastCommands[_commandIndex];
                    _commandIndex++;
                }
                catch (Exception ex)
                {
                    TerminalInput = string.Empty;
                    _commandIndex = 0;
                }
            }

            if (args.Key == Windows.System.VirtualKey.Down)
            {
                TerminalInput = string.Empty;
                _commandIndex = 0;
            }
        }

        public void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems[0] is string command)
            {
                TerminalInput = command;
            }
        }

        #endregion      
    }

    #region RelayCommand Class

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged;
    }

    #endregion
}
