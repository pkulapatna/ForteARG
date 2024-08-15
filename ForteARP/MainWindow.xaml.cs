using System;
using System.Windows;
using System.Windows.Controls;
using ForteARP.Modules;
using ForteARP.Model;
using ForteARP.ViewModels;
using ForteARP.Properties;
using Prism.Events;
using ForteArg.Services;

//using ForteARPServices;

namespace ForteARP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow AppWindows;

        private Sqlhandler _sqlhandler = new Sqlhandler();

        public static ClsParams AppParams;
        public ClsXml ClsXML;
        public static double MaxWindWidth = SystemParameters.PrimaryScreenWidth;
        public static double MaxWindHeight = SystemParameters.PrimaryScreenHeight;

        /// <summary>
        /// turn Modules on or off....
        /// </summary>
        public bool bBaleArchive;
        public bool bBaleRealtime;
       
        public bool bDropProfille;
        public bool bDropOptions;
        public bool bDualGraph;
        public bool bRealTimeGraph;
        public bool bWetlayer;
        public bool bWetTrend;
        public bool bVariables;
        public bool bHistroGram;
        public bool bProdEstimate;
        public int iBalesinDrop;
       
        public bool bDebug = false;
        public bool bBrokerSql = false;

       
        public bool bDropGraph = false;
        public bool bDropPosition = false;
        public bool bRemoteProfile = false;

        public string StatusMessage = string.Empty;

        protected readonly IEventAggregator _eventAggregator;

        public MainWindow()
        {
            InitializeComponent();
            
            AppWindows = this;
            AppWindows.Focus();
            AppWindows.Focusable = true;

            _sqlhandler.SetSqlParams();
            _sqlhandler.SetConnectionString();

            ClsSerilog.LogMessage(ClsSerilog.Info, $"Config SQL Connections ......................");

            AppParams = new ClsParams();         
            ClsXML = new ClsXml();
     
            SetLanguageDictionary();
            InitAllModules();

            /*
            if (bAllModChecked)
            {
                bBrokerSql = _sqlhandler.CheckSqlBroker();
                if (bBrokerSql)
                {
                    //ClsSerilog.LogMessage(ClsSerilog.Info, $"Start ARG Application -> {DateTime.Now}");(MsgTypes.INFO, MsgSources.DBSQL, "SQL Service Broker: ENABLED");
                }
                else
                {
                    //ClsSerilog.LogMessage(ClsSerilog.Info, $"Start ARG Application -> {DateTime.Now}");(MsgTypes.INFO, MsgSources.DBSQL, "SQL Service Broker: NOT ENABLED!");
                    bool bsetBroker = _sqlhandler.SetSqlBroker(true);
                    if (bsetBroker) //ClsSerilog.LogMessage(ClsSerilog.Info, $"Start ARG Application -> {DateTime.Now}");(MsgTypes.INFO, MsgSources.DBSQL, "SET SQL Service Broker ENABLE");
                }
            }
            */
            // ClassCommon.LogInfo(MsgTypes.INFO, MsgSources.DBSQL, "SET SQL Service Broker ENABLE");


        }

        public void SetUpSql()
        {
            _sqlhandler = Sqlhandler.Instance;
            _sqlhandler.SetSqlParams();
            _sqlhandler.SetConnectionString();
            _sqlhandler.SetupWorkStation();

           
        }

        public bool InitAllModules()
        {

            bool bCheckOK = false;
            Settings.Default.LocalHost = Environment.MachineName;
            Settings.Default.Save();

            _sqlhandler.LocalHost = Environment.MachineName;

            bProdEstimate = Settings.Default.bShowProdEstimate;

            //System.Windows.MessageBox.Show("InitAllModules");

            try
            {
                System.Net.NetworkInformation.Ping pinger = new System.Net.NetworkInformation.Ping();
                System.Net.NetworkInformation.PingReply reply = pinger.Send(_sqlhandler.Host);


                //System.Windows.MessageBox.Show(_sqlhandler.Host.ToString());

                if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    StatusMessage = "Connections to " + _sqlhandler.Host + @"\" + _sqlhandler.SqlInstance + " is good";
                    SetupAppTitle("Forté Archives and Realtime From - " + _sqlhandler.Host + @"\" + _sqlhandler.SqlInstance);

                    // Check network is Available. Also need to check if sql server is running
                    if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                    {
                        Settings.Default.bNetWork = true;
                        ///////
                        // Check Wet layer database in SQL
                        //
                        ClassCommon.WLOptions = CheckWLSQL();
                      
                        bWetlayer = Settings.Default.wetLayerChecked;
                        bWetTrend = Settings.Default.wetLayerChecked;

                        ///////
                        ///Check and set Drop Options
                        // Return ClassCommon.DropOptions
                        //
                        _sqlhandler.CheckSetDropOption();
                        if (ClassCommon.bDropOption)
                        {
                            ClassCommon.bDropProfile = Settings.Default.DpProfileChecked;
                            ClassCommon.bDropGraph = Settings.Default.DropGraphChecked;
                            ClassCommon.bDropPosition = Settings.Default.DropPosChecked;
                            ClassCommon.bRemoteProfile = Settings.Default.RemoteProfileChecked;

                            iBalesinDrop = ClassCommon.BaleInADrop;
                            bDropOptions = ClassCommon.bDropPosition;
                        }
                        else
                        {
                            ClassCommon.bDropProfile = false;
                            ClassCommon.bDropGraph = false;
                            ClassCommon.bDropPosition = false;
                            ClassCommon.bRemoteProfile = false;
                        }

                        bBaleRealtime = Settings.Default.realTimeDataCheck;
                        bBaleArchive = Settings.Default.archCheck;
                        bDualGraph = Settings.Default.relTimeDualGrpCheck;
                        bRealTimeGraph = Settings.Default.realTimeGrpCheck;
                        bVariables = Settings.Default.variablesCheck; // true;
                        bHistroGram = Settings.Default.bShowHistrogram;
                    }
                    else
                    {
                        Settings.Default.bNetWork = false;
                        Settings.Default.Save();
                    }
                }
                else
                {
                    if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                    {
                      //  Console.WriteLine("machine not available");
                        StatusMessage = "STATUS: NO NETWORK CONNECTIONS ..... <Click Search to find Local Sql Server> ";
                        SetupAppTitle("Forté Archives and Realtime -> NETWORK IS NOT AVAILABLE!");
                        LoadSetupModule();
                    }
                }


                bCheckOK = true;
                ClsSerilog.LogMessage(ClsSerilog.Info, $"Initialize Modules ......................");
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show("ERROR in InitAllModules" + ex);
                StatusMessage = "NO SQL SERVER OR NO NETWORK";
                ClsSerilog.LogMessage(ClsSerilog.Info, $"NO SQL SERVER OR NO NETWORK .........." + ex);
                LoadSetupModule();
            }
            return bCheckOK;
        }

        public bool CheckWLSQL()
        {
            return _sqlhandler.FindSqlDatabase("ForteLayer");
        }

        public void SelectListBoxItem()
        {
            LoadModule(listbox.SelectedItem);
        }

        public void LoadSetupModule()
        {
            ContentPresenter.Content = new SetupModule();
        }


        private void LoadModule(object selectedItem)
        {
            ContentPresenter.Content = selectedItem;
        }

        private void RemoveModules()
        {
            ContentPresenter.DataContext = null;
            ContentPresenter.Content = null;
            ContentPresenter.UpdateLayout();
        }

        private void MainWindow_Load(object sender, RoutedEventArgs e)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }


        public void SetupAppTitle(string strTitle)
        {
            this.Title = strTitle.ToString();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            ClsSerilog.LogMessage(ClsSerilog.Info, $"Close ForteARP.....");
            RemoveModules();
            ClsSerilog.CloseLogger();
            AppWindows.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MainWindowViewModel.WLCSVrunning)
            {
                if (System.Windows.MessageBox.Show("Closing the Application will stop daily CSV auto-report !!!", "Shutdown ForteArp Program", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    e.Cancel = true;
                else
                {
                    if (System.Windows.MessageBox.Show("Are you Sure, you want to Exit ???", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                        e.Cancel = true;
                    else
                        e.Cancel = false;
                }
            }
            else
            {
                if (System.Windows.MessageBox.Show("Exit Application?", "Shutdown Application", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    e.Cancel = true;
                else
                    e.Cancel = false;
            }
        }

        private void Window_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }

        /// <summary>
        /// Settings.Default.iLanguageIdx;
        /// </summary>
        public void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (Settings.Default.iLanguageIdx ) //Thread.CurrentThread.CurrentCulture.ToString())
            {
                case 0: // "en-US":
                    dict.Source = new Uri("WpfLanguageResource;component/StringResources.xaml", UriKind.Relative);
                    break;
                case 1: //"Sp-SP":
                    dict.Source = new Uri("WpfLanguageResource;component/StringResources.Sp.xaml", UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("WpfLanguageResource;component/StringResources.xaml", UriKind.Relative);
                    break;
            }

            //System.Windows.MessageBox.Show("SetLanguageDictionary");

            this.Resources.MergedDictionaries.Add(dict);
            ClsSerilog.LogMessage(ClsSerilog.Info, $"Set Language ......................");
        }
    }




    #region ApplicationService and Event


   

    #endregion ApplicationService and Event


}
