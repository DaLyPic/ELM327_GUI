using System.IO.Ports;
using System.Windows;
using ELM327_GUI.MVVM.ViewModel;

namespace ELM327_GUI.MVVM.View
{
    public partial class PortSelectorWindow : Window
    {
        public string SelectedPort { get; private set; }

        public PortSelectorWindow()
        {
            InitializeComponent();
            PortComboBox.ItemsSource = SerialPort.GetPortNames();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedPort = PortComboBox.SelectedItem as string;
            DialogResult = true;
            Close();
        }
    }
}