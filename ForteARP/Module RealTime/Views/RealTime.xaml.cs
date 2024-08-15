

using ForteArg.Services;
using ForteARP.Module_RealTime.ViewModels;
using ForteARP.Modules;
using ForteARP.Properties;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ForteARP.Module_RealTime.Views
{
    /// <summary>
    /// Interaction logic for RealTime.xaml
    /// </summary>
    [Export(typeof(IModule))]
    public partial class RealTime : UserControl, IModule
    {
        public static RealTime RTWindows;

        private readonly Storyboard myStoryboard = new Storyboard();
        readonly RealTimeViewModel MyRealtimeViewModel;
        private Point startpoint;
        private double IScreenWidth { get; set; }

        private double wdCoef = 0.0;
        public new GridLength Height { get; set; }
        

        private int _index;
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        public string MName
        {
            get { return GetTabName(); }
            set { }
        }
        public UserControl UserInterface
        {
            get { return new RealTime(); }
            set { }
        }
        public bool BActive
        {
            get { return MainWindow.AppWindows.bBaleRealtime; }
            set { }
        }


        private string GetTabName()
        {
            string tabName;

            switch (Settings.Default.iLanguageIdx) //Thread.CurrentThread.CurrentCulture.ToString())
            {
                case 0: // "en-US":
                    tabName = "RealTime";
                    break;
                case 1: //"Sp-SP":
                    tabName = "TiempoReal";
                    break;
                default:
                    tabName = "RealTime";
                    break;
            }
            return tabName;
        }

        public RealTime()
        {
            InitializeComponent();
            Index = 3;
            RTWindows = this;

            MyRealtimeViewModel = new RealTimeViewModel(ApplicationService.Instance.EventAggregator);
            this.DataContext = MyRealtimeViewModel;
        }

        private void PopUp_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point relative = e.GetPosition(null);
                Point AbsolutePos = new Point(relative.X, relative.Y);
                MyPopup.HorizontalOffset += AbsolutePos.X - startpoint.X;
                MyPopup.VerticalOffset += AbsolutePos.Y - startpoint.Y;
            }
        }

        private void PopUp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            startpoint = e.GetPosition(null);
        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            RTGridView.Columns[0].Visibility = Visibility.Collapsed;

            if (e.PropertyName.StartsWith("Moisture"))
            {
                switch (Settings.Default.MoistureUnit)
                {
                    case 0: // %MC
                        e.Column.Header = "MC %";
                        break;

                    case 1: // %MR
                        e.Column.Header = "MR %";
                        break;

                    case 2: // %AD
                        e.Column.Header = "AD %";
                        break;

                    case 3: // %BD
                        e.Column.Header = "BD %";
                        break;
                }
            }
            if (e.PropertyName.StartsWith("Weight"))
            {
                if (Settings.Default.WeightUnit == 0)
                    e.Column.Header = "Weight (Kg)";
                else
                    e.Column.Header = "Weight (lb)";
            }

            if (e.PropertyName.StartsWith("Deviation"))
                e.Column.Header = "%CV";

            if (e.PropertyName.StartsWith("Finish"))
                e.Column.Header = "Viscosity";

            if (e.PropertyName.StartsWith("FC_LotIdentString"))
                e.Column.Header = "CusLotNumber";

            if (e.PropertyName.StartsWith("CalibrationName"))
                e.Column.Header = "Calibrration";

            if (ClassCommon.WLOptions)
            {
                if (e.PropertyName.StartsWith("SpareSngFld3"))
                    e.Column.Header = "%CV";
            }

            //Package ForteStatus TareWeight

            if ((e.PropertyType == typeof(System.Single)) || (e.PropertyType == typeof(System.Double)))
            {
                e.Column.ClipboardContentBinding.StringFormat = "{0:0.##}";
                e.Column.Width = e.Column.Header.ToString().Length + wdCoef;
                //e.Column.Width = 110;
            }
            else if (e.PropertyType == typeof(System.DateTime))
            {
                e.Column.ClipboardContentBinding.StringFormat = "MM-dd-yyyy HH:mm";
                e.Column.Width = e.Column.Header.ToString().Length + wdCoef * 1.2;
            }
            else
                e.Column.Width = e.Column.Header.ToString().Length + wdCoef;


            RTGridView.SelectedIndex = 0;
            RTGridView.Focus();
        }


        private void LeftClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedHdrList.SelectedIndex > 0)
                {
                    ObservableCollection<string> newlist = (ObservableCollection<string>)SelectedHdrList.ItemsSource;
                    int NewIndex = SelectedHdrList.SelectedIndex - 1;
                    object selected = SelectedHdrList.SelectedItem;

                    // Removing removable element ItemsControl.ItemsSource
                    newlist.Remove(selected.ToString());
                    // Insert it in new position
                    newlist.Insert(NewIndex, selected.ToString());

                    MyRealtimeViewModel.SelectedHdrList = newlist;
                    SelectedHdrList.Focus();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in LeftClick " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in LeftClick -> {ex.Message}");
            }
        }

        private void RightClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((SelectedHdrList.SelectedIndex > -1) & (SelectedHdrList.SelectedIndex + 1 < SelectedHdrList.Items.Count))
                {
                    ObservableCollection<string> newlist = (ObservableCollection<string>)SelectedHdrList.ItemsSource;
                    int NewIndex = SelectedHdrList.SelectedIndex + 1;
                    object selected = SelectedHdrList.SelectedItem;

                    // Removing removable element ItemsControl.ItemsSource
                    newlist.Remove(selected.ToString());
                    // Insert it in new position
                    newlist.Insert(NewIndex, selected.ToString());

                    MyRealtimeViewModel.SelectedHdrList = newlist;
                    SelectedHdrList.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in RightClick " + ex.Message);
            }
        }

        public void MoveBaleOne(int iBaleNum)
        {
            DoubleAnimation myDoubleAnimation = new DoubleAnimation
            {
                From = 0,
                To = IScreenWidth, // 1700;
                Duration = new Duration(TimeSpan.FromSeconds(5)),
                AutoReverse = false
            };

            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(Canvas.LeftProperty));
            myStoryboard.Children.Add(myDoubleAnimation);

            switch (iBaleNum)
            {
                case 0:
                    Storyboard.SetTargetName(myDoubleAnimation, "b1c0");
                    myStoryboard.Begin(b1c0);
                    break;
                case 1:
                    Storyboard.SetTargetName(myDoubleAnimation, "b1c1");
                    myStoryboard.Begin(b1c1);
                    break;
                case 2:
                    Storyboard.SetTargetName(myDoubleAnimation, "b1c2");
                    myStoryboard.Begin(b1c2);
                    break;
                case 3:
                    Storyboard.SetTargetName(myDoubleAnimation, "b1c3");
                    myStoryboard.Begin(b1c3);
                    break;
                case 4:
                    Storyboard.SetTargetName(myDoubleAnimation, "b1c4");
                    myStoryboard.Begin(b1c4);
                    break;
                case 5:
                    Storyboard.SetTargetName(myDoubleAnimation, "b1c5");
                    myStoryboard.Begin(b1c5);
                    break;
                case 6:
                    Storyboard.SetTargetName(myDoubleAnimation, "b1c6");
                    myStoryboard.Begin(b1c6);
                    break;
                case 7:
                    Storyboard.SetTargetName(myDoubleAnimation, "b1c7");
                    myStoryboard.Begin(b1c7);
                    break;
                case 8:
                    Storyboard.SetTargetName(myDoubleAnimation, "b1c8");
                    myStoryboard.Begin(b1c7);
                    break;
                case 9:
                    Storyboard.SetTargetName(myDoubleAnimation, "b1c9");
                    myStoryboard.Begin(b1c7);
                    break;
                case 10:
                    Storyboard.SetTargetName(myDoubleAnimation, "b1c10");
                    myStoryboard.Begin(b1c7);
                    break;
                case 11:
                    Storyboard.SetTargetName(myDoubleAnimation, "b1c11");
                    myStoryboard.Begin(b1c7);
                    break;
                case 12:
                    Storyboard.SetTargetName(myDoubleAnimation, "b1c12");
                    myStoryboard.Begin(b1c7);
                    break;

                default:
                    Storyboard.SetTargetName(myDoubleAnimation, "b1c0");
                    myStoryboard.Begin(b1c0);
                    break;
            }
            myStoryboard.Children.Clear();
            myDoubleAnimation = null;
        }

        private void SampleBox_dclick(object sender, MouseButtonEventArgs e)
        {
            if (txtSample.IsReadOnly == false)
            {
                txtSample.Background = Brushes.AntiqueWhite;
                txtSample.IsReadOnly = true;
            }
            else
            {
                txtSample.Background = Brushes.White;
                txtSample.IsReadOnly = false;
            }
        }

        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextNumeric(e.Text);
        }

        private static bool IsTextNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9.]+");
            return reg.IsMatch(str);

        }

        private void TextBox_SizeChange(object sender, SizeChangedEventArgs e)
        {
            double xwidth = e.NewSize.Width * .25;

            txtbox1.FontSize = xwidth;
            txtbox2.FontSize = xwidth;
            txtbox3.FontSize = xwidth;
            txtbox4.FontSize = xwidth;
            txtbox5.FontSize = xwidth;
            txtbox6.FontSize = xwidth;
            txtbox7.FontSize = xwidth;
        }

        private void GridView_sidechanged(object sender, SizeChangedEventArgs e)
        {
            IScreenWidth = e.NewSize.Width + 10;
            wdCoef = e.NewSize.Width * 0.08;

            double dxGvHdrSize = e.NewSize.Width * .029;
            double dxGvRwHeight = e.NewSize.Width * .025;
            double dxGvFontSz = e.NewSize.Width * .012;
            double dCmbHeight = e.NewSize.Width * .02;
           // double txtBxHeight = e.NewSize.Width * .030;

            RTGridView.FontSize = dxGvFontSz;
            RTGridView.ColumnHeaderHeight = dxGvHdrSize;
            RTGridView.RowHeight = dxGvRwHeight;
            RTGridView.UpdateLayout();

            cmbOne.FontSize = dxGvFontSz;
            cmbTwo.FontSize = dxGvFontSz;
            cmbThree.FontSize = dxGvFontSz;
            cmbFour.FontSize = dxGvFontSz;
            cmbFive.FontSize = dxGvFontSz;
            cmbSix.FontSize = dxGvFontSz;
            cmbSeven.FontSize = dxGvFontSz;

            cmbOne.Height = dCmbHeight;
            cmbTwo.Height = dCmbHeight;
            cmbThree.Height = dCmbHeight;
            cmbFour.Height = dCmbHeight;
            cmbFive.Height = dCmbHeight;
            cmbSix.Height = dCmbHeight;
            cmbSeven.Height = dCmbHeight;
        }

    }
}
