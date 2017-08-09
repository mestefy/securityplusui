using System;
using System.IO;
using System.Windows;

using SecurityPlusUI.Model;
using SecurityPlusUI.Services;
using SecurityPlusUI.ViewModel;

namespace SecurityPlusUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void ApplicationStartup(object sender, StartupEventArgs e)
        {
//            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
            new MainWindow(new MainWindowViewModel()).Show();
//            NotificationService.DisplayWindows(new ContextBase());
        }
    }
}
