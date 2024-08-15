using ForteArg.Services;
using ForteARP.Module_WetLayer.ViewModels;
using ForteARP.Modules;
using ForteARP.Properties;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ForteARP.Module_WetLayer.Views
{
    /// <summary>
    /// Interaction logic for WetLayer.xaml
    /// </summary>
    [Export(typeof(IModule))]
    public partial class WetLayer : UserControl, IModule
    {
        private int _index;
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        public string MName
        {
            get { return "WetLayer"; }
            set { }
        }
        public UserControl UserInterface
        {
            get { return new WetLayer(); }
            set { }
        }
        public bool BActive
        {
            get { return Settings.Default.wetLayerChecked; }
            set { }
        }

        public WetLayer()
        {
            InitializeComponent();
            if (Settings.Default.wetLayerChecked)
            {
                Index = 10;
                this.DataContext = new WetLayerViewModel(ApplicationService.Instance.EventAggregator);
                ClsSerilog.LogMessage(ClsSerilog.Info, $"Initialize WetLayers");
            }          
        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName.StartsWith("ID"))
                e.Column.Header = "Number";

            if (e.PropertyName.StartsWith("BalerID"))
                e.Column.Header = "Baler";

            if (e.PropertyName.StartsWith("Param1"))
                e.Column.Header = "MAX";

            if (e.PropertyName.StartsWith("Param2"))
                e.Column.Header = "MIN";

            if (e.PropertyName.StartsWith("Deviation"))
                e.Column.Header = "CV";

            if (e.PropertyName.StartsWith("Time1"))
                e.Column.Header = "Inp";

            if (e.PropertyName.StartsWith("Time2"))
                e.Column.Header = "Mdl";

            if (e.PropertyName.StartsWith("Time3"))
                e.Column.Header = "Out";

            if (e.PropertyName.StartsWith("Sample"))
                e.Column.Header = "Total";

            if (e.PropertyName.StartsWith("Layers"))
                e.Column.Visibility = Visibility.Hidden;

            if (e.PropertyName.StartsWith("Title"))
                e.Column.Visibility = Visibility.Hidden;
            //Title
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

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {

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
