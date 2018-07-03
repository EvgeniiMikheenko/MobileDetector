using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;





namespace LGAR
{

    public static class Mike
    {

        public enum aFlagsR
        {
            Iprit = 1, Phosphor = 2, IpritThr = 4, PhosphorThr = 8, Failure = 16, Ready = 32
        }

        public enum aFlagsW
        {
            WriteFlash = 1, Kmax = 2, Filter = 4, Lock = 8, Enum = 16, Control =  32
        }


        internal static IList<DeviceParameter> Initialize()
        {

            return new List<DeviceParameter>()
            {
                new DeviceParameter(1, "Var_res_lev", 0, 64, 0, 64),
                new DeviceParameter(2, "Shunt_time", 0, 10, 0, 5),
                new DeviceParameter(3, "Shunt_tresh", 0, 5000, 0, 5),

                new DeviceParameter(4, "+Tresh", 0, 3000, 0, 3),
                new DeviceParameter(5, "-Tresh", 0, 3000, 0, 3),
                new DeviceParameter(6, "Ref_null", 0, 3300, 0, 3.3),
                new DeviceParameter(7, "+Tresh_Danger", 0, 3000, 0, 3),
                new DeviceParameter(8, "-Tresh_Danger", 0, 3000, 0, 3),




                new DeviceParameter(13, "Filter_Level", 0, 2000, 0, 2000),
                new DeviceParameter(19, "K2max", 0, 20000, 0, 20000),      //-200+32768, 200+32768, -20d, 20
                new DeviceParameter(14, "Zone_delay", 0, 20, 0, 10),



                //new DeviceParameter(1, "Var_res_lev", 1, 64, 1, 64),
                //new DeviceParameter(2, "+Tresh", 0, 8000, 0, 8),
                //new DeviceParameter(3, "+Tresh dang.", 0, 8000, 0, 8),
                //new DeviceParameter(4, "-Tresh", 0, 8000, 0, 8),
                //new DeviceParameter(5, "-Tresh dang.", 0, 8000, 0, 8),
                //new DeviceParameter(6, "Shunt_tresh", 0, 9000, 0, 9),
                //new DeviceParameter(7, "Shunt_time", 0, 10, 0, 5),
                //new DeviceParameter(8, "-Tresh_Danger", 0, 3300, 0, 3.3),





                //new DeviceParameter(9, "Filter_Level", 1, 20, 1, 20),
                //new DeviceParameter(15, "K2max", -200+32768, 200+32768, -20d, 20),
                //new DeviceParameter(10, "Zone_delay", 0, 20, 0, 10),


            };
        }

        internal static IList<DeviceParameter> InitializeInputs()
        {
            return new List<DeviceParameter>()
            {
                //new DeviceParameter(71, "+Value", 0, 3.3d),
                //new DeviceParameter(72, "-Value", 0, 3.3d),
                //new DeviceParameter(73, "Um", 1/10d),
                //new DeviceParameter(74, "Ecm", -3276.8, 3276.7),
                //new DeviceParameter(75, "Temperature", 1/10d),
                //new DeviceParameter(21, "+Value2", 0, 3.3d),
                //new DeviceParameter(22, "-Value2", 0, 3.3d),



                new DeviceParameter(1, "Sens_volt", 1/1000d),          //
                new DeviceParameter(2, "Temperature", 1/10d),
                new DeviceParameter(3, "Humidity",1/10d),





                new DeviceParameter(4, "Ecm", -3276.8, 3276.7),
                new DeviceParameter(5, "Temperature", 1/10d),
                new DeviceParameter(6, "Freq", 1d),
                new DeviceParameter(21, "+Value2", 0, 3.3d),
                new DeviceParameter(22, "-Value2", 0, 3.3d),


            };
        }
    }
}
