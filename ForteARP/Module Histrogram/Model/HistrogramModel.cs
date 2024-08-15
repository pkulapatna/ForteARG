using ForteArg.Services;
using ForteARP.Model;
using System.Collections.Generic;
using System.Data;

namespace ForteARP.Module_Histrogram.Model
{
    public class HistrogramModel
    {
    
        private Sqlhandler _sqlhandler;

        public HistrogramModel()
        {
            _sqlhandler = Sqlhandler.Instance;

           // SqlConfigModel = MainWindow.AppWindows.MainSqlCfg;
        }


        internal void InitSqlBaleArchiveModel()
        {
            _sqlhandler.SetupWorkStation();
        }

        internal List<string> GetSqlTableList()
        {
            return _sqlhandler.GettableList(Sqlhandler.BALE_ARCHIVE);
        }

        internal List<string> GetSqlStockList(string selectTableValue)
        {
            return _sqlhandler.GetUniqueStrItemlist("StockName", selectTableValue);
        }

        internal List<string> GetSqlGradeList(string selectTableValue)
        {
            return _sqlhandler.GetUniqueStrItemlist("GradeName", selectTableValue);
        }

        internal List<string> GetSqlLineList(string strTable)
        {
            return _sqlhandler.GetUniquIntitemlist("LineID", strTable);
        }

        internal List<string> GetSqlSourceList(string strTable)
        {
            return _sqlhandler.GetUniquIntitemlist("SourceID", strTable);
        }

        internal DataTable GetSqlBaleDataTable(string queryString)
        {
            return _sqlhandler.GetForteDataTable(queryString);
        }

    }
}
