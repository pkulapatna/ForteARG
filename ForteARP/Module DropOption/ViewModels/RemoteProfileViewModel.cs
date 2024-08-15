

using ForteArg.Services;
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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ForteARP.Module_DropOption.ViewModels
{
    public class RemoteProfileViewModel : BindableBase
    {
        protected readonly Prism.Events.IEventAggregator _eventAggregator;

        private Sqlhandler _sqlhandler = Sqlhandler.Instance;

        private System.Windows.Threading.DispatcherTimer dispatcherTimerL1;
        private System.Windows.Threading.DispatcherTimer dispatcherTimerL2;

        private List<double> MoistureAvgLst;
        private List<double> WeightAvgLst;

        private readonly DropModel DropModel;
        private DataTable DatatableLine1;
        private DataTable DatatableLine2;
        private List<string> HeaderList;
        private List<string> HeaderFieldsList;
        // private List<CheckedListItem> AvailableItemList;
        private DataTable Hdrtable;
        private int IntColSamples = 0;
        // private string strQuerySample;

        private int CurIndexL1 = 0;
        private int PreIndexL1 = 0;

        private int CurIndexL2 = 0;
        private int PreIndexL2 = 0;

        private const int iLine1 = 0;
        private const int iLine2 = 1;

        private string _DropNumber;
        public string DropNumber
        {
            get { return _DropNumber; }
            set { SetProperty(ref _DropNumber, value); }
        }


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


        private List<RemoteProfile> _itemListL1;
        public List<RemoteProfile> ItemListL1
        {
            get { return _itemListL1; }
            set { SetProperty(ref _itemListL1, value); }
        }

        private List<RemoteProfile> _itemListL2;
        public List<RemoteProfile> ItemListL2
        {
            get { return _itemListL2; }
            set { SetProperty(ref _itemListL2, value); }
        }


        private int _Lines;
        public int Lines
        {
            get { return _Lines; }
            set { SetProperty(ref _Lines, value); }
        }

        private bool _rtrunning = false;
        public bool RTRunning
        {
            get { return _rtrunning; }
            set 
            { 
                SetProperty(ref _rtrunning, value);
                if (value) RTIdle = false;
                else RTIdle = true;
            }
        }

        private bool _rtIdle = true;
        public bool RTIdle
        {
            get { return _rtIdle; }
            set { SetProperty(ref _rtIdle, value); }
        }

        private bool _PickTabOne =  true;
        public bool PickTabOne
        {
            get { return _PickTabOne; }
            set { SetProperty(ref _PickTabOne, value); }
        }

        private bool _PickTabTwo;
        public bool PickTabTwo
        {
            get { return _PickTabTwo; }
            set { SetProperty(ref _PickTabTwo, value); }
        }

        private bool _TabTwoEnable;
        public bool TabTwoEnable
        {
            get { return _TabTwoEnable; }
            set { SetProperty(ref _TabTwoEnable, value); }
        }


        private DataTable _RemoteProfileTable;
        public DataTable RemoteProfileTable
        {
            get { return _RemoteProfileTable; }
            set { SetProperty(ref _RemoteProfileTable, value); }
        }

        private DataTable _RemoteProfileTable2;
        public DataTable RemoteProfileTable2
        {
            get { return _RemoteProfileTable2; }
            set { SetProperty(ref _RemoteProfileTable2, value); }
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

        private ObservableCollection<string> _selectedItemList;
        public ObservableCollection<string> SelectedHdrList
        {
            get { return _selectedItemList; }
            set { SetProperty(ref _selectedItemList, value); }
        }

        private ObservableCollection<CheckedListItem> _AvailableHdrList;
        public ObservableCollection<CheckedListItem> AvailableHdrList
        {
            get { return _AvailableHdrList; }
            set { SetProperty(ref _AvailableHdrList, value); }
        }

        private int _GridRowHeight = 64;
        public int GridRowHeight
        {
            get { return _GridRowHeight; }
            set { SetProperty(ref _GridRowHeight, value); }
        }



        #region ListviewColumns


        private double _DGColWidth;
        public double DGColWidth
        {
            get { return _DGColWidth; }
            set { SetProperty(ref _DGColWidth, value); }
        }

        private double _DGColHeight;
        public double DGColHeight
        {
            get { return _DGColHeight; }
            set { SetProperty(ref _DGColHeight, value); }
        }


        private GridLength _Col8Width;// = new GridLength(0, GridUnitType.Pixel);
        public GridLength Col8Width
        {
            get
            {
                if (_Col8Width.IsAuto)
                    _Col8Width = new GridLength();
                return _Col8Width;
            }
            set { SetProperty(ref _Col8Width, value); }
        }
        private GridLength _Col9Width = new GridLength(1, GridUnitType.Star);
        public GridLength Col9Width
        {
            get
            {
                if (_Col9Width.IsAuto)
                    _Col9Width = new GridLength();
                return _Col9Width;
            }
            set { SetProperty(ref _Col9Width, value); }
        }



        #endregion ListviewColumns

        private int _dropsample = 0;
        public int DropSamples
        {
            get { return _dropsample; }
            set
            {
                if ((value > 0) & (value < 12))
                    SetProperty(ref _dropsample, value);
                else
                    SetProperty(ref _dropsample, 12);
            }
        }

        private List<string> _LineList;
        public List<string> LineList
        {
            get { return _LineList; }
            set { SetProperty(ref _LineList, value); }
        }

        private List<string> _SourceList;
        public List<string> SourceList
        {
            get { return _SourceList; }
            set { SetProperty(ref _SourceList, value); }
        }

        private int _BalePositionL1;
        public int BalePositionL1
        {
            get { return _BalePositionL1; }
            set { SetProperty(ref _BalePositionL1, value); }
        }

        private int _BalePositionL2;
        public int BalePositionL2
        {
            get { return _BalePositionL2; }
            set { SetProperty(ref _BalePositionL2, value); }
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


        private string _LVGridWidth;
        public string LVGridWidth
        {
            get { return _LVGridWidth; }
            set { SetProperty(ref _LVGridWidth, value); }
        }

        private string _LVHdrWidth;
        public string LVHdrWidth
        {
            get { return _LVHdrWidth; }
            set { SetProperty(ref _LVHdrWidth, value); }
        }

        private string _Pos1Text;
        public string Pos1Text
        {
            get { return _Pos1Text; }
            set { SetProperty(ref _Pos1Text, value); }
        }

        private string _Pos2Text;
        public string Pos2Text
        {
            get { return _Pos2Text; }
            set { SetProperty(ref _Pos2Text, value); }
        }

        private string _Pos3Text;
        public string Pos3Text
        {
            get { return _Pos3Text; }
            set { SetProperty(ref _Pos3Text, value); }
        }

        private string _Pos4Text;
        public string Pos4Text
        {
            get { return _Pos4Text; }
            set { SetProperty(ref _Pos4Text, value); }
        }

        private string _Pos5Text;
        public string Pos5Text
        {
            get { return _Pos5Text; }
            set { SetProperty(ref _Pos5Text, value); }
        }

        private string _Pos6Text;
        public string Pos6Text
        {
            get { return _Pos6Text; }
            set { SetProperty(ref _Pos6Text, value); }
        }

        private string _Pos7Text;
        public string Pos7Text
        {
            get { return _Pos7Text; }
            set { SetProperty(ref _Pos7Text, value); }
        }

        private string _Pos8Text;
        public string Pos8Text
        {
            get { return _Pos8Text; }
            set { SetProperty(ref _Pos8Text, value); }
        }

        private int _HrdFontSize;
        public int HdrFontSize
        {
            get { return _HrdFontSize; }
            set { SetProperty(ref _HrdFontSize, value); }
        }


        

        /// <summary>
        /// For Big Numbers
        /// </summary>
        private ObservableCollection<string> _avgMoisture;
        public ObservableCollection<string> AvgMoisture
        {
            get { return _avgMoisture; }
            set { SetProperty(ref _avgMoisture, value); }
        }

        private ObservableCollection<string> _AvgWeight;
        public ObservableCollection<string> AvgWeight
        {
            get { return _AvgWeight; }
            set { SetProperty(ref _AvgWeight, value); }
        }

        private System.Windows.Media.Brush _BackgroundColor;
        public System.Windows.Media.Brush BackgroundColor
        {
            get { return _BackgroundColor; }
            set { SetProperty(ref _BackgroundColor, value); }
        }


        private string _updateinfo;
        public string UpdateInfo
        {
            get { return _updateinfo; }
            set { SetProperty(ref _updateinfo, value); }
        }



        public RemoteProfileViewModel(Prism.Events.IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;
            DropModel = new DropModel();
         
            LoadedPageICommand = new DelegateCommand(LoadedPageExecute, LoadedPageCanExecute);
            ClosedPageICommand = new DelegateCommand(ClosedPageExecute, ClosedPageCanExecute);
            StartCommand = new DelegateCommand(StartExecute, StartCanExecute).ObservesProperty(() => RTRunning);
            StopCommand = new DelegateCommand(StopExecute, StopCanExecute).ObservesProperty(() => RTRunning);

            //PopUp Windows
            SetFieldCommand = new DelegateCommand(SetFieldExecute).ObservesCanExecute(() => RTIdle);
            CloseSetUpCommand = new DelegateCommand(CloseSetupExecute, CloseSetupCanExecute).ObservesProperty(() => OpenSetup);
            ModifyCommand = new DelegateCommand(ModifyExecute, ModifyCanExecute);
            SaveUpdateCommand = new DelegateCommand(SaveModExecute, SaveModCanExecute).ObservesProperty(() => BModifySetup).ObservesProperty(() => OpenSetup);
            SelectAllCommand = new DelegateCommand(SelectAllExecute, SelectAllCanExecute).ObservesProperty(() => BModifySetup);
            ClearAllCommand = new DelegateCommand(ClearAllExecute, ClearAllCanExecute).ObservesProperty(() => BModifySetup);
            OnCheckCommand = new DelegateCommand(OnCheckExecute, OnCheckCanExecute).ObservesProperty(() => BModifySetup);

            _eventAggregator.GetEvent<UpdatedEventShutdown>().Subscribe(ProgramShutdown);
        }

        private void ProgramShutdown(bool obj)
        {
            if (obj)
            {
                if (dispatcherTimerL1 != null)
                {
                    dispatcherTimerL1.Stop();
                    dispatcherTimerL1 = null;
                }
                if (dispatcherTimerL2 != null)
                {
                    dispatcherTimerL2.Stop();
                    dispatcherTimerL2 = null;
                }
            }
        }

        private void InitDataGrid()
        {
            BalesInOneDrop = DropModel.BalesinDrop();
            IntColSamples = (BalesInOneDrop * DropSamples); //+ BalesInOneDrop; //Added extra BalesInOneDrop bales,
                                                            //incase count in the middle of 7 
        }


        private bool SelectAllCanExecute()
        {
            return false;
        }

        private void SelectAllExecute()
        {
            //throw new NotImplementedException();
        }

        private bool ClearAllCanExecute()
        {
            return false;
        }

        private void ClearAllExecute()
        {
            //throw new NotImplementedException();
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
                SelectedHdrList = DropModel.RemoveHdrItem(orgList, ItemRemove.ElementAt(0).ToString());
            }
            else //add item
            {
                IEnumerable<string> ItemAdd = NewList.Except(orgList);
                SelectedHdrList = DropModel.AddHdrItem(orgList, ItemAdd.ElementAt(0).ToString());
            }
        }

        private bool SaveModCanExecute()
        {
            return BModifySetup;
        }

        private void SaveModExecute()
        {
            DropModel.SaveXmlcolumnList(SelectedHdrList);
            BModifySetup = false;
            OpenSetup = false; //Close the popup window;
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

        private bool StopCanExecute()
        {
            return RTRunning;
        }

        private void StopExecute()
        {
            Application.Current.Dispatcher.Invoke(new Action(() => { StopTimerL1(); }));
        }

        private bool StartCanExecute()
        {
            return !RTRunning;
        }

        private void StartExecute()
        {
            InitializeTimerL1();
            //InitializeTimerL2();

            StartTimerL1();
           // StartTimerL2();

            RTRunning = true;
            _eventAggregator.GetEvent<UpdatedEvent>().Publish(RTRunning);
        }

        private bool LoadedPageCanExecute()
        {
            return true;
        }

        private void LoadedPageExecute()
        {
            try
            {
                //Set_DropdownList();

                Set_columnbaleInDrop();

                DatatableLine1 = new DataTable();
                DatatableLine2 = new DataTable();

                MoistureAvgLst = new List<double>();
                WeightAvgLst = new List<double>();

                BackgroundColor = System.Windows.Media.Brushes.Red;

                AvgMoisture = new ObservableCollection<string>();

                AvgWeight = new ObservableCollection<string>();


                SelectedHdrList = new ObservableCollection<string>();
                AvailableHdrList = new ObservableCollection<CheckedListItem>();
                HeaderList = new List<string>();
                HeaderFieldsList = new List<string>();

                SourceList = new List<string>();
                LineList = new List<string>();

                RTIdle = true;

                DropSamples = 1; // Settings.Default.iDropProfile;

                DropModel.SetUpSql();
                DropModel.GetLineSource();
                DropModel.InitSqlDropProfileModel();

                LineList = DropModel.m_LineList;
                SourceList = DropModel.m_SourceList;

                //Setup for Big Numbers

              


                if (LineList.Count > 0)
                    for (int i = 0; i < LineList.Count; i++)
                    {
                        AvgMoisture.Add("0.00");
                        AvgWeight.Add("0.00");
                    }

                Set_Popupwindow();

                GetHeaderList();
                Set_ListviewGrid();
                InitDataGrid();

                // Take data when the last bale of the drop arrived!
                if (Settings.Default.bDropHitoLow)
                {
                    Pos1Text = "1";
                    Pos2Text = "2";
                    Pos3Text = "3";
                    Pos4Text = "4";
                    Pos5Text = "5";
                    Pos6Text = "6";
                    Pos7Text = "7";
                    Pos8Text = "8";
                }
                else
                {
                    switch (BalesInOneDrop)
                    {
                        case 3:
                            Pos1Text = "3";
                            Pos2Text = "2";
                            Pos3Text = "1";
                            Pos4Text = "";
                            Pos5Text = "";
                            Pos6Text = "";
                            Pos7Text = "";
                            Pos8Text = "";
                            break;
                        case 4:
                            Pos1Text = "4";
                            Pos2Text = "3";
                            Pos3Text = "2";
                            Pos4Text = "1";
                            Pos5Text = "";
                            Pos6Text = "";
                            Pos7Text = "";
                            Pos8Text = "";
                            break;
                        case 5:
                            Pos1Text = "5";
                            Pos2Text = "4";
                            Pos3Text = "3";
                            Pos4Text = "2";
                            Pos5Text = "1";
                            Pos6Text = "";
                            Pos7Text = "";
                            Pos8Text = "";
                            break;
                        case 6:
                            Pos1Text = "6";
                            Pos2Text = "5";
                            Pos3Text = "4";
                            Pos4Text = "3";
                            Pos5Text = "2";
                            Pos6Text = "1";
                            Pos7Text = "";
                            Pos8Text = "";
                            break;
                        case 7:
                            Pos1Text = "7";
                            Pos2Text = "6";
                            Pos3Text = "5";
                            Pos4Text = "4";
                            Pos5Text = "3";
                            Pos6Text = "2";
                            Pos7Text = "1";
                            Pos8Text = "";
                            break;
                        case 8:
                            Pos1Text = "8";
                            Pos2Text = "7";
                            Pos3Text = "6";
                            Pos4Text = "5";
                            Pos5Text = "4";
                            Pos6Text = "3";
                            Pos7Text = "2";
                            Pos8Text = "1";
                            break;

                    }


                }

                //CreateDynamicGridView();

                GetNewDataLineOne();
                //.GetNewDataLineTwo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in LoadedPageExecute " + ex);
            }
          //  MainWindow.AppWindows.SetupAppTitle("Forté Remote Profile From  -> " + _sqlhandler.Host + @"\" + _sqlhandler.SqlInstance);
        }


        /// <summary>
        /// Setup columns for number of bales in a drop
        /// </summary>
        private void Set_columnbaleInDrop()
        {
            TabTwoEnable = true;

            Col8Width = new GridLength(0, GridUnitType.Pixel);
            Col9Width = new GridLength(0, GridUnitType.Pixel);
        }


        private bool ClosedPageCanExecute()
        {
            return true;
        }

        private void ClosedPageExecute()
        {
            if (dispatcherTimerL1 != null)
            {
                dispatcherTimerL1.Stop();
                dispatcherTimerL1 = null;
            }

            if (dispatcherTimerL2 != null)
            {
                dispatcherTimerL2.Stop();
                dispatcherTimerL2 = null;
            }

            if (AvgMoisture != null) AvgMoisture = null;
            if (AvgWeight != null) AvgWeight = null;

            DatatableLine1 = null;
            DatatableLine2 = null;
            ItemListL1.Clear();
            ItemListL1 = null;
            ItemListL2.Clear();
            ItemListL2 = null;

            if (MoistureAvgLst != null) MoistureAvgLst = null;
            if (WeightAvgLst != null) WeightAvgLst = null;
        }

        private void SetFieldExecute()
        {
            MyPopup_Open();
        }

        private void MyPopup_Open()
        {
            this.OpenSetup = true;
        }

        /// <summary>
        /// Only work for 8 bale drop only.
        /// </summary>
        private void Set_ListviewGrid()
        {
            ItemListL1 = new List<RemoteProfile>();

            for (int i = 0; i < HeaderList.Count * 2 + 2; i++)
            {
                ItemListL1.Add(new RemoteProfile() { RowsName = "", GvCol1 = "", GvCol2 = "", GvCol3 = "", GvCol4 = "", GvCol5 = "", GvCol6 = "", GvCol7 = "", GvCol8 = "" });
            }

            for (int i = 0; i < HeaderList.Count; i++)
            {
                ItemListL1[i].RowsName = HeaderList[i];
            }
            ItemListL1[HeaderList.Count].RowsName = "";

            for (int i = 0; i < HeaderList.Count; i++)
            {
                ItemListL1[HeaderList.Count + 1 + i].RowsName = HeaderList[i];
            }

            //For Line 2
            ItemListL2 = new List<RemoteProfile>();

            for (int i = 0; i < HeaderList.Count * 2 + 2; i++)
            {
                ItemListL2.Add(new RemoteProfile() { RowsName = "", GvCol1 = "", GvCol2 = "", GvCol3 = "", GvCol4 = "", GvCol5 = "", GvCol6 = "", GvCol7 = "", GvCol8 = "" });
            }

            for (int i = 0; i < HeaderList.Count; i++)
            {
                ItemListL2[i].RowsName = HeaderList[i];
            }
            ItemListL2[HeaderList.Count].RowsName = "";

            for (int i = 0; i < HeaderList.Count; i++)
            {
                ItemListL2[HeaderList.Count + 1 + i].RowsName = HeaderList[i];
            }

        }


        /// <summary>
        /// Line 1
        /// </summary>
        private void Update_ListviewLine1()
        {
            MoveL1ListViewDown(ItemListL1, HeaderList.Count);
            _ = UpdateListViewBalebyBaleAsyncL1();
        }
        private async Task UpdateListViewBalebyBaleAsyncL1()
        {
            if (Settings.Default.bDropHitoLow)
            {
                //from left to right
                for (int i = 1; i > BalesInOneDrop; i++)
                {
                    await Task.Delay(300);
                    UpdateListViewL1(i);
                }
            }
            else
            {
                //from right to left
                for (int i = BalesInOneDrop; i > 0; i--)
                {
                    await Task.Delay(300);
                    UpdateListViewL1(i);
                }
            }
        }

        /// <summary>
        /// Line 2
        /// </summary>
        private async Task Update_ListviewLine2Async()
        {
            MoveL1ListViewDown(ItemListL2, HeaderList.Count);
            await UpdateListViewBalebyBaleAsyncL2();

        }
        private async Task UpdateListViewBalebyBaleAsyncL2()
        {
            if (Settings.Default.bDropHitoLow)
            {
                //from left to right
                for (int i = 0; i > BalesInOneDrop; i++)
                {
                    await Task.Delay(300);
                    UpdateListViewL2(i);
                }
            }
            else
            {
                //from right to left
                for (int i = BalesInOneDrop; i > -1; i--)
                {
                    await Task.Delay(300);
                    UpdateListViewL2(i);
                }
            }
        }


        private void UpdateListViewL1(int idx)
        {
           
            string strFormat = "HH:mm:ss";

            if(DatatableLine1  != null)
            {
                switch (idx)
                {
                    case 1:

                        for (int i = 0; i < HeaderList.Count; i++)
                        {
                            if ((DatatableLine1.Rows[0][HeaderList[i]].GetType().Name == "Single") || (DatatableLine1.Rows[0][HeaderList[i]].GetType().Name == "Double"))
                            {
                                double dTemp = Convert.ToDouble(DatatableLine1.Rows[0][HeaderList[i]].ToString());
                                ItemListL1[i].GvCol1 = dTemp.ToString("#0.0#");
                            }
                            else if (DatatableLine1.Rows[0][HeaderList[i]].GetType().FullName == "System.DateTime")
                                ItemListL1[i].GvCol1 = Convert.ToDateTime(DatatableLine1.Rows[0][HeaderList[i]]).ToString(strFormat);
                            else
                                ItemListL1[i].GvCol1 = DatatableLine1.Rows[0][HeaderList[i]].ToString();
                        }
                        break;

                    case 2:
                        for (int i = 0; i < HeaderList.Count; i++)
                        {
                            if ((DatatableLine1.Rows[1][HeaderList[i]].GetType().Name == "Single") || (DatatableLine1.Rows[1][HeaderList[i]].GetType().Name == "Double"))
                            {
                                double dTemp = Convert.ToDouble(DatatableLine1.Rows[1][HeaderList[i]].ToString());
                                ItemListL1[i].GvCol2 = dTemp.ToString("#0.0#");
                            }
                            else if (DatatableLine1.Rows[1][HeaderList[i]].GetType().FullName == "System.DateTime")
                                ItemListL1[i].GvCol2 = Convert.ToDateTime(DatatableLine1.Rows[1][HeaderList[i]]).ToString(strFormat);
                            else
                                ItemListL1[i].GvCol2 = DatatableLine1.Rows[1][HeaderList[i]].ToString();

                        }
                        break;

                    case 3:
                        for (int i = 0; i < HeaderList.Count; i++)
                        {
                            if ((DatatableLine1.Rows[2][HeaderList[i]].GetType().Name == "Single") || (DatatableLine1.Rows[2][HeaderList[i]].GetType().Name == "Double"))
                            {
                                double dTemp = Convert.ToDouble(DatatableLine1.Rows[2][HeaderList[i]].ToString());
                                ItemListL1[i].GvCol3 = dTemp.ToString("#0.0#");
                            }
                            else if (DatatableLine1.Rows[2][HeaderList[i]].GetType().FullName == "System.DateTime")
                                ItemListL1[i].GvCol3 = Convert.ToDateTime(DatatableLine1.Rows[2][HeaderList[i]]).ToString(strFormat);
                            else
                                ItemListL1[i].GvCol3 = DatatableLine1.Rows[2][HeaderList[i]].ToString();
                        }
                        break;

                    case 4:
                        for (int i = 0; i < HeaderList.Count; i++)
                        {
                            if ((DatatableLine1.Rows[3][HeaderList[i]].GetType().Name == "Single") || (DatatableLine1.Rows[3][HeaderList[i]].GetType().Name == "Double"))
                            {
                                double dTemp = Convert.ToDouble(DatatableLine1.Rows[3][HeaderList[i]].ToString());
                                ItemListL1[i].GvCol4 = dTemp.ToString("#0.0#");
                            }
                            else if (DatatableLine1.Rows[3][HeaderList[i]].GetType().FullName == "System.DateTime")
                                ItemListL1[i].GvCol4 = Convert.ToDateTime(DatatableLine1.Rows[3][HeaderList[i]]).ToString(strFormat);
                            else
                                ItemListL1[i].GvCol4 = DatatableLine1.Rows[3][HeaderList[i]].ToString();
                        }
                        break;

                    case 5:
                        for (int i = 0; i < HeaderList.Count; i++)
                        {
                            if ((DatatableLine1.Rows[4][HeaderList[i]].GetType().Name == "Single") || (DatatableLine1.Rows[4][HeaderList[i]].GetType().Name == "Double"))
                            {
                                double dTemp = Convert.ToDouble(DatatableLine1.Rows[4][HeaderList[i]].ToString());
                                ItemListL1[i].GvCol5 = dTemp.ToString("#0.0#");
                            }
                            else if (DatatableLine1.Rows[4][HeaderList[i]].GetType().FullName == "System.DateTime")
                                ItemListL1[i].GvCol5 = Convert.ToDateTime(DatatableLine1.Rows[4][HeaderList[i]]).ToString(strFormat);
                            else
                                ItemListL1[i].GvCol5 = DatatableLine1.Rows[4][HeaderList[i]].ToString();
                        }
                        break;

                    case 6:
                        for (int i = 0; i < HeaderList.Count; i++)
                        {
                            if ((DatatableLine1.Rows[5][HeaderList[i]].GetType().Name == "Single") || (DatatableLine1.Rows[5][HeaderList[i]].GetType().Name == "Double"))
                            {
                                double dTemp = Convert.ToDouble(DatatableLine1.Rows[5][HeaderList[i]].ToString());
                                ItemListL1[i].GvCol6 = dTemp.ToString("#0.0#");
                            }
                            else if (DatatableLine1.Rows[5][HeaderList[i]].GetType().FullName == "System.DateTime")
                                ItemListL1[i].GvCol6 = Convert.ToDateTime(DatatableLine1.Rows[5][HeaderList[i]]).ToString(strFormat);
                            else
                                ItemListL1[i].GvCol6 = DatatableLine1.Rows[5][HeaderList[i]].ToString();
                        }
                        break;

                    case 7:
                        for (int i = 0; i < HeaderList.Count; i++)
                        {
                            if ((DatatableLine1.Rows[6][HeaderList[i]].GetType().Name == "Single") || (DatatableLine1.Rows[6][HeaderList[i]].GetType().Name == "Double"))
                            {
                                double dTemp = Convert.ToDouble(DatatableLine1.Rows[6][HeaderList[i]].ToString());
                                ItemListL1[i].GvCol7 = dTemp.ToString("#0.0#");
                            }
                            else if (DatatableLine1.Rows[6][HeaderList[i]].GetType().FullName == "System.DateTime")
                                ItemListL1[i].GvCol7 = Convert.ToDateTime(DatatableLine1.Rows[6][HeaderList[i]]).ToString(strFormat);
                            else
                                ItemListL1[i].GvCol7 = DatatableLine1.Rows[6][HeaderList[i]].ToString();
                        }
                        break;

                    case 8:
                        for (int i = 0; i < HeaderList.Count; i++)
                        {
                            if ((DatatableLine1.Rows[7][HeaderList[i]].GetType().Name == "Single") || (DatatableLine1.Rows[7][HeaderList[i]].GetType().Name == "Double"))
                            {
                                double dTemp = Convert.ToDouble(DatatableLine1.Rows[7][HeaderList[i]].ToString());
                                ItemListL1[i].GvCol8 = dTemp.ToString("#0.0#");
                            }
                            else if (DatatableLine1.Rows[7][HeaderList[i]].GetType().FullName == "System.DateTime")
                                ItemListL1[i].GvCol8 = Convert.ToDateTime(DatatableLine1.Rows[7][HeaderList[i]]).ToString(strFormat);
                            else
                                ItemListL1[i].GvCol8 = DatatableLine1.Rows[7][HeaderList[i]].ToString();
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Line 2
        /// </summary>
        /// <param name="idx"></param>
        private void UpdateListViewL2(int idx)
        {
            switch (idx)
            {
                case 0:
                    for (int i = 0; i < HeaderList.Count; i++)
                    {
                        if ((DatatableLine2.Rows[0][HeaderList[i]].GetType().Name == "Single") || (DatatableLine2.Rows[0][HeaderList[i]].GetType().Name == "Double"))
                        {
                            double dTemp = Convert.ToDouble(DatatableLine2.Rows[0][HeaderList[i]].ToString());
                            ItemListL2[i].GvCol1 = dTemp.ToString("#0.0#");
                        }
                        else
                            ItemListL2[i].GvCol1 = DatatableLine2.Rows[0][HeaderList[i]].ToString();
                    }
                    break;

                case 1:
                    for (int i = 0; i < HeaderList.Count; i++)
                    {
                        if ((DatatableLine2.Rows[1][HeaderList[i]].GetType().Name == "Single") || (DatatableLine2.Rows[1][HeaderList[i]].GetType().Name == "Double"))
                        {
                            double dTemp = Convert.ToDouble(DatatableLine2.Rows[0][HeaderList[i]].ToString());
                            ItemListL2[i].GvCol2 = dTemp.ToString("#0.0#");
                        }
                        else
                            ItemListL2[i].GvCol2 = DatatableLine2.Rows[1][HeaderList[i]].ToString();
                    }
                    break;

                case 2:
                    for (int i = 0; i < HeaderList.Count; i++)
                    {
                        if ((DatatableLine2.Rows[2][HeaderList[i]].GetType().Name == "Single") || (DatatableLine2.Rows[2][HeaderList[i]].GetType().Name == "Double"))
                        {
                            double dTemp = Convert.ToDouble(DatatableLine2.Rows[2][HeaderList[i]].ToString());
                            ItemListL2[i].GvCol3 = dTemp.ToString("#0.0#");
                        }
                        else
                            ItemListL2[i].GvCol3 = DatatableLine2.Rows[2][HeaderList[i]].ToString();
                    }
                    break;

                case 3:
                    for (int i = 0; i < HeaderList.Count; i++)
                    {
                        if ((DatatableLine2.Rows[3][HeaderList[i]].GetType().Name == "Single") || (DatatableLine2.Rows[3][HeaderList[i]].GetType().Name == "Double"))
                        {
                            double dTemp = Convert.ToDouble(DatatableLine2.Rows[3][HeaderList[i]].ToString());
                            ItemListL2[i].GvCol4 = dTemp.ToString("#0.0#");
                        }
                        else
                            ItemListL2[i].GvCol4 = DatatableLine2.Rows[3][HeaderList[i]].ToString();
                    }
                    break;

                case 4:
                    for (int i = 0; i < HeaderList.Count; i++)
                    {
                        if ((DatatableLine2.Rows[4][HeaderList[i]].GetType().Name == "Single") || (DatatableLine2.Rows[4][HeaderList[i]].GetType().Name == "Double"))
                        {
                            double dTemp = Convert.ToDouble(DatatableLine2.Rows[4][HeaderList[i]].ToString());
                            ItemListL2[i].GvCol5 = dTemp.ToString("#0.0#");
                        }
                        else
                            ItemListL2[i].GvCol5 = DatatableLine2.Rows[4][HeaderList[i]].ToString();
                    }
                    break;

                case 5:
                    for (int i = 0; i < HeaderList.Count; i++)
                    {
                        if ((DatatableLine2.Rows[5][HeaderList[i]].GetType().Name == "Single") || (DatatableLine2.Rows[5][HeaderList[i]].GetType().Name == "Double"))
                        {
                            double dTemp = Convert.ToDouble(DatatableLine2.Rows[5][HeaderList[i]].ToString());
                            ItemListL2[i].GvCol6 = dTemp.ToString("#0.0#");
                        }
                        else
                            ItemListL2[i].GvCol6 = DatatableLine2.Rows[5][HeaderList[i]].ToString();
                    }
                    break;

                case 6:
                    for (int i = 0; i < HeaderList.Count; i++)
                    {
                        if ((DatatableLine2.Rows[6][HeaderList[i]].GetType().Name == "Single") || (DatatableLine2.Rows[6][HeaderList[i]].GetType().Name == "Double"))
                        {
                            double dTemp = Convert.ToDouble(DatatableLine2.Rows[6][HeaderList[i]].ToString());
                            ItemListL2[i].GvCol7 = dTemp.ToString("#0.0#");
                        }
                        else
                            ItemListL2[i].GvCol7 = DatatableLine2.Rows[6][HeaderList[i]].ToString();
                    }
                    break;

                case 7:
                    for (int i = 0; i < HeaderList.Count; i++)
                    {
                        if ((DatatableLine2.Rows[7][HeaderList[i]].GetType().Name == "Single") || (DatatableLine2.Rows[7][HeaderList[i]].GetType().Name == "Double"))
                        {
                            double dTemp = Convert.ToDouble(DatatableLine2.Rows[7][HeaderList[i]].ToString());
                            ItemListL2[i].GvCol8 = dTemp.ToString("#0.0#");
                        }
                        else
                            ItemListL2[i].GvCol8 = DatatableLine2.Rows[7][HeaderList[i]].ToString();
                    }

                    break;
            }
        }

        private void MoveL1ListViewDown(List<RemoteProfile> ItemList, int HdrCount)
        {
            int y = HdrCount + 1;

            try
            {
                for (int i = 0; i < HdrCount; i++)
                {
                    ItemList[y + i].GvCol1 = ItemList[i].GvCol1;
                    ItemList[y + i].GvCol2 = ItemList[i].GvCol2;
                    ItemList[y + i].GvCol3 = ItemList[i].GvCol3;
                    ItemList[y + i].GvCol4 = ItemList[i].GvCol4;
                    ItemList[y + i].GvCol5 = ItemList[i].GvCol5;
                    ItemList[y + i].GvCol6 = ItemList[i].GvCol6;
                    ItemList[y + i].GvCol7 = ItemList[i].GvCol7;
                    ItemList[y + i].GvCol8 = ItemList[i].GvCol8;
                    // Clear Top Rows
                    ItemList[i].GvCol1 = "";
                    ItemList[i].GvCol2 = "";
                    ItemList[i].GvCol3 = "";
                    ItemList[i].GvCol4 = "";
                    ItemList[i].GvCol5 = "";
                    ItemList[i].GvCol6 = "";
                    ItemList[i].GvCol7 = "";
                    ItemList[i].GvCol8 = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in MoveL1ListViewDown " + ex.Message);
            }
        }


        private void Set_Popupwindow()
        {
            //Setup All Listbox
            SelectedHdrList.Clear();
            SelectedHdrList = DropModel.GetXmlSelectedHdrCheckedList();
            AvailableHdrList = DropModel.AvailableItemList; // Checkbox selected ListBox items in popup
        }

        private void GetNewDataLineOne()
        {
            int iFirstBale;
           
            string strGetIndex = "Select Top 1 [index] FROM dbo.[" +
                                 DropModel.CurrentBaleTable +
                                 "] WHERE LineId=1 ORDER BY [TimeStart] DESC; ";

            string strGetSingleNewData = "Select Top 1 " +
                                           " Position, Weight, Forte, Moisture, UpCount, DownCount, DropNumber, NetWeight, BDWeight, BasisWeight, LotBaleNumber,SerialNumber,DropNumber,[Index] FROM dbo.[" +
                                            DropModel.CurrentBaleTable +
                                            "] WHERE LineId=1 And Position > 0 ORDER BY [TimeStart] DESC;";
            try
            {
               
                DataTable SingleDataTable = DropModel.GetNewDataTable(strGetSingleNewData);
                BalePositionL1 = Convert.ToInt32(SingleDataTable.Rows[0]["Position"].ToString());

                if (Settings.Default.bDropHitoLow)
                    iFirstBale = 1;
                else
                    iFirstBale = BalesInOneDrop;

                if (BalePositionL1 == iFirstBale)
                {
                    DatatableLine1.Clear();
                    CurIndexL1 = DropModel.GetNewItemData(strGetIndex);

                    if (CurIndexL1 != PreIndexL1) // not for the same bale!
                    {
                        string newquery = BuildQueryString(1);
                        DatatableLine1 = DropModel.GetNewDataTable(newquery);

                        if (DatatableLine1.Columns.Contains("index"))
                            DatatableLine1.Columns.Remove("index");

                        PreIndexL1 = CurIndexL1;
                        Update_ListviewLine1();
                        //UpdateBigNumbersL1(DatatableLine1);
                        UpdateBigNumbers(DatatableLine1, iLine1);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in GetNewDataLineOne" + ex.Message);
            }
        }


        private void GetNewDataLineTwo()
        {
            int iFirstBale;
          
            DropModel.m_Line = "2";

            string strGetIndex = "Select Top 1 [index] FROM dbo.[" +
                                       DropModel.CurrentBaleTable +
                                       "] WHERE LineId=2 ORDER BY [TimeStart] DESC; ";

            string strGetSingleNewData = "Select Top 1 " +
                               " Position, Weight, Forte, Moisture, UpCount, DownCount, DropNumber, NetWeight, BDWeight, BasisWeight, LotBaleNumber,SerialNumber,DropNumber,[Index] FROM dbo.[" +
                                DropModel.CurrentBaleTable +
                                "] WHERE LineId = 2 " +
                                " And Position > 0 ORDER BY [TimeStart] DESC;";

            try
            {
                DataTable SingleDataTable = DropModel.GetNewDataTable(strGetSingleNewData);
                BalePositionL2 = Convert.ToInt32(SingleDataTable.Rows[0]["Position"].ToString());


                if (Settings.Default.bDropHitoLow)
                    iFirstBale = 1;
                else
                    iFirstBale = BalesInOneDrop;

                if (BalePositionL2 == iFirstBale)
                {
                    DatatableLine2.Clear();
                    CurIndexL2 = DropModel.GetNewItemData(strGetIndex);

                    if (CurIndexL2 != PreIndexL2) // not for the same bale!
                    {
                        string newquery = BuildQueryString(2);
                        DatatableLine2 = DropModel.GetNewDataTable(newquery);

                        if (DatatableLine2.Columns.Contains("index"))
                            DatatableLine2.Columns.Remove("index");

                        PreIndexL2 = CurIndexL2;
                        _ = Update_ListviewLine2Async();
                        UpdateBigNumbers(DatatableLine2, iLine2);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in GetNewDataLineTwo" + ex.Message);
            }
        }


        private void GetHeaderList()
        {
            if (Hdrtable != null) Hdrtable = null;
            Hdrtable = new DataTable();

            HeaderFieldsList.Clear();
            HeaderList.Clear();

            try
            {

                Hdrtable = DropModel.GetSqlScema();
                if (Hdrtable.Rows.Count > 0)
                {
                    int i = 0;
                    foreach (var column in Hdrtable.Rows)
                    {
                        HeaderFieldsList.Add(Hdrtable.Rows[i]["COLUMN_NAME"].ToString());
                        i += 1;
                    }
                }

                if (SelectedHdrList.Count > 0)
                {
                    foreach (var item in SelectedHdrList)
                        HeaderList.Add(item);
                }
                else
                {
                    HeaderList.Add("LotBaleNumber");
                    HeaderList.Add("LotNumber");
                    HeaderList.Add("Weight");
                    HeaderList.Add("Moisture");
                    HeaderList.Add("Forte");
                    HeaderList.Add("TimeComplete");
                    HeaderList.Add("SerialNumber");
                    HeaderList.Add("StockName");
                    HeaderList.Add("SourceID");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR In GetHeaderList " + ex);
            }
        }

        private string BuildQueryString(int LineID)
        {
            string strtemp = string.Empty;
            string strList = string.Empty;
            char charsToTrim = ',';
            string strLineSeleted = "WHERE LineId = " + LineID.ToString(); //_dropModel.m_Line;

            try
            {

                GetHeaderList();

                strList += "[index],";
                foreach (var item in HeaderFieldsList)
                {
                    strList += item + ",";
                }
                //Get all fields 
                strtemp = "SELECT TOP  "
                    + IntColSamples + " "
                    + strList.Trim(charsToTrim)
                    + " FROM " + "dbo.["
                    + DropModel.CurrentBaleTable
                    + "] "
                    + strLineSeleted
                    + " ORDER BY [TimeStart] DESC";

                return strtemp;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in ViewModel BuildQueryString " + ex.Message);
            }
            return strtemp;
        }

        private void UpdateBigNumbers(DataTable MyDataTable, int iLine)
        {

            double MoistureAvg;
            double WeightAvg;

            MoistureAvgLst.Clear();
            WeightAvgLst.Clear();

            if (MyDataTable.Rows.Count > 0)
            {
                for (int i = 0; i < BalesInOneDrop; i++)
                {
                    MoistureAvgLst.Add(Convert.ToDouble(MyDataTable.Rows[i]["Moisture"].ToString()));
                    WeightAvgLst.Add(Convert.ToDouble(MyDataTable.Rows[i]["Weight"].ToString()));

                    DropNumber = MyDataTable.Rows[i]["DropNumber"].ToString();
                }

                MoistureAvg = MoistureAvgLst.Average();
                WeightAvg = WeightAvgLst.Average();
                AvgMoisture[iLine] = MoistureAvg.ToString("#0.00");
                AvgWeight[iLine] = WeightAvg.ToString("#0.00");

              

            }
            //Console.WriteLine(" xxxxxxxxxxxxxxxxxxxxxxxx Reset Line = " + iLine);
        }


        #region DispatcherTimer /////////////////////////////////////////////////////////////////////////////////////

        internal int BalesInOneDrop { get; set; }

        private void InitializeTimerL1()
        {
            if (dispatcherTimerL1 != null) dispatcherTimerL1 = null;
            dispatcherTimerL1 = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(Settings.Default.iScanRate)
            };
            dispatcherTimerL1.Tick += new EventHandler(DispatcherTimer_Tick);
        }
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            dispatcherTimerL1.Stop();
            //Application.Current.Dispatcher.Invoke(new Action(() => { GetNewData(); }));
            Application.Current.Dispatcher.Invoke(new Action(() => { GetNewDataLineOne(); }));

            Thread.Sleep(800); //Rest for 1/2 Sec.
            dispatcherTimerL1.Start();
        }

        private void StartTimerL1()
        {
            dispatcherTimerL1.Start();
            UpdateInfo = "Status : Scan timer Start";

        }
        private void StopTimerL1()
        {
            dispatcherTimerL1.Stop();
            RTRunning = false;
            UpdateInfo = "Status : Scan timer Stop";
            _eventAggregator.GetEvent<UpdatedEvent>().Publish(RTRunning);
            //ShowMe = 0.1;
            // Opac = 1.0;
        }

        #endregion DispatcherTimer /////////////////////////////////////////////////////////////////////////////////////////


      

    }
}
