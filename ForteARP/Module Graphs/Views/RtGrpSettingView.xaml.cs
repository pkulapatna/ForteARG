using ForteARP.Module_Graphs.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ForteARP.Module_Graphs.Views
{
    /// <summary>
    /// Interaction logic for RtGrpSettingView.xaml
    /// </summary>
    public partial class RtGrpSettingView : UserControl, IDisposable
    {

        public RtGrpSettingView(Prism.Events.IEventAggregator _eventAggregator)
        {
            InitializeComponent();
            this.DataContext = new RtGrpSettingViewModel(_eventAggregator);
        }




        public void Dispose()
        {
           // throw new NotImplementedException();
        }
    }
}
