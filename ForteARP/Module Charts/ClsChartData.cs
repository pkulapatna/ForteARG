using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ForteARP.Charts
{
    public class ClsChartData : BindableBase
    {
        private int _Index;
        public int Index
        {
            get { return _Index; }
            set { SetProperty(ref _Index, value); }
        }

        private double _value;
        public double Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        private Brush _color;
        public Brush ChartColor
        {
            get { return _color; }
            set { SetProperty(ref _color, value); }
        }

    }

    public class ClsChartData2 : BindableBase
    {
        private string _Index;
        public string Index
        {
            get { return _Index; }
            set { SetProperty(ref _Index, value); }
        }

        private double _value;
        public double Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        private Brush _color;
        public Brush ChartColor
        {
            get { return _color; }
            set { SetProperty(ref _color, value); }
        }

    }
}
