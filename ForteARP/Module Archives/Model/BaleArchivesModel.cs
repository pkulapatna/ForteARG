using ForteArg.Services;
using ForteARP.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForteARP.Archives_Module.Model
{
    public class BaleArchivesModel
    {
        public ObservableCollection<string> CustomItemsList;

        private Sqlhandler _sqlhandler;

        private readonly ClsXml MyXml = MainWindow.AppWindows.ClsXML;

        internal List<string> GetCustomXmlTable()
        {
           return MyXml.ReadXmlGridView(MyXml.XMLHdrFilePath); // XMLGdvFilePath);   
        }

        public BaleArchivesModel()
        {
            _sqlhandler = Sqlhandler.Instance;
            //SqlConfigModel = MainWindow.AppWindows.MainSqlCfg;
            //SqlConfigModel.SetupWorkStation();
        }

        internal List<string> GetSqlTableList()
        {
            return _sqlhandler.GettableList(Sqlhandler.BALE_ARCHIVE);
        }

        public List<string> GetAllItemsListModel()
        {
            List<string> listX = new List<string>();
            DataTable HdrTable = _sqlhandler.GetSqlScema();

            listX.Clear();
            for (int i = 0; i < HdrTable.Rows.Count; i++)
            {
                listX.Add(HdrTable.Rows[i]["COLUMN_NAME"].ToString());
            }
            return listX;
        }

        internal List<string> GetSqlStockList(string strTable)
        {

            return _sqlhandler.GetUniqueStrItemlist("StockName", strTable);
        }

        internal List<string> GetSqlGradeList(string strTable)
        {
            return _sqlhandler.GetUniqueStrItemlist("GradeName", strTable);
        }

        internal List<string> GetSqlLineList(string strTable)
        {
            return _sqlhandler.GetUniquIntitemlist("LineID", strTable);
        }

        internal List<string> GetSqlSourceList(string strTable)
        {
            return _sqlhandler.GetUniquIntitemlist("SourceID", strTable);
        }

        public DataTable GetBaleArchiveDataTable(string strClause)
        {
            return _sqlhandler.GetForteDataTable(strClause);
        }


        public async Task<DataTable> GetBaleArchiveDataTableAsync(string strClause)
        {
            return await _sqlhandler.GetForteDataTableAsync(strClause);
        }


        internal List<string> GetSqlLotTableList()
        {
            return _sqlhandler.GettableList(Sqlhandler.LOT_ARCHIVE);
        }

        public DataTable GetLotArchiveDataTable(string strClause)
        {
            return _sqlhandler.GetForteDataTable(strClause);
        }

        internal List<string> GetSqlUnitTableList()
        {
            return _sqlhandler.GettableList(Sqlhandler.UNIT_ARCHIVE);
        }

        public DataTable GetUnitArchiveDataTable(string strClause)
        {
            return _sqlhandler.GetForteDataTable(strClause);
        }

        public DataTable GetQulityArchiveDataTable(string strClause)
        {
            return _sqlhandler.GetForteDataTable(strClause);
        }

        internal List<string> GetSqlLotList(string strTable)
        {

            return _sqlhandler.GetUniqueStrItemlist("LotNum", strTable);
        }

        internal void InitSqlBaleArchiveModel()
        {
            _sqlhandler.SetupWorkStation();
        }

        internal List<string> GetSqlQulityTableList()
        {
            return _sqlhandler.GettableList(Sqlhandler.QULITY_ARCHIVE);
        }

        internal DataTable GetTableByLotNum(string lotIdString, string strQuery, DateTime opendate, DateTime closedate, string strItem, string selectedMonth)
        {
            return _sqlhandler.GetTableByLotNum(lotIdString, strQuery, opendate, closedate, strItem, selectedMonth);
        }

    }
}
