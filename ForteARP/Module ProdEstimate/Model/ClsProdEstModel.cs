using ForteArg.Services;
using ForteARP.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ForteARP.Module_ProdEsitmate.Model
{
    public class ClsProdEstModel
    {
        private Sqlhandler _sqlhandler;
        public bool bProdEstReady = false;
        
        public ClsProdEstModel()
        {
            _sqlhandler = Sqlhandler.Instance;
            //_sqlhandler = MainWindow.AppWindows.MainSqlCfg;
        }

        internal void SetupAndInitSql()
        {
            try
            {

                bProdEstReady = _sqlhandler.CheckSqlTable("ProductionEst");

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in SetupAndInitSql " + ex.ToString());
            }
        }

        internal DataTable GetProdEstTable()
        {

            string strquery = "SELECT top 2 * From dbo.[" + "ProductionEst" + " ]";

            return _sqlhandler.GetProdEstTable(strquery);
        }
    }
}
