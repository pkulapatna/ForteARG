
using ForteArg.Services;
using ForteARP.Archives_Module.Model;
using ForteARP.Charts;
using ForteARP.Module_FieldsSelect.Views;
using ForteARP.Properties;
using ForteARP.Reports.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;

namespace ForteARP.Archives_Module.ViewModels
{
    public class BaleArchivesViewModel : BindableBase
    {
        public BaleArchivesModel CBaleAchiveModel;
        public Graph01 BaleGraph;
        private SelectItems MyDataItems;
        private Sqlhandler _sqlhandler;

        private AccessHandler _accesshandler;

        private DataTable UnitDataTable = new DataTable();

        private bool _bSelbox;
        public bool BSelbox
        {
            get { return _bSelbox; }
            set { SetProperty(ref _bSelbox, value); }
        }

        private int _pagetable;
        public int PageTable
        {
            get { return _pagetable; }
            set { SetProperty(ref _pagetable, value); }
        }

        private int _totalcount;
        public int Totalcount
        {
            get { return _totalcount; }
            set { SetProperty(ref _totalcount, value); }
        }

        private ObservableCollection<string> _pagecount;
        public ObservableCollection<string> PageCount
        {
            get { return _pagecount; }
            set { SetProperty(ref _pagecount, value); }
        }

        private bool _cmbPagesenable;
        public bool CmbPagesEnable
        {
            get { return _cmbPagesenable; }
            set { SetProperty(ref _cmbPagesenable, value); }
        }


        private bool _blockChecked = true;
        public bool BlockChecked
        {
            get { return _blockChecked; }
            set
            {
                if (value) BoxSelectorOpac = 1.0;
                else BoxSelectorOpac = 0.3;
                SetProperty(ref _blockChecked, value);
            }
        }

        private bool _bUp = false;
        public bool BUp
        {
            get { return _bUp; }
            set { SetProperty(ref _bUp, value); }
        }

        private bool _bDown = false;
        public bool BDown
        {
            get { return _bDown; }
            set { SetProperty(ref _bDown, value); }
        }

        private double _BoxSelectorOpac = 1.0;
        public double BoxSelectorOpac
        {
            get { return _BoxSelectorOpac; }
            set { SetProperty(ref _BoxSelectorOpac, value); }
        }


        //Update status on screen
        private string _strStatus;
        public string StringStatus
        {
            get { return _strStatus; }
            set { SetProperty(ref _strStatus, value); }
        }


        #region TAB CONTROL ////////////////////////////////////////////////////////
        /// <summary>
        /// Tab handle here.
        /// </summary>

        private int iTabSelected = 0;
        private bool _SelectBaleTab;
        public bool SelectBaleTab
        {
            get { return _SelectBaleTab; }
            set
            {
                if (value)
                {
                    iTabSelected = 1;
                    QueryOn = false;
                    WriteOpc = 0.2;
                    LoadPage(iTabSelected);
                    BSelbox = true;
                }
                SetProperty(ref _SelectBaleTab, value);
            }
        }

        private bool _SelectLotTab;
        public bool SelectLotTab
        {
            get { return _SelectLotTab; }
            set
            {
                if (value)
                {
                    iTabSelected = 2;
                    QueryOn = false;
                    WriteOpc = 0.2;
                    BSelbox = false;
                    LoadPage(iTabSelected);
                }
                SetProperty(ref _SelectLotTab, value);
            }
        }

        private bool _selectunitTab;
        public bool SelectUnitTab
        {
            get { return _selectunitTab; }
            set
            {
                if (value)
                {
                    iTabSelected = 3;
                    QueryOn = false;
                    WriteOpc = 0.2;
                    BSelbox = false;
                    LoadPage(iTabSelected);
                }
                SetProperty(ref _selectunitTab, value);
            }
        }

        private bool _SelectQulityTab;
        public bool SelectQulityTab
        {
            get { return _SelectQulityTab; }
            set
            {
                if (value)
                {
                    iTabSelected = 4;
                    QueryOn = false;
                    WriteOpc = 0.2;
                    BSelbox = false;
                    LoadPage(iTabSelected);
                }
                SetProperty(ref _SelectQulityTab, value);
            }
        }


        // Tab 1///////////////////////////////////////////////////////////////////

        private DelegateCommand _selectFieldsCommand;
        public DelegateCommand SelectFieldsCommand =>
            _selectFieldsCommand ?? (_selectFieldsCommand =
            new DelegateCommand(SelectFieldsExecute));
        private void SelectFieldsExecute()
        {
            MyDataItems = new SelectItems();
            MyDataItems.ShowDialog();
        }

        public string StockSelected { get; set; }
        public string GradeSelected { get; set; }
        public string LineSelected { get; set; }
        public string SourceSelected { get; set; }
        public string LotSelected { get; set; }
     
        public bool DefFieldChecked { get; set; }


        private int _recCount;
        public int RecCount
        {
            get { return _recCount; }
            set { SetProperty(ref _recCount, value); }
        }

        private int _lotRecCount;
        public int LotRecCount
        {
            get { return _lotRecCount; }
            set { SetProperty(ref _lotRecCount, value); }
        }

        //LotRecCount


        private string _eventValue;
        public string EventValue
        {
            get { return _eventValue; }
            set
            {
                if (value == "All")
                    QuanEnable = false;
                else
                    QuanEnable = true;
                SetProperty(ref _eventValue, value);
            }
        }

        private bool _quanEnable;
        public bool QuanEnable
        {
            get { return _quanEnable; }
            set { SetProperty(ref _quanEnable, value); }
        }

