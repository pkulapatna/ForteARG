using ForteArg.Services;
using ForteARP.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;

namespace ForteARP.Module_FieldsSelect.Model
{
    public class SelectItemModel
    {
        public ObservableCollection<CheckedListItem> AvailableItemList { get; set; }
        public DataTable HdrTable;
        public List<string> XmlColumnList = new List<string>();

        private Sqlhandler _sqlhandler;
        private readonly ClsXml MyXml;

        /// <summary>
        ///  HML Data stored here ////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private string _XmlGdvDirectory;
        public string XMLRealTimeGdvFile
        {
            get { return _XmlGdvDirectory + Path.Combine(SettingsDirectory, "HdrItems.xml"); } 
            set
            {
                if (value != null)
                    _XmlGdvDirectory = value;
            }
        }
        public string SettingsDirectory
        { get { return System.AppDomain.CurrentDomain.BaseDirectory; } } 
        
        
        public SelectItemModel()
        {
            _sqlhandler = Sqlhandler.Instance;

           // SqlConfigModel = MainWindow.AppWindows.MainSqlCfg;
            MyXml = MainWindow.AppWindows.ClsXML; 
            AvailableItemList = new ObservableCollection<CheckedListItem>();
        }

        internal ObservableCollection<string> GetSelectHrdCheckList()
        {
            ObservableCollection<string> XmlCheckedList = new ObservableCollection<string>();

            AvailableItemList.Clear();
            XmlColumnList.Clear();

           
       
            /*
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
                    */

            try
            {
                HdrTable = new DataTable();
                HdrTable = _sqlhandler.GetSqlScema();
               
                XmlColumnList = GetXmlcolumnList(XMLRealTimeGdvFile);

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
                AvailableItemList = new ObservableCollection<CheckedListItem>(AvailableItemList.OrderBy(x => x.Name)); //Sort
            }
            catch (Exception ex )
            {
                MessageBox.Show($"ERROR in GetSqlHdrList {ex.Message}");
            }           
            return XmlCheckedList;
        }

        internal ObservableCollection<string> AddHdrItem(ObservableCollection<string> orgList, string NewItem)
        {
            ObservableCollection<string>  tempList = orgList;
            tempList.Add(NewItem);
            return tempList;
        }

        internal ObservableCollection<string> RemoveHdrItem(ObservableCollection<string> orgList, string Removeitem)
        {
            ObservableCollection<string> tempList = orgList;
            tempList.Remove(Removeitem);
            return tempList;
        }

        internal void SaveXmlcolumnList(ObservableCollection<string> selectedHdrList)
        {
            MyXml.UpdateXMlcolumnList(selectedHdrList, XMLRealTimeGdvFile);
        }

        internal void SaveModified_setting()
        {
            List<CheckedListItem> CustomHdrList = new List<CheckedListItem>();

            foreach (var item in AvailableItemList)
            {
                if (item.IsChecked)
                    CustomHdrList.Add(new CheckedListItem(item.Id, item.Name, item.IsChecked, item.FieldType));
            }
            MyXml.WriteXmlGridView(CustomHdrList, MyXml.XMLGdvFilePath);

            CustomHdrList.Clear();
        }

        private List<string> GetXmlcolumnList(string xMLDropsGdvFile)
        {
            return MyXml.ReadXmlGridView(xMLDropsGdvFile);
        }

        private bool AllowField(string strItem)
        {
            foreach (var item in _sqlhandler.RemoveFieldsList)
            {
                if (item == strItem) return false;
            }
            return true;
        }

       
    }
}
