using PdfSharp.Fonts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using static ELM327_GUI.MainWindow;

namespace ELM327_GUI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            GlobalFontSettings.FontResolver = new CustomFontResolver();
        }
    }
}
