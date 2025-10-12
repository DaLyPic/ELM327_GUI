using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ELM327_GUI;

namespace ELM327_GUI.MVVM.View
{
    public partial class CommandWindow : Window
    {
        // Esemény, amely a beírt parancsot továbbítja
        public event Action<string> CommandEntered;

        public CommandWindow()
        {
            InitializeComponent();
            CommandTextBox.Focus();
        }

        private void SendCommand()
        {
            string command = CommandTextBox.Text.Trim();
            if (string.IsNullOrEmpty(command))
                return;

            if (command.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                this.Close();
                return;
            }
            
            CommandEntered?.Invoke(command);
            CommandTextBox.Clear();
            CommandTextBox.Focus();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendCommand();
        }

        private void CommandTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendCommand();
                e.Handled = true; // Ne csináljon más Enter eseményt
            }
        }
    }
}

   