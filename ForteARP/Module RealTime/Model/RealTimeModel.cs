

using ForteArg.Services;
using ForteARP.Model;
using ForteARP.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ForteARP.Module_RealTime.Model
{
    /// <summary>
    /// Bale Realtime procedure here!
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(false)]
    [System.Serializable]
    public class RealTimeModel
    {
        public DataTable HdrTable;
        //public DataTable RealTimeDataTable { get; set; }
        public int m_iMoistureType;
        public int m_iWeightType;
        public ObservableCollection<CheckedListItem> AvailableItemList { get; set; }
     
        public ObservableCollection<string> SelectedHdrListModel = new ObservableCollection<string>();

        private Sqlhandler _sqlhandler;

        private readonly ClsXml MyXml;

        public List<string> HdrDropDownList = new List<string>();
        public ObservableCollection<string> selectedHdrList = new ObservableCollection<string>();

        public List<string> XmlColumnList = new List<string>();

        private string CurrentBaleTable { get; set; }
        private string m_strQueryString = string.Empty;
       

        private readonly int iLayerCount = 30;

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

        public RealTimeModel()
        {
            _sqlhandler = Sqlhandler.Instance;
          //  SqlConfigModel = MainWindow.AppWindows.MainSqlCfg;
          //  SqlConfigModel.SetupWorkStation();

            MyXml = MainWindow.AppWindows.ClsXML; 
            AvailableItemList = new ObservableCollection<CheckedListItem>();
        }

        internal void SetupWorkStation()
        {
           // SqlConfigModel.SetupWorkStation();

            CurrentBaleTable = _sqlhandler.GetCurrentBaleTable();
            m_iMoistureType = Settings.Default.MoistureUnit;
            m_iWeightType = Settings.Default.WeightUnit;

            SetUpdate_DropDownList();
        }

        private void SetUpdate_DropDownList()
        {
            HdrDropDownList = GetHdrDropDownlist();
            m_strQueryString = BuildQueryString(HdrDropDownList);
        }

        /// <summary>
        /// To Update Realtime Data
        /// </summary>
        /// <param name="ComboList"></param>
        /// <returns></returns>
        internal DataTable GetNewRealTimeTable(List<string> ComboList)
        {
            DataTable RealTimeDataTable = null;

      
            if (ComboList.Contains("ADWeight"))
                ComboList.Remove("ADWeight");
            if (ComboList.Contains("BoneDry%"))
                ComboList.Remove("BoneDry%");
            if (ComboList.Contains("Dirt_mm2/kg2"))
                ComboList.Remove("Dirt_mm2/kg2");
            if (ComboList.Contains("AirDry%"))
                ComboList.Remove("AirDry%");
            if (ComboList.Contains("Regain%"))
                ComboList.Remove("Regain%");

            for (int i = 0; i < ComboList.Count; i++)
            {
                if (ComboList[i].Contains("Moisture Content%"))
                    ComboList[i] = "Moisture";
            }
            //SpareSngFld3

            try
            {
                selectedHdrList = GetXmlSelectedHdrCheckedList();

                RealTimeDataTable = _sqlhandler.GetCurrentBaleSourceLine(Settings.Default.sRealTimeSource,
                    Settings.Default.sRealTimeLine, Settings.Default.iRealTimeBale, ComboList);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in RealTimeModel GetNewRealTimeData " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in GetNewRealTimeData -> {ex.Message}");
            }
            return RealTimeDataTable;
        }


        /// <summary>
        /// call from VariableViewModel()
        /// </summary>
        /// <param name="itemlist"></param>
        /// <returns></returns>
        internal DataTable GetNewUpdateDataTable(List<string> itemlist)
        {
            DataTable TempTable = new DataTable();

            if (itemlist.Contains("ADWeight"))
                itemlist.Remove("ADWeight");
            if (itemlist.Contains("BoneDry%"))
                itemlist.Remove("BoneDry%");
            if (itemlist.Contains("Dirt_mm2/kg2"))
                itemlist.Remove("Dirt_mm2/kg2");
            if (itemlist.Contains("AirDry%"))
                itemlist.Remove("AirDry%");
            if (itemlist.Contains("Regain%"))
                itemlist.Remove("Regain%");

            // if (itemlist.Contains("%CV"))
            //     itemlist.Remove("%CV");

            for (int i = 0; i < itemlist.Count; i++)
            {
                if (itemlist[i].Contains("Moisture Content%"))
                    itemlist[i] = "Moisture";

                if (itemlist[i].Contains("%CV"))
                    itemlist[i] = "SpareSngFld3";
            }

            try
            {
                m_strQueryString = BuildQueryfromItemList(itemlist);
                TempTable = _sqlhandler.GetSQLDataTable(m_strQueryString);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in  GetNewUpdateDataTable " + ex);
            }
            return TempTable;
        }

        private string BuildQueryfromItemList(List<string> itemlist)
        {
            char charsToTrim = ',';
            string strList = string.Empty;
            CurrentBaleTable = _sqlhandler.GetCurrentBaleTable();

            if (itemlist.Contains("ADWeight")) itemlist.Remove("ADWeight");

            for (int i = 0; i < itemlist.Count; i++)
                strList += itemlist[i] + ",";

            return $"SELECT TOP 1 {strList.Trim(charsToTrim)},[index]  " +
                $"FROM  dbo.[{CurrentBaleTable}]  with (NOLOCK) ORDER BY TimeComplete DESC ";
        }

        private DataTable ProcessPopupTable(DataTable mytable)
        {
            DataTable temptable = new DataTable();
            int iRowsCount = mytable.Rows.Count;
            List<double> fLayerVal;

            try
            {
                if (iRowsCount > 0)
                {
                    int iLayerset = 0;
                    CALC_RESULTS StructLast = new CALC_RESULTS();
                    fLayerVal = new List<double>();
                    //    mytable.Columns.Add("%CV", typeof(Double)).SetOrdinal(mytable.Columns["layer30"].Ordinal + 1);

                    for (int y = 0; y < iRowsCount; y++)
                    {
                        for (int i = 1; i < iLayerCount + 1; i++)
                        {
                            if (mytable.Rows[y]["layer" + i].ToString() != string.Empty)
                            {
                                iLayerset += 1;
                                fLayerVal.Add(ConvToMR(mytable.Rows[y].Field<Single>("layer" + i)));
                            }
                        }
                        CalCVMinMax(fLayerVal, iLayerset, out StructLast);
                        mytable.Rows[y]["%CV"] = StructLast.dDeviation.ToString("#0.00");
                        fLayerVal.Clear();
                    }
                }

                for (int i = 1; i < iLayerCount + 1; i++)
                {
                    if (mytable.Columns.Contains("layer" + i.ToString()))
                        mytable.Columns.Remove("layer" + i.ToString());
                }
                if (mytable.Columns.Contains("index"))
                    mytable.Columns.Remove("index");

                temptable = mytable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in ProcessPopupTable " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in ProcessPopupTable -> {ex.Message}");
            }
            return temptable;
        }


        /// <summary>
        /// GetList from XmlFile "GridviewItems.xml"
        /// there should be at least 8 items to fill 8 top screen Big Numbers
        /// </summary>
        /// <returns></returns>
        public List<string> GetHdrDropDownlist()
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


        private bool AllowField(string strItem)
        {
            foreach (var item in _sqlhandler.RemoveFieldsList)
            {
                if (item == strItem) return false;
            }
            return true;
        }

        public void UpdateQueryString(ObservableCollection<string> HeaderList)
        {
            string strQ = string.Empty;
            string strList = string.Empty;
            char charsToTrim = ',';

            CurrentBaleTable = _sqlhandler.GetCurrentBaleTable(); //if month changes

            if (HeaderList.Count > 0)
            {
                foreach (var Item in HeaderList)
                {
                    strList += Item + ",";
                }
                strQ = "SELECT TOP 12 " + strList.Trim(charsToTrim) + ",[index] FROM dbo.[" + CurrentBaleTable + "] with (NOLOCK) ORDER BY [index] DESC";
            }
            m_strQueryString = strQ;
        }

        private string BuildQueryString(List<string> HeaderList)
        {
            string strQ = string.Empty;
            string strList = string.Empty;
            char charsToTrim = ',';

            HeaderList.Remove("-Blank-");

            CurrentBaleTable = _sqlhandler.GetCurrentBaleTable(); //if month changes

            /*
            if (ClassCommon.WLOptions)
            {
                string m_CurrentWlTable = _sqlhandler.GetCurrentWLTable();

                if (HeaderList.Count > 0)
                {
                    foreach (var Item in HeaderList)
                    {
                        if (Item == "%CV")
                        {
                            for (int i = 1; i < iLayerCount + 1; i++)
                            {
                                strList += m_CurrentWlTable + ".Layer" + i.ToString() + ","; //Layer1
                            }
                        }
                        else
                            strList += m_CurrentBaleTable + "." + Item + ",";
                    }

                    if (HeaderList.Contains("%CV"))
                    {
                        strQ = "SELECT TOP 12 " + strList.Trim(charsToTrim)
                        + ",[index] FROM [ForteData].[dbo].[" + m_CurrentBaleTable + "]  INNER JOIN [ForteLayer].[dbo].["
                        + m_CurrentWlTable + "] ON " + m_CurrentBaleTable + ".TimeStart = " + m_CurrentWlTable + ".ReadTime ORDER BY [TimeStart] DESC";
                    }
                    else
                        strQ = "SELECT TOP 12 " + strList.Trim(charsToTrim) + ",[index] FROM dbo.[" + m_CurrentBaleTable + "] ORDER BY [index] DESC";
                }

            }
            else*/
            strQ = "SELECT TOP 12 " + strList.Trim(charsToTrim) + ",[index] FROM dbo.[" + CurrentBaleTable + "] with (NOLOCK) ORDER BY [index] DESC";

            return strQ;
        }

        internal List<string> Getlinelist()
        {
            return _sqlhandler.LineList;
        }

        internal List<string> Getsourcelist()
        {
            return _sqlhandler.SourceList;// _sqlhandler.getSourceList();
        }

        public void SaveModified_setting()
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

        internal long GetNewIndex()
        {
            return _sqlhandler.GetIndexNumber();
        }

        private List<string> GetXmlcolumnList(string xMLDropsGdvFile)
        {
            return MyXml.ReadXmlGridView(xMLDropsGdvFile);
        }

        internal List<string> GetHdrList()
        {
            List<string> hdrList = new List<string>();
            DataTable tempTable = _sqlhandler.GetSqlScema();

            foreach (DataRow item in tempTable.Rows)
            {
                hdrList.Add(item[1].ToString());
            }

            if (hdrList.Contains("ForteStatus"))
                hdrList.Remove("ForteStatus");

            //if (hdrList.Contains("UpCount"))
            //    hdrList.Remove("UpCount");

            // if (hdrList.Contains("DownCount"))
            //     hdrList.Remove("DownCount");

            if (hdrList.Contains("SpareSngFld1"))
                hdrList.Remove("SpareSngFld1");

            if (hdrList.Contains("SpareSngFld2"))
                hdrList.Remove("SpareSngFld2");

            /*
             if (hdrList.Contains("SpareSngFld3"))
             {
                 hdrList.Remove("SpareSngFld3");
             //    hdrList.Add("%CV");
             }
             */

            if (hdrList.Contains("AsciiFld1"))
                hdrList.Remove("AsciiFld1");

            if (hdrList.Contains("AsciiFld2"))
                hdrList.Remove("AsciiFld2");

            // if (hdrList.Contains("FC_LotIdentString"))
            //     hdrList.Remove("FC_LotIdentString");

            for (int i = 0; i < hdrList.Count; i++)
            {
                if (hdrList[i].Contains("Finish"))
                    hdrList[i] = "Viscosity";

                if (hdrList[i].Contains("FC_LotIdentString"))
                    hdrList[i] = "CusLotNumber";

                if (hdrList[i].Contains("SpareSngFld3"))
                    hdrList[i] = "%CV";
            }

            for (int i = 0; i < hdrList.Count; i++)
            {
                if (hdrList[i].Contains("Moisture"))
                    hdrList[i] = "Moisture Content%";
            }
            hdrList.Add("ADWeight");
            hdrList.Add("BoneDry%");
            hdrList.Add("AirDry%");
            hdrList.Add("Dirt_mm2/kg2");
            hdrList.Add("Regain%");
            return hdrList;
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


            /*
            if (ClassCommon.WLOptions)
            {
                foreach (DataRow item in HdrTable.Rows)
                {
                    if (item[1].ToString() == "SpareSngFld3") item.Delete();
                }
                HdrTable.Rows.Add(new Object[] { 100, "%CV", "real" });
                HdrTable.AcceptChanges();
            }
            else
            {
                foreach (DataRow item in HdrTable.Rows)
                {
                    if (item[1].ToString() == "%CV") item.Delete();
                    if (item[1].ToString() == "SpareSngFld3") item.Delete();
                }
            }
            */

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

        internal ObservableCollection<string> RemoveHdrItem(ObservableCollection<string> orgList, string Removeitem)
        {
            //ObservableCollection<string> tempList = new ObservableCollection<string>();
            ObservableCollection<string>  tempList = orgList;
            tempList.Remove(Removeitem);
            return tempList;
        }

        internal ObservableCollection<string> AddHdrItem(ObservableCollection<string> orgList, string NewItem)
        {
            //ObservableCollection<string> tempList = new ObservableCollection<string>();
            ObservableCollection<string>  tempList = orgList;
            tempList.Add(NewItem);
            return tempList;
        }

        internal void SaveXmlcolumnList(ObservableCollection<string> selectedHdrList)
        {
            MyXml.UpdateXMlcolumnList(selectedHdrList, XMLRealTimeGdvFile);
        }

        private double ConvToMR(double fMoisture)
        {
            return (fMoisture / (1 - fMoisture / 100));
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
    }
}
