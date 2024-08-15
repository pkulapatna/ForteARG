using ForteArg.Services.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ForteArg.Services
{
    public class Sqlhandler
    {
        private static readonly object padlock = new object();
    
        private static Sqlhandler _instance = null;
        public static Sqlhandler Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new Sqlhandler();
                    }
                    return _instance;
                }
            }
        }

        public string CurrentBaleTable { get; set; }
        public string PreviousBaleTable { get; set; }
        public string CurrentWLTable { get; set; }
        public string PreviousWLTable { get; set; }

        //Work Stations
        public string LocalWorkStationID { get; set; }
        public string TargetWorkStationID { get; set; }

        //Connection strings
        public const string MASTER_DB = "Master";
        public string ConString { get; set; }
        public string WLConStr { get; set; }
        public string MasterConStr { get; set; }
        public string StrUserName { get; set; }
        public string StrPassWrd { get; set; }
        public string StrDatabase { get; set; }

        public string StrDataSource { get; set; }
        public string StrWLDatabase { get; set; }
        public string StrHostID { get; set; }
        public string StrInstance { get; set; }

        public const int BALE_ARCHIVE = 0;
        public const int LOT_ARCHIVE = 1;
        public const int UNIT_ARCHIVE = 2;
        public const int WET_LAYER = 3;
        public const int QULITY_ARCHIVE = 4;

        public List<string> BaleTableList { get; set; }
        public List<string> RemoveFieldsList = null;
        public List<string> LineList { get; set; }
        public List<string> SourceList { get; set; }
        public List<string> WLTableList;

        public int m_BaleInDrop = 0;
        public int iSourceCount = 0;
        public int iLineCount = 0;
        public int iNewIndexNumber = 0;

        public string strCustName = string.Empty;
        public string strWtUnit = string.Empty;
        public string strMoistureTyp = string.Empty;

      
        private int CurUID = 0;

        public Sqlhandler()
        {
            WLTableList = new List<string>();
          //  SetSqlParams();
          //  SetConnectionString();
          
        }

        public bool BLocal
        {
            get => Settings.Default.bLocal; 
            set 
            {
                Settings.Default.bLocal = value;
                Settings.Default.Save();
            }
        }

        public bool BSerRemote
        {
            get => Settings.Default.bSerRemote;
            set
            {
                Settings.Default.bSerRemote = value;
                Settings.Default.Save();
            }
        }
        public string Host
        {
            get => Settings.Default.Host;
            set
            {
                Settings.Default.Host = value;
                Settings.Default.Save();
            }
        }

        public string SqlInstance
        {
            get => Settings.Default.Instance;
            set
            {
                Settings.Default.Instance = value;
                Settings.Default.Save();
            }
        }

        public string Database
        {
            get => Settings.Default.Database;
            set
            {
                Settings.Default.Database = value;
                Settings.Default.Save();
            }
        }
        public string UserName
        {
            get => Settings.Default.UserName;
            set
            {
                Settings.Default.UserName = value;
                Settings.Default.Save();
            }
        }
        public string Password
        {
            get => Settings.Default.PassWord;
            set
            {
                Settings.Default.PassWord = value;
                Settings.Default.Save();
            }
        }
        public int CbServerSelect
        {
            get => Settings.Default.CbServerSelect;
            set
            {
                Settings.Default.CbServerSelect = value;
                Settings.Default.Save();
            }
        }

        public string LocalHost
        {
            get => Settings.Default.LocalHost;
            set
            {
                Settings.Default.LocalHost = value;
                Settings.Default.Save();
            }
        }

        public int WeightUnit
        {
            get => Settings.Default.WeightUnit;
            set
            {
                Settings.Default.WeightUnit = value;
                Settings.Default.Save();
            }
        }

        
        public int MoistureUnit
        {
            get => Settings.Default.MoistureUnit;
            set
            {
                Settings.Default.MoistureUnit = value;
                Settings.Default.Save();
            }
        }

        public int LanguageIdx
        {
            get => Settings.Default.iLanguageIdx;
            set
            {
                Settings.Default.iLanguageIdx = value;
                Settings.Default.Save();
            }
        }


        public string GetHostName()
        {
            return Settings.Default.Host;
        }

        public string GetInstance()
        {
            return Settings.Default.Instance;
        }
        public string GetUserId()
        {
            return Settings.Default.UserName;
        }
        public string GetPassWord()
        {
            return Settings.Default.PassWord;
        }
        public string GetDastaBase()
        {
            return Settings.Default.Database;
        }

        public void SetSqlParams()
        {

            LocalWorkStationID = Settings.Default.LocalHost;
            TargetWorkStationID = Settings.Default.Host;

            if (Settings.Default.Instance != null)
                StrInstance = Settings.Default.Instance;
            else
                StrInstance = "SQLEXPRESS";

            if (Settings.Default.UserName != null)
                StrUserName = Settings.Default.UserName;
            else
                StrUserName = "forte";

            if (Settings.Default.PassWord != null)
                StrPassWrd = Settings.Default.PassWord;
            else
                StrPassWrd = "etrof";

            if (Settings.Default.Database != null)
                StrDatabase = Settings.Default.Database;
            else
                StrDatabase = "fortedata";

            if (Settings.Default.WLDatabase != null)
                StrWLDatabase = Settings.Default.WLDatabase;
            else
                StrWLDatabase = "ForteLayer";
        }

        private string SetSqlAuConnString(string SqlDatabase)
        {
            return "Data Source ='" + StrDataSource + "'; Database = "
                + SqlDatabase + "; User id= '" + StrUserName + "'; Password = '"
                + StrPassWrd + "'; connection timeout=30;Persist Security Info=True;";
        }

        public string BuildQueryString(int iSample)
        {
            iSample += 1;

            CurrentBaleTable = GetCurrentBaleTable();

            return "SELECT TOP " + iSample.ToString()
                + " Moisture,Weight,BDWeight,NetWeight,Forte,Dirt,Brightness,Finish,UpCount,DownCount,SourceName,LineName,SourceID,LineID,Position,[index] FROM dbo.["
                + CurrentBaleTable
                + "] with (NOLOCK) ORDER BY [TimeStart] DESC";
        }


        public void SetConnectionString()
        {
            LocalWorkStationID = Settings.Default.LocalHost;
            TargetWorkStationID = Settings.Default.Host;

            if (Settings.Default.bSerRemote)
                StrDataSource = TargetWorkStationID + @"\" + StrInstance;
            else
                StrDataSource = LocalWorkStationID + @"\" + StrInstance;

            //Realtime db, SQL authentication connection string
            ConString = SetSqlAuConnString(StrDatabase);

            //Wetlayer db, SQL authentication connection string
            WLConStr = SetSqlAuConnString(StrWLDatabase);

            //Master db, SQL authentication connection string 
            MasterConStr = SetSqlAuConnString(MASTER_DB);
        }


        public bool TestSqlConnection(string horst, string strInstance, string database, string username, string passwd)
        {
            string Source = horst + @"\" + strInstance;
            string constring = "Data Source = '" + Source + "'; Database = " + database + "; user id = '" + username +
                                "'; Password = '" + passwd + "'; connection timeout=30;Persist Security Info=True;";

            bool bConnected = false;

            try
            {
                using (var sqlConnection = new SqlConnection(constring))
                {
                    sqlConnection?.Open();
                    bConnected = true;
                    sqlConnection?.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in TestSqlConnection " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in TestSqlConnection -> {ex.Message}");
            }
            return bConnected;
        }

        public Task<DataTable> GetServers()
        {
            return Task.Run(() =>
            {
                return System.Data.Sql.SqlDataSourceEnumerator.Instance.GetDataSources();
            });
        }

        public DataTable GetSqlScema()
        {
            DataTable dx = new DataTable();
            string strQuery = "SELECT ORDINAL_POSITION,COLUMN_NAME,DATA_TYPE FROM ForteData.INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'" + GetCurrentBaleTable() + "'";

            try
            {
                using (var sqlConnection = new SqlConnection(ConString))
                {

                    sqlConnection.Open();
                    using (SqlCommand comm = new SqlCommand(strQuery, sqlConnection))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dx.Load(reader);
                        }
                    }
                    sqlConnection?.Close();
                }
                SetRemoveFields();

                foreach (var item in this.RemoveFieldsList)
                {
                    RemoveHrdItem(dx, item);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error in GetSqlScema -> " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in GetSqlScema -> {ex.Message}");
            }
            return dx;
        }


        public string GetCurrentBaleTable()
        {
            List<string> tablelist = new List<string>();
            string strquery = "SELECT top 2 [name],create_date FROM sys.tables WHERE [name] LIKE '%BaleArchive%' ORDER BY create_date DESC";

            try
            {
                using (var sqlConnection = new SqlConnection(ConString))
                {
                    sqlConnection?.Open();
                    using (var command = new SqlCommand(strquery, sqlConnection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    if (reader.GetString(0) != null)
                                        tablelist.Add(reader.GetString(0));
                                }
                        }
                    }
                    sqlConnection?.Close();
                }

                CurrentBaleTable = tablelist[0].ToString();
                if (tablelist.Count > 1) PreviousBaleTable = tablelist[1].ToString();
                else PreviousBaleTable = tablelist[0].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in GetCurrentBaleTable " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in GetCurrentBaleTable -> {ex.Message}");
            }
            return CurrentBaleTable;
        }

        public string GetCurrentWLTable()
        {
            List<string> tablelist = new List<string>();
            string strquery = "SELECT TOP 2 [name],create_date FROM sys.tables WHERE [name] LIKE '%FValueReadings%' ORDER BY create_date DESC";

            try
            {
                using (var sqlConnection = new SqlConnection(WLConStr))
                {
                    sqlConnection?.Open();
                    using (var command = new SqlCommand(strquery, sqlConnection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    if (reader.GetString(0) != null)
                                        tablelist.Add(reader.GetString(0));
                                }
                        }
                    }
                    sqlConnection?.Close();
                }
                CurrentWLTable = tablelist[0].ToString();
                if (tablelist.Count > 1) PreviousWLTable = tablelist[1].ToString();
                else PreviousWLTable = tablelist[0].ToString();
                //return m_CurrentWLTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in GetCurrentWLTable " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in GetCurrentWLTable -> {ex.Message}");
            }
            return CurrentWLTable;
        }

        public List<string> GetAllWLTables()
        {
            WLTableList = GettableList(WET_LAYER);
            return WLTableList;
        }
        public List<string> GettableList(int tabletype)
        {
            List<string> tablelist = new List<string>();
            string strquery = string.Empty;
            string MyConStr = string.Empty;

            switch (tabletype)
            {
                case BALE_ARCHIVE:
                    strquery = "SELECT [name],create_date FROM sys.tables WHERE [name] LIKE '%BaleArchive%' ORDER BY create_date DESC";
                    MyConStr = ConString;
                    break;
                case LOT_ARCHIVE:
                    strquery = "SELECT [name],create_date FROM sys.tables WHERE [name] LIKE '%LotArchive%' ORDER BY create_date DESC";
                    MyConStr = ConString;
                    break;
                case UNIT_ARCHIVE:
                    strquery = "SELECT [name],create_date FROM sys.tables WHERE [name] LIKE '%UnitArchive%' ORDER BY create_date DESC";
                    MyConStr = ConString;
                    break;
                case WET_LAYER:
                    strquery = "SELECT [name],create_date FROM sys.tables WHERE [name] LIKE '%FValueReadings%' ORDER BY create_date DESC";
                    MyConStr = WLConStr;
                    break;

                case QULITY_ARCHIVE:
                    strquery = "SELECT [name],create_date FROM sys.tables WHERE [name] LIKE '%QualityArchive%' ORDER BY create_date DESC";
                    MyConStr = ConString;
                    break;

                default:

                    break;
            }

            try
            {
                using (var sqlConnection = new SqlConnection(MyConStr))
                {
                    sqlConnection?.Open();
                    using (var command = new SqlCommand(strquery, sqlConnection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    if (reader.GetString(0) != null)
                                        tablelist.Add(reader.GetString(0));
                                }
                        }
                    }
                    sqlConnection?.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in GettableList -> " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in GettableList -> {ex.Message}");
            }
            return tablelist;
        }
        public DataTable GetProdEstTable(string strquery)
        {
            DataTable mytable = new DataTable();

            try
            {
                using (var sqlConnection = new SqlConnection(ConString))
                {
                    using (var adapter = new SqlDataAdapter(strquery, sqlConnection))
                    {
                        adapter.Fill(mytable);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR IN GetProdEstTable " + ex.Message);
            }
            return mytable;
        }

        public bool CheckSqlTable(string StrTablename)
        {
            bool bfound = false;
            string xstrQuery = "SELECT [name],create_date FROM sys.tables WHERE [name] LIKE '%" + StrTablename + "%' ORDER BY create_date DESC";
            string MyConstr = ConString;

            try
            {
                using (var sqlConnection = new SqlConnection(MyConstr))
                {
                    sqlConnection?.Open();
                    using (var command = new SqlCommand(xstrQuery, sqlConnection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                bfound = true;
                        }
                    }
                    sqlConnection?.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in CheckSqlTable" + ex);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in CheckSqlTable -> {ex.Message}");

            }
            return bfound;
        }

        public bool FindSqlDatabase(string StrTable)
        {
            bool bFoundTable = false;
            string strQuery = "SELECT * FROM sys.databases d WHERE d.database_id>4";

            try
            {
                using (var sqlConnection = new SqlConnection(ConString))
                {
                    if (sqlConnection != null)sqlConnection?.Open();
                    else return false;
                    using (var command = new SqlCommand(strQuery, sqlConnection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    if (reader != null)
                                    {
                                        if (reader[0].ToString() == StrTable)
                                            bFoundTable = true;
                                    }
                                }
                            }
                        }
                    }
                    sqlConnection?.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in SQL FindSqlDatabase " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in FindSqlDatabase -> {ex.Message}");

            }
            return bFoundTable;
        }

        public void CheckSetDropOption()
        {
            Settings.Default.bDropOption = false;

            List<string> DropNumberList = new List<string>();
            List<string> PositionList = new List<string>();
            var iBaleDrop = new List<int>();

            string strQuery = "SELECT DISTINCT DropNumber  From " + GetCurrentBaleTable();
            string strQuery2 = "SELECT DISTINCT Position  From " + GetCurrentBaleTable() ;
            string strQuery3 = "SELECT TOP 20 Position  From " + GetCurrentBaleTable() + " ORDER BY UID ASC";

            try
            {
                //Drop Numbers list
                using (var sqlConnection = new SqlConnection(ConString))
                {
                    sqlConnection?.Open();
                    using (var command = new SqlCommand(strQuery, sqlConnection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    if (reader != null)
                                        DropNumberList.Add(reader[0].ToString());
                                }
                        }
                    }
                    //Positions list
                    using (var command = new SqlCommand(strQuery2, sqlConnection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    if (reader != null)
                                        if (reader[0].ToString() != "0")
                                            PositionList.Add(reader[0].ToString());
                                }
                        }
                    }

                    //Bale in a Drop
                    using (var command = new SqlCommand(strQuery3, sqlConnection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    if (reader != null)
                                        if (reader[0].ToString() != "0")
                                            iBaleDrop.Add(Convert.ToInt32(reader[0].ToString()));
                                }
                        }
                    }
                    sqlConnection.Close();
                }

                if ((DropNumberList.Count > 1) && (PositionList.Count > 1) && (iBaleDrop.Count > 0))
                {
                    
                    Settings.Default.bDropOption = true;
                    Settings.Default.bDropNumbers = true;
                  
                    ClassCommon.bDropOption = true;
                    ClassCommon.BaleInADrop = iBaleDrop.Max();
                }
                else
                {
                  
                    Settings.Default.bDropOption = false;
                    Settings.Default.bDropNumbers = false;

                    ClassCommon.bDropOption = false;
                    ClassCommon.BaleInADrop = 0;
                }
                Settings.Default.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in CheckSetDropOption " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in CheckSetDropOption -> {ex.Message}");
            }
        }

        public DataTable GetTableByLotNum(string selectedLot, string strItems, DateTime datestart, DateTime dateEnd, string lotid, string strMonth)
        {
            DataTable LotTable = new DataTable();
            string ArchiveMonth = "BaleArchive" + strMonth;

            try
            {
                string MyQueryString = "SELECT " + strItems + " FROM [ForteData].[dbo].[" + ArchiveMonth + "] with (NOLOCK) WHERE LotNumber = "
                    + selectedLot + " AND TimeStart BETWEEN '" + datestart + "' AND '" + dateEnd + "' ORDER BY [TimeStart] ASC; ";

                using (var sqlConnection = new SqlConnection(ConString))
                {
                    sqlConnection?.Open();
                    using (SqlCommand comm = new SqlCommand(MyQueryString, sqlConnection))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.HasRows)
                                LotTable.Load(reader);
                        }
                    }
                    sqlConnection?.Close();

                    /*
                    using (var adapter = new SqlDataAdapter(MyQueryString, sqlConnection))
                    {
                        adapter.Fill(LotTable);
                    }
                    */
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in GetTableByLotNum" + ex);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in GetTableByLotNum -> {ex.Message}");
            }
            return LotTable;
        }

        public async Task<DataTable> GetTablebyLotNumberAsync(string selectedLot, string strItems, DateTime datestart, DateTime dateEnd, string lotid, string strMonth)
        {
            DataTable ArchiveTable = new DataTable();
            string ArchiveMonth = "BaleArchive" + strMonth;

            try
            {
                string MyQueryString = "SELECT " + strItems + " FROM [ForteData].[dbo].[" + ArchiveMonth + "] with (NOLOCK) WHERE LotNumber = "
                    + selectedLot + " AND TimeStart BETWEEN '" + datestart + "' AND '" + dateEnd + "' ORDER BY [TimeStart] ASC; ";

                await Task.Run(() =>
                {
                    using (SqlConnection sqlConnection = new SqlConnection(ConString))
                    {
                        sqlConnection?.Open();
                        using (SqlCommand comm = new SqlCommand(MyQueryString, sqlConnection))
                        {
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                if (reader.HasRows)
                                    ArchiveTable.Load(reader);
                            }
                        }
                        sqlConnection?.Close();
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in GetTablebyLotNumberAsync" + ex);
            }
            return ArchiveTable;
        }


        public DataTable GetSQLDataTable(string strClause)
        {
            DataTable mytable = new DataTable();

            try
            {
                using (var sqlConnection = new SqlConnection(ConString))
                {

                    sqlConnection?.Open();
                    using (SqlCommand comm = new SqlCommand(strClause, sqlConnection))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.HasRows)
                                mytable.Load(reader);
                        }
                    }
                    sqlConnection?.Close();
                }

                DataColumnCollection columns = mytable.Columns;

                if (columns.Contains("Weight"))
                {
                    DataRow[] rows = mytable.Select();
                    for (int i = 0; i < rows.Length; i++)
                    {
                        if (Settings.Default.WeightUnit == 1) //English Unit lb
                        {
                            if ((columns.Contains("Weight")) & (rows[i]["Weight"] != null))
                                rows[i]["Weight"] = rows[i].Field<float>("Weight") * 2.20462; //Lb.
                            if (columns.Contains("BDWeight"))
                            {
                                if ((rows[i]["BDWeight"] != null))
                                    rows[i]["BDWeight"] = (rows[i].Field<float>("BDWeight") * 2.20462); //Lb.
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in GetSQLDataTable " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in GetSQLDataTable -> {ex.Message}");
            }
            return mytable;
        }

        

        public DataTable GetCurrentBaleSourceLine( string source, string line, int items, List<string> itemlist)
        {
            string strSource = string.Empty;
            string strLine = string.Empty;
            string strList = string.Empty;

            if (source == "ALL")
            {
                strSource = string.Empty;
                if (line == "ALL")
                {
                    strLine = string.Empty;
                }
                else
                    strLine = $"LineId = {line}";
            }
            else
            {
                strSource = $"SourceID = {source}";
                if (line == "ALL")
                {
                    strLine = string.Empty;
                }
                else
                    strLine = $" AND LineId = {line}";
            }
               

            

            if (items < -1) items = 1;
            if (itemlist.Contains("ADWeight")) itemlist.Remove("ADWeight");
            char charsToTrim = ',';

            for (int i = 0; i < itemlist.Count; i++)
                strList += itemlist[i] + ",";


            DataTable mytable = new DataTable();
            DataTable newtable = new DataTable();

            string strQ = $"SELECT TOP {items} {strList.Trim(charsToTrim)},UID FROM dbo.[{CurrentBaleTable}] with (NOLOCK) WHERE {strSource} {strLine} ORDER BY TimeComplete DESC";

            try
            {

                using (var sqlConnection = new SqlConnection(ConString))
                {

                    sqlConnection?.Open();
                    using (SqlCommand comm = new SqlCommand(strQ, sqlConnection))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.HasRows)
                                mytable.Load(reader);
                        }
                    }
                    sqlConnection?.Close();
                }
                if(mytable.Rows.Count > 0)
                {
                    if (Convert.ToInt32(mytable.Rows[0]["UID"]) != CurUID)
                    {
                        newtable = mytable;
                        CurUID = Convert.ToInt32(mytable.Rows[0]["UID"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in GetCurrentBaleSourceLine -> " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in GetCurrentBaleSourceLine -> {ex.Message}");
            }
            return newtable;
        }


       
        public List<string> GetDistinctitemlist(string strItem)
        {
            List<string> myList = new List<string>();
            string strQuery = "SELECT DISTINCT " + strItem + " From " + GetCurrentBaleTable();

            try
            {
                using (var sqlConnection = new SqlConnection(ConString))
                {
                    sqlConnection?.Open();
                    using (var command = new SqlCommand(strQuery, sqlConnection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    if (reader != null)
                                    {
                                        myList.Add(reader[0].ToString());
                                    }
                                }
                            }
                        }
                    }
                    sqlConnection?.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in GetDistinctitemlist " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in GetDistinctitemlist -> {ex.Message}");
            }
            return myList;
        }


        public void SetupWorkStation()
        {
            LineList = new List<string>();
            SourceList = new List<string>();

            try
            {
                CurrentBaleTable = GetCurrentBaleTable();
                SourceList = GetUniquIntitemlist("SourceID", CurrentBaleTable);
                if ((SourceList.Count > 0) && (SourceList[0] != "0"))
                    iSourceCount = SourceList.Count;

                if (SourceList.Count > 1) SourceList.Add("ALL");

                LineList = GetUniquIntitemlist("LineID", CurrentBaleTable);
                if ((LineList.Count > 0) && (LineList[0] != "0"))
                    iLineCount = LineList.Count;

                if (LineList.Count > 1) LineList.Add("ALL");

                m_BaleInDrop = GetMaxValfromtable("Position", CurrentBaleTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in SetupWorkStation " + ex);
            }
        }

        public int GetMaxValfromtable(string target, string table)
        {
            string strMaxQuery = "Select TOP 20 " + target + " FROM " + table + " WHERE Position > 0 ORDER BY [TimeComplete] DESC;";
            int iMax = 0;

            try
            {
                using (var sqlConnection = new SqlConnection(ConString))
                {
                    sqlConnection?.Open();
                    using (var command = new SqlCommand(strMaxQuery, sqlConnection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    if (reader != null)
                                    {
                                        if (Convert.ToInt32(reader[0].ToString()) > iMax)
                                        {
                                            iMax = Convert.ToInt32(reader[0].ToString());
                                        }
                                    }
                                }
                        }
                    }
                    sqlConnection?.Close();
                }
                return iMax;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in SqlModel GetMaxValfromtable" + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in GetMaxValfromtable -> {ex.Message}");

            }
            return iMax;
        }


        public long GetIndexNumber()
        {
            long idx = 0;

            string strIdxQry = "SELECT TOP 5 [index] FROM dbo.[" + GetCurrentBaleTable() + "] ORDER BY [TimeStart]  DESC";
            List<int> itemList = new List<int>();

            try
            {
                using (var sqlConnection = new SqlConnection(ConString))
                {
                    sqlConnection?.Open();
                    using (var command = new SqlCommand(strIdxQry, sqlConnection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    if (reader != null)
                                        itemList.Add(Convert.ToInt32(reader[0].ToString()));
                                }
                            }
                        }
                    }
                    sqlConnection?.Close();
                }
                if (itemList.Count > 0) idx = itemList[0]; //Newest index in the table.
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in GetIndexNumber " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in GetIndexNumber -> {ex.Message}");
            }
            return idx;
        }

        public async Task<DataTable> GetForteDataTableAsync(string strQuery)
        {
            DataTable mytable = new DataTable();
            mytable.Clear();
            DataColumnCollection columns = mytable.Columns;

            try
            {
                await Task.Run(() =>
                {
                    using (SqlConnection sqlConnection = new SqlConnection(ConString))
                    {
                        sqlConnection?.Open();
                        using (SqlCommand comm = new SqlCommand(strQuery, sqlConnection))
                        {
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                if (reader.HasRows)
                                    mytable.Load(reader);
                            }
                        }
                        sqlConnection?.Close();
                    }

                    if (mytable.Rows.Count > 0 & columns.Contains("index"))
                    {
                        iNewIndexNumber = mytable.Rows[0].Field<int>("index");
                        mytable.Columns.Remove("index");
                    }

                    DataRow[] rows = mytable.Select();

                    for (int i = 0; i < rows.Length; i++)
                    {
                        if (columns.Contains("Moisture"))
                        {
                            if(rows[i]["Moisture"] != null)
                                rows[i]["Moisture"] = ClassCommon.CalulateMoisture(rows[i].Field<float>("Moisture").ToString(), Settings.Default.MoistureUnit);
                        }

                        if (columns.Contains("Weight"))
                        {
                            if (Settings.Default.WeightUnit == 1) //English Unit lb
                            {
                                if ((columns.Contains("Weight")) & (rows[i]["Weight"] != null))
                                    rows[i]["Weight"] = rows[i].Field<float>("Weight") * 2.20462; //Lb

                                if (columns.Contains("BDWeight"))
                                {
                                    if ((rows[i]["BDWeight"] != null))
                                        rows[i]["BDWeight"] = (rows[i].Field<float>("BDWeight") * 2.20462); //Lb.
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in GetForteDataTableAsync -> " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in GetForteDataTableAsync -> {ex.Message}");
            }
            return mytable;
        }


        public DataTable GetForteDataTable(string strQuery)
        {
            DataTable mytable = new DataTable();
            mytable.Clear();
            DataColumnCollection columns = mytable.Columns;

            try
            {
                using (var sqlConnection = new SqlConnection(ConString))
                {
                    sqlConnection.Open();
                    using (SqlCommand comm = new SqlCommand(strQuery, sqlConnection))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.HasRows)
                                mytable.Load(reader);
                        }
                    }
                    sqlConnection?.Close();
                }

                if(mytable.Rows.Count > 0)
                {

                    DataRow[] rows = mytable.Select();

                    for (int i = 0; i < rows.Length; i++)
                    {
                        if (columns.Contains("Moisture"))
                        {
                            switch (Settings.Default.MoistureUnit)
                            {
                                case 0: // %MC == moisture from Sql database
                                    if ((columns.Contains("Moisture")) & (rows[i]["Moisture"] != null))
                                        rows[i]["Moisture"] = rows[i]["Moisture"];
                                    break;

                                case 1: // %MR  = Moisture / ( 1- Moisture / 100)
                                    if ((columns.Contains("Moisture")) & (rows[i]["Moisture"] != null))
                                        rows[i]["Moisture"] = rows[i].Field<float>("Moisture") / (1 - rows[i].Field<float>("Moisture") / 100);
                                    break;

                                case 2: // %AD = (100 - moisture) / 0.9
                                    if ((columns.Contains("Moisture")) & (rows[i]["Moisture"] != null))
                                        rows[i]["Moisture"] = (100 - rows[i].Field<float>("Moisture")) / 0.9;
                                    break;

                                case 3: // %BD  = 100 - moisture
                                    if ((columns.Contains("Moisture")) & (rows[i]["Moisture"] != null))
                                        rows[i]["Moisture"] = 100 - rows[i].Field<float>("Moisture");
                                    break;
                            }
                        }

                        if (columns.Contains("Weight"))
                        {
                            if (Settings.Default.WeightUnit == 1) //English Unit lb
                            {
                                if ((columns.Contains("Weight")) & (rows[i]["Weight"] != null))
                                    rows[i]["Weight"] = rows[i].Field<float>("Weight") * 2.20462; //Lb
                                if (columns.Contains("BDWeight"))
                                {
                                    if (rows[i]["BDWeight"] != null)
                                        rows[i]["BDWeight"] = rows[i].Field<float>("BDWeight") * 2.20462; //Lb.
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in GetForteDataTable -> " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in GetForteDataTable -> {ex.Message}");

            }
            return mytable;
        }


        internal int GetNewItemData(string strQueryString)
        {
            int strTarget = 0;

            try
            {
                using (var sqlConnection = new SqlConnection(ConString))
                {
                    sqlConnection?.Open();
                    using (var command = new SqlCommand(strQueryString, sqlConnection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    if (reader != null)
                                        strTarget = reader.GetInt32(0);
                                }
                        }
                    }
                    sqlConnection?.Close();
                }
                return strTarget;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in GetNewItemData -> " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in GetNewItemData -> {ex.Message}");
                return 0;
            }
        }

        public int GetIntNewItemData(string strQueryString)
        {
            int strTarget = 0;

            try
            {
                using (var sqlConnection = new SqlConnection(ConString))
                {
                    sqlConnection?.Open();
                    using (var command = new SqlCommand(strQueryString, sqlConnection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    if (reader != null)
                                        strTarget = Convert.ToInt32(reader[0]);
                                }
                        }
                    }
                    sqlConnection?.Close();
                }
                return strTarget;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in GetIntNewItemData -> " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in GetIntNewItemData -> {ex.Message}");
                return 0;
            }
        }

        private void RemoveHrdItem(DataTable Ttable, string strItem)
        {
            foreach (DataRow item in Ttable.Rows)
            {
                if (item[1].ToString() == strItem)
                {
                    item.Delete();
                }
            }
            Ttable.AcceptChanges();
        }


        private void SetRemoveFields()
        {
            if (RemoveFieldsList == null)
            {
                RemoveFieldsList = new List<string>
                {
                    "Index",
                    "Empty",
                    "QualityUID",
                    "AsciiFld1",
                    "AsciiFld2",
                    "OrderStr",
                    "QualityName",
                    "GradeLabel1",
                    "StockLabel1",
                    "StockLabel2",
                    "StockLabel3",
                    "StockLabel4",
                    "JobNum",
                    "Forte1",
                    "Forte2",
                    "ForteAveraging",
                    //RemoveFieldsList.Add("UpCount");
                    //RemoveFieldsList.Add("DownCount");
                    "DownCount2",
                    //RemoveFieldsList.Add("Brightness");
                    "BaleHeight",
                    "SourceId",
                    //RemoveFieldsList.Add("Finish");
                    "SheetArea",
                    "SheetCount",
                    "CalibrationID",
                    "PkgMoistMethod",
                    "SpareSngFld1",
                    "SpareSngFld2",
                    //RemoveFieldsList.Add("SpareSngFld3");
                    "LastInGroup",
                    "MoistMes",
                    "ProdDayStart",
                    "ProdDayEnd",
                    "SourceID",
                    //RemoveFieldsList.Add("LineID");
                    "StockID",
                    "GradeID",
                    "WtMes",
                    "AsciiFld3",
                    "AsciiFld4",
                    "SR",
                    "UID",
                    // RemoveFieldsList.Add("Package");
                    "ResultDesc",
                    "GradeLabel2",
                    "WLAlarm",
                    "WLAStatus",
                    //
                    "Status",
                    "WeightStatus",
                    "TemperatureStatus",
                    "OrigWeightStatus",
                    "ForteStatus",
                    "Forte1Status",
                    "Forte2Status",
                    "UpCountStatus",
                    "DownCountStatus",
                    "DownCount2Status",
                    "BrightnessStatus",
                    "TimeStartStatus",
                    "BaleHeightStatus",
                    "TimeStartStatus",
                    "TimeCompleteStatus",
                    "SourceIDStatus",
                    "StockIDStatus",
                    "GradeIDStatus",
                    "TareWeightStatus",
                    "AllowanceStatus",
                    "SheetCountStatus",
                    "MoistureStatus",
                    "NetWeightStatus",
                    "CalibrationIDStatus",
                    "SeriAlNumberStatus",
                    "LotNumberStatus",
                    "TemperatureStatus",
                    "UnitNumberStatus",
                    "UnitIdent",
                    "Temperature"
                };


                //DownCount
                //RemoveFieldsList.Add("FC_IdentString");
                //RemoveFieldsList.Add("Dirt");
                //ItemRemoveLst.Add("BasisWeight")
            }
        }


        public List<string> GetUniqueStrItemlist(string strItem, string strTable)
        {
            string constr = ConString;
            List<string> itemList = new List<string>();
            string strQuery = "SELECT DISTINCT " + strItem + " From " + strTable + " ORDER BY " + strItem + ";";

            if (strItem == "BalerID") constr = WLConStr;

            try
            {
                using (var sqlConnection = new SqlConnection(constr))
                {
                    sqlConnection?.Open();
                    using (var command = new SqlCommand(strQuery, sqlConnection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    if (reader != null)
                                        itemList.Add(reader[0].ToString());
                                }
                        }
                    }
                    sqlConnection?.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in GetUniqueStrItemlist " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in GetUniqueStrItemlist -> {ex.Message}");
            }
            return itemList;
        }

        public List<string> GetUniquIntitemlist(string strItem, string strTable)
        {
            List<string> itemList = new List<string>();
            string strQuery = "SELECT DISTINCT " + strItem + " From " + strTable + " ORDER BY " + strItem + ";";

            try
            {
                using (var sqlConnection = new SqlConnection(ConString))
                {
                    sqlConnection?.Open();
                    using (var command = new SqlCommand(strQuery, sqlConnection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    if (reader != null)
                                        itemList.Add(reader.GetInt32(0).ToString());
                                }
                        }
                    }
                    sqlConnection?.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in GetUniquIntitemlist " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in GetUniquIntitemlist -> {ex.Message}");
            }
            return itemList;
        }

        public DataTable GetWetLayerDataTable(string strMonth, string strWetQuery)
        {
            DataTable mytable = new DataTable();
            try
            {
                using (var sqlConnection = new SqlConnection(WLConStr))
                {
                    sqlConnection.Open();
                    using (SqlCommand comm = new SqlCommand(strWetQuery, sqlConnection))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.HasRows)
                                mytable.Load(reader);
                        }
                    }
                    sqlConnection?.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in GetWetLayerDataTable" + ex);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in GetWetLayerDataTable -> {ex.Message}");
            }
            return mytable;
        }


        private string StrIniItem(string strLine)
        {
            string strtarget = string.Empty;
            char[] separators = { '=' };

            try
            {
                string[] words = strLine.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                if (words.Length > 1) strtarget = words[1];
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in StrIniItem " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in StrIniItem -> {ex.Message}");
            }
            return strtarget;
        }

        public bool ReadSharedinifile()
        {
            bool bFoundFile = false;
            string inifilepath = @"\\" + Settings.Default.Host + @"\ForteSystem\System\FSys.ini";
            StreamReader sr;
            string line;

            try
            {
                if (System.IO.File.Exists(inifilepath))
                {
                    sr = new StreamReader(inifilepath);
                    do
                    {
                        line = sr.ReadLine();
                        if ((line.Contains("CustomerName")) && !line.Contains(":"))
                        {
                            strCustName = StrIniItem(line).Trim();
                        }
                        if ((line.Contains("Weight Units")) && !line.Contains(":") && !line.Contains("Alt Weight Unit"))
                        {
                            strWtUnit = StrIniItem(line).Trim();

                        }
                        if ((line.Contains("MoistureType")) && !line.Contains(":"))
                        {
                            strMoistureTyp = StrIniItem(line).Trim();
                        }

                    } while (sr.Peek() != -1);
                    sr.Close();
                    sr.Dispose();
                    bFoundFile = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in ReadRemoteINI " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in ReadRemoteINI -> {ex.Message}");

            }
            return bFoundFile;
        }
    }
}
