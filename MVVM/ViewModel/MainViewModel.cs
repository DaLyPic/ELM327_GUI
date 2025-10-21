using ELM327_GUI.MVVM.ViewModel;

namespace ELM327_GUI.MVVM.ViewModel
{
    public class MainViewModel
    {
        public DbcParserViewModel DbcParserVM { get; }
        public MainWindowViewModel MainVM { get; }
        public PIDCommandWindowViewModel PIDCommandVM { get; }
        public ATCommandWindowViewModel ATCommandVM { get; }

        public MainViewModel()
        {
            DbcParserVM = new DbcParserViewModel();
            MainVM = new MainWindowViewModel();
            PIDCommandVM = new PIDCommandWindowViewModel();
            ATCommandVM = new ATCommandWindowViewModel();
        }
    }
}