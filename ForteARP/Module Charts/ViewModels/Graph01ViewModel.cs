using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForteARP.Model;
using System.Collections.ObjectModel;
using System.Data;
using ForteARP.Properties;
using System.Threading;
using ForteArg.Services;

namespace ForteARP.ViewModels
{
     public class Graph01ViewModel : BindableBase
     { 
        public DelegateCommand LoadedGraph1ICommand { get; set; }
        public DelegateCommand ClosedGraph1ICommand { get; set; }

        public DelegateCommand RedrawCommand { get; set; }

        private ClsGaph01Model Graph01Model;

        private readonly string selectedLot;
        private readonly DateTime datestart;
        private readonly DateTime dateEnd;
        private readonly string lotid;

        private ObservableCollection<KeyValuePair<double, int>> _baleList;
        public ObservableCollection<KeyValuePair<double, int>> ItemsList
        {
            get { return _baleList; }
            set { SetProperty(ref _baleList, value); }
        }

        private ObservableCollection<KeyValuePair<double, int>> _averageList;
        public ObservableCollection<KeyValuePair<double, int>> ItemsAvg
        {
            get { return _averageList; }
            set { SetProperty(ref _averageList, value); }
        }

        private string _lotcharttitle;
        public string LotChartTitle
        {
            get { return _lotcharttitle; }
            set { SetProperty(ref _lotcharttitle, value); }
        }


        //Graph height lower
        private double _minimumheight;
        public double MinimumHeight
        {
            get { return _minimumheight; }
            set { SetProperty(ref _minimumheight, value); }
        }
        //Graph height Upper
        private double _maximumheight;
        public double MaximumHeight
        {
            get { return _maximumheight; }
            set { SetProperty(ref _maximumheight, value); }
        }


