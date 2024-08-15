

using ForteArg.Services;
using ForteARP.Module_ProdEsitmate.Model;
using ForteARP.Properties;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Data;
using System.Threading;
using System.Windows;

namespace ForteARP.Module_ProdEsitmate.ViewModels
{
    public class ProductEstViewModel : BindableBase
    {

        protected readonly Prism.Events.IEventAggregator _eventAggregator;
        public DelegateCommand LoadedPageICommand { get; set; } //Load Page
        public DelegateCommand ClosedPageICommand { get; set; } //Close page

        readonly ClsProdEstModel ProdEstModel;

        private bool _rtrunning = false;
        public bool RTRunning
        {
            get { return _rtrunning; }
            set { SetProperty(ref _rtrunning, value); }
        }
        private bool _rtIdle = true;
        public bool RTIdle
        {
            get { return _rtIdle; }
            set { SetProperty(ref _rtIdle, value); }
        }

        public int IScanDuration
        {
            get { return Settings.Default.iScanRate; }
        }

        /// <summary>
        /// MC1
        /// </summary>
        private Double _RTno1MC1;
        public Double RTno1MC1
        {
            get { return _RTno1MC1; }
            set { SetProperty(ref _RTno1MC1, value); }
        }
        private Double _RTno2MC1;
        public Double RTno2MC1
        {
            get { return _RTno2MC1; }
            set { SetProperty(ref _RTno2MC1, value); }
        }
        private Double _RTno3MC1;
        public Double RTno3MC1
        {
            get { return _RTno3MC1; }
            set { SetProperty(ref _RTno3MC1, value); }
        }
        private Double _DiaMC1;
        public Double DiaMC1
        {
            get { return _DiaMC1; }
            set { SetProperty(ref _DiaMC1, value); }
        }
        /// <summary>
        /// Estimate
        /// </summary>
        private Double _Esno1MC1;
        public Double Esno1MC1
        {
            get { return _Esno1MC1; }
            set { SetProperty(ref _Esno1MC1, value); }
        }
        private Double _Esno2MC1;
        public Double Esno2MC1
        {
            get { return _Esno2MC1; }
            set { SetProperty(ref _Esno2MC1, value); }
        }
        private Double _Esno3MC1;
        public Double Esno3MC1
        {
            get { return _Esno3MC1; }
            set { SetProperty(ref _Esno3MC1, value); }
        }
        private Double _EsMC1;
        public Double EsMC1
        {
            get { return _EsMC1; }
            set { SetProperty(ref _EsMC1, value); }
        }

        private Double _Tono1MC1;
        public Double Tono1MC1
        {
            get { return _Tono1MC1; }
            set { SetProperty(ref _Tono1MC1, value); }
        }
        private Double _Tono2MC1;
        public Double Tono2MC1
        {
            get { return _Tono2MC1; }
            set { SetProperty(ref _Tono2MC1, value); }
        }
        private Double _Tono3MC1;
        public Double Tono3MC1
        {
            get { return _Tono3MC1; }
            set { SetProperty(ref _Tono3MC1, value); }
        }
        private Double _ToMC1;
        public Double ToMC1
        {
            get { return _ToMC1; }
            set { SetProperty(ref _ToMC1, value); }
        }

        private int _Velocidad1;
        public int Velocidad1
        {
            get { return _Velocidad1; }
            set { SetProperty(ref _Velocidad1, value); }
        }

        private Double _PesoBase1;
        public Double PesoBase1
        {
            get { return _PesoBase1; }
            set { SetProperty(ref _PesoBase1, value); }
        }

        private string _AirDry1;
        public string AirDry1
        {
            get { return _AirDry1; }
            set { SetProperty(ref _AirDry1, value); }
        }


