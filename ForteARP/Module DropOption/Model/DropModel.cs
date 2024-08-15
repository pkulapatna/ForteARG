

using ForteArg.Services;
using ForteARP.Model;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Windows;
using System.Xml;

namespace ForteARP.Module_DropOption.Model
{
    public class DropModel
    {
        private readonly ClsXml MyXml;
        private List<string> XmlColumnList = new List<string>();
        private List<string> HdrDropDownList = new List<string>();
        
        public DataTable HdrTable;

        private Sqlhandler _sqlhandler = Sqlhandler.Instance;

        public string m_strQueryString;

        public List<string> m_LineList;
        public List<string> m_SourceList;
        public string m_Line;
        public string m_Source;

        public int BaleinADrop = 0;

        public ObservableCollection<CheckedListItem> AvailableItemList { get; set; }
        public List<string> SelectedHdrListModel = new List<string>();

        public string CurrentBaleTable
        {
            get { return _sqlhandler.GetCurrentBaleTable(); }
        }

        internal void GetLineSource()
        {
          //  _sqlhandler.SetupWorkStation();

            m_LineList.Clear();
            m_LineList = _sqlhandler.LineList;
         
            m_SourceList.Clear();
            m_SourceList = _sqlhandler.SourceList;
            

        }

        internal bool AllowField(string strItem)
        {
            foreach (var item in _sqlhandler.RemoveFieldsList)
            {
                if (item == strItem) return false;
            }
            return true;
        }


        public DropModel()
        { 
            MyXml = MainWindow.AppWindows.ClsXML;
            AvailableItemList = new ObservableCollection<CheckedListItem>();

            m_LineList = new List<string>();
            m_SourceList = new List<string>();

        }

        public void SetUpSql()
        {
            _sqlhandler = Sqlhandler.Instance;
            _sqlhandler.SetSqlParams();
            _sqlhandler.SetConnectionString();
            _sqlhandler.SetupWorkStation();

            BaleinADrop = BalesinDrop();
        }


        internal int GetNewItemData(string strQueryString)
        {
            return _sqlhandler.GetIntNewItemData(strQueryString);
        }

        internal void InitSqlDropProfileModel()
        {
           // _sqlhandler.SetupWorkStation();
            SetUpdate_DropDownList();
        }

        internal DataTable GetNewDataTable(string strGetSingleNewData)
        {
            //return _sqlhandler.GetBaleDataTable(strGetSingleNewData);
            return _sqlhandler.GetSQLDataTable(strGetSingleNewData);

            //GetSQLDataTable
        }

        internal DataTable GetNewDropGaphDataTable(string strGetSingleNewData)
        {
            DataTable Mydatatable = _sqlhandler.GetForteDataTable(strGetSingleNewData);
            Mydatatable.Columns.Add("ADWeight", typeof(double));
            Mydatatable.AcceptChanges();
            for (int i = 0; i < Mydatatable.Rows.Count; i++)
            {
                if (Mydatatable.Rows[i]["BDWeight"] != null)
                    Mydatatable.Rows[i]["ADWeight"] = Mydatatable.Rows[i].Field<Single>("BDWeight") / 0.9;
            }
            return Mydatatable;
        }

        public int BalesinDrop()
        {
            return _sqlhandler.GetMaxValfromtable("Position", CurrentBaleTable);
        }

        internal List<string> GetXmlcolumnList(string SettingsGdvFile)
        {
            return MyXml.ReadXmlGridView(SettingsGdvFile);
        }

        internal List<int> GetXmlHdrList(string HdrFileLocation)
        {
            List<int> ReadXmlHdrList = new List<int>();
            XmlDocument doc = new XmlDocument();

            try
            {
                if (System.IO.File.Exists(HdrFileLocation))
                {
                    doc.Load(HdrFileLocation);
                    XmlNodeList xn2 = doc.SelectNodes("CustomHdr/Field/Value");
                    if ((xn2 != null) && (xn2.Count > 0))
                    {
                        foreach (XmlNode itemNode in xn2)
                        {
                            ReadXmlHdrList.Add(Convert.ToInt32(itemNode.InnerText));
                        }

                    }
                }
                return ReadXmlHdrList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Model GetXmlHdrList " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in GetXmlHdrList -> {ex.Message}");
            }
            return ReadXmlHdrList;
        }

        internal DataTable GetSqlScema()
        {
            return _sqlhandler.GetSqlScema();
        }


        /// <summary>
        /// HML Data stored here ////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private string _XmlGdvDirectory;
        public string XMLDropsGdvFile
        {
            get { return _XmlGdvDirectory + Path.Combine(SettingsDirectory, "GridviewItems.xml"); }
            set
            {
                if (value != null)
                    _XmlGdvDirectory = value;
            }
        }
        public string SettingsDirectory
        { get { return System.AppDomain.CurrentDomain.BaseDirectory; } } // Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ForteData"); } }return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ForteData"); } }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        internal void SaveXmlcolumnList(ObservableCollection<string> selectedHdrList)
        {
            MyXml.UpdateXMlcolumnList(selectedHdrList, XMLDropsGdvFile);
        }

        private void SetUpdate_DropDownList()
        {
            HdrDropDownList = GetHdrDropDownlist();
            m_strQueryString = BuildQueryString(HdrDropDownList);
        }


