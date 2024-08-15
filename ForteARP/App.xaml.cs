using System.Windows;
using System.ComponentModel.Composition.Hosting;
using ForteARP.Modules;
using ForteARP.ViewModels;
using System.Threading;
using System;
using ForteArg.Services;


namespace ForteARP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Thread newWindowThread;
        private LoadingWIndow tempWindow;
     

        protected override void OnStartup(StartupEventArgs e)
        {
            newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.IsBackground = true;
            newWindowThread.Start();

            var LogMessage = new ClsSerilog();
            ClsSerilog.LogMessage(ClsSerilog.Info, $"-------------------------------------------------");
            ClsSerilog.LogMessage(ClsSerilog.Info, $"Start ARG Application -> {DateTime.Now}");

            MainWindow mainwindow = new MainWindow();
            AssemblyCatalog catalog = new AssemblyCatalog(GetType().Assembly);
            CompositionContainer container = new CompositionContainer(catalog);

            var modules = container.GetExportedValues<IModule>();
            mainwindow.DataContext = new MainWindowViewModel(modules, ApplicationService.Instance.EventAggregator);
         
            mainwindow.Show();

            newWindowThread.Abort();
            if (newWindowThread != null) newWindowThread = null;
            
        }

        private void ThreadStartingPoint()
        {
            try
            {
                tempWindow = new LoadingWIndow();
                tempWindow.Show();
                System.Windows.Threading.Dispatcher.Run();
            }
            catch (ThreadAbortException)
            {
                tempWindow = null;
                //System.Windows.Threading.Dispatcher.InvokeShutdown();
            }
        }
    }
}
