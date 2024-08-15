using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using ForteARP.Properties;
using ForteARP.Model;
using System.Data;
using ForteARP.Modules;
using System.Threading;
using System.Configuration;
using System.Windows;
using Application = System.Windows.Forms.Application;
using System.Windows.Threading;
using System.Globalization;
using Microsoft.VisualBasic.ApplicationServices;
using System.Reflection;

using ForteARP.Help;
using ForteArg.Services;


namespace ForteARP.ViewModels
{
    class SetupViewModel : BindableBase
    {
        // clsAccess AccessDbHandler;

        private LoadingWIndow tempWindow;
        private SqlBackup mysqlbackup;
        private Thread newWindowThread;

     

        #region Module options

        //ArchCheck
        private bool _archCheck = Settings.Default.archCheck;
        public bool ArchCheck
        {
            get => _archCheck; 
            set => SetProperty(ref _archCheck, value); 
        }
        //RealTimeDataCheck
        private bool _realTimeDataCheck = Settings.Default.realTimeDataCheck;
        public bool RealTimeDataCheck
        {
            get => _realTimeDataCheck;
            set => SetProperty(ref _realTimeDataCheck, value);
        }
        //RealTimeGrpCheck
        private bool _realTimeGrpCheck = Settings.Default.realTimeGrpCheck;
        public bool RealTimeGrpCheck
        {
            get => _realTimeGrpCheck;
            set => SetProperty(ref _realTimeGrpCheck, value);
        }
        //RelTimeDualGrpCheck
        private bool _relTimeDualGrpCheck = Settings.Default.relTimeDualGrpCheck;
        public bool RelTimeDualGrpCheck
        {
            get => _relTimeDualGrpCheck;
            set => SetProperty(ref _relTimeDualGrpCheck, value);
        }
        //VariablesCheck
        private bool _variablesCheck = Settings.Default.variablesCheck;
        public bool VariablesCheck
        {
            get => _variablesCheck;
            set => SetProperty(ref _variablesCheck, value);
        }


        //WetLayerChecked
        private bool _wetLayerChecked = Settings.Default.wetLayerChecked;
        public bool WetLayerChecked
        {
            get => _wetLayerChecked;
            
            set { SetProperty(ref _wetLayerChecked, value); }
        }

        //WetLayerTrendChecked
        private bool _wetLayerTrendChecked = Settings.Default.bWlShowTrend;
        public bool WetLayerTrendChecked
        {
            get
            {
                if (ClassCommon.WLOptions) WetOpc = "1";
                else WetOpc = "0";
                return _wetLayerTrendChecked;
            }
            set => SetProperty(ref _wetLayerTrendChecked, value);
                
            
        }
      
        private bool _DropChecked = ClassCommon.bDropOption;
        public bool DropChecked
        {
            get
            {
                if (ClassCommon.bDropOption) DropOpc = 1;
                else DropOpc = 0.1;
                return ClassCommon.bDropOption;
            }
            set { SetProperty(ref _DropChecked, value); }
        }


        private bool _dpProfileChecked = Settings.Default.DpProfileChecked;
        public bool DpProfileChecked
        {
            get
            {
               // if (Settings.Default.bDropOption) DropOpc = 1;
               // else DropOpc = 0.1;
               return _dpProfileChecked;
            }
            set {
                
                SetProperty(ref _dpProfileChecked, value);
                //Settings.Default.DpProfileChecked = value;
                //Settings.Default.Save();
            }
        }

        private bool _dropGraphChecked = Settings.Default.DropGraphChecked;
        public bool DropGraphChecked
        {
            get
            {
                // if (Settings.Default.bDropOption) DropOpc = 1;
                // else DropOpc = 0.1;
                return _dropGraphChecked;
            }
            set 
            { 
                SetProperty(ref _dropGraphChecked, value);
                Settings.Default.DropGraphChecked = value;
            }
        }

        private bool _dropPosChecked = Settings.Default.DropPosChecked;
        public bool DropPosChecked
        {
            get
            {
                // if (Settings.Default.bDropOption) DropOpc = 1;
                // else DropOpc = 0.1;
                return _dropPosChecked;
            }
            set 
            { 
                SetProperty(ref _dropPosChecked, value);
                Settings.Default.DropPosChecked = value;
            }
        }

