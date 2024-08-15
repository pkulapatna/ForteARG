using ForteARP.Reports.ViewModels;
using System;
using System.Data;
using System.Windows;

namespace ForteARP.Reports.Views
{
    /// <summary>
    /// Interaction logic for CSVReport.xaml
    /// </summary>
    public partial class CSVReport : Window, IDisposable
    {

        public static CSVReport CSVDialog;

        private readonly CSVReportViewModel MyCsvViewModel;

        public CSVReport()
        {
            InitializeComponent();
            CSVDialog = this;
            MyCsvViewModel = new CSVReportViewModel();
            DataContext = MyCsvViewModel;  
        }

        public void InitCsv(DataTable MyData, string strtable, int strStart, int strEnd)
        {
            MyCsvViewModel.MyDataTable = MyData;
            MyCsvViewModel.StrFileName = strtable;
            MyCsvViewModel.StrPathFile = MyCsvViewModel.StrFileLocation + "\\" + MyCsvViewModel.StrFileName + ".csv";
            MyCsvViewModel.FindCreateDir(MyCsvViewModel.StrFileLocation);
        }


        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
