using ForteARP.Module_ProdEsitmate.ViewModels;
using ForteARP.Module_RealTime.Views;
using ForteARP.Modules;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace ForteARP.Module_ProdEsitmate.Views
{
    /// <summary>
    /// Interaction logic for ProductionEstimates.xaml
    /// </summary>
    [Export(typeof(IModule))]
    public partial class ProductionEstimates : UserControl, IModule
    {
        public static ProductionEstimates ProductionEstimatesWindows;
        readonly ProductEstViewModel ProdEstViewModel;


        private int _index;
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        public string MName
        {
            get { return "ProductionEstimates"; }
            set { }
        }
        public UserControl UserInterface
        {
            get { return new ProductionEstimates(); }
            set { }
        }
        public bool BActive
        {
            get { return MainWindow.AppWindows.bProdEstimate; }
            set { }
        }


        public ProductionEstimates()
        {
            InitializeComponent();           
            if(MainWindow.AppWindows.bProdEstimate)
            {
                Index = 20;
                ProductionEstimatesWindows = this;

                ProdEstViewModel = new ProductEstViewModel(ApplicationService.Instance.EventAggregator);
                this.DataContext = ProdEstViewModel;
            }
        }
    }
}
