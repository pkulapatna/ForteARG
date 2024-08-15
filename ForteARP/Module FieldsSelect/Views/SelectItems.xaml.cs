

using ForteArg.Services;
using ForteARP.Module_FieldsSelect.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace ForteARP.Module_FieldsSelect.Views
{
    /// <summary>
    /// Interaction logic for SelectItems.xaml
    /// </summary>
    public partial class SelectItems : Window
    {
        private readonly SelectItemsViewModel SelItemViewModel;
        protected readonly Prism.Events.IEventAggregator _eventAggregator;


        public SelectItems()
        {
            InitializeComponent();
            SelItemViewModel = new SelectItemsViewModel();
            this.DataContext = SelItemViewModel;

            if (SelItemViewModel.CloseAction == null)
                SelItemViewModel.CloseAction = new Action(this.Close);
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

                    SelItemViewModel.SelectedHdrList = newlist;
                    SelectedHdrList.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in RightClick " + ex.Message);
                ClsSerilog.LogMessage(ClsSerilog.Error, $"EROR in RightClick -> {ex.Message}");
               
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

                    if ((NewIndex > -1) || (NewIndex >= SelectedHdrList.Items.Count))
                    {
                        object selected = SelectedHdrList.SelectedItem;

                        // Removing removable element ItemsControl.ItemsSource
                        newlist.Remove(selected.ToString());
                        // Insert it in new position
                        newlist.Insert(NewIndex, selected.ToString());
                        // Restore selection
                        SelItemViewModel.SelectedHdrList = newlist;

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

    }
}