        private bool _dropDataChecked = Settings.Default.RemoteProfileChecked;
        public bool DropDataChecked
        {
            get
            {
                // if (Settings.Default.bDropOption) DropOpc = 1;
                // else DropOpc = 0.1;
                return _dropDataChecked;
            }
            set 
            { 
                SetProperty(ref _dropDataChecked, value);
                Settings.Default.RemoteProfileChecked = value;
            }
        }

        //HistroChecked
        private bool _HistroChecked = Settings.Default.bShowHistrogram; //MainWindow.AppWindows.bHistroGram;
        public bool HistroChecked
        {
            get { return _HistroChecked; }
            set
            {
                SetProperty(ref _HistroChecked, value);
                Settings.Default.bShowHistrogram = value;
                Settings.Default.Save();
            }
        }
        //ProdEstChecked
        private bool _prodEstChecked = Settings.Default.bShowProdEstimate;
        public bool ProdEstChecked
        {
            get => _prodEstChecked;
            set
            {
                SetProperty(ref _prodEstChecked, value);
                Settings.Default.bShowProdEstimate = value;
                Settings.Default.Save();
            }
        }

        #endregion Module options





        private bool _bModify = false;
        public bool BModify
        {
            get { return _bModify; }
            set { SetProperty(ref _bModify, value); }
        }

        private bool _bModselectMan = false;
        public bool BModselectMan
        {
            get { return _bModselectMan; }
            set { SetProperty(ref _bModselectMan, value); }
        }

        private bool bWLEnable = false;
        public bool BWLEnable
        {
            get { return bWLEnable; }
            set { SetProperty(ref bWLEnable, value); }
        }


        private double _DropOpc;
        public double DropOpc
        {
            get { return _DropOpc; }
            set { SetProperty(ref _DropOpc, value); }
        }

        private double _BackupOpa;
        public double BackupOpa
        {
            get { return _BackupOpa; }
            set { SetProperty(ref _BackupOpa, value); }
        }

        private string _WetOpc;
        public string WetOpc
        {
            get { return _WetOpc; }
            set { SetProperty(ref _WetOpc, value); }
        }

        private string _HisOpc;
        public string HisOpc
        {
            get { return _HisOpc; }
            set { SetProperty(ref _HisOpc, value); }
        }

        private string _LoadBoxOp;
        public string LoadBoxOp
        {
            get { return _LoadBoxOp; }
            set { SetProperty(ref _LoadBoxOp, value); }
        }

        private bool _bIniShareFile = false;
        public bool BIniShareFile
        {
            get { return _bIniShareFile; }
            set
            {
                if (value) ShareFileOpc = 1.0;
                else ShareFileOpc = 0.2;
                SetProperty(ref _bIniShareFile, value);
            }
        }

        private string _customerName = String.Empty;
        public string CustomerName
        {
            get { return _customerName; }
            set { SetProperty(ref _customerName, value); }
        }

        private string _weightUnit = string.Empty;
        public string WeightUnit
        {
            get { return _weightUnit; }
            set { SetProperty(ref _weightUnit, value); }
        }

        private string _moisturetype = string.Empty;
        public string MoistureType
        {
            get { return _moisturetype; }
            set { SetProperty(ref _moisturetype, value); }
        }

        private double _ShareFileOpc = 0.1;
        public double ShareFileOpc
        {
            get { return _ShareFileOpc; }
            set { SetProperty(ref _ShareFileOpc, value); }
        }

        private bool _bbudialog = false;
        public bool Bbudialog
        {
            get { return _bbudialog; }
            set { SetProperty(ref _bbudialog, value); }
        }

        private string _txtPropSaveLoc;
        public string TxtPropSaveLoc
        {
            get { return _txtPropSaveLoc; }
            set { SetProperty(ref _txtPropSaveLoc, value); }
        }

        private List<string> _DayEndList;
        public List<string> DayEndList
        {
            get
            {
                _DayEndList = new List<string>();
                DateTime date = new DateTime();

                var result = Enumerable.Repeat(date, 24)
                                       .Select((x, i) => x.AddHours(i).ToString("HH:MM"));

                var hours2 = Enumerable.Range(00, 24).Select(i => (DateTime.MinValue.AddHours(i)).ToString("hh.mm"));

                var hours = from i in Enumerable.Range(0, 24)
                            let h = new DateTime(2019, 1, 1, i, 0, 0)
                            select h.ToString("t", CultureInfo.InvariantCulture);

                foreach (var item in hours)
                {
                    _DayEndList.Add(item.ToString());
                }
                return _DayEndList;
            }
        }

