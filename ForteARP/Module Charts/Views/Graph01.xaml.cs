using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using Microsoft.Win32.SafeHandles;
using System.Collections.ObjectModel;
using Prism.Mvvm;
using ForteARP.ViewModels;
using ForteARP.Properties;
using System.Data;

namespace ForteARP.Charts
{
    /// <summary>
    /// Interaction logic for Graph01.xaml
    /// </summary>
    public partial class Graph01 : Window
    {
   
        private readonly Graph01ViewModel GraphViewModel;
        private Point startpoint;

        public Graph01( string SelectedLot, DateTime datestart, DateTime dateEnd, string Lotid, string strmonth)
        {
            InitializeComponent();
            GraphViewModel = new Graph01ViewModel(SelectedLot, datestart, dateEnd, Lotid, strmonth);
            DataContext = GraphViewModel;

            
        }

        private void BtnShowData_Click(object sender, RoutedEventArgs e)
        {
            MyPopup.IsOpen = true;
            Double DMvalue;
            Double DWvalue;
            double Coef;

            if (Settings.Default.WeightUnit == 0)
                Coef = 1; //Kg
            else
                Coef = 2.20462; //Lb.

            if (RealTimeGridView2 != null)
            {
                foreach (DataRow Item in GraphViewModel.LotDatatable.Rows)
                {
                    DMvalue = GetMoisture(Item.Field<Single>("Moisture"));  
                    Item["Moisture"] = DMvalue;

                    DWvalue = Item.Field<Single>("Weight");
                    Item["Weight"] = DWvalue * Coef; 

                }
                RealTimeGridView2.DataContext = GraphViewModel.LotDatatable;
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

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName.StartsWith("index")) e.Column.Visibility = Visibility.Hidden;
            
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
                    e.Column.Header = "Weight KG.";
                else
                    e.Column.Header = "Weight LB.";
            }

            if ((e.PropertyType == typeof(System.Single)) || (e.PropertyType == typeof(System.Double)))
            {
                e.Column.ClipboardContentBinding.StringFormat = "{0:0.##}";
                e.Column.Width = 80;
            }


        }

        private void CSVAll_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Hide_Click(object sender, RoutedEventArgs e)
        {
            MyPopup.IsOpen = false;
        }

        private void CSV_Click(object sender, RoutedEventArgs e)
        {

        }

        private Double GetMoisture(double dVal)
        {
            switch (Settings.Default.MoistureUnit)
            {
                case 0: // %MC == moisture from Sql database

                    break;

                case 1: // %MR  = Moisture / ( 1- Moisture / 100)
                    dVal /= (1 - dVal / 100);
                    break;

                case 2: // %AD = (100 - moisture) / 0.9
                    dVal = (100 - dVal) / 0.9;
                    break;

                case 3: // %BD  = 100 - moisture
                    dVal = 100 - dVal;
                    break;
            }
            return dVal;
        }
    }
    
}
