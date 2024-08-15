using ForteArg.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ForteARP.Module_Combine.Model
{
    internal class CombineModel
    {
        private Sqlhandler _sqlhandler;
        private readonly ClsXml MyXml;

        public DataTable HdrTable;
        private string CurrentBaleTable { get; set; }

        private string m_strQueryString = string.Empty;

        public int m_Line = 0;
        public int m_Source = 0;
        public List<string> HdrDropDownList = new List<string>();

        /// <summary>
        ///  HML Data stored here ////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private string _XmlGdvDirectory;
        public string XMLRealTimeGdvFile
        {
            get { return _XmlGdvDirectory + Path.Combine(SettingsDirectory, "GdvRealtimeList.xml"); }
            set
            {
                if (value != null)
                    _XmlGdvDirectory = value;
            }
        }
        public string SettingsDirectory
        { get { return System.AppDomain.CurrentDomain.BaseDirectory; } } // Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ForteData"); } }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public struct CALC_RESULTS
        {
            public long BaleID;
            public int iBalerID;
            public string strBaler; //*10
            public double dDeviation;
            public double dAverage;
            public double dMaxValue;
            public double dMinValue;
            public int iNumbOfSpots;
            public string strResult; //*10
            public int[] iVals;
            public int iSize;
            public double[] dCalcResults;
            public List<double> dLayers;
            public int iLayers;
            public double dMoisture;
            public bool bAlarm;
            public bool bTCStampsAssigned;
        };


        public CombineModel()
        {
            _sqlhandler = Sqlhandler.Instance;

            MyXml = MainWindow.AppWindows.ClsXML;

            HdrTable = new DataTable();
            HdrTable = _sqlhandler.GetSqlScema();

            AvailableItemList = new ObservableCollection<CheckedListItem>();
        }
        internal int GetLineCount()
        {
            return _sqlhandler.iLineCount;
        }
        internal int GetSourceCount()
        {
            return _sqlhandler.iSourceCount;
        }
        internal long GetNewIndex()
        {
            return _sqlhandler.GetIndexNumber();
        }

        public ObservableCollection<CheckedListItem> AvailableItemList { get; set; }
        public List<string> XmlColumnList = new List<string>();


        internal ObservableCollection<string> GetXmlSelectedHdrCheckedList()
        {
            ObservableCollection<string> XmlCheckedList = new ObservableCollection<string>();

            HdrTable = new DataTable();
            XmlColumnList.Clear();
            AvailableItemList.Clear();

            try
            {
                HdrTable = _sqlhandler.GetSqlScema();
                XmlColumnList = GetXmlcolumnList(XMLRealTimeGdvFile);

                if ((HdrTable.Rows.Count > 0) && (XmlColumnList.Count > 0))
                {
                    foreach (DataRow item in HdrTable.Rows)
                    {
                        if (AllowField(item[1].ToString()))
                        {
                            if (XmlColumnList.Contains(item[1].ToString()))
                                AvailableItemList.Add(new CheckedListItem(Convert.ToInt32(item[0]), item[1].ToString(), true, item[2].ToString()));
                            else
                                AvailableItemList.Add(new CheckedListItem(Convert.ToInt32(item[0]), item[1].ToString(), false, item[2].ToString()));
                        }
                    }
                    foreach (var item in XmlColumnList)
                    {
                        XmlCheckedList.Add(item);
                    }
                }
                return XmlCheckedList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in ClsBaleRealTimeModel GetXmlSelectedHdrCheckedList" + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in ClsBaleRealTimeModel GetXmlSelectedHdrCheckedList -> {ex.Message}");
            }
            return XmlCheckedList;
        }

        private bool AllowField(string strItem)
        {
            foreach (var item in _sqlhandler.RemoveFieldsList)
            {
                if (item == strItem) return false;
            }
            return true;
        }

        private List<string> GetXmlcolumnList(string xMLDropsGdvFile)
        {
            return MyXml.ReadXmlGridView(xMLDropsGdvFile);
        }

        /// <summary>
        /// Get new data from SQL table here!
        /// </summary>
        /// <param name="iSample"></param>
        /// <returns></returns>
        public async Task<DataTable> GetNewGraphDataAsync(int iSample)
        {
            DataTable RTDataTable = new DataTable();
            RTDataTable.Clear();
            //string m_strQueryString = string.Empty;

            try
            {
                await Task.Run(async () =>
                {
                    string m_strQueryString = _sqlhandler.BuildQueryString(iSample);
                    RTDataTable = await _sqlhandler.GetForteDataTableAsync(m_strQueryString);

                    DataColumn NewCol = RTDataTable.Columns.Add("ADWeight", typeof(Single));
                    for (int i = 0; i < RTDataTable.Rows.Count; i++)
                    {
                        if ((RTDataTable.Rows[i]["BDWeight"] != null) && (RTDataTable.Rows[i].Field<Single>("BDWeight") > 0))
                            RTDataTable.Rows[i]["ADWeight"] = RTDataTable.Rows[i].Field<Single>("BDWeight") / 0.9;
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in GetDatatableAsync " + ex.Message);
            }
            return RTDataTable;
        }
        internal void GetLineSource()
        {
            m_Line = _sqlhandler.iLineCount;
            m_Source = _sqlhandler.iSourceCount;
        }
        internal void InitSqlRealTimeGraphModel()
        {
            _sqlhandler.SetupWorkStation();
        }

        private void CalCVMinMax(List<double> SampleList, int iLayers, out CALC_RESULTS tResults)
        {
            tResults = new CALC_RESULTS();
            double sumOfDerivation = 0;

            //Average
            tResults.dAverage = SampleList.Average();

            //Min Max
            tResults.dMinValue = SampleList.Min();
            tResults.dMaxValue = SampleList.Max();

            //MaxYAxis = SampleList.Max() + 5;

            //layers
            tResults.dLayers = new List<Double>();
            tResults.dLayers = SampleList;

            //Deviation
            tResults.bAlarm = false;
            foreach (var value in SampleList)
            {
                sumOfDerivation += (value - tResults.dAverage) * (value - tResults.dAverage);
            }

            //STD
            double Variance = sumOfDerivation / (SampleList.Count - 1);
            double StandardDeviation = Math.Sqrt(Variance);

            //%CV (Coefficient of Variation = Standard Deviation / Mean)
            tResults.dDeviation = (StandardDeviation / tResults.dAverage) * 100;
            tResults.bAlarm = false;
        }

        internal ObservableCollection<string> GetSelectHrdCheckList()
        {
            ObservableCollection<string> XmlCheckedList = new ObservableCollection<string>();
            AvailableItemList.Clear();

            HdrTable = new DataTable();
            HdrTable = _sqlhandler.GetSqlScema();
            XmlColumnList.Clear();


            DataRow workRow;

            workRow = HdrTable.NewRow();
            workRow[0] = HdrTable.Rows.Count + 1;
            workRow[1] = "ADWeight";
            workRow[2] = typeof(Single);
            HdrTable.Rows.Add(workRow);

            workRow = HdrTable.NewRow();
            workRow[0] = HdrTable.Rows.Count + 1;
            workRow[1] = "BoneDry%";
            workRow[2] = typeof(Single);
            HdrTable.Rows.Add(workRow);

            workRow = HdrTable.NewRow();
            workRow[0] = HdrTable.Rows.Count + 1;
            workRow[1] = "AirDry%";
            workRow[2] = typeof(Single);
            HdrTable.Rows.Add(workRow);

            workRow = HdrTable.NewRow();
            workRow[0] = HdrTable.Rows.Count + 1;
            workRow[1] = "Dirt_mm2/kg2";
            workRow[2] = typeof(Single);
            HdrTable.Rows.Add(workRow);

            workRow = HdrTable.NewRow();
            workRow[0] = HdrTable.Rows.Count + 1;
            workRow[1] = "Regain%";
            workRow[2] = typeof(Single);
            HdrTable.Rows.Add(workRow);


            XmlColumnList = GetXmlcolumnList(XMLRealTimeGdvFile);

            try
            {
                foreach (DataRow item in HdrTable.Rows)
                {
                    if (AllowField(item[1].ToString()))
                    {
                        if (XmlColumnList.Contains(item[1].ToString()))
                            AvailableItemList.Add(new CheckedListItem(Convert.ToInt32(item[0]), item[1].ToString(), true, item[2].ToString()));
                        else
                            AvailableItemList.Add(new CheckedListItem(Convert.ToInt32(item[0]), item[1].ToString(), false, item[2].ToString()));
                    }
                }

                AvailableItemList = new ObservableCollection<CheckedListItem>(AvailableItemList.OrderBy(x => x.Name)); //Sort

                foreach (var item in XmlColumnList)
                {
                    XmlCheckedList.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in ClsBaleRealTimeModel GetSelectHrdCheckList" + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in ClsBaleRealTimeModel GetSelectHrdCheckList -> {ex.Message}");
            }

            return XmlCheckedList;
        }

        internal void SaveModified_setting()
        {
            List<CheckedListItem> CustomHdrList = new List<CheckedListItem>();
            //  List<CheckedListItem> UpdateCustomHdrList = new List<CheckedListItem>();

            foreach (var item in AvailableItemList)
            {
                if (item.IsChecked)
                    CustomHdrList.Add(new CheckedListItem(item.Id, item.Name, item.IsChecked, item.FieldType));
            }
            MyXml.WriteXmlGridView(CustomHdrList, MyXml.XMLGdvFilePath);

            SetUpdate_DropDownList();
            CustomHdrList.Clear();
            CustomHdrList = null;
        }

        private void SetUpdate_DropDownList()
        {
            HdrDropDownList = GetHdrDropDownlist();
           // m_strQueryString = BuildQueryString(HdrDropDownList);
        }

        private string BuildQueryString(List<string> HeaderList)
        {
            string strQ = string.Empty;
            string strList = string.Empty;
            char charsToTrim = ',';

            HeaderList.Remove("-Blank-");

            CurrentBaleTable = _sqlhandler.GetCurrentBaleTable(); //if month changes

            strQ = "SELECT TOP 12 " + strList.Trim(charsToTrim) + ",[index] FROM dbo.[" + CurrentBaleTable + "] with (NOLOCK) ORDER BY [index] DESC";

            return strQ;
        }

        private List<string> GetHdrDropDownlist()
        {
            List<string> mylist = new List<string>();

            XmlColumnList = MyXml.ReadXmlGridView(MyXml.XMLGdvFilePath);// GetXmlColumnList();

            if (XmlColumnList.Count > 0)
            {
                foreach (var item in XmlColumnList)
                    mylist.Add(item);
            }
            else
            {
                mylist.Add("LotBaleNumber");
                mylist.Add("LotNumber");
                mylist.Add("Weight");
                mylist.Add("Moisture");
                mylist.Add("Forte");
                mylist.Add("TimeComplete");
                mylist.Add("SerialNumber");
                mylist.Add("StockName");
            }

            //[SourceName]
            //[LineName]
            //[StockName]
            //[CalibrationName]
            //[GradeName]
            //[Weight]
            //[Forte]
            //[UpCount]
            //[DownCount]
            //[TimeStart]
            //[TimeComplete]
            //[DropNumber]
            //[Position]
            //[Moisture]
            //[NetWeight]
            //[BDWeight]
            //[SerialNumber]
            //[LotNumber]
            //[BasisWeight]
            //[UnitNumber]

            //Added to get Bale Position for the bale animations
            if (!XmlColumnList.Contains("Position"))
                mylist.Add("Position");
            if (!XmlColumnList.Contains("ADWeight"))
                mylist.Add("ADWeight");

            //mylist.Add("-Blank-");

            return mylist;
        }

        internal void SaveXmlcolumnList(ObservableCollection<string> selectedHdrList)
        {
            MyXml.UpdateXMlcolumnList(selectedHdrList, XMLRealTimeGdvFile);
        }

        internal ObservableCollection<string> RemoveHdrItem(ObservableCollection<string> orgList, string Removeitem)
        {
            //ObservableCollection<string> tempList = new ObservableCollection<string>();
            ObservableCollection<string> tempList = orgList;
            tempList.Remove(Removeitem);
            return tempList;

        }

        internal ObservableCollection<string> AddHdrItem(ObservableCollection<string> orgList, string NewItem)
        {
            //ObservableCollection<string> tempList = new ObservableCollection<string>();
            ObservableCollection<string> tempList = orgList;
            tempList.Add(NewItem);
            return tempList;
        }


    }
}
