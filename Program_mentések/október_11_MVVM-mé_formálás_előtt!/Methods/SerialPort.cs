using System;
using System.IO;
using System.IO.Ports;
using System.Windows;
using System.Xml.Linq;


namespace ELM327_GUI.Methods
{
    internal class SerialPortHandler
    {
        private void serialPort()
        {
            string[] ports = SerialPort.GetPortNames();
            if (SerialPort.GetPortNames().Length == 0)
            {
                MessageBox.Show("Nincs elérhető COM port a gépen.", "Figyelmeztetés", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }
    }
}
