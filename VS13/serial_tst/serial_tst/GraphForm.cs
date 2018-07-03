using common.utils;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZedGraph;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Runtime.InteropServices;

namespace serial_tst
{
    public partial class GraphForm : Form
    {
        public GraphForm()
        {
            InitializeComponent();
            FillSelectValueTypeCombobox();
            Fill_Com_Ports1();

            m_bgwModbusUpdater = new BackgroundWorker();
            m_bgwModbusUpdater.WorkerSupportsCancellation = true;
            m_bgwModbusUpdater.WorkerReportsProgress = true;
            m_bgwModbusUpdater.DoWork += M_bgwModbusUpdater_DoWork;
            m_bgwModbusUpdater.ProgressChanged += M_bgwModbusUpdater_ProgressChanged;

            m_bgwModbusUpdater.RunWorkerAsync();
            GraphPane myPane = zedGraphControl1.GraphPane;
            LineItem myCurve = myPane.AddCurve("Data", points, Color.Blue, SymbolType.None);

            //Point points = new Point();



            this.Load += Form1_Load;
            // comboBox1.SelectedItem = SIP.SipRegisters.Temp1.ToString();
            comboBox1.SelectedItem = ACOK.SipRegisters.Temperature.ToString();
            richTextBox4.Text = A.ToString();
            richTextBox5.Text = B.ToString();
            trackBar1.Value = (int)(A*100);
        }

        //-------------------------------------------------------------------------
        #region Data
        //---------------------------------------------------------------------
        #region Private data
        //
        public static IModbusMaster m_mbMaster;
        readonly BackgroundWorker m_bgwModbusUpdater;



        //Point points = new Point();
        
        PointPairList points = new PointPairList();
       // LineItem myCurve = myPane.AddCurve("Sinc", list, Color.Blue, SymbolType.None);

        const int RegistersCountMax = 0x100;
        const int IOCountMax = 0x50;

        byte m_slaveAddr = 0x01;
        ushort m_startAddr = 0x0;
        ushort m_count = 5;


        enum WorkState
        {
            Idle,
            Update
        }

        WorkState m_state = WorkState.Idle;

        ushort[] rdBuf;
        ushort[] rxData;
        string m_lastError;
        object m_syncObject = new object();
        //
        #endregion // Private data
        //---------------------------------------------------------------------
        #region Public Fields
        //

        ACOK Data = new ACOK();
        //
        #endregion // Public Fields
        //---------------------------------------------------------------------
        #endregion //Data
        //-------------------------------------------------------------------------
        #region Public
        //


        //
        #endregion // Public
        //-------------------------------------------------------------------------
        #region Protected
        //

        //
        #endregion // Protected
        //-------------------------------------------------------------------------
        #region Private
        //
        common.devices.USM.MbUsmRegisters USM_Reg;
        bool ModbusUpdate()
        {
            try
            {
                lock (m_syncObject)
                {
                    
                    //for (int i = 0; i < 1281; i += 125)
                    //{
                    ///*rdBuf*/ = m_mbMaster.ReadHoldingRegisters(m_slaveAddr, m_startAddr, m_count);
                    //for (int j = 0; j < 126; j++)
                    //{
                    //    if (i + j < 1281)
                    //    {
                    //        //MyClass.rxData[i + j] = rdBuf[j];
                    //    }

                    //}
                    
                    int count = RegistersCountMax; 
                    ushort addr = m_startAddr;
                    int rdLen;
                    int index = 0;
                    if (rdBuf == null)
                        rdBuf = new ushort[RegistersCountMax];

                    while (count > 0)
                    {
                        rdLen = count;
                        if (rdLen > IOCountMax)
                            rdLen = IOCountMax;

                        ushort[] tBuf = m_mbMaster.ReadHoldingRegisters(m_slaveAddr, (ushort)index, (ushort)rdLen);
               //         ushort[] tBuf = m_mbMaster.ReadHoldingRegisters(1, 1, 5);
                        Array.Copy(tBuf, 0, rdBuf, index, rdLen);

                        index += rdLen;
                        addr += (ushort)rdLen;
                        count -= rdLen;

                        // ushort[] buf = ValueUtil.StructToBuff<ushort, RegistersData.MbRegisters>(USM_Reg);
                       









                        //GetVals(tBuf);
                    }
                    USM_Reg = ValueUtil.BufToStruct<common.devices.USM.MbUsmRegisters, ushort>(rdBuf);
                    //}
                    
                    



                    //rdBuf[280] = 1;
                }

                return true;
            }
            catch (Exception ex)
            {
                m_lastError = ex.Message;
                return false;
            }
        }

