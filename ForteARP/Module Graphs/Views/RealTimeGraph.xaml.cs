
using ForteArg.Services;
using ForteARP.Module_Graphs.ViewModels;
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

namespace ForteARP.Module_Graphs.Views
{
    /// <summary>
    /// Interaction logic for RealTimeGraph.xaml
    /// </summary>
    [Export(typeof(IModule))]
    public partial class RealTimeGraph : UserControl, IModule
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
            get { return new RealTimeGraph(); }
            set { }
        }
        public bool BActive
        {
            get { return MainWindow.AppWindows.bRealTimeGraph; }
            set { }
        }


        private string GetTabName()
        {
            string tabName;

            switch (Settings.Default.iLanguageIdx) //Thread.CurrentThread.CurrentCulture.ToString())
            {
                case 0: // "en-US":
                    tabName = "RTime Graph";
                    break;
                case 1: //"Sp-SP":
                    tabName = "Gráfico TiempoReal";
                    break;
                default:
                    tabName = "RTime Graph";
                    break;
            }
            return tabName;
        }

        public RealTimeGraph()
        {
            InitializeComponent();
            if (MainWindow.AppWindows.bRealTimeGraph) 
            {
                Index = 3;
                this.DataContext = new RealTimeGraphViewModel(ApplicationService.Instance.EventAggregator);
                ClsSerilog.LogMessage(ClsSerilog.Info, $"Initialize RealTimeGraph");
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
    }

    public class RadioMenuItem : MenuItem
    {
        public string GroupName { get; set; }
        protected override void OnClick()
        {
            ItemsControl ic = Parent as ItemsControl;
            if (null != ic)
            {
                var rmi = ic.Items.OfType<RadioMenuItem>().FirstOrDefault(i =>
                    i.GroupName == GroupName && i.IsChecked);
                if (null != rmi) rmi.IsChecked = false;

                IsChecked = true;
            }
            base.OnClick();
        }
    }
}