        private int _SelectDayEndIndex;
        public int SelectDayEndIndex
        {
            get { return Settings.Default.SelTimeEndIndex; }
            set
            {
                if (value > 0) //0 is 00:00 hour
                {
                    Settings.Default.SelTimeEndIndex = value;
                    Settings.Default.Save();
                    SetProperty(ref _SelectDayEndIndex, value);
                }
            }
        }

        private DateTime _selectDayEndTimeVal ;
        public DateTime SelectDayEndTimeVal
        {
            get { return _selectDayEndTimeVal; }
            set
            {
                SetProperty(ref _selectDayEndTimeVal, value);
                if(value != null)
                {
                    Settings.Default.ProdDayEnd = value;
                    Settings.Default.Save();
                }
            }
        }


        #region Type,Unit and Timer 1

       

        private bool _mcChecked;
        public bool MCChecked
        {
            get => _mcChecked;
            set => SetProperty(ref _mcChecked, value);
        }

        private bool _mrChecked;
        public bool MRChecked
        {
            get { return _mrChecked; }
            set { SetProperty(ref _mrChecked, value); }
        }

        private bool _adChecked;
        public bool ADChecked
        {
            get { return _adChecked; }
            set { SetProperty(ref _adChecked, value); }
        }

        private bool _bdChecked;
        public bool BDChecked
        {
            get { return _bdChecked; }
            set { SetProperty(ref _bdChecked, value); }
        }

        private bool _kgChecked;
        public bool KGChecked
        {
            get { return _kgChecked; }
            set { SetProperty(ref _kgChecked, value); }
        }

        private bool _lbChecked;
        public bool LBChecked
        {
            get { return _lbChecked; }
            set { SetProperty(ref _lbChecked, value); }
        }

        // 600000 mSec = 600 Sec. = 10 minutes. Max.
        // Default 5 sec., MIn 1 Sec.
        private int _scanrate = Settings.Default.iScanRate;
        public int ScanRate
        {
            get { return _scanrate; }
            set
            {
                if ((value > 0) & (value < 601))
                    SetProperty(ref _scanrate, value);
                else
                    SetProperty(ref _scanrate, 5);
            }
        }

        #endregion Type,Unit and Timer 1

        #region SQL1

        private Sqlhandler _sqlhandler = Sqlhandler.Instance;

        private int _selectedServerCombo;
        public int SelectedServerCombo
        {
            get
            {
                return _selectedServerCombo;
            }
            set
            {
                SetProperty(ref _selectedServerCombo, value);
                char[] separators = { '\\' };  //Host\\Instant
                string strNewHost = ServercomboList[SelectedServerCombo].ToString();
                string[] words = strNewHost.Split(separators);
                Host = words[0];
                Instant = words[1];
            }
        }
        private List<string> _servercomboList;
        public List<string> ServercomboList
        {
            get { return _servercomboList; }
            set{ SetProperty(ref _servercomboList, value); }
        }

    

        private string _LocalHost = Environment.MachineName;
        public string LocalHost
        {
            get { return _LocalHost; }
            set { SetProperty(ref _LocalHost, value); }
        }


        private string _host;
        public string Host
        {
            get { return _host; }
            set { SetProperty(ref _host, value); }
        }

        private string _instance;
        public string Instant
        {
            get { return _instance; }
            set { SetProperty(ref _instance, value); }
        }

