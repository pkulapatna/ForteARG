

using ForteArg.Services;
using ForteARP.Charts;
using ForteARP.Module_WetLayer.Model;
using ForteARP.Properties;
using ForteARP.Reports.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace ForteARP.Module_WetLayer.ViewModels
{
    public class WetLayerViewModel : BindableBase
    {
        protected readonly Prism.Events.IEventAggregator _eventAggregator;
        private Sqlhandler _sqlhandler = Sqlhandler.Instance;

        public WetLayerModel cWetLayerModel;
        public Graph02 CVGraph;
        //private DateTime writecsvnew;
        //private DateTime writecsvPre;

        private DateTime PreBaleReadtime = DateTime.Now;
        private DateTime CurBaleReadtime = DateTime.Now;

        // we assume this ctor is called from the UI thread!
        //  private readonly SynchronizationContext _syncContext = SynchronizationContext.Current;

        public DelegateCommand LoadedPageICommand { get; set; }
        public DelegateCommand ClosedPageICommand { get; set; }
        public DelegateCommand StartCommand { get; set; }       //Start Button
        public DelegateCommand StopCommand { get; set; }        //Stop Button
        public DelegateCommand QueryCommand { get; set; }

        //Setting Tab
        public DelegateCommand ModifyCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand ApplyCommand { get; set; }

        //CSV
        public DelegateCommand ShowGraphCommand { get; set; }
        public DelegateCommand WriteCSVCommand { get; set; } //Wrive to CSV file
        public DelegateCommand CSVTestCommand { get; set; }
        public DelegateCommand BrowseCommand { get; set; }
        public DelegateCommand TimerStartCommand { get; set; }
        public DelegateCommand TimerStopCommand { get; set; }


        List<Tuple<long, string, double>> MyCVListX { get; set; }

        private DataTable _wetlayerDataTable;
        public DataTable WetLayerDataTable
        {
            get { return _wetlayerDataTable; }
            set { SetProperty(ref _wetlayerDataTable, value); }
        }


        private List<string> _wlmonthTableList;
        public List<string> WLMonthTableList
        {
            get { return _wlmonthTableList; }
            set { SetProperty(ref _wlmonthTableList, value); }
        }


        private bool _rtrunning = false;
        public bool RTRunning
        {
            get { return _rtrunning; }
            set
            {
                RTIdle = !value;
                _eventAggregator.GetEvent<UpdatedEvent>().Publish(value);
                SetProperty(ref _rtrunning, value);
            }
        }

        private bool _rptrunning = false;
        public bool RPTRunning
        {
            get { return _rptrunning; }
            set
            {
                RTIdle = !value;
                RPTIdle = !value;
                _eventAggregator.GetEvent<UpdatedEvent>().Publish(value);
                _eventAggregator.GetEvent<UpdatedWLEvent>().Publish(value);
                SetProperty(ref _rptrunning, value);
            }
        }
        private bool _rptIdle = true;
        public bool RPTIdle
        {
            get { return _rptIdle; }
            set { SetProperty(ref _rptIdle, value); }
        }


        private bool _rtIdle = true;
        public bool RTIdle
        {
            get { return _rtIdle; }
            set { SetProperty(ref _rtIdle, value); }
        }

        private int IScanTimer { get; set; }

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

        private int _btnZidxRt;
        public int BtnZidxRt
        {
            get { return _btnZidxRt; }
            set { SetProperty(ref _btnZidxRt, value); }
        }


        private double _rtopac = 1.0;
        public double RTOpac
        {
            get { return _rtopac; }
            set { SetProperty(ref _rtopac, value); }
        }

        private string _selectWLmonth;
        public string SelectWLmonth
        {
            get { return _selectWLmonth; }
            set { SetProperty(ref _selectWLmonth, value); }
        }

        private int _selecttableindex;
        public int SelectTableIndex
        {
            get { return _selecttableindex; }
            set { SetProperty(ref _selecttableindex, value); }
        }

        private int _selectOCRindex;
        public int SelectOCRIndex
        {
            get { return _selectOCRindex; }
            set { SetProperty(ref _selectOCRindex, value); }
        }

        public bool _MonthChecked;
        public bool MonthChecked
        {
            get { return _MonthChecked; }
            set
            {
                SetProperty(ref _MonthChecked, value);
                if (value)
                {
                    PreBaleReadtime = DateTime.Now;
                    MonthListEnable = true;
                    Opac = 1;
                    RTOpac = 0.0;
                }
            }
        }

        private bool _realtimechecked;
        public bool RealTimeChecked
        {
            get { return _realtimechecked; }
            set
            {
                if (value)
                {
                    MonthListEnable = false;
                    DayCheck = false;
                    Opac = 0.0;
                    RTOpac = 1;
                    BalerCheck = false;
                    BtnZidxRt = 99;
                }
                else
                {
                    PreBaleReadtime = DateTime.Now;
                    MonthListEnable = true;
                    Opac = 1;
                    RTOpac = 0.0;
                    BtnZidxRt = 0;
                }
                SetProperty(ref _realtimechecked, value);
            }
        }

        private bool _MonthListEnable;
        public bool MonthListEnable
        {
            get { return _MonthListEnable; }
            set { SetProperty(ref _MonthListEnable, value); }
        }

        private bool _dayCheck;
        public bool DayCheck
        {
            get { return _dayCheck; }
            set
            {
                if (value == false) SelectOCRIndex = 0;
                SetProperty(ref _dayCheck, value);
            }
        }

        private string _selectTableValue;
        public string SelectTableValue
        {
            get { return _selectTableValue; }
            set
            {
                SetProperty(ref _selectTableValue, value);
                StrFileName = value;
            }
        }

        /// <summary>
        /// Not used!
        /// </summary>
        private int _iWlSamples = Settings.Default.iWLSamples;
        public int IWLSamples
        {
            get { return _iWlSamples; }
            set
            {
                if ((value > 0) & (value < 2001))
                    SetProperty(ref _iWlSamples, value);
                else
                    SetProperty(ref _iWlSamples, 20);
            }
        }

        private object _selectedItem;
        public object SelectedValue
        {
            get { return _selectedItem; }
            set
            {
                SetProperty(ref _selectedItem, value);
            }
        }

        private int _selectWlData;
        public int SelectWlData
        {
            get { return _selectWlData; }
            set
            {
                SetProperty(ref _selectWlData, value);
                if (value > 0)
                {
                    DrawWetLayerChart(value);
                }
                //  else
                // DrawWetLayerChart(0);
            }
        }

        private Brush _ChartColor = Brushes.Red;
        public Brush ChartColor
        {
            get { return _ChartColor; }
            set { SetProperty(ref _ChartColor, value); }
        }


        private ObservableCollection<ChartData> wlChartList;
        public ObservableCollection<ChartData> WLChartList
        {
            get { return wlChartList; }
            set
            {
                SetProperty(ref wlChartList, value);
            }
        }


        private int _XColumns = 17;
        public int XColumns
        {
            get { return _XColumns; }
            set { SetProperty(ref _XColumns, value); }
        }


        private DataTable _wlDataTable;
        public DataTable MyWLDataTable
        {
            get { return _wlDataTable; }
            set { SetProperty(ref _wlDataTable, value); }
        }

        private bool _bModify = false;
        public bool BModify
        {
            get { return _bModify; }
            set { SetProperty(ref _bModify, value); }
        }
        private double _MinLimit;
        public double MinLimit
        {
            get { return _MinLimit; }
            set { SetProperty(ref _MinLimit, value); }
        }
        private double _MaxLimit;
        public double MaxLimit
        {
            get { return _MaxLimit; }
            set { SetProperty(ref _MaxLimit, value); }
        }
        private string _NormLimit;
        public string NormLimit
        {
            get { return _NormLimit; }
            set { SetProperty(ref _NormLimit, value); }
        }

        private string _MinYAxis;
        public string MinYAxis
        {
            get { return _MinYAxis; }
            set { SetProperty(ref _MinYAxis, value); }
        }
        private string _MaxYAxis;
        public string MaxYAxis
        {
            get { return _MaxYAxis; }
            set { SetProperty(ref _MaxYAxis, value); }
        }
        private int _YAxisInterval;
        public int YAxisInterval
        {
            get { return _YAxisInterval; }
            set { SetProperty(ref _YAxisInterval, value); }
        }

        private List<string> _ColorList;
        public List<string> ColorList
        {
            get { return _ColorList; }
            set { SetProperty(ref _ColorList, value); }
        }

        private List<Brush> BrushList;


        private Brush _GraphHiColor;
        public Brush GraphHiColor
        {
            get { return _GraphHiColor; }
            set { SetProperty(ref _GraphHiColor, value); }
        }

        private Brush _GraphNormColor;
        public Brush GraphNormColor
        {
            get { return _GraphNormColor; }
            set { SetProperty(ref _GraphNormColor, value); }
        }

        private Brush _GraphLowColor;
        public Brush GraphLowColor
        {
            get { return _GraphLowColor; }
            set { SetProperty(ref _GraphLowColor, value); }
        }

        private Brush _AlarmColor;
        public Brush AlarmColor
        {
            get { return _AlarmColor; }
            set { SetProperty(ref _AlarmColor, value); }
        }

        private string _AlarmMsg;
        public string AlarmMsg
        {
            get { return _AlarmMsg; }
            set { SetProperty(ref _AlarmMsg, value); }
        }

        private bool _bDataOnScreen;
        public bool BDataOnScreen
        {
            get { return _bDataOnScreen; }
            set { SetProperty(ref _bDataOnScreen, value); }
        }

        private bool _bWLDataReady = false;
        public bool BWLDataReady
        {
            get { return _bWLDataReady; }
            set { SetProperty(ref _bWLDataReady, value); }
        }


        private int _SampleBales = 20;
        public int RealTimeSamples
        {
            get { return _SampleBales; }
            set { SetProperty(ref _SampleBales, value); }
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

        private string _SelectDayEndTimeVal;
        public string SelectDayEndTimeVal
        {
            get { return Settings.Default.WLDayEndTime; }
            set
            {
                if (value != string.Empty)
                {
                    Settings.Default.WLDayEndTime = value;
                    Settings.Default.Save();
                }
                SetProperty(ref _SelectDayEndTimeVal, value);
            }
        }

        public string DayStartstr { get; set; }


        /// <summary>
        /// ////////////////////////////////////////////////////////
        /// </summary>
        private int _SelectHighIndex;
        public int SelectHighIndex
        {
            get { return _SelectHighIndex; }
            set { SetProperty(ref _SelectHighIndex, value); }
        }
        private int _SelectNormIndex;
        public int SelectNormIndex
        {
            get { return _SelectNormIndex; }
            set { SetProperty(ref _SelectNormIndex, value); }
        }
        private int _SelectLoIndex;
        public int SelectLoIndex
        {
            get { return _SelectLoIndex; }
            set { SetProperty(ref _SelectLoIndex, value); }
        }
        private int _SelectAlarmIndex;
        public int SelectAlarmIndex
        {
            get { return _SelectAlarmIndex; }
            set { SetProperty(ref _SelectAlarmIndex, value); }
        }
        private int _SelectOKIndex;
        public int SelectOKIndex
        {
            get { return _SelectOKIndex; }
            set { SetProperty(ref _SelectOKIndex, value); }
        }

        private bool _bTabOne = false;
        public bool BTabOne
        {
            get { return _bTabOne; }
            set { SetProperty(ref _bTabOne, value); }
        }
        private bool _bTabTwo = false;
        public bool BTabTwo
        {
            get { return _bTabTwo; }
            set { SetProperty(ref _bTabTwo, value); }
        }
        private bool _bTabThree = false;
        public bool BTabThree
        {
            get { return _bTabThree; }
            set { SetProperty(ref _bTabThree, value); }
        }

        //SelectedTabIndex

        /////////////////////////////////////////////////////////////

        private string _WLChartTitle;
        public string WLChartTitle
        {
            get { return _WLChartTitle; }
            set { SetProperty(ref _WLChartTitle, value); }
        }

        private string _TxtStatus;
        public string TxtStatus
        {
            get { return _TxtStatus; }
            set { SetProperty(ref _TxtStatus, value); }
        }


        //Balers////////////////////////////////////////////////
        private bool _BalerCheck;
        public bool BalerCheck
        {
            get { return _BalerCheck; }
            set
            {
                SetProperty(ref _BalerCheck, value);
                if (value)
                {
                    SelectBalerIndex = 0;
                }
                else
                    SelectBalerValue = "ALL";
            }
        }
        private List<string> _BalerList;
        public List<string> BalerList
        {
            get { return _BalerList; }
            set { SetProperty(ref _BalerList, value); }
        }
        private string _SelectBalerValue;
        public string SelectBalerValue
        {
            get { return _SelectBalerValue; }
            set { SetProperty(ref _SelectBalerValue, value); }
        }
        private int _SelectBalerIndex;
        public int SelectBalerIndex
        {
            get { return _SelectBalerIndex; }
            set { SetProperty(ref _SelectBalerIndex, value); }
        }


        private Nullable<DateTime> _startQueryDate = null;
        public Nullable<DateTime> StartQueryDate
        {
            get
            {
                if (_startQueryDate == null)
                    _startQueryDate = DateTime.Today;
                return _startQueryDate;
            }
            set { SetProperty(ref _startQueryDate, value); }
        }

        private Nullable<DateTime> _endQueryDate = null;
        public Nullable<DateTime> EndQueryDate
        {
            get
            {
                if (_endQueryDate == null)
                    _endQueryDate = DateTime.Today;

                if (_endQueryDate < _startQueryDate)
                    _endQueryDate = _startQueryDate;

                return _endQueryDate;
            }
            set { SetProperty(ref _endQueryDate, value); }
        }


        private string _DayEnd;
        public string DayEnd
        {
            get { return _DayEnd; }
            set { SetProperty(ref _DayEnd, value); }
        }
        ////////////////////////////////////////////////////////


        #region CSV 

        private string _strFileLocation;
        public string StrFileLocation
        {
            get { return _strFileLocation; }
            set
            {
                SetProperty(ref _strFileLocation, value);
                Settings.Default.CsvFileLocation = value;
                Settings.Default.Save();
            }
        }

        private string _StrFileName;
        public string StrFileName
        {
            get { return _StrFileName; }
            set { SetProperty(ref _StrFileName, value); }
        }

        private string _strPathFile;
        public string StrPathFile
        {
            get { return _strPathFile; }
            set { SetProperty(ref _strPathFile, value); }
        }

        private string _CSVTextMsg;
        public string CSVTextMsg
        {
            get { return _CSVTextMsg; }
            set { SetProperty(ref _CSVTextMsg, value); }
        }

        private string _CVScanInterval = "40";
        public string CVScanInterval
        {
            get { return _CVScanInterval; }
            set { SetProperty(ref _CVScanInterval, value); }
        }

        #endregion CSV



        /// <summary>
        /// Chart Colors;
        /// </summary>
        public Brush WLMinLimitColor { get; set; }
        public Brush WLMaxLimitColor { get; set; }
        public Brush WLNorLimitColor { get; set; }
        public Brush WLAlarmColor { get; set; }
        public Brush WLOKColor { get; set; }

        readonly int DefaultCount = 16;
        int iLayerCount = 0;

        bool bOutofLimitAlarmOn = false;
        private string StrCurrMonth;

        //int NewBaleId = 0;
        //int PreBaleId = 0;

        public struct CALC_RESULTS
        {
            public long BaleID;
            public int iBalerID;
            public string strBaler; //*10
            public double dDeviation;
            public double dAverage;
            public double dMaxValue;
            public double dMinValue;
            public int iNumbOfSpots;
            public string strResult; //*10
            public int[] iVals;
            public int iSize;
            public double[] dCalcResults;
            public List<double> dLayers;
            public int iLayers;
            public double dMoisture;
            public bool bAlarm;
            public bool bTCStampsAssigned;
        };

        string strLayer;


        private List<string> _occrlist;
        public List<string> Occrlist
        {
            get { return _occrlist; }
            set { SetProperty(ref _occrlist, value); }
        }

        private int _selectOccr;
        public int SelectOccr
        {
            get { return _selectOccr; }
            set { SetProperty(ref _selectOccr, value); }
        }

        private string _eventValue;
        public string EventValue
        {
            get { return _eventValue; }
            set
            {
                if (value == "All")
                    QuanEnable = false;
                else
                    QuanEnable = true;
                SetProperty(ref _eventValue, value);
            }
        }

        private bool _quanEnable;
        public bool QuanEnable
        {
            get { return _quanEnable; }
            set { SetProperty(ref _quanEnable, value); }
        }

        private int _recCount;
        public int RecCount
        {
            get { return _recCount; }
            set { SetProperty(ref _recCount, value); }
        }

        public WetLayerViewModel(Prism.Events.IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;

            LoadedPageICommand = new DelegateCommand(LoadedPageExecute, LoadedPageCanExecute);
            ClosedPageICommand = new DelegateCommand(ClosedPageExecute, ClosedPageCanExecute);

            StartCommand = new DelegateCommand(StartExecute, StartCanExecute).ObservesProperty(() => RealTimeChecked).ObservesProperty(() => RTIdle);
            StopCommand = new DelegateCommand(StopExecute, StopCanExecute).ObservesProperty(() => RTRunning);

            QueryCommand = new DelegateCommand(QueryExecute, QueryCanExecute).ObservesProperty(() => MonthListEnable).ObservesProperty(() => BTabOne);

            ModifyCommand = new DelegateCommand(ModifyExecute, ModifyCanExecute).ObservesProperty(() => BModify);
            CancelCommand = new DelegateCommand(CancelExecute, CancelCanExecute);
            ApplyCommand = new DelegateCommand(ApplyExecute, ApplyCanExecute).ObservesProperty(() => BModify);

            WriteCSVCommand = new DelegateCommand(WriteCsvExecute, WriteCsvCanExecute).ObservesProperty(() => BDataOnScreen).ObservesProperty(() => MonthChecked);
            ShowGraphCommand = new DelegateCommand(ShowGraphExecute, ShowGraphCanExecute).ObservesProperty(() => BDataOnScreen).ObservesProperty(() => MonthChecked);

            CSVTestCommand = new DelegateCommand(CsvTestExecute, CsvTestCanExecute).ObservesProperty(() => BWLDataReady).ObservesProperty(() => RPTRunning);
            BrowseCommand = new DelegateCommand(BrowseExecute, BrowseCanExecute).ObservesProperty(() => RPTRunning);

            TimerStartCommand = new DelegateCommand(TimerStartExecute, TimerStartCanExecute).ObservesProperty(() => RPTRunning);
            TimerStopCommand = new DelegateCommand(TimerStopExecute, TimerStopCanExecute).ObservesProperty(() => RPTRunning);

            _eventAggregator.GetEvent<UpdatedEventShutdown>().Subscribe(ProgramShutdown);

            Occrlist = new List<string>
            {
                "Latest",
                "Oldest",
                "All"
            };

            RecCount = 200;

            
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

        private void TimerStopExecute()
        {
            StopTimerCV();
            RPTRunning = false;
            CSVTextMsg = "CVTimer Stopped!";
        }

        private bool TimerStopCanExecute()
        {
            return RPTRunning; // (RTRunning || RPTRunning);
        }

        private void TimerStartExecute()
        {
            InitializeTimerCV();
            StartTimerCV();
            RPTRunning = true;
            CSVTextMsg = "CVTimer Started!";

            StrCurrMonth = cWetLayerModel.GetWLCurrMonth();

        }

        private bool TimerStartCanExecute()
        {
            return !RPTRunning;
        }

        private void BrowseExecute()
        {
            try
            {
                System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    StrFileLocation = dlg.SelectedPath;
                }
                dlg = null;
                FindCreateDir(StrFileLocation);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in BrowseExecute " + ex);
            }
        }

        private void FindCreateDir(string dirname)
        {
            try
            {
                if (!Directory.Exists(dirname))
                {
                    DirectoryInfo Di = Directory.CreateDirectory(dirname);
                    Di.Attributes = FileAttributes.ReadOnly;
                    Di.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in FindCreateDir " + ex);
            }
        }

        private bool BrowseCanExecute()
        {
            return !RPTRunning;
        }

        private void CsvTestExecute()
        {
            GetWlDatabyDay(Settings.Default.WLDayEndTime);
        }

        private bool CsvTestCanExecute()
        {
            return !RPTRunning;
        }

        private bool ShowGraphCanExecute()
        {
            return BDataOnScreen;
        }

        private void ShowGraphExecute()
        {
            CVGraph = new Graph02(MyCVListX);
            CVGraph.ShowDialog();
        }

        private bool WriteCsvCanExecute()
        {
            return BDataOnScreen;
        }

        private void WriteCsvExecute()
        {
           // List<string> indwxlist = new List<string>();
            int iStart = 9999;
            int iEnd = WetLayerDataTable.Rows.Count;

            try
            {
                using (CSVReport csvDialog = new CSVReport())
                {
                    if (WetLayerDataTable.Rows.Count > 0)
                    {
                        csvDialog.InitCsv(WetLayerDataTable, SelectTableValue, iStart, iEnd);
                        csvDialog.ShowDialog();
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("ERROR in WriteCsvExecute " + ex);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in WriteCsvExecute -> {ex.Message}");
              
            }
        }

        private void WriteLayersCSV(DataTable Mydatatable)
        {
            List<string> indwxlist = new List<string>();
            //int iStart = 9999;
            int iEnd = Mydatatable.Rows.Count;
            CSVTextMsg = "Start writing CSV file!";

            try
            {
                if (Mydatatable.Rows.Count > 0)
                {
                    StrPathFile = StrFileLocation + "\\" + StrFileName + ".csv";
                    StreamWriter outFile = new StreamWriter(StrPathFile);

                    List<string> headerValues = new List<string>();

                    foreach (DataColumn column in Mydatatable.Columns)
                    {
                        if (column.ColumnName == "Deviation")
                            headerValues.Add(QuoteValue("'" + "%CV"));
                        else if (column.ColumnName == "Param1")
                            headerValues.Add(QuoteValue("'" + "MAX"));
                        else if (column.ColumnName == "Param2")
                            headerValues.Add(QuoteValue("'" + "MIN"));
                        else if (column.ColumnName == "Moisture")
                        {
                            if (Settings.Default.MoistureUnit == 0)
                                headerValues.Add(QuoteValue("'" + "%MC"));
                            if (Settings.Default.MoistureUnit == 0)
                                headerValues.Add(QuoteValue("'" + "%MR"));
                        }
                            
                        else
                            headerValues.Add(QuoteValue("'" + column.ColumnName));
                    }

                    //Header
                    outFile.WriteLine(string.Join(",", headerValues.ToArray()));

                    foreach (DataRow row in Mydatatable.Rows)
                    {
                        string[] fields = row.ItemArray.Select(field => field.ToString()).ToArray();
                        outFile.WriteLine(String.Join(",", fields));
                    }
                    outFile.Close();
                    CSVTextMsg = "Write CSV file Done " + DateTime.Now.Date.ToString("MM/dd/yyyy");
                    ClsSerilog.LogMessage(ClsSerilog.Info, $"{CSVTextMsg}");
                }
                else
                    CSVTextMsg = "No Wet Layers Data!";
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in WriteCsvExecute " + ex);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in WriteCsvExecute -> {ex.Message}");
            }
        }

        private string QuoteValue(string value)
        {
            return string.Concat("" + value + "");
        }

        private bool ApplyCanExecute()
        {
            return BModify;
        }

        private void ApplyExecute()
        {
            //Save all changes
            BModify = false;
            SaveGraphSettings();
        }

        private bool CancelCanExecute()
        {
            return true;
        }

        private void CancelExecute()
        {
            // do nothing;
            BModify = false;
        }

        private bool ModifyCanExecute()
        {
            return !BModify;
        }

        private void ModifyExecute()
        {
            BModify = true;
        }

        private bool QueryCanExecute()
        {
            return BTabOne;
        }

        /// <summary>
        /// This is for Archives data
        /// </summary>
        private void QueryExecute()
        {
            GetArchiveWLData(SelectTableValue);
        }


        private void StartExecute()
        {
            TxtStatus = " Timer Started";

            InitializeTimer();
            StartTimer();

            IScanTimer = Settings.Default.iScanRate;
            RTRunning = true;
            //GetWLDataGridview(cWetLayerModel.GetWLCurrMonth(), true);

            ShowMe = 0.1;
            Opac = 0.0;

        }

        private bool StartCanExecute()
        {
            return RealTimeChecked && RTIdle && BTabOne;
        }

        private bool StopCanExecute()
        {

            return RTRunning || RPTRunning;
        }

        private void StopExecute()
        {
            TxtStatus = " Timer Stopped";
            Application.Current.Dispatcher.Invoke(new Action(() => { StopTimer(); }));
            RTRunning = false;
            ShowMe = 0.1;
            Opac = 0.0;
        }

        private bool ClosedPageCanExecute()
        {
            return true;
        }

        private void ClosedPageExecute()
        {


            if (dispatcherTimer != null) dispatcherTimer = null;
            if (dispatcherTimerCV != null) dispatcherTimerCV = null;

        }

        private bool LoadedPageCanExecute()
        {
            return true;
        }

        private void LoadedPageExecute()
        {
            try
            {

                if (cWetLayerModel != null) cWetLayerModel = null;
                cWetLayerModel = new WetLayerModel();

                if (SelectTableValue != null)
                    StrFileName = SelectTableValue;

                WLMonthTableList = cWetLayerModel.GetWLMonthList();
                StrCurrMonth = cWetLayerModel.GetWLCurrMonth();

                BalerList = cWetLayerModel.GetBalerList(SelectTableValue);
                if (BalerList.Count > 1)
                {
                    BalerList.Add("ALL");
                    SelectBalerValue = "ALL";
                }
                BalerCheck = false;

                SelectTableIndex = 0;
                SelectOCRIndex = 0;
                MonthChecked = true;
                BModify = false;

                SelectOccr = 0;

                GetColorandBrushLists();
                LoadGraphSettingtab();
                BDataOnScreen = false;
                TxtStatus = " Page Loaded";
                
                MinYAxis = Settings.Default.WLMinYAxis;
                MaxYAxis = Settings.Default.WLMaxYAxis;

                StrFileLocation = Settings.Default.CsvFileLocation;

                MainWindow.AppWindows.SetupAppTitle("Forté Wetlayer From  -> " + _sqlhandler.Host + @"\" + _sqlhandler.SqlInstance);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR IN LoadedPageExecute WetLayerViewModel " + ex);
            }
        }


        /// <summary>
        /// Query WL Data between 2 time period
        /// Readtime will changed after day ended!
        /// </summary>
        /// <param name="selectTableValue"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        private void GetWlDatabyDay(string TimeNowStr)
        {
            string StrWLTable = cWetLayerModel.GetWLCurrMonth();

            //1st day of the month use previous month datatable
            // a new month table should be created after the end of day.
            string Today = DateTime.Now.Date.ToString("dd");
            if (Today == "01") StrWLTable = cWetLayerModel.GetPreMonth();

            DateTime StartTime = Convert.ToDateTime(TimeNowStr).AddDays(-1);
            DateTime EndTime = Convert.ToDateTime(TimeNowStr);

            string strQuery = "SELECT * FROM " + StrWLTable + " with (NOLOCK) WHERE (ReadTime BETWEEN '" + StartTime + "' AND '" + EndTime + "')";

            DataTable DWLDataTable = cWetLayerModel.GetNewWLDataTable(StrWLTable, strQuery);
            if (DWLDataTable.Rows.Count > 0)
            {
                ProccessDataByDay(DWLDataTable); //Produce WetLayerDataTable
                if (DWLDataTable.Columns.Contains("Title"))
                    DWLDataTable.Columns.Remove("Title");
                WriteLayersCSV(WetLayerDataTable);
            }
            else
                CSVTextMsg = "No Data Found!";
        }


        private void GetArchiveWLData(string selectTableValue)
        {
            string strStartTime = "00:00";
            string strEndTime = "23:59";

            string strStartDate = _startQueryDate.Value.Date.ToString("MM/dd/yyyy") + " " + strStartTime; // Settings.Default.WLDayEndTime;
            string strEndDate = _endQueryDate.Value.Date.ToString("MM/dd/yyyy") + " " + strEndTime; // Settings.Default.WLDayEndTime;

            //string strBase = string.Empty;

            string strWlQuery;
            string strBaleID = string.Empty;
            string strone = string.Empty;
            string strQueryOccr;
            string Timeline = string.Empty;

            switch (EventValue)
            {
                case "All":
                    strone = "SELECT * From ";
                    Timeline = "DESC;";
                    break;
                case "Latest":
                    strone = "SELECT TOP " + RecCount + " * From ";
                    Timeline = "DESC;";
                    break;
                case "Oldest":
                    strone = "SELECT TOP " + RecCount + " * From ";
                    Timeline = "ASC;";
                    break;
            }

            if (BalerCheck)
            {
                strBaleID = "  WHERE BalerID = '" + SelectBalerValue + "'";
            }


            if (DayCheck)
            {
                strQueryOccr = " WHERE CAST(ReadTime AS DATETIME) BETWEEN '" + strStartDate + "' and '" + strEndDate + "'  ORDER BY ReadTime " + Timeline;
                if (BalerCheck)
                    strQueryOccr = " AND CAST(ReadTime AS DATETIME) BETWEEN '" + strStartDate + "' and '" + strEndDate + "'  ORDER BY ReadTime " + Timeline;
            }
            else
                strQueryOccr = " ORDER BY ReadTime " + Timeline;

            strWlQuery = strone + selectTableValue + " with (NOLOCK) " + strBaleID + " " + strQueryOccr;


            try
            {

                using (var Mytable = cWetLayerModel.GetNewWLDataTable(selectTableValue, strWlQuery))
                {
                    if (Mytable.Rows.Count > 0)
                    {
                        CurBaleReadtime = Convert.ToDateTime(Mytable.Rows[0]["ReadTime"].ToString());
                        if ((CurBaleReadtime != PreBaleReadtime) || MonthChecked == true)
                        {
                            ProccessData(Mytable);
                            TxtStatus = " Number of Bales =  " + Mytable.Rows.Count;
                        }
                    }
                    else
                        TxtStatus = "No Bale Data to show in the DataGrid";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in GetWLDataGridview " + ex.Message);
            }
        }

        /// <summary>
        /// Query by month or a day in the month by selectable 24 hours DayEndTime
        /// stored in Settings.Default.WLDayEndTime
        /// </summary>
        /// <param name="selectTableValue"></param>
        /// <param name="bRTMode"></param>
        private void GetWLDataGridview(string selectTableValue, bool bRTMode)
        {
            string strBaleID = string.Empty;
            string strQueryOccr = string.Empty;
            string strWlQuery;

            List<string> SelMonthlst;
            List<DataTable> MyTables;

            bool bMultimonth = false;

            if (bRTMode == false)  //Archives data
            {
                if (BalerCheck)
                    strBaleID = "  WHERE BalerID = '" + SelectBalerValue + "'";

                //Check EventValue strEvents
                // 1 Occurrences 
                if (EventValue == "All")
                    strWlQuery = "SELECT ";
                else
                    strWlQuery = "SELECT TOP " + RecCount;


                if (MonthChecked)
                {
                    strWlQuery = "SELECT * FROM " + selectTableValue + " with (NOLOCK) " + strBaleID + " ORDER BY ReadTime DESC; ";
                }
                else if (DayCheck) //Day check can go across months
                {
                    string strStartDate = _startQueryDate.Value.Date.ToString("MM/dd/yyyy") + " " + Settings.Default.WLDayEndTime;
                    string strEndDate = _endQueryDate.Value.Date.ToString("MM/dd/yyyy") + " " + Settings.Default.WLDayEndTime;

                    if (BalerCheck)
                    {
                        strBaleID = "  WHERE BalerID = '" + SelectBalerValue + "'";
                    }

                    // Incase of more than one months
                    if (_endQueryDate.Value.Month > _startQueryDate.Value.Month)
                    {
                        bMultimonth = true;
                        SelMonthlst = new List<string>();
                        MyTables = new List<DataTable>();

                        try
                        {
                            for (int i = 0; i < ((_endQueryDate.Value.Month + 1) - _startQueryDate.Value.Month); i++)
                            {
                                SelMonthlst.Add(ClassCommon.WetTabMonth[_startQueryDate.Value.Month - 1 + i] + _startQueryDate.Value.ToString("yy"));
                            }

                            for (int i = 0; i < SelMonthlst.Count; i++)
                            {
                                string strWlQueryX = "SELECT * FROM " + SelMonthlst[i] + " with (NOLOCK);";
                                MyTables.Add(cWetLayerModel.GetNewWLDataTable(SelMonthlst[i], strWlQueryX));
                            }
                            DataTable datAll = new DataTable();
                            datAll = MyTables[0].Copy();
                            for (int i = 1; i < MyTables.Count; i++)
                            {
                                datAll.Merge(MyTables[i]);
                                datAll.AcceptChanges();
                            }

                            if (datAll.Rows.Count > 0)
                            {
                                DateTime DTrRemove;
                                foreach (DataRow row in datAll.Rows)
                                {
                                    DTrRemove = Convert.ToDateTime(row["ReadTime"].ToString());
                                    if (DTrRemove.Date < _startQueryDate.Value.Date)
                                    {
                                        row.Delete();
                                    }
                                }
                                datAll.AcceptChanges();

                                foreach (DataRow row in datAll.Rows)
                                {
                                    DTrRemove = Convert.ToDateTime(row["ReadTime"].ToString());
                                    if (DTrRemove.Date > _endQueryDate.Value.Date)
                                    {
                                        row.Delete();
                                    }
                                }
                                datAll.AcceptChanges();

                                if (datAll.Rows.Count > 0)
                                {
                                    ProccessData(datAll);
                                    TxtStatus = " With Multiple Months, Number of Bales =  " + datAll.Rows.Count;
                                }
                            }
                            else
                                TxtStatus = "No Bale Data to show in the DataGrid";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR in GetWLDataGridview Multiple Month" + ex.Message);
                        }
                    }
                    else
                    {
                        strQueryOccr = " WHERE CAST(ReadTime AS DATETIME) BETWEEN '" + strStartDate + "' and '" + strEndDate + "'  ORDER BY ReadTime DESC;";
                        strWlQuery = "SELECT * FROM " + selectTableValue + " with (NOLOCK) " + strBaleID + " " + strQueryOccr;
                    }
                }
            }
            else
                strWlQuery = "SELECT TOP 20 * FROM " + selectTableValue + " with (NOLOCK) " + strBaleID + " " + strQueryOccr + " ORDER BY ReadTime DESC;";

            try
            {
                Heartbeat(); //Timer
                if (!bMultimonth)
                {
                    using (var Mytable = cWetLayerModel.GetNewWLDataTable(selectTableValue, strWlQuery))
                    {
                        if (Mytable.Rows.Count > 0)
                        {
                            CurBaleReadtime = Convert.ToDateTime(Mytable.Rows[0]["ReadTime"].ToString());
                            if ((CurBaleReadtime != PreBaleReadtime) || MonthChecked == true)
                            {
                                ProccessData(Mytable);
                                TxtStatus = " Timer Status: New Bale Arrived at " + DateTime.Now + " Number of Bales=  " + Mytable.Rows.Count;
                            }
                        }
                        else
                            TxtStatus = "No Bale Data to show in the DataGrid";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in GetWLDataGridview " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in GetWLDataGridview -> {ex.Message}");
                
            }
        }

        private void ProccessData(DataTable MyTable)
        {
            List<double> fLayerVal;

            try
            {
                if (MyTable.Rows.Count > 0)
                {
                    MyCVListX = new List<Tuple<long, string, double>>();
                    BDataOnScreen = true;

                    if (string.IsNullOrEmpty(MyTable.Rows[0]["Layers"].ToString()))
                        iLayerCount = DefaultCount;
                    else
                        iLayerCount = MyTable.Rows[0].Field<int>("Layers");

                    WetLayerDataTable = new DataTable();
                    //Added for graph title
                    MyTable.Columns.Add("Title", typeof(string));
                    MyTable.AcceptChanges();

                    foreach (DataRow dtRow in MyTable.Rows)
                    {
                        CALC_RESULTS StructLast = new CALC_RESULTS();
                        fLayerVal = new List<double>();

                        for (int i = 1; i < iLayerCount + 1; i++)
                        {
                            strLayer = "Layer" + i.ToString();
                            if (!string.IsNullOrEmpty(dtRow[strLayer].ToString()) | (dtRow[strLayer].GetType().Name == "Double"))
                            {
                                if (Settings.Default.MoistureUnit == 0)
                                    fLayerVal.Add(dtRow.Field<Double>(strLayer));
                                if (Settings.Default.MoistureUnit == 1)
                                    fLayerVal.Add(ConvToMR(dtRow.Field<Double>(strLayer)));
                            }
                                
                        }

                        if (fLayerVal.Count > 0)
                        {
                            CalCVMinMax(fLayerVal, iLayerCount, out StructLast);

                            if (!string.IsNullOrEmpty(dtRow["Moisture"].ToString()) | (dtRow["Moisture"].GetType().Name == "Double"))
                            {
                                //Settings.Default.MoistureUnit

                                if (Settings.Default.MoistureUnit == 0)
                                    dtRow["Moisture"] = dtRow.Field<Double>("Moisture").ToString("#0.00");
                                else if (Settings.Default.MoistureUnit == 1)
                                    dtRow["Moisture"] = ConvToMR(dtRow.Field<Double>("Moisture")).ToString("#0.00");
                            }
                            dtRow["Deviation"] = StructLast.dDeviation.ToString("#0.00");
                            dtRow["Param1"] = StructLast.dMaxValue.ToString("#0.00");
                            dtRow["Param2"] = StructLast.dMinValue.ToString("#0.00");

                            //For CV Graph data list of Tuple<int, string, double>
                            MyCVListX.Add(new Tuple<long, string, double>(dtRow.Field<long>("ID"),
                                dtRow.Field<DateTime>("ReadTime").ToString(), dtRow.Field<Double>("Deviation")));

                            for (int i = 1; i < iLayerCount + 1; i++)
                            {
                                strLayer = "Layer" + i.ToString();
                                dtRow[strLayer] = StructLast.dLayers[i - 1].ToString("#0.00");
                            }
                            //Graph Title
                            dtRow["Title"] = dtRow["ReadTime"] + " - Baler " + dtRow["BalerID"] + ", Number - " + dtRow["ID"];
                        }
                    }
                    //Removed Un wanted items.
                    if (MyTable.Columns.Contains("Time1"))
                        MyTable.Columns.Remove("Time1");
                    if (MyTable.Columns.Contains("Time2"))
                        MyTable.Columns.Remove("Time2");
                    if (MyTable.Columns.Contains("Time3"))
                        MyTable.Columns.Remove("Time3");
                    if (MyTable.Columns.Contains("MaximumPrc"))
                        MyTable.Columns.Remove("MaximumPrc");
                    if (MyTable.Columns.Contains("Layer17"))
                        MyTable.Columns.Remove("Layer17");
                    if (MyTable.Columns.Contains("Layer18"))
                        MyTable.Columns.Remove("Layer18");
                    if (MyTable.Columns.Contains("Layer19"))
                        MyTable.Columns.Remove("Layer19");
                    if (MyTable.Columns.Contains("Layer20"))
                        MyTable.Columns.Remove("Layer20");
                    if (MyTable.Columns.Contains("Layer21"))
                        MyTable.Columns.Remove("Layer21");
                    if (MyTable.Columns.Contains("Layer22"))
                        MyTable.Columns.Remove("Layer22");
                    if (MyTable.Columns.Contains("Layer23"))
                        MyTable.Columns.Remove("Layer23");
                    if (MyTable.Columns.Contains("Layer24"))
                        MyTable.Columns.Remove("Layer24");
                    if (MyTable.Columns.Contains("Layer25"))
                        MyTable.Columns.Remove("Layer25");
                    if (MyTable.Columns.Contains("Layer26"))
                        MyTable.Columns.Remove("Layer26");
                    if (MyTable.Columns.Contains("Layer27"))
                        MyTable.Columns.Remove("Layer27");
                    if (MyTable.Columns.Contains("Layer28"))
                        MyTable.Columns.Remove("Layer28");
                    if (MyTable.Columns.Contains("Layer29"))
                        MyTable.Columns.Remove("Layer29");
                    if (MyTable.Columns.Contains("Layer30"))
                        MyTable.Columns.Remove("Layer30");
                    if (MyTable.Columns.Contains("Layers"))
                        MyTable.Columns.Remove("Layers");

                    if (MyTable.Columns.Contains("Title"))
                        MyTable.Columns.Remove("Title");

                    MyTable.AcceptChanges();

                    WetLayerDataTable = MyTable;
                    DrawWetLayerChart(0);

                    if (RealTimeChecked)
                        PreBaleReadtime = CurBaleReadtime;
                }
                else
                    TxtStatus = "! - No Data Found for this table - !";
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in ProccessData " + ex);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in ProccessData -> {ex.Message}");
            }
        }



        /// <summary>
        /// Remove unwanted fileds and calulate values from datatable
        /// then populate DataGrid
        /// </summary>
        /// <param name="MyTable"></param>
        private void ProccessDataByDay(DataTable MyTable)
        {
            List<double> fLayerVal;

            try
            {
                if (MyTable.Rows.Count > 0)
                {
                    MyCVListX = new List<Tuple<long, string, double>>();

                    if (MyTable.Rows[0]["Layers"] == null)
                        iLayerCount = DefaultCount;
                    else
                        iLayerCount = MyTable.Rows[0].Field<int>("Layers");

                    WetLayerDataTable = new DataTable();
                    //Added for graph title
                    MyTable.Columns.Add("Title", typeof(string));

                    foreach (DataRow dtRow in MyTable.Rows)
                    {
                        CALC_RESULTS StructLast = new CALC_RESULTS();
                        fLayerVal = new List<double>();

                        for (int i = 1; i < iLayerCount + 1; i++)
                        {
                            strLayer = "Layer" + i.ToString();
                            var field1 = dtRow[strLayer].ToString();
                            
                            if (Settings.Default.MoistureUnit == 0)
                                fLayerVal.Add(Convert.ToDouble(dtRow[strLayer].ToString()));
                            if (Settings.Default.MoistureUnit == 1)
                                fLayerVal.Add(ConvToMR(Convert.ToDouble(dtRow[strLayer].ToString())));
                        }

                        CalCVMinMax(fLayerVal, iLayerCount, out StructLast);

                        dtRow["Moisture"] = dtRow.Field<Single>("Moisture").ToString("#0.00");          
                        
                        dtRow["Deviation"] = StructLast.dDeviation.ToString("#0.00");
                        dtRow["Param1"] = StructLast.dMaxValue.ToString("#0.00");
                        dtRow["Param2"] = StructLast.dMinValue.ToString("#0.00");

                        //For CV Graph data list of Tuple<int, string, double>
                        MyCVListX.Add(new Tuple<long, string, double>(dtRow.Field<long>("ID"),
                            dtRow.Field<DateTime>("ReadTime").ToString(), dtRow.Field<double>("Deviation")));

                        for (int i = 1; i < iLayerCount + 1; i++)
                        {
                            strLayer = "Layer" + i.ToString();
                            dtRow[strLayer] = StructLast.dLayers[i - 1].ToString("#0.00");
                        }

                        //Graph Title
                        dtRow["Title"] = dtRow["ReadTime"] + " - Baler " + dtRow["BalerID"] + ", Number - " + dtRow["ID"];
                    }

                    MyTable.Columns.Remove("Time1");
                    MyTable.Columns.Remove("Time2");
                    MyTable.Columns.Remove("Time3");
                    MyTable.Columns.Remove("MaximumPrc");
                    MyTable.Columns.Remove("Layer17");
                    MyTable.Columns.Remove("Layer18");
                    MyTable.Columns.Remove("Layer19");
                    MyTable.Columns.Remove("Layer20");
                    MyTable.Columns.Remove("Layer21");
                    MyTable.Columns.Remove("Layer22");
                    MyTable.Columns.Remove("Layer23");
                    MyTable.Columns.Remove("Layer24");
                    MyTable.Columns.Remove("Layer25");
                    MyTable.Columns.Remove("Layer26");
                    MyTable.Columns.Remove("Layer27");
                    MyTable.Columns.Remove("Layer28");
                    MyTable.Columns.Remove("Layer29");
                    MyTable.Columns.Remove("Layer30");
                    MyTable.Columns.Remove("Layers");

                    WetLayerDataTable = MyTable;
                    DrawWetLayerChart(0);
                }
                else
                    TxtStatus = "! - No Data Found for this table - !";
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in ProccessDataByDay " + ex);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in ProccessDataByDay -> {ex.Message}");
            }
        }


        private void DrawWetLayerChart(int selectedrow)
        {
            MinYAxis = Settings.Default.WLMinYAxis;
            MaxYAxis = Settings.Default.WLMaxYAxis;


            if (WLChartList != null) WLChartList = null;
            WLChartList = new ObservableCollection<ChartData>();

            WLChartList.Clear();
            bOutofLimitAlarmOn = false;

            ///$$$PK set maximum to 16 Layers only
            for (int x = 1; x < iLayerCount + 1; x++)
            {
                if ((WetLayerDataTable.Rows[selectedrow]["Layer" + x].ToString() != string.Empty) & (WetLayerDataTable != null))
                {
                    double iDouble = WetLayerDataTable.Rows[selectedrow].Field<double>("Layer" + x);
                    WLChartList.Add(new ChartData() { Index = x, Value = iDouble, ChartColor = SetChartColors(iDouble) });
                }
            }

            if (bOutofLimitAlarmOn)
            {
                AlarmColor = BrushList[SelectAlarmIndex];
                AlarmMsg = "Alarm";
            }
            else
            {
                AlarmColor = BrushList[SelectOKIndex];
                AlarmMsg = "Ok";
            }
        }

        //Set the color of the graph by Hi Low or Good moisture values
        private Brush SetChartColors(double DBaleValue)
        {
            Brush crtColor;

            if (DBaleValue > MaxLimit)
            {
                crtColor = GraphHiColor;
                bOutofLimitAlarmOn = true;
            }
            else if (DBaleValue < MinLimit)
            {
                crtColor = GraphLowColor;
                bOutofLimitAlarmOn = true;
            }
            else
            {
                crtColor = GraphNormColor;
            }
            return crtColor;
        }


        private double ConvToMR(double fMoisture)
        {
            return fMoisture / (1 - fMoisture / 100);
        }

        private void CalCVMinMax(List<double> SampleList, int iLayers, out CALC_RESULTS tResults)
        {
            tResults = new CALC_RESULTS();
            double sumOfDerivation = 0;

            //Average
            tResults.dAverage = SampleList.Average();

            //Min Max
            tResults.dMinValue = SampleList.Min();
            tResults.dMaxValue = SampleList.Max();

            //layers
            tResults.dLayers = new List<Double>();
            tResults.dLayers = SampleList;

            //Deviation
            tResults.bAlarm = false;
            foreach (var value in SampleList)
            {
                sumOfDerivation += (value - tResults.dAverage) * (value - tResults.dAverage);
            }

            //STD
            double Variance = sumOfDerivation / (SampleList.Count - 1);
            double StandardDeviation = Math.Sqrt(Variance);

            //%CV (Coefficient of Variation = Standard Deviation / Mean)
            tResults.dDeviation = StandardDeviation / tResults.dAverage * 100;
            tResults.bAlarm = false;

        }

        //For Graph Settings.
        private void LoadGraphSettingtab()
        {
            MinLimit = Settings.Default.WLMinLimit;
            MaxLimit = Settings.Default.WLMaxLimit;

            MinYAxis = Settings.Default.WLMinYAxis;
            MaxYAxis = Settings.Default.WLMaxYAxis;

            YAxisInterval = Settings.Default.WLYAxisInterval;

            SelectHighIndex = Settings.Default.WLHiColorIndex;
            SelectNormIndex = Settings.Default.WLNormColorIndex;
            SelectLoIndex = Settings.Default.WLLoColorIndex;
            SelectAlarmIndex = Settings.Default.WLAlarmIndex;
            SelectOKIndex = Settings.Default.WLOkIndex;

            GraphHiColor = BrushList[SelectHighIndex];
            GraphNormColor = BrushList[SelectNormIndex];
            GraphLowColor = BrushList[SelectLoIndex];

            NormLimit = MinLimit.ToString() + " to " + MaxLimit.ToString();
        }

        private void SaveGraphSettings()
        {
            Settings.Default.WLMinLimit = MinLimit;
            Settings.Default.WLMaxLimit = MaxLimit;

            Settings.Default.WLMinYAxis = MinYAxis;
            Settings.Default.WLMaxYAxis = MaxYAxis;

            Settings.Default.WLYAxisInterval = YAxisInterval;

            Settings.Default.WLHiColorIndex = SelectHighIndex;
            Settings.Default.WLNormColorIndex = SelectNormIndex;
            Settings.Default.WLLoColorIndex = SelectLoIndex;
            Settings.Default.WLAlarmIndex = SelectAlarmIndex;
            Settings.Default.WLOkIndex = SelectOKIndex;

            NormLimit = MinLimit.ToString() + " to " + MaxLimit.ToString();

            Settings.Default.Save();

            GraphHiColor = BrushList[SelectHighIndex];
            GraphNormColor = BrushList[SelectNormIndex];
            GraphLowColor = BrushList[SelectLoIndex];
        }


        private void GetColorandBrushLists()
        {

            ColorList = new List<string>
            {
                "BlanchedAlmond",
                "Red",
                "Yellow",
                "Blue",
                "Green",
                "Brown",
                "Gray",
                "Puple",
                "Pink",
                "Orange",
                "Olive",
                "White",
                "Beige",
                "SlateGray",
                "SpringGreen",
                "LightGreen",
                "LightSteelBlue",
                "Salmon",
                "Azure",
                "Aqua",
                "Aquamarine"
            };


            BrushList = new List<Brush>
            {
                Brushes.BlanchedAlmond,
                Brushes.Red,
                Brushes.Yellow,
                Brushes.Blue,
                Brushes.Green,
                Brushes.Brown,
                Brushes.Gray,
                Brushes.Purple,
                Brushes.Pink,
                Brushes.Orange,
                Brushes.Olive,
                Brushes.White,
                Brushes.Beige,
                Brushes.SlateGray,
                Brushes.SpringGreen,
                Brushes.LightGreen,
                Brushes.LightSteelBlue,
                Brushes.Salmon,
                Brushes.Azure,
                Brushes.Aqua,
                Brushes.Aquamarine
            };
        }

        private void Heartbeat()
        {
            if (ShowMe == 0.1) ShowMe = 1;
            else if (ShowMe == 1) ShowMe = 0.1;
        }

        public List<string> GetBalerList()
        {
            List<string> Blist = new List<string>();
            return Blist;
        }

        #region DispatcherTimer /////////////////////////////////////////////////////////////////////////////////////

        private System.Windows.Threading.DispatcherTimer dispatcherTimer;

        /// <summary>
        /// System.Windows.Threading.DispatcherTimer
        /// </summary>
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

            Application.Current.Dispatcher.Invoke(new Action(() => { GetWLDataGridview(cWetLayerModel.GetWLCurrMonth(), true); }));
            // Application.Current.Dispatcher.Invoke(new Action(() => { Heartbeat(); }));

            Thread.Sleep(1000); //Rest for 1 Sec.
            dispatcherTimer.Start();
        }
        private void StartTimer()
        {
            dispatcherTimer.Start();
        }
        private void StopTimer()
        {

            if (dispatcherTimer != null)
            {
                dispatcherTimer.Stop();
                ShowMe = 0.1;
                Opac = 1.0;
            }

        }

        #endregion DispatcherTimer ////////////////////////////////////////////////////////////////////////////////////

        #region DispatcherTimerCV /////////////////////////////////////////////////////////////////////////////////////

        private System.Windows.Threading.DispatcherTimer dispatcherTimerCV;
        public string TimeNow { get; set; }

        /// <summary>
        /// 
        /// System.Windows.Threading.DispatcherTimer
        /// trigger by ProdDayEnd == timenow time in minute
        /// Scan time should be less than 1/2 of a minute, inorder to catch the right time.
        /// Now it is set to 20 mSec.
        /// 
        /// </summary>
        private void InitializeTimerCV()
        {
            if (dispatcherTimerCV != null) dispatcherTimerCV = null;
            dispatcherTimerCV = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(40) //40 Sec. CVScanInterval
            };
            dispatcherTimerCV.Tick += new EventHandler(DispatcherTimerCV_Tick);
        }

        private void DispatcherTimerCV_Tick(object sender, EventArgs e)
        {
            StopTimerCV();
            Application.Current.Dispatcher.Invoke(new Action(() => { Heartbeat(); }));
            TimeNow = DateTime.Now.ToString("HH:mm");

            if (Settings.Default.WLDayEndTime == TimeNow)
            {
                Application.Current.Dispatcher.Invoke(new Action(() => { GetWlDatabyDay(TimeNow); }));
            }
            Thread.Sleep(1000); //Rest for 1 Sec.
            StartTimerCV();
        }

        private void StartTimerCV()
        {
            dispatcherTimerCV.Start();
        }

        private void StopTimerCV()
        {
            dispatcherTimerCV.Stop();
        }


        #endregion DispatcherTimerCV //////////////////////////////////////////////////////////////////////////////////




    }


}
