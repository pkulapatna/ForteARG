using ForteARP.Module_Combine.ViewModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ForteARP.Module_Combine.Views
{
    /// <summary>
    /// Interaction logic for ComboSetupView.xaml
    /// </summary>
    public partial class ComboSetupView : UserControl, IDisposable
    {
        public ComboSetupView(Prism.Events.IEventAggregator _eventAggregator)
        {
            InitializeComponent();
            this.DataContext = new ComboSetupViewModel(_eventAggregator);
        }
        public void Dispose()
        {
            // throw new NotImplementedException();
        }
    }
}
