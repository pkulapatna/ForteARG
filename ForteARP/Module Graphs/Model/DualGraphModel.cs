using ForteArg.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows;

namespace ForteARP.Module_Graphs.Model
{
    public class DualGraphModel
    {

        private Sqlhandler _sqlhandler;

        private string CurrentBaleTable { get; set; }
        public int ISampleCount { get; set; }
        public int ILineCount { get; set; }

        public int iNewIndexNum = 0;

        public string m_Line;
        public string m_Source;

        public List<string> m_LineList;
        public List<string> m_SourceList;


        private int _iBalerType;

        public int BalerType
        {
            get { return _iBalerType; }
            set { SetProperty(ref _iBalerType, value); }
        }

        private void SetProperty(ref int iBalerType, int value)
        {
            _iBalerType = value;
        }

        public DualGraphModel()
        {
            _sqlhandler = Sqlhandler.Instance;

            //SqlConfigModel = MainWindow.AppWindows.MainSqlCfg;
            m_LineList = new List<string>();
            m_SourceList = new List<string>();
        }

        private string BuildQueryString()
        {
            List<string> StrType = new List<string>();
            //string strLineSeleted = string.Empty;
            //string strSourceSeleted = string.Empty;
            string strSourceLine = string.Empty;

            if ((m_LineList.Count > 0) & (m_SourceList.Count > 0))
            {
                if ((m_Line == "ALL") & (m_Source == "ALL"))
                {
                    strSourceLine = string.Empty;
                }
                else if ((m_Line != "ALL") & (m_Source != "ALL"))
                {
                    strSourceLine = "WHERE LineId = " + m_Line + " AND  SourceID = " + m_Source;
                }
                else if ((m_Line == "ALL") & (m_Source != "ALL"))
                {
                    strSourceLine = "WHERE SourceID = " + m_Source;
                }
                else if ((m_Line != "ALL") & (m_Source == "ALL"))
                {
                    strSourceLine = "WHERE LineId = " + m_Line;
                }

            }

            // strSourceLine = strLineSeleted + " AND " + strSourceSeleted;

            StrType.Add("SourceID");
            StrType.Add("LineID");
            CurrentBaleTable = _sqlhandler.GetCurrentBaleTable();

            return "SELECT TOP " + ISampleCount.ToString()
                + " Moisture,Weight,BDWeight,Forte,UpCount,DownCount,SourceName,LineName,SourceID,LineID,[index] FROM dbo.["
                + CurrentBaleTable
                + "]"
                + strSourceLine
                + " ORDER BY [TimeStart] DESC";
        }


        /// <summary>
        /// Get new data from SQL table
        /// </summary>
        /// <returns></returns>
        public async Task<DataTable> GetNewGraphDataAsync()
        {
            DataTable RealTimeDataTable = new DataTable();
            
            try
            {
                string m_strQueryString = BuildQueryString();
                RealTimeDataTable = await _sqlhandler.GetForteDataTableAsync(m_strQueryString);
                iNewIndexNum = _sqlhandler.iNewIndexNumber;
            }
            catch (Exception ex )
            {
                MessageBox.Show("Error in GetNewGraphDataAsync " + ex.Message);  
            }
            return RealTimeDataTable;
        }



        internal long GetNewIndex()
        {
            return _sqlhandler.GetIndexNumber();
        }

        internal void GetLineSource()
        {
            m_LineList.Clear();
            m_LineList = _sqlhandler.LineList;
            m_SourceList.Clear();
            m_SourceList = _sqlhandler.SourceList;
        }

        internal void InitSqlDualGraphModel()
        {
            _sqlhandler.SetupWorkStation();
        }

        internal void SetupWorkStation()
        {
            _sqlhandler.SetupWorkStation();
            m_LineList = _sqlhandler.LineList;
            m_SourceList = _sqlhandler.SourceList;
        }

    }
}
