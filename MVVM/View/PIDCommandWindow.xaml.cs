using ELM327_GUI.MVVM.ViewModel;
using System;
using System.Windows;


namespace ELM327_GUI.MVVM.View
{
    public partial class PIDCommandWindow : Window
    {
        public event Action<string> PIDCommandEntered;
        public PIDCommandWindow(Action<string> commandHandler)
        {
            InitializeComponent();
            var vm = new PIDCommandWindowViewModel();
            this.DataContext = vm;

            vm.CommandEntered += commandHandler;
            vm.RequestClose += () => this.Close();

            this.Loaded += (s, e) => PIDCommandTextBox.Focus();
        }
    }
}
