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
using System.Linq;
using System.Reflection.Metadata;
using System.Drawing.Text;

namespace Kubix.ViewModel
{
    public partial class TerminalViewModel : ObservableObject
    {
        private ScrollViewer _scrollViewer;
        private Process _process;
        private List<string> _lastCommands;
        private int _commandIndex = 0;

        ICommand ExecuteCommand { get; }

        [ObservableProperty]
        private string terminalOutput;

        [ObservableProperty]
        private string terminalInput;

        [ObservableProperty]
        private string currentDirectory;

        public TerminalViewModel()
        {
            CurrentDirectory = Environment.CurrentDirectory;
            TerminalOutput = $"{CurrentDirectory}>";
            ExecuteCommand = new RelayCommand(InitializeTerminal);

            _lastCommands = new List<string>();
        }

        private void InitializeTerminal(object parameter)
        {    
            if (!string.IsNullOrEmpty(TerminalInput))
            {
                TerminalOutput += $"\n> {TerminalInput}";

                try
                {
                    _process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "cmd.exe",
                            Arguments = $"/C {TerminalInput}",
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

                    GetTerminalOutput(parameter as string);
                    VerifyChangeFolderCommand();
                }
                catch (Exception ex)
                {
                    TerminalOutput += $"\nError: {ex.Message}";
                }

                TerminalInput = string.Empty;
                _scrollViewer.ChangeView(0, _scrollViewer.ScrollableHeight, null);
            }
        }

        private bool GetTerminalOutput(string parameter)
        {
            string result = _process.StandardOutput.ReadToEnd();
            string error = _process.StandardError.ReadToEnd();
            _process.WaitForExit();

            if (!string.IsNullOrEmpty(result))
            {
                TerminalOutput += $"\n{result}";

                if (!_lastCommands.Contains(parameter))
                {
                    _lastCommands.Insert(0, parameter as string);
                }
                else
                {
                    _lastCommands.RemoveAll(x => x.Equals(parameter));
                    _lastCommands.Insert(0, parameter as string);
                    _commandIndex--;
                }
            }
            if (!string.IsNullOrEmpty(error))
            {
                TerminalOutput += $"\nError: {error}";
                return false;
            }

            return true;
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

                if (ExecuteCommand.CanExecute(null))
                {
                    ExecuteCommand.Execute(null);
                }

                args.Handled = true;
            }

            if (args.Key == Windows.System.VirtualKey.Up)
            {
                try
                {
                    TerminalInput = _lastCommands[_commandIndex];
                    _commandIndex++;
                }
                catch(Exception ex)
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
