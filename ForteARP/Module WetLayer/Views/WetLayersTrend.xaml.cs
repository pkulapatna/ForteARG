using ForteArg.Services;
using ForteARP.Module_WetLayer.ViewModels;
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

namespace ForteARP.Module_WetLayer.Views
{
    /// <summary>
    /// Interaction logic for WetLayersTrend.xaml
    /// </summary>
    [Export(typeof(IModule))]
    public partial class WetLayersTrend : UserControl, IModule
    {
        private int _index;
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        public string MName
        {
            get { return "Layers Trend"; }
            set { }
        }
        public UserControl UserInterface
        {
            get { return new WetLayersTrend(); }
            set { }
        }
        public bool BActive
        {
            get { return Settings.Default.bWlShowTrend & ClassCommon.WLOptions; }
            set { }
        }


        public WetLayersTrend()
        {
            InitializeComponent();

            if (BActive) 
            {
                Index = 11;
                this.DataContext = new WetLayersTrendViewModel(ApplicationService.Instance.EventAggregator);
                ClsSerilog.LogMessage(ClsSerilog.Info, $"Initialize WetLayers Trend!");
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

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDlg = new PrintDialog();

            if (printDlg.ShowDialog() == true)
            {
                printDlg.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;
                Size pageSize = new Size(printDlg.PrintableAreaWidth - 30, printDlg.PrintableAreaHeight - 30);
                _PrintGrid.Measure(pageSize);
                _PrintGrid.Arrange(new Rect(0, 0, pageSize.Width, pageSize.Height));

                printDlg.PrintVisual(_PrintGrid, "My Canvas");
            }

            printDlg = null;
        }
    }
}
