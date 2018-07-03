using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;


namespace serial_tst
{
    class SIP
    {
        const int RegistersCountMax = 1281;

        internal ushort[] Registers = new ushort[RegistersCountMax];



        public void FillRegVal() {

            



        }

       // public int readdatanum = 0;

       // public double dataarray;


        public enum SipRegisters : int
        {
            [Description("Флаги")]
            Flags = 0,

            Temp1,
            Temp2,
            Heater_Power,
            Temp_Case,
            Athm_Pressure,
            Valve_Level,
            Pump_Total_Flow,
            HV_Value,
            Pump_Time_Low,
            Pump_Time_High,
            Humidity,
            Flow_Total,
            Flow_In,
            Device_Mode,
            Substance,
            FirmWare_Version,
            Data1,
           // Data1 = 257,
        }


        //public ushort GetVal(SipRegisters name)
        //{
        ////    GetVal<float>(SipRegisters.Device_Mode);

        //    return Registers[(int)name];
        //}


        //int i=0;
        public double GetVal(SipRegisters name)
        {

            int i=0;
            // object result = default(T);
            //ushort result;
            switch (name) 
            {
                case SipRegisters.Flags:

                   // result = Registers[(int)name];
                    
                    break;
                case SipRegisters.Temp1:

                    i = 1;

                    //Registers[(int)SipRegisters.Temp1] /= 10;
                    break;
                case SipRegisters.Temp2:

                    i = 1;

                    //Registers[(int)SipRegisters.Temp2] /= 10;
                    break;
                case SipRegisters.Heater_Power:
                    break;
                case SipRegisters.Temp_Case:
                    i = 1;
                    break;
                case SipRegisters.Athm_Pressure:
                    i = 1;
                    break;
                case SipRegisters.Valve_Level:
                    break;
                case SipRegisters.Pump_Total_Flow:
                    break;
                case SipRegisters.HV_Value:

                    i = 2;


                    //Registers[(int)SipRegisters.HV_Value] /= 100;
                    break;
                case SipRegisters.Pump_Time_Low:
                    break;
                case SipRegisters.Pump_Time_High:
                    break;
                case SipRegisters.Humidity:
                    i = 1;
                    break;
                case SipRegisters.Flow_Total:
                    break;
                case SipRegisters.Flow_In:
                    break;
                case SipRegisters.Device_Mode:
                    break;
                case SipRegisters.Substance:
                    break;
                case SipRegisters.FirmWare_Version:
                    break;
                default:
                    break;
                case SipRegisters.Data1:

                   // return 0;
                                      
                    break;
            }




            //return Registers[(int)name]/(Math.Pow(10, i));
            return 0;
        }





    }
}
