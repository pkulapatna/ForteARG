using ForteArg.Services.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForteArg.Services
{
    public class ClsParams
    {
        public static ClsParams AppParams;

        public struct MType
        {
            public string Name;
            public string Unit;
            public string Sname;
            public int iWidth;
        }
        public static MType[] MoistureTypeList;

        public struct WType
        {
            public string Name;
            public string Unit;
        }
        public static WType[] WeightUnitList;


        public ClsParams()
        {
            AppParams = this;

            switch (Settings.Default.iLanguageIdx) //Thread.CurrentThread.CurrentCulture.ToString())
            {
                case 0: // "en-US":
                    MoistureTypeList = new MType[4];
                    MoistureTypeList[0].Name = "Moisture Content %";  // Moisture Content % = Moisture
                    MoistureTypeList[0].Unit = "%MC";
                    MoistureTypeList[0].Sname = "Moisture";
                    MoistureTypeList[0].iWidth = 120;
                    //
                    MoistureTypeList[1].Name = "Moisture Regain %";  // Moisture Regain % = Moisture / ( 1- Moisture / 100)
                    MoistureTypeList[1].Unit = "%MR";
                    MoistureTypeList[1].Sname = "Regain%";
                    MoistureTypeList[1].iWidth = 120;
                    //
                    MoistureTypeList[2].Name = "Air Dry %";  // Air Dry % =  (100 - Moisture) / .9
                    MoistureTypeList[2].Unit = "%AD";
                    MoistureTypeList[2].Sname = "AirDry%";
                    MoistureTypeList[2].iWidth = 100;
                    //
                    MoistureTypeList[3].Name = "Bone Dry %";  // Bone Dry % = 100 - Moisture
                    MoistureTypeList[3].Unit = "%BD";
                    MoistureTypeList[3].Sname = "BoneDry%";
                    MoistureTypeList[3].iWidth = 100;

                    WeightUnitList = new WType[2];
                    WeightUnitList[0].Name = "Kilograms";
                    WeightUnitList[0].Unit = "Kg."; // "Kilograms"
                                                    //
                    WeightUnitList[1].Name = "Pounds";
                    WeightUnitList[1].Unit = "Lb."; // "Pounds"
                    break;
                case 1: //"Sp-SP":
                    MoistureTypeList = new MType[4];
                    MoistureTypeList[0].Name = "Contenido de Humedad %";  // Moisture Content % = Moisture
                    MoistureTypeList[0].Unit = "% Contenido de Humedad";
                    MoistureTypeList[0].Sname = "Moisture";
                    MoistureTypeList[0].iWidth = 120;
                    //
                    MoistureTypeList[1].Name = "Recuperacion de humedad %";  // Moisture Regain % = Moisture / ( 1- Moisture / 100)
                    MoistureTypeList[1].Unit = "% Recuperacion de humedad";
                    MoistureTypeList[1].Sname = "Regain%";
                    MoistureTypeList[1].iWidth = 120;
                    //
                    MoistureTypeList[2].Name = "Seco %";  // Air Dry % =  (100 - Moisture) / .9
                    MoistureTypeList[2].Unit = "% Seco";
                    MoistureTypeList[2].Sname = "AirDry%";
                    MoistureTypeList[2].iWidth = 100;
                    //
                    MoistureTypeList[3].Name = "Bone Dry %";  // Bone Dry % = 100 - Moisture
                    MoistureTypeList[3].Unit = "%BD";
                    MoistureTypeList[3].Sname = "BoneDry";
                    MoistureTypeList[3].iWidth = 100;

                    WeightUnitList = new WType[2];
                    WeightUnitList[0].Name = "Kilograms";
                    WeightUnitList[0].Unit = "Kg."; // "Kilograms"
                                                    //
                    WeightUnitList[1].Name = "Pounds";
                    WeightUnitList[1].Unit = "Lb."; // "Pounds"
                    break;
                default:
                    MoistureTypeList = new MType[4];
                    MoistureTypeList[0].Name = "Moisture Content %";  // Moisture Content % = Moisture
                    MoistureTypeList[0].Unit = "%MC";
                    MoistureTypeList[0].Sname = "Moisture";
                    MoistureTypeList[0].iWidth = 120;
                    //
                    MoistureTypeList[1].Name = "Moisture Regain %";  // Moisture Regain % = Moisture / ( 1- Moisture / 100)
                    MoistureTypeList[1].Unit = "%MR";
                    MoistureTypeList[1].Sname = "Regain%";
                    MoistureTypeList[1].iWidth = 120;
                    //
                    MoistureTypeList[2].Name = "Air Dry %";  // Air Dry % =  (100 - Moisture) / .9
                    MoistureTypeList[2].Unit = "%AD";
                    MoistureTypeList[2].Sname = "AirDry%";
                    MoistureTypeList[2].iWidth = 100;
                    //
                    MoistureTypeList[3].Name = "Bone Dry %";  // Bone Dry % = 100 - Moisture
                    MoistureTypeList[3].Unit = "%BD";
                    MoistureTypeList[3].Sname = "BoneDry%";
                    MoistureTypeList[3].iWidth = 100;

                    WeightUnitList = new WType[2];
                    WeightUnitList[0].Name = "Kilograms";
                    WeightUnitList[0].Unit = "Kg."; // "Kilograms"
                                                    //
                    WeightUnitList[1].Name = "Pounds";
                    WeightUnitList[1].Unit = "Lb."; // "Pounds"
                    break;
            }
            ClsSerilog.LogMessage(ClsSerilog.Info, $"Config Moisture Parameters ......................");
        }
    }
}