        private string BuildQueryString(List<string> HeaderList)
        {
            string strQ = string.Empty;
            string strList = string.Empty;
            char charsToTrim = ',';

            if (HeaderList.Count > 0)
            {
                foreach (var Item in HeaderList)
                {
                    strList += Item + ",";
                }
                strQ = "SELECT TOP 12 " + strList.Trim(charsToTrim) + ",[index] FROM dbo.[" + CurrentBaleTable + "] ORDER BY [index] DESC";
            }

            return strQ;
        }

        public List<string> GetHdrDropDownlist()
        {
            List<string> mylist = new List<string>();

            XmlColumnList = GetXmlColumnList();
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
            return mylist;
        }

        private List<string> GetXmlColumnList()
        {
            // XML ORDINAL_POSITION,COLUMN_NAME,DATA_TYPE 
            XmlColumnList.Clear();
            XmlColumnList = MyXml.ReadXmlGridView(XMLDropsGdvFile);
            return XmlColumnList;
        }

        internal List<string> GetSourceList()
        {
            return _sqlhandler.LineList; // GetDistinctitemlist(item);
        }

        internal List<string> GetListof(string item)
        {
            return _sqlhandler.GetDistinctitemlist(item);
        }


        internal ObservableCollection<string> GetXmlSelectedHdrCheckedList()
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

            XmlColumnList = GetXmlcolumnList(XMLDropsGdvFile);

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

                foreach (var item in XmlColumnList)
                {
                    XmlCheckedList.Add(item);
                }

                return XmlCheckedList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR2 in Model GetXmlSelectedHdrCheckedList" + ex.Message.ToString());
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in GetXmlSelectedHdrCheckedList -> {ex.Message}");
            }
            return XmlCheckedList;
        }

        internal string GetPreviousTable()
        {
            List<string> tList = _sqlhandler.GettableList(Sqlhandler.BALE_ARCHIVE);
            if (tList.Count > 1)
                return tList[1];
            else
                return string.Empty;
        }

        internal ObservableCollection<string> AddHdrItem(ObservableCollection<string> orgList, string NewItem)
        {
            //ObservableCollection<string> tempList = new ObservableCollection<string>();
            ObservableCollection<string>  tempList = orgList;
            tempList.Add(NewItem);
            return tempList;
        }

        internal ObservableCollection<string> RemoveHdrItem(ObservableCollection<string> orgList, string Removeitem)
        {
            //ObservableCollection<string> tempList = new ObservableCollection<string>();
            ObservableCollection<string>  tempList = orgList;
            tempList.Remove(Removeitem);
            return tempList;
        }

    }

    /// <summary>
    /// This is for RemoteProfile
    /// </summary>
    public class RemoteProfile : BindableBase
    {
        private string _Headers;
        public string RowsName
        {
            get { return _Headers; }
            set { SetProperty(ref _Headers, value); }
        }

        private string _pos1;
        public string GvCol1
        {
            get { return _pos1; }
            set { SetProperty(ref _pos1, value); }
        }

        private string _pos2;
        public string GvCol2
        {
            get { return _pos2; }
            set { SetProperty(ref _pos2, value); }
        }

        private string _pos3;
        public string GvCol3
        {
            get { return _pos3; }
            set { SetProperty(ref _pos3, value); }
        }

        private string _pos4;
        public string GvCol4
        {
            get { return _pos4; }
            set { SetProperty(ref _pos4, value); }
        }

        private string _pos5;
        public string GvCol5
        {
            get { return _pos5; }
            set { SetProperty(ref _pos5, value); }
        }

        private string _pos6;
        public string GvCol6
        {
            get { return _pos6; }
            set { SetProperty(ref _pos6, value); }
        }

        private string _pos7;
        public string GvCol7
        {
            get { return _pos7; }
            set { SetProperty(ref _pos7, value); }
        }

        private string _pos8;
        public string GvCol8
        {
            get { return _pos8; }
            set { SetProperty(ref _pos8, value); }
        }

        private string _pos9;
        public string GvCol9
        {
            get { return _pos9; }
            set { SetProperty(ref _pos9, value); }
        }

        private string _pos10;
        public string GvCol10
        {
            get { return _pos10; }
            set { SetProperty(ref _pos10, value); }
        }

        private string _pos11;
        public string GvCol11
        {
            get { return _pos11; }
            set { SetProperty(ref _pos11, value); }
        }

        private string _pos12;
        public string GvCol12
        {
            get { return _pos12; }
            set { SetProperty(ref _pos12, value); }
        }
    }

    public class RemoteProfile5 : BindableBase
    {
        private string _Headers;
        public string RowsName
        {
            get { return _Headers; }
            set { SetProperty(ref _Headers, value); }
        }

        private string _pos1;
        public string GvCol1
        {
            get { return _pos1; }
            set { SetProperty(ref _pos1, value); }
        }

        private string _pos2;
        public string GvCol2
        {
            get { return _pos2; }
            set { SetProperty(ref _pos2, value); }
        }

        private string _pos3;
        public string GvCol3
        {
            get { return _pos3; }
            set { SetProperty(ref _pos3, value); }
        }

        private string _pos4;
        public string GvCol4
        {
            get { return _pos4; }
            set { SetProperty(ref _pos4, value); }
        }

        private string _pos5;
        public string GvCol5
        {
            get { return _pos5; }
            set { SetProperty(ref _pos5, value); }
        }
    }



}
