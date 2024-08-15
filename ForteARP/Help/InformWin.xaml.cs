using ForteARP.Help;
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

namespace ForteARP
{
    /// <summary>
    /// Interaction logic for InformWin.xaml
    /// </summary>
    public partial class InformWin : Window
    {
        public InformWin()
        {
            InitializeComponent();
            Main.Content = new InfoPage();

        }
    }
}
