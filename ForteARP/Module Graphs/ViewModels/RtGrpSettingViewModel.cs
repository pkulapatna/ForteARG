using ForteARP.Properties;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ForteARP.Module_Graphs.ViewModels
{
    public class RtGrpSettingViewModel : BindableBase
    {
        protected readonly IEventAggregator _eventAggregator;


        private bool _bModify = false;
        public bool BModify
        {
            get =>  _bModify;
            set => SetProperty(ref _bModify, value);
        }

        private bool _bScaleAuto = Settings.Default.BScaleAuto;
        public bool BScaleAuto
        {
            get => _bScaleAuto; 
            set => SetProperty(ref _bScaleAuto, value); 
        }


        private bool _autoChecked;
        public bool AutoChecked
        {
            get => _autoChecked;
            set => SetProperty(ref _autoChecked, value);
        }

        private bool _manualChecked;
        public bool ManualChecked
        {
            get => _manualChecked;
            set => SetProperty(ref _manualChecked, value);
        }

        private Brush _ChartColor = Brushes.Black;
        public Brush BackGroundChartColor
        {
            get { return _ChartColor; }
            set { SetProperty(ref _ChartColor, value); }
        }

        private int _backgndchartidx = Settings.Default.backgndchartidx;
        public int Backgndchartidx
        {
            get { return _backgndchartidx; }
            set 
            {
                SetProperty(ref _backgndchartidx, value);
                Settings.Default.backgndchartidx = value;
                Settings.Default.Save();
            }
        }

        private List<string> _backgndchartLst;
        public List<string> BackgndchartLst
        {
            get { return _backgndchartLst; }
            set { SetProperty(ref _backgndchartLst, value); }
        }

        //
        private List<string> _graphByLst;
        public List<string> GraphByLst
        {
            get { return _graphByLst; }
            set { SetProperty(ref _graphByLst, value); }
        }

        private int _graphByLstidx = Settings.Default.graphByLstidx;
        public int GraphByLstidx
        {
            get { return _graphByLstidx; }
            set
            {
                SetProperty(ref _graphByLstidx, value);
                Settings.Default.graphByLstidx = value;
                Settings.Default.Save();
            }
        }



        //Moisture lowlimit
        private float _dGraphLoM = Settings.Default.GraphLoMenuOne;
        public float DGraphLoM
        {
            get { return _dGraphLoM; }
            set { SetProperty(ref _dGraphLoM, value); }
        }
        //Weight lowlimit
        private float _dGraphLoW = Settings.Default.GraphLoMenuTwo;
        public float DGraphLoW
        {
            get { return _dGraphLoW; }
            set { SetProperty(ref _dGraphLoW, value); }
        }
        //BoneDry Weight lowlimit
        private float _dGraphLoDBwt = Settings.Default.GraphLoMenuThree;
        public float DGraphLoDBwt
        {
            get { return _dGraphLoDBwt; }
            set { SetProperty(ref _dGraphLoDBwt, value); }
        }
        //AirDry Weight lowlimit
        private float _dGraphLoADWt = Settings.Default.GraphLoMenuFour;
        public float DGraphLoADWt
        {
            get { return _dGraphLoADWt; }
            set { SetProperty(ref _dGraphLoADWt, value); }
        }
        //Dirt lowLimit
        private float _dGraphLoDirt = Settings.Default.GraphLoMenuFive;
        public float DGraphLoDirt
        {
            get { return _dGraphLoDirt; }
            set { SetProperty(ref _dGraphLoDirt, value); }
        }
        //Brightness lowLimit
        private float _dGraphLoBright = Settings.Default.GraphLoMenuSix;
        public float DGraphLoBright
        {
            get { return _dGraphLoBright; }
            set { SetProperty(ref _dGraphLoBright, value); }
        }
        //Visc lowLimit
        private float _dGraphLoVisc = Settings.Default.GraphLoMenuSeven;
        public float DGraphLoVisco
        {
            get { return _dGraphLoVisc; }
            set { SetProperty(ref _dGraphLoVisc, value); }
        }

        //Moisture hilimit
        private float _dGraphHiM = Settings.Default.GraphHiMenuOne;
        public float DGraphHiM
        {
            get { return _dGraphHiM; }
            set { SetProperty(ref _dGraphHiM, value); }
        }
        //Weight hilimit
        private float _dGraphHiW = Settings.Default.GraphHiMenuTwo;
        public float DGraphHiW
        {
            get { return _dGraphHiW; }
            set { SetProperty(ref _dGraphHiW, value); }
        }
        //BoneDry Weight hilimit
        private float _dGraphHiDBwt = Settings.Default.GraphHiMenuThree;
        public float DGraphHiDBwt
        {
            get { return _dGraphHiDBwt; }
            set { SetProperty(ref _dGraphHiDBwt, value); }
        }
        //AirDry Weight hilimit
        private float _dGraphHiADWt = Settings.Default.GraphHiMenuFour;
        public float DGraphHiADWt
        {
            get { return _dGraphHiADWt; }
            set { SetProperty(ref _dGraphHiADWt, value); }
        }
        //Dirt hiLimit
        private float _dGraphHiDirt = Settings.Default.GraphHiMenuFive;
        public float DGraphHiDirt
        {
            get { return _dGraphHiDirt; }
            set { SetProperty(ref _dGraphHiDirt, value); }
        }
        //Brightness hiLimit
        private float _dGraphHiBright = Settings.Default.GraphHiMenuSix;
        public float DGraphHiBright
        {
            get { return _dGraphHiBright; }
            set { SetProperty(ref _dGraphHiBright, value); }
        }
        //Visc hiLimit
        private float _dGraphHiVisc = (float)Settings.Default.GraphHiMenuSeven;
        public float DGraphHiVisco
        {
            get { return _dGraphHiVisc; }
            set { SetProperty(ref _dGraphHiVisc, value); }
        }


        //Moisture Y-Axis bottom Boundary
        private float _dYxLowM = Settings.Default.dYxLowM;
        public float DYxLowM
        {
            get { return _dYxLowM; }
            set { SetProperty(ref _dYxLowM, value); }
        }
        //Weight Y-Axis bottom Boundary
        private float _dYxLowW = Settings.Default.dYxLowW;
        public float DYxLowW
        {
            get { return _dYxLowW; }
            set { SetProperty(ref _dYxLowW, value); }
        }

        //BoneDry Weight Y-Axis bottom Boundary
        private float _dYxLowBdWt = Settings.Default.dYxLowBdWt;
        public float DYxLowBdWt
        {
            get { return _dYxLowBdWt; }
            set { SetProperty(ref _dYxLowBdWt, value); }
        }

        //AirDry Weight  Y-Axis bottom Boundary
        private float _dYxLowADWt = Settings.Default.dYxLowADWt;
        public float DYxLowADWt
        {
            get { return _dYxLowADWt; }
            set { SetProperty(ref _dYxLowADWt, value); }
        }

        //Dirt  Y-Axis bottom Boundary
        private float _dYxLowDirt = Settings.Default.dYxLowDirt;
        public float DYxLowDirt
        {
            get { return _dYxLowDirt; }
            set { SetProperty(ref _dYxLowDirt, value); }
        }

        //BrightnessY-Axis bottom Boundary
        private float _dYxLowBright = Settings.Default.dYxLowBright;
        public float DYxLowBright
        {
            get { return _dYxLowBright; }
            set { SetProperty(ref _dYxLowBright, value); }
        }

        //Vicosity-Axis bottom Boundary
        private float _dYxLowVisco = Settings.Default.dYxLowVisco;
        public float DYxLowVisco
        {
            get { return _dYxLowVisco; }
            set { SetProperty(ref _dYxLowVisco, value); }
        }


        //--------------------------------------------------------------------


        //Moisture Y-Axis top Boundary
        private float _dYxHiM = Settings.Default.dYxHiM;
        public float DYxHiM
        {
            get { return _dYxHiM; }
            set { SetProperty(ref _dYxHiM, value); }
        }
        //Weight Y-Axis top Boundary
        private float _dYxHiW = Settings.Default.dYxHiW;
        public float DYxHiW
        {
            get { return _dYxHiW; }
            set { SetProperty(ref _dYxHiW, value); }
        }

        //BoneDry Weight Y-Axis top Boundary
        private float _dYxHiBdWt = Settings.Default.dYxHiBdWt;
        public float DYxHiBdWt
        {
            get { return _dYxHiBdWt; }
            set { SetProperty(ref _dYxHiBdWt, value); }
        }

        //AirDry Weight  Y-Axis top Boundary
        private float _dYxHiADWt = Settings.Default.dYxHiADWt;
        public float DYxHiADWt
        {
            get { return _dYxHiADWt; }
            set { SetProperty(ref _dYxHiADWt, value); }
        }

        //Dirt  Y-Axis top Boundary
        private float _dYxHiDirt = Settings.Default.dYxHiDirt;
        public float DYxHiDirt
        {
            get { return _dYxHiDirt; }
            set { SetProperty(ref _dYxHiDirt, value); }
        }

        //BrightnessY-Axis top Boundary
        private float _dYxHiBright = Settings.Default.dYxHiBright;
        public float DYxHiBright
        {
            get { return _dYxHiBright; }
            set { SetProperty(ref _dYxHiBright, value); }
        }


        //Vicosity-Axis top Boundary
        private float _dYxHiVisco = Settings.Default.dYxHiVisco;
        public float DYxHiVisco
        {
            get { return _dYxHiVisco; }
            set { SetProperty(ref _dYxHiVisco, value); }
        }


        //--------------------------------------------------------------------

        public RtGrpSettingViewModel(Prism.Events.IEventAggregator EventAggregator)
        {
            this._eventAggregator = EventAggregator;


            BackgndchartLst = new List<string>
            {
                "Black",
                "White"
            };

            GraphByLst = new List<string>
            {
                "All",
                "Source",
                "Line"
            };

            if (Settings.Default.BScaleAuto)
            {
                AutoChecked = true;
            }
            else
            {
                ManualChecked = true;
            }

        }

        private DelegateCommand _settingsCommand;
        public DelegateCommand SettingsCommand =>
        _settingsCommand ?? (_settingsCommand = new DelegateCommand(SettingsCommandExecute));
        private void SettingsCommandExecute()
        {
            BModify = true;
        }

        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
        _saveCommand ?? (_saveCommand =
            new DelegateCommand(SaveCommandExecute).ObservesCanExecute(() => BModify));
        private void SaveCommandExecute()
        {
            BModify = false;
            SaveValues();
        }

        private void SaveValues()
        {
            Settings.Default.GraphLoMenuOne = DGraphLoM;
            Settings.Default.GraphHiMenuOne = DGraphHiM;

            Settings.Default.GraphLoMenuTwo = DGraphLoW;
            Settings.Default.GraphHiMenuTwo = DGraphHiW;

            Settings.Default.GraphLoMenuThree = DGraphLoDBwt;
            Settings.Default.GraphHiMenuThree = DGraphHiDBwt;

            Settings.Default.GraphLoMenuFour = DGraphLoADWt;
            Settings.Default.GraphHiMenuFour = DGraphHiADWt;

            Settings.Default.GraphLoMenuFive = DGraphLoDirt;
            Settings.Default.GraphHiMenuFive = DGraphHiDirt;

            Settings.Default.GraphLoMenuSix = DGraphLoBright;
            Settings.Default.GraphHiMenuSix = DGraphHiBright;

            Settings.Default.GraphLoMenuSeven = DGraphLoVisco;
            Settings.Default.GraphHiMenuSeven = DGraphHiVisco;

            // moisture Y-Axis 0
            Settings.Default.dYxLowM = DYxLowM;
            Settings.Default.dYxHiM = DYxHiM;
            // weight Y-Axis 1
            Settings.Default.dYxLowW = DYxLowW;
            Settings.Default.dYxHiW = DYxHiW;
            //Bone Dry Weight  Y-Axis 2
            Settings.Default.dYxLowBdWt = DYxLowBdWt;
            Settings.Default.dYxHiBdWt = DYxHiBdWt;
            //Air Dry Weight  Y-Axis 3
            Settings.Default.dYxLowADWt = DYxLowADWt;
            Settings.Default.dYxHiADWt = DYxHiADWt;
            // Dirt  Y-Axis 4
            Settings.Default.dYxLowDirt = DYxLowDirt;
            Settings.Default.dYxHiDirt = DYxHiDirt;
            // Bright  Y-Axis 5
            Settings.Default.dYxLowBright = DYxLowBright;
            Settings.Default.dYxHiBright = DYxHiBright;
            // Visco  Y-Axis 6
            Settings.Default.dYxLowVisco = DYxLowVisco;
            Settings.Default.dYxHiVisco = DYxHiVisco;

            Settings.Default.BScaleAuto = AutoChecked;
            Settings.Default.Save();
        }
    }
}
