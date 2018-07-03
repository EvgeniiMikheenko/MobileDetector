using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Modbus.Device;
using System.Threading;


namespace GSAD_net_settings
{

    public partial class Form1 : Form
    {
        bool IsConnected;
        ushort[] rBuf;
        ushort[] tBuf = new ushort[5];
        ushort address = 0;

        public struct Message
        {
            public ushort baudrate; 
            public ushort databits;
            public ushort parity;
            public ushort stopbit;
            public ushort dev_address;
        }
        




        Message mb_packet = new Message();
        
            
        public Form1()
        {
            InitializeComponent();
            Fill_Com_Ports();

            Baudrate.SelectedIndex = 17;
            Databits.SelectedIndex = 0;
            Parity.SelectedIndex = 0;
            Stopbit.SelectedIndex = 0;
            module_addr1.Text = "1";
            
                

            m_mbMaster = ModbusSerialMaster.CreateRtu(COM1);
            m_mbMaster.Transport.ReadTimeout = 100;
            m_mbMaster.Transport.Retries = 5;

            
            baudrate_label.BackColor = Color.White;
            baudrate_label.Text = "Set";
            databits_label.BackColor = Color.White;
            databits_label.Text = "Set";
            parity_label.BackColor = Color.White;
            parity_label.Text = "Set";
            stopbit_label.BackColor = Color.White;
            stopbit_label.Text = "Set";

            addr_label.BackColor = Color.White;
            addr_label.Text = "Set";




        }




        string[] av_port;

        IModbusMaster m_mbMaster;
        readonly BackgroundWorker m_bgwModbusUpdater;
      
        private void Fill_Com_Ports()
        {

            av_port = System.IO.Ports.SerialPort.GetPortNames();


            this.COM_sw.AutoCompleteCustomSource.AddRange(av_port);

            COM_sw.Items.Clear();
            for (int i = 0; i < av_port.Length; i++)
            {
                COM_sw.Items.Add(av_port[i]);
            }
            if (av_port != null)
                COM_sw.SelectedIndex = 0;



            try
            {

                this.COM1.PortName = av_port[COM_sw.SelectedIndex];

            }
            catch( Exception ex)
            {
                string msg = ex.Message;
                //this.COM1.PortName = av_port[0];

            }


        }

        enum WorkState
        {
            Idle,
            Update
        }

        public int sleeptime = 100;
        WorkState m_state = WorkState.Idle;

        ushort[] rdBuf;
        ushort[] rxData;

       
        object m_syncObject = new object();
        const int RegistersCountMax = 1281;
        const int IOCountMax = 125;

        byte m_slaveAddr = 0x00;
        ushort m_startAddr = 0;
        ushort m_count = 125;
        string m_lastError;
       


       

        int[] br_enum = new int[] { 50, 75, 100, 110, 150, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 28800, 38400, 56000, 57600, 115200 };
        int[] dbits_enum = new int[] { 8, 9 };
        int[] par_enum = new int[] { 0, 1, 2 };
        int[] sbit_enum = new int[] { 1, 2, 3 };


        byte start_reg_addr = 20;
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void save_btn_Click(object sender, EventArgs e)
        {
            if (!COM1.IsOpen)
                COM1.Open();

            
            //int addr = int.Parse(module_addr1.Text);
            //bool addr_check = false;

            //if (addr > 255 || addr < 0)
            //{
            //    status_line.Text = "Задан неверный адрес устройства";
            //    addr_check = false;
            //}
            //else
            //    addr_check = true;


            //if( addr_check)
            //{
            //    tBuf[0] = (ushort)br_enum[Baudrate.SelectedIndex];
            //    tBuf[1] = (ushort)dbits_enum[Databits.SelectedIndex];
            //    tBuf[2] = (ushort)sbit_enum[Stopbit.SelectedIndex];
            //    tBuf[3] = (ushort)par_enum[Parity.SelectedIndex];
            //    tBuf[4] = (ushort)addr;

                
            //    try
            //    {


            //        m_mbMaster.WriteMultipleRegisters(0, 0x64, tBuf);


                    
            //    }
            //    catch (Exception ex)
            //    {
            //        this.Text = (string.IsNullOrEmpty(ex.Message)) ? "Form1" : ("Form1... Error: " + ex.Message);
            //    }

            //    Connect_check();
            if( IsConnected )
                try
                {

                    ushort[] flag_save = new ushort[1];
                    flag_save[0] = 0x01; 
                    m_mbMaster.WriteMultipleRegisters((byte)0, 0x00, flag_save);



                }
                catch (Exception ex)
                {
                    this.Text = (string.IsNullOrEmpty(ex.Message)) ? "Form1" : ("Form1... Error: " + ex.Message);
                }

            //}

            
        }



