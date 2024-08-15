

using ForteArg.Services;
using ForteARP.Model;
using ForteARP.Module_Graphs.Model;
using ForteARP.Properties;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ForteARP.Module_Graphs.ViewModels
{
    public class DualGraphViewModel : BindableBase
    {
        protected readonly Prism.Events.IEventAggregator _eventAggregator;

        private Sqlhandler _sqlhandler = Sqlhandler.Instance;

        public DualGraphModel CDualModel;
       

        public DelegateCommand LoadedPageICommand { get; set; }
        public DelegateCommand ClosedPageICommand { get; set; }
        public DelegateCommand StartCommand { get; set; }  //Start Button
        public DelegateCommand StopCommand { get; set; }   //Stop Button

        public DelegateCommand ReDrawCommand { get; set; } //Re-Draw Graph

        public DelegateCommand OnSourceCheckCommand { get; set; }
        public DelegateCommand OnLineCheckCommand { get; set; }

        private bool _rtrunning = false;
        public bool RTRunning
        {
            get { return _rtrunning; }
            set
            {
                RTIdle = !value;
                SetProperty(ref _rtrunning, value);
            }
        }

        private bool _rtIdle = true;
        public bool RTIdle
        {
            get { return _rtIdle; }
            set { SetProperty(ref _rtIdle, value); }
        }

        private double _showme = .1;
        public double ShowMe
        {
            get { return _showme; }
            set { SetProperty(ref _showme, value); }
        }

        private double _opac = 1.0;
        public double Opac
        {
            get { return _opac; }
            set { SetProperty(ref _opac, value); }
        }

        private string _moisturelow;
        public string MoistureLow
        {
            get { return _moisturelow; }
            set { SetProperty(ref _moisturelow, value); }
        }

        private string _moistureavg;
        public string MoistureAVG
        {
            get { return _moistureavg; }
            set { SetProperty(ref _moistureavg, value); }
        }


        private string _moisturehi;
        public string MoistureHi
        {
            get { return _moisturehi; }
            set { SetProperty(ref _moisturehi, value); }
        }

        private string _curMoisture;
        public string CurMoisture
        {
            get { return _curMoisture; }
            set { SetProperty(ref _curMoisture, value); }
        }


        private string _moistureype;
        public string MoistureType
        {
            get { return _moistureype; }
            set { SetProperty(ref _moistureype, value); }
        }

        private string _weightlow;
        public string WeightLow
        {
            get { return _weightlow; }
            set { SetProperty(ref _weightlow, value); }
        }

        private string _weighthi;
        public string WeightHi
        {
            get { return _weighthi; }
            set { SetProperty(ref _weighthi, value); }
        }


        private string _weightavg;
        public string WeightAVG
        {
            get { return _weightavg; }
            set { SetProperty(ref _weightavg, value); }
        }

        private string _curWeight;
        public string CurWeight
        {
            get { return _curWeight; }
            set { SetProperty(ref _curWeight, value); }
        }


        private string _weightunit;
        public string WeightUnit
        {
            get { return _weightunit; }
            set { SetProperty(ref _weightunit, value); }
        }


        private int _Sourcex;
        public int Source
        {
            get { return _Sourcex; }
            set { SetProperty(ref _Sourcex, value); }
        }

        private long preIndex = 0;
        private long newIndex = 0;

        private DataTable _realtimedatatable;
        public DataTable RealTimeDataTable
        {
            get { return _realtimedatatable; }
            set { SetProperty(ref _realtimedatatable, value); }
        }

        private int _bsample;
        public int BSamples
        {
            get { return _bsample; }
            set
            {
                if ((value > 0) & (value < 5001))
                    SetProperty(ref _bsample, value);
                else
                    SetProperty(ref _bsample, 100);

                Settings.Default.iDualGraphSamples = _bsample;
                Settings.Default.Save();
            }
        }

        private string _updateinfo;
        public string UpdateInfo
        {
            get { return _updateinfo; }
            set { SetProperty(ref _updateinfo, value); }
        }


        /// <summary>
        /// Sources ---------------------------------------------------------------------
        /// </summary>     
        private List<string> _SourceList;
        public List<string> SourceList
        {
            get { return _SourceList; }
            set { SetProperty(ref _SourceList, value); }
        }
        private int _SelectSourceIndex = 0;
        public int SelectSourceIndex
        {
            get { return _SelectSourceIndex; }
            set
            {
                SetProperty(ref _SelectSourceIndex, value);
                if (value > -1)
                {
                    CDualModel.m_Source = SourceList[value];
                    Settings.Default.iDropProfileSourceIndex = value;
                    Settings.Default.Save();
                }
            }
        }


        /// <summary>
        /// Line ---------------------------------------------------------------------------
        /// </summary>
        /// 
       // private string LineSelected;

        private List<string> _LineList;
        public List<string> LineList
        {
            get { return _LineList; }
            set { SetProperty(ref _LineList, value); }
        }
        private int _SelectLineIndex;
        public int SelectLineIndex
        {
            get { return _SelectLineIndex; }
            set
            {
                SetProperty(ref _SelectLineIndex, value);
                if (value > -1)
                {
                    CDualModel.m_Line = LineList[value];
                    Settings.Default.iDropProfileLineIndex = value;
                    Settings.Default.Save();
                }
            }
        }
        //-----------------------------------------------------------------------------------

        private int _MaxColumn;
        public int MaxColumn
        {
            get { return _MaxColumn; }
            set { SetProperty(ref _MaxColumn, value); }
        }



        public DualGraphViewModel(Prism.Events.IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;

            if (CDualModel != null) CDualModel = null;
            CDualModel = new DualGraphModel();

            // we assume this is called from the UI thread!
            SynchronizationContext _syncContext = SynchronizationContext.Current;

            LoadedPageICommand = new DelegateCommand(LoadedPageExecute, LoadedPageCanExecute);
            ClosedPageICommand = new DelegateCommand(ClosedPageExecute, ClosedPageCanExecute);

            StartCommand = new DelegateCommand(StartExecute, StartCanExecute).ObservesProperty(() => RTRunning);
            StopCommand = new DelegateCommand(StopExecute).ObservesCanExecute(() => RTRunning);

            ReDrawCommand = new DelegateCommand(ReDrawExecute, ReDrawCanExecute).ObservesProperty(() => RTRunning);

            MoistureType = ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Name;
            

            switch (Settings.Default.iLanguageIdx)
            {
                case 0: // "en-US":
                    WeightUnit = "Weight in " + ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name;
                    break;
                case 1: //"Sp-SP":
                    WeightUnit = "En Peso en " + ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name;
                    break;
                default:
                    WeightUnit = "Weight in " + ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name;
                    break;
            }

            _eventAggregator.GetEvent<UpdatedEventShutdown>().Subscribe(ProgramShutdown);

            ClsSerilog.LogMessage(ClsSerilog.Info, $"Initialize Dual Graphs");
        }

        private void ProgramShutdown(bool obj)
        {
            if (obj)
                if (dispatcherTimer != null)
                {
                    dispatcherTimer.Stop();
                    dispatcherTimer = null;
                }
        }

        private bool ReDrawCanExecute()
        {
            return !RTRunning;
        }

        private void ReDrawExecute()
        {
            LoadedPageExecute();
            _ = RedrawGraphsAsync();
        }


        private async Task RedrawGraphsAsync()
        {
            RealTimeDataTable = await CDualModel.GetNewGraphDataAsync();
            if (RealTimeDataTable.Rows.Count > 0)
                ProcessDatTable(RealTimeDataTable);
        }

        private bool ClosedPageCanExecute()
        {
            return true;
        }

        private void ClosedPageExecute()
        {
            //Clean up!
            if (CDualModel != null) CDualModel = null;
            if (dispatcherTimer != null) dispatcherTimer = null;
        }

        private bool LoadedPageCanExecute()
        {
            return true;
        }

        private void LoadedPageExecute()
        {
            try
            {
                if (CDualModel != null) CDualModel = null;
                CDualModel = new DualGraphModel();

                CDualModel.SetupWorkStation();

                CDualModel.InitSqlDualGraphModel();
                BSamples = Settings.Default.iDualGraphSamples;

                MoistureType = ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Name;
               
                switch (Settings.Default.iLanguageIdx)
                {
                    case 0: // "en-US":
                        MainWindow.AppWindows.SetupAppTitle("Forté Dual Graphs From  -> " + _sqlhandler.Host + @"\" + _sqlhandler.SqlInstance);
                        WeightUnit = "Weight in " + ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name;
                        break;
                    case 1: //"Sp-SP":
                        MainWindow.AppWindows.SetupAppTitle("Forté Gráfico Dual desde  -> " + _sqlhandler.Host + @"\" + _sqlhandler.SqlInstance);
                        WeightUnit = "En Peso en " + ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name;
                        break;
                    default:
                        MainWindow.AppWindows.SetupAppTitle("Forté Dual Graphs From  -> " + _sqlhandler.Host + @"\" + _sqlhandler.SqlInstance);
                        WeightUnit = "Weight in " + ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name;
                        break;
                }

                CDualModel.GetLineSource();

                if (LineList != null) LineList = null;
                LineList = new List<string>();
                LineList = CDualModel.m_LineList;

                if (LineList.Count > 0)
                    if (Settings.Default.iDropProfileLineIndex >= LineList.Count)
                        SelectLineIndex = 0;
                    else
                        SelectLineIndex = Settings.Default.iDropProfileLineIndex;

                if (SourceList != null) SourceList = null;
                SourceList = new List<string>();
                SourceList = CDualModel.m_SourceList;

                if (SourceList.Count > 0)
                    if (Settings.Default.iDropProfileSourceIndex >= SourceList.Count)
                        SelectSourceIndex = 0;
                    else
                        SelectSourceIndex = Settings.Default.iDropProfileSourceIndex;

                MaxColumn = BSamples + 1;
                CDualModel.ISampleCount = BSamples;
                /*
                RealTimeDataTable = CDualModel.GetNewGraphData();
                if (RealTimeDataTable.Rows.Count > 0)
                    ProcessDatTable(RealTimeDataTable);
                */
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in LoadedPageExecute  " + ex.Message);
            }
        }

        private void StopExecute()
        {
            Application.Current.Dispatcher.Invoke(new Action(() => { StopTimer(); }));
        }

        private bool StartCanExecute()
        {
            return !RTRunning;
        }

        private void StartExecute()
        {
            ShowMe = 0.1;
            Opac = 0.4;
            RTRunning = true;
            
            InitializeTimer();
            StartTimer();

            _eventAggregator.GetEvent<UpdatedEvent>().Publish(RTRunning);
        }


        /// <summary>
        /// Setup 
        /// </summary>
        private async Task GetNewDataAsync()
        {
            CDualModel.ISampleCount = BSamples;
            newIndex = CDualModel.GetNewIndex();

            if (preIndex != newIndex)
            {
                RealTimeDataTable = await CDualModel.GetNewGraphDataAsync();

                if (RealTimeDataTable.Rows.Count > 0)
                    ProcessDatTable(RealTimeDataTable);
                preIndex = newIndex;

                DateTimeOffset time = DateTimeOffset.Now;
                TimeSpan span = time - lastTime;
                lastTime = time;

                switch (Settings.Default.iLanguageIdx)
                {
                    case 0: // "en-US":
                        UpdateStatus("Status : New Bale arrived at : " + DateTime.Now.ToString() + " interval= " + span.ToString("c"));
                        break;
                    case 1: //"Sp-SP":
                        UpdateStatus("Estado : Actualizar datos @ " + DateTime.Now.ToString() + " intervalo = " + span.ToString("c"));
                        break;
                    default:
                        UpdateStatus("Status : New Bale arrived at : " + DateTime.Now.ToString() + " interval= " + span.ToString("c"));
                        break;
                }
            }
        }

        private void ProcessDatTable(DataTable realTimeDataTable)
        {
            double WCoef = 1.0;

            if (ItemsList != null) ItemsList = null;
            if (ItemsList2 != null) ItemsList2 = null;
            if (ItemsAvg != null) ItemsAvg = null;
            if (ItemsAvg2 != null) ItemsAvg2 = null;

            ItemsList = new ObservableCollection<KeyValuePair<Single, int>>();
            ItemsList2 = new ObservableCollection<KeyValuePair<Single, int>>();
            ItemsAvg = new ObservableCollection<KeyValuePair<Single, int>>();
            ItemsAvg2 = new ObservableCollection<KeyValuePair<Single, int>>();

            List<Single> MoistureList = new List<Single>();
            List<Single> WeightList = new List<Single>();

            try
            {
                int x = 1;

                CurMoisture = realTimeDataTable.Rows[0].Field<Single>("Moisture").ToString("#0.00");
                double CurrWt = realTimeDataTable.Rows[0].Field<Single>("Weight");
                CurWeight = (CurrWt * WCoef).ToString("#0.00");

                foreach (DataRow item in realTimeDataTable.Rows)
                {

                    //For Moisture graph               
                    ItemsList.Add(new KeyValuePair<Single, int>(item.Field<Single>("Moisture"), x));
                    MoistureList.Add(item.Field<Single>("Moisture"));

                    //For Weight Graph
                    ItemsList2.Add(new KeyValuePair<Single, int>((float)(item.Field<Single>("Weight") * WCoef), x));
                    WeightList.Add((float)(item.Field<Single>("Weight") * WCoef));
                    x += 1;
                }

                MoistureLow = MoistureList.Min().ToString("#0.00");
                MoistureHi = MoistureList.Max().ToString("#0.00");
                MoistureAVG = MoistureList.Average().ToString("#0.00");

                ItemsAvg.Clear();

                for (int i = 1; i < realTimeDataTable.Rows.Count; i++)
                {
                    ItemsAvg.Add(new KeyValuePair<Single, int>(MoistureList.Average(), i));
                }

                WeightLow = WeightList.Min().ToString("#0.00");
                WeightHi = WeightList.Max().ToString("#0.00");
                WeightAVG = WeightList.Average().ToString("#0.00");

                ItemsAvg2.Clear();
                for (int i = 1; i < realTimeDataTable.Rows.Count; i++)
                {
                    ItemsAvg2.Add(new KeyValuePair<Single, int>(WeightList.Average(), i));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in ProcessDatTable " + ex);
            }
        }


        private void Heartbeat()
        {
            if (ShowMe == 0.1) ShowMe = 1;
            else if (ShowMe == 1) ShowMe = 0.1;
        }


        private void UpdateStatus(string strMsg)
        {
            UpdateInfo = strMsg;
        }

        #region GraphData KeyValuePair


        private ObservableCollection<KeyValuePair<Single, int>> _itemonelist;
        public ObservableCollection<KeyValuePair<Single, int>> ItemsList
        {
            get { return _itemonelist; }
            set { SetProperty(ref _itemonelist, value); }
        }

        private ObservableCollection<KeyValuePair<Single, int>> _itemtwolist;
        public ObservableCollection<KeyValuePair<Single, int>> ItemsList2
        {
            get { return _itemtwolist; }
            set { SetProperty(ref _itemtwolist, value); }
        }

        private ObservableCollection<KeyValuePair<Single, int>> _itemthreelist;
        public ObservableCollection<KeyValuePair<Single, int>> ItemsAvg
        {
            get { return _itemthreelist; }
            set { SetProperty(ref _itemthreelist, value); }
        }

        private ObservableCollection<KeyValuePair<Single, int>> _itemfourlist;
        public ObservableCollection<KeyValuePair<Single, int>> ItemsAvg2
        {
            get { return _itemfourlist; }
            set { SetProperty(ref _itemfourlist, value); }
        }

        #endregion

        #region DispatcherTimer /////////////////////////////////////////////////////////////////////////////////////

        private System.Windows.Threading.DispatcherTimer dispatcherTimer;

        DateTimeOffset startTime;
        DateTimeOffset lastTime;

        private void InitializeTimer()
        {
            if (dispatcherTimer != null) dispatcherTimer = null;
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(Settings.Default.iScanRate)
            };
            dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
        }
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            Application.Current.Dispatcher.Invoke(new Action(() => { _ = GetNewDataAsync(); }));
            Application.Current.Dispatcher.Invoke(new Action(() => { Heartbeat(); }));

            Thread.Sleep(500); //Rest for 1/2 Sec.
            dispatcherTimer.Start();
        }
        private void StartTimer()
        {
            startTime = DateTimeOffset.Now;
            lastTime = startTime;
            dispatcherTimer.Start();
            switch (Settings.Default.iLanguageIdx)
            {
                case 0: // "en-US":
                    UpdateInfo = "Status : Scan timer Start";
                    break;
                case 1: //"Sp-SP":
                    UpdateInfo = "Estado : Tiempo de escaneo iniciado";
                    break;
                default:
                    UpdateInfo = "Status : Scan timer Start";
                    break;
            }
        }

        private void StopTimer()
        {
            dispatcherTimer.Stop();
            RTRunning = false;

            switch (Settings.Default.iLanguageIdx)
            {
                case 0: // "en-US":
                    Application.Current.Dispatcher.Invoke(new Action(() => { UpdateStatus("Status : Scan timer Stopped"); }));
                    break;
                case 1: //"Sp-SP":
                    Application.Current.Dispatcher.Invoke(new Action(() => { UpdateStatus("Estado : Tiempo de Scan Detenido"); }));
                    break;
                default:
                    Application.Current.Dispatcher.Invoke(new Action(() => { UpdateStatus("Status : Scan timer Stopped"); }));
                    break;
            }

            _eventAggregator.GetEvent<UpdatedEvent>().Publish(RTRunning);
            ShowMe = 0.1;
            Opac = 1.0;
        }

        #endregion DispatcherTimer /////////////////////////////////////////////////////////////////////////////////////////
    }
}
