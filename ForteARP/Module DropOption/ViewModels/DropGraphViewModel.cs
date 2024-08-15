

using ForteArg.Services;
using ForteARP.Charts;
using ForteARP.Model;
using ForteARP.Module_DropOption.Model;
using ForteARP.Properties;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace ForteARP.Module_DropOption.ViewModels
{
    public class DropGraphViewModel : BindableBase
    {
        protected readonly Prism.Events.IEventAggregator _eventAggregator;

        private readonly DropModel _dropModel;
        private Sqlhandler _sqlhandler;
        public DelegateCommand LoadedPageICommand { get; set; }
        public DelegateCommand ClosedPageICommand { get; set; }
        public DelegateCommand StartCommand { get; set; }       //Start Button
        public DelegateCommand StopCommand { get; set; }        //Stop Button
        public DelegateCommand ReDrawCommand { get; set; } //Re-Draw Graph

        //private int m_iMoistureType;
        //private int m_iWeightType;
        private int iMenuSelect;

        private bool _rtrunning = false;
        public bool RTRunning
        {
            get { return _rtrunning; }
            set
            {
                RTIdle = !value;
                SetProperty(ref _rtrunning, value);
            }
        }

        private bool _RTIdle = true;
        public bool RTIdle
        {
            get { return _RTIdle; }
            set { SetProperty(ref _RTIdle, value); }
        }


        private double _opac = 1.0;
        public double Opac
        {
            get { return _opac; }
            set { SetProperty(ref _opac, value); }
        }

        private string GraphtypeSelect = string.Empty;


        private string _updateinfo;
        public string UpdateInfo
        {
            get { return _updateinfo; }
            set { SetProperty(ref _updateinfo, value); }
        }


        private Visibility _show12 = Visibility.Hidden;
        public Visibility Show12
        {
            get { return _show12; }
            set { SetProperty(ref _show12, value); }
        }
        private Visibility _show11 = Visibility.Hidden;
        public Visibility Show11
        {
            get { return _show11; }
            set { SetProperty(ref _show11, value); }
        }
        private Visibility _show10 = Visibility.Hidden;
        public Visibility Show10
        {
            get { return _show10; }
            set { SetProperty(ref _show10, value); }
        }
        private Visibility _show9 = Visibility.Hidden;
        public Visibility Show9
        {
            get { return _show9; }
            set { SetProperty(ref _show9, value); }
        }
        private Visibility _show8 = Visibility.Hidden;
        public Visibility Show8
        {
            get { return _show8; }
            set { SetProperty(ref _show8, value); }
        }
        private Visibility _show7 = Visibility.Hidden;
        public Visibility Show7
        {
            get { return _show7; }
            set { SetProperty(ref _show7, value); }
        }
        private Visibility _show6 = Visibility.Hidden;
        public Visibility Show6
        {
            get { return _show6; }
            set { SetProperty(ref _show6, value); }
        }
        private Visibility _show5 = Visibility.Hidden;
        public Visibility Show5
        {
            get { return _show5; }
            set { SetProperty(ref _show5, value); }
        }
        private Visibility _show4 = Visibility.Hidden;
        public Visibility Show4
        {
            get { return _show4; }
            set { SetProperty(ref _show4, value); }
        }
        private Visibility _show3 = Visibility.Hidden;
        public Visibility Show3
        {
            get { return _show3; }
            set { SetProperty(ref _show3, value); }
        }
        private Visibility _show2 = Visibility.Hidden;
        public Visibility Show2
        {
            get { return _show2; }
            set { SetProperty(ref _show2, value); }
        }
        private Visibility _show1 = Visibility.Hidden;
        public Visibility Show1
        {
            get { return _show1; }
            set { SetProperty(ref _show1, value); }
        }

        public int BalesInOneDrop { get; set; }


        private string _ItemUnit;
        public string ItemUnit
        {
            get { return _ItemUnit; }
            set { SetProperty(ref _ItemUnit, value); }
        }


        private string nLine;
        public string NLine
        {
            get { return nLine; }
            set { SetProperty(ref nLine, value); }
        }

        private string nSource;
        public string NSource
        {
            get { return nSource; }
            set { SetProperty(ref nSource, value); }
        }




        #region Check Boxes


        private bool _allboxesEnable = false;
        public bool AllboxesEnable
        {
            get { return _allboxesEnable; }
            set { SetProperty(ref _allboxesEnable, value); }
        }


        private bool Drop1Active { get; set; }
        private bool Drop2Active { get; set; }
        private bool Drop3Active { get; set; }
        private bool Drop4Active { get; set; }
        private bool Drop5Active { get; set; }
        private bool Drop6Active { get; set; }
        private bool Drop7Active { get; set; }
        private bool Drop8Active { get; set; }
        private bool Drop9Active { get; set; }
        private bool Drop10Active { get; set; }
        private bool Drop11Active { get; set; }
        private bool Drop12Active { get; set; }


        private bool _AllBoxesChecked;
        public bool AllBoxesChecked
        {
            get { return _AllBoxesChecked; }
            set
            {
                SetProperty(ref _AllBoxesChecked, value);
                if (value)
                {
                    AllboxesEnable = false;
                    if (Drop1Active) BoxOneChecked = true;
                    if (Drop2Active) BoxtwoChecked = true;
                    if (Drop3Active) BoxthreeChecked = true;
                    if (Drop4Active) BoxfourChecked = true;
                    if (Drop5Active) BoxfiveChecked = true;
                    if (Drop6Active) BoxsixChecked = true;
                    if (Drop7Active) BoxsevenChecked = true;
                    if (Drop8Active) BoxeightChecked = true;
                    if (Drop9Active) BoxnineChecked = true;
                    if (Drop10Active) BoxtenChecked = true;
                    if (Drop11Active) BoxelevenChecked = true;
                    if (Drop12Active) BoxtwelveChecked = true;

                    //GetRealtimeData();
                }
                else
                {
                    AllboxesEnable = true;

                    if (Drop1Active) BoxOneChecked = false;
                    if (Drop2Active) BoxtwoChecked = false;
                    if (Drop3Active) BoxthreeChecked = false;
                    if (Drop4Active) BoxfourChecked = false;
                    if (Drop5Active) BoxfiveChecked = false;
                    if (Drop6Active) BoxsixChecked = false;
                    if (Drop7Active) BoxsevenChecked = false;
                    if (Drop8Active) BoxeightChecked = false;
                    if (Drop9Active) BoxnineChecked = false;
                    if (Drop10Active) BoxtenChecked = false;
                    if (Drop11Active) BoxelevenChecked = false;
                    if (Drop12Active) BoxtwelveChecked = false;

                }
            }
        }
        private bool _BoxOneChecked;
        public bool BoxOneChecked
        {
            get { return _BoxOneChecked; }
            set
            {
                SetProperty(ref _BoxOneChecked, value);
                bDropOn[0] = value;

                if (!AllBoxesChecked)
                    if (RTRunning) GetRealtimeData();
            }
        }

        private bool _BoxtwoChecked;
        public bool BoxtwoChecked
        {
            get { return _BoxtwoChecked; }
            set
            {
                SetProperty(ref _BoxtwoChecked, value);
                bDropOn[1] = value;
                if (!AllBoxesChecked)
                    if (RTRunning) GetRealtimeData();
            }
        }
        private bool _BoxthreeChecked;
        public bool BoxthreeChecked
        {
            get { return _BoxthreeChecked; }
            set
            {
                SetProperty(ref _BoxthreeChecked, value);
                bDropOn[2] = value;
                if (!AllBoxesChecked)
                    if (RTRunning) GetRealtimeData();

            }
        }
        private bool _BoxfourChecked;
        public bool BoxfourChecked
        {
            get { return _BoxfourChecked; }
            set
            {
                SetProperty(ref _BoxfourChecked, value);
                bDropOn[3] = value;
                if (!AllBoxesChecked)
                    if (RTRunning) GetRealtimeData();
            }
        }
        private bool _BoxfiveChecked;
        public bool BoxfiveChecked
        {
            get { return _BoxfiveChecked; }
            set
            {
                SetProperty(ref _BoxfiveChecked, value);
                bDropOn[4] = value;
                if (!AllBoxesChecked)
                    if (RTRunning) GetRealtimeData();
            }
        }
        private bool _BoxsixChecked;
        public bool BoxsixChecked
        {
            get { return _BoxsixChecked; }
            set
            {
                SetProperty(ref _BoxsixChecked, value);
                bDropOn[5] = value;
                if (!AllBoxesChecked)
                    if (RTRunning) GetRealtimeData();
            }
        }
        private bool _BoxsevenChecked;
        public bool BoxsevenChecked
        {
            get { return _BoxsevenChecked; }
            set
            {
                SetProperty(ref _BoxsevenChecked, value);
                bDropOn[6] = value;
                if (!AllBoxesChecked)
                    if (RTRunning) GetRealtimeData();
            }
        }
        private bool _BoxeightChecked;
        public bool BoxeightChecked
        {
            get { return _BoxeightChecked; }
            set
            {
                SetProperty(ref _BoxeightChecked, value);
                bDropOn[7] = value;
                if (!AllBoxesChecked)
                    if (RTRunning) GetRealtimeData();
            }
        }
        private bool _BoxnineChecked;
        public bool BoxnineChecked
        {
            get { return _BoxnineChecked; }
            set
            {
                SetProperty(ref _BoxnineChecked, value);
                bDropOn[8] = value;
                if (!AllBoxesChecked)
                    if (RTRunning) GetRealtimeData();
            }
        }
        private bool _BoxtenChecked;
        public bool BoxtenChecked
        {
            get { return _BoxtenChecked; }
            set
            {
                SetProperty(ref _BoxtenChecked, value);
                bDropOn[9] = value;
                if (!AllBoxesChecked)
                    if (RTRunning) GetRealtimeData();
            }
        }
        private bool _BoxelevenChecked;
        public bool BoxelevenChecked
        {
            get { return _BoxelevenChecked; }
            set
            {
                SetProperty(ref _BoxelevenChecked, value);
                bDropOn[10] = value;
                if (!AllBoxesChecked)
                    if (RTRunning) GetRealtimeData();
            }
        }
        private bool _BoxtwelveChecked;
        public bool BoxtwelveChecked
        {
            get { return _BoxtwelveChecked; }
            set
            {
                SetProperty(ref _BoxtwelveChecked, value);
                bDropOn[11] = value;
                if (!AllBoxesChecked)
                    if (RTRunning) GetRealtimeData();
            }
        }

        #endregion  Check Boxes

        private void GetRealtimeData()
        {
            CurrentBaleTable = _dropModel.CurrentBaleTable;
            PreviousTable = _dropModel.GetPreviousTable();
            GetDropDataArray(bDropOn);
            ProcessDatTable();
        }

        private void GetDropDataArray(List<bool> ActiveList)
        {
            string strSourceLine = $"AND LineID ={LineList[SelectSourceIndex]} AND SourceID ={SourceList[SelectSourceIndex]}";

            try
            {
                //loop 
                for (int i = 1; i < m_BalesinDrop + 1; i++)
                {
                    if (ActiveList[i - 1])
                    {
                       
                        string strQuery = $"SELECT TOP  {DropSamples} Position,Moisture,Weight,BDWeight,NetWeight,Dirt,Brightness," +
                            $"Finish,Forte,LotNumber,LotBaleNumber,SerialNumber,DropNumber,[index] " +
                            $"FROM dbo.[{CurrentBaleTable}] WHERE Position ={i} {strSourceLine}  ORDER BY [TimeStart] DESC; ";

                        DroptableList[i - 1].Clear();
                        DroptableList[i - 1] = _dropModel.GetNewDropGaphDataTable(strQuery);
                    }
                }

                if(DroptableList[0].Rows[0]["DropNumber"] != null)
                 GrpStd = DroptableList[0].Rows[0]["DropNumber"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in GetDropDataArray " + ex.Message);
            }
        }

 
        private void ProcessDatTable()
        {
            int iRow;
            SetNewCharts();
            List<double> ditems = new List<double>();
            //bool bBlankData = true;

            try
            {
                if ((DroptableList[0].Rows.Count > 0) && (bDropOn[0]))
                {
                    iRow = 1;
                    foreach (DataRow item in DroptableList[0].Rows)
                    {
                        Pos1.Add(new ChartData()
                        {
                            Index = iRow,
                            Value = Convert.ToDouble(item[GraphtypeSelect]),
                            ChartColor = Brushes.Green
                        });
                        ditems.Add(Convert.ToDouble(item[GraphtypeSelect]));
                        iRow += 1;
                    }

                }

                if ((DroptableList[1].Rows.Count > 0) && (bDropOn[1]))
                {
                    iRow = 1;
                    foreach (DataRow item in DroptableList[1].Rows)
                    {
                        Pos2.Add(new ChartData()
                        {
                            Index = iRow,
                            Value = Convert.ToDouble(item[GraphtypeSelect]),
                            ChartColor = Brushes.Aqua
                        });
                        ditems.Add(Convert.ToDouble(item[GraphtypeSelect]));
                        iRow += 1;
                    }
                }

                if ((DroptableList[2].Rows.Count > 0) && (bDropOn[2]))
                {
                    iRow = 1;
                    foreach (DataRow item in DroptableList[2].Rows)
                    {
                        Pos3.Add(new ChartData()
                        {
                            Index = iRow,
                            Value = Convert.ToDouble(item[GraphtypeSelect]),
                            ChartColor = Brushes.Red
                        });
                        ditems.Add(Convert.ToDouble(item[GraphtypeSelect]));
                        iRow += 1;
                    }
                }

                if ((DroptableList[3].Rows.Count > 0) && (bDropOn[3]))
                {
                    iRow = 1;
                    foreach (DataRow item in DroptableList[3].Rows)
                    {
                        Pos4.Add(new ChartData()
                        {
                            Index = iRow,
                            Value = Convert.ToDouble(item[GraphtypeSelect]),
                            ChartColor = Brushes.Yellow
                        });
                        ditems.Add(Convert.ToDouble(item[GraphtypeSelect]));
                        iRow += 1;
                    }
                }

                if ((DroptableList[4].Rows.Count > 0) && (bDropOn[4]))
                {
                    iRow = 1;
                    foreach (DataRow item in DroptableList[4].Rows)
                    {
                        Pos5.Add(new ChartData()
                        {
                            Index = iRow,
                            Value = Convert.ToDouble(item[GraphtypeSelect]),
                            ChartColor = Brushes.Purple
                        });
                        ditems.Add(Convert.ToDouble(item[GraphtypeSelect]));
                        iRow += 1;
                    }
                }

                if ((DroptableList[5].Rows.Count > 0) && (bDropOn[5]))
                {
                    iRow = 1;
                    foreach (DataRow item in DroptableList[5].Rows)
                    {
                        Pos6.Add(new ChartData()
                        {
                            Index = iRow,
                            Value = Convert.ToDouble(item[GraphtypeSelect]),
                            ChartColor = Brushes.White
                        });
                        ditems.Add(Convert.ToDouble(item[GraphtypeSelect]));
                        iRow += 1;
                    }
                }

                if ((DroptableList[6].Rows.Count > 0) && (bDropOn[6]))
                {
                    iRow = 1;
                    foreach (DataRow item in DroptableList[6].Rows)
                    {
                        Pos7.Add(new ChartData()
                        {
                            Index = iRow,
                            Value = Convert.ToDouble(item[GraphtypeSelect]),
                            ChartColor = Brushes.Gold
                        });
                        ditems.Add(Convert.ToDouble(item[GraphtypeSelect]));
                        iRow += 1;
                    }
                }

                if ((DroptableList[7].Rows.Count > 0) && (bDropOn[7]))
                {
                    iRow = 1;
                    foreach (DataRow item in DroptableList[7].Rows)
                    {
                        Pos8.Add(new ChartData()
                        {
                            Index = iRow,
                            Value = Convert.ToDouble(item[GraphtypeSelect]),
                            ChartColor = Brushes.Gray
                        });
                        ditems.Add(Convert.ToDouble(item[GraphtypeSelect]));
                        iRow += 1;
                    }
                }

                if ((DroptableList[8].Rows.Count > 0) && (bDropOn[8]))
                {
                    iRow = 1;
                    foreach (DataRow item in DroptableList[8].Rows)
                    {
                        Pos9.Add(new ChartData()
                        {
                            Index = iRow,
                            Value = Convert.ToDouble(item[GraphtypeSelect]),
                            ChartColor = Brushes.Aquamarine
                        });
                        ditems.Add(Convert.ToDouble(item[GraphtypeSelect]));
                        iRow += 1;
                    }
                }

                if ((DroptableList[9].Rows.Count > 0) && (bDropOn[9]))
                {
                    iRow = 1;
                    foreach (DataRow item in DroptableList[9].Rows)
                    {
                        Pos10.Add(new ChartData()
                        {
                            Index = iRow,
                            Value = Convert.ToDouble(item[GraphtypeSelect]),
                            ChartColor = Brushes.Salmon
                        });
                        ditems.Add(Convert.ToDouble(item[GraphtypeSelect]));
                        iRow += 1;
                    }
                }

                if ((DroptableList[10].Rows.Count > 0) && (bDropOn[10]))
                {
                    iRow = 1;
                    foreach (DataRow item in DroptableList[10].Rows)
                    {
                        Pos11.Add(new ChartData()
                        {
                            Index = iRow,
                            Value = Convert.ToDouble(item[GraphtypeSelect]),
                            ChartColor = Brushes.SeaGreen
                        });
                        ditems.Add(Convert.ToDouble(item[GraphtypeSelect]));
                        iRow += 1;
                    }
                }

                if ((DroptableList[11].Rows.Count > 0) && (bDropOn[11]))
                {
                    iRow = 1;
                    foreach (DataRow item in DroptableList[11].Rows)
                    {
                        Pos12.Add(new ChartData()
                        {
                            Index = iRow,
                            Value = Convert.ToDouble(item[GraphtypeSelect]),
                            ChartColor = Brushes.Coral
                        });
                        ditems.Add(Convert.ToDouble(item[GraphtypeSelect]));
                        iRow += 1;
                    }
                }

                MinimumHeight = 0;
                MaximumHeight = 0;


                ///set graph high and low range
                ///
                if (ditems.Count > 0)
                {
                    GrpHigh = ditems.Max().ToString("#0.00");
                    GrpLow = ditems.Min().ToString("#0.00");
                    GrpAvg = ditems.Average().ToString("#0.00");

                    if ((ditems.Max() > -1) && (ditems.Max() < 35)) // %MC
                    {
                        MaximumHeight = ditems.Max() + (ditems.Max() * .09);
                        MinimumHeight = ditems.Min() - (ditems.Min() * .09);
                    }
                    else if ((ditems.Max() > 36) && (ditems.Max() < 120))
                    {
                        MaximumHeight = ditems.Max() + (ditems.Max() * .005);
                        MinimumHeight = ditems.Min() - (ditems.Min() * .005);
                    }
                    else if (ditems.Max() > 160)
                    {
                        MaximumHeight = ditems.Max() + (ditems.Max() * .001);
                        MinimumHeight = ditems.Min() - (ditems.Min() * .001);
                    }
                    else if (ditems.Max() > 80)
                    {
                        MaximumHeight = ditems.Max() + (ditems.Max() * .02);
                        MinimumHeight = ditems.Min() - (ditems.Min() * .02);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in ProcessDatTable " + ex.Message);
            }
        }


        #region Graph Components

        private string _graphTitle;
        public string GraphTitle
        {
            get { return _graphTitle; }
            set { SetProperty(ref _graphTitle, value); }
        }
        private string _XAxisTitle;
        public string XAxisTitle
        {
            get { return _XAxisTitle; }
            set { SetProperty(ref _XAxisTitle, value); }
        }
        private string _YAxixTitle;
        public string YAxixTitle
        {
            get { return _YAxixTitle; }
            set { SetProperty(ref _YAxixTitle, value); }
        }
        private string _GrpHigh;
        public string GrpHigh
        {
            get { return _GrpHigh; }
            set { SetProperty(ref _GrpHigh, value); }
        }
        private string _GrpLow;
        public string GrpLow
        {
            get { return _GrpLow; }
            set { SetProperty(ref _GrpLow, value); }
        }
        private string _GrpAvg;
        public string GrpAvg
        {
            get { return _GrpAvg; }
            set { SetProperty(ref _GrpAvg, value); }
        }
        private string _GrpStd;
        public string GrpStd
        {
            get { return _GrpStd; }
            set { SetProperty(ref _GrpStd, value); }
        }

        private ObservableCollection<ChartData> ChartdataOne;
        public ObservableCollection<ChartData> Pos1
        {
            get { return ChartdataOne; }
            set { SetProperty(ref ChartdataOne, value); }
        }

        private ObservableCollection<ChartData> ChartdataTwo;
        public ObservableCollection<ChartData> Pos2
        {
            get { return ChartdataTwo; }
            set { SetProperty(ref ChartdataTwo, value); }
        }

        private ObservableCollection<ChartData> ChartdataThree;
        public ObservableCollection<ChartData> Pos3
        {
            get { return ChartdataThree; }
            set { SetProperty(ref ChartdataThree, value); }
        }

        private ObservableCollection<ChartData> ChartdataFour;
        public ObservableCollection<ChartData> Pos4
        {
            get { return ChartdataFour; }
            set { SetProperty(ref ChartdataFour, value); }
        }

        private ObservableCollection<ChartData> ChartdataFive;
        public ObservableCollection<ChartData> Pos5
        {
            get { return ChartdataFive; }
            set { SetProperty(ref ChartdataFive, value); }
        }

        private ObservableCollection<ChartData> ChartdataSix;
        public ObservableCollection<ChartData> Pos6
        {
            get { return ChartdataSix; }
            set { SetProperty(ref ChartdataSix, value); }
        }


        private ObservableCollection<ChartData> ChartdataSeven;
        public ObservableCollection<ChartData> Pos7
        {
            get { return ChartdataSeven; }
            set { SetProperty(ref ChartdataSeven, value); }
        }

        private ObservableCollection<ChartData> ChartdataEight;
        public ObservableCollection<ChartData> Pos8
        {
            get { return ChartdataEight; }
            set { SetProperty(ref ChartdataEight, value); }
        }

        private ObservableCollection<ChartData> ChartdataNine;
        public ObservableCollection<ChartData> Pos9
        {
            get { return ChartdataNine; }
            set { SetProperty(ref ChartdataNine, value); }
        }

        private ObservableCollection<ChartData> ChartdataTen;
        public ObservableCollection<ChartData> Pos10
        {
            get { return ChartdataTen; }
            set { SetProperty(ref ChartdataTen, value); }
        }

        private ObservableCollection<ChartData> ChartdataEleven;
        public ObservableCollection<ChartData> Pos11
        {
            get { return ChartdataEleven; }
            set { SetProperty(ref ChartdataEleven, value); }
        }

        private ObservableCollection<ChartData> ChartdataTwelve;
        public ObservableCollection<ChartData> Pos12
        {
            get { return ChartdataTwelve; }
            set { SetProperty(ref ChartdataTwelve, value); }
        }

        private int _DropSamples;
        public int DropSamples
        {
            get { return _DropSamples; }
            set
            {
                if ((value > 0) & (value < 5001))
                    SetProperty(ref _DropSamples, value);
                else
                    SetProperty(ref _DropSamples, 100);

                Settings.Default.iDropGraphSamples = _DropSamples;
                Settings.Default.Save();
                XAxisTitle = "Most Recent - " + value + " - Bales (from Loacation 1 on the Left)";
            }
        }


        #endregion Graph

        public string CurrentBaleTable { get; set; }
        public string PreviousTable { get; set; }

        private readonly List<bool> bDropOn;
        private int m_BalesinDrop;
        private readonly List<DataTable> DroptableList;
        
        private double _showme = .1;
        public double ShowMe
        {
            get { return _showme; }
            set { SetProperty(ref _showme, value); }
        }

        //Graph height lower
        private double _minimumheight;
        public double MinimumHeight
        {
            get { return _minimumheight; }
            set { SetProperty(ref _minimumheight, value); }
        }
        //Graph height Upper
        private double _maximumheight;
        public double MaximumHeight
        {
            get { return _maximumheight; }
            set { SetProperty(ref _maximumheight, value); }
        }

        private bool _allLineschecked;
        public bool AllLinesChecked
        {
            get { return _allLineschecked; }
            set { SetProperty(ref _allLineschecked, value); }
        }

        private bool _IsL1Active;
        public bool IsL1Active
        {
            get { return _IsL1Active; }
            set { SetProperty(ref _IsL1Active, value); }
        }

        private bool _IsL2Active;
        public bool IsL2Active
        {
            get { return _IsL2Active; }
            set { SetProperty(ref _IsL2Active, value); }
        }

        private bool _IsL3Active;
        public bool IsL3Active
        {
            get { return _IsL3Active; }
            set { SetProperty(ref _IsL3Active, value); }
        }

        private bool _IsL4Active;
        public bool IsL4Active
        {
            get { return _IsL4Active; }
            set { SetProperty(ref _IsL4Active, value); }
        }

        private bool _IsL5Active;
        public bool IsL5Active
        {
            get { return _IsL5Active; }
            set { SetProperty(ref _IsL5Active, value); }
        }

        private bool _IsL6Active;
        public bool IsL6Active
        {
            get { return _IsL6Active; }
            set { SetProperty(ref _IsL6Active, value); }
        }

        private string _MoistureType;
        public string MoistureType
        {
            get { return _MoistureType; }
            set { SetProperty(ref _MoistureType, value); }
        }

      

        private List<string> _LineList;
        public List<string> LineList
        {
            get { return _LineList; }
            set { SetProperty(ref _LineList, value); }
        }
        private int _SelectLineIndex = 0;
        public int SelectLineIndex
        {
            get { return _SelectLineIndex; }
            set
            {
                SetProperty(ref _SelectLineIndex, value);
                if (value > -1)
                {
                    _dropModel.m_Line = LineList[SelectLineIndex];
                    Settings.Default.iDropGraphLineIndex = value;
                    Settings.Default.Save();

                }
            }
        }

        private List<string> _SourceList;
        public List<string> SourceList
        {
            get { return _SourceList; }
            set { SetProperty(ref _SourceList, value); }
        }
        private int _SelectSourceIndex = 0;
        public int SelectSourceIndex
        {
            get { return _SelectSourceIndex; }
            set
            {
                SetProperty(ref _SelectSourceIndex, value);
                if (value > -1)
                {
                    _dropModel.m_Source = SourceList[SelectSourceIndex];
                    Settings.Default.iDropGraphSourceIndex = value;
                    Settings.Default.Save();
                }
            }
        }


        public DropGraphViewModel(IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;

            _sqlhandler = Sqlhandler.Instance;

            _dropModel = new DropModel();

            DroptableList = new List<DataTable>();
            bDropOn = new List<bool>();



            LoadedPageICommand = new DelegateCommand(LoadedPageExecute, LoadedPageCanExecute);
            ClosedPageICommand = new DelegateCommand(ClosedPageExecute, ClosedPageCanExecute);

            StartCommand = new DelegateCommand(StartExecute).ObservesCanExecute(() => RTIdle);
            StopCommand = new DelegateCommand(StopExecute).ObservesCanExecute(() => RTRunning);

            ReDrawCommand = new DelegateCommand(ReDrawExecute).ObservesCanExecute(() => RTIdle);

            _eventAggregator.GetEvent<UpdatedEventShutdown>().Subscribe(ProgramShutdown);
        }

        private void ProgramShutdown(bool obj)
        {
            
            if (obj)
                if (dispatcherTimer != null)
                {
                    dispatcherTimer.Stop();
                    dispatcherTimer = null;
                }
            
        }

        private bool _menuOneChecked;
        public bool MenuOneChecked
        {
            get { return _menuOneChecked; }
            set
            {
                SetProperty(ref _menuOneChecked, value);
                if (value)
                {
                    GraphtypeSelect = "Moisture";

                    switch (Settings.Default.iLanguageIdx)
                    {
                        case 0: // "en-US":
                            GraphTitle =  $"Trend Graph of Each Bale Position - {ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Name}";
                            break;
                        case 1: //"Sp-SP":
                            GraphTitle = "Perfil de descenso en seco de -  " + ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Name + " - desde " + _dropModel.CurrentBaleTable;
                            break;
                        default:
                            GraphTitle = "Drop Profile of -  " + ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Name + " - from " + _dropModel.CurrentBaleTable;
                            break;
                    }
                    YAxixTitle = ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Name;
                    ItemUnit = ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Unit;
                    iMenuSelect = 0;
                    Settings.Default.iDropGraphMenuSel = iMenuSelect;
                    Settings.Default.Save();
                    if(RTRunning) GetRealtimeData();
                }
            }
        }

        private bool _menuTwoChecked;
        public bool MenuTwoChecked
        {
            get { return _menuTwoChecked; }
            set
            {
                SetProperty(ref _menuTwoChecked, value);
                if (value)
                {
                    GraphtypeSelect = "Weight";
                    switch (Settings.Default.iLanguageIdx)
                    {
                        case 0: // "en-US":
                            GraphTitle = "Trend Graph of Each Bale Position - " + ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name;
                            break;
                        case 1: //"Sp-SP":
                            GraphTitle = "Perfil de descenso en seco de en peso " + ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name + " - desde " + _dropModel.CurrentBaleTable;
                            break;
                        default:
                            GraphTitle = "Drop Profile of Weight in " + ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name + " - from " + _dropModel.CurrentBaleTable;
                            break;
                    }

                    YAxixTitle = ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name;
                    ItemUnit = ClsParams.WeightUnitList[Settings.Default.WeightUnit].Unit;
                    iMenuSelect = 1;
                    Settings.Default.iDropGraphMenuSel = iMenuSelect;
                    Settings.Default.Save();
                    if (RTRunning) GetRealtimeData();
                }
            }
        }

        private bool _menuThreeChecked;
        public bool MenuThreeChecked
        {
            get { return _menuThreeChecked; }
            set
            {
                SetProperty(ref _menuThreeChecked, value);
                if (value)
                {
                    GraphtypeSelect = "BDWeight";
                    switch (Settings.Default.iLanguageIdx)
                    {
                        case 0: // "en-US":
                            GraphTitle = "Trend Graph of Each Bale Position - BoneDryWeight";
                            break;
                        case 1: //"Sp-SP":
                            GraphTitle = "Perfil de descenso en seco de Peso BD - desde " + _dropModel.CurrentBaleTable;
                            break;
                        default:
                            GraphTitle = "Drop Profile of Forte Number - from " + _dropModel.CurrentBaleTable;
                            break;
                    }

                    YAxixTitle = String.Empty;
                    ItemUnit = string.Empty;
                    iMenuSelect = 2;
                    Settings.Default.iDropGraphMenuSel = iMenuSelect;
                    Settings.Default.Save();
                    if (RTRunning) GetRealtimeData();
                }
            }
        }


        private bool _menu4Checked;
        public bool Menu4Checked
        {
            get { return _menu4Checked; }
            set
            {
                SetProperty(ref _menu4Checked, value);
                if (value)
                {
                    GraphtypeSelect = "ADWeight";
                    switch (Settings.Default.iLanguageIdx)
                    {
                        case 0: // "en-US":
                            GraphTitle = "Trend Graph of Each Bale Position - AirDryWeight";
                            break;
                        case 1: //"Sp-SP":
                            GraphTitle = "Perfil de descenso en seco de Peso AD - desde " + _dropModel.CurrentBaleTable;
                            break;
                        default:
                            GraphTitle = "Drop Profile of AirDryWeight - from " + _dropModel.CurrentBaleTable;
                            break;
                    }

                    YAxixTitle = String.Empty;
                    ItemUnit = string.Empty;
                    iMenuSelect = 3;
                    Settings.Default.iDropGraphMenuSel = iMenuSelect;
                    Settings.Default.Save();
                    if (RTRunning) GetRealtimeData();
                }
            }
        }

        private bool _menu5Checked;
        public bool Menu5Checked
        {
            get { return _menu5Checked; }
            set
            {
                SetProperty(ref _menu5Checked, value);
                if (value)
                {
                    GraphtypeSelect = "Dirt";
                    switch (Settings.Default.iLanguageIdx)
                    {
                        case 0: // "en-US":
                            GraphTitle = "Trend Graph of Each Bale - Dirt";
                            break;
                        case 1: //"Sp-SP":
                            GraphTitle = "Perfil de descenso en seco de Conteo de pintas - desde " + _dropModel.CurrentBaleTable;
                            break;
                        default:
                            GraphTitle = "Drop Profile of Dirt - from " + _dropModel.CurrentBaleTable;
                            break;
                    }

                    YAxixTitle = String.Empty;
                    ItemUnit = string.Empty;
                    iMenuSelect = 4;
                    Settings.Default.iDropGraphMenuSel = iMenuSelect;
                    Settings.Default.Save();
                    if (RTRunning) GetRealtimeData();
                }
            }
        }

        private bool _menu6Checked;
        public bool Menu6Checked
        {
            get { return _menu6Checked; }
            set
            {
                SetProperty(ref _menu6Checked, value);
                if (value)
                {
                    GraphtypeSelect = "Brightness";
                    switch (Settings.Default.iLanguageIdx)
                    {
                        case 0: // "en-US":
                            GraphTitle = "Trend Graph of Each Bale - Brightness";
                            break;
                        case 1: //"Sp-SP":
                            GraphTitle = "Perfil de descenso en seco de Blancura - desde " + _dropModel.CurrentBaleTable;
                            break;
                        default:
                            GraphTitle = "Drop Profile of Brightness - from " + _dropModel.CurrentBaleTable;
                            break;
                    }

                    YAxixTitle = String.Empty;
                    ItemUnit = string.Empty;
                    iMenuSelect = 5;
                    Settings.Default.iDropGraphMenuSel = iMenuSelect;
                    Settings.Default.Save();
                    if (RTRunning) GetRealtimeData();
                }
            }
        }


        private bool _menu7Checked;
        public bool Menu7Checked
        {
            get { return _menu7Checked; }
            set
            {
                SetProperty(ref _menu7Checked, value);
                if (value)
                {
                    GraphtypeSelect = "Finish";
                    switch (Settings.Default.iLanguageIdx)
                    {
                        case 0: // "en-US":
                            GraphTitle = "Trend Graph of Each Bale - Viscosity";
                            break;
                        case 1: //"Sp-SP":
                            GraphTitle = "Perfil de descenso en seco de Viscosidad - desde " + _dropModel.CurrentBaleTable;
                            break;
                        default:
                            GraphTitle = "Drop Profile of Viscosity - from " + _dropModel.CurrentBaleTable;
                            break;
                    }

                    YAxixTitle = String.Empty;
                    ItemUnit = string.Empty;
                    iMenuSelect = 6;
                    Settings.Default.iDropGraphMenuSel = iMenuSelect;
                    Settings.Default.Save();
                    if (RTRunning) GetRealtimeData();
                }
            }
        }

        private bool _menu8Checked;
        public bool Menu8Checked
        {
            get { return _menu8Checked; }
            set
            {
                SetProperty(ref _menu8Checked, value);
                if (value)
                {
                    GraphtypeSelect = "Forte";
                    switch (Settings.Default.iLanguageIdx)
                    {
                        case 0: // "en-US":
                            GraphTitle = "Trend Graph of Each Bale - Forte Number";
                            break;
                        case 1: //"Sp-SP":
                            GraphTitle = "Perfil de descenso en seco de Forte Number - desde " + _dropModel.CurrentBaleTable;
                            break;
                        default:
                            GraphTitle = "Drop Profile of Forte Number - from " + _dropModel.CurrentBaleTable;
                            break;
                    }
                  //  GraphTitle = "Graph of Forte Number ";

                    YAxixTitle = String.Empty;
                    ItemUnit = string.Empty;
                    iMenuSelect = 7;
                    Settings.Default.iDropGraphMenuSel = iMenuSelect;
                    Settings.Default.Save();
                    if (RTRunning) GetRealtimeData();
                }
            }
        }
        private void ReDrawExecute()
        {
            LoadedPageExecute();
        }

        private void StopExecute()
        {
            Application.Current.Dispatcher.Invoke(new Action(() => { StopTimer(); }));
            RTRunning = false;
        }

        private void StartExecute()
        {
            InitializeTimer();
            StartTimer();

            RTRunning = true;
            _eventAggregator.GetEvent<UpdatedEvent>().Publish(RTRunning);
            StartTimer();
            ShowMe = 0.1;
        }

        private bool ClosedPageCanExecute()
        {
            return true;
        }

        private void ClosedPageExecute()
        {
            //throw new NotImplementedException();
        }

        private bool LoadedPageCanExecute()
        {
            return true;
        }

        private void LoadedPageExecute()
        {
            _dropModel.SetUpSql();

            MoistureType = ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Name;

            LineList = new List<string>();
            SourceList = new List<string>();

            LineList = _dropModel.GetListof("LineID");
            SourceList = _dropModel.GetListof("SourceID");

            NLine = LineList.Count.ToString();
            NSource = SourceList.Count.ToString();


            ItemUnit = string.Empty;
            DropSamples = Settings.Default.iDropGraphSamples;

            _dropModel.GetLineSource();


            try
            {
              
                if (Settings.Default.iDropGraphLineIndex >= LineList.Count)
                    SelectLineIndex = 0;
                else
                    SelectLineIndex = Settings.Default.iDropGraphLineIndex;

              
                if (Settings.Default.iDropGraphSourceIndex >= SourceList.Count)
                    SelectSourceIndex = 0;
                else
                    SelectSourceIndex = Settings.Default.iDropGraphSourceIndex;

                InitGraph();

                RTRunning = false;
                AllBoxesChecked = true;

                MainWindow.AppWindows.SetupAppTitle("Forté Drop Graph From  -> " + _sqlhandler.Host + @"\" + _sqlhandler.SqlInstance);

                ShowMe = 0.1;

                switch (Settings.Default.iDropGraphMenuSel)
                {
                    case 0:
                        MenuOneChecked = true;
                        break;
                    case 1:
                        MenuTwoChecked = true;
                        break;
                    case 2:
                        MenuThreeChecked = true;
                        break;
                    case 3:
                        Menu4Checked = true;
                        break;
                    case 4:
                        Menu5Checked = true;
                        break;
                    case 5:
                        Menu6Checked = true;
                        break;
                    case 6:
                        Menu7Checked = true;
                        break;
                    case 7:
                        Menu8Checked = true;
                        break;
                    default:
                        MenuOneChecked = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in LoadedPageExecute " + ex.Message);
            }
        }

        private void SetNewCharts()
        {
            if (Pos1 != null) Pos1 = null;
            Pos1 = new ObservableCollection<ChartData>();

            if (Pos2 != null) Pos2 = null;
            Pos2 = new ObservableCollection<ChartData>();

            if (Pos3 != null) Pos3 = null;
            Pos3 = new ObservableCollection<ChartData>();

            if (Pos4 != null) Pos4 = null;
            Pos4 = new ObservableCollection<ChartData>();

            if (Pos5 != null) Pos5 = null;
            Pos5 = new ObservableCollection<ChartData>();

            if (Pos6 != null) Pos6 = null;
            Pos6 = new ObservableCollection<ChartData>();

            if (Pos7 != null) Pos7 = null;
            Pos7 = new ObservableCollection<ChartData>();

            if (Pos8 != null) Pos8 = null;
            Pos8 = new ObservableCollection<ChartData>();

            if (Pos9 != null) Pos9 = null;
            Pos9 = new ObservableCollection<ChartData>();

            if (Pos10 != null) Pos10 = null;
            Pos10 = new ObservableCollection<ChartData>();

            if (Pos11 != null) Pos11 = null;
            Pos11 = new ObservableCollection<ChartData>();

            if (Pos12 != null) Pos12 = null;
            Pos12 = new ObservableCollection<ChartData>();
        }



        private void InitGraph()
        {
            m_BalesinDrop = _dropModel.BalesinDrop();

            SetCheckBoxes(m_BalesinDrop);
            SetNewCharts();

            for (int i = 0; i < 12; i++)
            {
                DroptableList.Add(new DataTable());
                bDropOn.Add(false);
            }
        }


        private void SetCheckBoxes(int iBaleInOneDrop)
        {
            Drop1Active = false;
            Drop2Active = false;
            Drop3Active = false;
            Drop4Active = false;
            Drop5Active = false;
            Drop6Active = false;
            Drop7Active = false;
            Drop8Active = false;
            Drop9Active = false;
            Drop10Active = false;
            Drop11Active = false;
            Drop12Active = false;

            switch (iBaleInOneDrop)
            {
                case 12:
                    Show12 = Visibility.Visible;
                    Drop12Active = true;
                    goto case 11;
                case 11:
                    Show11 = Visibility.Visible;
                    Drop11Active = true;
                    goto case 10;
                case 10:
                    Show10 = Visibility.Visible;
                    Drop10Active = true;
                    goto case 9;
                case 9:
                    Show9 = Visibility.Visible;
                    Drop9Active = true;
                    goto case 8;
                case 8:
                    Show8 = Visibility.Visible;
                    Drop8Active = true;
                    goto case 7;
                case 7:
                    Show7 = Visibility.Visible;
                    Drop7Active = true;
                    goto case 6;
                case 6:
                    Show6 = Visibility.Visible;
                    Drop6Active = true;
                    goto case 5;
                case 5:
                    Show5 = Visibility.Visible;
                    Drop5Active = true;
                    goto case 4;
                case 4:
                    Show4 = Visibility.Visible;
                    Drop4Active = true;
                    goto case 3;
                case 3:
                    Show3 = Visibility.Visible;
                    Drop3Active = true;
                    goto case 2;
                case 2:
                    Show2 = Visibility.Visible;
                    Drop2Active = true;
                    goto case 1;
                case 1:
                    Show1 = Visibility.Visible;
                    Drop1Active = true;
                    break;
            }

        }

        private void Heartbeat()
        {
            if (ShowMe == 0.1) ShowMe = 1;
            else if (ShowMe == 1) ShowMe = 0.1;
        }


        /// <summary>
        /// 
        /// </summary>
        private void GetNewData()
        {
            int iFirstBale;
            CurrentBaleTable = _dropModel.CurrentBaleTable;
            PreviousTable = _dropModel.GetPreviousTable();

            string StrQuery = "SELECT Top 1 Position FROM dbo.[" +
                CurrentBaleTable + "]" + " ORDER BY [TimeStart] DESC; ";

            int iBalePos = _dropModel.GetNewItemData(StrQuery);

            switch (Settings.Default.iLanguageIdx)
            {
                case 0: // "en-US":
                    UpdateInfo = "Bale Position " + iBalePos.ToString() + " of " + m_BalesinDrop.ToString();
                    break;
                case 1: //"Sp-SP":
                    UpdateInfo = "Posicion de Fardo  " + iBalePos.ToString() + " de " + m_BalesinDrop.ToString();
                    break;
                default:
                    UpdateInfo = "Bale Position " + iBalePos.ToString() + " of " + m_BalesinDrop.ToString();
                    break;
            }


            //Get New Data when last bale in drop arrived
            if (Settings.Default.bDropHitoLow)
            {
                iFirstBale = 1;
            }
            else
            {
                iFirstBale = m_BalesinDrop;
            }

            if (iBalePos == iFirstBale)
            {
                //Put them in DroptableList[] 
                GetDropDataArray(bDropOn);
                ProcessDatTable();
            }
        }
        private void UpdateStatus(string strMsg)
        {
            UpdateInfo = strMsg;
        }

        #region Timer ////////////////////////////////////////////////////////////////////////

        private System.Windows.Threading.DispatcherTimer dispatcherTimer;

        private void InitializeTimer()
        {
            if (dispatcherTimer != null) dispatcherTimer = null;
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(Settings.Default.iScanRate)
            };
            dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();

            Application.Current.Dispatcher.Invoke(new Action(() => { GetNewData(); }));
            Application.Current.Dispatcher.Invoke(new Action(() => { Heartbeat(); }));

            Thread.Sleep(1000); //Rest for 1 Sec.
            dispatcherTimer.Start();
        }

        private void StartTimer()
        {
            dispatcherTimer.Start();

            switch (Settings.Default.iLanguageIdx)
            {
                case 0: // "en-US":
                    UpdateInfo = "Status : Scan timer Start";
                    break;
                case 1: //"Sp-SP":
                    UpdateInfo = "Estado :Tiempo de Scan Comienzo";
                    break;
                default:
                    UpdateInfo = "Status : Scan timer Start";
                    break;
            }
        }

        private void StopTimer()
        {
            if(dispatcherTimer != null)
            {
                dispatcherTimer.Stop();
                Worker_Stopped();

                switch (Settings.Default.iLanguageIdx)
                {
                    case 0: // "en-US":
                        UpdateInfo = "Status : Scan timer Stop";
                        break;
                    case 1: //"Sp-SP":
                        UpdateInfo = "Tiempo de Scan Detenido";
                        break;
                    default:
                        UpdateInfo = "Status : Scan timer Stop";
                        break;
                }
            }
        }

        private void Worker_Stopped()
        {
            RTRunning = false;
            _eventAggregator.GetEvent<UpdatedEvent>().Publish(RTRunning);
            ShowMe = 0.1;
            //Opac = 1.0;
            switch (Settings.Default.iLanguageIdx)
            {
                case 0: // "en-US":
                    Application.Current.Dispatcher.Invoke(new Action(() => { UpdateStatus("Status : Scan timer Stopped"); }));
                    break;
                case 1: //"Sp-SP":
                    Application.Current.Dispatcher.Invoke(new Action(() => { UpdateStatus("Estado : Tiempo de Scan Detenido"); }));
                    break;
                default:
                    Application.Current.Dispatcher.Invoke(new Action(() => { UpdateStatus("Status : Scan timer Stopped"); }));
                    break;
            }

        }

        #endregion timer ////////////////////////////////////////////////////////////////////
    }
}
