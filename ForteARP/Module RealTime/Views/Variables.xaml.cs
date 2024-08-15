using ForteARP.Module_RealTime.ViewModels;
using ForteARP.Modules;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;

namespace ForteARP.Module_RealTime.Views
{
    /// <summary>
    /// Interaction logic for Variables.xaml
    /// </summary>
    [Export(typeof(IModule))]
    public partial class Variables : UserControl, IModule
    {
        public static Variables VarsWindows;
        readonly VariableViewModel VarViewModel;

        private int _index;
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        public string MName
        {
            get { return "Variables"; }
            set { }
        }
        public UserControl UserInterface
        {
            get { return new Variables(); }
            set { }
        }
        public bool BActive
        {
            get { return MainWindow.AppWindows.bVariables; }
            set { }
        }

      

        public Variables()
        {
            InitializeComponent();
            
            if(MainWindow.AppWindows.bVariables)
            {
                Index = 12;
                VarsWindows = this;
                VarViewModel = new VariableViewModel(ApplicationService.Instance.EventAggregator);
                this.DataContext = VarViewModel;
            }
           
        }

        private void Window_SizeChenged(object sender, SizeChangedEventArgs e)
        {
            _ = e.NewSize.Width;
        }

        private void TextBox_SizeChange(object sender, SizeChangedEventArgs e)
        {
            double boxWidth = e.NewSize.Width;
            double xwidth = e.NewSize.Width * .25;

            //  Console.WriteLine("boxWidth = " + boxWidth + "  txtbox1  " + VarViewModel.BigNumBox2.Length.ToString() + " e.NewSize.Width * .25  " + e.NewSize.Width * .25);

            txtbox1.FontSize = xwidth;
            txtbox2.FontSize = xwidth;
            txtbox3.FontSize = xwidth;
            txtbox4.FontSize = xwidth;
            txtbox5.FontSize = xwidth;
            txtbox6.FontSize = xwidth;
            txtbox7.FontSize = xwidth;
            txtbox8.FontSize = xwidth;
            txtbox9.FontSize = xwidth;
            txtbox10.FontSize = xwidth;
            txtbox11.FontSize = xwidth;
            txtbox12.FontSize = xwidth;
        }
    }
}
