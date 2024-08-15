using ForteArg.Services;
using ForteARP.Module_DropOption.ViewModels;
using ForteARP.Modules;
using ForteARP.Properties;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ForteARP.Module_DropOption.Views
{
    /// <summary>
    /// Interaction logic for DropPosition.xaml
    /// </summary>
    [Export(typeof(IModule))]
    public partial class DropPosition : UserControl, IModule
    {
        private readonly DropPositionViewModel DpViewModel;

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
            get { return new DropPosition(); }
            set { }
        }
        public bool BActive
        {
            get { return ClassCommon.bDropPosition; }
            set { }
        }

        private string GetTabName()
        {
            string tabName;

            switch (Settings.Default.iLanguageIdx) //Thread.CurrentThread.CurrentCulture.ToString())
            {
                case 0: // "en-US":
                    tabName = "Drop_Position";
                    break;
                case 1: //"Sp-SP":
                    tabName = "Posicion por Bajada";
                    break;
                default:
                    tabName = "Drop_Position";
                    break;
            }
            return tabName;
        }

        public DropPosition()
        {
            InitializeComponent();
            if (ClassCommon.bDropPosition)
            {
                Index = 8;
                DpViewModel = new DropPositionViewModel(ApplicationService.Instance.EventAggregator);
                this.DataContext = DpViewModel;
                ClsSerilog.LogMessage(ClsSerilog.Info, $"Initialize DropPosition");
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
    }
}
