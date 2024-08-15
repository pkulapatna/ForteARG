

using ForteArg.Services;
using ForteARP.Charts;
using ForteARP.Module_WetLayer.Model;
using ForteARP.Properties;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;

namespace ForteARP.Module_WetLayer.ViewModels
{
    public class WetLayersTrendViewModel : BindableBase
    {
        protected readonly Prism.Events.IEventAggregator _eventAggregator;

        Sqlhandler _sqlhandler = Sqlhandler.Instance;

        public DelegateCommand LoadedPageICommand { get; set; }
        public DelegateCommand ClosedPageICommand { get; set; }
        public DelegateCommand QueryCommand { get; set; }
        public DelegateCommand PrintCommand { get; set; }

        private WetLayerModel cWetLayerModel;

 

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

        /// ////////////////////////////////////////////////////


        private string moistureLabel = "Layers MR% Data";
        public string MoistureLabel
        {
            get { return moistureLabel; }
            set { SetProperty(ref moistureLabel, value); }
        }



        private List<string> _wlmonthTableList;
        public List<string> WLMonthTableList
        {
            get { return _wlmonthTableList; }
            set { SetProperty(ref _wlmonthTableList, value); }
        }

        private int _selecttableindex;
        public int SelectTableIndex
        {
            get { return _selecttableindex; }
            set { SetProperty(ref _selecttableindex, value); }
        }

        private string _selectTableValue;
        public string SelectTableValue
        {
            get { return _selectTableValue; }
            set { SetProperty(ref _selectTableValue, value); }
        }

        private int _selectOCRindex;
        public int SelectOCRIndex
        {
            get { return _selectOCRindex; }
            set { SetProperty(ref _selectOCRindex, value); }
        }

        private int _iSampleBales = Settings.Default.iWetLayerTrendSample;
        public int ISampleBales
        {
            get { return _iSampleBales; }
            set { SetProperty(ref _iSampleBales, value); }
        }

        private string _TxtStatus;
        public string TxtStatus
        {
            get { return _TxtStatus; }
            set { SetProperty(ref _TxtStatus, value); }
        }

        private string _LayerAvg;
        public string LayerAvg
        {
            get { return _LayerAvg; }
            set { SetProperty(ref _LayerAvg, value); }
        }

        private string _LayerMax;
        public string LayerMax
        {
            get { return _LayerMax; }
            set { SetProperty(ref _LayerMax, value); }
        }

        private string _LayerMin;
        public string LayerMin
        {
            get { return _LayerMin; }
            set { SetProperty(ref _LayerMin, value); }
        }


        private double _MaximumHeight;
        public double MaximumHeight
        {
            get { return _MaximumHeight; }
            set { SetProperty(ref _MaximumHeight, value); }
        }

        private double _MinimumHeight;
        public double MinimumHeight
        {
            get { return _MinimumHeight; }
            set { SetProperty(ref _MinimumHeight, value); }
        }

        private DataTable _WetLayerDeltaTable;
        public DataTable WetLayerDeltaTable
        {
            get { return _WetLayerDeltaTable; }
            set { SetProperty(ref _WetLayerDeltaTable, value); }
        }

        private string _StringStatus = string.Empty;
        public string StringStatus
        {
            get { return _StringStatus; }
            set { SetProperty(ref _StringStatus, value); }
        }

        private string _totalcount = string.Empty;
        public string Totalcount
        {
            get { return _totalcount; }
            set { SetProperty(ref _totalcount, value); }
        }

        private bool _enablePrint = false;
        public bool EnablePrint
        {
            get { return _enablePrint; }
            set { SetProperty(ref _enablePrint, value); }
        }


        private double _Opac = 0.2;
        public double Opac
        {
            get { return _Opac; }
            set { SetProperty(ref _Opac, value); }
        }

        private string _YAxixTitle = "Moisture%";
        public string YAxixTitle
        {
            get { return _YAxixTitle; }
            set { SetProperty(ref _YAxixTitle, value); }
        }

        private string _xGraphTitle = string.Empty;
        private int iLayerCount;
        public int DefaultLayersCount = 16;

        public string XGraphTitle
        {
            get { return _xGraphTitle; }
            set { SetProperty(ref _xGraphTitle, value); }
        }


        private int _XColumns;
        public int XColumns
        {
            get { return _XColumns; }
            set { SetProperty(ref _XColumns, value); }
        }

        private string _GraphTitle;
        public string GraphTitle
        {
            get { return _GraphTitle; }
            set { SetProperty(ref _GraphTitle, value); }
        }

        private bool _MonthListEnable;
        public bool MonthListEnable
        {
            get { return _MonthListEnable; }
            set { SetProperty(ref _MonthListEnable, value); }
        }



