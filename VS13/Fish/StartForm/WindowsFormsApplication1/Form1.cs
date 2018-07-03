using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using Modbus.Device;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            is_connected = false;
            stateForm = WorkState.Idle;


            Form_Update = new BackgroundWorker();
            Form_Update.WorkerSupportsCancellation = true;
            Form_Update.WorkerReportsProgress = true;
            Form_Update.DoWork += Form_Update_DoWork;
            Form_Update.ProgressChanged += Form_Update_ProgressChanged;

            Form_Update.RunWorkerAsync();



            m_mbMaster = ModbusSerialMaster.CreateRtu(COM);
            m_mbMaster.Transport.ReadTimeout = 100;
            m_mbMaster.Transport.Retries = 3;
            m_mbMaster.Transport.SlaveBusyUsesRetryCount = true;
            m_mbMaster.Transport.WaitToRetryMilliseconds = 100;

            m_bgwModbusUpdater = new BackgroundWorker();
            m_bgwModbusUpdater.WorkerSupportsCancellation = true;
            m_bgwModbusUpdater.WorkerReportsProgress = true;
            m_bgwModbusUpdater.DoWork += M_bgwModbusUpdater_DoWork;
            m_bgwModbusUpdater.ProgressChanged += M_bgwModbusUpdater_ProgressChanged;
            m_bgwModbusUpdater.RunWorkerAsync();



        }

        private void Form_Update_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                switch (stateForm)
                {
                    case WorkState.Idle:
                        break;
                    case WorkState.Update:
                        if (state == State.New)
                        { 
                            state = State.Read;
                        }
                        break;


                }
            }
        }

        private void M_bgwModbusUpdater_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
          //  throw new NotImplementedException();
        }

        public int sleeptime = 100;
        private void M_bgwModbusUpdater_DoWork(object sender, DoWorkEventArgs e)
        {
            {
                while (true)
                {
                    if (m_bgwModbusUpdater.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }

                    switch (stateRegs)
                    {
                        case WorkState.Idle:
                            break;
                        case WorkState.Update:
                            ModbusUpdate();
                            m_bgwModbusUpdater.ReportProgress(1);


                            // this.Text = (string.IsNullOrEmpty(m_lastError)) ? "Form1" : ("Form1... Error: " + m_state);

                            break;
                        default:
                            break;
                    }

                    Thread.Sleep(sleeptime);
                }
            }
        }

        private void Form_Update_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
          //  throw new NotImplementedException();
        }

        public static IModbusMaster m_mbMaster;
        readonly BackgroundWorker m_bgwModbusUpdater;
        readonly BackgroundWorker Form_Update;
        private void ConnectMenu_Click(object sender, EventArgs e)
        {
            string[] av_port = System.IO.Ports.SerialPort.GetPortNames();
            ConnectMenu.DropDownItems.Clear();

            ConnectMenu.DropDownItems.Add("Отключить");
            ConnectMenu.DropDownItems.Add("Настройка");
            foreach (string port in av_port)
                ConnectMenu.DropDownItems.Add(port);
        }
        object m_syncObject = new object();
        public enum State
        {
            Read,
            Write,
            New,
            Set
        }
        public State state { get; set; }
        private bool ModbusUpdate()
        {
            try
            {
                lock (m_syncObject)
                {
                    switch (state)
                    {
                        case State.Read:
                            try
                            {
                                state = State.New;
                            }
                            catch
                            {
                                m_lastError = "Ошибка чтения";
                                return false;
                            }
                            break;
                        case State.Write:
                            try
                            {
                                state = State.Set;
                            }
                            catch (Exception ex)
                            {
                                return false;
                            }
                            break;
                        case State.New:
                            break;
                        case State.Set:
                            state = State.Read;
                            break;
                    }


                }

                return true;
            }
            catch (Exception ex)
            {
                m_lastError = ex.Message;
                return false;
            }
        }




        public enum WorkState
        {
            Idle,
            Update
        }
        bool is_connected = false;
        bool is_open = false;
        public bool Csisopen { get; set; }
        private WorkState stateForm = new WorkState();
        public WorkState stateRegs { get; set; }
        string m_lastError;
        public void COM_set(int br, int db, int par, int sb, Int32 ad)
        {

            switch (br)
            {
                case 0:
                    COM.BaudRate = 9600;
                    break;
                case 1:
                    COM.BaudRate = 115200;
                    break;
            }

            switch (par)
            {
                case 0:
                    COM.Parity = System.IO.Ports.Parity.None;
                    break;
                case 1:
                    COM.Parity = System.IO.Ports.Parity.Even;
                    break;
                case 2:
                    COM.Parity = System.IO.Ports.Parity.Odd;
                    break;
            }

            switch (sb)
            {
                case 0:
                    COM.StopBits = System.IO.Ports.StopBits.One;
                    break;
                case 1:
                    COM.StopBits = System.IO.Ports.StopBits.One;
                    break;

            }


            //m_slaveAddr = (byte)ad;

            COM.DataBits = db + 7;
            //COM.Parity = par;
            //COM.StopBits = sb;


        }
        private void ConnectMenu_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string item = e.ClickedItem.ToString(); //ToolStripItemClickedEventArgs.ClickedItem;
            if (item == "Отключить" && COM.IsOpen)
            {
                COM.Close();
                is_connected = false;
                stateForm = WorkState.Idle;
                // Connectbtn.Enabled = false;
                stateRegs = WorkState.Idle;

                //return;
            }
            else if (item == "Настройка")
            {
                if (!is_open)
                {
                    if (COM.IsOpen)
                        COM.Close();

                    stateRegs = WorkState.Idle;
                    stateForm = WorkState.Idle;



                    Form Cs = new WindowsFormsApplication1.COM_settings();

                    Cs.Show();
                    is_open = true;
                    Csisopen = true;


                }



            }
            else
            {

                if (COM.IsOpen)
                    COM.Close();

                COM.PortName = item;
                try
                {
                    COM.Open();

                    is_connected = true;

                    stateForm = WorkState.Update;
                    stateRegs = WorkState.Update;
                }
                catch (Exception ex)
                {
                    m_lastError = ex.Message;
                }


            }
        }



    }
}
