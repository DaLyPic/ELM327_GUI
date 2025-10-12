using ELM327_GUI.MVVM.ViewModel;
using System;
using System.Windows;

namespace ELM327_GUI.MVVM.View
{
    public partial class ATCommandWindow : Window
    {
        public ATCommandWindow(Action<string> commandHandler)
        {
            InitializeComponent();

            var vm = new ATCommandWindowViewModel();

            vm.CommandEntered += commandHandler;
            vm.RequestClose += () => this.Close();

            this.DataContext = vm;

            this.Loaded += (s, e) => ATCommandTextBox.Focus();
        }
    }
}


