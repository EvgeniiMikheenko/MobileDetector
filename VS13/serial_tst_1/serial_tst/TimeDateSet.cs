using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Modbus.Device;

namespace serial_tst
{
    public partial class TimeDateSet : Form
    {
        public TimeDateSet()
        {
            InitializeComponent();
            GetSysTime_Click(this, null);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Unicode)]
        public struct DateTimeSettings
        {
            /// <summary>
            /// Год
            /// </summary>
            public ushort Year;

            /// <summary>
            /// Месяц
            /// </summary>
            public ushort Month;

            /// <summary>
            /// День
            /// </summary>
            public ushort Day;

            /// <summary>
            /// Часы
            /// </summary>
            public ushort Hour;

            /// <summary>
            /// Минуты
            /// </summary>
            public ushort Minutes;

            /// <summary>
            /// Секунды
            /// </summary>
            public ushort Seconds;
        }



        public DateTimeSettings DateTimeSet;
        IModbusMaster m_mbMasterT;
        private void GetSysTime_Click(object sender, EventArgs e)
        {
            DateTime lockal = DateTime.Now;

            setDate.Text = lockal.Day.ToString("D2") + ":" + lockal.Month.ToString("D2") + ":" + lockal.Year.ToString("D2");

            setTime.Text = lockal.Hour.ToString("D2") + ":" + lockal.Minute.ToString("D2") + ":" + lockal.Second.ToString("D2");
        }

        private void setDate_TextChanged(object sender, EventArgs e)
        {
          string newDate = setDate.Text;

          string[] newDate_val = newDate.Split('.');

            //UInt16[] DMY = new UInt16[3];





            DateTimeSet.Day = Convert.ToUInt16(newDate_val[0]);
            DateTimeSet.Month = Convert.ToUInt16(newDate_val[1]);
            DateTimeSet.Year = Convert.ToUInt16(newDate_val[2]);

        }

        private void setTime_TextChanged(object sender, EventArgs e)
        {
            string newDate = setTime.Text;

            string[] newDate_val = newDate.Split(':');

            //UInt16[] DMY = new UInt16[3];

            DateTimeSet.Hour = Convert.ToUInt16(newDate_val[0]);
            DateTimeSet.Minutes = Convert.ToUInt16(newDate_val[1]);
            DateTimeSet.Seconds = Convert.ToUInt16(newDate_val[2]);
        }

        private void Save_Click(object sender, EventArgs e)
        {


            ushort[] tBuff = GraphForm.StructToBuff<ushort, DateTimeSettings>(DateTimeSet);

           
            ushort[] Password = new ushort[1];
            Password[0] = 0x0101;

            //if (!GraphForm.COM3.IsOpen)
            //{
            //    try
            //    {
            //        GraphForm.COM3.Open();
            //        GraphForm.m_mbMaster.WriteMultipleRegisters(0x01, 0x00, Password);
                    
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message, "Ошибка !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }

            //}

           // m_mbMasterT.WriteMultipleRegisters(0x01, 14, tBuff);
        }
    }
}
