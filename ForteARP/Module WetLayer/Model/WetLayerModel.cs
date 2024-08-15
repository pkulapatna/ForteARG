using ForteArg.Services;
using ForteARP.Model;
using System.Collections.Generic;
using System.Data;

namespace ForteARP.Module_WetLayer.Model
{
    public class WetLayerModel
    {
        private Sqlhandler _sqlhandler;

        public WetLayerModel()
        {
            _sqlhandler = Sqlhandler.Instance;
            //SqlConfigModel = MainWindow.AppWindows.MainSqlCfg;
        }

        internal DataTable GetNewWLDataTable(string selectWLmonth, string strQuery)
        {
            return _sqlhandler.GetWetLayerDataTable(selectWLmonth, strQuery);
        }

        internal string GetWLCurrMonth()
        {
            return _sqlhandler.GetCurrentWLTable();
        }

        internal List<string> GetWLMonthList()
        {
            return _sqlhandler.GetAllWLTables();
        }

        internal List<string> GetBalerList(string selectTableValue)
        {
            return _sqlhandler.GetUniqueStrItemlist("BalerID", selectTableValue);
        }

        internal string GetPreMonth()
        {
            return _sqlhandler.PreviousWLTable;
        }

    }
}