        private string _userid;
        public string Userid
        {
            get { return _userid; }
            set
            {
                SetProperty(ref _userid, value);
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private string _database;
        public string Database
        {
            get { return _database; }
            set { SetProperty(ref _database, value); }
        }

        private bool _bsqlMode;
        public bool BSqlMode
        {
            get { return _bsqlMode; }
            set { SetProperty(ref _bsqlMode, value); }
        }

        private bool _blocal;
        public bool BLocal
        {
            get { return _blocal; }
            set { SetProperty(ref _blocal, value); }
        }

        private bool _bremote;
        public bool BRemote
        {
            get { return _bremote; }
            set { SetProperty(ref _bremote, value); }
        }

        private bool _BLogMsgOn;
        public bool BLogMsgOn
        {
            get { return _BLogMsgOn; }
            set
            {
                if (value)
                {
                    Settings.Default.bLogMsgOn = true;
                }
                else
                {
                    Settings.Default.bLogMsgOn = false;
                }
                Settings.Default.Save();
                SetProperty(ref _BLogMsgOn, value);
            }
        }


        private bool _bTesting = false;
        public bool BTesting
        {
            get { return _bTesting; }
            set { SetProperty(ref _bTesting, value); }
        }

        private string _strStatus;
        public string StrStatus
        {
            get { return _strStatus; }
            set { SetProperty(ref _strStatus, value); }
        }

        private string _strTestStatus;
        public string StrTestStatus
        {
            get { return _strTestStatus; }
            set { SetProperty(ref _strTestStatus, value); }
        }

     

        #endregion SQL1

        private string _updateinfo;
        public string UpdateInfo
        {
            get { return _updateinfo; }
            set { SetProperty(ref _updateinfo, value); }
        }


        private bool _AutoChecked;
        public bool AutoChecked
        {
            get { return _AutoChecked; }
            set
            {
                if (value)
                {
                    Settings.Default.bAutoSetMod = true;
                    DOpac = 0.3;
                }
                else
                {
                    Settings.Default.bAutoSetMod = false;
                    DOpac = 1;
                }
                Settings.Default.Save();
                SetProperty(ref _AutoChecked, value);
            }
        }

        private bool _ManChecked;
        public bool ManChecked
        {
            get { return _ManChecked; }
            set { SetProperty(ref _ManChecked, value); }
        }

        private double _dOpac = 0.3;
        public double DOpac
        {
            get { return _dOpac; }
            set { SetProperty(ref _dOpac, value); }
        }

        private bool _NetworkChecked;
        public bool NetworkChecked
        {
            get
            {
                if (Settings.Default.bNetWork) WetOpc = "1";
                else WetOpc = "0.4";
                return Settings.Default.bNetWork;
            }
            set { SetProperty(ref _NetworkChecked, value); }
        }


        private bool _DropOption;
        public bool DropOption
        {
            get { return _DropOption; }
            set { SetProperty(ref _DropOption, value); }
        }


        private bool _SearchDone = false;
        public bool SearchDone
        {
            get { return _SearchDone; }
            set { SetProperty(ref _SearchDone, value); }
        }


        private bool _DropHitoLow;
        public bool DropHitoLow
        {
            get { return _DropHitoLow; }
            set
            {
                if (value)
                {
                    Settings.Default.bDropHitoLow = true;
                }
                else
                {
                    Settings.Default.bDropHitoLow = false;
                }
                SetProperty(ref _DropHitoLow, value);
                Settings.Default.Save();
            }
        }

        private int _BaleInDrop;
        public int BaleInDrop
        {
            get { return _BaleInDrop; }
            set { SetProperty(ref _BaleInDrop, value); }
        }


        private List<string> _LanguageList;
        public List<string> LanguageList
        {
            get { return _LanguageList; }
            set { SetProperty(ref _LanguageList, value); }
        }

        private int _Languageindex;

        public int Languageindex
        {
            get { return _Languageindex; }
            set { SetProperty(ref _Languageindex, value); }
        }

       
        private bool _bSqlDep;
        public bool BSqlDep
        {
            get { return _bSqlDep; }
            set { SetProperty(ref _bSqlDep, value); }
        }

      
        private Visibility _LoadingBox;
        public Visibility LoadingBox
        {
            get { return _LoadingBox; }
            set { SetProperty(ref _LoadingBox, value); }
        }

        private string _programVersion;
        public string ProgramVersion
        {
            get { return _programVersion; }
            set { SetProperty(ref _programVersion, value); }
        }

        //--------------------------------------------------------------------------------------------------------------
        #region DelegateCommand Button Events

        public DelegateCommand LoadedPageICommand { get; set; }
        public DelegateCommand ClosedPageICommand { get; set; }

        private DelegateCommand _backupComand;
        public DelegateCommand BackupCommand =>
            _backupComand ?? (_backupComand = new DelegateCommand(BackupExecute).ObservesCanExecute(() => BLocal));
        private void BackupExecute()
        {
            mysqlbackup = new SqlBackup();
            mysqlbackup.ShowDialog();
            Bbudialog = false;
        }

        private DelegateCommand _searchComand;
        public DelegateCommand SearchCommand =>
            _searchComand ?? (_searchComand = new DelegateCommand(SearchExecute).ObservesCanExecute(() => BModify));
        private void SearchExecute()
        {
            SearchSqlServers();
        }


        private async void SearchSqlServers()
        {
            LoadBoxOp = "1";
            LoadingBox = Visibility.Visible;
            StrStatus = "Searching for SQL Server on Local NetWork";
            newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.IsBackground = true;
            newWindowThread.Start();

            DataTable dt = await _sqlhandler.GetServers();

            if (dt.Rows.Count > 0)
            {
                ServercomboList = new List<string>();
                foreach (DataRow row in dt.Rows)
                {
                    if (row["InstanceName"].ToString() == "")
                        ServercomboList.Add(row["ServerName"].ToString());
                    else
                        ServercomboList.Add(row["ServerName"].ToString() + @"\" + row["InstanceName"].ToString());
                }

                SearchDone = true;
                StrStatus = "Search Done!";
                SelectedServerCombo = 0;

                LoadingBox = Visibility.Hidden;
                LoadBoxOp = "0";
                newWindowThread.Abort();
                if (newWindowThread != null) newWindowThread = null;
            }
            dt = null;
        }



        private DelegateCommand _testComand;
        public DelegateCommand TestCommand =>
       _testComand ?? (_testComand = new DelegateCommand(TestExecute).ObservesCanExecute(() => SearchDone).ObservesCanExecute(() => BModify));
        private void TestExecute()
        {
            BTesting = _sqlhandler.TestSqlConnection(Host, Instant, Database, Userid, Password);
            StrTestStatus = BTesting ? "Test connection Passed, Click Accept" : "Test connection Fail";
        }

        private DelegateCommand _acceptComand;
        public DelegateCommand AcceptCommand =>
       _acceptComand ?? (_acceptComand = new DelegateCommand(AcceptExecute).ObservesCanExecute(() => BTesting).ObservesCanExecute(() => BModify));
        private void AcceptExecute()
        {
            SaveSetting();
            ForteARP.MainWindow.AppWindows.SetupAppTitle("Forté Archives and Realtime From - " + _sqlhandler.Host + @"\" + _sqlhandler.SqlInstance);
            BTesting = false;
            StrTestStatus = $"Target Saved";
        }

        private DelegateCommand _showAboutCommand;
        public DelegateCommand ShowAboutCommand =>
       _showAboutCommand ?? (_showAboutCommand = new DelegateCommand(ShowAboutCommandExecute));
        private void ShowAboutCommandExecute()
        {
            InformWin aboutBox = new InformWin()
            {
                Topmost = true,
            };
            aboutBox.ShowDialog();
        }

        private DelegateCommand _showSQLHelpCommand;
        public DelegateCommand ShowSQLHelpCommand =>
       _showSQLHelpCommand ?? (_showSQLHelpCommand = new DelegateCommand(ShowSQLHelpCommandExecute));
        private void ShowSQLHelpCommandExecute()
        {
            SqlInfoWin aboutBox = new SqlInfoWin()
            {
                Topmost = true,
            };
            aboutBox.ShowDialog();
        }

        private DelegateCommand _settingsHelpCommand;
        public DelegateCommand SettingsHelpCommand =>
       _settingsHelpCommand ?? (_settingsHelpCommand = new DelegateCommand(SettingsHelpCommandExecute));
        private void SettingsHelpCommandExecute()
        {
            SettingsWin aboutBox = new SettingsWin()
            {
                Topmost = true,
            };
            aboutBox.ShowDialog();
        }

        private DelegateCommand _modifyCommand;
        public DelegateCommand ModifyCommand =>
        _modifyCommand ?? (_modifyCommand = new DelegateCommand(ModifyExecute));
        private void ModifyExecute()
        {
            BModify = true;
        }

        private DelegateCommand _applyCommand;
        public DelegateCommand ApplyCommand =>
        _applyCommand ?? (_applyCommand = new DelegateCommand(ApplyExecute).ObservesCanExecute(() => BModify));
        private void ApplyExecute()
        {
            if (ScanRate > 0)
                Settings.Default.iScanRate = ScanRate;

            Settings.Default.iLanguageIdx = Languageindex;
            _sqlhandler.LanguageIdx = Languageindex;

            Settings.Default.archCheck = _archCheck;
            Settings.Default.realTimeDataCheck = _realTimeDataCheck;
            Settings.Default.realTimeGrpCheck = _realTimeGrpCheck;
            Settings.Default.relTimeDualGrpCheck = _relTimeDualGrpCheck;
            Settings.Default.variablesCheck = _variablesCheck;
            Settings.Default.wetLayerChecked = _wetLayerChecked;
            Settings.Default.bWlShowTrend = _wetLayerTrendChecked;

            if (ClassCommon.bDropOption)
            {
                Settings.Default.DpProfileChecked = DpProfileChecked;
                Settings.Default.DropGraphChecked = DropGraphChecked;
                Settings.Default.DropPosChecked = DropPosChecked;
                Settings.Default.RemoteProfileChecked = DropDataChecked;
            }


            if (MCChecked)
            {
                Settings.Default.MoistureUnit = 0;
                _sqlhandler.MoistureUnit = 0;
            }
            if (MRChecked)
            {
                Settings.Default.MoistureUnit = 1;
                _sqlhandler.MoistureUnit = 1;
            }
            if (ADChecked)
            {
                Settings.Default.MoistureUnit = 2;
                _sqlhandler.MoistureUnit = 2;
            }
            if (KGChecked)
            {
                Settings.Default.WeightUnit = 0;
                _sqlhandler.WeightUnit = 0;
            }
            if (LBChecked)
            {
                Settings.Default.WeightUnit = 1;
                _sqlhandler.WeightUnit = 1;
            }

            Settings.Default.Save();
            BModify = false;
            System.Windows.MessageBox.Show("The Application will shutdown and restart in a few secounds.");

            MainWindow.AppWindows.Close();
            Thread.Sleep(100);
            Application.Restart();
        }

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand =>
        _cancelCommand ?? (_cancelCommand = new DelegateCommand(CancelExecute).ObservesCanExecute(() => BModify));
        private void CancelExecute()
        {
            BModify = false;

            Host = _sqlhandler.GetHostName();
            Instant = _sqlhandler.GetInstance();
            Userid = _sqlhandler.GetUserId();
            Password = _sqlhandler.GetPassWord();
            Database = _sqlhandler.GetDastaBase();
        }

        #endregion DelegateCommand Button Events
        //----------------------------------------------------------------------------------------------------------------


        public SetupViewModel()
        {
         
            if (_sqlhandler.Host == _sqlhandler.LocalHost)
                Bbudialog = true;

            DropHitoLow = Settings.Default.bDropHitoLow;

            BSqlDep = MainWindow.AppWindows.bBrokerSql;
            BaleInDrop = ClassCommon.BaleInADrop;// MainWindow.AppWindows.iBalesinDrop;

            LoadedPageICommand = new DelegateCommand(LoadPageExecute);
            ClosedPageICommand = new DelegateCommand(ClosePageExecute);


            if (Settings.Default.bAutoSetMod) AutoChecked = true;
            else AutoChecked = false;

            SetupLocalRemote();

            TxtPropSaveLoc = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;

            LanguageList = new List<string>
            {
                "English",
                "Spanish"
            };

            Languageindex = Settings.Default.iLanguageIdx;

            AssemblyInfo entryAssemblyInfo = new AssemblyInfo(Assembly.GetEntryAssembly());
            ProgramVersion = $"Program Version : {GetLastModTime()}";

            ClsSerilog.LogMessage(ClsSerilog.Info, $"Initialize Main Program Setup");
        }

        private string GetLastModTime()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(assembly.Location);
            DateTime lastModified = fileInfo.LastWriteTime;
            return $" {lastModified}";
        }

        private void SetupLocalRemote()
        {
            Host = _sqlhandler.GetHostName();
            Instant = _sqlhandler.GetInstance();
            Userid = _sqlhandler.GetUserId();
            Password = _sqlhandler.GetPassWord();
            Database = _sqlhandler.GetDastaBase();

            //string inifilepath =  @"C:\ForteSystem\Reports\LVFormats.mdb";

            if (_sqlhandler.LocalHost == Host)
            {
                Settings.Default.BSerRemote = false;
                BLocal = true;
                BRemote = false;
                BackupOpa = 1;

                //if (System.IO.File.Exists(inifilepath))        
                //     MainWindow.AppWindows.AccessDbHandler.SetupAccessDB();
                //LVHdrFmtBaleTable = MainWindow.AppWindows.AccessDbHandler.LVHdrFmtBale;
                //SetConfigfromIni();
            }
            else
            {
                Settings.Default.BSerRemote = true;
                BLocal = false;
                BRemote = true;
                BackupOpa = 0;
            }
            Settings.Default.Save();
        }

        private void ClosePageExecute()
        {
            SearchDone = false;
            if (newWindowThread != null) newWindowThread = null;
        }

        private void LoadPageExecute()
        {
            BTesting = false;
            BModify = false;
            Bbudialog = true;

            WetOpc = ClassCommon.WLOptions ? "1" : "0.4";

            DropChecked = ClassCommon.bDropPosition;

            LoadingBox = Visibility.Visible;
            SelectDayEndTimeVal = Settings.Default.ProdDayEnd;

             LoadBoxOp = "1";
          
            BLogMsgOn = Settings.Default.bLogMsgOn;
            ManChecked = true;
            UpdateInfo = MainWindow.AppWindows.StatusMessage; 
            ScanRate = (Settings.Default.iScanRate);

            BWLEnable = ClassCommon.WLOptions;

            if(BWLEnable == false)
            {
                WetLayerChecked = false;
                WetLayerTrendChecked = false;
            }

            switch (Settings.Default.MoistureUnit)
            {
                case 0:
                    MCChecked = true;
                    _sqlhandler.MoistureUnit = 0;
                    break;
                case 1:
                    MRChecked = true;
                    _sqlhandler.MoistureUnit = 1;
                    break;
                case 2:
                    ADChecked = true;
                    _sqlhandler.MoistureUnit = 2;
                    break;
                case 3:
                    BDChecked = true;
                    _sqlhandler.MoistureUnit = 3;
                    break;
                default:
                    break;
            }
            if (Settings.Default.WeightUnit == 0)
            {
                KGChecked = true;
                _sqlhandler.WeightUnit = 0;
            }    
            else
            {
                LBChecked = true;
                _sqlhandler.WeightUnit = 1;
            }
                
            StrStatus = string.Empty;
            StrTestStatus = string.Empty;
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

        private void SetConfigfromIni()
        {
            BIniShareFile = _sqlhandler.ReadSharedinifile();
            if (BIniShareFile)
            {
                CustomerName = _sqlhandler.strCustName;
                MoistureType = _sqlhandler.strMoistureTyp;
                WeightUnit = _sqlhandler.strWtUnit;

                switch (_sqlhandler.strMoistureTyp)
                {
                    case "MC":
                        Settings.Default.MoistureUnit = 0;
                        _sqlhandler.WeightUnit = 0;
                        break;

                    case "MR":
                        Settings.Default.MoistureUnit = 1;
                        _sqlhandler.WeightUnit = 1;
                        break;

                    case "AD":
                        Settings.Default.MoistureUnit = 2;
                        _sqlhandler.WeightUnit = 2;
                        break;

                    case "BD":
                        Settings.Default.MoistureUnit = 3;
                        _sqlhandler.WeightUnit = 3;
                        break;
                }

                if (_sqlhandler.strWtUnit == "Metric")
                {
                    Settings.Default.WeightUnit = 0;
                    _sqlhandler.WeightUnit = 0;
                }
                else // English
                {
                    Settings.Default.WeightUnit = 1;
                    _sqlhandler.WeightUnit = 1;
                }
                Settings.Default.Save();
            }
        }

        #region SQL 2
        
        private void SaveSetting()
        {
            //ServercomboList
            //SelectedServerCombo

            if (_sqlhandler.LocalHost == Host)
            {
                _sqlhandler.BLocal = true;
                _sqlhandler.BSerRemote = false;

                Settings.Default.BSerRemote = false;
                BLocal = true;
                BRemote = false;
            }
            else
            {
                _sqlhandler.BLocal = false;
                _sqlhandler.BSerRemote = true;

                Settings.Default.BSerRemote = true;
                BLocal = false;
                BRemote = true;
            }

            _sqlhandler.Host = Host;
            _sqlhandler.SqlInstance = Instant;
            _sqlhandler.Database = Database;
            _sqlhandler.UserName = Userid;
            _sqlhandler.Password = Password;
            _sqlhandler.CbServerSelect = SelectedServerCombo;

            Settings.Default.Host = Host;
            Settings.Default.Instance = Instant;
            Settings.Default.Database = Database;
            Settings.Default.UserName = Userid;
            Settings.Default.PassWord = Password;
            Settings.Default.iCbServerSelect = SelectedServerCombo;
            Settings.Default.Save();

            //Update Sql Connections
            //_sqlhandler.SetConnectionString(); 

        }

        #endregion SQL 2


        void CloseWindowSafe(Window w)
        {
            if (w.Dispatcher.CheckAccess())
                w.Close();
            else
                w.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(w.Close));
        }

    }
}