        //Balers////////////////////////////////////////////////
        private bool _BalerCheck;
        public bool BalerCheck
        {
            get { return _BalerCheck; }
            set { SetProperty(ref _BalerCheck, value); }
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

        public WetLayersTrendViewModel(Prism.Events.IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;

            LoadedPageICommand = new DelegateCommand(LoadedPageExecute, LoadedPageCanExecute);
            ClosedPageICommand = new DelegateCommand(ClosedPageExecute, ClosedPageCanExecute);
            QueryCommand = new DelegateCommand(QueryExecute, QueryCanExecute);
            PrintCommand = new DelegateCommand(PrintExecute, PrintCanExecute);

           

        }

        private bool PrintCanExecute()
        {
            return true; //for now
        }
        private void PrintExecute()
        {
            //Do nothing 
        }


        private bool QueryCanExecute()
        {
            return true;
        }

        private void QueryExecute()
        {
            if (ISampleBales > 0)
            {
                Settings.Default.iWetLayerTrendSample = ISampleBales;
                Settings.Default.Save();
                GetWLDataGridview(SelectTableValue, ISampleBales);
            }
            else
            {
                ISampleBales = 3;
                Settings.Default.iWetLayerTrendSample = ISampleBales;
                Settings.Default.Save();
            }
        }

        private void GetWLDataGridview(string selectTableValue, int BaleSample)
        {
            string strOccr = string.Empty;
            // string stritems = "Layer1,Layer2,Layer3,Layer4,Layer5,Layer6" +
            //     ",Layer7,Layer8,Layer9,Layer10,Layer11,Layer12,Layer13,Layer14,Layer15,Layer16";

            string strAllColumns = "*";

            if (BalerCheck)
            {
                if (SelectBalerValue == "ALL")
                {
                    if (SelectOCRIndex == 0) strOccr = " ORDER BY ReadTime DESC ;";
                    else if (SelectOCRIndex == 1) strOccr = " ORDER BY ReadTime ASC ;";
                }
                else
                {
                    if (SelectOCRIndex == 0) strOccr = " WHERE BalerID = " + SelectBalerValue + " ORDER BY ReadTime DESC ;";
                    else if (SelectOCRIndex == 1) strOccr = " WHERE BalerID= " + SelectBalerValue + " ORDER BY ReadTime ASC ;";
                }
            }
            else
            {
                if (SelectOCRIndex == 0) strOccr = " ORDER BY ReadTime DESC ;";
                else if (SelectOCRIndex == 1) strOccr = " ORDER BY ReadTime ASC ;";
            }

            string strQuery = "SELECT TOP " + BaleSample + " " + strAllColumns + " From " + SelectTableValue + " with (NOLOCK) " + strOccr;

            try
            {
                DataTable WLDataTable = new DataTable();
                WLDataTable = cWetLayerModel.GetNewWLDataTable(SelectTableValue, strQuery);
                if (WLDataTable.Rows.Count > 0)
                {
                    EnablePrint = true;
                    Opac = 1.0;
                    ProccessData(WLDataTable);
                    TxtStatus = "Each Layers are from the average of " + WLDataTable.Rows.Count.ToString() + " Bales";
                    XGraphTitle = "Layers Trend of " + WLDataTable.Rows.Count.ToString() + " Bales from " + SelectTableValue;
                }
                else
                {
                    MessageBox.Show("No Record found in  = " + SelectTableValue);
                    TxtStatus = "No Record Found in " + SelectTableValue;
                    Totalcount = "0";
                    EnablePrint = false;
                    Opac = 0.1;
                }
            }
            catch (Exception ex)
            {
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in GetWLDataGridview -> {ex.Message}");
                MessageBox.Show("ERROR in GetWLDataGridview = " + ex.Message);
            }
        }


        private void ProccessData(DataTable wLDataTable)
        {
            
            List<Double> ListAvg = new List<double>();

            if (wLDataTable.Rows[0]["Layers"] == null)
                iLayerCount = DefaultLayersCount;
            else
                iLayerCount = Convert.ToInt32(wLDataTable.Rows[0]["Layers"].ToString());

            Totalcount = wLDataTable.Rows.Count.ToString();
            XColumns = iLayerCount + 1;
            DataTable DtaTable = SetUpDatTable(XColumns);

            //Array of double for each layers.
            List<double>[] fLayer = new List<double>[iLayerCount];

            SetNewChart();

            try
            {
                for (int y = 0; y < iLayerCount; y++)
                {
                    fLayer[y] = new List<double>();
                    for (int i = 0; i < wLDataTable.Rows.Count; i++)
                    {
                        if (wLDataTable.Rows[i]["Layer" + (y + 1).ToString()].ToString() != String.Empty)
                        {
                            if (Settings.Default.MoistureUnit == 0)
                                fLayer[y].Add(wLDataTable.Rows[i].Field<Double>("Layer" + (y + 1)));
                            if (Settings.Default.MoistureUnit == 1)
                                fLayer[y].Add(ConvToMR(wLDataTable.Rows[i].Field<Double>("Layer" + (y + 1))));
                        }
                    }
                }

                if (Settings.Default.MoistureUnit == 0)
                    DtaTable.Rows.Add("MC.%");
                else if (Settings.Default.MoistureUnit == 1)
                    DtaTable.Rows.Add("MR.%");

                for (int i = 1; i < iLayerCount + 1; i++)
                {
                    if (fLayer[i - 1].Count > 1)
                    {
                        Pos1.Add(new ChartData() { Index = i, Value = fLayer[i - 1].Average() });
                        ListAvg.Add(fLayer[i - 1].Average());
                        DtaTable.Rows[0][i] = fLayer[i - 1].Average().ToString("#0.00");
                    }
                }

                LayerAvg = ListAvg.Average().ToString("#0.00");
                LayerMax = ListAvg.Max().ToString("#0.00");
                LayerMin = ListAvg.Min().ToString("#0.00");

                MaximumHeight = ListAvg.Max() + 0.5;
                MinimumHeight = ListAvg.Min() - 0.5;

                for (int i = 0; i < iLayerCount + 2; i++)
                {
                    Pos2.Add(new ChartData() { Index = i, Value = ListAvg.Average() });
                    Pos3.Add(new ChartData() { Index = i, Value = ListAvg.Max() });
                    Pos4.Add(new ChartData() { Index = i, Value = ListAvg.Min() });
                }

                if (Settings.Default.MoistureUnit == 0)
                    DtaTable.Rows.Add("Avg.- MC.%");
                else if (Settings.Default.MoistureUnit == 1)
                    DtaTable.Rows.Add("Avg.- MR.%");

                for (int i = 1; i < iLayerCount + 1; i++)
                {
                    if (fLayer[i - 1].Count > 1)
                    {
                        DtaTable.Rows[1][i] = (fLayer[i - 1].Average() - ListAvg.Average()).ToString("#0.00");
                    }
                }

                WetLayerDeltaTable = DtaTable;
            }
            catch (Exception ex)
            {
               ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in ProccessData -> {ex.Message}");
                MessageBox.Show("ERROR in ProccessData = " + ex.Message);
            }
        }


        private DataTable SetUpDatTable(int LayersColumn)
        {
            DataTable NewTable = new DataTable();
            DataColumn[] DatColumn = new DataColumn[LayersColumn];

            DatColumn[0] = new DataColumn("Type", typeof(string));
            NewTable.Columns.Add(DatColumn[0]);

            for (int i = 1; i < LayersColumn; i++)
            {
                DatColumn[i] = new DataColumn("Layer" + i.ToString(), typeof(double));
                NewTable.Columns.Add(DatColumn[i]);
                //Console.WriteLine("DatColumn[i] = " + DatColumn[i] + " i= " + i);
            }
            return NewTable;
        }

        private bool ClosedPageCanExecute()
        {
            return true;
        }

        private void ClosedPageExecute()
        {
            if (cWetLayerModel != null)
                cWetLayerModel = null;
        }

        private bool LoadedPageCanExecute()
        {
            return true;
        }

        private void LoadedPageExecute()
        {
            cWetLayerModel = new WetLayerModel();

            try
            {
                WLMonthTableList = cWetLayerModel.GetWLMonthList();

                BalerList = cWetLayerModel.GetBalerList(SelectTableValue);
                if (BalerList.Count > 1)
                {
                    BalerList.Add("ALL");
                    SelectBalerValue = "ALL";
                }
                else
                    SelectBalerIndex = 0;

                if (Settings.Default.MoistureUnit == 0)
                    MoistureLabel = "Layers MC% Data";
                if (Settings.Default.MoistureUnit == 1)
                    MoistureLabel = "Layers MR% Data";

                if (WLMonthTableList.Count > 0)
                {
                    SelectTableIndex = 0;
                    SelectOCRIndex = 0;
                    EnablePrint = false;
                    Opac = 0.1;
                    MonthListEnable = true;
                }
                else
                    MonthListEnable = false;

                BalerCheck = false;

                MainWindow.AppWindows.SetupAppTitle("Forté Wetlayer Trend View From  -> " + _sqlhandler.Host + @"\" + _sqlhandler.SqlInstance);
            }
            catch (Exception ex)
            {
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in LoadedPageExecute WetLayerTrend -> {ex.Message}");
                MessageBox.Show("ERROR IN LoadedPageExecute WetLayerTrend " + ex.Message);
            }
        }

        private void SetNewChart()
        {
            if (Settings.Default.MoistureUnit == 0)
                YAxixTitle = "Moisture Content %";
            else
                    if (Settings.Default.MoistureUnit == 0)
                        YAxixTitle = "Moisture Regain %";


            if (Pos1 != null) Pos1 = null;
            Pos1 = new ObservableCollection<ChartData>();

            if (Pos2 != null) Pos2 = null;
            Pos2 = new ObservableCollection<ChartData>();

            if (Pos3 != null) Pos3 = null;
            Pos3 = new ObservableCollection<ChartData>();

            if (Pos4 != null) Pos4 = null;
            Pos4 = new ObservableCollection<ChartData>();
        }

        private double ConvToMR(double fMoisture)
        {
            return (fMoisture / (1 - fMoisture / 100));
        }



    }
}
