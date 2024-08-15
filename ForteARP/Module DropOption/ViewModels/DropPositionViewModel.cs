

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

namespace ForteARP.Module_DropOption.ViewModels
{
    public class DropPositionViewModel : BindableBase
    {
        protected readonly Prism.Events.IEventAggregator _eventAggregator;
        private Sqlhandler _sqlhandler;
        private readonly DropModel _dropModel;

        private DataTable Mydatatable;
        private int IntColSamples;
        private int iBaleInDrop;
      //  private List<string> GraphType;

        private int CurIndex = 0;
        private int PreIndex = 0;

        private string _strSelectedItem;
        public string StrSelectedItem
        {
            get { return _strSelectedItem; }
            set { SetProperty(ref _strSelectedItem, value); }
        }

        private string _balePosition;
        public string BalePosition
        {
            get { return _balePosition; }
            set { SetProperty(ref _balePosition, value); }
        }

        private string _AverageHeader;
        public string AverageHeader
        {
            get { return _AverageHeader; }
            set { SetProperty(ref _AverageHeader, value); }
        }


        private string _MoistureType;
        public string MoistureType
        {
            get { return _MoistureType; }
            set { SetProperty(ref _MoistureType, value); }
        }

        private string _bPosition;
        public string BPosition
        {
            get { return _bPosition; }
            set { SetProperty(ref _bPosition, value); }
        }


        private System.Windows.Media.Brush _positionColor;
        public System.Windows.Media.Brush PositionColor
        {
            get => _positionColor;
            set => SetProperty(ref _positionColor, value);
        }

        private int _SampleSize;
        public int SampleSize
        {
            get { return _SampleSize; }
            set
            {
                if ((value > 0) & (value < 11))
                    SetProperty(ref _SampleSize, value);
                else
                    SetProperty(ref _SampleSize, 10);
                Settings.Default.iDropSample = value;
                Settings.Default.Save();
            }
        }

        private int _XMax;
        public int XMax
        {
            get { return _XMax; }
            set { SetProperty(ref _XMax, value); }
        }

        private string _GraphTitle;
        public string GraphTitle
        {
            get { return _GraphTitle; }
            set { SetProperty(ref _GraphTitle, value); }
        }
        private string _GraphTitlebot;
        public string GraphTitlebot
        {
            get { return _GraphTitlebot; }
            set { SetProperty(ref _GraphTitlebot, value); }
        }

