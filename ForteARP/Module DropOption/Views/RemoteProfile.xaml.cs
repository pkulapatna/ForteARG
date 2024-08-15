

using ForteArg.Services;
using ForteARP.Module_DropOption.ViewModels;
using ForteARP.Modules;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ForteARP.Module_DropOption.Views
{
    /// <summary>
    /// Interaction logic for RemoteProfile.xaml
    /// </summary>
    [Export(typeof(IModule))]
    public partial class RemoteProfile : UserControl, IModule
    {
        private readonly RemoteProfileViewModel  MyRemoteProfileViewModel;

        private Point startpoint;
      

        private int _index;
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        public string MName
        {
            get { return "RemoteProfile"; }
            set { }
        }
        public UserControl UserInterface
        {
            get { return new RemoteProfile(); }
            set { }
        }
        public bool BActive
        {
            get { return ClassCommon.bRemoteProfile; }
            set { }
        }

        public RemoteProfile()
        {
            InitializeComponent();

            if (ClassCommon.bRemoteProfile) 
            {
                Index = 8;

                MyRemoteProfileViewModel = new RemoteProfileViewModel(ApplicationService.Instance.EventAggregator);
                this.DataContext = MyRemoteProfileViewModel;
                ClsSerilog.LogMessage(ClsSerilog.Info, $"Initialize Remote Profile");
            }

          
        }

        private void GridView_sidechanged(object sender, SizeChangedEventArgs e)
        {
            double iScreenWidth = e.NewSize.Width;
          //  int colwidsth;

            if (iScreenWidth < 1450)
            {
               // colwidsth = 100;
                //ListView2.ColumnWidth = 100;
                // ListView2.FontSize = 24;
                //MyRemoteProfileViewModel.DGColWidth = 100;
            }
            else
            {
              //  colwidsth = 180;
              //  //ListView2.ColumnWidth = 180;
                //ListView2.FontSize = 30;
                //MyRemoteProfileViewModel.DGColWidth = 180;
            }
            //ListView2.UpdateLayout();

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
                        MyRemoteProfileViewModel.SelectedHdrList = newlist;

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

                    MyRemoteProfileViewModel.SelectedHdrList = newlist;
                    SelectedHdrList.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in RightClick " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in RightClick -> {ex.Message}");
               
            }
        }

        private void PopUp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point relative = e.GetPosition(null);
                Point AbsolutePos = new Point(relative.X, relative.Y);
                MyPopup.HorizontalOffset += AbsolutePos.X - startpoint.X;
                MyPopup.VerticalOffset += AbsolutePos.Y - startpoint.Y;
            }
        }

        private void PopUp_MouseMove(object sender, MouseEventArgs e)
        {
            startpoint = e.GetPosition(null);
        }
    }
}
