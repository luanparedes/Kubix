using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kubix.Services.Interfaces;
using CommunityToolkit.Mvvm.DependencyInjection;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using Microsoft.UI.Dispatching;
using Windows.Storage;
using System.Linq;

namespace Kubix.ViewModel
{
    public partial class TerminalViewModel : ObservableObject
    {
        private readonly string COMMANDS_FILE = "default_commands.txt";

        public DispatcherQueue DispatcherQueueVM { get; set; }

        private ScrollViewer _scrollViewer;
        private Process _process;
        private int _commandIndex = 0;

        private readonly IDataService _dataService;

        ICommand ExecuteCommand { get; }

        [ObservableProperty]
        private ObservableCollection<string> lastCommands;

        [ObservableProperty]
        private ObservableCollection<string> defaultCommands;

        [ObservableProperty]
        private string terminalOutput;

        [ObservableProperty]
        private string terminalInput;

        [ObservableProperty]
        private string currentDirectory;

        [ObservableProperty]
        private bool isWaitingFinishCommand = false;

        public TerminalViewModel()
        {
            _dataService = Ioc.Default.GetService<IDataService>();
            Teste();
            //CreateCommandsFile();
            //CopyDefaultFileIfNotExistsAsync();
            LastCommands = GetConvertedCommands();
            //DefaultCommands = LoadCommandsAsync().GetAwaiter().GetResult();
            //LoadCommandsAsync();

            CurrentDirectory = Environment.CurrentDirectory;
            ExecuteCommand = new RelayCommand(InitializeTerminal);
        }

        private async void Teste()
        {
            await CopyDefaultFileIfNotExistsAsync();
            await LoadCommandsAsync();
        }

        private ObservableCollection<string> GetConvertedCommands()
        {
            ObservableCollection<string> convertedCommands = new ObservableCollection<string>();
            List<string> commands = _dataService.GetDBCommands();

            foreach (var command in commands)
            {
                convertedCommands.Add(command);
            }

            return convertedCommands;
        }

        private async void InitializeTerminal(object parameter)
        {
            IsWaitingFinishCommand = true;

            if (!string.IsNullOrEmpty(TerminalInput))
            {
                TerminalOutput += $"\n> {TerminalInput}";

                try
                {
                    await Task.Run(async () =>
                    {
                        _process = new Process
                        {
                            StartInfo = new ProcessStartInfo
                            {
                                FileName = "powershell.exe",
                                Arguments = $"-NoLogo -NoProfile -Command \"[Console]::OutputEncoding = [System.Text.Encoding]::UTF8; {TerminalInput}\"",
                                RedirectStandardOutput = true,
                                RedirectStandardError = true,
                                StandardOutputEncoding = Encoding.UTF8,
                                StandardErrorEncoding = Encoding.UTF8,
                                UseShellExecute = false,
                                CreateNoWindow = true,
                                WorkingDirectory = CurrentDirectory
                            }
                        };

                        _process.Start();
                        await GetTerminalOutput(parameter as string);
                    });

                    VerifyChangeFolderCommand();
                }
                catch (Exception ex)
                {
                    TerminalOutput += $"\nError: {ex.Message}";
                }
                finally
                {
                    IsWaitingFinishCommand = false;
                }
                
                TerminalInput = string.Empty;
                _commandIndex = 0;

                _scrollViewer.ChangeView(0, _scrollViewer.ScrollableHeight, null);
            }
        }

        private async Task<bool> GetTerminalOutput(string parameter)
        {
            string result = _process.StandardOutput.ReadToEnd();
            string error = _process.StandardError.ReadToEnd();
            _process.WaitForExit();

            bool resultTask = true;

            DispatcherQueueVM.TryEnqueue(() =>
            {
                if (!string.IsNullOrEmpty(result))
                {
                    TerminalOutput += $"\n{result}";

                    if (!LastCommands.Contains(parameter))
                    {
                        LastCommands.Insert(0, parameter as string);
                        _dataService.InsertIntoCommands(parameter as string);
                    }
                    else
                    {
                        LastCommands.Remove(parameter);
                        LastCommands.Insert(0, parameter as string);
                        _commandIndex--;
                    }
                }
                if (!string.IsNullOrEmpty(error))
                {
                    TerminalOutput += $"\nError: {error}";
                    resultTask = false;
                }
            });
            
            return resultTask;
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

        public async Task CopyDefaultFileIfNotExistsAsync()
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

        private async Task LoadCommandsAsync()
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(COMMANDS_FILE);
            var lines = await FileIO.ReadLinesAsync(file);

            ObservableCollection<string> commands = new ObservableCollection<string>(lines.Where(line => !string.IsNullOrWhiteSpace(line)));

            var sorted = commands.OrderBy(c => c).ToList();
            commands.Clear();

            DefaultCommands = new ObservableCollection<string>();

            foreach (var item in sorted)
                DefaultCommands.Add(item);
        }

        public void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            _scrollViewer = sender as ScrollViewer;
        }

        public void CommandTextBox_KeyDown(object sender, KeyRoutedEventArgs args)
        {
            if (args.Key == Windows.System.VirtualKey.Enter)
            {
                TerminalInput = (args.OriginalSource as TextBox).Text;
                ExecuteCommand.Execute(TerminalInput);
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

        public async Task ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems[0] is string command)
            {
                TerminalInput = command;
            }
        }
    }

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
}
