using ForteArg.Services;
using ForteARP.Module_Combine.ViewModels;
using ForteARP.Modules;
using ForteARP.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ForteARP.Module_Combine.Views
{
    /// <summary>
    /// Interaction logic for CombineView.xaml
    /// </summary>
    [Export(typeof(IModule))]
    public partial class CombineView : UserControl, IModule
    {
        public static CombineView CombineWindows;

        readonly CombineViewModel MyViewmodel;
        private double IScreenWidth { get; set; }

        private double wdCoef = 0.0;

        private Point startpoint;

        private int _index;
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        public string MName
        {
            get { return "CombineView"; }
            set { }
        }
        public UserControl UserInterface
        {
            get { return new CombineView(); }
            set { }
        }
        public bool BActive
        {
            get { return false; }
            set { }
        }

        public CombineView()
        {
            InitializeComponent();

            Index = 10;
            MyViewmodel = new CombineViewModel(ApplicationService.Instance.EventAggregator);
            this.DataContext = MyViewmodel;
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

        private void TextHiLim_dclick(object sender, MouseButtonEventArgs e)
        {
            if (txtHiLimit.IsReadOnly == false)
            {
                txtHiLimit.Background = Brushes.AntiqueWhite;
                txtHiLimit.IsReadOnly = true;
            }
            else
            {
                txtHiLimit.Background = Brushes.White;
                txtHiLimit.IsReadOnly = false;
            }
        }

        private void TextLoLim_dclick(object sender, MouseButtonEventArgs e)
        {
            if (txtLoLimit.IsReadOnly == false)
            {
                txtLoLimit.Background = Brushes.AntiqueWhite;
                txtLoLimit.IsReadOnly = true;
            }
            else
            {
                txtLoLimit.Background = Brushes.White;
                txtLoLimit.IsReadOnly = false;
            }
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

                    MyViewmodel.SelectedHdrList = newlist;
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

                    MyViewmodel.SelectedHdrList = newlist;
                    SelectedHdrList.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in RightClick " + ex.Message);
            }
        }

        private void PopUp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            startpoint = e.GetPosition(null);
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
    }
}