        //---------------------------------------------------------------------------
        /// <summary>
        /// MC2
        /// </summary>
        private Double _RTno1MC2;
        public Double RTno1MC2
        {
            get { return _RTno1MC2; }
            set { SetProperty(ref _RTno1MC2, value); }
        }
        private Double _RTno2MC2;
        public Double RTno2MC2
        {
            get { return _RTno2MC2; }
            set { SetProperty(ref _RTno2MC2, value); }
        }
        private Double _RTno3MC2;
        public Double RTno3MC2
        {
            get { return _RTno3MC2; }
            set { SetProperty(ref _RTno3MC2, value); }
        }
        private Double _DiaMC2;
        public Double DiaMC2
        {
            get { return _DiaMC2; }
            set { SetProperty(ref _DiaMC2, value); }
        }
        //-----------------------------------------
        private Double _Esno1MC2;
        public Double Esno1MC2
        {
            get { return _Esno1MC2; }
            set { SetProperty(ref _Esno1MC2, value); }
        }
        private Double _Esno2MC2;
        public Double Esno2MC2
        {
            get { return _Esno2MC2; }
            set { SetProperty(ref _Esno2MC2, value); }
        }
        private Double _Esno3MC2;
        public Double Esno3MC2
        {
            get { return _Esno3MC2; }
            set { SetProperty(ref _Esno3MC2, value); }
        }
        private Double _EsMC2;
        public Double EsMC2
        {
            get { return _EsMC2; }
            set { SetProperty(ref _EsMC2, value); }
        }
        //------------------------------------------
        private Double _Tono1MC2;
        public Double Tono1MC2
        {
            get { return _Tono1MC2; }
            set { SetProperty(ref _Tono1MC2, value); }
        }
        private Double _Tono2MC2;
        public Double Tono2MC2
        {
            get { return _Tono2MC2; }
            set { SetProperty(ref _Tono2MC2, value); }
        }
        private Double _Tono3MC2;
        public Double Tono3MC2
        {
            get { return _Tono3MC2; }
            set { SetProperty(ref _Tono3MC2, value); }
        }
        private Double _ToMC2;
        public Double ToMC2
        {
            get { return _ToMC2; }
            set { SetProperty(ref _ToMC2, value); }
        }
        private int _Velocidad2;
        public int Velocidad2
        {
            get { return _Velocidad2; }
            set { SetProperty(ref _Velocidad2, value); }
        }

        private Double _PesoBase2;
        public Double PesoBase2
        {
            get { return _PesoBase2; }
            set { SetProperty(ref _PesoBase2, value); }
        }

        private String _AirDry2;
        public String AirDry2
        {
            get { return _AirDry2; }
            set { SetProperty(ref _AirDry2, value); }
        }

        //-----------------------------------------------------------------

        private string _updateinfo;
        public string UpdateInfo
        {
            get { return _updateinfo; }
            set { SetProperty(ref _updateinfo, value); }
        }

        private string _strScanDuration;
        public string StrScanDuration
        {
            get { return _strScanDuration; }
            set { SetProperty(ref _strScanDuration, value); }
        }


        private string _DataInfo;
        public string DataInfo
        {
            get { return _DataInfo; }
            set { SetProperty(ref _DataInfo, value); }
        }


        public ProductEstViewModel(Prism.Events.IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;

            ProdEstModel = new ClsProdEstModel();

            LoadedPageICommand = new DelegateCommand(LoadPageExecute, LoadPageCanExecute);
            ClosedPageICommand = new DelegateCommand(ClosedPageExecute, ClosedPageCanExecute);
            ClsSerilog.LogMessage(ClsSerilog.Info, $"Initialize Product Estimate");
        }


        private bool ClosedPageCanExecute()
        {
            return true;
        }

        private void ClosedPageExecute()
        {
            StopTimer();
        }

        private bool LoadPageCanExecute()
        {
            return true;
        }

        private void LoadPageExecute()
        {
            ProdEstModel.SetupAndInitSql();

            if (ProdEstModel.bProdEstReady)
            {
                InitializeTimer();
                StrScanDuration = "Duración del escaneo: " + Settings.Default.iScanRate.ToString() + " Segundos.";
                StartTimer();



            }

            // MessageBox.Show("Start Timer! StrScanDuration = " + iScanDuration.ToString());
        }


        private double _showme = .1;
        public double ShowMe
        {
            get { return _showme; }
            set { SetProperty(ref _showme, value); }
        }

        private void Heartbeat()
        {
            if (ShowMe == 0.1) ShowMe = 1;
            else if (ShowMe == 1) ShowMe = 0.1;
        }

