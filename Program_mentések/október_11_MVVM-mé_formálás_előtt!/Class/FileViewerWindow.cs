using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ELM327_GUI
{
    public class FileViewerWindow : Window
    {
        public FileViewerWindow(string filePath)
        {
            Title = "PID parancsok";
            Width = 550;
            Height = 450;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            var scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                Margin = new Thickness(10)
            };

            var textBox = new TextBox
            {
                IsReadOnly = true,
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto
            };

            if (File.Exists(filePath))
            {
                
                //string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "FULL_PID_list_with_explanation.txt");
                textBox.Text = File.ReadAllText(filePath);
                MessageBox.Show($"Kapott fájlútvonal: {filePath}");

            }
            else
            {
                textBox.Text = "A fájl nem található!";
            }

            scrollViewer.Content = textBox;
            Content = scrollViewer;
        }
    }
}

