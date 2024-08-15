
using ForteArg.Services;
using ForteARP.Model;
using ForteARP.Module_Graphs.Model;
using ForteARP.Module_Graphs.Views;
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
using System.Windows.Media;

namespace ForteARP.Module_Graphs.ViewModels
{
    public class RealTimeGraphViewModel : BindableBase
    {
        public RealTimeGraphModel CGraphModel;
        protected readonly Prism.Events.IEventAggregator _eventAggregator;
        private Sqlhandler _sqlhandler = Sqlhandler.Instance;
        private long preIndex = 0;
        private long newIndex = 0;
        private Window GrapgSetupWindow;
        public List<string> MenuOptions { get; set; }

        #region GraphData KeyValuePair

        private Brush _backGroundChartColor;
        public Brush BackGroundChartColor
        {
            get { return _backGroundChartColor; }
            set { SetProperty(ref _backGroundChartColor, value); }
        }

        private int _backgndchartidx = Settings.Default.backgndchartidx;
        public int Backgndchartidx
        {
            get { return _backgndchartidx; }
            set { SetProperty(ref _backgndchartidx, value); }
        }

        private List<Brush> _backgndchartLst;
        public List<Brush> BackgndchartLst
        {
            get { return _backgndchartLst; }
            set { SetProperty(ref _backgndchartLst, value); }
        }

        private string _legTitle = "Drop(s)"; 
        public string LegTitle
        {
            get { return _legTitle; }
            set { SetProperty(ref _legTitle, value); }
        }

        private List<string> _graphByLst;
        public List<string> GraphByLst
        {
            get { return _graphByLst; }
            set { SetProperty(ref _graphByLst, value); }
        }

        private ObservableCollection<KeyValuePair<Single, int>> _pos1;
        public ObservableCollection<KeyValuePair<Single, int>> Pos1
        {
            get { return _pos1; }
            set { SetProperty(ref _pos1, value); }
        }

        private ObservableCollection<KeyValuePair<Single, int>> _pos2;
        public ObservableCollection<KeyValuePair<Single, int>> Pos2
        {
            get { return _pos2; }
            set { SetProperty(ref _pos2, value); }
        }

        private ObservableCollection<KeyValuePair<Single, int>> _pos3;
        public ObservableCollection<KeyValuePair<Single, int>> Pos3
        {
            get { return _pos3; }
            set { SetProperty(ref _pos3, value); }
        }

        private ObservableCollection<KeyValuePair<Single, int>> _pos4;
        public ObservableCollection<KeyValuePair<Single, int>> Pos4
        {
            get { return _pos4; }
            set { SetProperty(ref _pos4, value); }
        }

        private ObservableCollection<KeyValuePair<Single, int>> _pos5;
        public ObservableCollection<KeyValuePair<Single, int>> Pos5
        {
            get { return _pos5; }
            set { SetProperty(ref _pos5, value); }
        }

        private ObservableCollection<KeyValuePair<Single, int>> _pos6;
        public ObservableCollection<KeyValuePair<Single, int>> Pos6
        {
            get { return _pos6; }
            set { SetProperty(ref _pos6, value); }
        }

        private ObservableCollection<KeyValuePair<Single, int>> _itemsMax;
        public ObservableCollection<KeyValuePair<Single, int>> ItemsMax
        {
            get { return _itemsMax; }
            set { SetProperty(ref _itemsMax, value); }
        }

        private ObservableCollection<KeyValuePair<Single, int>> _itemsMin;
        public ObservableCollection<KeyValuePair<Single, int>> ItemsMin
        {
            get { return _itemsMin; }
            set { SetProperty(ref _itemsMin, value); }
        }

        private Visibility _showBoxOne = Visibility.Hidden;
        public Visibility ShowBoxOne
        {
            get { return _showBoxOne; }
            set { SetProperty(ref _showBoxOne, value); }
        }

        private Visibility _showBoxTwo = Visibility.Hidden;
        public Visibility ShowBoxTwo
        {
            get { return _showBoxTwo; }
            set { SetProperty(ref _showBoxTwo, value); }
        }

        private Visibility _showBoxthree = Visibility.Hidden;
        public Visibility ShowBoxThree
        {
            get { return _showBoxthree; }
            set { SetProperty(ref _showBoxthree, value); }
        }

        private Visibility _showBoxFour = Visibility.Hidden;
        public Visibility ShowBoxFour
        {
            get { return _showBoxFour; }
            set { SetProperty(ref _showBoxFour, value); }
        }

        private Visibility _showBoxFive = Visibility.Hidden;
        public Visibility ShowBoxFive
        {
            get { return _showBoxFive; }
            set { SetProperty(ref _showBoxFive, value); }
        }

        private Visibility _showBoxSix = Visibility.Hidden;
        public Visibility ShowBoxSix
        {
            get { return _showBoxSix; }
            set { SetProperty(ref _showBoxSix, value); }
        }

        #endregion GraphData

        //--------------------------------------------------------/

