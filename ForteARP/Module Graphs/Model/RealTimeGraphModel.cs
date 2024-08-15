using ForteArg.Services;
using ForteARP.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ForteARP.Module_Graphs.Model
{
    public class RealTimeGraphModel
    {
        private Sqlhandler _sqlhandler;

        //  private string CurrentBaleTable { get; set; }
        //  public int ISampleCount { get; set; }
        //  public int ILineCount { get; set; }

        public int m_Line = 0;
        public int m_Source = 0;
        public List<string> HdrDropDownList = new List<string>();
       
        public struct CALC_RESULTS
        {
            public long BaleID;
            public int iBalerID;
            public string strBaler; //*10
            public double dDeviation;
            public double dAverage;
            public double dMaxValue;
            public double dMinValue;
            public int iNumbOfSpots;
            public string strResult; //*10
            public int[] iVals;
            public int iSize;
            public double[] dCalcResults;
            public List<double> dLayers;
            public int iLayers;
            public double dMoisture;
            public bool bAlarm;
            public bool bTCStampsAssigned;
        };

        public RealTimeGraphModel()
        {
            _sqlhandler = Sqlhandler.Instance;

            //SqlConfigModel = MainWindow.AppWindows.MainSqlCfg;

          
        }

        internal int GetLineCount()
        {
            return _sqlhandler.iLineCount;
        }

        internal int GetSourceCount()
        {
            return _sqlhandler.iSourceCount;
        }

        internal long GetNewIndex()
        {
            return _sqlhandler.GetIndexNumber();
        }

        /// <summary>
        /// Get new data from SQL table here!
        /// </summary>
        /// <param name="iSample"></param>
        /// <returns></returns>
        public async Task<DataTable> GetNewGraphDataAsync(int iSample)
        {
            DataTable RealTimeDataTable = new DataTable();
            RealTimeDataTable.Clear();
            string m_strQueryString = string.Empty;

            try
            {
                m_strQueryString = _sqlhandler.BuildQueryString(iSample);
                RealTimeDataTable =  await _sqlhandler.GetForteDataTableAsync(m_strQueryString);

                await Task.Run(() =>
                {
                    DataColumn NewCol = RealTimeDataTable.Columns.Add("ADWeight", typeof(Single));
                    for (int i = 0; i < RealTimeDataTable.Rows.Count; i++)
                    {
                        if ((RealTimeDataTable.Rows[i]["BDWeight"] != null) && (RealTimeDataTable.Rows[i].Field<Single>("BDWeight") > 0))
                            RealTimeDataTable.Rows[i]["ADWeight"] = RealTimeDataTable.Rows[i].Field<Single>("BDWeight") / 0.9;
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in GetDatatableAsync " + ex.Message);
            }
            return RealTimeDataTable;
        }

 
        internal void GetLineSource()
        {
            m_Line = _sqlhandler.iLineCount;
            m_Source = _sqlhandler.iSourceCount;
        }

        internal void InitSqlRealTimeGraphModel()
        {
            _sqlhandler.SetupWorkStation();
        }

        private void CalCVMinMax(List<double> SampleList, int iLayers, out CALC_RESULTS tResults)
        {
            tResults = new CALC_RESULTS();
            double sumOfDerivation = 0;

            //Average
            tResults.dAverage = SampleList.Average();

            //Min Max
            tResults.dMinValue = SampleList.Min();
            tResults.dMaxValue = SampleList.Max();

            //MaxYAxis = SampleList.Max() + 5;

            //layers
            tResults.dLayers = new List<Double>();
            tResults.dLayers = SampleList;

            //Deviation
            tResults.bAlarm = false;
            foreach (var value in SampleList)
            {
                sumOfDerivation += (value - tResults.dAverage) * (value - tResults.dAverage);
            }

            //STD
            double Variance = sumOfDerivation / (SampleList.Count - 1);
            double StandardDeviation = Math.Sqrt(Variance);

            //%CV (Coefficient of Variation = Standard Deviation / Mean)
            tResults.dDeviation = (StandardDeviation / tResults.dAverage) * 100;
            tResults.bAlarm = false;
        }


      
    }

   






}
