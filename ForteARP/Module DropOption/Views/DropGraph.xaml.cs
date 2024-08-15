using ForteArg.Services;
using ForteARP.Module_DropOption.ViewModels;
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

namespace ForteARP.Module_DropOption.Views
{
    /// <summary>
    /// Interaction logic for DropGraph.xaml
    /// </summary>
    [Export(typeof(IModule))]
    public partial class DropGraph : UserControl, IModule
    {

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
            get { return new DropGraph(); }
            set { }
        }
        public bool BActive
        {
            get { return ClassCommon.bDropGraph; }
            //get { return false; }
            set { }
        }

        private string GetTabName()
        {
            string tabName;

            switch (Settings.Default.iLanguageIdx) //Thread.CurrentThread.CurrentCulture.ToString())
            {
                case 0: // "en-US":
                    tabName = "Drop_Graph";
                    break;
                case 1: //"Sp-SP":
                    tabName = "Gráfico Bajada";
                    break;
                default:
                    tabName = "Drop_Graph";
                    break;
            }
            return tabName;
        }

        public DropGraph()
        {
            InitializeComponent();
            if (ClassCommon.bDropGraph)
            {
                Index = 7;
                this.DataContext = new DropGraphViewModel(ApplicationService.Instance.EventAggregator);
                ClsSerilog.LogMessage(ClsSerilog.Info, $"Initialize DropGraph");
            }
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
    }
}