        private DelegateCommand _loadedPageICommand;
        public DelegateCommand LoadedPageICommand =>
        _loadedPageICommand ?? (_loadedPageICommand = new DelegateCommand(LoadedPageExecute));
        private void LoadedPageExecute()
        {
            ShowMe = 0.1;

            if (CGraphModel != null) CGraphModel = null;
            CGraphModel = new RealTimeGraphModel();
            CGraphModel.InitSqlRealTimeGraphModel();

            Line = CGraphModel.GetLineCount();
            Source = CGraphModel.GetSourceCount();

            GraphYype = new List<string>() { "Line", "Source" };
            
           // SelectGTypeIndex = 0;

            MoistureType = ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Name;

            try
            {
                setMenuOptions();
                setgraphtype();

                MainWindow.AppWindows.SetupAppTitle("Forté RealTime Graph From  -> " + _sqlhandler.Host + @"\" + _sqlhandler.SqlInstance);

                switch (Settings.Default.iRTGMenuSelected)
                {
                    case 0:
                        MenuOneChecked = true;

                        break;
                    case 1:
                        MenuTwoChecked = true;
                        break;
                    case 2:
                        MenuThreeChecked = true;
                        break;
                    case 3:
                        MenuFourChecked = true;
                        break;
                    case 4:
                        MenuFiveChecked = true;
                        break;
                    case 5:
                        MenuSixChecked = true;
                        break;
                    case 6:
                        MenuSevenChecked = true;
                        break;
                    case 7:
                        MenuEightChecked = true;
                        break;
                    default:
                        MenuOneChecked = true;
                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in LoadedPageExecute " + ex.Message);
            }
            Opac = 1.0;

            switch (Settings.Default.iLanguageIdx)
            {
                case 0: // "en-US":
                    MainWindow.AppWindows.SetupAppTitle("Forté RealTime Graph From  -> " + _sqlhandler.Host + @"\" + _sqlhandler.SqlInstance);
                    break;
                case 1: //"Sp-SP":
                    MainWindow.AppWindows.SetupAppTitle("Forté Gráfico TiempoReal desde  -> " + _sqlhandler.Host + @"\" + _sqlhandler.SqlInstance);
                    break;
                default:
                    MainWindow.AppWindows.SetupAppTitle("Forté RealTime Graph From  -> " + _sqlhandler.Host + @"\" + _sqlhandler.SqlInstance);
                    break;
            }
        }

        private DelegateCommand _closedPageICommand;
        public DelegateCommand ClosedPageICommand =>
        _closedPageICommand ?? (_closedPageICommand = new DelegateCommand(ClosedPageExecute));
        private void ClosedPageExecute()
        {
            if (CGraphModel != null) CGraphModel = null;
            if (dispatcherTimer != null) dispatcherTimer = null;

            if (HeartBeatTimer != null)
            {
                HeartBeatTimer.Stop();
                HeartBeatTimer = null;
            }
        }

        private DelegateCommand _startCommand;
        public DelegateCommand StartCommand =>
        _startCommand ?? (_startCommand = new DelegateCommand(StartExecute).ObservesCanExecute(() => RTIdle));
        private void StartExecute()
        {
            switch (Settings.Default.iLanguageIdx)
            {
                case 0: // "en-US":
                    XAxisTitle = "Most Recent - " + BSamples + " - Bales (from Location 1 on the Left)";
                    break;
                case 1: //"Sp-SP":
                    XAxisTitle = "Más reciente - " + BSamples + " - Fardo (ubicación 1 a la izquierda)";
                    break;
                default:
                    XAxisTitle = "Most Recent - " + BSamples + " - Bales (from Location 1 on the Left)";
                    break;
            }

            RTRunning = true;
            _eventAggregator.GetEvent<UpdatedEvent>().Publish(RTRunning);

            Settings.Default.ibaleSamples = BSamples;
            Settings.Default.Save();

            ShowMe = 0.1;
            Opac = 0.4;

            InitializeTimer();
            StartTimer();

            InitializeHeartBeatTimer();
        }

        private DelegateCommand _stopCommand;
        public DelegateCommand StopCommand =>
        _stopCommand ?? (_stopCommand = new DelegateCommand(StopExecute).ObservesCanExecute(() => RTRunning));
        private void StopExecute()
        {
            Application.Current.Dispatcher.Invoke(new Action(() => { StopTimer(); }));
            switch (Settings.Default.iLanguageIdx)
            {
                case 0: // "en-US":
                    UpdateInfo = "Status : Scan timer Stop";
                    break;
                case 1: //"Sp-SP":
                    UpdateInfo = "Estado : Tiempo de Scan Detenido";
                    break;
                default:
                    UpdateInfo = "Status : Scan timer Stop";
                    break;
            }
        }


        //-----------------------------------------------------------/
        private List<string> _graphYype;
        public List<string> GraphYype
        {
            get { return _graphYype; }
            set { SetProperty(ref _graphYype, value); }
        }

        private int _selectGTypeIndex = Settings.Default.graphByLstidx;
        public int SelectGTypeIndex
        {
            get { return _selectGTypeIndex; }
            set 
            { 
                SetProperty(ref _selectGTypeIndex, value);

                Settings.Default.graphByLstidx = value;
                Settings.Default.Save();
                setgraphtype();
            }
        }

    
        private bool _byline;
        public bool Byline
        {
            get { return _byline; }
            set
            {
                SetProperty(ref _byline, value);
            }
        }
        private bool _bySource;
        public bool BySource
        {
            get { return _bySource; }
            set
            {
                SetProperty(ref _bySource, value);
            }
        }

        private bool _selectedSource;
        public bool SelectedSource
        {
            get { return _selectedSource; }
            set { SetProperty(ref _selectedSource, value); }
        }

        private bool _selectedline;
        public bool Selectedline
        {
            get { return _selectedline; }
            set { SetProperty(ref _selectedline, value); }
        }

       
        private string _LSSelectiontxt;
        public string LSSelectiontxt
        {
            get { return _LSSelectiontxt; }
            set { SetProperty(ref _LSSelectiontxt, value); }
        }

        private bool _sourceSelected;
        public bool SourceSelected
        {
            get { return _sourceSelected; }
            set { SetProperty(ref _sourceSelected, value); }
        }

        private bool _lineSelected;
        public bool LineSelected
        {
            get { return _lineSelected; }
            set { SetProperty(ref _lineSelected, value); }
        }

       
        private int iMenuSelect;

        private string _MoistureType;
        public string MoistureType
        {
            get { return _MoistureType; }
            set { SetProperty(ref _MoistureType, value); }
        }

        private System.Windows.Media.Brush _positionColor;
        public System.Windows.Media.Brush PositionColor
        {
            get => _positionColor;
            set => SetProperty(ref _positionColor, value);
        }

        private string _txtInfo;
        public string TxtInfo
        {
            get => _txtInfo;
            set => SetProperty(ref _txtInfo, value);
        }


        public int MenuSelected { get; set; }

        //---------------------------------------------------------------------------------------------------------
        private bool _menuOneChecked; //Moisture
        public bool MenuOneChecked
        {
            get => _menuOneChecked; 
            set
            {
                SetProperty(ref _menuOneChecked, value);
                SetMenuOptionOne(value);
            }
        }

        private void SetMenuOptionOne(bool bChecked)
        {
            try
            {
                if (bChecked)
                {
                    MenuSelected = 0;
                    switch (Settings.Default.iLanguageIdx)
                    {
                        case 0: // "en-US":
                            GraphTitle = "Graph of " + ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Name;
                            break;
                        case 1: //"Sp-SP":
                            GraphTitle = "Gráfico de " + ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Name;
                            break;
                        default:
                            GraphTitle = "Graph of " + ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Name;
                            break;
                    }
                    YAxixTitle = ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Name;

                    iMenuSelect = 0;
                    MenuSelect = MenuOptions[0];
                    Settings.Default.iRTGMenuSelected = 0;
                    Settings.Default.Save();

                    if(YAxisMax == 0) //Only the first time
                        YAxisMax = Settings.Default.dYxHiM; // 11.4; // YAxisLimitHigh + (YAxisLimitHigh * .05);
                    if (YAxisMin == 0) //Only the first time
                        YAxisMin = Settings.Default.dYxLowM;  //9.6; // YAxisLimitLow - (YAxisLimitLow * .05);
                   
                    HighLimit = Settings.Default.GraphHiMenuOne;
                    LowLimit = Settings.Default.GraphLoMenuOne;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in SetMenuOptionOne {ex.Message}");
            }
        }


        private async Task GetUpdateDataAsync(int iSample)
        {
            await Task.Run(async () =>
            {
                RealTimeDataTable = await CGraphModel.GetNewGraphDataAsync(BSamples);
                if (RealTimeDataTable.Rows.Count > 0)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() => { ProcessDataTable(RealTimeDataTable); }));
                    RealTimePopupTable = RealTimeDataTable;
                }
            });
        }


        private bool _menuMRChecked;
        public bool MenuMRChecked
        {
            get { return _menuMRChecked; }
            set
            {
                SetProperty(ref _menuMRChecked, value);
                if (value)
                {
                    switch (Settings.Default.iLanguageIdx) //Thread.CurrentThread.CurrentCulture.ToString())
                    {
                        case 0: // "en-US":
                            GraphTitle = "Graph of " + ClsParams.MoistureTypeList[1].Name; //MR %
                            break;
                        case 1: //"Sp-SP":
                            GraphTitle = "Gráfico de en peso " + ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name;
                            break;
                        default:
                            GraphTitle = "Graph of " + ClsParams.MoistureTypeList[1].Name; //MR %
                            break;
                    }
                    YAxixTitle = ClsParams.MoistureTypeList[1].Name;
                }
            }
        }
        private bool _menuADChecked;
        public bool MenuADChecked
        {
            get { return _menuADChecked; }
            set
            {
                SetProperty(ref _menuADChecked, value);
                if (value)
                {
                    switch (Settings.Default.iLanguageIdx) //Thread.CurrentThread.CurrentCulture.ToString())
                    {
                        case 0: // "en-US":
                            GraphTitle = "Graph of " + ClsParams.MoistureTypeList[2].Name; //AD %
                            break;
                        case 1: //"Sp-SP":
                            GraphTitle = "Gráfico de Peso AD";
                            break;
                        default:
                            GraphTitle = "Graph of " + ClsParams.MoistureTypeList[2].Name; //AD %
                            break;
                    }
                    YAxixTitle = ClsParams.MoistureTypeList[2].Name;
                }
            }
        }
        private bool _menuBDChecked;
        public bool MenuBDChecked
        {
            get { return _menuBDChecked; }
            set
            {
                SetProperty(ref _menuBDChecked, value);
                if (value)
                {
                    switch (Settings.Default.iLanguageIdx) //Thread.CurrentThread.CurrentCulture.ToString())
                    {
                        case 0: // "en-US":
                            GraphTitle = "Graph of " + ClsParams.MoistureTypeList[3].Name; //BD %
                            break;
                        case 1: //"Sp-SP":
                            GraphTitle = "Gráfico de Conteo de pintas";
                            break;
                        default:
                            GraphTitle = "Graph of " + ClsParams.MoistureTypeList[3].Name; //BD %
                            break;
                    }
                    YAxixTitle = ClsParams.MoistureTypeList[3].Name;
                }
            }
        }
        //--------------------------------------------------------------------------------------------------------

        private bool _menuTwoChecked; //Weight
        public bool MenuTwoChecked
        {
            get { return _menuTwoChecked; }
            set
            {
                SetProperty(ref _menuTwoChecked, value);
                if (value)
                {
                    MenuSelected = 1;
                    switch (Settings.Default.iLanguageIdx) //Thread.CurrentThread.CurrentCulture.ToString())
                    {
                        case 0: // "en-US":
                            GraphTitle = "Graph of Weight in " + ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name;
                            break;
                        case 1: //"Sp-SP":
                            GraphTitle = "Gráfico de en peso " + ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name;
                            break;
                        default:
                            GraphTitle = "Graph of Weight in " + ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name;
                            break;
                    }
                  
                    YAxixTitle = ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name;

                    iMenuSelect = 1;
                    MenuSelect = MenuOptions[1];
                    Settings.Default.iRTGMenuSelected = 1;
                    Settings.Default.Save();

                    if(YAxisMax == 0)
                        YAxisMax = Settings.Default.dYxHiW;
                    if(YAxisMin == 0)
                        YAxisMin = Settings.Default.dYxLowW;
                  
                    HighLimit = Settings.Default.GraphHiMenuTwo;
                    LowLimit = Settings.Default.GraphLoMenuTwo;
                }
            }
        }

        private bool _menuThreeChecked;
        public bool MenuThreeChecked
        {
            get { return _menuThreeChecked; }
            set
            {
                SetProperty(ref _menuThreeChecked, value);
                if (value)
                {
                    MenuSelected = 2;

                    switch (Settings.Default.iLanguageIdx)
                    {
                        case 0: // "en-US":
                            GraphTitle = "Graph of BoneDry Weight ";
                            break;
                        case 1: //"Sp-SP":
                            GraphTitle = "Gráfico de Pesco BD ";
                            break;
                        default:
                            GraphTitle = "Graph of BoneDry Weight ";
                            break;
                    }

                    YAxixTitle = String.Empty;

                    iMenuSelect = 2;
                    MenuSelect = MenuOptions[2];
                    Settings.Default.iRTGMenuSelected = 2;
                    Settings.Default.Save();

                    if(YAxisMax == 0)
                        YAxisMax = Settings.Default.dYxHiBdWt;
                    if(YAxisMin == 0)
                    YAxisMin = Settings.Default.dYxLowBdWt;

                    HighLimit = Settings.Default.GraphHiMenuThree;
                    LowLimit = Settings.Default.GraphLoMenuThree;

                    // _ = GetUpdateDataAsync(BSamples);
                }
                
            }
        }

        private bool _menu4Checked;
        public bool MenuFourChecked
        {
            get { return _menu4Checked; }
            set
            {
                MenuSelected = 3;
                SetProperty(ref _menu4Checked, value);
                if (value)
                {
                    switch (Settings.Default.iLanguageIdx)
                    {
                        case 0: // "en-US":
                            GraphTitle = "Graph of AirDry Weight";
                            break;
                        case 1: //"Sp-SP":
                            GraphTitle = "Gráfico de Peso AD";
                            break;
                        default:
                            GraphTitle = "Graph of AirDry Weight";
                            break;
                    }
                   
                    YAxixTitle = String.Empty;

                    iMenuSelect = 3;
                    MenuSelect = MenuOptions[3];
                    Settings.Default.iRTGMenuSelected = 3;
                    Settings.Default.Save();

                    if(YAxisMax == 0)
                        YAxisMax = Settings.Default.dYxHiADWt;
                    if(YAxisMin == 0)
                        YAxisMin = Settings.Default.dYxLowADWt;

                    HighLimit = Settings.Default.GraphHiMenuFour;
                    LowLimit = Settings.Default.GraphLoMenuFour;

                    // _ = GetUpdateDataAsync(BSamples);
                }
            }
        }

        private bool _menu5Checked;
        public bool MenuFiveChecked
        {
            get { return _menu5Checked; }
            set
            {
                MenuSelected = 4;

                SetProperty(ref _menu5Checked, value);
                if (value)
                {
                    switch (Settings.Default.iLanguageIdx)
                    {
                        case 0: // "en-US":
                            GraphTitle = "Graph of Dirt";
                            break;
                        case 1: //"Sp-SP":
                            GraphTitle = "Gráfico de Conteo de pintas";
                            break;
                        default:
                            GraphTitle = "Graph of Dirt";
                            break;
                    }
                    
                    YAxixTitle = String.Empty;

                    iMenuSelect = 4;
                    MenuSelect = MenuOptions[4];
                    Settings.Default.iRTGMenuSelected = 4;
                    Settings.Default.Save();

                    if(YAxisMax == 0)
                        YAxisMax = Settings.Default.dYxHiDirt;
                    if(YAxisMin == 0)
                        YAxisMin = Settings.Default.dYxLowDirt;

                    HighLimit = Settings.Default.GraphHiMenuFive;
                    LowLimit = Settings.Default.GraphLoMenuFive;

                    // _ = GetUpdateDataAsync(BSamples);
                }
            }
        }

        private bool _menu6Checked;
        public bool MenuSixChecked
        {
            get { return _menu6Checked; }
            set
            {
                MenuSelected = 5;

                SetProperty(ref _menu6Checked, value);
                if (value)
                {
                    switch (Settings.Default.iLanguageIdx)
                    {
                        case 0: // "en-US":
                            GraphTitle = "Graph of Brightness";
                            break;
                        case 1: //"Sp-SP":
                            GraphTitle = "Gráfico de Blancura";
                            break;
                        default:
                            GraphTitle = "Graph of Brightness";
                            break;
                    }
                    
                    YAxixTitle = String.Empty;

                    iMenuSelect = 5;
                    MenuSelect = MenuOptions[5];
                    Settings.Default.iRTGMenuSelected = 5;
                    Settings.Default.Save();

                    if(YAxisMax == 0)
                        YAxisMax = Settings.Default.dYxHiBright;
                    if(YAxisMin == 0)
                        YAxisMin = Settings.Default.dYxLowBright;

                    HighLimit = Settings.Default.GraphHiMenuSix;
                    LowLimit = Settings.Default.GraphLoMenuSix;

                    // _ = GetUpdateDataAsync(BSamples);
                }
            }
        }



        private bool _menu7Checked;
        public bool MenuSevenChecked
        {
            get { return _menu7Checked; }
            set
            {
                MenuSelected = 6;

                SetProperty(ref _menu7Checked, value);
                if (value)
                {
                    switch (Settings.Default.iLanguageIdx)
                    {
                        case 0: // "en-US":
                            GraphTitle = "Graph of Viscosity ";
                            break;
                        case 1: //"Sp-SP":
                            GraphTitle = "Gráfico de Viscosidad ";
                            break;
                        default:
                            GraphTitle = "Graph of Viscosity ";
                            break;
                    }
                    
                    YAxixTitle = String.Empty;

                    iMenuSelect = 6;
                    MenuSelect = MenuOptions[6];
                    Settings.Default.iRTGMenuSelected = 6;
                    Settings.Default.Save();

                    if(YAxisMax == 0)
                        YAxisMax = Settings.Default.dYxHiVisco;
                    if(YAxisMin == 0)
                        YAxisMin = Settings.Default.dYxLowVisco;

                    HighLimit = Settings.Default.GraphHiMenuSeven;
                    LowLimit = Settings.Default.GraphLoMenuSeven;

                    // _ = GetUpdateDataAsync(BSamples);
                }
            }
        }

        private bool _menu8Checked;
        public bool MenuEightChecked
        {
            get { return _menu8Checked; }
            set
            {
                MenuSelected = 7;
                SetProperty(ref _menu8Checked, value);
                if (value)
                {
                    switch (Settings.Default.iLanguageIdx)
                    {
                        case 0: // "en-US":
                            GraphTitle = "Graph of Forte Number ";
                            break;
                        case 1: //"Sp-SP":
                            GraphTitle = "Gráfico de Forte Number ";
                            break;
                        default:
                            GraphTitle = "Graph of Forte Number ";
                            break;
                    }
                    YAxixTitle = String.Empty;

                    iMenuSelect = 7;
                    MenuSelect = MenuOptions[7];
                    Settings.Default.iRTGMenuSelected = 7;
                    Settings.Default.Save();

                    LowLimit = Settings.Default.GraphLoMenuEight;
                    HighLimit = Settings.Default.GraphHiMenuEight;

                   // _ = GetUpdateDataAsync(BSamples);
                }
            }
        }

     
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


        private DataTable _realtimedatatable;
        public DataTable RealTimeDataTable
        {
            get { return _realtimedatatable; }
            set { SetProperty(ref _realtimedatatable, value); }
        }

        private DataTable _realtimepopuptable;
        public DataTable RealTimePopupTable
        {
            get { return _realtimepopuptable; }
            set { SetProperty(ref _realtimepopuptable, value); }
        }


        private int _linex;
        public int Line
        {
            get { return _linex; }
            set { SetProperty(ref _linex, value); }
        }

        private int _Sourcex;
        public int Source
        {
            get { return _Sourcex; }
            set { SetProperty(ref _Sourcex, value); }
        }

        private int _bsample = Settings.Default.ibaleSamples;
        public int BSamples
        {
            get { return _bsample; }
            set
            {
                if ((value > 0) & (value < 1001))
                    SetProperty(ref _bsample, value);
                else
                    SetProperty(ref _bsample, 100);

                Settings.Default.ibaleSamples = value;
                Settings.Default.Save();

                XAxisTitle = "Most Recent - " + value + " - Bales (from Location 1 on the Left)";
            }
        }

        private readonly List<double> dAllVal = new List<double>();

        private Single _lowLimit;
        public Single LowLimit
        {
            get { return _lowLimit; }
            set
            {
                if ((value > 0) & (value < 9000))
                    SetProperty(ref _lowLimit, value);
                else
                    SetProperty(ref _lowLimit, 1);

                switch (iMenuSelect)
                {
                    case 0:
                        Settings.Default.GraphLoMenuOne = value;
                        break;
                    case 1:
                        Settings.Default.GraphLoMenuTwo = value;
                        break;
                    case 2:
                        Settings.Default.GraphLoMenuThree = value;
                        break;
                    case 3:
                        Settings.Default.GraphLoMenuFour = value;
                        break;
                    case 4:
                        Settings.Default.GraphLoMenuFive = value;
                        break;
                    case 5:
                        Settings.Default.GraphLoMenuSix = value;
                        break;
                    case 6:
                        Settings.Default.GraphLoMenuSeven = value;
                        break;
                    case 7:
                        Settings.Default.GraphLoMenuEight = value;
                        break;
                    default:
                        Settings.Default.GraphLoMenuOne = value;
                        break;
                }
                Settings.Default.Save();
            }
        }

        private Single _highLimit;
        public Single HighLimit
        {
            get { return _highLimit; }
            set
            {
                if ((value > 0) & (value < 9000))
                    SetProperty(ref _highLimit, value);
                else
                    SetProperty(ref _highLimit, 10);


                switch (iMenuSelect)
                {
                    case 0:
                        Settings.Default.GraphHiMenuOne = value;
                        break;
                    case 1:
                        Settings.Default.GraphHiMenuTwo = value;
                        break;
                    case 2:
                        Settings.Default.GraphHiMenuThree = value;
                        break;
                    case 3:
                        Settings.Default.GraphHiMenuFour = value;
                        break;
                    case 4:
                        Settings.Default.GraphHiMenuFive = value;
                        break;
                    case 5:
                        Settings.Default.GraphHiMenuSix = value;
                        break;
                    case 6:
                        Settings.Default.GraphHiMenuSeven = value;
                        break;
                    case 7:
                        Settings.Default.GraphHiMenuEight = value;
                        break;
                    default:
                        Settings.Default.GraphHiMenuOne = value;
                        break;
                }
                Settings.Default.Save();
            }
        }


        private string _nmenuSelect;
        public string MenuSelect
        {
            get { return _nmenuSelect; }
            set { SetProperty(ref _nmenuSelect, value); }
        }

        private string _graphTitle;
        public string GraphTitle
        {
            get { return _graphTitle; }
            set { SetProperty(ref _graphTitle, value); }
        }

        private string _updateinfo;
        public string UpdateInfo
        {
            get { return _updateinfo; }
            set { SetProperty(ref _updateinfo, value); }
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

        #region YAxis

        private string _YAxixTitle;
        public string YAxixTitle
        {
            get { return _YAxixTitle; }
            set { SetProperty(ref _YAxixTitle, value); }
        }


        private double _yAxisLimitLow;
        public double YAxisLimitLow
        {
            get { return _yAxisLimitLow; }
            set { SetProperty(ref _yAxisLimitLow, value); }
        }
        private double _yAxisLimitHigh;
        public double YAxisLimitHigh
        {
            get { return _yAxisLimitHigh; }
            set { SetProperty(ref _yAxisLimitHigh, value); }
        }
        //Graph Y-Axis bottom bounderly
        private double _yAxisMin;
        public double YAxisMin
        {
            get { return _yAxisMin; }
            set { SetProperty(ref _yAxisMin, value); }
        }
        //Graph  Y-Axis top bounderly
        private double _yAxisMax;
        public double YAxisMax
        {
            get { return _yAxisMax; }
            set { SetProperty(ref _yAxisMax, value); }
        }

        #endregion YAxis

        private string _xAxisTitle = "Most Recent - " + Settings.Default.ibaleSamples + " - Bales (from Location 1 on the Left)";
        public string XAxisTitle
        {
            get { return _xAxisTitle; }
            set { SetProperty(ref _xAxisTitle, value); }
        }

        private int _DropSamples;
        public int DropSamples
        {
            get { return _DropSamples; }
            set
            {
                // XAxisTitle = "Most Recent - " + value + " - Bales (from Location 1 on the Left)";
                SetProperty(ref _DropSamples, value);
            }
        }

        public RealTimeGraphViewModel(Prism.Events.IEventAggregator eventAggregator) 
        {
            this._eventAggregator = eventAggregator;

            BackgndchartLst = new List<Brush>();
            BackgndchartLst.Add(Brushes.Black);
            BackgndchartLst.Add(Brushes.White);

            GraphByLst = new List<string>();
            GraphByLst.Add("Source");
            GraphByLst.Add("Line");

            BackGroundChartColor = BackgndchartLst[Settings.Default.backgndchartidx];

            // we assume this ctor is called from the UI thread!
            SynchronizationContext _syncContext = SynchronizationContext.Current;
          
            _eventAggregator.GetEvent<UpdatedEventShutdown>().Subscribe(ProgramShutdown);
        }



        private DelegateCommand _graphSetCommand;
        public DelegateCommand GraphSetCommand =>
        _graphSetCommand ?? (_graphSetCommand = new DelegateCommand(GraphSetCommandExecute));
        private void GraphSetCommandExecute()
        {

            using(RtGrpSettingView RtGrpSet =  new RtGrpSettingView(_eventAggregator))
            {
                GrapgSetupWindow = new Window()
                {
                    Title = "Graph Setup Window",
                    Width = 800,
                    Height = 430,
                    Topmost = true,
                    Content = RtGrpSet
                };
                // GrapgSetupWindow.WindowStyle = WindowStyle.None;
                GrapgSetupWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                GrapgSetupWindow.ResizeMode = ResizeMode.NoResize;
                GrapgSetupWindow.ShowDialog();
            }
        }

      

        private void ProgramShutdown(bool obj)
        {
            if (obj)
            {
                if (dispatcherTimer != null)
                {
                    dispatcherTimer.Stop();
                    dispatcherTimer = null;
                }
                if (HeartBeatTimer != null)
                {
                    HeartBeatTimer.Stop();
                    HeartBeatTimer = null;
                }
            }
        }

        public string SourceType { get; set; }
        public int linesourcecnt { get; set; }

        private void setgraphtype()
        {

            string gtype = GraphByLst[Settings.Default.graphByLstidx];

            if (gtype == "Source")
            {
                BySource = true;
                SelectedSource = true;
                SourceType = "SourceID";
                linesourcecnt = Source;

                LSSelectiontxt = "Sources Selections;";
                CGraphModel.GetLineSource();
                this.Source = CGraphModel.m_Source;
                SetActiveLines(Source);
            }
            else if (gtype == "Line")
            {

                Byline = true;
                Selectedline = true;
                SourceType = "LineID";
                linesourcecnt = Line;

                LSSelectiontxt = "Lines Selections;";
                CGraphModel.GetLineSource();
                this.Line = CGraphModel.m_Line;
                SetActiveLines(Line);
            }

        }

        private void SetActiveLines(int icount)
        {
            ResetLines();
            switch (icount)
            {
                case 6:
                    IsL6Active = true;
                    LineSixChecked = true;
                    goto case 5;
                case 5:
                    IsL5Active = true;
                    LineFiveChecked = true;
                    goto case 4;
                case 4:
                    IsL4Active = true;
                    LineFourChecked = true;
                    goto case 3;
                case 3:
                    IsL3Active = true;
                    LineThreeChecked = true;
                    goto case 2;
                case 2:
                    IsL2Active = true;
                    LineTwoChecked = true;
                    goto case 1;
                case 1:
                    IsL1Active = true;
                    LineOneChecked = true;
                    break;

                default:
                    ResetLines();
                    break;
            }
        }

        private void ResetLines()
        {
            IsL1Active = false;
            IsL2Active = false;
            IsL3Active = false;
            IsL4Active = false;
            IsL5Active = false;
            IsL6Active = false;

            LineOneChecked = false;
            LineTwoChecked = false;
            LineThreeChecked = false;
            LineFourChecked = false;
            LineFiveChecked = false;
            LineSixChecked = false;
        }

        #region Source/line selects

        private bool _lineonechecked;
        public bool LineOneChecked
        {
            get { return _lineonechecked; }
            set
            {
                SetProperty(ref _lineonechecked, value);
                if (value)
                {
                  //  _ = GetUpdateDataAsync(BSamples);
                }

            }
        }

        private bool _linetwochecked;
        public bool LineTwoChecked
        {
            get { return _linetwochecked; }
            set
            {
                SetProperty(ref _linetwochecked, value);
                if (value)
                {
                  //  _ = GetUpdateDataAsync(BSamples);
                }
            }
        }

        private bool _linethreechecked;
        public bool LineThreeChecked
        {
            get { return _linethreechecked; }
            set
            {
                SetProperty(ref _linethreechecked, value);
                if (value)
                {
                  //  _ = GetUpdateDataAsync(BSamples);
                }
            }
        }

        private bool _linefourchecked;
        public bool LineFourChecked
        {
            get { return _linefourchecked; }
            set
            {
                SetProperty(ref _linefourchecked, value);
                if (value)
                {
                  //  _ = GetUpdateDataAsync(BSamples);

                }
            }
        }

        private bool _linefivechecked;
        public bool LineFiveChecked
        {
            get { return _linefivechecked; }
            set
            {
                SetProperty(ref _linefivechecked, value);
                if (value)
                {
                   // _ = GetUpdateDataAsync(BSamples);
                }
            }
        }

        private bool _linesixchecked;
        public bool LineSixChecked
        {
            get { return _linesixchecked; }
            set
            {
                SetProperty(ref _linesixchecked, value);
                if (value)
                {
                  //  _ = GetUpdateDataAsync(BSamples);
                }
            }
        }

        private bool _IsL1Active;
        public bool IsL1Active
        {
            get { return _IsL1Active; }
            set { SetProperty(ref _IsL1Active, value); }
        }

        private bool _IsL2Active;
        public bool IsL2Active
        {
            get { return _IsL2Active; }
            set { SetProperty(ref _IsL2Active, value); }
        }

        private bool _IsL3Active;
        public bool IsL3Active
        {
            get { return _IsL3Active; }
            set { SetProperty(ref _IsL3Active, value); }
        }

        private bool _IsL4Active;
        public bool IsL4Active
        {
            get { return _IsL4Active; }
            set { SetProperty(ref _IsL4Active, value); }
        }

        private bool _IsL5Active;
        public bool IsL5Active
        {
            get { return _IsL5Active; }
            set { SetProperty(ref _IsL5Active, value); }
        }

        private bool _IsL6Active;
        public bool IsL6Active
        {
            get { return _IsL6Active; }
            set { SetProperty(ref _IsL6Active, value); }
        }


        #endregion Source/line selects

      

     


        /// <summary>
        /// Graph type Moisture, Weight or Forte
        /// Source or Line
        /// Correct all Moisture type %MC. %AD
        /// correct weight unit. Lb or Kg.
        /// sometime item is not single use Convert.ToSingle()
        /// </summary>
        /// <param name="Mydata"></param>
        private void ProcessDataTable(DataTable Mydata)
        {

            int iCount;
            Single Dvalue = 1;
            // string strSourceType;

            if (Pos1 != null) Pos1 = null;
            if (Pos2 != null) Pos2 = null;
            if (Pos3 != null) Pos3 = null;
            if (Pos4 != null) Pos4 = null;
            if (Pos5 != null) Pos5 = null;
            if (Pos6 != null) Pos6 = null;


            if (GraphByLst[Settings.Default.graphByLstidx] == "All")
            {
                ShowBoxOne = Visibility.Visible;
                Pos1 = new ObservableCollection<KeyValuePair<Single, int>>();
            }
            else
            {
                if (BySource)
                {
                    iCount = Source;
                    //strSourceType = "SourceID";
                }
                else
                {
                    iCount = Line;
                    // strSourceType = "LineID";
                }

                switch (iCount)
                {
                    case 6:
                        if (LineSixChecked)
                        {
                            ShowBoxSix = Visibility.Visible;
                            Pos6 = new ObservableCollection<KeyValuePair<Single, int>>();
                        }
                        goto case 5;
                    case 5:
                        if (LineFiveChecked)
                        {
                            ShowBoxFive = Visibility.Visible;
                            Pos5 = new ObservableCollection<KeyValuePair<Single, int>>();
                        }
                           
                        goto case 4;
                    case 4:
                        if (LineFourChecked)
                        {
                            ShowBoxFour = Visibility.Visible;
                            Pos4 = new ObservableCollection<KeyValuePair<Single, int>>();
                        }
                           
                        goto case 3;
                    case 3:
                        if (LineThreeChecked)
                        {
                            ShowBoxThree = Visibility.Visible;
                            Pos3 = new ObservableCollection<KeyValuePair<Single, int>>();
                        }
                            
                        goto case 2;
                    case 2:
                        if (LineTwoChecked)
                        {
                            ShowBoxTwo = Visibility.Visible;
                            Pos2 = new ObservableCollection<KeyValuePair<Single, int>>();
                        }
                           
                        goto case 1;
                    case 1:
                        if (LineOneChecked) 
                        {
                            ShowBoxOne = Visibility.Visible;
                            Pos1 = new ObservableCollection<KeyValuePair<Single, int>>();
                        }           
                        break;
                }
            }

            if (ItemsMax != null) ItemsMax = null;
            ItemsMax = new ObservableCollection<KeyValuePair<Single, int>>();

            if (ItemsMin != null) ItemsMin = null;
            ItemsMin = new ObservableCollection<KeyValuePair<Single, int>>();

            //  List<double> dAllVal = new List<double>();

            dAllVal.Clear();

            try
            {
                int i = 0;
                foreach (DataRow Item in Mydata.Rows)
                {
                    //   Console.WriteLine("Item[strSourceType].ToString() = " + Item[strSourceType].ToString());

                    if ((Pos1 != null) & (Item[SourceType].ToString() == "1"))
                    {
                        if (!string.IsNullOrEmpty(Item[MenuSelect].ToString()))
                        {
                            Dvalue = Convert.ToSingle(Item[MenuSelect]);
                            Pos1.Add(new KeyValuePair<Single, int>(Dvalue, i));
                            dAllVal.Add(Dvalue);
                        }
                    }
                    if ((Pos2 != null) & (Item[SourceType].ToString() == "2"))
                    {
                        if (!string.IsNullOrEmpty(Item[MenuSelect].ToString()))
                        {
                            Dvalue = Convert.ToSingle(Item[MenuSelect]);
                            Pos2.Add(new KeyValuePair<Single, int>(Dvalue, i));
                            dAllVal.Add(Dvalue);
                        }
                    }

                    if ((Pos3 != null) & (Item[SourceType].ToString() == "3"))
                    {
                        if (!string.IsNullOrEmpty(Item[MenuSelect].ToString()))
                        {
                            Dvalue = Convert.ToSingle(Item[MenuSelect]);
                            Pos3.Add(new KeyValuePair<Single, int>(Dvalue, i));
                            dAllVal.Add(Dvalue);
                        }
                    }

                    if ((Pos4 != null) & (Item[SourceType].ToString() == "4"))
                    {
                        if (!string.IsNullOrEmpty(Item[MenuSelect].ToString()))
                        {
                            Dvalue = Convert.ToSingle(Item[MenuSelect]);
                            Pos4.Add(new KeyValuePair<Single, int>(Dvalue, i));
                            dAllVal.Add(Dvalue);
                        }
                    }

                    if ((Pos5 != null) & (Item[SourceType].ToString() == "5"))
                    {
                        if (!string.IsNullOrEmpty(Item[MenuSelect].ToString()))
                        {
                            Dvalue = Convert.ToSingle(Item[MenuSelect]);
                            Pos5.Add(new KeyValuePair<Single, int>(Dvalue, i));
                            dAllVal.Add(Dvalue);
                        }
                    }

                    if ((Pos6 != null) & (Item[SourceType].ToString() == "6"))
                    {
                        if (!string.IsNullOrEmpty(Item[MenuSelect].ToString()))
                        {
                            Dvalue = Convert.ToSingle(Item[MenuSelect]);
                            Pos6.Add(new KeyValuePair<Single, int>(Dvalue, i));
                            dAllVal.Add(Dvalue);
                        }
                    }
                    

                    ItemsMax.Add(new KeyValuePair<Single, int>(HighLimit, i));
                    ItemsMin.Add(new KeyValuePair<Single, int>(LowLimit, i));

                    i += 1;
                }
               
                if (dAllVal.Count > 0)
                {
                    YAxisLimitHigh = dAllVal.Max();
                    YAxisLimitLow = dAllVal.Min();

                    YAxisMax = YAxisMin + 100; //Always > min
                    YAxisMin = 0;

                    if(Settings.Default.BScaleAuto)
                    {
                        GraphAutoScale();
                    }
                    else
                    {
                        switch (MenuSelected)
                        {
                            case 0: //Moisture
                                YAxisMax = Settings.Default.dYxHiM; // 11.4; // YAxisLimitHigh + (YAxisLimitHigh * .05);
                                YAxisMin = Settings.Default.dYxLowM;  //9.6; // YAxisLimitLow - (YAxisLimitLow * .05);
                                HighLimit = Settings.Default.GraphHiMenuOne;
                                LowLimit = Settings.Default.GraphLoMenuOne;
                                break;

                            case 1: //Weight
                                YAxisMax = Settings.Default.dYxHiW;
                                YAxisMin = Settings.Default.dYxLowW;
                                HighLimit = Settings.Default.GraphHiMenuTwo;
                                LowLimit = Settings.Default.GraphLoMenuTwo;
                                break;

                            case 2: //Bone Dry Weight
                                YAxisMax = Settings.Default.dYxHiBdWt;
                                YAxisMin = Settings.Default.dYxLowBdWt;
                                HighLimit = Settings.Default.GraphHiMenuThree;
                                LowLimit = Settings.Default.GraphLoMenuThree;
                                break;

                            case 3: //Air Dry Weight
                                YAxisMax = Settings.Default.dYxHiADWt;
                                YAxisMin = Settings.Default.dYxLowADWt;
                                HighLimit = Settings.Default.GraphHiMenuFour;
                                LowLimit = Settings.Default.GraphLoMenuFour;
                                break;

                            case 4: // Dirt
                                YAxisMax = Settings.Default.dYxHiDirt;
                                YAxisMin = Settings.Default.dYxLowDirt;
                                HighLimit = Settings.Default.GraphHiMenuFive;
                                LowLimit = Settings.Default.GraphLoMenuFive;
                                break;

                            case 5: // Bright
                                YAxisMax = Settings.Default.dYxHiBright;
                                YAxisMin = Settings.Default.dYxLowBright;
                                HighLimit = Settings.Default.GraphHiMenuSix;
                                LowLimit = Settings.Default.GraphLoMenuSix;
                                break;

                            case 6:
                                YAxisMax = Settings.Default.dYxHiVisco;
                                YAxisMin = Settings.Default.dYxLowVisco;
                                HighLimit = Settings.Default.GraphHiMenuSeven;
                                LowLimit = Settings.Default.GraphLoMenuSeven;
                                break;

                            case 7:
                                GraphAutoScale();
                                break;

                            default:
                                GraphAutoScale();
                                break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR in ProcessDataTable " + ex.Message);
                MessageBox.Show("ERROR in ProcessDataTable " + ex.Message);
            }
        }


        private void GraphAutoScale()
        {
          //  YAxisLimitHigh = dAllVal.Max();
          //  YAxisLimitLow = dAllVal.Min();

//            YAxisMax = YAxisMin + 100; //Always > min
            //YAxisMin = 0;
            
            if ((YAxisLimitHigh > -1) && (YAxisLimitHigh < 35)) // %MC
            {
                YAxisMax = YAxisLimitHigh + (YAxisLimitHigh * .05);
                YAxisMin = YAxisLimitLow - (YAxisLimitLow * .05);
            }
            else if ((YAxisLimitHigh > 36) && (YAxisLimitHigh < 120))
            {
                YAxisMax = YAxisLimitHigh + (YAxisLimitHigh * .005);
                YAxisMin = YAxisLimitLow - (YAxisLimitLow * .005);
            }
            else if (YAxisLimitHigh > 125)
            {
                YAxisMax = YAxisLimitHigh + (YAxisLimitHigh * .01);
                YAxisMin = YAxisLimitLow - (YAxisLimitLow * .01);
            }
            else if (YAxisLimitHigh > 800)
            {
                YAxisMax = YAxisLimitHigh + (YAxisLimitHigh * .01);
                YAxisMin = YAxisLimitLow - (YAxisLimitLow * .01);
            }
            else
            {
                YAxisMax = YAxisLimitHigh + (YAxisLimitHigh * .01);
                YAxisMin = YAxisLimitLow - (YAxisLimitLow * .01);
            }
            
        }

        private void UpdateStatus(string strMsg)
        {
            UpdateInfo = strMsg;
        }

        private void Heartbeat()
        {
            if (ShowMe == 0.1) ShowMe = 1;
            else if (ShowMe == 1) ShowMe = 0.1;
        }

        private void GetUpdateData()
        {
            newIndex = CGraphModel.GetNewIndex();
            DropSamples = BSamples + 0;

            //only update new bale!
            if (preIndex != newIndex)
            {
                StartHeartBeatTimer();

                _ = GetUpdateDataAsync(BSamples);

                preIndex = newIndex;

                DateTimeOffset time = DateTimeOffset.Now;
                TimeSpan span = time - lastTime;
                lastTime = time;

                PositionColor = System.Windows.Media.Brushes.White;
                TxtInfo = DateTime.Now.ToString("HH:m:ss");

                switch (Settings.Default.iLanguageIdx)
                {
                    case 0: // "en-US":
                        UpdateStatus("Status : New Bale arrived at : " + DateTime.Now.ToString() + " interval= " + span.ToString("c"));
                        break;
                    case 1: //"Sp-SP":
                        UpdateStatus("Estado : nueva fardoarrive aparecer a : " + DateTime.Now.ToString() + " intervalo= " + span.ToString("hh") + ":" + span.ToString("mm") + ":" + span.ToString("ss"));
                        break;
                    default:
                        UpdateStatus("Status : New Bale arrived at : " + DateTime.Now.ToString() + " interval= " + span.ToString("c"));
                        break;
                }
            }
        }

        private void setMenuOptions()
        {
            MenuOptions = new List<string>
            {
                "Moisture",
                "Weight",
                "BDWeight",
                "ADWeight",
                "Dirt",
                "Brightness",
                "Finish",
                "Forte"
            };

            // MenuOptions.Add("UpCount");
            // MenuOptions.Add("DownCount");
        }

        bool CheckNullOrEmpty(string s)
        {
            bool result;
            result = s == null || s == string.Empty;
            return result;
        }

      
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
            Application.Current.Dispatcher.Invoke(new Action(() => { GetUpdateData(); }));
          
            Thread.Sleep(500); //Rest for 1 Sec.
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
                    UpdateInfo = "Estado :Tiempo de Scan Comienzo";
                    break;
                default:
                    UpdateInfo = "Status : Scan timer Start";
                    break;
            }

        }
        private void StopTimer()
        {
            switch (Settings.Default.iLanguageIdx)
            {
                case 0: // "en-US":
                    UpdateInfo = "Status : Scan timer Stop";
                    break;
                case 1: //"Sp-SP":
                    UpdateInfo = "Estado : Tiempo de Scan Detenido";
                    break;
                default:
                    UpdateInfo = "Status : Scan timer Stop";
                    break;
            }

            if (dispatcherTimer != null)
                dispatcherTimer.Stop();
            
            RTRunning = false;

            if (HeartBeatTimer != null)
            {
                HeartBeatTimer.Stop();
                HeartBeatTimer = null;
            }

            _eventAggregator.GetEvent<UpdatedEvent>().Publish(RTRunning);
            //ShowMe = 0.1;
            Opac = 1.0;
        }

        /// <summary>
        /// HeartBeat Timer
        /// </summary>
        private System.Windows.Threading.DispatcherTimer HeartBeatTimer;
        private void InitializeHeartBeatTimer()
        {
            if (HeartBeatTimer != null) HeartBeatTimer = null;
            HeartBeatTimer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            HeartBeatTimer.Tick += new EventHandler(HeartBeatTimer_Tick);
        }

        private void HeartBeatTimer_Tick(object sender, EventArgs e)
        {
            HeartBeatTimer.Stop();
            ShowMe = 0.1;
        }
        private void StartHeartBeatTimer()
        {
            HeartBeatTimer.Start();
            ShowMe = 0.8;
        }




        #endregion DispatcherTimer /////////////////////////////////////////////////////////////////////////////////////////



    }
}
