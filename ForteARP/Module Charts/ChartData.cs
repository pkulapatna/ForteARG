using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ForteARP.Charts
{
    public class ChartData : BindableBase
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

    /// <summary>
    /// 
    /// </summary>
    public class ChartColors : BindableBase
    {

        private int _Index;
        public int Index
        {
            get { return _Index; }
            set { SetProperty(ref _Index, value); }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }

        private Brush _BrushColor;
        public Brush BrushColor
        {
            get { return _BrushColor; }
            set { SetProperty(ref _BrushColor, value); }
        }

    }
}
