using System.Windows;
using System.Windows.Controls;

namespace ELM327_GUI
{
    public partial class InputDialog : Window
    {
        private TextBox inputTextBox;
        public string Result { get; private set; }

        public InputDialog(string prompt, string title, string defaultValue)
        {
            Title = title;
            Width = 300;
            Height = 150;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            var stackPanel = new StackPanel { Margin = new Thickness(10) };

            var promptText = new TextBlock { Text = prompt, Margin = new Thickness(0, 0, 0, 10) };
            stackPanel.Children.Add(promptText);

            inputTextBox = new TextBox { Text = defaultValue };
            stackPanel.Children.Add(inputTextBox);

            var buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
            var okButton = new Button { Content = "OK", Width = 75, Margin = new Thickness(5, 0, 0, 0) };
            okButton.Click += (s, e) => { Result = inputTextBox.Text; DialogResult = true; Close(); };
            var cancelButton = new Button { Content = "Cancel", Width = 75, Margin = new Thickness(5, 0, 0, 0) };
            cancelButton.Click += (s, e) => { DialogResult = false; Close(); };
            buttonPanel.Children.Add(okButton);
            buttonPanel.Children.Add(cancelButton);

            stackPanel.Children.Add(buttonPanel);

            Content = stackPanel;
        }


        public new string ShowDialog()
        {
            return base.ShowDialog() == true ? Result : null;
        }

    }
}