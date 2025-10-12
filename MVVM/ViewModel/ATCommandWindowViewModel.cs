using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ELM327_GUI.MVVM.ViewModel
{
    public class ATCommandWindowViewModel : ObservableObject
    {
        private string _commandText;

        public string CommandText
        {
            get => _commandText;
            set => SetProperty(ref _commandText, value);
        }

        public ICommand SendCommand { get; }

        public event Action<string> CommandEntered;
        public event Action RequestClose;

        public ATCommandWindowViewModel()
        {
            SendCommand = new RelayCommand(ExecuteSend);
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
    }

}
