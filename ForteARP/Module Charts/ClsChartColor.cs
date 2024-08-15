using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ForteARP.Charts
{
    public class ClsChartColor
    {

        public  List<Brush> ChartColorList;


        //initialization
        public ClsChartColor()
        {
            SetUpGraphsColors();
        }


        public enum Idx : int
        {
            ForestGreen = 0,
            LightSalmon = 1,
            CadetBlue = 2,
            FFF57D7D = 3,
            FFF7F773 = 4,
            FFCD7ACD = 5,
            White = 6,
            Gold = 7,
            Gray = 8,
            Aquamarine= 9,
            Salmon = 10,
            SeaGreen = 11,
            Coral = 12,
            Red = 13,
            Yellow = 14,
            Blue = 15,
            Green = 16,
            Brown = 17,
            Pink = 18,
            Orange = 19,
            Olive = 20,
            Beige = 21,
            SlateGray = 22,
            SpringGreen = 23,
            LightGreen = 24,
            LightSteelBlue = 25,
            Azure = 26,
            Aqua = 27,
            BlanchedAlmond = 28
        };



        private void SetUpGraphsColors()
        {
            BrushConverter bc = new BrushConverter();

            ChartColorList = new List<Brush>
            {
                Brushes.ForestGreen,
                Brushes.LightSalmon,
                Brushes.CadetBlue,

                (Brush)bc.ConvertFrom("#FFF57D7D"),
                (Brush)bc.ConvertFrom("#FFF7F773"),
                (Brush)bc.ConvertFrom("#FFCD7ACD"),

                Brushes.White,
                Brushes.Gold,
                Brushes.Gray,
                Brushes.Aquamarine,
                Brushes.Salmon,
                Brushes.SeaGreen,
                Brushes.Coral,

                Brushes.Red,
                Brushes.Yellow,
                Brushes.Blue,
                Brushes.Green,
                Brushes.Brown,

                Brushes.Pink,
                Brushes.Orange,
                Brushes.Olive,
                Brushes.Beige,
                Brushes.SlateGray,

                Brushes.SpringGreen,
                Brushes.LightGreen,
                Brushes.LightSteelBlue,
                Brushes.Azure,
                Brushes.Aqua,
                Brushes.BlanchedAlmond
            };
            //Max 27

        }
    }
}
