using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForteARP.Model;
using System.Collections.ObjectModel;
using System.Data;
using ForteARP.Properties;
using Prism.Mvvm;
using Prism.Commands;

namespace ForteARP.ViewModels
{
    class Graph02ViewModel : BindableBase
    {
        public DelegateCommand LoadedGraphICommand { get; set; }
        public DelegateCommand RedrawCommand { get; set; }

        public DelegateCommand WriteCVCommand { get; set; }

       

        private ObservableCollection<KeyValuePair<double, int>> _baleList;
        public ObservableCollection<KeyValuePair<double, int>> ItemsList
        {
            get { return _baleList; }
            set { SetProperty(ref _baleList, value); }
        }

        private ObservableCollection<KeyValuePair<double, int>> _averageList;
        //private DataTable wetLayerDataTable;
        private readonly List<Tuple<long, string, double>> wetLayerDataList;

        public ObservableCollection<KeyValuePair<double, int>> ItemsAvg
        {
            get { return _averageList; }
            set { SetProperty(ref _averageList, value); }
        }


        public Graph02ViewModel(List<Tuple<long, string, double>> wetLayerDataList)
        {
            this.wetLayerDataList = wetLayerDataList;

            LoadedGraphICommand = new DelegateCommand(LoadedGraphExecute, LoadedGraphCanExecute);
            WriteCVCommand = new DelegateCommand(WriteCVExecute, WriteCVCanExecute);
            ShowGraph(this.wetLayerDataList);
        }


        private double _daverage;
        public double Daverage
        {
            get { return _daverage; }
            set { SetProperty(ref _daverage, value); }
        }

        public string AverageVal { get; set; }
        public string StdValue { get; set; }


        private string _dmax;
        public string DMax
        {
            get { return _dmax; }
            set { SetProperty(ref _dmax, value); }
        }

        private string _dmin;
        public string DMin
        {
            get { return _dmin; }
            set { SetProperty(ref _dmin, value); }
        }

        private string _TxtStatus;
        public string TxtStatus
        {
            get { return _TxtStatus; }
            set { SetProperty(ref _TxtStatus, value); }
        }

        //CVDataTable

        private DataTable _CVDataTable;
        public DataTable CVDataTable
        {
            get { return _CVDataTable; }
            set { SetProperty(ref _CVDataTable, value); }
        }


        private void CreateNewDatatable(List<Tuple<long, string, double>> DataList)
        {
            
            CVDataTable = new DataTable();

            // Declare DataColumn and DataRow variables.
            DataColumn column;

            // Create new DataColumn, set DataType, ColumnName and add to DataTable.    
            column = new DataColumn
            {
                DataType = System.Type.GetType("System.Int32"),
                ColumnName = "Number"
            };
            CVDataTable.Columns.Add(column);

            // Create second column.
            column = new DataColumn
            {
                DataType = System.Type.GetType("System.String"),
                ColumnName = "ReadTime"
            };
            CVDataTable.Columns.Add(column);

            // Create third column.
            column = new DataColumn
            {
                DataType = System.Type.GetType("System.String"),
                ColumnName = "CV Value"
            };
            CVDataTable.Columns.Add(column);

            for (int i = 0; i < DataList.Count; i++)
            {
                DataRow row = CVDataTable.NewRow();
                CVDataTable.Rows.InsertAt(row, i);
                row["Number"] = DataList[i].Item1;
                row["ReadTime"] = DataList[i].Item2;
                row["CV Value"] = DataList[i].Item3.ToString("#0.00");
            }
        }

        private void ShowGraph(List<Tuple<long, string, double>> DataListx)
        {
            List<double> DataList = new List<double>();
            
            for( int i =0; i < DataListx.Count; i++)
            {
                DataList.Add(DataListx[i].Item3);
            }

            if (DataList.Count > 1)
            {
                ItemsList = new ObservableCollection<KeyValuePair<double, int>>();
                _averageList = new ObservableCollection<KeyValuePair<double, int>>();

                Daverage = DataList.Average();
                AverageVal = Daverage.ToString("#0.00");
                DMax = DataList.Max().ToString("#0.00");
                DMin = DataList.Min().ToString("#0.00");
                int i = 1;
                double sumOfDerivation = 0;

                foreach (var item in DataList)
                {
                    ItemsList.Add(new KeyValuePair<double, int>(item, i));
                    _averageList.Add(new KeyValuePair<double, int>(Daverage, i));
                    
                    i += 1;
                }

                //Calculate STD
                foreach (var Value in DataList)
                {
                    sumOfDerivation += (Value - Convert.ToDouble(Daverage)) * (Value - Convert.ToDouble(Daverage));
                }
                double Variance = sumOfDerivation / (DataList.Count - 1);
                StdValue = Math.Sqrt(Variance).ToString("#0.00");

                TxtStatus = "Coefficient of Variation (CV) Graph from " + DataList.Count.ToString() + " Bales";
            }
            else
                TxtStatus = "No Data found, canot create Graph";

            CreateNewDatatable(DataListx);
        }


        private bool WriteCVCanExecute()
        {
            return true;
        }

        private void WriteCVExecute()
        {
            //throw new NotImplementedException();
        }

        private bool LoadedGraphCanExecute()
        {
            return true;
        }

        private void LoadedGraphExecute()
        {
            ClsGaph01Model Graph02Model = new ClsGaph01Model();
        }
    }
}