        private ushort[] Get_tBuf_Val()
        {

            ushort[] Buf = new ushort[5];

            Buf[0] = (ushort) br_enum[Baudrate.SelectedIndex];
            Buf[1] = (ushort) dbits_enum[Databits.SelectedIndex];
            Buf[2] = (ushort) sbit_enum[Stopbit.SelectedIndex];
            Buf[3] = (ushort) par_enum[Parity.SelectedIndex];
            Buf[4] = ushort.Parse(module_addr1.Text);



            return  Buf;
        }

        private void Connect_check()
        {
            COM1.BaudRate = br_enum[Baudrate.SelectedIndex];
            switch (Databits.SelectedIndex)
            {
                case 0: COM1.DataBits = 8; break;
                case 1: COM1.DataBits = 9; break;

            }


            switch (Parity.SelectedIndex)
            {
                case 0: COM1.Parity = System.IO.Ports.Parity.None; break;
                case 1: COM1.Parity = System.IO.Ports.Parity.Odd; break;
                case 2: COM1.Parity = System.IO.Ports.Parity.Even; break;
                default: COM1.Parity = System.IO.Ports.Parity.None; break;
            }



            switch (Stopbit.SelectedIndex)
            {

                case 0: COM1.StopBits = System.IO.Ports.StopBits.One; break;
                case 1: COM1.StopBits = System.IO.Ports.StopBits.Two; break;
                case 2: COM1.StopBits = System.IO.Ports.StopBits.OnePointFive; break;
                default: COM1.StopBits = System.IO.Ports.StopBits.None; break;

            }

            status_line.Text = null;
            checkBox1.CheckState = CheckState.Unchecked;

            try
            {

                if (!COM1.IsOpen)
                {
                    COM1.Open();
                }

                //Thread read = 
                //m_mbMaster = ModbusSerialMaster.CreateRtu(COM1);
                //m_mbMaster.Transport.ReadTimeout = 1000;
                //m_state = WorkState.Update;
                ushort[] tBuf = m_mbMaster.ReadHoldingRegisters(0, 90, 10);


                if ((tBuf[0] == 0x0102) && (tBuf[9] == 0x01))
                {
                    status_line.Text = " Подключен ";
                    IsConnected = true;
                    checkBox1.CheckState = CheckState.Checked;

                    baudrate_label.BackColor = Color.Green;
                    baudrate_label.Text = "Set";
                    databits_label.BackColor = Color.Green;
                    databits_label.Text = "Set";
                    parity_label.BackColor = Color.Green;
                    parity_label.Text = "Set";
                    stopbit_label.BackColor = Color.Green;
                    stopbit_label.Text = "Set";

                    addr_label.BackColor = Color.Green;
                    addr_label.Text = "Set";



                }



            }
            catch (Exception ex)
            {
                status_line.Text = " Ошибка подключения";
                checkBox1.CheckState = CheckState.Unchecked;
                MessageBox.Show(ex.Message, "Ошибка !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



            //if(!COM1.IsOpen)
            //{
            //    COM1.Open();
            //    m_mbMaster = ModbusSerialMaster.CreateRtu(COM1);
            //}
            //   rxBuf = m_mbMaster.ReadHoldingRegisters(0, 20, 1); 
        }
        private void connect_btn_Click(object sender, EventArgs e)
        {
            Connect_check();


            if(IsConnected )
            {
                try
                {

                    ushort[] addr = m_mbMaster.ReadHoldingRegisters(0x00, 0x68, 1);

                    if (addr != null)
                    {
                        module_addr1.Text = addr[0].ToString();
                        addr_label.BackColor = Color.Green;
                        addr_label.Text = "Set";
                    }

                }
                catch (Exception ex)
                {
                    this.Text = (string.IsNullOrEmpty(ex.Message)) ? "Form1" : ("Form1... Error: " + ex.Message);
                }




            }

        }
        ushort[] rxBuf;

        private void Search_Click(object sender, EventArgs e)
        {
            if (!COM1.IsOpen)
            {
                COM1.Open();
            }

            Baudrate_Search();
            if (!mod_faund)
            {   Parity_Search(); }
                
            if (!mod_faund)
            {   Stop_Bit_Search(); }
                


            if ( !mod_faund )                                                           // ваще не нашли
            {

                  status_line.Text = " Не найден";

            }
            else
            {
                baudrate_label.BackColor = Color.Green;
                baudrate_label.Text = "Set";
                databits_label.BackColor = Color.Green;
                databits_label.Text = "Set";
                parity_label.BackColor = Color.Green;
                parity_label.Text = "Set";
                stopbit_label.BackColor = Color.Green;
                stopbit_label.Text = "Set";

                addr_label.BackColor = Color.Green;
                addr_label.Text = "Set";

                ushort[] Addr = null;
                try
                {
                    Addr = m_mbMaster.ReadHoldingRegisters(0, 0x68, 1);         // читаем адрес и обновляем
                }
                catch( Exception ex)
                {
                    status_line.Text = " Ошибка адреса";            
                }


                if (Addr != null)
                {
                    mb_packet.dev_address = (ushort)Addr[0];
                    module_addr1.Text = Addr[0].ToString();

                    addr_label.BackColor = Color.Green;
                    addr_label.Text = "Set";
                }
                else
                    module_addr1.Text = "0";

            }

        }

        bool mod_faund = false;
        bool br_faund = false;
        private void Baudrate_Search()
        {
            byte br_lng = (byte)(br_enum.Length - 1);
            byte br_tmp = 5;
            


            COM1.BaudRate = br_enum[0];

            ushort[] rBuf = null;
            while (rBuf == null && br_lng >= br_tmp)                                                                          // перебор по br
            {
                COM1.BaudRate = br_enum[br_tmp];
                status_line.Text = " Поиск ";
                try
                {
                    rBuf = m_mbMaster.ReadHoldingRegisters((byte)address, 90, 10);
                }
                catch
                {
                    ++br_tmp;
                }
                if (rBuf != null)
                {
                    if ((rBuf[0] == 0x0102) && (rBuf[9] == 0x01))
                    {
                        status_line.Text = " Подключен ";

                        baudrate_label.BackColor = Color.Green;
                        baudrate_label.Text = "Set";
                        databits_label.BackColor = Color.Green;
                        databits_label.Text = "Set";
                        parity_label.BackColor = Color.Green;
                        parity_label.Text = "Set";
                        stopbit_label.BackColor = Color.Green;
                        stopbit_label.Text = "Set";

                        addr_label.BackColor = Color.Green;
                        addr_label.Text = "Set";



                        Baudrate.SelectedIndex = br_tmp;
                        checkBox1.CheckState = CheckState.Checked;
                        br_faund = true;
                        mod_faund = true;
                    }
                }
            }


        }

        bool par_faund = false;
        private void Parity_Search()
        {


            byte par_lng = (byte)(par_enum.Length - 1);
            byte par_ind = 0;
            while( !mod_faund && par_lng >= par_ind)
            {
                if (!mod_faund)
                {
                    
                    switch (par_ind)
                    {
                        case 0: COM1.Parity = System.IO.Ports.Parity.None; break;
                        case 1: COM1.Parity = System.IO.Ports.Parity.Odd; break;
                        case 2: COM1.Parity = System.IO.Ports.Parity.Even; break;
                     }
                    par_ind++ ;
                }
                Parity.SelectedIndex = --par_ind;
                Baudrate_Search();


            }
            if( mod_faund )
            {

                
                par_faund = true;
            }
            //Parity.SelectedIndex = --par_ind;

        }


        bool stb_faund = false;
        private void Stop_Bit_Search()
        {

            byte stb_lng = (byte)(sbit_enum.Length - 1);
            byte stb_ind = 0;

            while( !mod_faund && stb_ind <= stb_lng)
            {

                if( !mod_faund)
                {
                    switch(stb_ind)
                    {
                        case 0: COM1.StopBits = System.IO.Ports.StopBits.One; break;
                        case 1: COM1.StopBits = System.IO.Ports.StopBits.Two; break;
                        case 2: COM1.StopBits = System.IO.Ports.StopBits.OnePointFive; break;
                    }
                    stb_ind++;
                }
                Stopbit.SelectedIndex = --stb_ind;
                Parity_Search();
            }
            if (mod_faund)
            {
                
                stb_faund = true;
            }
            //Stopbit.SelectedIndex = --stb_ind;

        }



        private void Baudrate_SelectedIndexChanged(object sender, EventArgs e)
        {
            baudrate_label.BackColor = Color.Red;
            baudrate_label.Text = "Changed";

           
            //if( br_enum[Baudrate.SelectedIndex] == 0x1C200 )
            //{
            //    mb_packet.baudrate = 0xC200;
            //}
            //else
            //    mb_packet.baudrate = (ushort)br_enum[Baudrate.SelectedIndex];
 

        }

        private void Serial_Port(object sender, System.IO.Ports.SerialPinChangedEventArgs e)
        {
           
        }

        private void COM_sw_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                this.COM1.PortName = av_port[COM_sw.SelectedIndex];

            }
            catch
            {
                this.COM1.PortName = av_port[0];

            }
        }

