using ForteARP.Module_Histrogram.ViewModels;
using ForteARP.Modules;
using ForteARP.Properties;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ForteARP.Module_Histrogram.Views
{
    /// <summary>
    /// Interaction logic for HistroGram.xaml
    /// </summary>
    [Export(typeof(IModule))]
    public partial class HistroGram : UserControl, IModule
    {
        private readonly HistrogramViewModel HisViewModel;

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
            get { return new HistroGram(); }
            set { }
        }
        public bool BActive
        {
            get { return MainWindow.AppWindows.bHistroGram; }
            set { }
        }

        private string GetTabName()
        {
            string tabName;

            switch (Settings.Default.iLanguageIdx) //Thread.CurrentThread.CurrentCulture.ToString())
            {
                case 0: // "en-US":
                    tabName = "Histogram";
                    break;
                case 1: //"Sp-SP":
                    tabName = "Histograma";
                    break;
                default:
                    tabName = "Histogram";
                    break;
            }
            return tabName;
        }

        public HistroGram()
        {
            InitializeComponent();

            if(MainWindow.AppWindows.bHistroGram)
            {
                Index = 13;
                HisViewModel = new HistrogramViewModel();
                this.DataContext = HisViewModel;
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
}