        private List<string> _LineList;
        public List<string> LineList
        {
            get { return _LineList; }
            set { SetProperty(ref _LineList, value); }
        }
        private int _SelectLineIndex;
        public int SelectLineIndex
        {
            get { return _SelectLineIndex; }
            set
            {
                SetProperty(ref _SelectLineIndex, value);
                if (value > -1)
                {
                    Settings.Default.iDropProfileLineIndex = value;
                    Settings.Default.Save();
                }
            }
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
                    Settings.Default.iDropProfileSourceIndex = value;
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

        private bool _rtIdle = true;
        public bool RTIdle
        {
            get { return _rtIdle; }
            set { SetProperty(ref _rtIdle, value); }
        }
    
        private string _updateinfo;
        public string UpdateInfo
        {
            get { return _updateinfo; }
            set { SetProperty(ref _updateinfo, value); }
        }

        public ObservableCollection<ClsChartData> _ChartdataOne;
        public ObservableCollection<ClsChartData> ChartdataOne
        {
            get { return _ChartdataOne; }
            set { SetProperty(ref _ChartdataOne, value); }
        }

        public ObservableCollection<ClsChartData> _ChartdataTwo;
        public ObservableCollection<ClsChartData> ChartdataTwo
        {
            get { return _ChartdataTwo; }
            set { SetProperty(ref _ChartdataTwo, value); }
        }

        public ObservableCollection<ClsChartData> _Chartdatathree;
        public ObservableCollection<ClsChartData> ChartdataThree
        {
            get { return _Chartdatathree; }
            set { SetProperty(ref _Chartdatathree, value); }
        }

        public ObservableCollection<ClsChartData> _ChartdataFour;
        public ObservableCollection<ClsChartData> ChartdataFour
        {
            get { return _ChartdataFour; }
            set { SetProperty(ref _ChartdataFour, value); }
        }

        public ObservableCollection<ClsChartData> _ChartdataFive;
        public ObservableCollection<ClsChartData> ChartdataFive
        {
            get { return _ChartdataFive; }
            set { SetProperty(ref _ChartdataFive, value); }
        }

        public ObservableCollection<ClsChartData> _ChartdataSix;
        public ObservableCollection<ClsChartData> ChartdataSix
        {
            get { return _ChartdataSix; }
            set { SetProperty(ref _ChartdataSix, value); }
        }

        public ObservableCollection<ClsChartData> _ChartdataSeven;
        public ObservableCollection<ClsChartData> ChartdataSeven
        {
            get { return _ChartdataSeven; }
            set { SetProperty(ref _ChartdataSeven, value); }
        }

        public ObservableCollection<ClsChartData> _ChartdataEight;
        public ObservableCollection<ClsChartData> ChartdataEight
        {
            get { return _ChartdataEight; }
            set { SetProperty(ref _ChartdataEight, value); }
        }

        public ObservableCollection<ClsChartData> _ChartdataNine;
        public ObservableCollection<ClsChartData> ChartdataNine
        {
            get { return _ChartdataNine; }
            set { SetProperty(ref _ChartdataNine, value); }
        }

        public ObservableCollection<ClsChartData> _ChartdataTen;
        public ObservableCollection<ClsChartData> ChartdataTen
        {
            get { return _ChartdataTen; }
            set { SetProperty(ref _ChartdataTen, value); }
        }

        public ObservableCollection<ClsChartData> _ChartdataEleven;
        public ObservableCollection<ClsChartData> ChartdataEleven
        {
            get { return _ChartdataEleven; }
            set { SetProperty(ref _ChartdataEleven, value); }
        }

        public ObservableCollection<ClsChartData> _ChartdataTwelve;
        public ObservableCollection<ClsChartData> ChartdataTwelve
        {
            get { return _ChartdataTwelve; }
            set { SetProperty(ref _ChartdataTwelve, value); }
        }

        private System.Windows.Media.Brush _BaleColor;
        public System.Windows.Media.Brush BaleColor
        {
            get { return _BaleColor; }
            set { SetProperty(ref _BaleColor, value); }
        }


        private System.Windows.Media.Brush _Color1;
        public System.Windows.Media.Brush Color1
        {
            get { return _Color1; }
            set { SetProperty(ref _Color1, value); }
        }

        private System.Windows.Media.Brush _Color2;
        public System.Windows.Media.Brush Color2
        {
            get { return _Color2; }
            set { SetProperty(ref _Color2, value); }
        }

        private System.Windows.Media.Brush _Color3;
        public System.Windows.Media.Brush Color3
        {
            get { return _Color3; }
            set { SetProperty(ref _Color3, value); }
        }

        private System.Windows.Media.Brush _Color4;
        public System.Windows.Media.Brush Color4
        {
            get { return _Color4; }
            set { SetProperty(ref _Color4, value); }
        }
        private System.Windows.Media.Brush _Color5;
        public System.Windows.Media.Brush Color5
        {
            get { return _Color5; }
            set { SetProperty(ref _Color5, value); }
        }
        private System.Windows.Media.Brush _Color6;
        public System.Windows.Media.Brush Color6
        {
            get { return _Color6; }
            set { SetProperty(ref _Color6, value); }
        }
        private System.Windows.Media.Brush _Color7;
        public System.Windows.Media.Brush Color7
        {
            get { return _Color7; }
            set { SetProperty(ref _Color7, value); }
        }
        private System.Windows.Media.Brush _Color8;
        public System.Windows.Media.Brush Color8
        {
            get { return _Color8; }
            set { SetProperty(ref _Color8, value); }
        }
        private System.Windows.Media.Brush _Color9;
        public System.Windows.Media.Brush Color9
        {
            get { return _Color9; }
            set { SetProperty(ref _Color9, value); }
        }
        private System.Windows.Media.Brush _Color10;
        public System.Windows.Media.Brush Color10
        {
            get { return _Color10; }
            set { SetProperty(ref _Color10, value); }
        }

        /// <summary>
        /// Display Sample Box Colors Enable
        /// </summary>
        private Visibility _VisCrtOne;
        public Visibility VisCrtOne
        {
            get { return _VisCrtOne; }
            set { SetProperty(ref _VisCrtOne, value); }
        }
        private Visibility _VisCrtTwo;
        public Visibility VisCrtTwo
        {
            get { return _VisCrtTwo; }
            set { SetProperty(ref _VisCrtTwo, value); }
        }
        private Visibility _VisCrtThree;
        public Visibility VisCrtThree
        {
            get { return _VisCrtThree; }
            set { SetProperty(ref _VisCrtThree, value); }
        }
        private Visibility _VisCrtFour;
        public Visibility VisCrtFour
        {
            get { return _VisCrtFour; }
            set { SetProperty(ref _VisCrtFour, value); }
        }
        private Visibility _VisCrtFive;
        public Visibility VisCrtFive
        {
            get { return _VisCrtFive; }
            set { SetProperty(ref _VisCrtFive, value); }
        }
        private Visibility _VisCrtSix;
        public Visibility VisCrtSix
        {
            get { return _VisCrtSix; }
            set { SetProperty(ref _VisCrtSix, value); }
        }
        private Visibility _VisCrtSeven;
        public Visibility VisCrtSeven
        {
            get { return _VisCrtSeven; }
            set { SetProperty(ref _VisCrtSeven, value); }
        }
        private Visibility _VisCrtEight;
        public Visibility VisCrtEight
        {
            get { return _VisCrtEight; }
            set { SetProperty(ref _VisCrtEight, value); }
        }
        private Visibility _VisCrtNine;
        public Visibility VisCrtNine
        {
            get { return _VisCrtNine; }
            set { SetProperty(ref _VisCrtNine, value); }
        }
        private Visibility _VisCrtTen;
        public Visibility VisCrtTen
        {
            get { return _VisCrtTen; }
            set { SetProperty(ref _VisCrtTen, value); }
        }

        private List<Double> BalesLis1;
        private List<Double> BalesLis2;
        private List<Double> BalesLis3;
        private List<Double> BalesLis4;
        private List<Double> BalesLis5;
        private List<Double> BalesLis6;
        private List<Double> BalesLis7;
        private List<Double> BalesLis8;
        private List<Double> BalesLis9;
        private List<Double> BalesLis10;


        public List<double> BalePos1Lst { get; set; }
        private string _BalePos1Avg;
        public string BalePos1Avg
        {
            get { return _BalePos1Avg; }
            set { SetProperty(ref _BalePos1Avg, value); }
        }

        private string _BalePos2Avg;
        public string BalePos2Avg
        {
            get { return _BalePos2Avg; }
            set { SetProperty(ref _BalePos2Avg, value); }
        }

        private string _BalePos3Avg;
        public string BalePos3Avg
        {
            get { return _BalePos3Avg; }
            set { SetProperty(ref _BalePos3Avg, value); }
        }

        private string _BalePos4Avg;
        public string BalePos4Avg
        {
            get { return _BalePos4Avg; }
            set { SetProperty(ref _BalePos4Avg, value); }
        }

        private string _BalePos5Avg;
        public string BalePos5Avg
        {
            get { return _BalePos5Avg; }
            set { SetProperty(ref _BalePos5Avg, value); }
        }

        private string _BalePos6Avg;
        public string BalePos6Avg
        {
            get { return _BalePos6Avg; }
            set { SetProperty(ref _BalePos6Avg, value); }
        }

        private string _BalePos7Avg;
        public string BalePos7Avg
        {
            get { return _BalePos7Avg; }
            set { SetProperty(ref _BalePos7Avg, value); }
        }

        private string _BalePos8Avg;
        public string BalePos8Avg
        {
            get { return _BalePos8Avg; }
            set { SetProperty(ref _BalePos8Avg, value); }
        }

        private string _BalePos9Avg;
        public string BalePos9Avg
        {
            get { return _BalePos9Avg; }
            set { SetProperty(ref _BalePos9Avg, value); }
        }

        private string _BalePos10Avg;
        public string BalePos10Avg
        {
            get { return _BalePos10Avg; }
            set { SetProperty(ref _BalePos10Avg, value); }
        }

        public DelegateCommand LoadedPageICommand { get; set; }
        public DelegateCommand ClosedPageICommand { get; set; }
        public DelegateCommand StartCommand { get; set; }  //Start Button
        public DelegateCommand StopCommand { get; set; }   //Stop Button


        private bool _menuOneChecked;
        public bool MenuOneChecked
        {
            get { return _menuOneChecked; }
            set
            {
                if (value)
                {
                    StrSelectedItem = "Moisture";
                    Settings.Default.iMenuSelected = 0;
                    Settings.Default.Save();

                    switch (Settings.Default.iLanguageIdx)
                    {
                        case 0: // "en-US":
                            GraphTitle = "Profile of -  " + ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Name + " -By Bale Position";
                            GraphTitlebot = "Most Recent - " + SampleSize + " - consecutive drops, group by Bale Position 1 to " + _sqlhandler.m_BaleInDrop;
                            AverageHeader = "Average of " + ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Name +
                                            " of " + Settings.Default.iDropSample + " drops by bale position";
                            break;
                        case 1: //"Sp-SP":
                            GraphTitle = "Perfil de -  " + ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Name + " - Por posición de Fardo";
                            GraphTitlebot = SampleSize.ToString() + " - Gotas más recientes ordenadas por Posición de paca 1 a " + _sqlhandler.m_BaleInDrop;
                            AverageHeader = "Promedio de " + ClsParams.MoistureTypeList[Settings.Default.WeightUnit].Name +
                                            " de " + Settings.Default.iDropSample + " Bajada,  por posición de Fardo";
                            break;
                        default:
                            GraphTitle = "Profile of -  " + ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Name + " -By Bale Position";
                            GraphTitlebot = "Most Recent - " + SampleSize + " - consecutive drops, group by Bale Position 1 to " + _sqlhandler.m_BaleInDrop;
                            AverageHeader = "Average of " + ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Name +
                                            " of " + Settings.Default.iDropSample + " drops by bale position";
                            break;
                    }


                }
                SetProperty(ref _menuOneChecked, value);
            }
        }

        //Weight
        private bool _menuTwoChecked;
        public bool MenuTwoChecked
        {
            get { return _menuTwoChecked; }
            set
            {
                if (value)
                {
                    StrSelectedItem = "Weight";
                    Settings.Default.iMenuSelected = 1;
                    Settings.Default.Save();


                    switch (Settings.Default.iLanguageIdx)
                    {
                        case 0: // "en-US":
                            GraphTitle = "Profile of Weight in " + ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name + " - By Bale Position";
                            GraphTitlebot = "Most Recent - " + SampleSize + " - consecutive drops, group by Bale Position 1 to " + _sqlhandler.m_BaleInDrop;
                            AverageHeader = "Average of Weight in " + ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name +
                                            " of " + Settings.Default.iDropSample + " drops by bale position";
                            break;
                        case 1: //"Sp-SP":
                            GraphTitle = "Perfil de en peso " + ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name + " - Por posición de Fardo";
                            GraphTitlebot = SampleSize.ToString() + " - Gotas más recientes ordenadas por Posición de paca 1 a " + _sqlhandler.m_BaleInDrop;
                            AverageHeader = "Promedio de en peso " + ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name +
                                                " de " + Settings.Default.iDropSample + " Bajada,  por posición de Fardo";
                            break;
                        default:
                            GraphTitle = "Profile of Weight in " + ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name + " - By Bale Position";
                            GraphTitlebot = "Most Recent - " + SampleSize + " - consecutive drops, group by Bale Position 1 to " + _sqlhandler.m_BaleInDrop;
                            AverageHeader = "Average of Weight in " + ClsParams.WeightUnitList[Settings.Default.WeightUnit].Name +
                                            " of " + Settings.Default.iDropSample + " drops by bale position";
                            break;
                    }

                }
                SetProperty(ref _menuTwoChecked, value);
            }
        }

        private bool _menuThreeChecked;
        public bool MenuThreeChecked
        {
            get { return _menuThreeChecked; }
            set
            {
                if (value)
                {
                    StrSelectedItem = "BDWeight";
                    Settings.Default.iMenuSelected = 2;
                    Settings.Default.Save();

                    switch (Settings.Default.iLanguageIdx)
                    {
                        case 0: // "en-US":
                            GraphTitle = "Profile of BoneDry Weight by bale position";
                            GraphTitlebot = "Most Recent - " + SampleSize + " - consecutive drops, group by Bale Position 1 to " + _sqlhandler.m_BaleInDrop;
                            AverageHeader = "Average of BDWeight of " + Settings.Default.iDropSample + " drops by bale position";
                            break;
                        case 1: //"Sp-SP":
                            GraphTitle = "Perfil de Peso BD by bale position";
                            GraphTitlebot = SampleSize.ToString() + " - Gotas más recientes ordenadas por Posición de paca 1 a " + _sqlhandler.m_BaleInDrop;
                            AverageHeader = "Promedio de Peso BD de " + Settings.Default.iDropSample + " Bajada,  por posición de Fardo";
                            break;
                        default:
                            GraphTitle = "Profile of BoneDry Weight by bale position";
                            GraphTitlebot = "Most Recent - " + SampleSize + " - consecutive drops, group by Bale Position 1 to " + _sqlhandler.m_BaleInDrop;
                            AverageHeader = "Average of BDWeight of " + Settings.Default.iDropSample + " drops by bale position";
                            break;
                    }

                }
                SetProperty(ref _menuThreeChecked, value);
            }
        }

        private bool _menuFourChecked;
        public bool MenuFourChecked
        {
            get { return _menuFourChecked; }
            set
            {
                if (value)
                {
                    StrSelectedItem = "ADWeight";
                    Settings.Default.iMenuSelected = 3;
                    Settings.Default.Save();


                    switch (Settings.Default.iLanguageIdx)
                    {
                        case 0: // "en-US":
                            GraphTitle = "Profile of AirDry Weight by bale position";
                            GraphTitlebot = "Most Recent - " + SampleSize + " - consecutive drops, group by Bale Position 1 to " + _sqlhandler.m_BaleInDrop;
                            AverageHeader = "Average of ADWeight of " + Settings.Default.iDropSample + " drops by bale position";
                            break;
                        case 1: //"Sp-SP":
                            GraphTitle = "Perfil de Peso AD by bale position";
                            GraphTitlebot = SampleSize.ToString() + " - Gotas más recientes ordenadas por Posición de paca 1 a " + _sqlhandler.m_BaleInDrop;
                            AverageHeader = "Promedio de Peso AD de " + Settings.Default.iDropSample + " Bajada,  por posición de Fardo";
                            break;
                        default:
                            GraphTitle = "Profile of AirDry Weight by bale position";
                            GraphTitlebot = "Most Recent - " + SampleSize + " - consecutive drops, group by Bale Position 1 to " + _sqlhandler.m_BaleInDrop;
                            AverageHeader = "Average of ADWeight of " + Settings.Default.iDropSample + " drops by bale position";
                            break;
                    }

                }
                SetProperty(ref _menuFourChecked, value);
            }
        }


        private bool _menuFiveChecked;
        public bool MenuFiveChecked
        {
            get { return _menuFiveChecked; }
            set
            {
                if (value)
                {
                    StrSelectedItem = "Forte";
                    Settings.Default.iMenuSelected = 4;
                    Settings.Default.Save();


                    switch (Settings.Default.iLanguageIdx)
                    {
                        case 0: // "en-US":
                            GraphTitle = "Profile of Forte Number by bale position";
                            GraphTitlebot = "Most Recent - " + SampleSize + " - consecutive drops, group by Bale Position 1 to " + _sqlhandler.m_BaleInDrop;
                            AverageHeader = "Average of Forte Numbers of " + Settings.Default.iDropSample + " drops by bale position";
                            break;
                        case 1: //"Sp-SP":
                            GraphTitle = "Perfil de Forte Number by bale position";
                            GraphTitlebot = SampleSize.ToString() + " - Gotas más recientes ordenadas por Posición de paca 1 a " + _sqlhandler.m_BaleInDrop;
                            AverageHeader = "Promedio de Forte Numbers de " + Settings.Default.iDropSample + " Bajada,  por posición de Fardo";
                            break;
                        default:
                            GraphTitle = "Profile of Forte Number by bale position";
                            GraphTitlebot = "Most Recent - " + SampleSize + " - consecutive drops, group by Bale Position 1 to " + _sqlhandler.m_BaleInDrop;
                            AverageHeader = "Average of Forte Numbers of " + Settings.Default.iDropSample + " drops by bale position";
                            break;
                    }

                }
                SetProperty(ref _menuFiveChecked, value);
            }
        }

        public DropPositionViewModel(IEventAggregator eventAggregator)
        {
            this._eventAggregator = eventAggregator;

            _sqlhandler = Sqlhandler.Instance;

            _dropModel = new DropModel();

            // SqlModel = MainWindow.AppWindows.MainSqlCfg;
            LineList = new List<string>();

            LoadedPageICommand = new DelegateCommand(LoadedPageExecute);
            ClosedPageICommand = new DelegateCommand(ClosedPageExecute);

            StartCommand = new DelegateCommand(StartExecute).ObservesCanExecute(() => RTIdle);
            StopCommand = new DelegateCommand(StopExecute).ObservesCanExecute(() => RTRunning);

            PositionColor = System.Windows.Media.Brushes.White;

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

        private void StopExecute()
        {
            BalePosition = string.Empty;
            Application.Current.Dispatcher.Invoke(new Action(() => { StopTimer(); }));
        }


        private void StartExecute()
        {
            RTRunning = true;
            _eventAggregator.GetEvent<UpdatedEvent>().Publish(RTRunning);

            InitializeTimer();
            StartTimer();
        }

        private void InitGraph()
        {
            try
            {
                _sqlhandler.SetupWorkStation();

                LineList = _dropModel.GetListof("LineID");
                SourceList = _dropModel.GetListof("SourceID");

                SelectLineIndex = Settings.Default.iDropProfileLineIndex;
                SelectSourceIndex = Settings.Default.iDropProfileSourceIndex;
             
                SetUpGraphsColors();

                iBaleInDrop = ClassCommon.BaleInADrop; // MainWindow.AppWindows.iBalesinDrop;
                SampleSize = Settings.Default.iDropSample;
                IntColSamples = (iBaleInDrop * SampleSize) + (iBaleInDrop * 3);

                GraphTitle = "Most Recent - " + SampleSize.ToString() + " - consecutive drops by Drop Number  [ " + iBaleInDrop.ToString() + " Bales in each drop ]";
                XMax = iBaleInDrop + 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in InitGraph " + ex.Message);
            }
        }

        private void SetUpGraphsColors()
        {
            BaleColor = System.Windows.Media.Brushes.LightSteelBlue;

            Color1 = System.Windows.Media.Brushes.ForestGreen;
            Color2 = System.Windows.Media.Brushes.LightSalmon;
            Color3 = System.Windows.Media.Brushes.CadetBlue;

            Color4 = System.Windows.Media.Brushes.Gold;
            Color5 = System.Windows.Media.Brushes.White;
            Color6 = System.Windows.Media.Brushes.IndianRed;

            Color7 = System.Windows.Media.Brushes.DarkBlue;
            Color8 = System.Windows.Media.Brushes.Gray;
            Color9 = System.Windows.Media.Brushes.Aquamarine;
            Color10 = System.Windows.Media.Brushes.Salmon;

            VisCrtOne = System.Windows.Visibility.Hidden;
            VisCrtTwo = System.Windows.Visibility.Hidden;
            VisCrtThree = System.Windows.Visibility.Hidden;
            VisCrtFour = System.Windows.Visibility.Hidden;
            VisCrtFive = System.Windows.Visibility.Hidden;
            VisCrtSix = System.Windows.Visibility.Hidden;
            VisCrtSeven = System.Windows.Visibility.Hidden;
            VisCrtEight = System.Windows.Visibility.Hidden;
            VisCrtNine = System.Windows.Visibility.Hidden;
            VisCrtTen = System.Windows.Visibility.Hidden;

        }

        private void GetDataOffLine()
        {
          //  SetChartData();
            InitGraph();

            switch (Settings.Default.iMenuSelected)
            {
                case 0:
                    StrSelectedItem = "Moisture";
                    MenuOneChecked = true;
                    break;
                case 1:
                    StrSelectedItem = "Weight";
                    MenuTwoChecked = true;
                    break;
                case 2:
                    StrSelectedItem = "BdWeight";
                    MenuThreeChecked = true;
                    break;
                case 3:
                    StrSelectedItem = "ADWeight";
                    MenuFourChecked = true;
                    break;
                case 4:
                    StrSelectedItem = "Forte";
                    MenuFiveChecked = true;
                    break;
                default:
                    StrSelectedItem = "Moisture";
                    MenuOneChecked = true;
                    break;
            }

            if (Mydatatable != null)
                ProcessData(Mydatatable, StrSelectedItem);
        }

        public string BuildQueryString(int iSample)
        {
            string m_Line = LineList[SelectLineIndex];
            string m_Source = SourceList[SelectSourceIndex];
            string strSourceLine = string.Empty;

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

            return "SELECT TOP " + iSample.ToString()
                + " Moisture,Weight,BDWeight,NetWeight,Forte,Dirt,Brightness,Finish,UpCount,DownCount,SourceName,LineName,SourceID,LineID,Position,[index] FROM dbo.["
                + _sqlhandler.GetCurrentBaleTable()
                + "] "
                + strSourceLine
                + " ORDER BY [TimeStart] DESC";
        }

        private void LoadedPageExecute()
        {
            MoistureType = ClsParams.MoistureTypeList[Settings.Default.MoistureUnit].Name;


            UpdateInfo = string.Empty;
            BalePosition = string.Empty;
            GetDataOffLine();
        }

        private void ProcessData(DataTable mydatatable, string GraphType)
        {
            List<double> AveragePosition = new List<double>();

            SetChartNew();

            int iBaleinDrop = ClassCommon.BaleInADrop; // MainWindow.AppWindows.iBalesinDrop;
            string BaleInDrop = iBaleinDrop.ToString();

            Double iCoef = 1.0;
            int iSample = SampleSize;
          
            if (GraphType == "Weight")
            {
                if (ClsParams.WeightUnitList[Settings.Default.WeightUnit].Unit == "Lb")
                    iCoef = 2.20462;
            }

            if (!Mydatatable.Columns.Contains("ADWeight"))
            {
                Mydatatable.Columns.Add("ADWeight", typeof(double));
                Mydatatable.AcceptChanges();
                for (int i = 0; i < Mydatatable.Rows.Count; i++)
                {
                    if (Mydatatable.Rows[i]["BDWeight"] != null)
                        Mydatatable.Rows[i]["ADWeight"] = Mydatatable.Rows[i].Field<Single>("BDWeight") / 0.9;
                }
            }

            try
            {
                DataView dv = mydatatable.DefaultView;
                mydatatable.DefaultView.Sort = "Index ASC";

                int xy = 0;
                int j = 0;

                foreach (var item in mydatatable.Rows)
                {
                    if (mydatatable.Rows[xy]["Position"].ToString() == BaleInDrop)
                    {
                        if (iSample > 0)
                        {
                            int idx1 = 1;
                            j = xy;
                            ChartdataOne = new ObservableCollection<ClsChartData>();
                            do
                            {
                                if (StrSelectedItem == "Forte")
                                {
                                    ChartdataOne.Add(new ClsChartData()
                                    {
                                        Index = idx1,
                                        Value = (int)mydatatable.Rows[j][StrSelectedItem] * iCoef,
                                        ChartColor = Color1
                                    });
                                    BalesLis1.Add((int)mydatatable.Rows[j][StrSelectedItem] * iCoef);
                                }
                                else
                                {
                                    ChartdataOne.Add(new ClsChartData()
                                    {
                                        Index = idx1,
                                        Value = mydatatable.Rows[j].Field<Single>(StrSelectedItem) * iCoef,
                                        ChartColor = Color1
                                    });
                                    BalesLis1.Add(mydatatable.Rows[j].Field<Single>(StrSelectedItem) * iCoef);
                                }
                                j++;
                                idx1++;

                            } while (j < Convert.ToUInt32(iBaleinDrop + xy));
                            iSample--;
                        }

                        ///                        
                        if (iSample > 0)
                        {
                            int idx2 = 1;
                            xy = j;
                            do
                            {
                                if (StrSelectedItem == "Forte")
                                {
                                    ChartdataTwo.Add(new ClsChartData()
                                    {
                                        Index = idx2,
                                        Value = (int)mydatatable.Rows[j][StrSelectedItem] * iCoef,
                                        ChartColor = Color2
                                    });
                                    BalesLis2.Add((int)mydatatable.Rows[j][StrSelectedItem] * iCoef);
                                }
                                else
                                {
                                    ChartdataTwo.Add(new ClsChartData()
                                    {
                                        Index = idx2,
                                        Value = mydatatable.Rows[j].Field<Single>(StrSelectedItem) * iCoef,
                                        ChartColor = Color2
                                    });
                                    BalesLis2.Add(mydatatable.Rows[j].Field<Single>(StrSelectedItem) * iCoef);
                                }
                                j++;
                                idx2++;

                            } while (j < Convert.ToUInt32(iBaleinDrop + xy));
                            iSample--;
                        }
                        ///                        
                        if (iSample > 0)
                        {
                            int idx2 = 1;
                            xy = j;
                            do
                            {
                                if (StrSelectedItem == "Forte")
                                {
                                    ChartdataThree.Add(new ClsChartData()
                                    {
                                        Index = idx2,
                                        Value = (int)mydatatable.Rows[j][StrSelectedItem] * iCoef,
                                        ChartColor = Color3
                                    });
                                    BalesLis3.Add((int)mydatatable.Rows[j][StrSelectedItem] * iCoef);
                                }
                                else
                                {
                                    ChartdataThree.Add(new ClsChartData()
                                    {
                                        Index = idx2,
                                        Value = mydatatable.Rows[j].Field<Single>(StrSelectedItem) * iCoef,
                                        ChartColor = Color3
                                    });
                                    BalesLis3.Add(mydatatable.Rows[j].Field<Single>(StrSelectedItem) * iCoef);
                                }
                                j++;
                                idx2++;

                            } while (j < Convert.ToUInt32(iBaleinDrop + xy));
                            iSample--;
                        }
                        ///                        
                        if (iSample > 0)
                        {
                            int idx2 = 1;
                            xy = j;
                            do
                            {
                                if (StrSelectedItem == "Forte")
                                {
                                    ChartdataFour.Add(new ClsChartData()
                                    {
                                        Index = idx2,
                                        Value = (int)mydatatable.Rows[j][StrSelectedItem] * iCoef,
                                        ChartColor = Color4
                                    });
                                    BalesLis4.Add((int)mydatatable.Rows[j][StrSelectedItem] * iCoef);
                                }
                                else
                                {
                                    ChartdataFour.Add(new ClsChartData()
                                    {
                                        Index = idx2,
                                        Value = mydatatable.Rows[j].Field<Single>(StrSelectedItem) * iCoef,
                                        ChartColor = Color4
                                    });
                                    BalesLis4.Add(mydatatable.Rows[j].Field<Single>(StrSelectedItem) * iCoef);
                                }
                                j++;
                                idx2++;

                            } while (j < Convert.ToUInt32(iBaleinDrop + xy));
                            iSample--;
                        }
                        ///                        
                        if (iSample > 0)
                        {
                            int idx2 = 1;
                            xy = j;
                            do
                            {
                                if (StrSelectedItem == "Forte")
                                {
                                    ChartdataFive.Add(new ClsChartData()
                                    {
                                        Index = idx2,
                                        Value = (int)mydatatable.Rows[j][StrSelectedItem] * iCoef,
                                        ChartColor = Color5
                                    });
                                    BalesLis5.Add((int)mydatatable.Rows[j][StrSelectedItem] * iCoef);
                                }
                                else
                                {
                                    ChartdataFive.Add(new ClsChartData()
                                    {
                                        Index = idx2,
                                        Value = mydatatable.Rows[j].Field<Single>(StrSelectedItem) * iCoef,
                                        ChartColor = Color5
                                    });
                                    BalesLis5.Add(mydatatable.Rows[j].Field<Single>(StrSelectedItem) * iCoef);
                                }
                                j++;
                                idx2++;

                            } while (j < Convert.ToUInt32(iBaleinDrop + xy));
                            iSample--;

                        }
                        ///                        
                        if (iSample > 0)
                        {
                            int idx2 = 1;
                            xy = j;
                            do
                            {
                                if (StrSelectedItem == "Forte")
                                {
                                    ChartdataSix.Add(new ClsChartData()
                                    {
                                        Index = idx2,
                                        Value = (int)mydatatable.Rows[j][StrSelectedItem] * iCoef,
                                        ChartColor = Color6
                                    });
                                    BalesLis6.Add((int)mydatatable.Rows[j][StrSelectedItem] * iCoef);
                                }
                                else
                                {
                                    ChartdataSix.Add(new ClsChartData()
                                    {
                                        Index = idx2,
                                        Value = mydatatable.Rows[j].Field<Single>(StrSelectedItem) * iCoef,
                                        ChartColor = Color6
                                    });
                                    BalesLis6.Add(mydatatable.Rows[j].Field<Single>(StrSelectedItem) * iCoef);
                                }
                                j++;
                                idx2++;

                            } while (j < Convert.ToUInt32(iBaleinDrop + xy));
                            iSample--;
                        }

                        if (iSample > 0)
                        {
                            int idx2 = 1;
                            xy = j;
                            do
                            {
                                if (StrSelectedItem == "Forte")
                                {
                                    ChartdataSeven.Add(new ClsChartData()
                                    {
                                        Index = idx2,
                                        Value = (int)mydatatable.Rows[j][StrSelectedItem] * iCoef,
                                        ChartColor = Color7
                                    });
                                    BalesLis7.Add((int)mydatatable.Rows[j][StrSelectedItem] * iCoef);
                                }
                                else
                                {
                                    ChartdataSeven.Add(new ClsChartData()
                                    {
                                        Index = idx2,
                                        Value = mydatatable.Rows[j].Field<Single>(StrSelectedItem) * iCoef,
                                        ChartColor = Color7
                                    });
                                    BalesLis7.Add(mydatatable.Rows[j].Field<Single>(StrSelectedItem) * iCoef);
                                }

                                j++;
                                idx2++;

                            } while (j < Convert.ToUInt32(iBaleinDrop + xy));
                            iSample--;
                        }
                        if (iSample > 0)
                        {
                            int idx2 = 1;
                            xy = j;
                            do
                            {
                                if (StrSelectedItem == "Forte")
                                {
                                    ChartdataEight.Add(new ClsChartData()
                                    {
                                        Index = idx2,
                                        Value = (int)mydatatable.Rows[j][StrSelectedItem] * iCoef,
                                        ChartColor = Color8
                                    });
                                    BalesLis8.Add((int)mydatatable.Rows[j][StrSelectedItem] * iCoef);
                                }
                                else
                                {
                                    ChartdataEight.Add(new ClsChartData()
                                    {
                                        Index = idx2,
                                        Value = mydatatable.Rows[j].Field<Single>(StrSelectedItem) * iCoef,
                                        ChartColor = Color8
                                    });
                                    BalesLis8.Add(mydatatable.Rows[j].Field<Single>(StrSelectedItem) * iCoef);
                                }
                                j++;
                                idx2++;

                            } while (j < Convert.ToUInt32(iBaleinDrop + xy));
                            iSample--;
                        }
                        if (iSample > 0)
                        {
                            int idx2 = 1;
                            xy = j;
                            do
                            {
                                if (StrSelectedItem == "Forte")
                                {
                                    ChartdataNine.Add(new ClsChartData()
                                    {
                                        Index = idx2,
                                        Value = (int)mydatatable.Rows[j][StrSelectedItem] * iCoef,
                                        ChartColor = Color9
                                    });
                                    BalesLis9.Add((int)mydatatable.Rows[j][StrSelectedItem] * iCoef);
                                }
                                else
                                {
                                    ChartdataNine.Add(new ClsChartData()
                                    {
                                        Index = idx2,
                                        Value = mydatatable.Rows[j].Field<Single>(StrSelectedItem) * iCoef,
                                        ChartColor = Color9
                                    });
                                    BalesLis9.Add(mydatatable.Rows[j].Field<Single>(StrSelectedItem) * iCoef);
                                }
                                j++;
                                idx2++;

                            } while (j < Convert.ToUInt32(iBaleinDrop + xy));
                            iSample--;
                        }

                        if (iSample > 0)
                        {
                            int idx2 = 1;
                            xy = j;
                            do
                            {
                                if (StrSelectedItem == "Forte")
                                {
                                    ChartdataTen.Add(new ClsChartData()
                                    {
                                        Index = idx2,
                                        Value = (int)mydatatable.Rows[j][StrSelectedItem] * iCoef,
                                        ChartColor = Color10
                                    });
                                    BalesLis10.Add((int)mydatatable.Rows[j][StrSelectedItem] * iCoef);
                                }
                                else
                                {
                                    ChartdataTen.Add(new ClsChartData()
                                    {
                                        Index = idx2,
                                        Value = mydatatable.Rows[j].Field<Single>(StrSelectedItem) * iCoef,
                                        ChartColor = Color10
                                    });
                                    BalesLis10.Add(mydatatable.Rows[j].Field<Single>(StrSelectedItem) * iCoef);
                                }
                                j++;
                                idx2++;

                            } while (j < Convert.ToUInt32(iBaleinDrop + xy));
                            iSample--;
                        }
                    }
                }


                for (int i = 0; i < iBaleInDrop; i++)
                {
                    if (BalesLis1.Count > 0)
                        AveragePosition.Add(BalesLis1[i]);

                    if (BalesLis2.Count > 0)
                        AveragePosition.Add(BalesLis2[i]);

                    if (BalesLis3.Count > 0)
                        AveragePosition.Add(BalesLis3[i]);

                    if (BalesLis4.Count > 0)
                        AveragePosition.Add(BalesLis4[i]);

                    if (BalesLis5.Count > 0)
                        AveragePosition.Add(BalesLis5[i]);

                    if (BalesLis6.Count > 0)
                        AveragePosition.Add(BalesLis6[i]);

                    if (BalesLis7.Count > 0)
                        AveragePosition.Add(BalesLis7[i]);

                    if (BalesLis8.Count > 0)
                        AveragePosition.Add(BalesLis8[i]);

                    if (BalesLis9.Count > 0)
                        AveragePosition.Add(BalesLis9[i]);

                    if (BalesLis10.Count > 0)
                        AveragePosition.Add(BalesLis10[i]);

                    GetAverageBalePosition(i, AveragePosition);
                    AveragePosition.Clear();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in DropPositionViewModel=> ProcessData " + ex.Message);
            }
        }

        private void GetAverageBalePosition(int i, List<double> AveragePosition)
        {
            if (AveragePosition.Count > 0)
            {
                switch (i)
                {
                    case 0:
                        VisCrtOne = System.Windows.Visibility.Visible;
                        BalePos1Avg = AveragePosition.Average().ToString("#0.00");
                        break;
                    case 1:
                        VisCrtTwo = System.Windows.Visibility.Visible;
                        BalePos2Avg = AveragePosition.Average().ToString("#0.00");
                        break;
                    case 2:
                        VisCrtThree = System.Windows.Visibility.Visible;
                        BalePos3Avg = AveragePosition.Average().ToString("#0.00");
                        break;
                    case 3:
                        VisCrtFour = System.Windows.Visibility.Visible;
                        BalePos4Avg = AveragePosition.Average().ToString("#0.00");
                        break;
                    case 4:
                        VisCrtFive = System.Windows.Visibility.Visible;
                        BalePos5Avg = AveragePosition.Average().ToString("#0.00");
                        break;
                    case 5:
                        VisCrtSix = System.Windows.Visibility.Visible;
                        BalePos6Avg = AveragePosition.Average().ToString("#0.00");
                        break;
                    case 6:
                        VisCrtSeven = System.Windows.Visibility.Visible;
                        BalePos7Avg = AveragePosition.Average().ToString("#0.00");
                        break;
                    case 7:
                        VisCrtEight = System.Windows.Visibility.Visible;
                        BalePos8Avg = AveragePosition.Average().ToString("#0.00");
                        break;
                    case 8:
                        VisCrtNine = System.Windows.Visibility.Visible;
                        BalePos9Avg = AveragePosition.Average().ToString("#0.00");
                        break;
                    case 9:
                        VisCrtTen = System.Windows.Visibility.Visible;
                        BalePos10Avg = AveragePosition.Average().ToString("#0.00");
                        break;

                    default:
                        break;
                }
            }
        }

        private void SetChartNew()
        {
            if (ChartdataOne != null) ChartdataOne = null;
            ChartdataOne = new ObservableCollection<ClsChartData>();

            if (ChartdataTwo != null) ChartdataTwo = null;
            ChartdataTwo = new ObservableCollection<ClsChartData>();

            if (ChartdataThree != null) ChartdataThree = null;
            ChartdataThree = new ObservableCollection<ClsChartData>();

            if (ChartdataFour != null) ChartdataFour = null;
            ChartdataFour = new ObservableCollection<ClsChartData>();

            if (ChartdataFive != null) ChartdataFive = null;
            ChartdataFive = new ObservableCollection<ClsChartData>();

            if (ChartdataSix != null) ChartdataSix = null;
            ChartdataSix = new ObservableCollection<ClsChartData>();

            if (ChartdataSeven != null) ChartdataSeven = null;
            ChartdataSeven = new ObservableCollection<ClsChartData>();

            if (ChartdataEight != null) ChartdataEight = null;
            ChartdataEight = new ObservableCollection<ClsChartData>();

            if (ChartdataNine != null) ChartdataNine = null;
            ChartdataNine = new ObservableCollection<ClsChartData>();

            if (ChartdataTen != null) ChartdataTen = null;
            ChartdataTen = new ObservableCollection<ClsChartData>();

            if (BalesLis1 != null) BalesLis1 = null;
            BalesLis1 = new List<double>();

            if (BalesLis2 != null) BalesLis2 = null;
            BalesLis2 = new List<double>();

            if (BalesLis3 != null) BalesLis3 = null;
            BalesLis3 = new List<double>();

            if (BalesLis4 != null) BalesLis4 = null;
            BalesLis4 = new List<double>();

            if (BalesLis5 != null) BalesLis5 = null;
            BalesLis5 = new List<double>();

            if (BalesLis6 != null) BalesLis6 = null;
            BalesLis6 = new List<double>();

            if (BalesLis7 != null) BalesLis7 = null;
            BalesLis7 = new List<double>();

            if (BalesLis8 != null) BalesLis8 = null;
            BalesLis8 = new List<double>();

            if (BalesLis9 != null) BalesLis9 = null;
            BalesLis9 = new List<double>();

            if (BalesLis10 != null) BalesLis10 = null;
            BalesLis10 = new List<double>();
        }

        private void ClearObjects()
        {
            if (dispatcherTimer != null) dispatcherTimer = null;

            if (ChartdataOne != null) ChartdataOne = null;
            if (ChartdataTwo != null) ChartdataTwo = null;
            if (ChartdataThree != null) ChartdataThree = null;
            if (ChartdataFour != null) ChartdataFour = null;
            if (ChartdataFive != null) ChartdataFive = null;
            if (ChartdataSix != null) ChartdataSix = null;
            if (ChartdataSeven != null) ChartdataSeven = null;
            if (ChartdataEight != null) ChartdataEight = null;
            if (ChartdataNine != null) ChartdataNine = null;
            if (ChartdataTen != null) ChartdataTen = null;

            if (BalesLis1 != null) BalesLis1 = null;
            if (BalesLis2 != null) BalesLis2 = null;
            if (BalesLis3 != null) BalesLis3 = null;
            if (BalesLis4 != null) BalesLis4 = null;
            if (BalesLis5 != null) BalesLis5 = null;
            if (BalesLis6 != null) BalesLis6 = null;
            if (BalesLis7 != null) BalesLis7 = null;
            if (BalesLis8 != null) BalesLis8 = null;
            if (BalesLis9 != null) BalesLis9 = null;
            if (BalesLis10 != null) BalesLis10 = null;

        }


        private void ClosedPageExecute()
        {
            BalePosition = string.Empty;
            ClearObjects();
        }

        private void GetNewData()
        {
            int iLastBale;
            string BalePosition;

            //Get New Data when last bale in drop arrived
            if (Settings.Default.bDropHitoLow)
                iLastBale = 1;
            else
                iLastBale = ClassCommon.BaleInADrop; // MainWindow.AppWindows.iBalesinDrop;

            string StrQuery = BuildQueryString(IntColSamples);
            Mydatatable = _sqlhandler.GetForteDataTable(StrQuery);

            BalePosition = Mydatatable.Rows[0]["Position"].ToString();
            this.BalePosition = BalePosition + "   -> Scan timer Update @ : " + DateTime.Now;

            BPosition = BalePosition;

            if (BalePosition == iLastBale.ToString())
            {
                PositionColor = System.Windows.Media.Brushes.Yellow;
                // CurIndex = Convert.ToInt32(Mydatatable.Rows[0]["index"]);
                CurIndex = Mydatatable.Rows[0].Field<int>("index");
                if (CurIndex != PreIndex)
                {
                    ProcessData(Mydatatable, StrSelectedItem);
                    PreIndex = CurIndex;
                }
            }
            else
                PositionColor = System.Windows.Media.Brushes.White;
        }


        private void UpdateStatus(string strMsg)
        {
            UpdateInfo = strMsg;
        }




        #region DispatcherTimer /////////////////////////////////////////////////////////////////////////////////////

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
            //  Application.Current.Dispatcher.Invoke(new Action(() => { Heartbeat(); }));

            Thread.Sleep(500); //Rest for 1/2 Sec.
            dispatcherTimer.Start();
        }

        // private void Heartbeat()
        //  {
        //      //throw new NotImplementedException();
        //  }


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
            dispatcherTimer.Stop();
            RTRunning = false;


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

            _eventAggregator.GetEvent<UpdatedEvent>().Publish(RTRunning);
            // ShowMe = 0.1;
            // Opac = 1.0;
        }

        #endregion DispatcherTimer /////////////////////////////////////////////////////////////////////////////////////////


    }


}