        private bool _cusFieldChecked;
        public bool CusFieldChecked
        {
            get { return _cusFieldChecked; }
            set 
            { 
                SetProperty(ref _cusFieldChecked, value);
               CustomFieldVis = (value == true) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private bool _allFieldsChecked;
        public bool AllFieldsChecked
        {
            get { return _allFieldsChecked; }
            set
            {
                SetProperty(ref _allFieldsChecked, value);
                if (value) CustomFieldVis = Visibility.Hidden;
            }
        }

        private bool _MonthChecked;
        public bool MonthChecked
        {
            get { return _MonthChecked; }
            set 
            { 
                SetProperty(ref _MonthChecked, value);
                DataSelEnable = true;
                SortOrdEnable = true;
            }
        }

        private bool _daychecked;
        public bool DayChecked
        {
            get { return _daychecked; }
            set 
            { 
                SetProperty(ref _daychecked, value);
                if (value)
                {
                    AllDataCheck = true;
                    SelectSortOrder = 0;
                    MonthChecked = false;
                    DataSelEnable = false;
                    SortOrdEnable = false;
                }       
            }
        }


        private bool _sortOrdEnable;
        public bool SortOrdEnable
        {
            get { return _sortOrdEnable; }
            set { SetProperty(ref _sortOrdEnable, value); }
        }


        private DataTable _archiveDataTable;
        public DataTable ArchiveDataTable
        {
            get { return _archiveDataTable; }
            set
            {
                if (value.Rows.Count > 0) BLotDataexcist = true;
                else BLotDataexcist = false;

                SetProperty(ref _archiveDataTable, value);
            }
        }

        private DataTable _lotDatatoGraph;
        public DataTable LotDatatabletoGraph
        {
            get { return _lotDatatoGraph; }
            set { SetProperty(ref _lotDatatoGraph, value); }
        }


        private int _selectOccr;
        public int SelectOccr
        {
            get { return _selectOccr; }
            set { SetProperty(ref _selectOccr, value); }
        }


        private bool _stockChecked;
        public bool StockChecked
        {
            get { return _stockChecked; }
            set
            {
                SetProperty(ref _stockChecked, value);
                if (value)
                {
                    if (iTabSelected == 1)
                        StockList = CBaleAchiveModel.GetSqlStockList(SelectTableValue);

                    if (iTabSelected == 2)
                        StockList = CBaleAchiveModel.GetSqlStockList(SelectLotTableValue);
                }
                else
                    StockList.Clear();
            }
        }


        private bool _gradeChecked;
        public bool GradeChecked
        {
            get { return _gradeChecked; }
            set
            {
                SetProperty(ref _gradeChecked, value);
                GradeList = CBaleAchiveModel.GetSqlGradeList(SelectTableValue);
            }
        }

        private bool _lineChecked;
        public bool LineChecked
        {
            get { return _lineChecked; }
            set
            {
                SetProperty(ref _lineChecked, value);
                LineList = CBaleAchiveModel.GetSqlLineList(SelectTableValue);
            }
        }

        private bool _sourceChecked;
        public bool SourceChecked
        {
            get { return _sourceChecked; }
            set
            {
                SetProperty(ref _sourceChecked, value);
                SourceList = CBaleAchiveModel.GetSqlSourceList(SelectTableValue);
            }
        }

        private bool _occorChecked;
        public bool OccorChecked
        {
            get { return _occorChecked; }
            set { SetProperty(ref _occorChecked, value); }
        }

        private List<string> _monthtableList;
        public List<string> MonthTableList
        {
            get { return _monthtableList; }
            set { SetProperty(ref _monthtableList, value); }
        }

        private List<string> _occrlist;
        public List<string> Occrlist
        {
            get { return _occrlist; }
            set { SetProperty(ref _occrlist, value); }
        }

        private List<string> _sortOrder;
        public List<string> SortOrder
        {
            get { return _sortOrder; }
            set { SetProperty(ref _sortOrder, value); }
        }

        private int _selectSortOrder = 0;
        public int SelectSortOrder
        {
            get { return _selectSortOrder; }
            set { SetProperty(ref _selectSortOrder, value); }
        }

        private int _selectLotSortOrder = 0;
        public int SelectLotSortOrder
        {
            get { return _selectLotSortOrder; }
            set { SetProperty(ref _selectLotSortOrder, value); }
        }



        private bool _allDataCheck;
        public bool AllDataCheck
        {
            get { return _allDataCheck; }
            set 
            { 
                SetProperty(ref _allDataCheck, value);
                if(value) CusDataBoxVis = Visibility.Hidden;
            }
        }
        private bool _customDataCheck;
        public bool CustomDataCheck
        {
            get { return _customDataCheck; }
            set 
            { 
                SetProperty(ref _customDataCheck, value);
                if (value) CusDataBoxVis = Visibility.Visible;
            }
        }

        private bool _allLotDataCheck;
        public bool AllLotDataCheck
        {
            get { return _allLotDataCheck; }
            set
            {
                SetProperty(ref _allLotDataCheck, value);
                CusLotDataBoxVis = (value == true) ? Visibility.Hidden : Visibility.Visible;
            }
        }
        private bool _customLotDataCheck;
        public bool CustomLotDataCheck
        {
            get { return _customLotDataCheck; }
            set
            {
                SetProperty(ref _customLotDataCheck, value);
                if (value) CusDataBoxVis = Visibility.Visible;
            }
        }



     

        private bool _dataSelEnable;
        public bool DataSelEnable
        {
            get { return _dataSelEnable; }
            set
            {
                SetProperty(ref _dataSelEnable, value);
            }
        }

        //CusLotDataBoxVis

        private Visibility _cusDataBoxVis;
        public Visibility CusDataBoxVis
        {
            get { return _cusDataBoxVis; }
            set { SetProperty(ref _cusDataBoxVis, value); }
        }

        private Visibility _customFieldVis;
        public Visibility CustomFieldVis
        {
            get { return _customFieldVis; }
            set { SetProperty(ref _customFieldVis, value); }
        }

        private Visibility _cusLotDataBoxVis;
        public Visibility CusLotDataBoxVis
        {
            get { return _cusLotDataBoxVis; }
            set { SetProperty(ref _cusLotDataBoxVis, value); }
        }


        private bool _bqueryon;
        public bool BQueryon
        {
            get { return _bqueryon; }
            set { SetProperty(ref _bqueryon, value); }
        }

        
        private int _pageselectidx = 0;
        public int PageSelectIdx
        {
            get { return _pageselectidx; }
            set
            {
                if ((value >= 0) && (value < splittedtables.Length - 1))
                    BUp = true;
                else
                    BUp = false;

                if ((value >= 1) && (value < splittedtables.Length))
                    BDown = true;
                else
                    BDown = false;


                SetProperty(ref _pageselectidx, value);
                if (value > 0)
                    ArchiveDataTable = splittedtables[value];
            }
        }

        private List<string> _stockList;
        public List<string> StockList
        {
            get { return _stockList; }
            set { SetProperty(ref _stockList, value); }
        }

        private List<string> _gradeList;
        public List<string> GradeList
        {
            get { return _gradeList; }
            set { SetProperty(ref _gradeList, value); }
        }

        private List<string> _lineList;
        public List<string> LineList
        {
            get { return _lineList; }
            set { SetProperty(ref _lineList, value); }
        }

        private List<string> _sourceList;
        public List<string> SourceList
        {
            get { return _sourceList; }
            set { SetProperty(ref _sourceList, value); }
        }

        private string _Selectedmonth;
        public string SelectedMonth
        {
            get { return _Selectedmonth; }
            set { SetProperty(ref _Selectedmonth, value); }
        }



        private int _selecttableindex;
        public int SelectTableIndex
        {
            get { return _selecttableindex; }
            set { SetProperty(ref _selecttableindex, value); }
        }

        private string _selectTableValue;
        public string SelectTableValue
        {

            get { return _selectTableValue; }
            set
            {
                SetProperty(ref _selectTableValue, value);

                string strMonth = SelectTableValue.Substring(11, 3);
                string strYear = SelectTableValue.Substring(14, 2);
                SelectedMonth = strMonth + strYear;
                string startDate = "01" + "/" + strMonth + "/" + strYear;

                DateTime.TryParse(startDate, out DateTime dateValue);

                if (StockChecked) StockChecked = false;
                if (GradeChecked) GradeChecked = false;
                if (LineChecked) LineChecked = false;
                if (SourceChecked) SourceChecked = false;

                if (SelectTableIndex == 0)
                {
                    StartQueryDate = dateValue;
                    EndQueryDate = DateTime.Now;
                }
                else
                {
                    StartQueryDate = dateValue;
                    EndQueryDate = dateValue;
                }
            }
        }

        private Nullable<DateTime> _startQueryDate = null;
        public Nullable<DateTime> StartQueryDate
        {
            get
            {
                if (_startQueryDate == null)
                    _startQueryDate = DateTime.Today;
                return _startQueryDate;
            }
            set { SetProperty(ref _startQueryDate, value); }
        }

        private Nullable<DateTime> _endQueryDate = null;
        public Nullable<DateTime> EndQueryDate
        {
            get
            {
                if (_endQueryDate == null)
                    _endQueryDate = DateTime.Today;
                return _endQueryDate;
            }
            set { SetProperty(ref _endQueryDate, value); }
        }

        // End Tab 1 ////////////////////////////////////////////////////////////////
        // Tab 2 ////////////////////////////////////////////////////////////////////

        private List<string> RemoveLotColumnList;


        private string _selectlotTableValue;
        public string SelectLotTableValue
        {
            get { return _selectlotTableValue; }
            set
            {
                SetProperty(ref _selectlotTableValue, value);
               // int iValLength = _selectlotTableValue.Length;

                string strMonth = SelectLotTableValue.Substring(10, 3);
                string strYear = SelectLotTableValue.Substring(13, 2);

                string startDate = "01" + "/" + strMonth + "/" + strYear;

                DateTime.TryParse(startDate, out DateTime dateValue);

                if (SelectTableIndex == 0)
                {
                    StartQueryDate = dateValue;
                    EndQueryDate = DateTime.Now;
                }
                else
                {
                    StartQueryDate = dateValue;
                    EndQueryDate = dateValue;
                }
            }
        }

        private List<string> _lotmonthtableList;
        public List<string> LotMonthTableList
        {
            get { return _lotmonthtableList; }
            set { SetProperty(ref _lotmonthtableList, value); }
        }

        private bool _lotstockChecked;
        public bool LotStockChecked
        {
            get { return _lotstockChecked; }
            set
            {
                SetProperty(ref _lotstockChecked, value);
                StockList = CBaleAchiveModel.GetSqlStockList(SelectLotTableValue);
            }
        }

        private List<string> _lotList;
        public List<string> LotList
        {
            get { return _lotList; }
            set
            {
                SetProperty(ref _lotList, value);
            }
        }

        private bool _lotchecked;
        public bool LotChecked
        {
            get { return _lotchecked; }
            set
            {
                SetProperty(ref _lotchecked, value);
                LotList = CBaleAchiveModel.GetSqlLotList(SelectLotTableValue);
            }
        }


        private bool _LotGradeChecked;
        public bool LotGradeChecked
        {
            get { return _LotGradeChecked; }
            set
            {
                SetProperty(ref _LotGradeChecked, value);
            }
        }


        private bool _lotlineChecked;
        public bool LotLineChecked
        {
            get { return _lotlineChecked; }
            set
            {
                SetProperty(ref _lotlineChecked, value);
                //LineList = CBaleAchiveModel.GetSqlLineList(SelectLotTableValue);
            }
        }

        private bool _lotsourceChecked;
        public bool LotSourceChecked
        {
            get { return _lotsourceChecked; }
            set
            {
                SetProperty(ref _lotsourceChecked, value);
                //SourceList = CBaleAchiveModel.GetSqlSourceList(SelectLotTableValue);
            }
        }


        private int _SelectedBaleIndex;
        public int SelectedBaleIndex
        {
            get { return _SelectedBaleIndex; }
            set
            {
                if (value > -1) BLotDataexcist = true;
                SetProperty(ref _SelectedBaleIndex, value);
            }
        }

        private bool _bLotDataexcist = false;
        public bool BLotDataexcist
        {
            get { return _bLotDataexcist; }
            set
            {
                SetProperty(ref _bLotDataexcist, value);
            }
        }

        private bool _bLocal;
        public bool BLocal
        {
            get { return _bLocal; }
            set { SetProperty(ref _bLocal, value); }
        }



        // End Tab 2 /////////////////////////////////////////////////////////////////


        // Tab 3 Unit ////////////////////////////////////////////////////////////////

        //  UnitMonthTableList
        private List<string> _unitMonthTableList;
        public List<string> UnitMonthTableList
        {
            get { return _unitMonthTableList; }
            set
            {
                SetProperty(ref _unitMonthTableList, value);
            }

        }

        private string _selectUnitTableValue;
        public string SelectUnitTableValue
        {
            get { return _selectUnitTableValue; }
            set
            {
                SetProperty(ref _selectUnitTableValue, value);

                string strMonth = SelectTableValue.Substring(11, 3);
                string strYear = SelectTableValue.Substring(14, 2);
                string startDate = "01" + "/" + strMonth + "/" + strYear;

                DateTime.TryParse(startDate, out DateTime dateValue);

                if (SelectTableIndex == 0)
                {
                    StartQueryDate = dateValue;
                    EndQueryDate = DateTime.Now;
                }
                else
                {
                    StartQueryDate = dateValue;
                    EndQueryDate = dateValue;
                }
            }
        }



        //LotTabEnable
        private bool _LotTabEnable;
        public bool LotTabEnable
        {
            get { return _LotTabEnable; }
            set { SetProperty(ref _LotTabEnable, value); }
        }


        private bool _unitTabEnable;
        public bool UnitTabEnable
        {
            get { return _unitTabEnable; }
            set { SetProperty(ref _unitTabEnable, value); }
        }


        //SelectUnitTableValue

        // Tab 3 Unit End ///////////////////////////////////////////////////////////


        // Tab 4 Qulity  ////////////////////////////////////////////////////////////////
        private bool _qulityTabEnable;
        public bool QulityTabEnable
        {
            get { return _qulityTabEnable; }
            set { SetProperty(ref _qulityTabEnable, value); }
        }


        //QulityMonthTableList
        private List<string> _QulityMonthTableList;
        public List<string> QulityMonthTableList
        {
            get { return _QulityMonthTableList; }
            set { SetProperty(ref _QulityMonthTableList, value); }
        }

        private string _SelectQulityTableValue;
        public string SelectQulityTableValue
        {
            get { return _SelectQulityTableValue; }
            set
            {
                SetProperty(ref _SelectQulityTableValue, value);

                string strMonth = SelectTableValue.Substring(11, 3);
                string strYear = SelectTableValue.Substring(14, 2);
                string startDate = "01" + "/" + strMonth + "/" + strYear;

                DateTime.TryParse(startDate, out DateTime dateValue);

                if (SelectTableIndex == 0)
                {
                    StartQueryDate = dateValue;
                    EndQueryDate = DateTime.Now;
                }
                else
                {
                    StartQueryDate = dateValue;
                    EndQueryDate = dateValue;
                }
            }
        }


        // Tab 4 Qulity ended  ////////////////////////////////////////////////////////////////

        #endregion TAB CONTROL


        private bool _bDataExcist;
        public bool BDataExcist
        {
            get { return _bDataExcist; }
            set { SetProperty(ref _bDataExcist, value); }
        }

        private double _wrtopc;
        public double WriteOpc
        {
            get { return _wrtopc; }
            set { SetProperty(ref _wrtopc, value); }
        }

        private bool _queryOn;
        public bool QueryOn
        {
            get { return _queryOn; }
            set { SetProperty(ref _queryOn, value); }
        }

        private List<string> CustomItemsList { get; set; }

        private DataTable[] splittedtables;


        #region DelegateCommand

        //  Load Page------------------------------------------------    
        
        private DelegateCommand _loadedPageICommand;
        public DelegateCommand LoadedPageICommand =>
           _loadedPageICommand ?? (_loadedPageICommand =
          new DelegateCommand(LoadPageExecute));
        private void LoadPageExecute()
        {
            SetUpArchive();

            try
            {
                MonthChecked = true;

                if (CBaleAchiveModel != null) CBaleAchiveModel = null;
                CBaleAchiveModel = new BaleArchivesModel();
                CBaleAchiveModel.InitSqlBaleArchiveModel();

                RemoveLotColumnList = new List<string>();
                CreateLotFieldListtoRemove();
                QueryOn = false;
                WriteOpc = 0.1;
                MainWindow.AppWindows.SetupAppTitle("Forté Archives From  -> " + _sqlhandler.Host + @"\" + _sqlhandler.SqlInstance);

                LotMonthTableList = CBaleAchiveModel.GetSqlLotTableList();
                if (LotMonthTableList.Count > 0)
                    LotTabEnable = true;
                else
                    LotTabEnable = false;

                UnitMonthTableList = CBaleAchiveModel.GetSqlUnitTableList();
                if (UnitMonthTableList.Count > 0)
                    UnitTabEnable = true;
                else
                    UnitTabEnable = false;

                QulityMonthTableList = CBaleAchiveModel.GetSqlQulityTableList();
                if (QulityMonthTableList.Count > 0)
                    QulityTabEnable = true;
                else
                    QulityTabEnable = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in LoadPageExecute BaleArchivesViewModel " + ex.Message);
            }
        }

        // UnLoad Page ----------------------------------------------

        private DelegateCommand _unLoadedPageICommand;
        public DelegateCommand UnLoadedPageICommand =>
          _unLoadedPageICommand ?? (_unLoadedPageICommand =
         new DelegateCommand(UnLoadPageExecute));
        private void UnLoadPageExecute()
        {
            if (CBaleAchiveModel != null) CBaleAchiveModel = null;
            //  if (CustomConfig != null) CustomConfig = null;
        }

        // Show Graph -----------------------------------------------

        private DelegateCommand _showGraphCommand;
        public DelegateCommand ShowGraphCommand =>
            _showGraphCommand ?? (_showGraphCommand =
           new DelegateCommand(ShowGraphExecute).ObservesCanExecute(() => BLotDataexcist));
        private void ShowGraphExecute()
        {
            string LotIdString = string.Empty;
            string StrItem = string.Empty;
            DateTime Opendate = DateTime.Today;
            DateTime Closedate = DateTime.Today;
            int iCloseHour = 0;

            char[] separators = { ':' };

            string[] words = Settings.Default.DayEndTime.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length > 1) iCloseHour = Convert.ToInt32(words[0]);

            if (ArchiveDataTable.Rows.Count > 0)
            {
                if (SelectedBaleIndex == -1)
                    MessageBox.Show("Please Select lot Number to display graph !");
                else
                {
                    if (ArchiveDataTable.Rows[SelectedBaleIndex]["LotNum"] != null)
                        StrItem = ArchiveDataTable.Rows[SelectedBaleIndex]["LotNum"].ToString();

                    if (ArchiveDataTable.Rows[SelectedBaleIndex]["OpenTD"] != null)
                        Opendate = ArchiveDataTable.Rows[SelectedBaleIndex].Field<DateTime>("OpenTD");

                    if (ArchiveDataTable.Rows[SelectedBaleIndex]["CloseTD"] != null)
                        Closedate = ArchiveDataTable.Rows[SelectedBaleIndex].Field<DateTime>("CloseTD");

                    if (ArchiveDataTable.Rows[SelectedBaleIndex]["FC_IdentString"] != null)
                        LotIdString = ArchiveDataTable.Rows[SelectedBaleIndex].Field<String>("FC_IdentString");

                    if (Opendate.Hour < iCloseHour)
                        Opendate = Opendate.AddDays(1);

                    if (Closedate.Hour < iCloseHour)
                        Closedate = Closedate.AddDays(1);

                    //Show Graph
                    BaleGraph = new Graph01(StrItem, Opendate, Closedate, LotIdString, SelectedMonth);
                    BaleGraph.ShowDialog();
                }
            }
            else
                MessageBox.Show("No data for the Selected month, Please select another month and do new query!");
        }


        private DelegateCommand _showUnitGraphCommand;
        public DelegateCommand ShowUnitGraphCommand =>
           _showUnitGraphCommand ?? (_showUnitGraphCommand =
          new DelegateCommand(ShowGraphUnitExecute).ObservesCanExecute(() => BLotDataexcist));
        private void ShowGraphUnitExecute()
        {
            if (ArchiveDataTable.Rows.Count > 0)
            {
                if (SelectedBaleIndex == -1)
                    MessageBox.Show("Please Select lot Number to display graph !");
                else
                {
                    if (ArchiveDataTable.Rows.Count > 0)
                    {
                        int unit = ArchiveDataTable.Rows[SelectedBaleIndex].Field<int>("UnitNum");
                        int lotnum = ArchiveDataTable.Rows[SelectedBaleIndex].Field<int>("LotNumber");
                        string closeTD = ArchiveDataTable.Rows[SelectedBaleIndex].Field<DateTime>("CloseTD").ToString("dd-MM-yyyy-H-mm");
                        string query = $"SELECT * FROM [ForteData].[dbo].[{SelectTableValue}] with (NOLOCK) WHERE UnitNumber = {unit} AND LotNumber = {lotnum}";
                        DataTable SelUnitTable = CBaleAchiveModel.GetBaleArchiveDataTable(query);

                       
                    }
                }


            }
        }



       




        // Write CSV file -------------------------------------------

        private DelegateCommand _writeCSVAllCommand;
        public DelegateCommand WriteCSVAllCommand =>
             _writeCSVAllCommand ?? (_writeCSVAllCommand =
            new DelegateCommand(WriteCSVAllExecute).ObservesCanExecute(() => QueryOn));
        private void WriteCSVAllExecute()
        {
            int iStart = 9999;
            int iEnd = ArchiveDataTable.Rows.Count;

            try
            {
                switch (iTabSelected)
                {
                    case 1:
                        if (ArchiveDataTable.Rows.Count > 0)
                        {
                            using (CSVReport csvDlg = new CSVReport())
                            {
                                csvDlg.InitCsv(ArchiveDataTable, SelectTableValue, iStart, iEnd);
                                csvDlg.ShowDialog();
                            }
                        }
                        break;

                    case 2:
                        if (SelectedBaleIndex == -1)
                            MessageBox.Show("No data for the Selected month, Please select another month and do new query!");
                        else
                        {
                            WriteLotCsv(iStart, iEnd);
                        }
                        break;

                    case 3:
                        if (SelectedBaleIndex == -1)
                            MessageBox.Show("No data for the Selected month, Please select another month and do new query!");
                        else
                        {
                            if (ArchiveDataTable.Rows.Count > 0) 
                            {
                                int unit  = ArchiveDataTable.Rows[SelectedBaleIndex].Field<int>("UnitNum");
                                int lotnum = ArchiveDataTable.Rows[SelectedBaleIndex].Field<int>("LotNumber");
                                string closeTD = ArchiveDataTable.Rows[SelectedBaleIndex].Field<DateTime>("CloseTD").ToString("dd-MM-yyyy-H-mm");
                                string query = $"SELECT * FROM [ForteData].[dbo].[{SelectTableValue}] with (NOLOCK) WHERE UnitNumber = {unit} AND LotNumber = {lotnum}";
                                DataTable SelUnitTable = CBaleAchiveModel.GetBaleArchiveDataTable(query);

                                using (CSVReport csvDlg = new CSVReport())
                                {
                                    csvDlg.InitCsv(SelUnitTable, $"Unit_{unit} Date_{closeTD}", iStart, iEnd);
                                    csvDlg.ShowDialog();
                                }
                            }
                        }
                        break;

                    default:
                        QueryOn = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in MenuWrite_Click " + ex);
            }
        }


        // Apply Query Data -------------------------------------------
        private DelegateCommand _ApplyCommand; //Start Button
        public DelegateCommand ApplyCommand => _ApplyCommand ?? (_ApplyCommand =
            new DelegateCommand(ApplyExecute));
        private void ApplyExecute()
        {
            BQueryon = true;
            WriteOpc = 1.0;
            int PageDevider = 20;
            DataTable TempTable = new DataTable();

            switch (iTabSelected)
            {
                case 1:
                    // bale Archives
                    CustomItemsList = CBaleAchiveModel.GetCustomXmlTable();
                    TempTable = GetArchivesDataTable(SelectTableValue);

                    if (TempTable.Rows.Count < 1)
                        StringStatus = "No Record Found!";
                    else
                        StringStatus = string.Empty;
                    break;

                case 2:
                    // Lot Archives
                    if (LotMonthTableList.Count > 0)
                    {
                        //TempTable = CBaleAchiveModel.GetLotArchiveDataTable(BuildStringQuery(SelectLotTableValue));

                        TempTable = GetLotArchivesDataTable();

                        if (TempTable.Rows.Count > 0)
                        {
                            foreach (var item in RemoveLotColumnList)
                            {
                                if (RemoveLotColumnList.Contains(item))
                                    TempTable.Columns.Remove(item);
                            }
                            StringStatus = string.Empty;
                        }
                        else
                        {
                            StringStatus = "No Record Found!";
                        }
                    }
                    break;

                case 3: // Unit Archives
                    if (UnitMonthTableList.Count > 0)
                    {
                        BDataExcist = true;

                        TempTable = GetUnitArchivesDataTable();

                        //UnitDataTable

                        if (TempTable.Rows.Count < 1)
                            StringStatus = "No Record Found!";
                        else
                            StringStatus = string.Empty;
                    }
                    break;

                case 4: // Qulity Archives
                    if (QulityMonthTableList.Count > 0)
                    {
                        BDataExcist = true;
                        TempTable = CBaleAchiveModel.GetQulityArchiveDataTable(BuildStringQuery(SelectQulityTableValue));
                        if (TempTable.Rows.Count < 1)
                            StringStatus = "No Record Found!";
                        else
                            StringStatus = string.Empty;
                    }
                    break;

                default:
                    break;

            }

            if (BlockChecked)
            {
                CmbPagesEnable = true;
                //Split table
                splittedtables = TempTable.AsEnumerable()
                                .Select((row, index) => new { row, index })
                                .GroupBy(x => x.index / PageDevider)  // integer division, the fractional part is truncated
                                .Select(g => g.Select(x => x.row).CopyToDataTable())
                                .ToArray();

                if (splittedtables.Length > 1)
                    PageTable = splittedtables.Length;
                else
                    PageTable = 1;

                Totalcount = TempTable.Rows.Count;
                PageCount.Clear();

                for (int i = 0; i < PageTable; i++)
                {
                    PageCount.Add((i + 1).ToString());
                }

                PageSelectIdx = 0;
                if (PageSelectIdx > 0)
                {
                    ArchiveDataTable = splittedtables[PageSelectIdx];
                }
                else
                    ArchiveDataTable = TempTable;
            }
            else
            {
                CmbPagesEnable = false;
                PageCount.Clear();
                PageTable = 1;
                ArchiveDataTable = TempTable;
                Totalcount = TempTable.Rows.Count;
            }
            QueryOn = true;
        }

        #endregion DelegateCommand


        public BaleArchivesViewModel()
        {
            _sqlhandler = Sqlhandler.Instance;
            _sqlhandler.SetSqlParams();
            _sqlhandler.SetConnectionString();

            _accesshandler = AccessHandler.Instance;


            ClsSerilog.LogMessage(ClsSerilog.Info, $"Initialize Archives");
        }

        private void SetUpArchive()
        {
            BQueryon = false;
            WriteOpc = 0.2;

            MonthChecked = true;
            SortOrder = new List<string>() { "Newest", "Oldest" };
            AllDataCheck = true;
            AllFieldsChecked = true;

            AllLotDataCheck = true;
            LotRecCount = 200;

            Occrlist = new List<string>
            {
                "All",
                "Newest",
                "Oldest"
            };

            SelectOccr = 0;

            OccorChecked = true;
            RecCount = 200;

            if (Settings.Default.BSerRemote)
                BLocal = false;
            else
                BLocal = true;
        }
   
        private void WriteLotCsv(int iStart, int iEnd)
        {
            string LotIdString = string.Empty;
            string StrItem = string.Empty;
            DateTime Opendate = DateTime.Today;
            DateTime Closedate = DateTime.Today;

            if (ArchiveDataTable.Rows[SelectedBaleIndex]["LotNum"] != null)
                StrItem = ArchiveDataTable.Rows[SelectedBaleIndex]["LotNum"].ToString();

            if (ArchiveDataTable.Rows[SelectedBaleIndex]["OpenTD"] != null)
                Opendate = ArchiveDataTable.Rows[SelectedBaleIndex].Field<DateTime>("OpenTD");

            if (ArchiveDataTable.Rows[SelectedBaleIndex]["CloseTD"] != null)
                Closedate = ArchiveDataTable.Rows[SelectedBaleIndex].Field<DateTime>("CloseTD");

            if (ArchiveDataTable.Rows[SelectedBaleIndex]["FC_IdentString"] != null)
                LotIdString = ArchiveDataTable.Rows[SelectedBaleIndex]["FC_IdentString"].ToString();

            using (DataTable LotCsvTable = CBaleAchiveModel.GetTableByLotNum(StrItem, SetupStrCSVitems(), Opendate, Closedate, LotIdString, SelectedMonth))
            {
                if (LotCsvTable.Rows.Count > 0)
                {
                    using (CSVReport csvDlg = new CSVReport())
                    {
                        csvDlg.InitCsv(LotCsvTable, $"LOT_{StrItem} Date_{Closedate.Day}_{Closedate.Month}_{Closedate.Year}" + LotIdString, iStart, iEnd);
                        csvDlg.ShowDialog();
                    }
                }
            }
        }

        private string SetupStrCSVitems()
        {
            
            string strQuery = string.Empty;
            char[] charsToTrim = { ',' };

            List<string> CSVItems = new List<string>
            {
                "LotBaleNumber",
                "Weight",
                "Moisture",
                "StockName",
                //"UpCount",
                //"DownCount",
                "TimeComplete",
                "UnitNumber",
                "Forte"
            };

            foreach (var item in CSVItems)
            {
                strQuery += item + ",";
            }
            return strQuery.TrimEnd(charsToTrim);
        }

        private void LoadPage(int iPage)
        {
            if (CBaleAchiveModel == null)
                CBaleAchiveModel = new BaleArchivesModel();
            PageCount = new ObservableCollection<string>();

            BlockChecked = false;

            switch (iPage)
            {
                case 1: // Bale Archive
                    MonthTableList = CBaleAchiveModel.GetSqlTableList();
                    //newest table
                    if (MonthTableList.Count > 0)
                    {
                        BDataExcist = true;
                        SelectTableIndex = 0;
                    }
                    else BDataExcist = false;
                    SelectOccr = 0;
                    CmbPagesEnable = false;

                    break;

                case 2: // Lot Archive
                    LotMonthTableList = CBaleAchiveModel.GetSqlLotTableList();
                    //newest table
                    BlockChecked = false;
                    if (LotMonthTableList.Count > 0)
                    {
                        BDataExcist = true;
                        StringStatus = string.Empty;
                        SelectTableIndex = 0;
                    }
                    else
                    {
                        StringStatus = "NO LOT ARCHIVES";
                        BDataExcist = false;
                    }

                    SelectOccr = 0;
                    CmbPagesEnable = false;
                    break;

                case 3: //Unit Archives
                    BlockChecked = false;
                    if (UnitMonthTableList.Count > 0)
                    {
                        BDataExcist = true;
                        StringStatus = string.Empty;
                        SelectTableIndex = 0;
                    }
                    else
                    {
                        StringStatus = "NO UNIT ARCHIVES";
                        BDataExcist = false;
                    }

                    SelectOccr = 0;
                    CmbPagesEnable = false;
                    break;

                case 4: //Qulity Archives
                        // QulityMonthTableList = CBaleAchiveModel.GetSqlQulityTableList();
                    if (QulityMonthTableList.Count > 0)
                    {
                        BDataExcist = true;
                        StringStatus = string.Empty;
                        SelectTableIndex = 0;
                    }
                    else
                    {
                        StringStatus = "NO QULITY ARCHIVES";
                        BDataExcist = false;
                    }
                    SelectOccr = 0;
                    CmbPagesEnable = false;
                    break;

                default:
                    break;
            }
            if (ArchiveDataTable != null)
                ArchiveDataTable.Clear();
            BLotDataexcist = false;
        }


        private DataTable GetUnitArchivesDataTable()
        {
            DataTable UnitTable = new DataTable();
            string strUnitQuery = string.Empty;
            List<string> SelMonthlst;

            string strItems = string.Empty;
            string oldorNew = string.Empty;

            if(EventValue == "All")
            {
                strItems = $"*";
                oldorNew = $"ORDER by [CloseTD] DESC";
            }
            else if (EventValue == "Newest")
            {
                strItems = $"TOP {RecCount} * ";
                oldorNew = $"ORDER by [CloseTD] DESC";
            }
            else if (EventValue == "Oldest")
            {
                strItems = $"TOP {RecCount} * ";
                oldorNew = $"ORDER by [CloseTD] ASC";
            }

            try
            {
                if (MonthChecked)
                {
                    strUnitQuery = $"SELECT {strItems} from [ForteData].[dbo].[{SelectUnitTableValue}] with (NOLOCK) {oldorNew}";
                    UnitTable = CBaleAchiveModel.GetUnitArchiveDataTable(strUnitQuery);
                }
                if (DayChecked)
                {
                    if (EndQueryDate > DateTime.Now)
                        EndQueryDate = DateTime.Now;

                    if (_endQueryDate.Value.Month >= _startQueryDate.Value.Month)
                    {
                        SelMonthlst = new List<string>();
                        List<DataTable> MyUnitTables = new List<DataTable>();

                        for (int i = 0; i < ((_endQueryDate.Value.Month + 1) - _startQueryDate.Value.Month); i++)
                        {
                            SelMonthlst.Add(ClassCommon.LotMonth[_startQueryDate.Value.Month - 1 + i] + _startQueryDate.Value.ToString("yy"));
                        }
                        for (int i = 0; i < SelMonthlst.Count; i++)
                        {
                            string strsqr = BuildUnitStringQuery(SelMonthlst[i], strItems, oldorNew);

                            MyUnitTables.Add(CBaleAchiveModel.GetBaleArchiveDataTable(strsqr));
                        }
                        DataTable datAll = new DataTable();
                        datAll = MyUnitTables[0].Copy();

                        for (int i = 1; i < MyUnitTables.Count; i++)
                        {
                            datAll.Merge(MyUnitTables[i]);
                            datAll.AcceptChanges();
                        }
                        datAll.AcceptChanges();
                        UnitTable = datAll;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in GetUnitArchivesDataTable " + ex.Message);
            }
            return UnitTable;
        }

        private DataTable GetLotArchivesDataTable()
        {
            DataTable MyLotTable = new DataTable();
            List<string> SelMonthlst;
            _ = new List<DataTable>();

            try
            {
                if (MonthChecked)
                {
                    string QuaryString = BuildLotStringQuery(SelectLotTableValue);
                    MyLotTable = CBaleAchiveModel.GetBaleArchiveDataTable(QuaryString);
                }
                if (DayChecked)
                {
                    if (EndQueryDate > DateTime.Now)
                        EndQueryDate = DateTime.Now;

                    if (_endQueryDate.Value.Month > _startQueryDate.Value.Month)
                    {
                        SelMonthlst = new List<string>();
                        List<DataTable> MyLotTables = new List<DataTable>();

                        for (int i = 0; i < ((_endQueryDate.Value.Month + 1) - _startQueryDate.Value.Month); i++)
                        {
                            SelMonthlst.Add(ClassCommon.LotMonth[_startQueryDate.Value.Month - 1 + i] + _startQueryDate.Value.ToString("yy"));
                        }
                        for (int i = 0; i < SelMonthlst.Count; i++)
                        {
                            string strsqr = BuildLotStringQuery(SelMonthlst[i]);
                            MyLotTables.Add(CBaleAchiveModel.GetBaleArchiveDataTable(strsqr));
                        }
                        DataTable datAll = new DataTable();
                        datAll = MyLotTables[0].Copy();

                        for (int i = 1; i < MyLotTables.Count; i++)
                        {
                            datAll.Merge(MyLotTables[i]);
                            datAll.AcceptChanges();
                        }
                        datAll.AcceptChanges();
                        MyLotTable = datAll;
                    }
                }
            }
            catch (Exception ex )
            {
                MessageBox.Show("ERROR in GetLotArchivesDataTable " + ex.Message);
            }
            return MyLotTable;
        }

        
        private DataTable GetArchivesDataTable(string selectTableValue)
        {
            DataTable MyTable = new DataTable();

            List<string> SelMonthlst;
            List<DataTable> MyTables;
            
            if (MonthChecked)
            {
                string QuaryString = BuildStringQuery(selectTableValue);
                MyTable = CBaleAchiveModel.GetBaleArchiveDataTable(QuaryString);
            }
            if (DayChecked) //Day check can go across months
            {
                if (EndQueryDate > DateTime.Now)
                    EndQueryDate = DateTime.Now;

                if (_endQueryDate.Value.Month > _startQueryDate.Value.Month)
                {
                    SelMonthlst = new List<string>();
                    MyTables = new List<DataTable>();

                    try
                    {
                        for (int i = 0; i < ((_endQueryDate.Value.Month + 1) - _startQueryDate.Value.Month); i++)
                        {
                            SelMonthlst.Add(ClassCommon.RtMonth[_startQueryDate.Value.Month - 1 + i] + _startQueryDate.Value.ToString("yy"));
                        }
                        for (int i = 0; i < SelMonthlst.Count; i++)
                        {
                            string strsqr = BuildStringQuery(SelMonthlst[i]);
                            MyTables.Add(CBaleAchiveModel.GetBaleArchiveDataTable(strsqr));
                        }

                        DataTable datAll = new DataTable();
                        datAll = MyTables[0].Copy();
                        for (int i = 1; i < MyTables.Count; i++)
                        {
                            datAll.Merge(MyTables[i]);
                            datAll.AcceptChanges();
                        }
                        if (datAll.Rows.Count > 0)
                        {
                            DateTime DTrRemove;
                            foreach (DataRow row in datAll.Rows)
                            {
                                DTrRemove = Convert.ToDateTime(row["TimeStart"].ToString());
                                if (DTrRemove.Date < _startQueryDate.Value.Date)
                                {
                                    row.Delete();
                                }
                            }
                            datAll.AcceptChanges();

                            foreach (DataRow row in datAll.Rows)
                            {
                                DTrRemove = Convert.ToDateTime(row["TimeStart"].ToString());
                                if (DTrRemove.Date > _endQueryDate.Value.Date)
                                {
                                    row.Delete();
                                }
                            }
                            datAll.AcceptChanges();
                        }
                        MyTable = datAll;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ERROR in GetArchivesDataTable Multiple Month" + ex.Message);
                    }
                }
                else
                {
                    String QuaryString = BuildStringQuery(selectTableValue);
                    MyTable = CBaleAchiveModel.GetBaleArchiveDataTable(QuaryString);
                }
            }

            return MyTable;
        }

        private string BuildStringQuery(string strTable)
        {
            string strQuantity;
            string strQueryFields = string.Empty;
            string strOrder;
            string strsgls = string.Empty;
            string strTimeFrame;

            string strBaleQuan = string.Empty;

            List<string> strBoxes = new List<string>();
            List<string> AllItemsList = CBaleAchiveModel.GetAllItemsListModel();
            

            if (iTabSelected == 1) // bale Archives
            {
                strQuantity = (AllDataCheck == true) ? "SELECT " : "SELECT TOP " + RecCount;

                if (AllFieldsChecked)
                {
                    if (AllItemsList.Count > 0)
                    {
                        string AllList = string.Empty;

                        foreach (var item in AllItemsList)
                        {
                            AllList = AllList + item + ",";
                        }
                        AllList = AllList.Remove(AllList.Length - 1);

                        strQueryFields = AllList + " ";
                    }
                    else
                        strQueryFields = " * ";
                }
                if (CusFieldChecked)
                {
                    string customList = string.Empty;

                    if (CustomItemsList.Count > 0)
                    {
                        foreach (var item in CustomItemsList)
                        {
                            customList = customList + item + ",";
                        }
                        customList = customList.Remove(customList.Length - 1);
                        strQueryFields = customList + " ";
                    }
                    else
                        strQueryFields = "*";
                }

                if (DefFieldChecked)
                {
                    string customList = String.Empty;

                    DataTable DefaultTable = _accesshandler.LVHdrFmtBale;

                    if (DefaultTable.Rows.Count > 0)
                    {
                        foreach (DataRow Row in DefaultTable.Rows)
                        {
                            CustomItemsList.Add(Row[0].ToString());
                        }
                    }

                    if (CustomItemsList.Count > 0)
                    {
                        foreach (var item in CustomItemsList)
                        {
                            customList = customList + item + ",";
                        }
                        customList = customList.Remove(customList.Length - 1);
                        strQueryFields = customList + " ";
                    }
                    else
                        strQueryFields = "*";
                }

                // 3 from stock,grade,line or/and source
                if (StockChecked)
                    strBoxes.Add(" StockName = '" + StockSelected + "'");
                if (GradeChecked)
                    strBoxes.Add(" GradeName = '" + GradeSelected + "'");
                if (LineChecked)
                    strBoxes.Add(" LineID = '" + LineSelected + "'");
                if (SourceChecked)
                    strBoxes.Add(" SourceID = '" + SourceSelected + "'");

                if (strBoxes.Count > 0)
                {
                    foreach (var item in strBoxes)
                    {
                        strsgls = strsgls + item + " and ";
                    }
                    strsgls = " WHERE " + strsgls;
                    strsgls = strsgls.Remove(strsgls.Length - 5);
                }

                strOrder = (SelectSortOrder == 0) ? " ORDER BY [Index] DESC;" : " ORDER BY [Index] ASC;";

              
                if (DayChecked)
                {
                    string strStartTime = "00:00";
                    string strEndTime = "23:59";

                    strOrder = " ORDER BY [Index] ASC;";

                    if (EndQueryDate > DateTime.Now)
                        EndQueryDate = DateTime.Now;

                    string strStartDate = StartQueryDate.Value.Date.ToString("MM/dd/yyyy") + " " + strStartTime;
                    string strEndDate = EndQueryDate.Value.Date.AddDays(1).ToString("MM/dd/yyyy") + " " + strEndTime;

                    if (strBoxes.Count > 0)
                    {
                        strTimeFrame = " AND CAST(TimeStart AS DATETIME) BETWEEN '" + strStartDate + "' and '" + strEndDate + "' ";
                    }
                    else
                        strTimeFrame = " WHERE CAST(TimeStart AS DATETIME) BETWEEN '" + strStartDate + "' and '" + strEndDate + "' ";
                }
                else
                    strTimeFrame = string.Empty;

                strBaleQuan = strQuantity + " " + strQueryFields + " FROM " + strTable + " with (NOLOCK) " + strsgls + strTimeFrame + strOrder;

            }
            
            return strBaleQuan;
        }

        private string BuildUnitStringQuery(string selectUnitTableValue, string items, string order)
        {
            List<string> strBoxes = new List<string>();
            string strTimeFrame = string.Empty;
            string strUnitQuery;

            if (DayChecked)
            {
                string strStartTime = "00:00";
                string strEndTime = "23:59";
                if (EndQueryDate > DateTime.Now)
                    EndQueryDate = DateTime.Now;

                string strStartDate = StartQueryDate.Value.Date.ToString("MM/dd/yyyy") + " " + strStartTime;
                string strEndDate = EndQueryDate.Value.Date.ToString("MM/dd/yyyy") + " " + strEndTime;

                if (strBoxes.Count > 0)
                {
                    strTimeFrame = " AND CAST(OpenTD AS DATETIME) BETWEEN '" + strStartDate + "' and '" + strEndDate + "' ";
                }
                else
                    strTimeFrame = " WHERE CAST(OpenTD AS DATETIME) BETWEEN '" + strStartDate + "' and '" + strEndDate + "' ";

                strUnitQuery = $"SELECT {items} from [ForteData].[dbo].[{SelectUnitTableValue}] with (NOLOCK) {strTimeFrame} {order}";
            }
            else
                strUnitQuery = $"SELECT {items} from {SelectUnitTableValue} with (NOLOCK) {strTimeFrame} {order}";

            return strUnitQuery;
        }



        private string BuildLotStringQuery(string strTable)
        {
            
            string strQuantity;
            string strQueryFields;
            string strOrder;
            string strsgls = string.Empty;
            string strTimeFrame;
            
            List<string> strBoxes = new List<string>();

            strQuantity = (AllLotDataCheck == true) ? "SELECT " : "SELECT TOP " + LotRecCount;

            strQueryFields = "*";

            // 3 from stock,grade,line or/and source
            if (StockChecked)
                strBoxes.Add(" StockName = '" + StockSelected + "'");
            if (GradeChecked)
                strBoxes.Add(" GradeName = '" + GradeSelected + "'");
            if (LineChecked)
                strBoxes.Add(" LineID = '" + LineSelected + "'");
            if (SourceChecked)
                strBoxes.Add(" SourceID = '" + SourceSelected + "'");

            if (strBoxes.Count > 0)
            {
                foreach (var item in strBoxes)
                {
                    strsgls = strsgls + item + " and ";
                }
                strsgls = " WHERE " + strsgls;
                strsgls = strsgls.Remove(strsgls.Length - 5);
            }

            strOrder = (SelectSortOrder == 0) ? " ORDER BY [Index] DESC;" : " ORDER BY [Index] ASC;";

            if (strBoxes.Count > 0)
            {
                foreach (var item in strBoxes)
                {
                    strsgls = strsgls + item + " and ";
                }
                strsgls = " WHERE " + strsgls;
                strsgls = strsgls.Remove(strsgls.Length - 5);
            }

            strOrder = (SelectSortOrder == 0) ? " ORDER BY [Index] DESC;" : " ORDER BY [Index] ASC;";

            if (DayChecked)
            {
                string strStartTime = "00:00";
                string strEndTime = "23:59";

                strOrder = " ORDER BY [Index] ASC;";

                if (EndQueryDate > DateTime.Now)
                    EndQueryDate = DateTime.Now;

                string strStartDate = StartQueryDate.Value.Date.ToString("MM/dd/yyyy") + " " + strStartTime;
                string strEndDate = EndQueryDate.Value.Date.AddDays(1).ToString("MM/dd/yyyy") + " " + strEndTime;

                if (strBoxes.Count > 0)
                {
                    strTimeFrame = " AND CAST(CloseTD AS DATETIME) BETWEEN '" + strStartDate + "' and '" + strEndDate + "' ";
                }
                else
                    strTimeFrame = " WHERE CAST(CloseTD AS DATETIME) BETWEEN '" + strStartDate + "' and '" + strEndDate + "' ";
            }
            else
                strTimeFrame = string.Empty;


            return strQuantity + " " + strQueryFields + " FROM " + strTable + " with (NOLOCK) " + strsgls + strTimeFrame + strOrder;
        }



        private void CreateLotFieldListtoRemove()
        {
            if (RemoveLotColumnList.Count > 0) RemoveLotColumnList.Clear();
            else
            {
                RemoveLotColumnList.Add("Empty");
                RemoveLotColumnList.Add("PriGrp");
                RemoveLotColumnList.Add("SecGrp");
                RemoveLotColumnList.Add("OnHold");
                RemoveLotColumnList.Add("JobNum");
                RemoveLotColumnList.Add("NW2N");
                RemoveLotColumnList.Add("MeanNW");
                RemoveLotColumnList.Add("StdDevNW");
                RemoveLotColumnList.Add("MC2M");
                RemoveLotColumnList.Add("AsciiFld1");
                RemoveLotColumnList.Add("AsciiFld2");
                RemoveLotColumnList.Add("AsciiFld3");
                RemoveLotColumnList.Add("AsciiFld4");
                RemoveLotColumnList.Add("SpareSngFld1");
                RemoveLotColumnList.Add("SpareSngFld2");
                RemoveLotColumnList.Add("SpareSngFld3");
                RemoveLotColumnList.Add("CloseBySize");
                RemoveLotColumnList.Add("CloseByTime");
                RemoveLotColumnList.Add("NextBaleNumber");
            }
        }


    }
}