        public void DataUpd()
        {
            float reg_val = 0;
            switch (Selected_Reg_line)
            {
                case "Temperature":
                    reg_val = (float)USM_Reg.Temperature;
                    break;
                case "Humidity":
                    reg_val = (float)USM_Reg.Humidity;
                    break;
                case "Pressure":
                    reg_val = (float)USM_Reg.Pressure;
                    break;
                case "WindSpeed":
                    reg_val = (float)USM_Reg.WindSpeed;
                    break;
                case "WindDir":
                    reg_val = (float)USM_Reg.WindDir;
                    break;
                case "TemperatureAvg":
                    reg_val = (float)USM_Reg.TemperatureAvg;
                    break;
                case "HumidityAvg":
                    reg_val = (float)USM_Reg.HumidityAvg;
                    break;
                case "PressureAvg":
                    reg_val = (float)USM_Reg.PressureAvg;
                    break;
                case "WindSpeedAvg":
                    reg_val = (float)USM_Reg.WindSpeedAvg;
                    break;
                case "WindDirAvg":
                    reg_val = (float)USM_Reg.WindDirAvg;
                    break;
                case "Pair1AvgTime":
                    reg_val = (float)USM_Reg.Pair1AvgTime;
                    break;
                case "Pair1Time":
                    reg_val = (float)USM_Reg.Pair1Time;
                    break;
                case "Pair2AvgTime":
                    reg_val = (float)USM_Reg.Pair2AvgTime;
                    break;
                case "Pair2Time":
                    reg_val = (float)USM_Reg.Pair2Time;
                    break;

                
                default:
                    break;



            }


            Azimuth.Text = USM_Reg.WindDir.ToString();

            PlotPoint(reg_val);
            


        }
        int counter = 0;
        void PlotPoint(float point)
        {

            //GraphPane myPane = zedGraphControl1.GraphPane;
            //Point points = new Point();

            points.Add(counter, point);
            LineItem myCurveTwo = zedGraphControl1.GraphPane.AddCurve("USM_Data", points, Color.Black, SymbolType.None);
            zedGraphControl1.AxisChange();

            // Обновляем график
            zedGraphControl1.Invalidate();

            counter++;

            
        }


        void UIUpdate()
        {
            lock (m_syncObject)
            {
                this.Text = (string.IsNullOrEmpty(m_lastError)) ? "Form1" : ("Form1... Error: " + m_lastError);
                button1_Click_1(this, null);

                setTime.Text = USM_Reg.DateTime.Hour.ToString("D2") + ":" + USM_Reg.DateTime.Minutes.ToString("D2") + ":" + USM_Reg.DateTime.Seconds.ToString("D2");

                setDate.Text = USM_Reg.DateTime.Day.ToString("D2") + ":" + USM_Reg.DateTime.Month.ToString("D2") + ":" + USM_Reg.DateTime.Year.ToString("D4");

                DataUpd();

            }
        }

        void FillSelectValueTypeCombobox()
        {
            comboBox1.Items.Clear();
            foreach (ACOK.SipRegisters item in Enum.GetValues(typeof(ACOK.SipRegisters)))
            {
                comboBox1.Items.Add(EnumDescriptor.Get(item));
            }
        }


        public static Tret BufToStruct<Tret, Tparams>(Tparams[] buf) where Tret : struct
        {
            GCHandle handle = GCHandle.Alloc(buf, GCHandleType.Pinned);		// Выделить память

            IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buf, 0);	// и взять адрес
            Tret ret = (Tret)Marshal.PtrToStructure(ptr, typeof(Tret));		// создать структуру
            handle.Free();													// Освобождить дескриптор