        private void Databits_SelectedIndexChanged(object sender, EventArgs e)
        {

            databits_label.BackColor = Color.Red;
            databits_label.Text = "Changed";
            //mb_packet.databits = (ushort)(Databits.SelectedIndex + 8);


        }

        private void Parity_SelectedIndexChanged(object sender, EventArgs e)
        {
            parity_label.BackColor = Color.Red;
            parity_label.Text = "Changed";
            //mb_packet.parity = (ushort)Parity.SelectedIndex;
        }

        private void Stopbit_SelectedIndexChanged(object sender, EventArgs e)
        {
            stopbit_label.BackColor = Color.Red;
            stopbit_label.Text = "Changed";
            //mb_packet.stopbit = (ushort)(Stopbit.SelectedIndex + 1);
        }

        private void COM_sw_Click(object sender, EventArgs e)
        {
            if( !COM1.IsOpen)
                Fill_Com_Ports();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try {
                if (!COM1.IsOpen)
                {
                    COM1.Close();
                    COM1.PortName = "COM3";
                    COM1.BaudRate = 115200;
                    COM1.DataBits = 8;
                    COM1.Parity = System.IO.Ports.Parity.None;
                    COM1.StopBits = System.IO.Ports.StopBits.One;
                    COM1.Open();

                }
                m_mbMaster = ModbusSerialMaster.CreateRtu(COM1);

                ushort[] rBuf = { 200 };
                m_mbMaster.WriteMultipleRegisters(m_slaveAddr, 1, rBuf);
                System.Threading.Thread.Sleep(200);
                ushort[] tBuf = m_mbMaster.ReadHoldingRegisters(m_slaveAddr, 0x00, 104);
                int t = 5;
                t++;
            }
            catch(Exception ex)
            {
                this.Text = (string.IsNullOrEmpty(ex.Message)) ? "Form1" : ("Form1... Error: " + ex.Message);
            }

        }


        

