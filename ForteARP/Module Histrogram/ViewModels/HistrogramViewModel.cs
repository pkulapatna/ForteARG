

using ForteArg.Services;
using ForteARP.Module_Histrogram.Model;
using ForteARP.Properties;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace ForteARP.Module_Histrogram.ViewModels
{
    public class HistrogramViewModel : BindableBase
    {
        private Sqlhandler _sqlhandler = Sqlhandler.Instance;

        private HistrogramModel HistrogramModel;

        public DelegateCommand LoadedPageICommand { get; set; }
        public DelegateCommand UnLoadedPageICommand { get; set; }
        public DelegateCommand DrawCommand { get; set; }

        public bool DefFieldChecked { get; set; }
        public string StockSelected { get; set; }
        public string GradeSelected { get; set; }
        public string LineSelected { get; set; }
        public string SourceSelected { get; set; }
        public string LotSelected { get; set; }


        private string _graphTitle;
        public string GraphTitle
        {
            get { return _graphTitle; }
            set { SetProperty(ref _graphTitle, value); }
        }

        private ObservableCollection<HistChartData> _chartList;
        public ObservableCollection<HistChartData> ChartList
        {
            get { return _chartList; }
            set
            {
                SetProperty(ref _chartList, value);
            }
        }

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

        public List<string> ColorList
        {
            get { return GetColorsList(); ; }
        }

        private List<Brush> BrushList
        {
            get { return GetBrushList(); ; }
        }


        private List<string> _monthtableList;
        public List<string> MonthTableList
        {
            get { return _monthtableList; }
            set { SetProperty(ref _monthtableList, value); }
        }

        private List<string> _lotmonthtableList;
        public List<string> LotMonthTableList
        {
            get { return _lotmonthtableList; }
            set { SetProperty(ref _lotmonthtableList, value); }
        }

        private bool _bDataExcist;
        public bool BDataExcist
        {
            get { return _bDataExcist; }
            set { SetProperty(ref _bDataExcist, value); }
        }

        public bool MonthChecked { get; set; }

        private bool _daychecked;
        public bool DayChecked
        {
            get { return _daychecked; }
            set { SetProperty(ref _daychecked, value); }
        }

        private int _MaxColumn;
        public int MaxColumn
        {
            get { return _MaxColumn; }
            set { SetProperty(ref _MaxColumn, value); }
        }

        private Brush _ChartColor = Brushes.Red;
        public Brush ChartColor
        {
            get { return _ChartColor; }
            set { SetProperty(ref _ChartColor, value); }
        }

        public List<string> Occrlist
        {
            get { return GetFreqList(); }
        }

        private bool _occorChecked;
        public bool OccorChecked
        {
            get { return _occorChecked; }
            set { SetProperty(ref _occorChecked, value); }
        }

        private int _recCount;
        public int RecCount
        {
            get { return _recCount; }
            set { SetProperty(ref _recCount, value); }
        }

        private int _selectOccr;
        public int SelectOccr
        {
            get { return _selectOccr; }
            set { SetProperty(ref _selectOccr, value); }
        }

        private List<string> _stockList;
        public List<string> StockList
        {
            get { return _stockList; }
            set { SetProperty(ref _stockList, value); }
        }

        private List<string> _gradeList;
        public List<string> GradeList
        {
            get { return _gradeList; }
            set { SetProperty(ref _gradeList, value); }
        }

        private List<string> _lineList;
        public List<string> LineList
        {
            get { return _lineList; }
            set { SetProperty(ref _lineList, value); }
        }

        private List<string> _sourceList;
        public List<string> SourceList
        {
            get { return _sourceList; }
            set { SetProperty(ref _sourceList, value); }
        }

        private bool _stockChecked;
        public bool StockChecked
        {
            get { return _stockChecked; }
            set
            {
                SetProperty(ref _stockChecked, value);
                if (value)
                {
                    StockList = HistrogramModel.GetSqlStockList(SelectTableValue);
                }
                else
                    StockList.Clear();
            }
        }

        private bool _gradeChecked;
        public bool GradeChecked
        {
            get { return _gradeChecked; }
            set
            {
                SetProperty(ref _gradeChecked, value);
                GradeList = HistrogramModel.GetSqlGradeList(SelectTableValue);
            }
        }

        private bool _lineChecked;
        public bool LineChecked
        {
            get { return _lineChecked; }
            set
            {
                SetProperty(ref _lineChecked, value);
                LineList = HistrogramModel.GetSqlLineList(SelectTableValue);
            }
        }

        private bool _sourceChecked;
        public bool SourceChecked
        {
            get { return _sourceChecked; }
            set
            {
                SetProperty(ref _sourceChecked, value);
                SourceList = HistrogramModel.GetSqlSourceList(SelectTableValue);
            }
        }

        private Nullable<DateTime> _startDateProp = null;
        public Nullable<DateTime> StartDateProp
        {
            get
            {
                if (_startDateProp == null)
                    _startDateProp = DateTime.Today;

                return _startDateProp;
            }
            set { SetProperty(ref _startDateProp, value); }
        }

        private Nullable<DateTime> _endDateProp = null;
        public Nullable<DateTime> EndDateProp
        {
            get
            {
                if (_endDateProp == null)
                    _endDateProp = DateTime.Today;
                return _endDateProp;
            }
            set { SetProperty(ref _endDateProp, value); }
        }

        private string _Selectedmonth;
        public string SelectedMonth
        {
            get { return _Selectedmonth; }
            set { SetProperty(ref _Selectedmonth, value); }
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
            set
            {
                SetProperty(ref _selectTableValue, value);

                string strMonth = SelectTableValue.Substring(11, 3);
                string strYear = SelectTableValue.Substring(14, 2);
                SelectedMonth = strMonth + strYear;
                string startDate = "01" + "/" + strMonth + "/" + strYear;

                DateTime.TryParse(startDate, out DateTime dateValue);

                if (StockChecked) StockChecked = false;
                if (GradeChecked) GradeChecked = false;
                if (LineChecked) LineChecked = false;
                if (SourceChecked) SourceChecked = false;

                if (SelectTableIndex == 0)
                {
                    StartDateProp = dateValue;
                    EndDateProp = DateTime.Now;
                }
                else
                {
                    StartDateProp = dateValue;
                    EndDateProp = dateValue;
                }
            }
        }

        private string _MaxHigh = string.Empty;
        public string MaxHigh
        {
            get { return _MaxHigh; }
            set { SetProperty(ref _MaxHigh, value); }
        }
        private string _MinLow = string.Empty;
        public string MinLow
        {
            get { return _MinLow; }
            set { SetProperty(ref _MinLow, value); }
        }

        private int _highLimit;
        public int HighLimit
        {
            get { return _highLimit; }
            set
            {
                if ((value > 0) & (value < 1000))
                    SetProperty(ref _highLimit, value);
                else
                    SetProperty(ref _highLimit, 10);

                Settings.Default.HistHiLimits = value;
                Settings.Default.Save();
            }
        }

        private int _lowLimit;
        public int LowLimit
        {
            get { return _lowLimit; }
            set
            {
                if ((value > 0) & (value < 1000))
                    SetProperty(ref _lowLimit, value);
                else
                    SetProperty(ref _lowLimit, 1);

                SetProperty(ref _lowLimit, value);
                Settings.Default.HistLowLimits = value;
                Settings.Default.Save();
            }
        }

        private int _SampleSize;
        public int SampleSize
        {
            get { return _SampleSize; }
            set { SetProperty(ref _SampleSize, value); }
        }

        private string _TxtStatus = string.Empty;
        public string TxtStatus
        {
            get { return _TxtStatus; }
            set { SetProperty(ref _TxtStatus, value); }
        }

        private int _SelectHighIndex;
        public int SelectHighIndex
        {
            get { return _SelectHighIndex; }
            set { SetProperty(ref _SelectHighIndex, value); }
        }

        private int _SelectCHighIndex;
        public int SelectCHighIndex
        {
            get { return _SelectCHighIndex; }
            set { SetProperty(ref _SelectCHighIndex, value); }
        }

        private int _SelectNormIndex;
        public int SelectNormIndex
        {
            get { return _SelectNormIndex; }
            set { SetProperty(ref _SelectNormIndex, value); }
        }



        public HistrogramViewModel()
        {
            LoadedPageICommand = new DelegateCommand(LoadPageExecute, LoadPageCanExecute);
            UnLoadedPageICommand = new DelegateCommand(UnLoadPageExecute, UnLoadPageCanExecute);

            DrawCommand = new DelegateCommand(DrawExecute, DrawCanExecute);

            MonthChecked = true;
            OccorChecked = true;
            ClsSerilog.LogMessage(ClsSerilog.Info, $"Initialize Histrogram");
        }


        private void DrawExecute()
        {

            GraphHiColor = BrushList[SelectHighIndex];
            GraphNormColor = BrushList[SelectCHighIndex];
            GraphLowColor = BrushList[SelectNormIndex];

            Settings.Default.HisHiColorIndex = SelectHighIndex;
            Settings.Default.HisNorColorIndex = SelectNormIndex;
            Settings.Default.HisCHiColorIndex = SelectCHighIndex;
            Settings.Default.Save();

            try
            {
                string queryString = BuildQueryString(SelectTableValue);
                using (var Mytable = HistrogramModel.GetSqlBaleDataTable(queryString))
                {
                    SampleSize = Mytable.Rows.Count;
                    if (SampleSize > 0)
                    {
                        ProcessDatTable(Mytable);
                        TxtStatus = "Acquire data from - " + SelectTableValue;
                    }
                    else TxtStatus = "No Data from " + SelectTableValue;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in DrawExecute " + ex);
            }
        }

        /// <summary>
        /// Using Linq and Lambda Expression
        /// </summary>
        /// <param name="tempTable"></param>
        private void ProcessDatTable(DataTable tempTable)
        {
            double DForte = 0.00;
            double DWeight = 0.00;

            if (ChartList != null) ChartList = null;
            ChartList = new ObservableCollection<HistChartData>();
            ChartList.Clear();

            try
            {

                uint[] ditems = new uint[tempTable.Rows.Count];

                for (int i = 0; i < tempTable.Rows.Count; i++)
                {
                    DForte = Convert.ToDouble(tempTable.Rows[i]["Forte"]);
                    DWeight = tempTable.Rows[i].Field<Single>("Weight");

                    if (((DForte / DWeight) * 100 > 0) & ((DForte / DWeight) * 100 < 65536)) //uiint max = 65536
                        ditems[i] = Convert.ToUInt16((DForte / DWeight) * 100);
                }

                ditems
                    .GroupBy(i => i)
                    .Select(g => new {
                        Item = g.Key,
                        Count = g.Count()
                    })
                    .OrderBy(g => g.Item)
                    .ToList()
                    .ForEach(g => {
                        // Console.WriteLine("{0} occurred {1} times", g.Item, g.Count);
                        ChartList.Add(new HistChartData() { Index = g.Count, Value = g.Item, ChartColor = SetChartColors(g.Item) });
                    });

                MinLow = Convert.ToString(ditems.Min());
                MaxHigh = Convert.ToString(ditems.Max());

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in ProcessDatTable " + ex);
            }
        }

        private Brush SetChartColors(uint item)
        {
            Brush crtColor;

            if (Convert.ToInt32(item) > HighLimit) crtColor = GraphHiColor;
            else if (Convert.ToInt32(item) < LowLimit) crtColor = GraphLowColor;
            else crtColor = GraphNormColor;
            return crtColor;
        }

        private string BuildQueryString(string strTable)
        {
            string strClause;
            string strQuantity;
            string strQueryFields;
            string strOrder;
            string strsgls = string.Empty;
            string strTimeFrame;

            List<string> strBoxes = new List<string>();

            // 1 Occurrences 
            if (EventValue == "All")
                strQuantity = "SELECT ";
            else
                strQuantity = "SELECT TOP " + RecCount;

            // 2 Field Selections
            strQueryFields = "Moisture,Forte,Weight";

            // 3 from stock,grade,line or/and source
            if (StockChecked)
                strBoxes.Add(" StockName = '" + StockSelected + "'");
            if (GradeChecked)
                strBoxes.Add(" GradeName = '" + GradeSelected + "'");
            if (LineChecked)
                strBoxes.Add(" LineID = '" + LineSelected + "'");
            if (SourceChecked)
                strBoxes.Add(" SourceID = '" + SourceSelected + "'");


            if (strBoxes.Count > 0)
            {
                foreach (var item in strBoxes)
                {
                    strsgls = strsgls + item + " and ";
                }
                strsgls = " WHERE " + strsgls;
                strsgls = strsgls.Remove(strsgls.Length - 5);
            }

            //4 Old new or all
            if (EventValue == "Oldest")
                strOrder = " ORDER BY [Index] ASC;";
            else
                strOrder = " ORDER BY [Index] DESC;";

            if (DayChecked)
            {
                if (EndDateProp > DateTime.Now)
                    EndDateProp = DateTime.Now;

                string strStartDate = StartDateProp.Value.Date.ToString("MM/dd/yyyy");
                string strEndDate = EndDateProp.Value.Date.AddDays(1).ToString("MM/dd/yyyy");

                strTimeFrame = " WHERE CAST(TimeStart AS DATETIME) BETWEEN '" + strStartDate + "' and '" + strEndDate + "' ";
            }
            else
                strTimeFrame = string.Empty;

            strClause = strQuantity + " " + strQueryFields + " FROM " + strTable + strsgls + strTimeFrame + strOrder;

            return strClause;
        }

        private bool DrawCanExecute()
        {
            return true;
        }

        private void UnLoadPageExecute()
        {

        }

        private bool UnLoadPageCanExecute()
        {
            return true;
        }

        private void LoadPageExecute()
        {

            try
            {
                if (HistrogramModel != null) HistrogramModel = null;
                HistrogramModel = new HistrogramModel();
                HistrogramModel.InitSqlBaleArchiveModel();

                SelectHighIndex = Settings.Default.HisHiColorIndex;
                SelectNormIndex = Settings.Default.HisNorColorIndex;
                SelectCHighIndex = Settings.Default.HisCHiColorIndex;

                GraphHiColor = BrushList[SelectHighIndex];
                GraphNormColor = BrushList[SelectNormIndex];
                GraphLowColor = BrushList[SelectCHighIndex];


                MonthTableList = HistrogramModel.GetSqlTableList();
                if (MonthTableList.Count > 0)
                {
                    BDataExcist = true;
                    SelectTableIndex = 0;
                }
                else BDataExcist = false;
                SelectOccr = 0;

                RecCount = 200;

                LowLimit = Settings.Default.HistLowLimits;
                HighLimit = Settings.Default.HistHiLimits;

                GraphTitle = "Histogram of Forté Index (ForteNumber/Weight)";
                MainWindow.AppWindows.SetupAppTitle("histogram of Forté Index From  -> " + _sqlhandler.Host + @"\" + _sqlhandler.SqlInstance);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in LoadPageExecute HistrogramViewModel " + ex.Message);
            }
        }


        private List<string> GetFreqList()
        {
            List<string> templist = new List<string>
            {
                "Latest",
                "Oldest",
                "All"
            };
            return templist;
        }


        private List<string> GetColorsList()
        {
            List<string> CList = new List<string>();
            CList.Clear();
            CList.Add("BlanchedAlmond");
            CList.Add("Red");
            CList.Add("Yellow");
            CList.Add("Blue");
            CList.Add("Green");
            CList.Add("Brown");
            CList.Add("Gray");
            CList.Add("Puple");
            CList.Add("Pink");
            CList.Add("Orange");
            CList.Add("Olive");
            CList.Add("White");
            CList.Add("Beige");
            CList.Add("SlateGray");
            CList.Add("SpringGreen");
            CList.Add("LightGreen");
            CList.Add("LightSteelBlue");
            CList.Add("Salmon");
            CList.Add("Azure");
            CList.Add("Aqua");
            CList.Add("Aquamarine");
            return CList;
        }

        private List<Brush> GetBrushList()
        {
            List<Brush> BList = new List<Brush>();
            BList.Clear();
            BList.Add(Brushes.BlanchedAlmond);
            BList.Add(Brushes.Red);
            BList.Add(Brushes.Yellow);
            BList.Add(Brushes.Blue);
            BList.Add(Brushes.Green);
            BList.Add(Brushes.Brown);
            BList.Add(Brushes.Gray);
            BList.Add(Brushes.Purple);
            BList.Add(Brushes.Pink);
            BList.Add(Brushes.Orange);
            BList.Add(Brushes.Olive);
            BList.Add(Brushes.White);
            BList.Add(Brushes.Beige);
            BList.Add(Brushes.SlateGray);
            BList.Add(Brushes.SpringGreen);
            BList.Add(Brushes.LightGreen);
            BList.Add(Brushes.LightSteelBlue);
            BList.Add(Brushes.Salmon);
            BList.Add(Brushes.Azure);
            BList.Add(Brushes.Aqua);
            BList.Add(Brushes.Aquamarine);
            return BList;
        }
        private bool LoadPageCanExecute()
        {
            return true;
        }
    }

    /// <summary>
    /// Select different colors for Bar Graph
    /// </summary>
    public class HistChartData : BindableBase
    {
        private int _Index;
        public int Index
        {
            get { return _Index; }
            set { SetProperty(ref _Index, value); }
        }

        private uint _value;
        public uint Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        private Brush _color;
        public Brush ChartColor
        {
            get { return _color; }
            set { SetProperty(ref _color, value); }
        }
    }
}
