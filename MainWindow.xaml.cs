using PdfSharp.Fonts;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ELM327_GUI.MVVM.ViewModel;

namespace ELM327_GUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(); // Az új aggregátor ViewModel

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
        }
        public class CustomFontResolver : IFontResolver
        {
            public byte[] GetFont(string faceName)
            {
                return File.ReadAllBytes("E:\\Git\\source\\repo\\ELM327_GUI\\Fonts\\Poppins-Medium.ttf");
            }

            public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
            {
                return new FontResolverInfo("Poppins-Thin");
            }
        }
        private FrameworkElement GetGridCell(Grid grid, int row, int column)
        {
            foreach (UIElement element in grid.Children)
            {
                if (Grid.GetRow(element) == row && Grid.GetColumn(element) == column)
                    return element as FrameworkElement;
            }
            return null;
        }
    }
}
