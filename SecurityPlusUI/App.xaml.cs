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
            //this.ShutdownMode = ShutdownMode.OnMainWindowClose;

            var viewModel = new MainWindowViewModel();

            Application.Current.DispatcherUnhandledException += (s, args) =>
            {
                if (null != viewModel)
                {
                    viewModel.Exit();
                }

                File.WriteAllText("Critical.txt", args.Exception.ToString());

                args.Handled = true;
            };

            new MainWindow(viewModel).Show();
        }
    }
}
