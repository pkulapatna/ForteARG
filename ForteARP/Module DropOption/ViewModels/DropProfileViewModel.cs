

using ForteArg.Services;
using ForteARP.Charts;
using ForteARP.Model;
using ForteARP.Module_DropOption.Model;
using ForteARP.Properties;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace ForteARP.Module_DropOption.ViewModels
{
    public class DropProfileViewModel : BindableBase
    {
        protected readonly Prism.Events.IEventAggregator _eventAggregator;

        private readonly DropModel _dropModel;
        private ClsChartColor ChartColors;

        private Sqlhandler _sqlhandler = Sqlhandler.Instance;

        public DelegateCommand LoadedPageICommand { get; set; }
        public DelegateCommand ClosedPageICommand { get; set; }
        public DelegateCommand StartCommand { get; set; }       //Start Button
        public DelegateCommand StopCommand { get; set; }        //Stop Button

        //For Popup
        public DelegateCommand SetFieldCommand { get; set; }
        public DelegateCommand SaveUpdateCommand { get; set; }
        public DelegateCommand CloseSetUpCommand { get; set; }
        public DelegateCommand ModifyCommand { get; set; }
        public DelegateCommand ClearAllCommand { get; set; }
        public DelegateCommand SelectAllCommand { get; set; }

        public DelegateCommand OnCheckCommand { get; set; }

        public DelegateCommand CheckMenuOneCommand { get; set; }
        public DelegateCommand CheckMenuTwoCommand { get; set; }
        public DelegateCommand CheckMenuThreeCommand { get; set; }

        public DelegateCommand CheckMenuFourCommand { get; set; }
        public DelegateCommand CheckMenuFiveCommand { get; set; }
        public DelegateCommand CheckMenuSixCommand { get; set; }
        public DelegateCommand CheckMenuSevenCommand { get; set; }

        private int m_BalesinDrop;

        private string _graphTitle;
        public string GraphTitle
        {
            get { return _graphTitle; }
            set { SetProperty(ref _graphTitle, value); }
        }

        private string _MainChartTitle;
        public string MainChartTitle
        {
            get { return _MainChartTitle; }
            set { SetProperty(ref _MainChartTitle, value); }
        }

        private string _updateinfo;
        public string UpdateInfo
        {
            get { return _updateinfo; }
            set { SetProperty(ref _updateinfo, value); }
        }

        private int _SamplesCount;
        public int SamplesCount
        {
            get { return _SamplesCount; }
            set { SetProperty(ref _SamplesCount, value); }
        }

        private string _MoistureType;
        public string MoistureType
        {
            get { return _MoistureType; }
            set { SetProperty(ref _MoistureType, value); }
        }

        private bool _menuOneChecked;
        public bool MenuOneChecked
        {
            get { return _menuOneChecked; }
            set
            {
                if (value)
                {
                    switch (Settings.Default.iLanguageIdx) //Thread.CurrentThread.CurrentCulture.ToString())
                    {
                        case 0: // "en-US":
                            MainChartTitle = $"Drop Profile of -  {ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Name} ";
                            GraphTitle = "Most Recent - " + DropSamples + " - consecutive drops by Drop Number  [ " + _dropModel.BaleinADrop + " Bales In Each drop ]";
                            break;
                        case 1: //"Sp-SP":
                            MainChartTitle = "Perfil de descenso en seco de -  " + ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Name + " - desde " + _dropModel.CurrentBaleTable;
                            GraphTitle = DropSamples + " - Bajadas mas recientes ordenadas por número de bajada " + _dropModel.BaleinADrop;
                            break;
                        default:
                            MainChartTitle = "Drop Profile of -  " + ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Name + " - from " + _dropModel.CurrentBaleTable;
                            GraphTitle = "Most Recent - " + DropSamples + " - consecutive drops by Drop Number  [ " + _dropModel.BaleinADrop + " Bales In Each drop ]";
                            break;
                    }
                    StrSelectedItem = ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Sname;
                    Settings.Default.iMenuSelected = 0;
                    Settings.Default.Save();
                    if(RTRunning) GetNewData();
                }
                SetProperty(ref _menuOneChecked, value);
            }
        }

        private bool _menuTwoChecked;
        public bool MenuTwoChecked
        {
            get { return _menuTwoChecked; }
            set
            {
                if (value)
                {

                    switch (Settings.Default.iLanguageIdx) //Thread.CurrentThread.CurrentCulture.ToString())
                    {
                        case 0: // "en-US":
                            MainChartTitle = $"Drop Profile of Weight in {ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name}";
                            GraphTitle = "Most Recent - " + DropSamples + " - consecutive drops by Drop Number  [ " + _dropModel.BaleinADrop + " Bales In Each drop ]";
                            break;
                        case 1: //"Sp-SP":
                            MainChartTitle = "Perfil de descenso en seco de Peso en " + ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name + " - desde " + _dropModel.CurrentBaleTable;
                            GraphTitle = DropSamples + " - Bajadas mas recientes ordenadas por número de bajada " + _dropModel.BaleinADrop;
                            break;
                        default:
                            MainChartTitle = "Drop Profile of Weight in " + ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name + " - from " + _dropModel.CurrentBaleTable;
                            GraphTitle = "Most Recent - " + DropSamples + " - consecutive drops by Drop Number  [ " + _dropModel.BaleinADrop + " Bales In Each drop ]";
                            break;
                    }
                    Settings.Default.iMenuSelected = 1;
                    StrSelectedItem = "Weight";
                    Settings.Default.Save();
                    if (RTRunning)  GetNewData();
                }
                SetProperty(ref _menuTwoChecked, value);
            }
        }

        private bool _menuThreeChecked;
        public bool MenuThreeChecked
        {
            get { return _menuThreeChecked; }
            set
            {
                if (value)
                {
                    switch (Settings.Default.iLanguageIdx) //Thread.CurrentThread.CurrentCulture.ToString())
                    {
                        case 0: // "en-US":
                            MainChartTitle = $"Drop Profile of Bone Dry Weight";
                            GraphTitle = "Most Recent - " + DropSamples + " - consecutive drops by Drop Number  [ " + _dropModel.BaleinADrop + " Bales In Each drop ]";
                            break;
                        case 1: //"Sp-SP":
                            MainChartTitle = "Perfil de descenso en seco de Peso BD- desde " + _dropModel.CurrentBaleTable;
                            GraphTitle = DropSamples + " - Bajadas mas recientes ordenadas por número de bajada " + _dropModel.BaleinADrop;
                            break;
                        default:
                            MainChartTitle = "Drop Profile of Bone Dry Weight - from " + _dropModel.CurrentBaleTable;
                            GraphTitle = "Most Recent - " + DropSamples + " - consecutive drops by Drop Number  [ " + _dropModel.BaleinADrop + " Bales In Each drop ]";
                            break;
                    }
                    Settings.Default.iMenuSelected = 2;
                    StrSelectedItem = "BDWeight";
                    Settings.Default.Save();
                    if (RTRunning) GetNewData();
                }
                SetProperty(ref _menuThreeChecked, value);
            }
        }

        private bool _menuFourChecked;
        public bool MenuFourChecked
        {
            get { return _menuFourChecked; }
            set
            {
                if (value)
                {
                    switch (Settings.Default.iLanguageIdx) //Thread.CurrentThread.CurrentCulture.ToString())
                    {
                        case 0: // "en-US":
                            MainChartTitle = $"Drop Profile of AirDry Weight";
                            GraphTitle = "Most Recent - " + DropSamples + " - consecutive drops by Drop Number  [ " + _dropModel.BaleinADrop + " Bales In Each drop ]";
                            break;
                        case 1: //"Sp-SP":
                            MainChartTitle = "Perfil de descenso en seco de Peso AD - desde " + _dropModel.CurrentBaleTable;
                            GraphTitle = DropSamples + " - Bajadas mas recientes ordenadas por número de bajada " + _dropModel.BaleinADrop;
                            break;
                        default:
                            MainChartTitle = "Drop Profile of AirDry Weight - from " + _dropModel.CurrentBaleTable;
                            GraphTitle = "Most Recent - " + DropSamples + " - consecutive drops by Drop Number  [ " + _dropModel.BaleinADrop + " Bales In Each drop ]";
                            break;
                    }
                    Settings.Default.iMenuSelected = 3;
                    StrSelectedItem = "ADWeight";
                    Settings.Default.Save();
                    if (RTRunning) GetNewData();
                }
                SetProperty(ref _menuFourChecked, value);
            }
        }

        private bool _MenuFiveChecked;
        public bool MenuFiveChecked
        {
            get { return _MenuFiveChecked; }
            set
            {
                if (value)
                {
                    switch (Settings.Default.iLanguageIdx) //Thread.CurrentThread.CurrentCulture.ToString())
                    {
                        case 0: // "en-US":
                            MainChartTitle = $"Drop Profile of Forte Number";
                            GraphTitle = "Most Recent - " + DropSamples + " - consecutive drops by Drop Number  [ " + _dropModel.BaleinADrop + " Bales In Each drop ]";
                            break;
                        case 1: //"Sp-SP":
                            MainChartTitle = "Perfil de descenso en seco de Forte Number - desde " + _dropModel.CurrentBaleTable;
                            GraphTitle = DropSamples + " - Bajadas mas recientes ordenadas por número de bajada " + _dropModel.BaleinADrop;
                            break;
                        default:
                            MainChartTitle = "Drop Profile of Forte Number - from " + _dropModel.CurrentBaleTable;
                            GraphTitle = "Most Recent - " + DropSamples + " - consecutive drops by Drop Number  [ " + _dropModel.BaleinADrop + " Bales In Each drop ]";
                            break;
                    }
                    Settings.Default.iMenuSelected = 4;
                    StrSelectedItem = "Forte";
                    Settings.Default.Save();
                    if (RTRunning) GetNewData();
                }
                SetProperty(ref _MenuFiveChecked, value);
            }
        }

        public List<string> _cmbDropDownList;
        public List<string> CmbDropDownList
        {
            get { return _cmbDropDownList; }
            set { SetProperty(ref _cmbDropDownList, value); }
        }

        private ObservableCollection<ChartData> ChartdataOne;
        public ObservableCollection<ChartData> Pos1
        {
            get { return ChartdataOne; }
            set { SetProperty(ref ChartdataOne, value); }
        }

        private ObservableCollection<ChartData> ChartdataTwo;
        public ObservableCollection<ChartData> Pos2
        {
            get { return ChartdataTwo; }
            set { SetProperty(ref ChartdataTwo, value); }
        }

        private ObservableCollection<ChartData> ChartdataThree;
        public ObservableCollection<ChartData> Pos3
        {
            get { return ChartdataThree; }
            set { SetProperty(ref ChartdataThree, value); }
        }

        private ObservableCollection<ChartData> ChartdataFour;
        public ObservableCollection<ChartData> Pos4
        {
            get { return ChartdataFour; }
            set { SetProperty(ref ChartdataFour, value); }
        }

        private ObservableCollection<ChartData> ChartdataFive;
        public ObservableCollection<ChartData> Pos5
        {
            get { return ChartdataFive; }
            set { SetProperty(ref ChartdataFive, value); }
        }

        private ObservableCollection<ChartData> ChartdataSix;
        public ObservableCollection<ChartData> Pos6
        {
            get { return ChartdataSix; }
            set { SetProperty(ref ChartdataSix, value); }
        }


        private ObservableCollection<ChartData> ChartdataSeven;
        public ObservableCollection<ChartData> Pos7
        {
            get { return ChartdataSeven; }
            set { SetProperty(ref ChartdataSeven, value); }
        }

        private ObservableCollection<ChartData> ChartdataEight;
        public ObservableCollection<ChartData> Pos8
        {
            get { return ChartdataEight; }
            set { SetProperty(ref ChartdataEight, value); }
        }

        private ObservableCollection<ChartData> ChartdataNine;
        public ObservableCollection<ChartData> Pos9
        {
            get { return ChartdataNine; }
            set { SetProperty(ref ChartdataNine, value); }
        }

        private ObservableCollection<ChartData> ChartdataTen;
        public ObservableCollection<ChartData> Pos10
        {
            get { return ChartdataTen; }
            set { SetProperty(ref ChartdataTen, value); }
        }

        private ObservableCollection<ChartData> ChartdataEleven;
        public ObservableCollection<ChartData> Pos11
        {
            get { return ChartdataEleven; }
            set { SetProperty(ref ChartdataEleven, value); }
        }

        private ObservableCollection<ChartData> ChartdataTwelve;
        public ObservableCollection<ChartData> Pos12
        {
            get { return ChartdataTwelve; }
            set { SetProperty(ref ChartdataTwelve, value); }
        }

        private ObservableCollection<CheckedListItem> _AvailableHdrList;
        public ObservableCollection<CheckedListItem> AvailableHdrList
        {
            get { return _AvailableHdrList; }
            set { SetProperty(ref _AvailableHdrList, value); }
        }

        private ObservableCollection<string> _selectedItemList;
        public ObservableCollection<string> SelectedHdrList
        {
            get { return _selectedItemList; }
            set { SetProperty(ref _selectedItemList, value); }
        }


        private Brush _GridRowBackGround = Brushes.Transparent;
        public Brush GridRowBackGround
        {
            get { return _GridRowBackGround; }
            set { SetProperty(ref _GridRowBackGround, value); }
        }

        private string _DropNumber;
        public string DropNumber
        {
            get { return _DropNumber; }
            set { SetProperty(ref _DropNumber, value); }
        }

        private List<Brush> CrtColorList;
        private Brush Crt_1Color;
        private Brush Crt_2Color;
        private Brush Crt_3Color;
        private Brush Crt_4Color;
        private Brush Crt_5Color;
        private Brush Crt_6Color;
        private Brush Crt_7Color;
        private Brush Crt_8Color;
        private Brush Crt_9Color;
        private Brush Crt_10Color;
        private Brush Crt_11Color;
        private Brush Crt_12Color;

        private Brush NewDropColor;
        //private List<string> BigNumberList;
        // private List<string> ColumnDisplayList;
        // private List<CheckedListItem> AvailableItemList;
        private DataTable Hdrtable;
        /////////////////////////////////

        private List<string> LstHeaderAllFields;

        //private bool bSetupOpen = false;
        private List<string> LstHeader;

        //private string strMonth;
        // private string strQueryString = string.Empty;
        //private int GrdViewColWidthCon;
        //private int ioldIndex;
        //private int iScreenWidth;
        //private int intLineID = 1;
        //private int m_iMoistureType;
        //private int m_iWeightType;

        private int CurIndex;
        private int PreIndex;

        private int IntColSamples = 0;

      
        private DataTable _RealTimeDataTable;
        public DataTable RealTimeDataTable
        {
            get { return _RealTimeDataTable; }
            set { SetProperty(ref _RealTimeDataTable, value); }
        }

        private string _strSelectedItem;
        public string StrSelectedItem
        {
            get { return _strSelectedItem; }
            set { SetProperty(ref _strSelectedItem, value); }
        }

        private int _hdrColWidth;
        public int HdrColWidth
        {
            get { return _hdrColWidth; }
            set { SetProperty(ref _hdrColWidth, value); }
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

        private bool _RTIdle = false;
        public bool RTIdle
        {
            get { return _RTIdle; }
            set { SetProperty(ref _RTIdle, value); }
        }

        private int _BalePosition;
        public int BalePosition
        {
            get { return _BalePosition; }
            set { SetProperty(ref _BalePosition, value); }
        }

        private string _BaleMoisture = string.Empty;
        public string BaleMoisture
        {
            get { return _BaleMoisture; }
            set { SetProperty(ref _BaleMoisture, value); }
        }

        private string _BaleWeight = string.Empty;
        public string BaleWeight
        {
            get { return _BaleWeight; }
            set { SetProperty(ref _BaleWeight, value); }
        }

        private string _BaleLotNumber = string.Empty;
        public string BaleLotNumber
        {
            get { return _BaleLotNumber; }
            set { SetProperty(ref _BaleLotNumber, value); }
        }

        private string _BaleSerialNumber = string.Empty;
        public string BaleSerialNumber
        {
            get { return _BaleSerialNumber; }
            set { SetProperty(ref _BaleSerialNumber, value); }
        }

        public int BalesInOneDrop { get; set; }
        private DataTable Mydatatable;

        private int _dropsample = 0;
        public int DropSamples
        {
            get { return _dropsample; }
            set
            {
                if ((value > 1) & (value < 5))
                    SetProperty(ref _dropsample, value);
                else
                    SetProperty(ref _dropsample, 5);
                Settings.Default.iDropProfile = value;
                Settings.Default.Save();
            }
        }

        private bool _bOpenSetup = false;
        public bool OpenSetup
        {
            get { return _bOpenSetup; }
            set { SetProperty(ref _bOpenSetup, value); }
        }

        private bool _bCloseSetup;
        public bool CloseSetup
        {
            get { return _bOpenSetup; }
            set { SetProperty(ref _bCloseSetup, value); }
        }

        private bool _bModSetup = false;
        public bool BModifySetup
        {
            get { return _bModSetup; }
            set { SetProperty(ref _bModSetup, value); }
        }


        private int _selectedBox1Combo;
        public int SelectedBox1Combo
        {
            get { return _selectedBox1Combo; }
            set
            {
                if (value > -1)
                    Settings.Default.iDropProBox1 = value;
                Settings.Default.Save();
                SetProperty(ref _selectedBox1Combo, value);
            }
        }

        private int _selectedBox2Combo;
        public int SelectedBox2Combo
        {
            get { return _selectedBox2Combo; }
            set
            {
                if (value > -1)
                    Settings.Default.iDropProBox2 = value;
                Settings.Default.Save();
                SetProperty(ref _selectedBox2Combo, value);
            }
        }

        private int _selectedBox3Combo;
        public int SelectedBox3Combo
        {
            get { return _selectedBox3Combo; }
            set
            {
                if (value > -1)
                    Settings.Default.iDropProBox3 = value;
                Settings.Default.Save();
                SetProperty(ref _selectedBox3Combo, value);
            }
        }

        private int _selectedBox4Combo;
        public int SelectedBox4Combo
        {
            get { return _selectedBox4Combo; }
            set
            {
                if (value > -1)
                    Settings.Default.iDropProBox4 = value;
                Settings.Default.Save();
                SetProperty(ref _selectedBox4Combo, value);
            }
        }

        private string _bigNumBox1;
        public string BigNumBox1
        {
            get { return _bigNumBox1; }
            set { SetProperty(ref _bigNumBox1, value); }
        }

        private string _bigNumBox2;
        public string BigNumBox2
        {
            get { return _bigNumBox2; }
            set { SetProperty(ref _bigNumBox2, value); }
        }

        private string _bigNumBox3;
        public string BigNumBox3
        {
            get { return _bigNumBox3; }
            set { SetProperty(ref _bigNumBox3, value); }
        }

        private string _bigNumBox4;
        public string BigNumBox4
        {
            get { return _bigNumBox4; }
            set { SetProperty(ref _bigNumBox4, value); }
        }



        private string m_Line;
        private string m_Source;

        /// <summary>
        /// Sources -------------------------------------------------------------------
        /// </summary>
        private List<string> _SourceList;
        public List<string> SourceList
        {
            get { return _SourceList; }
            set { SetProperty(ref _SourceList, value); }
        }
        private int _SelectSourceIndex;
        public int SelectSourceIndex
        {
            get { return _SelectSourceIndex; }
            set
            {
                SetProperty(ref _SelectSourceIndex, value);
                if (value > -1)
                {
                    m_Source = SourceList[value];
                    Settings.Default.iDropProfileSourceIndex = value;
                    Settings.Default.Save();
                }

            }
        }
        /// <summary>
        /// Lines -------------------------------------------------------------------
        /// </summary>
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
                    m_Line = LineList[value];
                    Settings.Default.iDropProfileLineIndex = value;
                    Settings.Default.Save();
                }
            }
        }
        //---------------------------------------------------------------------------

        private bool _HdrListboxIsChecked;
        public bool HdrListboxIsChecked
        {
            get { return _HdrListboxIsChecked; }
            set { SetProperty(ref _HdrListboxIsChecked, value); }
        }

        private int _DGFontSize;
        public int DGFontSize
        {
            get { return _DGFontSize; }
            set { SetProperty(ref _DGFontSize, value); }
        }


        public int iBaleCount;

        private double MoistureAvg = 0.0;
        private double WeightAvg = 0.0;
        private double BDWeightAvg = 0.0;
        private double ADWeightAvg = 0.0;
        private double NetWeightAvg = 0.0;
        private double BasisWeightAvg = 0.0;


        public DropProfileViewModel(Prism.Events.IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
            HdrColWidth = 115;

            _dropModel = new DropModel();

          

            RTRunning = false;

            LoadedPageICommand = new DelegateCommand(LoadedPageExecute, LoadedPageCanExecute);
            ClosedPageICommand = new DelegateCommand(ClosedPageExecute, ClosedPageCanExecute);

            StartCommand = new DelegateCommand(StartExecute, StartCanExecute).ObservesProperty(() => RTRunning);
            StopCommand = new DelegateCommand(StopExecute, StopCanExecute).ObservesProperty(() => RTRunning);

            //PopUp Windows
            SetFieldCommand = new DelegateCommand(SetFieldExecute, SetFieldCanExecute).ObservesProperty(() => RTRunning);
            CloseSetUpCommand = new DelegateCommand(CloseSetupExecute, CloseSetupCanExecute).ObservesProperty(() => OpenSetup);
            ModifyCommand = new DelegateCommand(ModifyExecute, ModifyCanExecute);
            SaveUpdateCommand = new DelegateCommand(SaveModExecute, SaveModCanExecute).ObservesProperty(() => BModifySetup).ObservesProperty(() => OpenSetup);
            SelectAllCommand = new DelegateCommand(SelectAllExecute, SelectAllCanExecute).ObservesProperty(() => BModifySetup);
            ClearAllCommand = new DelegateCommand(ClearAllExecute, ClearAllCanExecute).ObservesProperty(() => BModifySetup);
            OnCheckCommand = new DelegateCommand(OnCheckExecute, OnCheckCanExecute).ObservesProperty(() => BModifySetup);

            CheckMenuOneCommand = new DelegateCommand(CheckMenuOneExecute, CheckMenuOneCanExecute).ObservesProperty(() => RTRunning);
            CheckMenuTwoCommand = new DelegateCommand(CheckMenuTwoExecute, CheckMenuTwoCanExecute).ObservesProperty(() => RTRunning);
            CheckMenuThreeCommand = new DelegateCommand(CheckMenuThreeExecute, CheckMenuThreeCanExecute).ObservesProperty(() => RTRunning);
            CheckMenuFourCommand = new DelegateCommand(CheckMenuFourExecute, CheckMenuFourCanExecute).ObservesProperty(() => RTRunning);
            CheckMenuFiveCommand = new DelegateCommand(CheckMenuFiveExecute, CheckMenuFiveCanExecute).ObservesProperty(() => RTRunning);
            _eventAggregator.GetEvent<UpdatedEventShutdown>().Subscribe(ProgramShutdown);
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

        private void CheckMenuFiveExecute()
        {
            MenuFiveChecked = true;
        }

        private bool CheckMenuFiveCanExecute()
        {
            return !RTRunning;
        }

        private void CheckMenuFourExecute()
        {
            MenuFourChecked = true;
        }

        private bool CheckMenuFourCanExecute()
        {
            return !RTRunning;
        }

        private bool CheckMenuOneCanExecute()
        {
            return !RTRunning;
        }

        private void CheckMenuOneExecute()
        {
            MenuOneChecked = true;
        }

        private bool CheckMenuTwoCanExecute()
        {
            return !RTRunning;
        }

        private void CheckMenuTwoExecute()
        {
            MenuTwoChecked = true;
        }

        private bool CheckMenuThreeCanExecute()
        {
            return !RTRunning;
        }

        private void CheckMenuThreeExecute()
        {
            MenuThreeChecked = true;
        }

        private bool ModifyCanExecute()
        {
            return true;
        }

        private void ModifyExecute()
        {
            BModifySetup = true;
        }

        private bool CloseSetupCanExecute()
        {
            return OpenSetup;
        }

        private void CloseSetupExecute()
        {
            BModifySetup = false;
            OpenSetup = false;
        }

        private bool OnCheckCanExecute()
        {
            return BModifySetup;
        }

        private void OnCheckExecute()
        {
            ObservableCollection<string> NewList = new ObservableCollection<string>();
            ObservableCollection<string> orgList = SelectedHdrList;

            for (int i = 0; i < AvailableHdrList.Count; i++)
            {
                if (AvailableHdrList[i].IsChecked == true) NewList.Add(AvailableHdrList[i].Name);
            }

            if (orgList.Count > NewList.Count) //Remove item
            {
                IEnumerable<string> ItemRemove = orgList.Except(NewList);
                SelectedHdrList = _dropModel.RemoveHdrItem(orgList, ItemRemove.ElementAt(0).ToString());
            }
            else //add item
            {
                IEnumerable<string> ItemAdd = NewList.Except(orgList);
                SelectedHdrList = _dropModel.AddHdrItem(orgList, ItemAdd.ElementAt(0).ToString());
            }
        }

        private bool ClearAllCanExecute()
        {
            return false; // return BModifySetup;
        }

        private void ClearAllExecute()
        {
            //throw new NotImplementedException();
        }

        private bool SelectAllCanExecute()
        {
            return false;  //return BModifySetup;
        }

        private void SelectAllExecute()
        {
            //throw new NotImplementedException();
        }

        private bool SaveModCanExecute()
        {
            return BModifySetup;
        }

        private void SaveModExecute()
        {
            _dropModel.SaveXmlcolumnList(SelectedHdrList);

            BModifySetup = false;
            OpenSetup = false; //Close the popup window
        }

        private void Set_DropdownList()
        {
            CmbDropDownList = new List<string>
            {
                "-Blank-",
                "Avg - " + ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Name,
                "Avg - Weight in - " + ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name,
                "Avg - BD Weight",
                "Avg - Net Weight",
                "Avg - Basis Weight",
                "Avg - AD Weight",
                "Drop Number"
            };

            SelectedBox1Combo = Settings.Default.iDropProBox1;
            SelectedBox2Combo = Settings.Default.iDropProBox2;
            SelectedBox3Combo = Settings.Default.iDropProBox3;
            SelectedBox4Combo = Settings.Default.iDropProBox4;
        }

        private bool SetFieldCanExecute()
        {
            return !RTRunning;
        }

        private void SetFieldExecute()
        {
            MyPopup_Open();
        }

        private void MyPopup_Open()
        {
            this.OpenSetup = true;
        }

        private bool StopCanExecute()
        {
            return RTRunning;
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

            InitializeTimer();
            StartTimer();


            m_BalesinDrop = _dropModel.BaleinADrop;

            RTRunning = true;
            _eventAggregator.GetEvent<UpdatedEvent>().Publish(RTRunning);
        }

        private bool ClosedPageCanExecute()
        {
            return true;
        }

        private void ClosedPageExecute()
        {
            if (dispatcherTimer != null) dispatcherTimer = null;
            if (Mydatatable != null) Mydatatable = null;
            if (SelectedHdrList != null) SelectedHdrList = null;
            if (AvailableHdrList != null) AvailableHdrList = null;
        }

        private bool LoadedPageCanExecute()
        {
            return true;
        }

        private void Set_Popupwindow()
        {
            //Setup All Listbox
            SelectedHdrList.Clear();
            SelectedHdrList = _dropModel.GetXmlSelectedHdrCheckedList();
            AvailableHdrList = _dropModel.AvailableItemList; // Checkbox selected ListBox items in popup
            AvailableHdrList = new ObservableCollection<CheckedListItem>(AvailableHdrList.OrderBy(x => x.Name)); //Sort
        }

        private void LoadedPageExecute()
        {
            _dropModel.SetUpSql();

            try
            {
                MoistureType = ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Name;

                _dropModel.GetLineSource();

                Mydatatable = new DataTable();
                SelectedHdrList = new ObservableCollection<string>();
                LstHeader = new List<string>();
                LstHeaderAllFields = new List<string>();

                DropSamples = 12; // Settings.Default.iDropProfile;
                _dropModel.InitSqlDropProfileModel();

                Set_Popupwindow();
                Set_DropdownList();

                LineList = new List<string>();
                SourceList = new List<string>();
                
                LineList = _dropModel.GetListof("LineID");
                SourceList = _dropModel.GetListof("SourceID");


                if (Settings.Default.iDropProfileLineIndex >= LineList.Count)
                    SelectLineIndex = 0;
                else
                    SelectLineIndex = Settings.Default.iDropProfileLineIndex;

                if (Settings.Default.iDropProfileSourceIndex >= SourceList.Count)
                    SelectSourceIndex = 0;
                else
                    SelectSourceIndex = Settings.Default.iDropProfileSourceIndex;

                MainWindow.AppWindows.SetupAppTitle("Forté Drop Profile From  -> " + _sqlhandler.Host + @"\" + _sqlhandler.SqlInstance);


                //m_iMoistureType = Settings.Default.MoistureUnit;
               // m_iWeightType = Settings.Default.WeightUnit;

                RTRunning = false;

                switch (Settings.Default.iMenuSelected)
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
                    default:
                        MenuOneChecked = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in RemoteProfile LoadedPageExecute " + ex.Message);
            }
        }

        private void InitGraph()
        {
            BalesInOneDrop = _dropModel.BaleinADrop;
            IntColSamples = (BalesInOneDrop * DropSamples) + BalesInOneDrop; //Added extra BalesInOneDrop bales,
                                                                             //incase count in the middle of 7           
            SamplesCount = DropSamples + 1; //Add extra end point, To move end graph in 1 position
            SetChartColors();
        }

        private void Heartbeat()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// This will Update data and graphic when a new drop arrives.
        /// </summary>
        private void GetNewData()
        {
            int iLastBale;
            InitGraph();

            if (Settings.Default.bDropHitoLow)
                iLastBale = 1;
            else
                iLastBale = ClassCommon.BaleInADrop; // Settings.Default.iBalesInDrop;

            try
            {
                Mydatatable = _dropModel.GetNewDataTable(BuildQueryString());

                if (Mydatatable.Rows.Count > 0)
                {
                    DataColumn NewCol1 = Mydatatable.Columns.Add("ADWeight", typeof(Single));
                    DataColumn NewCol2 = Mydatatable.Columns.Add("BoneDry%", typeof(Single));
                    DataColumn NewCol3 = Mydatatable.Columns.Add("AirDry%", typeof(Single));
                    DataColumn NewCol4 = Mydatatable.Columns.Add("Dirt_mm2/kg2", typeof(Single));
                    DataColumn NewCol5 = Mydatatable.Columns.Add("Regain%", typeof(Single));
                    Mydatatable.AcceptChanges();

                    for (int i = 0; i < Mydatatable.Rows.Count; i++)
                    {
                        if (Mydatatable.Rows[i]["BDWeight"] != null)
                            Mydatatable.Rows[i]["ADWeight"] = Mydatatable.Rows[i].Field<Single>("BDWeight") / 0.9;

                        if (Mydatatable.Rows[i]["Moisture"] != null)
                        {
                            Mydatatable.Rows[i]["AirDry%"] = (100 - Mydatatable.Rows[i].Field<Single>("Moisture")) / 0.9;
                            Mydatatable.Rows[i]["BoneDry%"] = 100 - Mydatatable.Rows[i].Field<Single>("Moisture");
                            Mydatatable.Rows[i]["Regain%"] = Mydatatable.Rows[i].Field<Single>("Moisture") / (1 - Mydatatable.Rows[i].Field<Single>("Moisture") / 100);
                        }

                        if ((Mydatatable.Rows[i]["BasisWeight"] != null) & (Mydatatable.Rows[i].Field<Single>("BasisWeight") > 0))
                            Mydatatable.Rows[i]["Dirt_mm2/kg2"] = Mydatatable.Rows[i].Field<Single>("Dirt") / Mydatatable.Rows[i].Field<Single>("BasisWeight") * 2000;
                        else
                            Mydatatable.Rows[i]["Dirt_mm2/kg2"] = 0;
                    }
                }

                if (Convert.ToInt32(Mydatatable.Rows[0]["Position"].ToString()) > 0)
                {

                    BalePosition = Convert.ToInt32(Mydatatable.Rows[0]["Position"].ToString()); // Mydatatable.Rows[0].Field<Int32>("Position");

                    double dMoise = Mydatatable.Rows[0].Field<Single>(ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Sname);

                    BaleMoisture = dMoise.ToString("#0.00");

                    double dWeight = Mydatatable.Rows[0].Field<Single>("Weight");
                    BaleWeight = dWeight.ToString("#0.00");
                    BaleLotNumber = Mydatatable.Rows[0]["LotBaleNumber"].ToString();
                    BaleSerialNumber = Mydatatable.Rows[0]["SerialNumber"].ToString();

                    if (MainWindow.AppWindows.bDebug)
                        Console.WriteLine("BalePosition= " + BalePosition);

                    if (BalePosition == iLastBale)
                    {
                        CurIndex = Mydatatable.Rows[0].Field<Int32>("index");

                        if (CurIndex != PreIndex) // not for the same bale!
                        {
                            Process_ChartData(Mydatatable);
                            Process_GridviewData(Mydatatable);
                            PreIndex = CurIndex;

                            switch (Settings.Default.iLanguageIdx)
                            {
                                case 0: // "en-US":
                                    UpdateInfo = "New Drop Data arrived at " + DateTime.Now;
                                    break;
                                case 1: //"Sp-SP":
                                    UpdateInfo = "nueva fardoarrive aparecer a " + DateTime.Now;
                                    break;
                                default:
                                    UpdateInfo = "New Drop Data arrived at " + DateTime.Now;
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in ViewModel GetNewData " + ex.Message);
            }
        }


        /// <summary>
        /// Update Chart data up to 12 drops max.
        /// </summary>
        /// <param name="mydatatable"></param>
        private void Process_ChartData(DataTable mydat)
        {
            int iMakRows = mydat.Rows.Count;

            try
            {
                if (iMakRows > 0)
                {
                    SetChartNew();
                    int i1 = 1;
                    int i2 = 1;
                    int i3 = 1;
                    int i4 = 1;
                    int i5 = 1;
                    int i6 = 1;
                    int i7 = 1;
                    int i8 = 1;
                    int i9 = 1;
                    int i10 = 1;
                    int i11 = 1;
                    int i12 = 1;

                    Brush mycolor;

                    DropSamples = 13;

                    int iMaxBaleinDrop = m_BalesinDrop + 1;


                    foreach (DataRow item in mydat.Rows)
                    {
                        // one
                        if ((iMaxBaleinDrop - 1) > 0)
                            if (item.Field<byte>("Position") == GetPosition(1))
                            {
                                if (i1 <= DropSamples)
                                {
                                    if (i1 == 1)
                                        mycolor = NewDropColor;
                                    else
                                        mycolor = Crt_1Color;

                                    if (StrSelectedItem != string.Empty)
                                    {
                                        if (StrSelectedItem == "Forte")
                                        {
                                            Pos1.Add(new ChartData()
                                            {
                                                Index = i1,
                                                Value = item.Field<int>(StrSelectedItem),
                                                ChartColor = mycolor
                                            });
                                        }
                                        else
                                        {
                                            Pos1.Add(new ChartData()
                                            {
                                                Index = i1,
                                                Value = item.Field<Single>(StrSelectedItem),
                                                ChartColor = mycolor
                                            });
                                        }
                                    }
                                }
                                i1 += 1;
                            }

                        // two
                        if ((iMaxBaleinDrop - 2) > 0)
                            if (item.Field<byte>("Position") == GetPosition(2))
                            {
                                if (i2 <= DropSamples)
                                {
                                    if (i2 == 1)
                                        mycolor = NewDropColor;
                                    else
                                        mycolor = Crt_2Color;

                                    if (StrSelectedItem != string.Empty)
                                    {
                                        if (StrSelectedItem == "Forte")
                                        {
                                            Pos2.Add(new ChartData() { Index = i2, Value = item.Field<int>(StrSelectedItem), ChartColor = mycolor });
                                        }
                                        else
                                        {
                                            Pos2.Add(new ChartData() { Index = i2, Value = item.Field<Single>(StrSelectedItem), ChartColor = mycolor });
                                        }
                                    }
                                }
                                i2 += 1;
                            }

                        // three
                        if ((iMaxBaleinDrop - 3) > 0)
                            if (item.Field<byte>("Position") == GetPosition(3))
                            {
                                if (i3 <= DropSamples)
                                {
                                    if (i3 == 1)
                                        mycolor = NewDropColor;
                                    else
                                        mycolor = Crt_3Color;

                                    if (StrSelectedItem != string.Empty)
                                    {
                                        if (StrSelectedItem == "Forte")
                                        {
                                            Pos3.Add(new ChartData() { Index = i3, Value = item.Field<int>(StrSelectedItem), ChartColor = mycolor });
                                        }
                                        else
                                        {
                                            Pos3.Add(new ChartData() { Index = i3, Value = item.Field<Single>(StrSelectedItem), ChartColor = mycolor });
                                        }
                                    }
                                }
                                i3 += 1;
                            }

                        // Four
                        if ((iMaxBaleinDrop - 4) > 0)
                            if (item.Field<byte>("Position") == GetPosition(4))
                            {
                                if (i4 <= DropSamples)
                                {
                                    if (i4 == 1)
                                        mycolor = NewDropColor;
                                    else
                                        mycolor = Crt_4Color;

                                    if (StrSelectedItem != string.Empty)
                                    {
                                        if (StrSelectedItem == "Forte")
                                        {
                                            Pos4.Add(new ChartData() { Index = i4, Value = item.Field<int>(StrSelectedItem), ChartColor = mycolor });
                                        }
                                        else
                                        {
                                            Pos4.Add(new ChartData() { Index = i4, Value = item.Field<Single>(StrSelectedItem), ChartColor = mycolor });
                                        }
                                    }
                                }
                                i4 += 1;
                            }

                        // Five
                        if ((iMaxBaleinDrop - 5) > 0)
                            if (item.Field<byte>("Position") == GetPosition(5))
                            {
                                if (i5 <= DropSamples)
                                {
                                    if (i5 == 1)
                                        mycolor = NewDropColor;
                                    else
                                        mycolor = Crt_5Color;

                                    if (StrSelectedItem != string.Empty)
                                    {
                                        if (StrSelectedItem == "Forte")
                                        {
                                            Pos5.Add(new ChartData() { Index = i5, Value = item.Field<int>(StrSelectedItem), ChartColor = mycolor });
                                        }
                                        else
                                        {
                                            Pos5.Add(new ChartData() { Index = i5, Value = item.Field<Single>(StrSelectedItem), ChartColor = mycolor });
                                        }
                                    }
                                }
                                i5 += 1;
                            }

                        // Six          
                        if ((iMaxBaleinDrop - 6) > 0)
                            if (item.Field<byte>("Position") == GetPosition(6))
                            {
                                if (i6 <= DropSamples)
                                {
                                    if (i6 == 1)
                                        mycolor = NewDropColor;
                                    else
                                        mycolor = Crt_6Color;

                                    if (StrSelectedItem != string.Empty)
                                    {
                                        if (StrSelectedItem == "Forte")
                                        {
                                            Pos6.Add(new ChartData() { Index = i6, Value = item.Field<int>(StrSelectedItem), ChartColor = mycolor });
                                        }
                                        else
                                        {
                                            Pos6.Add(new ChartData() { Index = i6, Value = item.Field<Single>(StrSelectedItem), ChartColor = mycolor });
                                        }
                                    }
                                }
                                i6 += 1;
                            }

                        // Seven 
                        if ((iMaxBaleinDrop - 7) > 0)
                            if (item.Field<byte>("Position") == GetPosition(7))
                            {
                                if (i7 <= DropSamples)
                                {
                                    if (i7 == 1)
                                        mycolor = NewDropColor;
                                    else
                                        mycolor = Crt_7Color;

                                    if (StrSelectedItem != string.Empty)
                                    {
                                        if (StrSelectedItem == "Forte")
                                        {
                                            Pos7.Add(new ChartData() { Index = i7, Value = item.Field<int>(StrSelectedItem), ChartColor = mycolor });
                                        }
                                        else
                                        {
                                            Pos7.Add(new ChartData() { Index = i7, Value = item.Field<Single>(StrSelectedItem), ChartColor = mycolor });
                                        }
                                    }
                                }
                                i7 += 1;
                            }

                        // Eight
                        if ((iMaxBaleinDrop - 8) > 0)
                            if (item.Field<byte>("Position") == GetPosition(8))
                            {
                                if (i8 <= DropSamples)
                                {
                                    if (i8 == 1)
                                        mycolor = NewDropColor;
                                    else
                                        mycolor = Crt_8Color;

                                    if (StrSelectedItem != string.Empty)
                                    {
                                        if (StrSelectedItem == "Forte")
                                        {
                                            Pos8.Add(new ChartData() { Index = i8, Value = item.Field<int>(StrSelectedItem), ChartColor = mycolor });
                                        }
                                        else
                                        {
                                            Pos8.Add(new ChartData() { Index = i8, Value = item.Field<Single>(StrSelectedItem), ChartColor = mycolor });
                                        }
                                    }
                                }
                                i8 += 1;
                            }

                        // Nine
                        if ((iMaxBaleinDrop - 9) > 0)
                            if (item.Field<byte>("Position") == GetPosition(9))
                            {
                                if (i9 <= DropSamples)
                                {
                                    if (i9 == 1)
                                        mycolor = NewDropColor;
                                    else
                                        mycolor = Crt_9Color;

                                    if (StrSelectedItem != string.Empty)
                                    {
                                        if (StrSelectedItem == "Forte")
                                        {
                                            Pos9.Add(new ChartData() { Index = i9, Value = item.Field<int>(StrSelectedItem), ChartColor = mycolor });
                                        }
                                        else
                                        {
                                            Pos9.Add(new ChartData() { Index = i9, Value = item.Field<Single>(StrSelectedItem), ChartColor = mycolor });
                                        }
                                    }
                                }
                                i9 += 1;
                            }

                        // Ten
                        if ((iMaxBaleinDrop - 10) > 0)
                            if (item.Field<byte>("Position") == GetPosition(10))
                            {
                                if (i10 <= DropSamples)
                                {
                                    if (i10 == 1)
                                        mycolor = NewDropColor;
                                    else
                                        mycolor = Crt_10Color;

                                    if (StrSelectedItem != string.Empty)
                                    {
                                        if (StrSelectedItem == "Forte")
                                        {
                                            Pos10.Add(new ChartData() { Index = i10, Value = item.Field<int>(StrSelectedItem), ChartColor = mycolor });
                                        }
                                        else
                                        {
                                            Pos10.Add(new ChartData() { Index = i10, Value = item.Field<Single>(StrSelectedItem), ChartColor = mycolor });
                                        }
                                    }
                                }
                                i10 += 1;
                            }

                        // Eleven
                        if ((iMaxBaleinDrop - 11) > 0)
                            if (item.Field<byte>("Position") == GetPosition(11))
                            {
                                if (i11 <= DropSamples)
                                {
                                    if (i11 == 1)
                                        mycolor = NewDropColor;
                                    else
                                        mycolor = Crt_11Color;

                                    if (StrSelectedItem != string.Empty)
                                    {
                                        if (StrSelectedItem == "Forte")
                                        {
                                            Pos11.Add(new ChartData() { Index = i11, Value = item.Field<int>(StrSelectedItem), ChartColor = mycolor });
                                        }
                                        else
                                        {
                                            Pos11.Add(new ChartData() { Index = i11, Value = item.Field<Single>(StrSelectedItem), ChartColor = mycolor });
                                        }
                                    }
                                }
                                i11 += 1;
                            }

                        // Twelve 
                        if ((iMaxBaleinDrop - 12) > 0)
                            if (item.Field<byte>("Position") == GetPosition(12))
                            {
                                if (i12 <= DropSamples)
                                {
                                    if (i12 == 1)
                                        mycolor = NewDropColor;
                                    else
                                        mycolor = Crt_12Color;

                                    if (StrSelectedItem != string.Empty)
                                    {
                                        if (StrSelectedItem == "Forte")
                                        {
                                            Pos12.Add(new ChartData() { Index = i12, Value = item.Field<int>(StrSelectedItem), ChartColor = mycolor });
                                        }
                                        else
                                        {
                                            Pos12.Add(new ChartData() { Index = i12, Value = item.Field<Single>(StrSelectedItem), ChartColor = mycolor });
                                        }
                                    }
                                }
                                i12 += 1;
                            }

                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in ViewModel Process_ChartData " + ex.Message);
            }
        }

        private int GetPosition(int ipos)
        {
            int ipositon;

            if (Settings.Default.bDropHitoLow)
            {
                if (DropSamples > ipos)
                    ipositon = DropSamples - ipos;
                else
                    ipositon = 0;
            }
            else
            {
                ipositon = ipos;
            }
            return ipositon;
        }

        private void SetChartNew()
        {
            int i = 1;

            if (Pos1 != null) Pos1 = null;
            if (i <= BalesInOneDrop)
                Pos1 = new ObservableCollection<ChartData>();
            i += 1;

            if (Pos2 != null) Pos2 = null;
            if (i <= BalesInOneDrop)
                Pos2 = new ObservableCollection<ChartData>();
            i += 1;

            if (Pos3 != null) Pos3 = null;
            if (i <= BalesInOneDrop)
                Pos3 = new ObservableCollection<ChartData>();
            i += 1;

            if (Pos4 != null) Pos4 = null;
            if (i <= BalesInOneDrop)
                Pos4 = new ObservableCollection<ChartData>();
            i += 1;

            if (Pos5 != null) Pos5 = null;
            if (i <= BalesInOneDrop)
                Pos5 = new ObservableCollection<ChartData>();
            i += 1;

            if (Pos6 != null) Pos6 = null;
            if (i <= BalesInOneDrop)
                Pos6 = new ObservableCollection<ChartData>();
            i += 1;

            //////////////////////////////////////////////

            if (Pos7 != null) Pos7 = null;
            if (i <= BalesInOneDrop)
                Pos7 = new ObservableCollection<ChartData>();
            i += 1;

            if (Pos8 != null) Pos8 = null;
            if (i <= BalesInOneDrop)
                Pos8 = new ObservableCollection<ChartData>();
            i += 1;

            if (Pos9 != null) Pos9 = null;
            if (i <= BalesInOneDrop)
                Pos9 = new ObservableCollection<ChartData>();
            i += 1;

            if (Pos10 != null) Pos10 = null;
            if (i <= BalesInOneDrop)
                Pos10 = new ObservableCollection<ChartData>();
            i += 1;

            if (Pos11 != null) Pos11 = null;
            if (i <= BalesInOneDrop)
                Pos11 = new ObservableCollection<ChartData>();
            i += 1;

            if (Pos12 != null) Pos12 = null;
            if (i <= BalesInOneDrop)
                Pos12 = new ObservableCollection<ChartData>();
            i += 1;
        }


        private void Process_GridviewData(DataTable Mydatatable)
        {
            try
            {
                if (Mydatatable.Columns.Contains("index"))
                    Mydatatable.Columns.Remove("index");

                UpdateGridView(Mydatatable);
                UpdateBigNumbers(Mydatatable);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in ViewModel ProcessLVLine1 " + ex.Message);
            }
        }

       
        private void UpdateGridView(DataTable dropTable)
        {

            DataTable TestTable = new DataTable();

            if (LstHeader.Contains("-Blank-"))
                LstHeader.Remove("-Blank-");

            try
            {

                for (int i = 0; i < LstHeader.Count; i++)
                {
                    TestTable.Columns.Add(LstHeader[i], dropTable.Columns[LstHeader[i]].DataType);
                }

                //using linq to add empty rows
                dropTable.AsEnumerable().All(row => { TestTable.Rows.Add(); return true; });


                foreach (DataColumn v_Column in TestTable.Columns)
                {
                    string ColumnName = v_Column.ColumnName;

                    if (dropTable.Columns.Contains(ColumnName))
                    {
                        for (int i = 0; i < TestTable.Rows.Count; i++)
                        {

                            if (dropTable.Rows[i][ColumnName] != null)
                                TestTable.Rows[i][ColumnName] = dropTable.Rows[i][ColumnName];

                            if ((TestTable.Rows[i][ColumnName].GetType().Name == "Single") || (TestTable.Rows[i][ColumnName].GetType().Name == "Single"))
                            {
                                // double dTemp = Convert.ToDouble(TestTable.Rows[i][ColumnName]);
                                double dTemp = TestTable.Rows[i].Field<Single>(ColumnName);
                                TestTable.Rows[i][ColumnName] = dTemp.ToString("#0.0#");
                            }

                            if (dropTable.Rows[i]["Moisture"] != null)
                                TestTable.Rows[i]["Moisture"] = SetMoistureType(dropTable.Rows[i].Field<Single>("Moisture")).ToString("#0.##");

                            if (MainWindow.AppWindows.bDebug)
                                Console.WriteLine("UpdateNewRealTimeData -- " + TestTable.Rows[i][ColumnName]);
                        }
                    }
                }

                RealTimeDataTable = TestTable;


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in ViewModel UpdateGridView " + ex.Message);
            }

            TestTable = null;

        }

        private void UpdateBigNumbers(DataTable MyTable)
        {

            List<double> MoistureAvgLst = new List<double>();
            List<double> WeightAvgLst = new List<double>();
            List<double> BDWeightAvgLst = new List<double>();
            List<double> NetWeightAvgLst = new List<double>();
            List<double> BasisWeightAvgLst = new List<double>();

            List<double> ADWeightAvgLst = new List<double>();


            MoistureAvg = 0.0;
            WeightAvg = 0.0;
            BDWeightAvg = 0.0;
            ADWeightAvg = 0.0;
            NetWeightAvg = 0.0;
            BasisWeightAvg = 0.0;


            if (MyTable.Rows.Count > 0)
            {
                DropNumber = MyTable.Rows[0]["DropNumber"].ToString();

                for (int i = 0; i < BalesInOneDrop; i++)
                {
                    MoistureAvgLst.Add(MyTable.Rows[i].Field<Single>(ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Sname));
                    WeightAvgLst.Add(MyTable.Rows[i].Field<Single>("Weight"));
                    BDWeightAvgLst.Add(MyTable.Rows[i].Field<Single>("BDWeight"));
                    BasisWeightAvgLst.Add(MyTable.Rows[i].Field<Single>("BasisWeight"));
                    NetWeightAvgLst.Add(MyTable.Rows[i].Field<Single>("NetWeight"));
                    ADWeightAvgLst.Add(MyTable.Rows[i].Field<Single>("BDWeight") / 0.9);

                    /*
                    MoistureAvgLst.Add(Convert.ToDouble(MyTable.Rows[i][ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Sname].ToString()));
                    WeightAvgLst.Add(Convert.ToDouble(MyTable.Rows[i]["Weight"].ToString()));
                    BDWeightAvgLst.Add(Convert.ToDouble(MyTable.Rows[i]["BDWeight"].ToString()));
                    BasisWeightAvgLst.Add(Convert.ToDouble(MyTable.Rows[i]["BasisWeight"].ToString()));
                    NetWeightAvgLst.Add(Convert.ToDouble(MyTable.Rows[i]["NetWeight"].ToString()));
                    ADWeightAvgLst.Add(Convert.ToDouble(MyTable.Rows[i]["BDWeight"])/0.9);
                    */
                }
            }

            MoistureAvg = MoistureAvgLst.Average();
            WeightAvg = WeightAvgLst.Average();
            BDWeightAvg = BDWeightAvgLst.Average();
            NetWeightAvg = NetWeightAvgLst.Average();
            BasisWeightAvg = BasisWeightAvgLst.Average();

            ADWeightAvg = ADWeightAvgLst.Average();


            switch (Settings.Default.iDropProBox1)
            {
                case 0:
                    BigNumBox1 = "Forté";
                    break;
                case 1:
                    BigNumBox1 = MoistureAvg.ToString("#0.00");
                    break;
                case 2:
                    BigNumBox1 = WeightAvg.ToString("#0.00");
                    break;
                case 3:
                    BigNumBox1 = BDWeightAvg.ToString("#0.00");
                    break;
                case 4:
                    BigNumBox1 = NetWeightAvg.ToString("#0.00");
                    break;
                case 5:
                    BigNumBox1 = BasisWeightAvg.ToString("#0.00");
                    break;
                case 6:
                    BigNumBox1 = ADWeightAvg.ToString("#0.00");
                    break;
                case 7:
                    BigNumBox1 = DropNumber;
                    break;
            }

            switch (Settings.Default.iDropProBox2)
            {
                case 0:
                    BigNumBox2 = "Forté";
                    break;
                case 1:
                    BigNumBox2 = MoistureAvg.ToString("#0.00");
                    break;
                case 2:
                    BigNumBox2 = WeightAvg.ToString("#0.00");
                    break;
                case 3:
                    BigNumBox2 = BDWeightAvg.ToString("#0.00");
                    break;
                case 4:
                    BigNumBox2 = NetWeightAvg.ToString("#0.00");
                    break;
                case 5:
                    BigNumBox2 = BasisWeightAvg.ToString("#0.00");
                    break;
                case 6:
                    BigNumBox2 = ADWeightAvg.ToString("#0.00");
                    break;
                case 7:
                    BigNumBox2 = DropNumber;
                    break;
            }

            switch (Settings.Default.iDropProBox3)
            {
                case 0:
                    BigNumBox3 = "Forté";
                    break;
                case 1:
                    BigNumBox3 = MoistureAvg.ToString("#0.00");
                    break;
                case 2:
                    BigNumBox3 = WeightAvg.ToString("#0.00");
                    break;
                case 3:
                    BigNumBox3 = BDWeightAvg.ToString("#0.00");
                    break;
                case 4:
                    BigNumBox3 = NetWeightAvg.ToString("#0.00");
                    break;
                case 5:
                    BigNumBox3 = BasisWeightAvg.ToString("#0.00");
                    break;
                case 6:
                    BigNumBox3 = ADWeightAvg.ToString("#0.00");
                    break;
                case 7:
                    BigNumBox3 = DropNumber;
                    break;
            }

            switch (Settings.Default.iDropProBox4)
            {
                case 0:
                    BigNumBox4 = "Forté";
                    break;
                case 1:
                    BigNumBox4 = MoistureAvg.ToString("#0.00");
                    break;
                case 2:
                    BigNumBox4 = WeightAvg.ToString("#0.00");
                    break;
                case 3:
                    BigNumBox4 = BDWeightAvg.ToString("#0.00");
                    break;
                case 4:
                    BigNumBox4 = NetWeightAvg.ToString("#0.00");
                    break;
                case 5:
                    BigNumBox4 = BasisWeightAvg.ToString("#0.00");
                    break;
                case 6:
                    BigNumBox4 = ADWeightAvg.ToString("#0.00");
                    break;
                case 7:
                    BigNumBox4 = DropNumber;
                    break;
            }
            iBaleCount = 0;
        }


        private void UpdateStatus(string strMsg)
        {
            UpdateInfo = strMsg;
        }

        private string BuildQueryString()
        {
            string strtemp = string.Empty;
            string strList = string.Empty;
            char charsToTrim = ',';

            string strSourceLine = string.Empty;

            if (Hdrtable != null) Hdrtable = null;
            Hdrtable = new DataTable();

            LstHeaderAllFields.Clear();
            LstHeader.Clear();

            m_Line = LineList[SelectLineIndex];
            m_Source = SourceList[SelectSourceIndex];

            try
            {
                Hdrtable = _dropModel.GetSqlScema();
                if (Hdrtable.Rows.Count > 0)
                {
                    int i = 0;
                    foreach (var column in Hdrtable.Rows)
                    {
                        LstHeaderAllFields.Add(Hdrtable.Rows[i]["COLUMN_NAME"].ToString());
                        i += 1;
                    }
                }

                if (SelectedHdrList.Count > 0)
                {
                    foreach (var item in SelectedHdrList)
                        LstHeader.Add(item);
                }
                else
                {
                    LstHeader.Add("LotBaleNumber");
                    LstHeader.Add("LotNumber");
                    LstHeader.Add("Weight");
                    LstHeader.Add("BDWeight");
                    LstHeader.Add("NetWeight");
                    LstHeader.Add("Moisture");
                    LstHeader.Add("Forte");
                    LstHeader.Add("TimeComplete");
                    LstHeader.Add("SerialNumber");
                    LstHeader.Add("StockName");
                    LstHeader.Add("SourceID");
                }

                strList += "[index],";

                if (!LstHeaderAllFields.Contains("LineID"))
                    LstHeaderAllFields.Add("LineID");

                if (!LstHeaderAllFields.Contains("SourceID"))
                    LstHeaderAllFields.Add("SourceID");

                foreach (var item in LstHeaderAllFields)
                {
                    strList += item + ",";
                }

                if ((m_Line == "ALL") & (m_Source == "ALL"))
                {
                    strSourceLine = string.Empty;
                }
                else if ((m_Line != "ALL") & (m_Source != "ALL"))
                {
                    strSourceLine = "WHERE LineId = " + m_Line + " AND  SourceID = " + m_Source;
                }
                else if ((m_Line == "ALL") & (m_Source != "ALL"))
                {
                    strSourceLine = "WHERE SourceID = " + m_Source;
                }
                else if ((m_Line != "ALL") & (m_Source == "ALL"))
                {
                    strSourceLine = "WHERE LineId = " + m_Line;
                }
                strtemp = "SELECT TOP  "
                    + IntColSamples + " "
                    + strList.Trim(charsToTrim)
                    + " FROM " + "dbo.["
                    + _dropModel.CurrentBaleTable + "] "
                    + strSourceLine
                    + " AND Position > 0 ORDER BY [TimeStart] DESC";

                /*
                //Get all fields 
                strtemp = "SELECT TOP  " 
                    + IntColSamples + " " 
                    + strList.Trim(charsToTrim) 
                    + " FROM " + "dbo.[" 
                    + _dropModel.m_CurrentBaleTable
                    + "] WHERE LineID ="
                    + m_Line.ToString()
                    + "AND SourceID = "
                    + m_Source.ToString()
                    + " ORDER BY [UID] DESC";
                    */
                return strtemp;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in ViewModel BuildQueryString " + ex.Message);
            }
            return strtemp;
        }


        private void SetChartColors()
        {
            ChartColors = new ClsChartColor();

            CrtColorList = new List<Brush>();
            CrtColorList = ChartColors.ChartColorList;
            NewDropColor = Brushes.Orange;

            Crt_1Color = CrtColorList[(int)ClsChartColor.Idx.Aquamarine];
            Crt_2Color = CrtColorList[(int)ClsChartColor.Idx.Aquamarine];
            Crt_3Color = CrtColorList[(int)ClsChartColor.Idx.Aquamarine];
            Crt_4Color = CrtColorList[(int)ClsChartColor.Idx.Aquamarine];
            Crt_5Color = CrtColorList[(int)ClsChartColor.Idx.Aquamarine];
            Crt_6Color = CrtColorList[(int)ClsChartColor.Idx.Aquamarine];
            Crt_7Color = CrtColorList[(int)ClsChartColor.Idx.Aquamarine];
            Crt_8Color = CrtColorList[(int)ClsChartColor.Idx.Aquamarine];
            Crt_9Color = CrtColorList[(int)ClsChartColor.Idx.Aquamarine];
            Crt_10Color = CrtColorList[(int)ClsChartColor.Idx.Aquamarine];
            Crt_11Color = CrtColorList[(int)ClsChartColor.Idx.Aquamarine];
            Crt_12Color = CrtColorList[(int)ClsChartColor.Idx.Aquamarine];
        }

        private Double SetMoistureType(double dVal)
        {
            switch (Settings.Default.MoistureUnit)
            {
                case 0: // %MC == moisture from Sql database
                    break;
                case 1: // %MR  = Moisture / ( 1- Moisture / 100)
                    dVal /= 1 - dVal / 100;
                    break;
                case 2: // %AD = (100 - moisture) / 0.9
                    dVal = (100 - dVal) / 0.9;
                    break;
                case 3: // %BD  = 100 - moisture
                    dVal = 100 - dVal;
                    break;
            }
            return dVal;
        }


        #region Timer ////////////////////////////////////////////////////////////////////////

        private System.Windows.Threading.DispatcherTimer dispatcherTimer;

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

            Application.Current.Dispatcher.Invoke(new Action(() => { GetNewData(); }));
            Application.Current.Dispatcher.Invoke(new Action(() => { Heartbeat(); }));

            Thread.Sleep(1000); //Rest for 1 Sec.
            dispatcherTimer.Start();
        }


        private void StartTimer()
        {
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
            dispatcherTimer.Stop();
            Worker_Stopped();

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

        private void Worker_Stopped()
        {
            RTRunning = false;
            _eventAggregator.GetEvent<UpdatedEvent>().Publish(RTRunning);
            //ShowMe = 0.1;
            // Opac = 1.0;

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
        }

        #endregion timer ////////////////////////////////////////////////////////////////////
    }
}
