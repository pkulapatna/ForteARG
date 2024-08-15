using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Input;
using ForteARP.Properties;
using ForteARP.ViewModels;

namespace ForteARP.Modules
{
    /// <summary>
    /// Interaction logic for SetupModule.xaml
    /// </summary>
    [Export(typeof(IModule))]
    public partial class SetupModule : UserControl, IModule
    {

        public static SetupModule SetupModulesWindows;
        private readonly SetupViewModel SUViewModel;

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
            get { return new SetupModule(); }
            set { }
        }
        public bool BActive
        {
            get { return true; }
            set { }
        }


        private string GetTabName()
        {
            string tabName; // = string.Empty;

            switch (Settings.Default.iLanguageIdx) //Thread.CurrentThread.CurrentCulture.ToString())
            {
                case 0: // "en-US":
                    tabName = "Setup";
                    break;
                case 1: //"Sp-SP":
                    tabName = "Ajustes";
                    break;
                default:
                    tabName = "Setup";
                    break;
            }
            return tabName; 
        }

        public SetupModule()
        {
            InitializeComponent();
            Index = 0;
            SUViewModel = new SetupViewModel();
            DataContext = SUViewModel;
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
