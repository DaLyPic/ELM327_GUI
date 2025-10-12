using System.Windows;

namespace ELM327_GUI
{
    public partial class InputDialog : Window
    {
        public InputDialog(string message, Window owner, FrameworkElement target)
        {
            InitializeComponent();
            MessageText.Text = message;
            Owner = owner;

            // Center over target element
            var point = target.PointToScreen(new Point(0, 0));
            var width = target.ActualWidth;
            var height = target.ActualHeight;

            Left = point.X + (width - Width) / 2;
            Top = point.Y + (height - Height) / 2;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}