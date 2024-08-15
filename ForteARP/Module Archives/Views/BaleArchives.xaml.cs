using ForteArg.Services;
using ForteARP.Archives_Module.ViewModels;
using ForteARP.Modules;
using ForteARP.Properties;
using System;
using System.Collections.Generic;
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

namespace ForteARP.Module_Archives.Views
{
    /// <summary>
    /// Interaction logic for BaleArchives.xaml
    /// </summary>
    [Export(typeof(IModule))]
    public partial class BaleArchives : UserControl, IModule
    {

        private readonly BaleArchivesViewModel BaleViewModel;

        private double wdCoef = 0.0;

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
            get { return new BaleArchives(); }
            set { }
        }
        public bool BActive
        {
            get { return MainWindow.AppWindows.bBaleArchive; }
            set { }
        }

        private string GetTabName()
        {
            string tabName;

            switch (Settings.Default.iLanguageIdx) //Thread.CurrentThread.CurrentCulture.ToString())
            {
                case 0: // "en-US":
                    tabName = "Archives";
                    break;
                case 1: //"Sp-SP":
                    tabName = "Archivos";
                    break;
                default:
                    tabName = "Archives";
                    break;
            }
            return tabName;
        }


        public BaleArchives()
        {
            InitializeComponent();
            if (MainWindow.AppWindows.bBaleArchive)
            {
                Index = 1;
                BaleViewModel = new BaleArchivesViewModel();
                this.DataContext = BaleViewModel;
                ClsSerilog.LogMessage(ClsSerilog.Info, $"Initialize BaleArchives");
            }
           
        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

            GridBaleArchive.Columns[0].Visibility = Visibility.Collapsed;

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

            if (e.PropertyName.StartsWith("Finish"))
                e.Column.Header = "Viscosity";
            if (e.PropertyName.StartsWith("Package"))
                e.Column.Header = "Wrap";
            if (e.PropertyName.StartsWith("Brightness"))
                e.Column.Header = "Bright";
            if (e.PropertyName.StartsWith("ForteStatus"))
                e.Column.Header = "FtMsg";
            if (e.PropertyName.StartsWith("MoistureStatus"))
                e.Column.Header = "McMsg";
            if (e.PropertyName.StartsWith("TareWeight"))
                e.Column.Header = "Tare kg";
            if (e.PropertyName.StartsWith("FC_LotIdentString"))
                e.Column.Header = "Batch ID";
            if (e.PropertyName.StartsWith("LotBaleNumber"))
                e.Column.Header = "Bale #";
            if (e.PropertyName.StartsWith("Position"))
                e.Column.Header = "Pos";
            if (e.PropertyName.StartsWith("SpareSngFld3"))
                e.Column.Header = "CV %";

            // if (e.PropertyName.StartsWith("TimeComplete"))
            //     e.Column.Header = "Date";
            // if (e.PropertyName.StartsWith("GradeName"))
            //     e.Column.Header = "Grade";
            //  if (e.PropertyName.StartsWith("LotNumber"))
            //      e.Column.Header = "";

            //FC_LotIdentString

            if ((e.PropertyType == typeof(System.Single)) || (e.PropertyType == typeof(System.Double)))
            {
                e.Column.ClipboardContentBinding.StringFormat = "{0:0.##}";
                e.Column.Width = e.Column.Header.ToString().Length + wdCoef;
            }
            else if ((e.PropertyType == typeof(System.Single)) || (e.PropertyType == typeof(System.DateTime)))
            {
                e.Column.ClipboardContentBinding.StringFormat = "MM-dd-yyyy HH:mm";
                e.Column.Width = e.Column.Header.ToString().Length + wdCoef * 1.7;
            }
            else
                e.Column.Width = e.Column.Header.ToString().Length + wdCoef;// * 10;
        }

        private void GridView_sidechanged(object sender, SizeChangedEventArgs e)
        {
            double dColHdrHeight = e.NewSize.Width * 0.03;
            double iGVFontsize = e.NewSize.Width * 0.01;
            double iGVRowHeight = e.NewSize.Width * 0.020;
            wdCoef = e.NewSize.Width * 0.08;

            GridBaleArchive.ColumnHeaderHeight = dColHdrHeight;
            GridBaleArchive.FontSize = iGVFontsize;
            GridBaleArchive.RowHeight = iGVRowHeight;
            GridBaleArchive.UpdateLayout();
        }
    }
}
