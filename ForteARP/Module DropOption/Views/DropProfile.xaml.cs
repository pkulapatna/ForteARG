

using ForteArg.Services;
using ForteARP.Module_DropOption.ViewModels;
using ForteARP.Modules;
using ForteARP.Properties;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ForteARP.Module_DropOption.Views
{
    /// <summary>
    /// Interaction logic for DropProfile.xaml
    /// </summary>
    [Export(typeof(IModule))]
    public partial class DropProfile : UserControl, IModule
    {
        private readonly DropProfileViewModel DropProViewModel;
        private Point startpoint;

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
            get { return new DropProfile(); }
            set { }
        }
        public bool BActive
        {
            get { return ClassCommon.bDropProfile; }
            set { }
        }

        private string GetTabName()
        {
            string tabName;

            switch (Settings.Default.iLanguageIdx) //Thread.CurrentThread.CurrentCulture.ToString())
            {
                case 0: // "en-US":
                    tabName = "Drop_Profile";
                    break;
                case 1: //"Sp-SP":
                    tabName = "Perfil Bajada";
                    break;
                default:
                    tabName = "Drop_Profile";
                    break;
            }
            return tabName;
        }

        public DropProfile()
        {
            InitializeComponent();
            if (ClassCommon.bDropProfile)
            {
                Index = 6;
                DropProViewModel = new DropProfileViewModel(ApplicationService.Instance.EventAggregator);
                this.DataContext = DropProViewModel;
                ClsSerilog.LogMessage(ClsSerilog.Info, $"Initialize DropProfile");
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

                    DropProViewModel.SelectedHdrList = newlist;
                    SelectedHdrList.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in RightClick " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in RightClick -> {ex.Message}");
            }
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

        private void LeftClick(object sender, RoutedEventArgs e)
        {
            try
            {

                if (SelectedHdrList.SelectedIndex > 0)
                {
                    ObservableCollection<string> newlist = (ObservableCollection<string>)SelectedHdrList.ItemsSource;
                    int NewIndex = SelectedHdrList.SelectedIndex - 1;

                    if ((NewIndex > -1) || (NewIndex >= SelectedHdrList.Items.Count))
                    {
                        object selected = SelectedHdrList.SelectedItem;

                        // Removing removable element ItemsControl.ItemsSource
                        newlist.Remove(selected.ToString());
                        // Insert it in new position
                        newlist.Insert(NewIndex, selected.ToString());
                        // Restore selection
                        DropProViewModel.SelectedHdrList = newlist;

                        //SelectedHdrList.SelectedItem = selected;
                        SelectedHdrList.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in LeftClick " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in LeftClick -> {ex.Message}");
            }
        }

        private void RTGridView_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //Set first group of drop color
            if (DropProViewModel.iBaleCount < DropProViewModel.BalesInOneDrop)
                e.Row.Background = Brushes.DarkOrange;
            else
                e.Row.Background = Brushes.Transparent;

            DropProViewModel.iBaleCount += 1;
        }

        private void GridView_sidechanged(object sender, SizeChangedEventArgs e)
        {
            RTGridView.RowHeight = e.NewSize.Width * .071;// 74; // 60;
            RTGridView.FontSize = e.NewSize.Width * .02;
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
    }
}