        private void button2_Click(object sender, EventArgs e)
        {
            if (!COM1.IsOpen)
                COM1.Open();


            try
            {
                ushort[] tmpbuf = m_mbMaster.ReadHoldingRegisters(0, 0x64, 5);
                Thread.Sleep(200);
                Thread.Sleep(100);
            }
            catch(Exception ex)
            {
                this.Text = (string.IsNullOrEmpty(ex.Message)) ? "Form1" : ("Form1... Error: " + ex.Message);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void module_addr_TextChanged(object sender, EventArgs e)
        {
            addr_label.BackColor = Color.Red;
            addr_label.Text = "Changed";

            


            //mb_packet.dev_address = ushort.Parse( module_addr.Text );
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (COM1.IsOpen)
                COM1.Close();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (COM1.IsOpen)
                COM1.Close();
        }

        private void write_btn_Click(object sender, EventArgs e)
        {


            if (!COM1.IsOpen)
                COM1.Open();


            ushort[] SendBuf = pack_mb_msg();


            if (mb_packet.dev_address > 255 || mb_packet.dev_address < 0 )
            {
                status_line.Text = "Задан неверный адрес устройства";

            }
            else if( IsConnected )
            {
                try
                {
                    
                        m_mbMaster.WriteMultipleRegisters((byte)0, 0x64, SendBuf);

                }
                catch (Exception ex)
                {
                    this.Text = (string.IsNullOrEmpty(ex.Message)) ? "Form1" : ("Form1... Error: " + ex.Message);
                    baudrate_label.BackColor = Color.OrangeRed;
                    baudrate_label.Text = "Sent";
                    databits_label.BackColor = Color.OrangeRed;
                    databits_label.Text = "Sent";
                    parity_label.BackColor = Color.OrangeRed;
                    parity_label.Text = "Sent";
                    stopbit_label.BackColor = Color.OrangeRed;
                    stopbit_label.Text = "Sent";

                    addr_label.BackColor = Color.OrangeRed;
                    addr_label.Text = "Sent";
                }
            }
            else
            {
                status_line.Text = " Не полдключен ";
            }

  



         



        }


        private ushort[] pack_mb_msg()
        {



            mb_packet.dev_address = ushort.Parse(module_addr1.Text);


            if (br_enum[Baudrate.SelectedIndex] == 0x1C200)
            {
                mb_packet.baudrate = 0xC200;
            }
            else
                mb_packet.baudrate = (ushort)br_enum[Baudrate.SelectedIndex];

            mb_packet.databits = (ushort)(Databits.SelectedIndex + 8);

            mb_packet.parity = (ushort)Parity.SelectedIndex;

            mb_packet.stopbit = (ushort)(Stopbit.SelectedIndex + 1);





            //Message str;
            //str.dev_address = ushort.Parse(module_addr1.Text);


            //if (br_enum[Baudrate.SelectedIndex] == 0x1C200)
            //{
            //    str.baudrate = 0xC200;
            //}
            //else
            //    str.baudrate = (ushort)br_enum[Baudrate.SelectedIndex];

            //str.databits = (ushort)(Databits.SelectedIndex + 8);

            //str.parity = (ushort)Parity.SelectedIndex;

            //str.stopbit = (ushort)(Stopbit.SelectedIndex + 1);

            ushort[] Buf = { mb_packet.baudrate, mb_packet.databits, mb_packet.stopbit, mb_packet.parity, mb_packet.dev_address };

            return Buf;
        }

        private void Default_Click(object sender, EventArgs e)
        {
            Baudrate.SelectedIndex = 17;
            Databits.SelectedIndex = 0;
            Parity.SelectedIndex = 0;
            Stopbit.SelectedIndex = 0;
            module_addr1.Text = "1";
        }
    }
}