        private bool _menuOneChecked =true;
        public bool MenuOneChecked
        {
            get { return _menuOneChecked; }
            set
            {
                if(value)
                {
                     LotChartTitle = "Graph of " + ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Name + " From Bale Data of Lot # " + selectedLot;
                    
                     ItemUnit = ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Unit;
                     ItemLegend = ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Unit;
                     ProcessGraph("Moisture");
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
                if(value)
                {
                    LotChartTitle = "Graph of Weight in " + ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name + " From Bale Data of Lot # " + selectedLot;
                   
                    ItemUnit = ClsParams.WeightUnitList[Settings.Default.WeightUnit].Unit;
                    ItemLegend = "Weight";
                    ProcessGraph("Weight");
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
                if(value)
                {
                    LotChartTitle = "Graph of Forte Number from Bale Data  of Lot # " + selectedLot;
                    
                    ItemUnit = string.Empty;
                    ItemLegend = "Forte";
                    ProcessGraph("Forte");
                }
                SetProperty(ref _menuThreeChecked, value);
            }
        }

        public int iBaleCount = 0;
        
        private string _lowvalue;
        public string LowValue
        {
            get { return _lowvalue; }
            set { SetProperty(ref _lowvalue, value); }
        }
        private string _hiValue;
        public string HiValue
        {
            get { return _hiValue; }
            set { SetProperty(ref _hiValue, value); }
        }
        private string _AvgValue;
        public string AvgValue
        {
            get { return _AvgValue; }
            set { SetProperty(ref _AvgValue, value); }
        }
        private string _stdValue;
        public string STDValue
        {
            get { return _stdValue; }
            set { SetProperty(ref _stdValue, value); }
        }

        private string _ItemUnit;
        public string ItemUnit
        {
            get { return _ItemUnit; }
            set { SetProperty(ref _ItemUnit, value); }
        }


        private string _ItemLegend;
        public string ItemLegend
        {
            get { return _ItemLegend; }
            set { SetProperty(ref _ItemLegend, value); }
        }

        private string _TxtStatus;
        public string TxtStatus
        {
            get { return _TxtStatus; }
            set { SetProperty(ref _TxtStatus, value); }
        }

        private string _ArchiveTable;
        public string ArchiveTable
        {
            get { return _ArchiveTable; }
            set { SetProperty(ref _ArchiveTable, value); }
        }

        private int _BaleInLot;
        public int BaleInLot
        {
            get { return _BaleInLot; }
            set { SetProperty(ref _BaleInLot, value); }
        }

        //BaleInLot

        enum GpType :int
        {
            LotBaleNumber = 0,
            Weight = 1,
            Forte = 2 ,
            Moisture = 3,
            UpCount = 4,
            DownCount = 5,
            TimeComplete = 6,
        };

        List<string> GraphItems;


        public DataTable LotDatatable;
        private readonly string strmonth;

        /// <summary>
        /// class instantiation
        /// </summary>
        /// <param name="selectedLot"></param>
        /// <param name="datestart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="lotid"></param>
        public Graph01ViewModel(string selectedLot, DateTime datestart, DateTime dateEnd, string lotid, string strmonth)
        {
            this.selectedLot = selectedLot;
            this.datestart = datestart;
            this.dateEnd = dateEnd;
            this.lotid = lotid;
            this.strmonth = strmonth;

            LoadedGraph1ICommand = new DelegateCommand(LoadedGraph1Execute, LoadedGraph1CanExecute);
            RedrawCommand = new DelegateCommand(RedrawExecute, RedrawCanExecute);

          
        }

        private async Task SetupGaphicDataAsync()
        {
            GraphItems = new List<string>();
           

            string strQuery = string.Empty;
            char[] charsToTrim = { ',' }; 

            GraphItems.Add("LotBaleNumber");
            GraphItems.Add("Weight");
            GraphItems.Add("Moisture");
            GraphItems.Add("StockName");
            // GraphItems.Add("UpCount");
            // GraphItems.Add("DownCount");
            GraphItems.Add("TimeComplete");
            GraphItems.Add("UnitNumber");
            GraphItems.Add("Forte");
            GraphItems.Add("[index]");
            //GraphItems.Add("FC_LotIdentString");
           
            foreach (var item in GraphItems)
            {
                strQuery += item + ","; 
            }

            LotDatatable = await Graph01Model.GetTableByLotNumAsync(selectedLot, strQuery.TrimEnd(charsToTrim), datestart, dateEnd, lotid, this.strmonth);
            MenuOneChecked = true;

            if (LotDatatable.Rows.Count > 0)
                ArchiveTable = this.strmonth;
            else
                ArchiveTable = string.Empty;

            BaleInLot = LotDatatable.Rows.Count;

        }

        private void ProcessGraph(string ind)
        {
            TxtStatus = "No Bale Found";


            if (LotDatatable.Rows.Count > 0)
            {
          
                if (ItemsList != null) ItemsList = null;
                if (ItemsAvg != null) ItemsAvg = null;

                ItemsList = new ObservableCollection<KeyValuePair<double, int>>();
                ItemsAvg = new ObservableCollection<KeyValuePair<double, int>>();

                double Dvalue = 0.0;
                double Coef = 1;
                List<double> ItemList = new List<double>();
                int i = 1;
                double sumOfDerivation = 0;

                if (ind == "Weight")
                {
                    if (Settings.Default.WeightUnit == 0)
                        Coef = 1; //Kg
                    else
                        Coef = 2.20462; //Lb.
                }

                foreach (DataRow Item in LotDatatable.Rows)
                {
                    if (ind == "Moisture")
                        Dvalue = GetMoisture(Item.Field<Single>(ind));
                    else if (ind == "Weight")
                        Dvalue = Item.Field<Single>(ind) * Coef;
                    else //Forte#
                        Dvalue = Item.Field<int>(ind);

                    ItemsList.Add(new KeyValuePair<double, int>(Dvalue, i));
                    i += 1;
                    ItemList.Add(Dvalue);
                }

                HiValue = ItemList.Max().ToString("#0.00");
                LowValue = ItemList.Min().ToString("#0.00");
                AvgValue = ItemList.Average().ToString("#0.00");

                //Calculate STD
                foreach (var Value in ItemList)
                {
                    sumOfDerivation += (Value - Convert.ToDouble(AvgValue)) * (Value - Convert.ToDouble(AvgValue));
                }
                double Variance = sumOfDerivation / (ItemList.Count - 1);

                STDValue = Math.Sqrt(Variance).ToString("#0.00");

                i = 0;
                foreach (DataRow Item in LotDatatable.Rows)
                {
                    ItemsAvg.Add(new KeyValuePair<double, int>( Convert.ToDouble( AvgValue), i));
                    i += 1;
                }
                TxtStatus = "Bales in this Lot = " + LotDatatable.Rows.Count;
            }
        }

        private bool RedrawCanExecute()
        {
            return true;
        }

        private void RedrawExecute()
        {
            //throw new NotImplementedException();
        }

        private bool LoadedGraph1CanExecute()
        {
            return true;
        }

        private void LoadedGraph1Execute()
        {
            Graph01Model = new ClsGaph01Model();
            _ = SetupGaphicDataAsync();
        }

        private double GetMoisture(double dVal)
        {
            double dout = dVal;

            switch (Settings.Default.MoistureUnit)
            {
                case 0: // %MC == moisture from Sql database
                    dout = dVal;
                    break;

                case 1: // %MR  = Moisture / ( 1- Moisture / 100)
                    dout = dVal / (1 - dVal / 100);
                    break;

                case 2: // %AD = (100 - moisture) / 0.9
                    dout = (100 - dVal) / 0.9;
                    break;

                case 3: // %BD  = 100 - moisture
                    dout = 100 - dVal;
                    break;
            }
            return dout;
        }
    }
}
