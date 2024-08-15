using ForteArg.Services.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForteArg.Services
{
    public static class ClassCommon
    {

        public static int BaleInADrop
        {
            get => Settings.Default.iBalesInDrop;
            set
            {
                Settings.Default.iBalesInDrop = value;
                Settings.Default.Save();
            }
        }


        public static bool bDropOption
        {
            get => Settings.Default.bDropOption;
            set
            {
                Settings.Default.bDropOption = value;
                Settings.Default.Save();
            }
        }


        public static bool bDropProfile
        {
            get => Settings.Default.bDropProfile;
            set
            {
                Settings.Default.bDropProfile = value;
                Settings.Default.Save();
            }
        }
        public static bool bDropGraph
        {
            get => Settings.Default.bDropGraph;
            set
            {
                Settings.Default.bDropGraph = value;
                Settings.Default.Save();
            }
        }
        public static bool bDropPosition
        {
            get => Settings.Default.bDropPosition;
            set
            {
                Settings.Default.bDropPosition = value;
                Settings.Default.Save();
            }
        }
        public static bool bRemoteProfile
        {
            get => Settings.Default.bRemoteProfile;
            set
            {
                Settings.Default.bRemoteProfile = value;
                Settings.Default.Save();
            }
        }



        public static bool WLOptions
        {
            get => Settings.Default.bWLOption;
            set
            {
                Settings.Default.bWLOption = value;
                Settings.Default.Save();
            }

        }

        public static List<string> WetTabMonth = new List<string> 
        {
            "FValueReadingsJan",
            "FValueReadingsFeb",
            "FValueReadingsMar",
            "FValueReadingsApr",
            "FValueReadingsMay",
            "FValueReadingsJun",
            "FValueReadingsJul",
            "FValueReadingsAug",
            "FValueReadingsSep",
            "FValueReadingsOct",
            "FValueReadingsNov",
            "FValueReadingsDec"
        };

        public static List<string> RtMonth = new List<string>()
        {
            "BaleArchiveJan",
            "BaleArchiveFeb",
            "BaleArchiveMar",
            "BaleArchiveApr",
            "BaleArchiveMay",
            "BaleArchiveJun",
            "BaleArchiveJul",
            "BaleArchiveAug",
            "BaleArchiveSep",
            "BaleArchiveOct",
            "BaleArchiveNov",
            "BaleArchiveDec"
        };

        public static List<string> LotMonth = new List<string>()
        {
            "LotArchiveJan",
            "LotArchiveFeb",
            "LotArchiveMar",
            "LotArchiveApr",
            "LotArchiveMay",
            "LotArchiveJun",
            "LotArchiveJul",
            "LotArchiveAug",
            "LotArchiveSep",
            "LotArchiveOct",
            "LotArchiveNov",
            "LotArchiveDec"
        };

        /// <summary>
        /// Moisture data type = float = Single
        /// </summary>
        /// <param name="data"></param>
        /// <param name="mtype"></param>
        /// <returns></returns>
        public static string CalulateMoisture(string data, int mtype)
        {
            string Newdata = string.Empty;
            float ftMoisture = 0;

            switch (mtype)
            {
                case 0: // %MC = moisture from Sql database
                    ftMoisture = Convert.ToSingle(data);
                    break;

                case 1: // %MR = Moisture / ( 1- Moisture / 100)
                    ftMoisture = Convert.ToSingle(data) / (1 - Convert.ToSingle(data) / 100);
                    break;

                case 2: // %AD = (100 - moisture) / 0.9
                    ftMoisture = (float)((100 - Convert.ToSingle(data)) / 0.9);
                    
                    break;

                case 3: // %BD = 100 - moisture
                    ftMoisture = 100 - Convert.ToSingle(data);
                    break;
            }
            return ftMoisture.ToString("0.##");
        }



        public static string CalulateWeight(string data)
        {
            if (Settings.Default.WeightUnit == 0)
                return data;
            else
                return (Convert.ToDouble(data) * 2.20462).ToString();
        }
    }
}
