using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Security.Policy;
using System.Windows;
using System.Windows.Input;
using ELM327_GUI.MVVM.View;

namespace ELM327_GUI.MVVM.ViewModel
{
    public class PIDCommandWindowViewModel : ObservableObject
    {
        private string _commandText;

        public string CommandText
        {
            get => _commandText;
            set => SetProperty(ref _commandText, value);
        }

        public ICommand SendCommand { get; }
        public ICommand ShowListCommand { get; }

        public event Action<string> CommandEntered;
        public event Action RequestClose;

        public PIDCommandWindowViewModel()
        {
            SendCommand = new RelayCommand(ExecuteSend);
            ShowListCommand = new RelayCommand(ExecuteShowList);
        }

        private void ExecuteSend()
        {
            string command = CommandText?.Trim();
            if (string.IsNullOrEmpty(command))
                return;

            if (string.Equals(command, "exit", StringComparison.OrdinalIgnoreCase))
            {
                RequestClose?.Invoke();
                return;
            }

            CommandEntered?.Invoke(command);
            CommandText = string.Empty;
        }
        private void ExecuteShowList()
        {
            string filePath = System.IO.Path.Combine(
                    System.AppDomain.CurrentDomain.BaseDirectory, "Files", "FULL_PID_list_with_explanation.txt");
            var viewer = new FileViewerWindow(filePath);
            viewer.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            viewer.Owner = Application.Current.MainWindow;
            viewer.ShowDialog();
        }
    }
}