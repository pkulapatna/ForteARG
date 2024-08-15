using ForteARP.Properties;
using ForteARP.Reports.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;

namespace ForteARP.Reports.ViewModels
{
    public class CSVReportViewModel : BindableBase
    {

        private string _strFileLocation;
        public string StrFileLocation
        {
            get { return _strFileLocation; }
            set { SetProperty(ref _strFileLocation, value); }
        }
        private string _StrFileName;
        public string StrFileName
        {
            get { return _StrFileName; }
            set { SetProperty(ref _StrFileName, value); }
        }

        private string _strPathFile;
        public string StrPathFile
        {
            get { return _strPathFile; }
            set { SetProperty(ref _strPathFile, value); }
        }

 
        public DelegateCommand LoadedPageICommand { get; set; }
        public DelegateCommand ClosedPageICommand { get; set; }
        public DelegateCommand WriteCommand { get; set; }
        public DelegateCommand BrowseCommand { get; set; }

        private bool _bCSVCanwrite;
        public bool BCSVCanwrite
        {
            get { return _bCSVCanwrite; }
            set { SetProperty(ref _bCSVCanwrite, value); }
        }

        public DataTable MyDataTable;


        public CSVReportViewModel()
        {
            LoadedPageICommand = new DelegateCommand(LoadedPageExecute, LoadedPageCanExecute);
            ClosedPageICommand = new DelegateCommand(ClosedPageExecute, ClosedPageCanExecute);
            WriteCommand = new DelegateCommand(WriteExecute, WriteCanExecute).ObservesProperty(() => BCSVCanwrite);
            BrowseCommand = new DelegateCommand(BrowseExecute, BrowseCanExecute);

            StrFileLocation = Settings.Default.CsvFileLocation;

        }


        private bool BrowseCanExecute()
        {
            return true;
        }

        private void BrowseExecute()
        {
            try
            {
                System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    StrFileLocation = dlg.SelectedPath;
                }
                dlg = null;
                FindCreateDir(StrFileLocation);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in BrowseExecute " + ex);
            }
        }

        public void FindCreateDir(string dirname)
        {
            try
            {
                if (!Directory.Exists(dirname))
                {
                    DirectoryInfo Di = Directory.CreateDirectory(dirname);
                    Di.Attributes = FileAttributes.ReadOnly;
                    Di.Refresh();
                }
                BCSVCanwrite = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in findCreateDir " + ex);
            }
        }

        private bool WriteCanExecute()
        {
            return BCSVCanwrite;
        }


        /// <summary>
        /// Do the writting
        /// </summary>
        private void WriteExecute()
        {

            DataTable xDatatable = MyDataTable;

            StrPathFile = StrFileLocation + "\\" + StrFileName + ".csv";

            try
            {
                if (MyDataTable.Rows.Count > 0)
                {

                    StreamWriter outFile = new StreamWriter(StrPathFile);

                    List<string> headerValues = new List<string>();
                    foreach (DataColumn column in MyDataTable.Columns)
                    {
                        headerValues.Add(QuoteValue("'" + column.ColumnName));
                    }

                    //Header
                    outFile.WriteLine(string.Join(",", headerValues.ToArray()));

                    foreach (DataRow row in MyDataTable.Rows)
                    {
                        string[] fields = row.ItemArray.Select(field => field.ToString()).ToArray();
                        outFile.WriteLine(String.Join(",", fields));
                    }

                    outFile.Close();
                }

                //At the end
                Settings.Default.CsvFileLocation = StrFileLocation;
                Settings.Default.Save();
                BCSVCanwrite = false;

                
                if (CSVReport.CSVDialog != null)
                {
                    CSVReport.CSVDialog.Close();
                    CSVReport.CSVDialog = null;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in WriteExecute " + ex);
            }
            finally
            {
                MessageBox.Show("DONE!");
            }
        }


        public string QuoteValue(string value)
        {
            return string.Concat("" + value + "");
        }


        private bool ClosedPageCanExecute()
        {
            return true;
        }

        private void ClosedPageExecute()
        {
            //close window
        }

        private bool LoadedPageCanExecute()
        {
            return true;
        }

        /// <summary>
        /// Load Window
        /// </summary>
        private void LoadedPageExecute()
        {
            FindCreateDir(StrFileLocation);
        }

    }
}