            return ret;
        }
        //USM_Reg = ValueUtil.BufToStruct<common.devices.USM.MbUsmRegisters, ushort>(buf);
        public static Tret[] StructToBuff<Tret, Tparams>(Tparams value) where Tparams : struct
        {
            Tret tmp = default(Tret);
            Tret[] buf = new Tret[Marshal.SizeOf(value) / Marshal.SizeOf(tmp)];

            GCHandle handle = GCHandle.Alloc(buf, GCHandleType.Pinned);		// Выделить память
            IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buf, 0);	// и взять адрес
            Marshal.StructureToPtr(value, ptr, true);						// копировать в массив
            handle.Free();													// Освобождить дескриптор

            return buf;

        }


        public void Parse(ushort[] src, int startIndex, int count)
        {
            //ClearRegs();

            if ((src == null) || (src.Length <= 0))
                return;

            ushort[] buf = ValueUtil.StructToBuff<ushort, common.devices.USM.MbUsmRegisters>(USM_Reg);

            int end = startIndex + count;
            for (int i = startIndex; i < end; i++)
            {
                if (i >= buf.Length)
                    break;
                if (i >= src.Length)
                    break;

                buf[i] = src[i];
            }

            USM_Reg = ValueUtil.BufToStruct< common.devices.USM.MbUsmRegisters, ushort>(buf);
        }

        //
        #endregion // Private
        //-------------------------------------------------------------------------
        #region Events
        //

        public int sleeptime = 1000;
        private void M_bgwModbusUpdater_DoWork(object sender, DoWorkEventArgs e)
        {

            while (true)
            {
                if (m_bgwModbusUpdater.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                switch (m_state)
                {
                    case WorkState.Idle:
                        
                        break;
                    case WorkState.Update:
                        ModbusUpdate();
                        m_bgwModbusUpdater.ReportProgress(1);
                       // DataUpd();
                        break;
                    default:
                        break;
                }

                Thread.Sleep(sleeptime);
            }
        }

        private void M_bgwModbusUpdater_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            UIUpdate();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

            try
            {
                if (COM3.IsOpen)
                {
                    COM3.Write("3");
                    COM3.Close();
                    richTextBox1.Text = "закрыт";
                    m_state = WorkState.Idle;
                };
            }
            catch { }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!COM3.IsOpen)
            {
                try
                {
                    COM3.Open();
                    richTextBox1.Text = "открыт";

                    //Thread read = 
                    m_mbMaster = ModbusSerialMaster.CreateRtu(COM3);
                    m_state = WorkState.Update;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {



        }



        private void setDate_TextChanged(object sender, EventArgs e)
        {
            string newDate = setDate.Text;

            string[] newDate_val = newDate.Split('.');

            //UInt16[] DMY = new UInt16[3];





            USM_Reg.DateTime.Day = Convert.ToUInt16(newDate_val[0]);
            USM_Reg.DateTime.Month = Convert.ToUInt16(newDate_val[1]);
            USM_Reg.DateTime.Year = Convert.ToUInt16(newDate_val[2]);

        }

        private void setTime_TextChanged(object sender, EventArgs e)
        {
            string newDate = setTime.Text;

            string[] newDate_val = newDate.Split(':');

            //UInt16[] DMY = new UInt16[3];

            USM_Reg.DateTime.Hour = Convert.ToUInt16(newDate_val[0]);
            USM_Reg.DateTime.Minutes = Convert.ToUInt16(newDate_val[1]);
            USM_Reg.DateTime.Seconds = Convert.ToUInt16(newDate_val[2]);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DateTime lockal = DateTime.Now;

            setDate.Text = lockal.Day.ToString("D2") + ":" + lockal.Month.ToString("D2") + ":" + lockal.Year.ToString("D2");

            setTime.Text = lockal.Hour.ToString("D2") + ":" + lockal.Minute.ToString("D2") + ":" + lockal.Second.ToString("D2");

            setDate_TextChanged(this, null);
            setTime_TextChanged(this, null);

            
            ushort[] tBuff = ValueUtil.StructToBuff<ushort, common.devices.DateTimeRegisters>(USM_Reg.DateTime);


            ushort[] Password = new ushort[1];
            Password[0] = 257;
            ushort[] Pass = new ushort[1];
            Pass[0] = 16;
            ushort[] PassNull = new ushort[1];
            PassNull[0] = 0;
            try
                {
                    if (!COM3.IsOpen)
                        COM3.Open();


                //Thread read = 
                //WorkState m_state_prev = m_state;
                //m_state = WorkState.Idle; 

                m_mbMaster.WriteMultipleRegisters(0x01, 0x00, Password);
                m_mbMaster.WriteMultipleRegisters(0x01, 254, Pass);
                Thread.Sleep(10);
                m_mbMaster.WriteMultipleRegisters(0x01, 10, tBuff);
                Thread.Sleep(10);
               // m_mbMaster.WriteMultipleRegisters(0x01, 241, PassNull);

                //m_state = m_state_prev;
                //m_mbMaster.WriteMultipleRegisters(0x01, 0x00, Password);
                //m_mbMaster.WriteMultipleRegisters(0x01, 10, tBuff);

            }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

        }
        double[] Sipval = new double[1];



        string[] valarray = null;
        int i = 0;
        public void button6_Click(object sender, EventArgs e)
        {
            string selected = comboBox1.Text;



            ACOK.SipRegisters sel = EnumDescriptor.ToEnum<ACOK.SipRegisters>(selected);

            //if (selected == "Data1")
            //{
            //    Dispdata();
            //}


            //richTextBox2.Text += Data.GetVal(sel).ToString();
            //richTextBox2.Text += " ";
            //i++;
        }


        double sumx = 0, sumx2 = 0, sumy = 0, sumxy = 0, a, b, a1, b1;

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.L)
            {
                MessageBox.Show(":)");
            }
        }
        bool eventflagL = false;
        bool eventflagR = false;
        double x1 = 0, x2 = 0, y1 = 0, y2 = 0;
        double graphX, graphY, graphYn, graphX2, graphY2;
        int zoomcase = 1;



        private void Form1_Load(object sender, EventArgs e)
        {
            ////GraphPane myPane = zedGraphControl1.GraphPane;
            ////myPane.YAxis.Max = 40;
            ////myPane.YAxis.Min = 0;
            ////myPane.XAxis.Min = 0;
            ////myPane.XAxis.Max = 50;
        }





        private void Mousedown(object sender, MouseEventArgs e)
        {
            GraphPane myPane = zedGraphControl1.GraphPane;
            Point eventPoint = new Point(e.X, e.Y);


            if (e.Button == MouseButtons.Left & zoomcase == 1)
            {
                myPane.ReverseTransform(new PointF(e.X, e.Y), out graphX, out graphY, out graphYn);
                x1 = graphX;
                y1 = graphY;
                axisflag = 0;
                yscalectrl = 0;
                eventflagR = true;
                Cursor = Cursors.Cross;
            }


        }
        private void Mouseup(object sender, MouseEventArgs e)
        {

            GraphPane myPane = zedGraphControl1.GraphPane;

            if (e.Button == MouseButtons.Left & zoomcase == 1)
            {
                myPane.ReverseTransform(new PointF(e.X, e.Y), out graphX, out graphY, out graphYn);
                x2 = graphX;
                y2 = graphY;
                axisflag = 0;
                yscalectrl = 0;
                eventflagL = true;
                Cursor = Cursors.Default;
            }
            if (eventflagL & eventflagR == true)
            {


                if (Math.Abs(x1 - x2) < 0.01)
                {
                    x1 = myPane.XAxis.Max;
                }
                if (Math.Abs(y1 - y2) < 0.01)
                {
                    y1 = myPane.YAxis.Max;
                }


                myPane.XAxis.Max = Math.Max(x2, x1);
                myPane.XAxis.Min = Math.Min(x2, x1);
                myPane.YAxis.Max = Math.Max(y2, y1);
                myPane.YAxis.Min = Math.Min(y2, y1);
                eventflagL = false;
                eventflagR = false;
                zedGraphControl1.Refresh();
            }
        }
        double yscalectrl = 1;


        


        double A = 0.5;
        double B = 0.5;
        string Selected_Reg_line;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
             Selected_Reg_line = comboBox1.Text;



            ACOK.SipRegisters sel = EnumDescriptor.ToEnum<ACOK.SipRegisters>(Selected_Reg_line);
        }

        private void PortConfigBtn_Click(object sender, EventArgs e)
        {

            m_state = WorkState.Idle;
            GSAD_net_settings.Form1 f2 = new GSAD_net_settings.Form1();
            f2.SerialPort = COM3;
            f2.MbMaster = m_mbMaster;
            f2.Show();
        }
        string[] av_port1;
        private void PortSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                COM3.PortName = av_port1[PortSelect.SelectedIndex];

            }
            catch
            {
                COM3.PortName = av_port1[0];

            }
        }


        private void Fill_Com_Ports1()
        {

            av_port1 = System.IO.Ports.SerialPort.GetPortNames();


            this.PortSelect.AutoCompleteCustomSource.AddRange(av_port1);

            PortSelect.Items.Clear();
            for (int i = 0; i < av_port1.Length; i++)
            {
                PortSelect.Items.Add(av_port1[i]);
            }
            if (av_port1 != null)
                PortSelect.SelectedIndex = 0;



            try
            {

                COM3.PortName = av_port1[PortSelect.SelectedIndex];

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                //this.COM1.PortName = av_port[0];

            }


        }





        private void PortSelect_Click(object sender, EventArgs e)
        {
            if (!COM3.IsOpen)
                Fill_Com_Ports1();
        }

        private void TimeDatebtn_Click(object sender, EventArgs e)
        {
            Form f3 = new TimeDateSet();
            f3.Show();
        }

        private void setTime_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            A = (double)trackBar1.Value/100;
            B = 1 - A;

            richTextBox4.Text = A.ToString();
            richTextBox5.Text = B.ToString();


        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            COM3.Close();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            COM3.Close();
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {

            GraphPane myPane = zedGraphControl1.GraphPane;
            myPane.ReverseTransform(new PointF(e.X, e.Y), out graphX, out graphY, out graphYn);



            if (graphX > myPane.XAxis.Min & graphY > myPane.YAxis.Min & graphX < myPane.XAxis.Max & graphY < myPane.YAxis.Max)
            {
                richTextBox2.Text = graphX.ToString("N2");
                richTextBox3.Text = graphY.ToString("N2");
            }


            if (eventflagL & zoomcase == 3)
            {
                PointPairList diagline = new PointPairList();
                //diagline.Add(x1, y1);
                //diagline.Add(graphX, graphY);

                double[] cX = new double[1];
                double[] cY = new double[1];
                cX[0] = x1;
                cY[0] = y1;
                LineItem midline = myPane.AddCurve(null, cX, cY, Color.Blue, SymbolType.Star);
                //midline.Line.Style = DashStyle.Dot;
                //diagline.Clear();

            }

        }


        private void zedGraphControl1_MouseClick(object sender, MouseEventArgs e)
        {

            GraphPane myPane = zedGraphControl1.GraphPane;

            Point eventPoint = new Point(e.X, e.Y);
            if (e.Button == MouseButtons.Left & zoomcase == 2 & eventflagL == false)
            {
                myPane.ReverseTransform(new PointF(e.X, e.Y), out graphX, out graphY, out graphYn);
                x1 = graphX;
                y1 = graphY;

                if (eventflagR == false)
                {
                    Cursor = Cursors.Cross;
                }
                else
                {
                    Cursor = Cursors.Default;
                }
                eventflagL = true;
                yscalectrl = 0;
            }
            if (e.Button == MouseButtons.Right & zoomcase == 2 & eventflagR == false)
            {

                myPane.ReverseTransform(new PointF(e.X, e.Y), out graphX2, out graphY2, out graphYn);
                x2 = graphX2;
                y2 = graphY2;

                if (eventflagL == false)
                {
                    Cursor = Cursors.Cross;
                }
                else
                {
                    Cursor = Cursors.Default;
                }

                axisflag = 0;
                eventflagR = true;
                yscalectrl = 0;
            }

            if (e.Button == MouseButtons.Middle)
            {
                Scaledef();
                zedGraphControl1.Refresh();
                axisflag = 1;
                yscalectrl = 1;
                eventflagL = false;
                eventflagR = false;
                Cursor = Cursors.Default;
                zedGraphControl1.Refresh();

            }

            if (eventflagL & eventflagR == true & zoomcase == 2)
            {


                if (Math.Abs(x1 - x2) < 0.01)
                {
                    x1 = myPane.XAxis.Max;
                }
                if (Math.Abs(y1 - y2) < 0.01)
                {
                    y1 = myPane.YAxis.Max;
                }


                myPane.XAxis.Max = Math.Max(x2, x1);
                myPane.XAxis.Min = Math.Min(x2, x1);
                myPane.YAxis.Max = Math.Max(y2, y1);
                myPane.YAxis.Min = Math.Min(y2, y1);
                eventflagL = false;
                eventflagR = false;
                zedGraphControl1.Refresh();
                Cursor = Cursors.Default;
            }


        }

        double[] yval;

        int axisflag = 1;

        private void Scaledef()
        {
            GraphPane myPane = zedGraphControl1.GraphPane;
            myPane.YAxis.Max = 40;
            myPane.YAxis.Min = 0;
            myPane.XAxis.Min = 0;
            myPane.XAxis.Max = 50;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            GraphPane myPane = zedGraphControl1.GraphPane;
            PointPairList listPointsTwo = new PointPairList();
            string selected = comboBox1.Text;
           // ACOK.SipRegisters sel = EnumDescriptor.ToEnum<ACOK.SipRegisters>(selected);
            if (selected != "Data1")
            {


                if (i * yscalectrl > myPane.XAxis.Max - 5)
                {
                    myPane.XAxis.Max = (i + 10);
                    axisflag = 0;
                }
                i++;


   //             Sipval[(i - 1)] = Data.GetVal(sel);


                zedGraphControl1.AxisChange();
                zedGraphControl1.Refresh();

                //myPane.ScaledGap(1);
                ACOK.SipRegisters sel = EnumDescriptor.ToEnum<ACOK.SipRegisters>(selected);
               // Rangscale(sel);

                
                myPane.IsFontsScaled = true;


                PointPairList approx2 = new PointPairList();


                if (i > 1)
                {
                    a = (sumxy * (i - 1) - sumy * sumx) / (sumx2 * (i - 1) - Math.Pow(sumx, 2));
                    b = (sumy - a * sumx) / (i - 1);
                    for (double x = i - 1; x <= i; x += 0.5)
                    {
                        approx2.Add(x, a1 * x + b1);
                    }


                }
                sumx += i;
//                sumy += Data.GetVal(sel);
                sumx2 += Math.Pow(i, 2);
 //               sumxy += i * Data.GetVal(sel);

                a = (sumxy * i - sumy * sumx) / (sumx2 * i - Math.Pow(sumx, 2));
                b = (sumy - a * sumx) / i;
                PointPairList approx1 = new PointPairList();
                approx1.Clear();
                for (double x = 1; x <= i; x += 0.5)
                {

                    approx1.Add(x, a * x + b);
                }
                /*
                LineItem f1_curve = myPane.AddCurve(null, approx1, Color.Black, SymbolType.None);
                */
                myPane.CurveList.Clear();

                for (int u = 0; u < Sipval.Length; u++)
                {
                    listPointsTwo.Add(u + 1, Sipval[u]);

                }
              //  LineItem myCurveTwo = myPane.AddCurve("real T", listPointsTwo, Color.Black, SymbolType.None);
                if (i > 2)
                {
                    bezcurv(Sipval, i);
                }
                Array.Resize<double>(ref Sipval, Sipval.Length + 1);

                //byte[] temBuf = new byte[4];
                //int index = 0;

                //private void COM3_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
                //{

                //    ////byte[] tempval = new byte[3];
                //    //int[] tempval = new int[3];// tempval[3];
                //    //int j = 0;

                //        int count = COM3.BytesToRead;
                //        while (count > 0)
                //        {
                //            byte[] buf = new byte[count];
                //            COM3.Read(buf, 0, count);

                //            int wrCount = ((temBuf.Length - index) < count) ? count : count; //(temBuf.Length - index);
                //            Array.Copy(buf, 0, temBuf, index, wrCount);

                //            index += wrCount;
                //            if (index >= temBuf.Length)
                //            {
                //                index = 0;
                //                float temp = 0;
                //                try
                //                {
                //                    temp = BitConverter.ToSingle(temBuf, 0);
                //                }
                //                catch {}

                //                richTextBox1.Invoke(new Action(() =>
                //                {
                //                    richTextBox2.Text = string.Format("{0:F3}", temp); ;
                //                }));
                //            }

                //            //tempval[j]= COM3.Read(buf, 0, count); 
                //            //richTextBox4.Text += tempval[j];
                //            //j++;
                //            //if (j > 2)
                //            //{
                //            //    j = 0;
                //            //};



                //            //string strB = "";
                //            //for (int i = 0; i < count; i++)
                //            //{
                //            //    strB += string.Format("{0:X2}", buf[i]);
                //            //    strB += "\t";
                //            //}

                //            //richTextBox1.Invoke(new Action(() =>
                //            //{
                //            //   richTextBox2.Text += strB;
                //            //}));

                //            count = COM3.BytesToRead;
                //        }
                //    }

                //private void COM3_ErrorReceived(object sender, System.IO.Ports.SerialErrorReceivedEventArgs e)
                //{

                //}
                //   
                #endregion // Events
                //-------------------------------------------------------------------------





                //button1_Click_1(this, null);
            }
        }

        public double fact(double n)
        {
            if (n == 0)
            {
                return 1;
            }

            double k = 1;
            for (double o = n; o > 0; o--)
            {
                k *= o;
            }

            return k;
        }
        public double bern(double n, double tl, double nf)
        {
            double bernval;
            double midval1 = n - nf;
            double midval2 = (Math.Pow(tl, nf));
            double midval3 = (Math.Pow((1 - tl), (midval1)));
            double midval4 = fact(nf) * fact(n - nf);
            bernval = midval2 * midval3 * (fact(n) / midval4);
            return bernval;
        }


        public void bezcurv(double[] val, int abs)
        {
            //GraphPane myPane = zedGraphControl1.GraphPane;
            //PointPairList bez = new PointPairList();
            //double step = 0.01;

            //for (double t = 0; t < 1; t += step)
            //{
            //    if (t > 1) { t = 1; }
            //    double resvalx = 0;
            //    double resvaly = 0;
            //    for (int k = 0; k < abs; k++)
            //    {

            //        resvaly += val[k] * bern((abs - 1), t, k);
            //        resvalx += (k + 1) * bern((abs - 1), t, k);
            //    }
            //    bez.Add(resvalx, resvaly);
            //}
            //LineItem bezval = myPane.AddCurve("nonlinear curve", bez, Color.Red, SymbolType.None);
            //bez.Clear();
        }



        //double[] filtval = new double[RegistersCountMax - 257];
        //double[] filtval1 = new double[RegistersCountMax - 257];
        //double[] filtval2 = new double[RegistersCountMax - 257];
       // int upshift = 0;
        internal void Dispdata(ACOK.SipRegisters sel2)
        {         
           
            GraphPane myPane = zedGraphControl1.GraphPane;
            PointPairList listPoints = new PointPairList();
            PointPairList filtpoints = new PointPairList();
            PointPairList filtpoints1 = new PointPairList();
            PointPairList filtpoints2 = new PointPairList();

            //filtval[0] = rdBuf[257];
            //filtval2[0] = rdBuf[257];

            if (axisflag == 1 & yscalectrl == 1)
            {
                myPane.YAxis.Max = 2500;
                myPane.YAxis.Min = -10;
                myPane.XAxis.Max = 20;
                myPane.XAxis.Min = 0;
            }

            myPane.CurveList.Clear();

            //for(int yr =0; yr<rdBuf.Length; yr++)
            //{
            //    rdBuf[yr] /= 13;
            //}

            //for (int t = 1; t < RegistersCountMax-258; t++)
            //{
            //    filtval[t] =B * (t) + A * rdBuf[t + 257 - 1];
            //    filtpoints.Add((t)/51.2, filtval[t]/26.2);
            //    filtval1[t] = (rdBuf[t + 257 - 1] + rdBuf[t + 257] + rdBuf[t + 257 + 1])/3;
            //    filtpoints1.Add((t)/51.2, filtval1[t]/26.2);
            //    filtval2[t] = (rdBuf[t+257-1] + rdBuf[t+257]) / 2;
            //    filtpoints2.Add(t/51.2, filtval2[t]/26.2);

            //}

            ////double xfilt = 0;
            ////double yfilt = 0;
            ////for(int t =0; t < RegistersCountMax-257; t++)
            ////{
            ////    for(int j =0; j < t; j++)
            ////    {
            ////        yfilt = yfilt + rdBuf[j + 258-1];
            ////        xfilt += j; 
            ////    }
            ////    xfilt -= t;
            ////    filtpoints2.Add(t, B * xfilt - A * yfilt);


            ////}

            //LineItem FiltLine = myPane.AddCurve("Filtered data", filtpoints, Color.Black, SymbolType.None);
            //LineItem Filtline1 = myPane.AddCurve("Filtered data1", filtpoints1, Color.Blue, SymbolType.None);
            //LineItem Filtline2 = myPane.AddCurve("Filtered data2", filtpoints2, Color.Green, SymbolType.None);
            double[] apprBuf = new double[8];
            
            
            for (int t = 257; t < RegistersCountMax; t++)
            {
                if (t < RegistersCountMax - 4 & t > 260)
                {
                    for (int l = -3; l < 5; l++)
                    {
                        apprBuf[l+3] = rdBuf[t + l];
                    }
                }
                
                filtpoints.Add((t - 257)/51.2, apprBuf.Average()/26.2);
                //SIP.SipRegisters trx = t;
                listPoints.Add((t - 257)/51.2, rdBuf[t]/26.2);
  //              approxL[t - 257] = rdBuf[t];

            }

  //          bezcurv(approxL, RegistersCountMax - 257);






            //upshift++;
            //LineItem midline = myPane.AddCurve(null, cX, cY, Color.Blue, SymbolType.Star);
            LineItem Dataline = myPane.AddCurve("Data", listPoints, Color.Coral, SymbolType.None);
            LineItem Filtline = myPane.AddCurve("Averaged data", filtpoints, Color.Black, SymbolType.None);
            Dataline.Line.Width = 3;
            zedGraphControl1.Refresh();
            zedGraphControl1.AxisChange();
            //sleeptime = 1000;
        }

        private void Rangscale(ACOK.SipRegisters sel1)
        {


            zedGraphControl1.AxisChange();
            GraphPane myPane = zedGraphControl1.GraphPane;

            double tmpymax, tmpymin;

            if (axisflag == 1)
            {
                myPane.YAxis.Max = 5;
                myPane.YAxis.Min = -5;
                myPane.XAxis.Max = 5;
                myPane.XAxis.Min = -5;
            }

            if (i + 5 > myPane.XAxis.Max )
            {
                myPane.XAxis.Max += 10;
                axisflag = 0;
            }
            if (Data.GetVal(sel1) == 0) { axisflag = 1; }



            tmpymax = myPane.YAxis.Max;
            tmpymin = myPane.YAxis.Min;
            if ((Data.GetVal(sel1) > myPane.YAxis.Max) & axisflag == 0)
            {


                myPane.YAxis.Max = (Data.GetVal(sel1) * 1.3);
                myPane.YAxis.Min = (Data.GetVal(sel1) * 0.4);
                axisflag = 0;
            }

            double res = tmpymax - Data.GetVal(sel1) * 0.2;
            if (Math.Abs(res) > Data.GetVal(sel1) & axisflag == 0)
            {
                myPane.YAxis.Min = (Data.GetVal(sel1) * 0.4);
                myPane.YAxis.Max = (Data.GetVal(sel1) * 1.3);

            }

        }




    }
}











