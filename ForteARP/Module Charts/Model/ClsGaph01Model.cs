using ForteArg.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForteARP.Model
{
    class ClsGaph01Model : IDisposable
    {
        private Sqlhandler _sqlhandler;

        public ClsGaph01Model()
        {
            _sqlhandler = Sqlhandler.Instance;

        //    SqlConfigModel = MainWindow.AppWindows.MainSqlCfg;

        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        internal async Task<DataTable> GetTableByLotNumAsync(string selectedLot, string strQuery, DateTime datestart, DateTime dateEnd, string lotid, string strmonth)
        {
            return await _sqlhandler.GetTablebyLotNumberAsync(selectedLot, strQuery, datestart, dateEnd, lotid, strmonth);  
        }
    }
}
