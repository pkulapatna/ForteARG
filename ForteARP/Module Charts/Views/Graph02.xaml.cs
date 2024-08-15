using System;
using System.Collections.Generic;
using System.Windows;
using ForteARP.ViewModels;

namespace ForteARP.Charts
{
    /// <summary>
    /// Interaction logic for Graph02.xaml
    /// </summary>
    public partial class Graph02 : Window
    {
        private readonly Graph02ViewModel Graph02ViewModel;
        public Graph02(List<Tuple<long, string, double>> wetLayerDataList)
        {
            InitializeComponent();
            Graph02ViewModel = new Graph02ViewModel(wetLayerDataList);
            DataContext = Graph02ViewModel;
        }      
    }
}