        private void UpdateDisplay(DataTable MyTable)
        {

            RTno1MC1 = 0;
            RTno2MC1 = 0;
            RTno3MC1 = 0;

            Esno1MC1 = 0;
            Esno2MC1 = 0;
            Esno3MC1 = 0;

            RTno1MC2 = 0;
            RTno2MC2 = 0;
            RTno3MC2 = 0;

            Esno1MC2 = 0;
            Esno2MC2 = 0;
            Esno3MC2 = 0;

            Velocidad1 = 0;
            Velocidad2 = 0;
            PesoBase1 = 0.00;

            AirDry1 = "0.00";
            AirDry2 = "0.00";


            if (MyTable.Rows.Count > 1)
            {
                //MC1------------------------------------------
                RTno1MC1 = MyTable.Rows[0].Field<double>("Shift1Real");
                RTno2MC1 = MyTable.Rows[0].Field<double>("Shift2Real");
                RTno3MC1 = MyTable.Rows[0].Field<double>("Shift3Real");
                DiaMC1 = RTno1MC1 + RTno2MC1 + RTno3MC1;

                Esno1MC1 = MyTable.Rows[0].Field<double>("Shift1Est");
                Esno2MC1 = MyTable.Rows[0].Field<double>("Shift2Est");
                Esno3MC1 = MyTable.Rows[0].Field<double>("Shift3Est");
                EsMC1 = Esno1MC1 + Esno2MC1 + Esno3MC1;

                Tono1MC1 = RTno1MC1 + Esno1MC1;
                Tono2MC1 = RTno2MC1 + Esno2MC1;
                Tono3MC1 = RTno3MC1 + Esno3MC1;
                ToMC1 = Tono1MC1 + Tono2MC1 + Tono3MC1;

                Velocidad1 = MyTable.Rows[0].Field<int>("MachSpeed");
                PesoBase1 = MyTable.Rows[0].Field<Double>("AvBasisWt");

                AirDry1 = ((100 - MyTable.Rows[0].Field<Double>("AvMC")) / 0.9).ToString("0.##");

                //MC2------------------------------------------
                RTno1MC2 = MyTable.Rows[1].Field<double>("Shift1Real");
                RTno2MC2 = MyTable.Rows[1].Field<double>("Shift2Real");
                RTno3MC2 = MyTable.Rows[1].Field<double>("Shift3Real");
                DiaMC2 = RTno1MC2 + RTno2MC2 + RTno3MC2;

                Esno1MC2 = MyTable.Rows[1].Field<double>("Shift1Est");
                Esno2MC2 = MyTable.Rows[1].Field<double>("Shift2Est");
                Esno3MC2 = MyTable.Rows[1].Field<double>("Shift3Est");
                EsMC2 = Esno1MC2 + Esno2MC2 + Esno3MC2;

                Tono1MC2 = RTno1MC2 + Esno1MC2;
                Tono2MC2 = RTno2MC2 + Esno2MC2;
                Tono3MC2 = RTno3MC2 + Esno3MC2;
                ToMC2 = Tono1MC2 + Tono2MC2 + Tono3MC2;

                Velocidad2 = MyTable.Rows[1].Field<int>("MachSpeed");
                PesoBase2 = MyTable.Rows[1].Field<Double>("AvBasisWt");

                AirDry2 = ((100 - MyTable.Rows[1].Field<Double>("AvMC")) / 0.9).ToString("0.##");

                DataInfo = "Tiempo de cálculo " + MyTable.Rows[0].Field<DateTime>("CalcTime").ToString("dd:mm:yy hh:mm:ss");
            }
        }

        private void GetNewData()
        {
            Heartbeat();
  
            try
            {
                DataTable ProdEstTable = ProdEstModel.GetProdEstTable();

                if (ProdEstTable.Rows.Count > 0)
                    UpdateDisplay(ProdEstTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR IN GetNewData " + ex.Message);
            }
        }

        private void UpdateStatus(string strMsg)
        {
            UpdateInfo = strMsg;
        }

        #region DispatcherTimer /////////////////////////////////////////////////////////////////////////////////////

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
            Application.Current.Dispatcher.Invoke(new Action(() => { GetNewData(); }));

            Thread.Sleep(500); //Rest for 1/2 Sec.
            dispatcherTimer.Start();
        }

        private void StartTimer()
        {
            dispatcherTimer?.Start();
            UpdateInfo = "Tiempo de escaneo iniciado";

        }
        private void StopTimer()
        {
            dispatcherTimer?.Stop();
            RTRunning = false;

            Application.Current.Dispatcher.Invoke(new Action(() => { UpdateStatus("Tiempo de Scan Detenido"); }));

        }



        #endregion DispatcherTimer /////////////////////////////////////////////////////////////////////////////////////////

    }
}
