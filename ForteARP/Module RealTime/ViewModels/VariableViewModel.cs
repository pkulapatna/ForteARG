

using ForteArg.Services;
using ForteARP.Module_RealTime.Model;
using ForteARP.Properties;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Windows;


namespace ForteARP.Module_RealTime.ViewModels
{
    public class VariableViewModel : BindableBase
    {
        protected readonly Prism.Events.IEventAggregator _eventAggregator;

        private Sqlhandler _sqlhandler = Sqlhandler.Instance;

        public RealTimeModel CRealtimeModel;

        private int IScanTimer { get; set; }
        private readonly long preIndex = 0;
        private long newIndex = 0;


        
     
        public DelegateCommand StopCommand { get; set; }        //Stop Button

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
        public DelegateCommand Box8ComboCommand { get; set; }
        //Combo 7
        public DelegateCommand Box9ComboCommand { get; set; }
        public DelegateCommand Box10ComboCommand { get; set; }
        public DelegateCommand Box11ComboCommand { get; set; }
        public DelegateCommand Box12ComboCommand { get; set; }

        private bool _rtrunning = false;
        public bool RTRunning
        {
            get => _rtrunning; 
            set => SetProperty(ref _rtrunning, value); 
        }

        private bool _rtIdle = true;
        public bool RTIdle
        {
            get { return _rtIdle; }
            set { SetProperty(ref _rtIdle, value); }
        }
        private string _strScanDuration;
        public string StrScanDuration
        {
            get { return _strScanDuration; }
            set { SetProperty(ref _strScanDuration, value); }
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

        private int _selectedBox8Combo;
        public int SelectedBox8Combo
        {
            get { return _selectedBox8Combo; }
            set { SetProperty(ref _selectedBox8Combo, value); }
        }
        private int _selectedBox9Combo;
        public int SelectedBox9Combo
        {
            get { return _selectedBox9Combo; }
            set { SetProperty(ref _selectedBox9Combo, value); }
        }

        private int _selectedBox10Combo;
        public int SelectedBox10Combo
        {
            get { return _selectedBox10Combo; }
            set { SetProperty(ref _selectedBox10Combo, value); }
        }

        private int _selectedBox11Combo;
        public int SelectedBox11Combo
        {
            get { return _selectedBox11Combo; }
            set { SetProperty(ref _selectedBox11Combo, value); }
        }

        private int _selectedBox12Combo;
        public int SelectedBox12Combo
        {
            get { return _selectedBox12Combo; }
            set { SetProperty(ref _selectedBox12Combo, value); }
        }

        private string[] _BigTextBox;
        public string[] BigTextBox
        {
            get { return _BigTextBox; }
            set { SetProperty(ref _BigTextBox, value); }
        }

        public List<string> _cmbDropDownList;
        public List<string> CmbDropDownList
        {
            get { return _cmbDropDownList; }
            set { SetProperty(ref _cmbDropDownList, value); }
        }

        public Double _showme = 0.1;
        public Double ShowMe
        {
            get { return _showme; }
            set { SetProperty(ref _showme, value); }
        }

        private DataTable _realtimedatatable;
        public DataTable RealTimeDataTable
        {
            get { return _realtimedatatable; }
            set { SetProperty(ref _realtimedatatable, value); }
        }

        public VariableViewModel(Prism.Events.IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;

            StopCommand = new DelegateCommand(StopExecute, StopCanExecute).ObservesProperty(() => RTRunning);

            //Comboboxes
            Box1ComboCommand = new DelegateCommand(Box1Execute, Box1CanExecute).ObservesProperty(() => SelectedBox1Combo).ObservesProperty(() => RTRunning);
            Box2ComboCommand = new DelegateCommand(Box2Execute, Box2CanExecute).ObservesProperty(() => SelectedBox2Combo).ObservesProperty(() => RTRunning);
            Box3ComboCommand = new DelegateCommand(Box3Execute, Box3CanExecute).ObservesProperty(() => SelectedBox3Combo).ObservesProperty(() => RTRunning);
            Box4ComboCommand = new DelegateCommand(Box4Execute, Box4CanExecute).ObservesProperty(() => SelectedBox4Combo).ObservesProperty(() => RTRunning);
            Box5ComboCommand = new DelegateCommand(Box5Execute, Box5CanExecute).ObservesProperty(() => SelectedBox5Combo).ObservesProperty(() => RTRunning);
            Box6ComboCommand = new DelegateCommand(Box6Execute, Box6CanExecute).ObservesProperty(() => SelectedBox6Combo).ObservesProperty(() => RTRunning);
            Box7ComboCommand = new DelegateCommand(Box7Execute, Box7CanExecute).ObservesProperty(() => SelectedBox7Combo).ObservesProperty(() => RTRunning);

            Box8ComboCommand = new DelegateCommand(Box8Execute, Box8CanExecute).ObservesProperty(() => SelectedBox8Combo).ObservesProperty(() => RTRunning);
            Box9ComboCommand = new DelegateCommand(Box9Execute, Box9CanExecute).ObservesProperty(() => SelectedBox9Combo).ObservesProperty(() => RTRunning);

            Box10ComboCommand = new DelegateCommand(Box10Execute, Box10CanExecute).ObservesProperty(() => SelectedBox10Combo).ObservesProperty(() => RTRunning);
            Box11ComboCommand = new DelegateCommand(Box11Execute, Box11CanExecute).ObservesProperty(() => SelectedBox11Combo).ObservesProperty(() => RTRunning);
            Box12ComboCommand = new DelegateCommand(Box12Execute, Box12CanExecute).ObservesProperty(() => SelectedBox12Combo).ObservesProperty(() => RTRunning);

            _eventAggregator.GetEvent<UpdatedEventShutdown>().Subscribe(ProgramShutdown);

           ClsSerilog.LogMessage(ClsSerilog.Info, $"Initialize Realtime Variable");
            
        }

        private DelegateCommand _loadedPageICommand;
        public DelegateCommand LoadedPageICommand =>
       _loadedPageICommand ?? (_loadedPageICommand =
           new DelegateCommand(LoadPageExecute));
        private void LoadPageExecute()
        {
            if (CRealtimeModel != null) CRealtimeModel = null;
            CRealtimeModel = new RealTimeModel();

            Setup_DropDownList();
            ShowMe = 0.1;
            MainWindow.AppWindows.SetupAppTitle("Forté RealTime Variables From  -> " + _sqlhandler.Host + @"\" + _sqlhandler.SqlInstance);
        }

        private DelegateCommand _closedPageICommand;
        public DelegateCommand ClosedPageICommand =>
      _closedPageICommand ?? (_closedPageICommand =
          new DelegateCommand(ClosedPageExecute));
        private void ClosedPageExecute()
        {
            if (CRealtimeModel != null) CRealtimeModel = null;

        }


        private DelegateCommand _startICommand;
        public DelegateCommand StartCommand =>
       _startICommand ?? (_startICommand =
           new DelegateCommand(StartExecute).ObservesProperty(() => RTRunning));

        private void StartExecute()
        {
            IScanTimer = Settings.Default.iScanRate;

            RTRunning = true;
            RTIdle = false;
            _eventAggregator.GetEvent<UpdatedEvent>().Publish(RTRunning);

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

            this.InitializeTimer();
            this.StartTimer();

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

        private void Box1Execute()
        {
            Settings.Default.iVarBoxOne = SelectedBox1Combo;
            Settings.Default.Save();
        }

        private bool Box1CanExecute()
        {
            return !RTRunning;
        }

        private void Box2Execute()
        {
            Settings.Default.iVarBoxTwo = SelectedBox2Combo;
            Settings.Default.Save();
        }

        private bool Box2CanExecute()
        {
            return !RTRunning;
        }

        private void Box3Execute()
        {
            Settings.Default.iVarBoxThree = SelectedBox3Combo;
            Settings.Default.Save();
        }

        private bool Box3CanExecute()
        {
            return !RTRunning;
        }

        private void Box4Execute()
        {
            Settings.Default.iVarBoxFour = SelectedBox4Combo;
            Settings.Default.Save();
        }

        private bool Box4CanExecute()
        {
            return !RTRunning;
        }

        private void Box5Execute()
        {
            Settings.Default.iVarBoxFive = SelectedBox5Combo;
            Settings.Default.Save();
        }

        private bool Box5CanExecute()
        {
            return !RTRunning;
        }

        private void Box6Execute()
        {
            Settings.Default.iVarBoxSix = SelectedBox6Combo;
            Settings.Default.Save();
        }

        private bool Box6CanExecute()
        {
            return !RTRunning;
        }

        private void Box7Execute()
        {
            Settings.Default.iVarBoxSeven = SelectedBox7Combo;
            Settings.Default.Save();
        }

        private bool Box7CanExecute()
        {
            return !RTRunning;
        }

        private void Box8Execute()
        {
            Settings.Default.iVarBoxEight = SelectedBox8Combo;
            Settings.Default.Save();
        }

        private bool Box8CanExecute()
        {
            return !RTRunning;
        }

        private void Box9Execute()
        {
            Settings.Default.iVarBoxNine = SelectedBox9Combo;
            Settings.Default.Save();
        }

        private bool Box9CanExecute()
        {
            return !RTRunning;
        }

        private void Box10Execute()
        {
            Settings.Default.iVarBoxTen = SelectedBox10Combo;
            Settings.Default.Save();
        }



        private bool Box10CanExecute()
        {
            return !RTRunning;
        }

        private void Box11Execute()
        {
            Settings.Default.iVarBoxEleven = SelectedBox11Combo;
            Settings.Default.Save();
        }

        private bool Box11CanExecute()
        {
            return !RTRunning;
        }

        private void Box12Execute()
        {
            Settings.Default.iVarBoxTwelve = SelectedBox12Combo;
            Settings.Default.Save();
        }

        private bool Box12CanExecute()
        {
            return !RTRunning;
        }



        private void StopExecute()
        {
            Application.Current.Dispatcher.Invoke(new Action(() => { StopTimer(); }));
            
            RTRunning = false;
         
            _eventAggregator.GetEvent<UpdatedEvent>().Publish(RTRunning);
            
            ShowMe = 0.1;
            RTIdle = true;
        }

        private bool StopCanExecute()
        {
            return RTRunning;
        }

       

        private bool StartCanExecute()
        {
            return !RTRunning;
        }

        private void Setup_DropDownList()
        {
            try
            {
             
                CmbDropDownList = CRealtimeModel.GetHdrList();

                SelectedBox1Combo = Settings.Default.iVarBoxOne;
                SelectedBox2Combo = Settings.Default.iVarBoxTwo;
                SelectedBox3Combo = Settings.Default.iVarBoxThree;
                SelectedBox4Combo = Settings.Default.iVarBoxFour;
                SelectedBox5Combo = Settings.Default.iVarBoxFive;
                SelectedBox6Combo = Settings.Default.iVarBoxSix;
                SelectedBox7Combo = Settings.Default.iVarBoxSeven;
                SelectedBox8Combo = Settings.Default.iVarBoxEight;
                SelectedBox9Combo = Settings.Default.iVarBoxNine;
                SelectedBox10Combo = Settings.Default.iVarBoxTen;
                SelectedBox11Combo = Settings.Default.iVarBoxEleven;
                SelectedBox12Combo = Settings.Default.iVarBoxTwelve;

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR IN Setup_DropDownList VariableViewModel " + ex.Message);
            }
        }

        private void UpdateRealTimeData()
        {
           if (RTRunning)
           {
                try
                {
                    newIndex = CRealtimeModel.GetNewIndex();

                    if (preIndex != newIndex) //New Bale
                    {
                        for (int i = 0; i < CmbDropDownList.Count; i++)
                        {
                            if (CmbDropDownList[i].Contains("Viscosity"))
                                CmbDropDownList[i] = "Finish";

                            if (CmbDropDownList[i].Contains("CusLotNumber"))
                                CmbDropDownList[i] = "FC_LotIdentString";
                        }

                        using (var temptable = CRealtimeModel.GetNewUpdateDataTable(CmbDropDownList))
                        {
                            CmbDropDownList.Add("ADWeight");
                            CmbDropDownList.Add("BoneDry%"); //// Bone Dry % = 100 - Moisture
                            CmbDropDownList.Add("AirDry%");
                            CmbDropDownList.Add("Dirt_mm2/kg2");
                            CmbDropDownList.Add("Regain%");


                            if (temptable.Rows.Count > 0)
                            {
                                DataColumn NewCol = temptable.Columns.Add("ADWeight", typeof(Single));
                                DataColumn NewCol2 = temptable.Columns.Add("BoneDry%", typeof(Single));
                                DataColumn NewCol3 = temptable.Columns.Add("AirDry%", typeof(Single));
                                DataColumn NewCol4 = temptable.Columns.Add("Dirt_mm2/kg2", typeof(Single));
                                DataColumn NewCol5 = temptable.Columns.Add("Regain%", typeof(Single));

                                temptable.AcceptChanges();

                                for (int i = 0; i < temptable.Rows.Count; i++)
                                {
                                    if (temptable.Rows[i]["BDWeight"] != null)
                                        temptable.Rows[i]["ADWeight"] = temptable.Rows[i].Field<Single>("BDWeight") / 0.9;

                                    if (temptable.Rows[i]["Moisture"] != null)
                                    {
                                        temptable.Rows[i]["AirDry%"] = (100 - temptable.Rows[i].Field<Single>("Moisture")) / 0.9;
                                        temptable.Rows[i]["BoneDry%"] = 100 - temptable.Rows[i].Field<Single>("Moisture");
                                        temptable.Rows[i]["Regain%"] = temptable.Rows[i].Field<Single>("Moisture") / (1 - temptable.Rows[i].Field<Single>("Moisture") / 100);
                                    }

                                    temptable.Rows[i]["Dirt_mm2/kg2"] = (temptable.Rows[i]["BasisWeight"] != null) & (temptable.Rows[i].Field<Single>("BasisWeight") > 0)
                                        ? temptable.Rows[i].Field<Single>("Dirt") / temptable.Rows[i].Field<Single>("BasisWeight") * 2000
                                        : (object)0;
                                }
                                RealTimeDataTable = temptable;
                                UpdateBigNumbers(RealTimeDataTable);
                            }
                        }
                    }
                    if (ShowMe == 0.1) ShowMe = 0.8;
                    else if (ShowMe == 0.8) ShowMe = 0.1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"ERROR in VariableViewModel UpdateRealTimeData {ex.Message}");
                    ClsSerilog.LogMessage(ClsSerilog.Error, $"ERROR in VariableViewModel UpdateRealTimeData -> {ex.Message}");
                }
            }  
        }


        private string GetBigBoxData(DataTable MyTable, int SelectIndex)
        {
            string strItem = string.Empty;
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
            string strBox1 = string.Empty;
            string strBox2 = string.Empty;
            string strBox3 = string.Empty;
            string strBox4 = string.Empty;
            string strBox5 = string.Empty;
            string strBox6 = string.Empty;
            string strBox7 = string.Empty;
            string strBox8 = string.Empty;
            string strBox9 = string.Empty;
            string strBox10 = string.Empty;
            string strBox11 = string.Empty;
            string strBox12 = string.Empty;


            if (CmbDropDownList[SelectedBox1Combo].ToString() != "-Blank-")
            {
                strBox1 = GetBigBoxData(MyTable, _selectedBox1Combo);
            }
            if (CmbDropDownList[SelectedBox2Combo].ToString() != "-Blank-")
            {
                strBox2 = GetBigBoxData(MyTable, _selectedBox2Combo);
            }
            if (CmbDropDownList[SelectedBox3Combo].ToString() != "-Blank-")
            {
                strBox3 = GetBigBoxData(MyTable, _selectedBox3Combo);
            }

            if (CmbDropDownList[SelectedBox4Combo].ToString() != "-Blank-")
            {
                strBox4 = GetBigBoxData(MyTable, _selectedBox4Combo);
            }

            if (CmbDropDownList[SelectedBox5Combo].ToString() != "-Blank-")
            {
                strBox5 = GetBigBoxData(MyTable, _selectedBox5Combo);
            }

            if (CmbDropDownList[SelectedBox6Combo].ToString() != "-Blank-")
            {
                strBox6 = GetBigBoxData(MyTable, _selectedBox6Combo);
            }

            if (CmbDropDownList[SelectedBox7Combo].ToString() != "-Blank-")
            {
                strBox7 = GetBigBoxData(MyTable, _selectedBox7Combo);
            }

            if (CmbDropDownList[SelectedBox8Combo].ToString() != "-Blank-")
            {
                strBox8 = GetBigBoxData(MyTable, _selectedBox8Combo);
            }

            if (CmbDropDownList[SelectedBox9Combo].ToString() != "-Blank-")
            {
                strBox9 = GetBigBoxData(MyTable, _selectedBox9Combo);
            }

            if (CmbDropDownList[SelectedBox10Combo].ToString() != "-Blank-")
            {
                strBox10 = GetBigBoxData(MyTable, _selectedBox10Combo);
            }

            if (CmbDropDownList[SelectedBox11Combo].ToString() != "-Blank-")
            {
                strBox11 = GetBigBoxData(MyTable, _selectedBox11Combo);
            }

            if (CmbDropDownList[SelectedBox12Combo].ToString() != "-Blank-")
            {
                strBox12 = GetBigBoxData(MyTable, _selectedBox12Combo);
            }

            BigTextBox = new string[] { strBox1, strBox2, strBox3, strBox4, strBox5,
                strBox6, strBox7, strBox8, strBox9, strBox10, strBox11, strBox12 };
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
            
            if(RTRunning)
                Application.Current.Dispatcher.Invoke(new Action(() => { UpdateRealTimeData(); }));
            
            //Application.Current.Dispatcher.Invoke(new Action(() => { Animatebale(); }));

            Thread.Sleep(500); //Rest for 1/2 Sec.
            dispatcherTimer?.Start();
        }

        private void StartTimer()
        {
            dispatcherTimer?.Start();
            ClsSerilog.LogMessage(ClsSerilog.Info, $"Scan Timer started for Veriable Modeule");
            //  UpdateInfo = "Status : Scan timer Start";

        }
        private void StopTimer()
        {
            dispatcherTimer?.Stop();
            ClsSerilog.LogMessage(ClsSerilog.Info, $"Scan Timer stopped for Veriable Modeule");

            // UpdateInfo = "Status : Scan timer Stop";
           
            //_eventAggregator.GetEvent<UpdatedEvent>().Publish(RTRunning);
            //ShowMe = 0.1;
            // Opac = 1.0;
        }

        #endregion DispatcherTimer /////////////////////////////////////////////////////////////////////////////////////////

    }
}
