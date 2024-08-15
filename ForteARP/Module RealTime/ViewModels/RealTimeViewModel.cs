

using ForteArg.Services;
using ForteARP.Model;
using ForteARP.Module_RealTime.Model;
using ForteARP.Module_RealTime.Views;
using ForteARP.Properties;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace ForteARP.Module_RealTime.ViewModels
{
    public class RealTimeViewModel : BindableBase
    {
        protected readonly Prism.Events.IEventAggregator _eventAggregator;

        private Sqlhandler _sqlhandler = Sqlhandler.Instance;

        public RealTimeModel CRealtimeModel;

        private long preIndex = 0;
        private long newIndex = 0;

   
        //Main WIndow
        public DelegateCommand StartCommand { get; set; }       //Start Button
        public DelegateCommand StopCommand { get; set; }        //Stop Button
        //Popup Window
        public DelegateCommand SetFieldCommand { get; set; }    //Fields Button
        public DelegateCommand CloseSetUpCommand { get; set; }  //Close Button
        public DelegateCommand SaveUpdateCommand { get; set; }  //Save Button
        public DelegateCommand ModifyCommand { get; set; }      //Modify Button
        public DelegateCommand ClearAllCommand { get; set; }    //Clear Button
        public DelegateCommand SelectAllCommand { get; set; }   //Select all Button      
        public DelegateCommand OnCheckCommand { get; set; }     //Check Checkbox item in listbox 
        public DelegateCommand SelectedCommand { get; set; }
        public DelegateCommand LoadedPageICommand { get; set; } //Load Page
        public DelegateCommand ClosedPageICommand { get; set; } //Close page
        public DelegateCommand MoveLeftCommand { get; set; }
        public DelegateCommand MoveRightCommand { get; set; }
        //Combo1
        public DelegateCommand Box1ComboCommand { get; set; }
        //Combo2
        public DelegateCommand Box2ComboCommand { get; set; }
        //Combo3
        public DelegateCommand Box3ComboCommand { get; set; }
        //Combo4
        public DelegateCommand Box4ComboCommand { get; set; }
        //Combo5
        public DelegateCommand Box5ComboCommand { get; set; }
        //Combo6
        public DelegateCommand Box6ComboCommand { get; set; }
        //Combo 7
        public DelegateCommand Box7ComboCommand { get; set; }

        private ObservableCollection<string> _selectedItemList;
        public ObservableCollection<string> SelectedHdrList
        {
            get => _selectedItemList;
            set => SetProperty(ref _selectedItemList, value);
        }

        public List<string> _cmbDropDownList;
        public List<string> CmbDropDownList
        {
            get => _cmbDropDownList;
            set => SetProperty(ref _cmbDropDownList, value);
        }

        /// <summary>
        /// $$Mod1
        /// </summary>
        public List<string> _cmbItemsList;
        public List<string> CmbItemsList
        {
            get => _cmbItemsList;
            set => SetProperty(ref _cmbItemsList, value);
        }


        private ObservableCollection<CheckedListItem> _hdrListboxList;
        public ObservableCollection<CheckedListItem> AvailableHdrList
        {
            get => _hdrListboxList;
            set => SetProperty(ref _hdrListboxList, value);
        }

        //ScaleWeight
        private string _ScaleWeight;
        public string ScaleWeight
        {
            get => _ScaleWeight;
            set => SetProperty(ref _ScaleWeight, value);
        }

        //ForteNumber
        private string _ForteNumber;
        public string ForteNumber
        {
            get => _ForteNumber;
            set => SetProperty(ref _ForteNumber, value);
        }


        //CurMoisture
        private string _CurMoisture;
        public string CurMoisture
        {
            get => _CurMoisture;
            set => SetProperty(ref _CurMoisture, value);
        }
        private string _MoistureUnit;
        public string MoistureUnit
        {
            get => _MoistureUnit;
            set => SetProperty(ref _MoistureUnit, value);
        }

        private string _WeightUnit;
        public string WeightUnit
        {
            get => _WeightUnit;
            set => SetProperty(ref _WeightUnit, value);
        }

        private string _LineName;
        public string LineName
        {
            get => _LineName;
            set => SetProperty(ref _LineName, value);
        }

        private bool _hdrenable;
        public bool HdrEnable
        {
            get { return _hdrenable; }
            set { _hdrenable = value; }
        }

        private DataTable _hdrtable;
        public DataTable Hdrtable
        {
            get { return _hdrtable; }
            set { SetProperty(ref _hdrtable, value); }
        }

        private bool _rtIdle = true;
        public bool RTIdle
        {
            get { return _rtIdle; }
            set { SetProperty(ref _rtIdle, value); }
        }

        private bool _rtrunning = false;
        public bool RTRunning
        {
            get { return _rtrunning; }
            set { SetProperty(ref _rtrunning, value); }
        }

        private bool _bSetField = false;
        public bool BSetfield
        {
            get { return _bSetField; }
            set { SetProperty(ref _bSetField, value); }
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
            set { SetProperty(ref _selectedBox1Combo, value); }
        }

        private int _selectedBox2Combo;
        public int SelectedBox2Combo
        {
            get { return _selectedBox2Combo; }
            set { SetProperty(ref _selectedBox2Combo, value); }
        }

        private int _selectedBox3Combo;
        public int SelectedBox3Combo
        {
            get { return _selectedBox3Combo; }
            set { SetProperty(ref _selectedBox3Combo, value); }
        }

        private int _selectedBox4Combo;
        public int SelectedBox4Combo
        {
            get { return _selectedBox4Combo; }
            set { SetProperty(ref _selectedBox4Combo, value); }
        }

        private int _selectedBox5Combo;
        public int SelectedBox5Combo
        {
            get { return _selectedBox5Combo; }
            set { SetProperty(ref _selectedBox5Combo, value); }
        }

        private int _selectedBox6Combo;
        public int SelectedBox6Combo
        {
            get { return _selectedBox6Combo; }
            set { SetProperty(ref _selectedBox6Combo, value); }
        }

        private int _selectedBox7Combo;
        public int SelectedBox7Combo
        {
            get { return _selectedBox7Combo; }
            set { SetProperty(ref _selectedBox7Combo, value); }
        }

        //sql realtime datatable bind to datagridview
        private DataTable _realtimedatatable;
        public DataTable RealTimeDataTable
        {
            get { return _realtimedatatable; }
            set { SetProperty(ref _realtimedatatable, value); }
        }

        //Update status on screen
        private string _strStatus;
        public string StringStatus
        {
            get { return _strStatus; }
            set { SetProperty(ref _strStatus, value); }
        }

        //Scan duration
        private long _lscantimer;
        public long LScanTimer
        {
            get { return _lscantimer; }
            set { SetProperty(ref _lscantimer, value); }
        }

        private string _strScanDuration;
        public string StrScanDuration
        {
            get { return _strScanDuration; }
            set { SetProperty(ref _strScanDuration, value); }
        }

        private string[] _combobox;
        public string[] BigComboBox
        {
            get { return _combobox; }
            set { SetProperty(ref _combobox, value); }
        }

        private int IScanTimer { get; set; }

        public double _showme = 0.1;
        public double ShowMe
        {
            get { return _showme; }
            set { SetProperty(ref _showme, value); }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { SetProperty(ref _selectedIndex, value); }
        }

        private string _selecteditem;
        public string SelectedHrdItem
        {
            get { return _selecteditem; }
            set { SetProperty(ref _selecteditem, value); }
        }

        private bool _AllFieldCheck;
        public bool AllFieldCheck
        {
            get { return _AllFieldCheck; }
            set { SetProperty(ref _AllFieldCheck, value); }
        }
        private bool _CustFieldCheck = true;
        public bool CustFieldCheck
        {
            get { return _CustFieldCheck; }
            set { SetProperty(ref _CustFieldCheck, value); }
        }

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
                if (value > -1)
                {
                    SetProperty(ref _SelectSourceIndex, value);
                    m_Source = SourceList[value];
                    Settings.Default.sRealTimeSource = m_Source;
                    Settings.Default.iRealTimeSourceIdx = value;
                    Settings.Default.Save();
                }
            }
        }

        private string m_Line;
        /// <summary>
        /// Line ---------------------------------------------------------------------------
        /// </summary>
        /// 
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

                if (value > -1)
                {
                    SetProperty(ref _SelectLineIndex, value);
                    m_Line = LineList[value];
                    Settings.Default.sRealTimeLine = m_Line;
                    Settings.Default.iRealTimeLineidx = value;
                    Settings.Default.Save();
                }
            }
        }
        //-----------------------------------------------------------------------------------


        /// <summary>
        /// Set Max listview to 300 default 20 
        /// </summary>
        private int _bsample = Settings.Default.iRealTimeBale;
        public int BSamples
        {
            get { return _bsample; }
            set
            {
                if ((value > 0) & (value < 301))
                    SetProperty(ref _bsample, value);
                else
                    SetProperty(ref _bsample, 20);
            }
        }

        public RealTimeViewModel(Prism.Events.IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;

            //Main Window
            LoadedPageICommand = new DelegateCommand(LoadPageExecute, LoadPageCanExecute);
            ClosedPageICommand = new DelegateCommand(ClosedPageExecute, ClosedPageCanExecute);

            StartCommand = new DelegateCommand(StartExecute, StartCanExecute).ObservesProperty(() => RTRunning);
            StopCommand = new DelegateCommand(StopExecute, StopCanExecute).ObservesProperty(() => RTRunning);

            _eventAggregator.GetEvent<UpdatedEventShutdown>().Subscribe(ProgramShutdown);

            // _eventAggregator.GetEvent<UpdatedSqlTableEvent>().Subscribe(NewSqlDataEvent);

            //Popup window CustFieldCheck
            SetFieldCommand = new DelegateCommand(SetFieldExecute, SetFieldCanExecute).ObservesProperty(() => RTRunning).ObservesProperty(() => CustFieldCheck);
            CloseSetUpCommand = new DelegateCommand(CloseSetupExecute, CloseSetupCanExecute).ObservesProperty(() => OpenSetup);
            ModifyCommand = new DelegateCommand(ModifyExecute, ModifyCanExecute);
            SaveUpdateCommand = new DelegateCommand(SaveModExecute, SaveModCanExecute).ObservesProperty(() => BModifySetup).ObservesProperty(() => OpenSetup);
            SelectAllCommand = new DelegateCommand(SelectAllExecute, SelectAllCanExecute).ObservesProperty(() => BModifySetup);
            ClearAllCommand = new DelegateCommand(ClearAllExecute, ClearAllCanExecute).ObservesProperty(() => BModifySetup);
            OnCheckCommand = new DelegateCommand(OnCheckExecute, OnCheckCanExecute).ObservesProperty(() => BModifySetup);
            MoveLeftCommand = new DelegateCommand(MoveLeftExecute, MoveLeftCanExecute).ObservesProperty(() => BModifySetup);
            MoveRightCommand = new DelegateCommand(MoveRightExecute, MoveRightCanExecute).ObservesProperty(() => BModifySetup);

            SelectedCommand = new DelegateCommand(SelectedExecute, SelectedCanExecute);

            //Comboboxes
            Box1ComboCommand = new DelegateCommand(Box1Execute, Box1CanExecute).ObservesProperty(() => SelectedBox1Combo).ObservesProperty(() => RTRunning);
            Box2ComboCommand = new DelegateCommand(Box2Execute, Box2CanExecute).ObservesProperty(() => SelectedBox2Combo).ObservesProperty(() => RTRunning);
            Box3ComboCommand = new DelegateCommand(Box3Execute, Box3CanExecute).ObservesProperty(() => SelectedBox3Combo).ObservesProperty(() => RTRunning);
            Box4ComboCommand = new DelegateCommand(Box4Execute, Box4CanExecute).ObservesProperty(() => SelectedBox4Combo).ObservesProperty(() => RTRunning);
            Box5ComboCommand = new DelegateCommand(Box5Execute, Box5CanExecute).ObservesProperty(() => SelectedBox5Combo).ObservesProperty(() => RTRunning);
            Box6ComboCommand = new DelegateCommand(Box6Execute, Box6CanExecute).ObservesProperty(() => SelectedBox6Combo).ObservesProperty(() => RTRunning);
            Box7ComboCommand = new DelegateCommand(Box7Execute, Box7CanExecute).ObservesProperty(() => SelectedBox7Combo).ObservesProperty(() => RTRunning);

            ClsSerilog.LogMessage(ClsSerilog.Info, $"Initialize Bale RealTime");

           // string x = (string)Application.Current.FindResource("lbBaleSample");
        }


        private void ProgramShutdown(bool obj)
        {
            if (obj)
            {
                if (dispatcherTimer != null)
                {
                    dispatcherTimer?.Stop();
                    dispatcherTimer = null;
                }
                if(HeartBeatTimer != null)
                {
                    HeartBeatTimer?.Stop();
                    HeartBeatTimer = null;
                }
            }
               
            
        }

        private bool ClosedPageCanExecute()
        {
            return true;
        }

        private void ClosedPageExecute()
        {
            if (dispatcherTimer != null)
            {
                dispatcherTimer?.Stop();
                dispatcherTimer = null;
            }
            if (HeartBeatTimer != null)
            {
                HeartBeatTimer?.Stop();
                HeartBeatTimer = null;
            }
        }

        private bool SelectedCanExecute()
        {
            return false;
        }

        private void SelectedExecute()
        {
            //throw new NotImplementedException();
        }


        /// <summary>
        /// Move selected column to Right
        /// </summary>
        /// <returns></returns>
        private bool MoveRightCanExecute()
        {
            return BModifySetup;
        }
        private void MoveRightExecute()
        {
            //
        }

        /// <summary>
        /// move selccted column to Left
        /// </summary>
        /// <returns></returns>
        private bool MoveLeftCanExecute()
        {
            return BModifySetup;
        }
        private void MoveLeftExecute()
        {

        }


        private bool LoadPageCanExecute()
        {
            return true;
        }

        private void LoadPageExecute()
        {
            if (CRealtimeModel != null) CRealtimeModel = null;
            CRealtimeModel = new RealTimeModel();

            CRealtimeModel.SetupWorkStation();

            _sqlhandler.SetSqlParams();
            _sqlhandler.SetConnectionString();
            _sqlhandler.SetupWorkStation();
            
            try
            {
                SelectedHdrList = new ObservableCollection<string>();
                SourceList = new List<string>();

                Hdrtable = CRealtimeModel.HdrTable;

                Setup_DropDownLists();
                Set_Popupwindow();

                SourceList = CRealtimeModel.Getsourcelist();
                if (SourceList.Count > 0)
                    if (Settings.Default.iRealTimeSourceIdx >= SourceList.Count)
                        SelectSourceIndex = 0;
                    else
                        SelectSourceIndex = Settings.Default.iRealTimeSourceIdx;

                LineList = CRealtimeModel.Getlinelist();
                if (LineList.Count > 0)
                    if (Settings.Default.iRealTimeLineidx >= LineList.Count)
                        SelectLineIndex = 0;
                    else
                        SelectLineIndex = Settings.Default.iRealTimeLineidx;

             
                if (Settings.Default.iScanRate.ToString() == null)
                {
                    Settings.Default.iScanRate = 5;
                    Settings.Default.Save();
                }

               

                HdrEnable = false;
                ShowMe = 0.1;

                MoistureUnit = ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Name;
                WeightUnit = ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name;

              
                switch (Settings.Default.iLanguageIdx)
                {
                    case 0: // "en-US":
                        StringStatus = "Status: BaleRealTime";
                        StrScanDuration = "Scan Duration: " + Settings.Default.iScanRate.ToString() + " Seconds.";
                        MainWindow.AppWindows.SetupAppTitle("Forté RealTime Data From  -> " + _sqlhandler.Host + @"\" + _sqlhandler.SqlInstance);
                        break;
                    case 1: //"Sp-SP":
                        StringStatus = "Estado: Tiempo real de Fardo";
                        StrScanDuration = "Duración del escaneo: " + Settings.Default.iScanRate.ToString() + " Segundos.";
                        MainWindow.AppWindows.SetupAppTitle("Forté TiempoReal desde  -> " + _sqlhandler.Host + @"\" + _sqlhandler.SqlInstance);
                        break;
                    default:
                        StringStatus = "Status: BaleRealTime";
                        StrScanDuration = "Scan Duration: " + Settings.Default.iScanRate.ToString() + " Seconds.";
                        MainWindow.AppWindows.SetupAppTitle("Forté RealTime Data From  -> " + _sqlhandler.Host + @"\" + _sqlhandler.SqlInstance);
                        break;
                }

                //UpdateNewRealTimeData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in LoadPageExecute " + ex.Message);
            }
        }

        private void Setup_DropDownLists()
        {
           
            CmbDropDownList = CRealtimeModel.GetHdrList();
            if(CmbDropDownList.Count < 1) CmbDropDownList.Add("Forte");

            SelectedBox1Combo = Settings.Default.iRTBox1;
            SelectedBox2Combo = Settings.Default.iRTBox2;
            SelectedBox3Combo = Settings.Default.iRTBox3;
            SelectedBox4Combo = Settings.Default.iRTBox4;
            SelectedBox5Combo = Settings.Default.iRTBox5;
            SelectedBox6Combo = Settings.Default.iRTBox6;
            SelectedBox7Combo = Settings.Default.iRTBox7;

          //  CmbDropDownList = new List<string>(CmbDropDownList.OrderBy(x => x));
        }

        private bool OnCheckCanExecute()
        {
            return BModifySetup;
        }

        private void OnCheckExecute()
        {
            //  SelectedHdrList = CRealtimeModel.UpdateSelectedItem();

            ObservableCollection<string> NewList = new ObservableCollection<string>();
            ObservableCollection<string> orgList = SelectedHdrList;

            for (int i = 0; i < AvailableHdrList.Count; i++)
            {
                if (AvailableHdrList[i].IsChecked == true) NewList.Add(AvailableHdrList[i].Name);
            }

            if (orgList.Count > NewList.Count) //Remove item
            {
                IEnumerable<string> ItemRemove = orgList.Except(NewList);
                SelectedHdrList = CRealtimeModel.RemoveHdrItem(orgList, ItemRemove.ElementAt(0).ToString());
            }
            else //add item
            {
                IEnumerable<string> ItemAdd = NewList.Except(orgList);
                SelectedHdrList = CRealtimeModel.AddHdrItem(orgList, ItemAdd.ElementAt(0).ToString());
            }
        }

        private bool Box1CanExecute()
        {
            return !RTRunning;
        }
        private void Box1Execute()
        {
            Settings.Default.iRTBox1 = _selectedBox1Combo;
            Settings.Default.Save();
            if (MainWindow.AppWindows.bDebug)
                Console.WriteLine("Combo1 Select = " + CmbDropDownList[_selectedBox1Combo].ToString());

        }

        private bool Box2CanExecute()
        {
            return !RTRunning;
        }
        private void Box2Execute()
        {
            Settings.Default.iRTBox2 = SelectedBox2Combo;
            Settings.Default.Save();
            if (MainWindow.AppWindows.bDebug)
                Console.WriteLine("Combo2 Select = " + CmbDropDownList[SelectedBox2Combo].ToString());

        }

        private bool Box3CanExecute()
        {
            return !RTRunning;
        }
        private void Box3Execute()
        {
            Settings.Default.iRTBox3 = SelectedBox3Combo;
            Settings.Default.Save();

            if (MainWindow.AppWindows.bDebug)
                Console.WriteLine("Combo3 Select = " + CmbDropDownList[SelectedBox3Combo].ToString());

        }

        private bool Box4CanExecute()
        {
            return !RTRunning;
        }
        private void Box4Execute()
        {
            Settings.Default.iRTBox4 = SelectedBox4Combo;
            Settings.Default.Save();
            if (MainWindow.AppWindows.bDebug)
                Console.WriteLine("Combo4 Select = " + CmbDropDownList[SelectedBox4Combo].ToString());
        }

        private bool Box5CanExecute()
        {
            return !RTRunning;
        }
        private void Box5Execute()
        {
            Settings.Default.iRTBox5 = SelectedBox5Combo;
            Settings.Default.Save();
            if (MainWindow.AppWindows.bDebug)
                Console.WriteLine("Combo5 Select = " + CmbDropDownList[SelectedBox5Combo].ToString());
        }

        private bool Box6CanExecute()
        {
            return !RTRunning;
        }
        private void Box6Execute()
        {
            Settings.Default.iRTBox6 = SelectedBox6Combo;
            Settings.Default.Save();
            if (MainWindow.AppWindows.bDebug)
                Console.WriteLine("Combo6 Select = " + CmbDropDownList[SelectedBox6Combo].ToString());
        }

        private bool Box7CanExecute()
        {
            return !RTRunning;
        }
        private void Box7Execute()
        {
            Settings.Default.iRTBox7 = SelectedBox7Combo;
            Settings.Default.Save();
            if (MainWindow.AppWindows.bDebug)
                Console.WriteLine("Combo7 Select = " + CmbDropDownList[SelectedBox7Combo].ToString());
        }


        #region Main Window

        /// <summary>
        /// Stop Button
        /// </summary>
        /// <returns></returns>
        private bool StopCanExecute()
        {
            return RTRunning;
        }
        private void StopExecute()
        {
            
            Application.Current.Dispatcher.Invoke(new Action(() => { StopTimer(); }));
        
            StrScanDuration = "AUTO UPDATE STOP! ";

            RTRunning = false;
            RTIdle = true;
            _eventAggregator.GetEvent<UpdatedEvent>().Publish(RTRunning);
        }


        /// <summary>
        /// Start Button
        /// </summary>
        /// <returns></returns>
        private bool StartCanExecute()
        {
            return !RTRunning;
        }
        private void StartExecute()
        {
            IScanTimer = Settings.Default.iScanRate;
            Settings.Default.iRealTimeBale = BSamples;
            Settings.Default.Save();

            this.InitializeTimer();
            this.StartTimer();

            InitializeHeartBeatTimer();

            switch (Settings.Default.iLanguageIdx)
            {
                case 0: // "en-US":
                    StrScanDuration = "Scan Duration: " + Settings.Default.iScanRate.ToString() + " Seconds.";
                    break;
                case 1: //"Sp-SP":
                    StrScanDuration = "Duración del escaneo: " + Settings.Default.iScanRate.ToString() + " Segundos.";
                    break;
                default:
                    StrScanDuration = "Scan Duration: " + Settings.Default.iScanRate.ToString() + " Seconds.";
                    break;
            }
            

            RTRunning = true;
            RTIdle = false;
            ShowMe = 0.1;
            _eventAggregator.GetEvent<UpdatedEvent>().Publish(RTRunning);

        }

        #endregion


        #region POPUP Window

        /// <summary>
        /// Clear all fields
        /// </summary>
        /// <returns></returns>
        private bool ClearAllCanExecute()
        {
            return BModifySetup;
        }
        private void ClearAllExecute()
        {
            //throw new NotImplementedException();
        }


        /// <summary>
        /// Select all fields
        /// </summary>
        /// <returns></returns>
        private bool SelectAllCanExecute()
        {
            return BModifySetup;
        }

        private void SelectAllExecute()
        {
            // throw new NotImplementedException();
        }


        /// <summary>
        /// Save Button
        /// </summary>
        private bool SaveModCanExecute()
        {
            return BModifySetup;
        }
        private void SaveModExecute()
        {
            CRealtimeModel.SaveModified_setting();
            CRealtimeModel.SaveXmlcolumnList(SelectedHdrList);
            BModifySetup = false;
            OpenSetup = false; //Close the popup window
        }


        /// <summary>
        /// Modify button
        /// </summary>
        private bool ModifyCanExecute()
        {
            return true;
        }
        private void ModifyExecute()
        {
            BModifySetup = true;
            //HdrEnable = true;
        }

        /// <summary>
        /// Cancel button
        /// </summary>
        /// <returns></returns>
        private bool CloseSetupCanExecute()
        {
            return OpenSetup;
        }
        private void CloseSetupExecute()
        {
            BModifySetup = false;
            OpenSetup = false;
            if (dispatcherTimer != null) dispatcherTimer = null;
        }


        /// <summary>
        /// Fileds button 
        /// </summary>
        private bool SetFieldCanExecute()
        {
            return !RTRunning && CustFieldCheck;
        }
        private void SetFieldExecute()
        {
            //Do work here
            HdrEnable = false;
            Set_Popupwindow();
            MyPopup_Open();

        }

        private void Set_Popupwindow()
        {
            //Setup listbox binding
            SelectedHdrList = new ObservableCollection<string>();
            SelectedHdrList = CRealtimeModel.GetSelectHrdCheckList();
            AvailableHdrList = CRealtimeModel.AvailableItemList;
        }

        private void MyPopup_Open()
        {
            this.OpenSetup = true; //Open the popup window   
        }

        private string GetBigBoxData(DataTable MyTable, int SelectIndex)
        {
            string strItem;
            string strFormat = "HH:mm:ss";
            var Newdat = MyTable.Rows[0][CmbDropDownList[SelectIndex]];

            if ((Newdat.GetType().FullName == "System.Single") || (Newdat.GetType().FullName == "System.Double"))
            {
                strItem = Convert.ToDouble(Newdat.ToString()).ToString("0.##");
            }
            else if (Newdat.GetType().FullName == "System.DateTime")
                strItem = Convert.ToDateTime(Newdat).ToString(strFormat); //System.DateTime
            else
                strItem = Newdat.ToString();
            Newdat = null;
            return strItem;
        }

        private void UpdateBigNumbers(DataTable MyTable)
        {
            for (int i = 0; i < CmbDropDownList.Count; i++)
            {
                if (CmbDropDownList[i].Contains("Viscosity"))
                    CmbDropDownList[i] = "Finish";

                if (CmbDropDownList[i].Contains("CusLotNumber"))
                    CmbDropDownList[i] = "FC_LotIdentString";
            }

            string strBox1 = GetBigBoxData(MyTable, _selectedBox1Combo);
            string strBox2 = GetBigBoxData(MyTable, _selectedBox2Combo);
            string strBox3 = GetBigBoxData(MyTable, _selectedBox3Combo);
            string strBox4 = GetBigBoxData(MyTable, _selectedBox4Combo);
            string strBox5 = GetBigBoxData(MyTable, _selectedBox5Combo);
            string strBox6 = GetBigBoxData(MyTable, _selectedBox6Combo);
            string strBox7 = GetBigBoxData(MyTable, _selectedBox7Combo);


            BigComboBox = new string[] { strBox1, strBox2, strBox3, strBox4, strBox5, strBox6, strBox7 };

            for (int i = 0; i < CmbDropDownList.Count; i++)
            {
                if (CmbDropDownList[i].Contains("SpareSngFld3"))
                    CmbDropDownList[i] = "%CV";
            }

            // AllFieldstable.Columns["SpareSngFld3"].ColumnName = "%CV";
        }




        #endregion


        /// <summary>
        /// Update all display
        /// If not configed for Drop Tracking, the bale position is always be 0
        /// </summary>
        private void UpdateNewRealTimeData()
        {
            byte iPosition = 0;

            newIndex = this.CRealtimeModel.GetNewIndex();

            DataTable CustomTable = new DataTable();

            if (preIndex != newIndex)
            {
                StartHeartBeatTimer();

                for (int i = 0; i < CmbDropDownList.Count; i++)
                {
                    if (CmbDropDownList[i].Contains("Viscosity"))
                        CmbDropDownList[i] = "Finish";
                    if (CmbDropDownList[i].Contains("CusLotNumber"))
                        CmbDropDownList[i] = "FC_LotIdentString";
                    if (CmbDropDownList[i].Contains("%CV"))
                        CmbDropDownList[i] = "SpareSngFld3";
                }

                using (var AllFieldstable = CRealtimeModel.GetNewRealTimeTable(CmbDropDownList))
                {
                    CmbDropDownList.Add("ADWeight");
                    CmbDropDownList.Add("BoneDry%"); //// Bone Dry % = 100 - Moisture
                    CmbDropDownList.Add("AirDry%");
                    CmbDropDownList.Add("Dirt_mm2/kg2");
                    CmbDropDownList.Add("Regain%");

                    if (AllFieldstable.Rows.Count > 0)
                    {
                        DataColumn NewCol = AllFieldstable.Columns.Add("ADWeight", typeof(float));
                        DataColumn NewCol2 = AllFieldstable.Columns.Add("BoneDry%", typeof(float));
                        DataColumn NewCol3 = AllFieldstable.Columns.Add("AirDry%", typeof(float));
                        DataColumn NewCol4 = AllFieldstable.Columns.Add("Dirt_mm2/kg2", typeof(float));
                        DataColumn NewCol5 = AllFieldstable.Columns.Add("Regain%", typeof(float));
                        AllFieldstable.AcceptChanges();

                        try
                        {
                            for (int i = 0; i < AllFieldstable.Rows.Count; i++)
                            {

                                if (AllFieldstable.Rows[i]["BDWeight"] != null)
                                    AllFieldstable.Rows[i]["ADWeight"] = AllFieldstable.Rows[i].Field<float>("BDWeight") / 0.9;

                                if (AllFieldstable.Rows[i]["Moisture"] != null)
                                {
                                    AllFieldstable.Rows[i]["AirDry%"] =   (100 - AllFieldstable.Rows[i].Field<float>("Moisture")) / 0.9;
                                    AllFieldstable.Rows[i]["BoneDry%"] = 100 - AllFieldstable.Rows[i].Field<float>("Moisture");
                                    AllFieldstable.Rows[i]["Regain%"] = AllFieldstable.Rows[i].Field<float>("Moisture") / (1 - AllFieldstable.Rows[i].Field<float>("Moisture") / 100);
                                    
                                }

                                AllFieldstable.Rows[i]["Dirt_mm2/kg2"] = (AllFieldstable.Rows[i]["BasisWeight"] != null) & (AllFieldstable.Rows[i].Field<float>("BasisWeight") > 0)
                                    ? AllFieldstable.Rows[i].Field<float>("Dirt") / AllFieldstable.Rows[i].Field<float>("BasisWeight") * 2000
                                    : (object)0;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR in UpdateNewRealTimeData" + ex.Message);
                        }

                        for (int i = 0; i < CRealtimeModel.XmlColumnList.Count; i++)
                        {
                            CustomTable.Columns.Add(CRealtimeModel.XmlColumnList[i], AllFieldstable.Columns[CRealtimeModel.XmlColumnList[i]].DataType);
                        }

                        //using linq to add empty rows
                        AllFieldstable.AsEnumerable().All(row => { CustomTable.Rows.Add(); return true; });

                        foreach (DataColumn v_Column in CustomTable.Columns)
                        {
                            string ColumnName = v_Column.ColumnName;

                            if (AllFieldstable.Columns.Contains(ColumnName))
                            {
                                for (int i = 0; i < CustomTable.Rows.Count; i++)
                                {
                                    if (ColumnName == "Moisture")
                                    {
                                        switch (Settings.Default.MoistureUnit)
                                        {
                                            case 0: // %MC
                                                if (AllFieldstable.Rows[i][ColumnName] != null)
                                                    CustomTable.Rows[i][ColumnName] = AllFieldstable.Rows[i][ColumnName];
                                                break;

                                            case 1: // %MR Moisture / ( 1- Moisture / 100)
                                                if (AllFieldstable.Rows[i][ColumnName] != null)
                                                    CustomTable.Rows[i][ColumnName] = AllFieldstable.Rows[i].Field<float>(ColumnName) / (1 - AllFieldstable.Rows[i].Field<float>(ColumnName) / 100);
                                                break;

                                            case 2: // %AD
                                                if (AllFieldstable.Rows[i][ColumnName] != null)
                                                    CustomTable.Rows[i][ColumnName] = AllFieldstable.Rows[i]["AirDry%"];
                                                break;

                                            case 3: // %BD
                                                if (AllFieldstable.Rows[i][ColumnName] != null)
                                                    CustomTable.Rows[i][ColumnName] = AllFieldstable.Rows[i]["BoneDry%"];
                                                break;
                                        }
                                    }

                                    else
                                    {
                                        if (AllFieldstable.Rows[i][ColumnName] != null)
                                            CustomTable.Rows[i][ColumnName] = AllFieldstable.Rows[i][ColumnName];
                                    }

                                    if (MainWindow.AppWindows.bDebug)
                                        Console.WriteLine("UpdateNewRealTimeData -- " + CustomTable.Rows[i][ColumnName]);
                                }
                            }
                        }

                        CustomTable.AcceptChanges();

                        if (AllFieldstable.Columns.Contains("Position"))
                        {
                            if (AllFieldstable.Rows[0]["Position"].ToString() != null)
                            {
                                iPosition = AllFieldstable.Rows[0].Field<Byte>("Position");
                                Application.Current.Dispatcher.Invoke(new Action(() => { Animatebale(iPosition); }));
                            }

                            if (!CRealtimeModel.XmlColumnList.Contains("Position"))
                            {
                              //  CRealtimeModel.XmlColumnList.Remove("Position");
                              //  AllFieldstable.Columns.Remove("Position");
                            }
                        }

                        //This is for the textbox on the conveyor
                        if (AllFieldstable.Columns.Contains("Weight"))
                            ScaleWeight = AllFieldstable.Rows[0].Field<float>("Weight").ToString("#0.##");

                        if (AllFieldstable.Columns.Contains("Forte"))
                            ForteNumber = AllFieldstable.Rows[0]["Forte"].ToString();

                        if (AllFieldstable.Columns.Contains("Moisture"))
                            CurMoisture = ClassCommon.CalulateMoisture( AllFieldstable.Rows[0].Field<float>("Moisture").ToString(), Settings.Default.MoistureUnit);

                        if (AllFieldstable.Columns.Contains("LineName"))
                            LineName = AllFieldstable.Rows[0]["LineName"].ToString();

                        UpdateBigNumbers(AllFieldstable);

                        if (RealTimeDataTable == null) RealTimeDataTable = new DataTable();

                        if (CustFieldCheck)
                            RealTimeDataTable = UpdateDataGrid(RealTimeDataTable, CustomTable);
                        else
                            RealTimeDataTable = UpdateDataGrid(RealTimeDataTable, AllFieldstable);

                        
                        switch (Settings.Default.iLanguageIdx)
                        {
                            case 0: // "en-US":
                                StringStatus = "Status : Bale Update @ : " + DateTime.Now;
                                break;
                            case 1: //"Sp-SP":
                                StringStatus = "Estado : Actualizar datos @ : " + DateTime.Now;
                                break;
                            default:
                                StringStatus = "Status : New Bale Update @ : " + DateTime.Now;
                                break;
                        }

                        preIndex = newIndex;
                        CustomTable = null;
                    }
                }
            }

           // if (ShowMe == 0.1) ShowMe = 0.8;
           // else if (ShowMe == 0.8) ShowMe = 0.1;
        }

        private void Animatebale(int iBale)
        {
            RealTime.RTWindows.MoveBaleOne(iBale);
        }

        private DataTable UpdateDataGrid(DataTable oldtable, DataTable newtable)
        {
            DataTable xTable = new DataTable();

            int balecount = Settings.Default.iRealTimeBale;

            try
            {
                if ((oldtable.Columns.Count == 0) || (oldtable.Columns.Count != newtable.Columns.Count)) xTable = newtable;
                else
                {
                    DataRow NewRow = oldtable.NewRow();
                    NewRow.ItemArray = newtable.Rows[0].ItemArray;

                    oldtable.Rows.InsertAt(NewRow, 0);

                    if (oldtable.Rows.Count > balecount)
                        oldtable.Rows.RemoveAt(oldtable.Rows.Count - 1);
                    xTable = oldtable; // newtable;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in UpdateDataGrid " + ex.Message);
            }
            return xTable;
        }


        #region DispatcherTimer /////////////////////////////////////////////////////////////////////////////////////

        // System.Timers.Timer RtScanTimer;
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
            dispatcherTimer?.Stop();

            //Application.Current.Dispatcher.Invoke(new Action(() => { UpdateRealTimeData(); }));
            Application.Current.Dispatcher.Invoke(new Action(() => { UpdateNewRealTimeData(); }));
            //Application.Current.Dispatcher.Invoke(new Action(() => { Animatebale(); }));

            Thread.Sleep(500); //Rest for 1/2 Sec.
            dispatcherTimer?.Start();
        }

        private void StartTimer()
        {
            dispatcherTimer?.Start();
        }

        private void StopTimer()
        {
            dispatcherTimer?.Stop();
            RTRunning = false;

            //   UpdateInfo = "Status : Scan timer Stop";
            _eventAggregator.GetEvent<UpdatedEvent>().Publish(RTRunning);
            ShowMe = 0.1;
            // Opac = 1.0;
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

    public class CustomDataGrid : DataGrid
    {
        public static int IRowSelected { get; set; }

        public CustomDataGrid()
        {
            this.SelectionChanged += CustomDataGrid_SelectionChanged;
        }

        void CustomDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid grid = (sender as DataGrid);
            if (this.SelectedItemsList != null)
            {
                grid.Dispatcher.Invoke(delegate {
                    grid.UpdateLayout();
                    grid.ScrollIntoView(grid.SelectedItem, null);
                    grid.Focus(); // added this to make it focus to the grid
                });
            }
            this.SelectedItemsList = this.SelectedItems;
            IRowSelected = this.SelectedIndex;
            if (MainWindow.AppWindows.bDebug)
                Console.WriteLine("SelectedItems=  " + this.SelectedItemsList.ToString());
        }

        #region SelectedItemsList

        public object SelectedItemsList
        {
            get { return (object)GetValue(SelectedItemsListProperty); }
            set { SetValue(SelectedItemsListProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsListProperty =
                 DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(CustomDataGrid), new PropertyMetadata(null));

        #endregion
    }
}
