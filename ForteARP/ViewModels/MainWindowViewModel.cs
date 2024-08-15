using System.Collections.Generic;
using ForteARP.Modules;
using Prism.Mvvm;
using System.Windows.Controls;
using Prism.Regions;
using Prism.Commands;
using System.Linq;
using System;
using Prism.Events;
using ForteARP.Model;
using ForteARP.Properties;
using System.Windows;
using Microsoft.VisualBasic.Devices;
using ForteArg.Services;


namespace ForteARP.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        protected readonly IEventAggregator _eventAggregator;

        Sqlhandler _sqlhandler = Sqlhandler.Instance;

        public IEnumerable<IModule> Modules { get; set; }    
   
        private DelegateCommand _navigateCmd;
        public DelegateCommand NavigateCmd =>
        _navigateCmd ?? (_navigateCmd =
            new DelegateCommand(NavigateExecute).ObservesCanExecute(() => BTargetonNetwork));
        private void NavigateExecute()
        {
            MainWindow.AppWindows.SelectListBoxItem();
        }

        private DelegateCommand _loadedPageICommand;
        public DelegateCommand LoadedPageICommand =>
        _loadedPageICommand ?? (_loadedPageICommand =
            new DelegateCommand(LoadedPageExecute));
        private void LoadedPageExecute()
        {
            SetMenuWidth(100);
        }
        
        public DelegateCommand ClosedPageICommand { get; set; }

        public static bool WLCSVrunning { get; set; }

        private IModule _selectedModule;
        public IModule SelectedModule
        {
            get => _selectedModule; 
            set => SetProperty(ref _selectedModule, value); 
        }          
        public UserControl UserInterface
        {
            get
            {
                if (SelectedModule != null)
                    return SelectedModule.UserInterface;
                else
                    return null;
            }
            set { UserInterface = value;}
        }

        private bool _bMenuEnable;
        public bool BTargetonNetwork
        {
            get { return _bMenuEnable; }
            set { SetProperty(ref _bMenuEnable, value); }
        }

        private bool _sqlConnection;
        public bool SqlConnection
        {
            get { return _sqlConnection; }
            set { SetProperty(ref _sqlConnection, value); }
        }


        private string _mainWindowTitle;
        public string MainWindowTitle
        {
            get => _mainWindowTitle; 
            set => SetProperty(ref _mainWindowTitle, value); 
        }

        private bool _ModuleOveride = false;
        public bool ModuleOveride
        {
            get => _ModuleOveride; 
            set
            {
                Settings.Default.bModOveride = value;
                Settings.Default.Save();
                SetProperty(ref _ModuleOveride, value);
            }
        }

        private int _MenuWdt;
        public int MenuWdt
        {
            get { return _MenuWdt; }
            set { SetProperty(ref _MenuWdt, value); }
        }


        /// <summary>
        /// Assign and load modules
        /// </summary>
        /// <param name="modules"></param>
        public MainWindowViewModel(IEnumerable<IModule> modules, IEventAggregator eventAggregator)
        {
            this._eventAggregator= eventAggregator;

            //Only load the modules with bActive =  true...
            this.Modules = modules.Where(u => u.BActive == true);
            //Order modules by the index numbers
            this.Modules = Modules.OrderBy(Modules => Modules.Index);

            BTargetonNetwork = true; // PingRemoteHost();

          //  SqlConnection = _sqlhandler.TestSqlConnection();

            ClosedPageICommand = new DelegateCommand(ClosedPageExecute);

            _eventAggregator.GetEvent<UpdatedEvent>().Subscribe(Update);
            _eventAggregator.GetEvent<UpdatedWLEvent>().Subscribe(UpdateWL);

        }

        private void UpdateWL(bool obj)
        {
            WLCSVrunning = obj;
        }

        private void ClosedPageExecute()
        {
            _eventAggregator.GetEvent<UpdatedEventShutdown>().Publish(true);
        }

        private void Update(bool obj)
        {
            BTargetonNetwork = !obj;
            MainWindow.AppWindows.listbox.IsEnabled = BTargetonNetwork;

            if (BTargetonNetwork)
                SetMenuWidth(100);
            else
                SetMenuWidth(0);
        }

        private bool PingRemoteHost()
        {
            bool AOK = false;
        
            try
            {
                System.Net.NetworkInformation.Ping pinger = new System.Net.NetworkInformation.Ping();
                System.Net.NetworkInformation.PingReply reply = pinger.Send(_sqlhandler.Host);
                if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    AOK = true;
                    ClsSerilog.LogMessage(ClsSerilog.Info, $"Ping Host -> " +
                        _sqlhandler.Host +  " - " + reply.Status.ToString());
                }
                else
                {
                    AOK = false;
                    ClsSerilog.LogMessage(ClsSerilog.Info, $"Ping Host -> " +
                       _sqlhandler.Host + " - " + reply.Status.ToString());
                }
            }
            catch (Exception ex)
            {
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in PingRemoteHost -> {ex.Message}");
                //MessageBox.Show("ERROR in PingRemoteHost " + ex);
            }
            return AOK;
        }

        public void SetMenuWidth(int iWidth)
        {
            MenuWdt = iWidth;
        }
    }
}
