using ELM327_GUI.Methods;
using ELM327_GUI.MVVM.View;
using Microsoft.VisualBasic;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using IOPath = System.IO.Path;

namespace ELM327_GUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CultureInfo magyarKultura = new CultureInfo("hu-HU");

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) =>
            {
                string datum = DateTime.Now.ToString("yyyy. MMMM dd.", magyarKultura);
                string nap = DateTime.Now.ToString("dddd", magyarKultura);
                dateTextBlock.Text = $"{datum}\n{nap}";
            };
            timer.Start();

            // Portok betöltése a ComboBox-ba
            //LoadSerialPorts();
        }
        private SerialPort serialPort;


        private List<string> response = new List<string>();

        //private void LoadSerialPorts()
        //{
        //    PortSelector.ItemsSource = SerialPort.GetPortNames();
        //    if (PortSelector.Items.Count > 0)
        //        PortSelector.SelectedIndex = 0;
        //}
        private void ATcommandButton_Click(object sender, RoutedEventArgs e)
        {
           
            var selectorWindow = new PortSelectorWindow();
            selectorWindow.Owner = this;
            bool? result = selectorWindow.ShowDialog();

            if (result != true || selectorWindow.SelectedPort == null)
            {
                MessageBox.Show("Nem választottál portot.");
                return;
            }

            string selectedPort = selectorWindow.SelectedPort;

            try
            {
                if (serialPort != null && serialPort.IsOpen)
                    serialPort.Close();

                serialPort = new SerialPort(selectedPort, 115200);
                serialPort.DataBits = 8;
                serialPort.Parity = Parity.None;
                serialPort.StopBits = StopBits.One;
                serialPort.Handshake = Handshake.None;
                serialPort.ReadTimeout = 3000;
                serialPort.WriteTimeout = 3000;
                serialPort.NewLine = "\r";
                serialPort.Open();

                MessageBox.Show($"A port {selectedPort} megnyitva.");

                var commandWindow = new CommandWindow();
                commandWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                commandWindow.Owner = this;

                commandWindow.CommandEntered += (cmd) =>
                {
                    
                    SendATCommand(cmd);
                };

                commandWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt a port megnyitásakor: {ex.Message}");
            }
        }

        private void SendATCommand(string command)
        {
            if (serialPort == null || !serialPort.IsOpen)
            {
                MessageBox.Show("A port nincs megnyitva!");
                return;
            }

            try
            {
                serialPort.WriteLine(command);
                // Válasz olvasása
                string resp = serialPort.ReadLine();
                MessageBox.Show($"Válasz: {resp}");

                // Save response to array
                response.Add(resp);

                // Save response to file
                string filePath = System.IO.Path.Combine(
                    System.AppDomain.CurrentDomain.BaseDirectory, "response.txt");
                File.AppendAllText(filePath, resp + Environment.NewLine);
            }
            catch (TimeoutException)
            {
                MessageBox.Show("Nincs válasz az ELM327 eszköztől (időtúllépés).");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba az AT parancs küldésekor: {ex.Message}");
            }
        }

        private void ParsingButton_Click(object sender, RoutedEventArgs e)
        {
            string result = ParserMethods.RunParser(this);
            MessageBox.Show(result, "Parsing Result");
        }

        //private void ParsingButton_Click(object sender, RoutedEventArgs e)
        //{
        //    string result = ParserMethods.RunParser(this);
        //    txtParsingOutput.Text = result;
        //    // MessageBox eltávolítva
        //}

        private void UdsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("UDS kompatibilitás ellenőrzés");
        }
        private void HowtouseButton_Click(object sender, RoutedEventArgs e)
        {
            // Path to the PDF in the Files folder next to the executable
            string pdfPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Files", "HowToUse.pdf");
            if (File.Exists(pdfPath))
            {
                Process.Start(pdfPath);
            }
            else
            {
                MessageBox.Show("HowToUse.pdf not found in Files folder.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OBDsetting_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("OBD eszköz beállítás gombra kattintottál. Itt konfigurálhatod a portot, baud rate-et stb.");
        }

        private void PDFreportgen_Click(object sender, RoutedEventArgs e)
        {
            string responseFile = IOPath.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "response.txt");
            if (!File.Exists(responseFile))
            {
                MessageBox.Show("response.txt nem található.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string[] lines = File.ReadAllLines(responseFile);

            // PDFSharp PDF generálás
            PdfDocument document = new PdfDocument();
            document.Info.Title = "ELM327 AT Command Responses";

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Arial", 12);

            double margin = 40;
            double lineHeight = 20;
            double yPoint = margin;
            double usableHeight = page.Height.Point - margin;

            foreach (string line in lines)
            {
                gfx.DrawString(
                    line,
                    font,
                    XBrushes.Black,
                    new XRect(
                        margin,
                        yPoint,
                        page.Width.Point - 2 * margin,
                        usableHeight
                    ),
                    XStringFormats.TopLeft
                );

                yPoint += lineHeight;

                if (yPoint > page.Height.Point - margin)
                {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    yPoint = margin;
                }
            }

            string pdfPath = IOPath.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "response_report.pdf");
            document.Save(pdfPath);
            MessageBox.Show($"PDF riport elkészült: {pdfPath}", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public class CustomFontResolver : IFontResolver
        {
            public byte[] GetFont(string faceName)
            {
                // Load font data from resources or files
                return File.ReadAllBytes("E:\\Git\\source\\repo\\ELM327_GUI\\Fonts\\Poppins-Medium.ttf");
            }

            public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
            {
                // Map the requested font to a specific font file
                return new FontResolverInfo("Poppins-Thin");
            }
        }

        private void Commandsfromfile_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Parancsok fájlból betöltése. Itt olvashatsz be AT vagy PID parancsokat egy fájlból.");
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        // Helper to get the cell FrameworkElement
        private FrameworkElement GetGridCell(Grid grid, int row, int column)
        {
            foreach (UIElement element in grid.Children)
            {
                if (Grid.GetRow(element) == row && Grid.GetColumn(element) == column)
                    return element as FrameworkElement;
            }
            return null;
        }

        private void AutodataButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new InputDialog("Enter data:", "Autó adatai", "");
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dialog.Owner = this;
            // Get the position of the Grid cell
            //var cell = GetGridCell(MainGrid, 1, 1);
            //if (cell != null)
            //{
            //      var point = cell.PointToScreen(new Point(cell.ActualWidth / 2, cell.ActualHeight / 2));
            //    dialog.WindowStartupLocation = WindowStartupLocation.Manual;
            //    dialog.Left = point.X - dialog.Width / 2;
            //    dialog.Top = point.Y - dialog.Height / 2;
            //}
            dialog.ShowDialog();
        }

        private void PIDcommandButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new InputDialog2("Szeretnél közvetlenül parancsot beírni, vagy szeretnél egy segédletet, azaz egy teljes listát a parancsokkal?", "PID parancsok");
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dialog.Owner = this;
            dialog.ShowDialog();
        }
    }
}
