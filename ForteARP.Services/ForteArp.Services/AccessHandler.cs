using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ForteArg.Services
{
    public class AccessHandler
    {
        private static readonly object padlock = new object();



        private readonly string dbProvider = "PROVIDER=Microsoft.Jet.OLEDB.4.0;";
        //private string DB_SUPJ4 = @"Data Source=C:\\ForteSystem\Reports\Rep_SupJ4.mdb;Persist Security Info=True;";
        private readonly string DB_LVFORMAT = @"Data Source=C:\\ForteSystem\Reports\LVFormats.mdb;Persist Security Info=True;";

        // string customconfigCn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=C:\\ForteSystem\\Realtime\\CustomConfig.mdb; User Id = admin; Password=";
        // string cfg7760Cn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=C:\\ForteSystem\\Realtime\\Cfg7760.mdb; User Id = admin; Password=";
        // string Cfg7760Con = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source= C:\ForteSystem\Realtime\Cfg7760.mdb; User Id = admin; Password=";


        public DataTable ConfigTable { get; set; }
        public DataTable WetMTable { get; set; }
        public DataTable IdentTable { get; set; }
        public DataTable DtTmTable { get; set; }
        public DataTable ModBusConfigTable { get; set; }
        public DataTable LVHdrFmtBale { get; set; }

        private static AccessHandler _instance = null;

        public static AccessHandler Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new AccessHandler();
                    }
                    return _instance;
                }
            }
        }

        public AccessHandler()
        {
            ClsSerilog.LogMessage(ClsSerilog.Info, $"Config Access Database ......................");
        }

        public DataTable GetLVHdrFmtBaleTable()
        {
            DataTable LVHdrFmtBaleTable = new DataTable();

            string strQuery = "SELECT FieldExpr,Text,Format FROM [LVHdrFmtBale]";
            string connectionString = dbProvider + DB_LVFORMAT;

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(strQuery, connection))
                    {
                        adapter.Fill(LVHdrFmtBaleTable);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("EROR in GetLVHdrFmtBaleTable " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in GetLVHdrFmtBaleTable -> {ex.Message}");
            }

            return LVHdrFmtBaleTable;
        }


        internal bool ConnectAccessSharedFiles()
        {
            bool bACon = false;
            // string dbProvider = "PROVIDER=Microsoft.Jet.OLEDB.4.0;";
            //   string dbSource = RT_7760_DB_Remote;
            //   string connectionString = dbProvider + dbSource;
            /*
            try
            {
                using (var conAcc = new OleDbConnection(connectionString))
                {
                    conAcc.Open();
                    bACon = true;
                    // ReadSharedinifile();
                    conAcc.Close();
                }
            }
            catch (Exception ex)
            {
                //throw new System.Exception("ConnectAccessSharedFiles; Cannot connect To remote Acccess Database, Set ForteSystem to Sharefile");
                //MessageBox.Show("EROR in ConnectAccessSharedFiles " + ex.Message);

            }
            */
            return bACon;
        }

        internal void SetupAccessDB()
        {
            LVHdrFmtBale = GetLVHdrFmtBaleTable();
        }
    }
}
