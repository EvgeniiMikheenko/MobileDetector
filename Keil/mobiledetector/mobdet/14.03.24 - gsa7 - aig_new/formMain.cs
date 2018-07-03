using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;
using System.Net.Sockets;
using System.Net;
using System.IO.Ports;
using System.IO;
using Modbus;
using Modbus.Device;

using LuaInterface;
using common.utils.Files;

namespace LGAR
{

    public partial class formMain : Form
    {
        public int state_select = 0;
        public byte numUm_reg = 0;
        public byte numF_reg = 0;
        public byte numEcm_reg = 0;
        GsaRM Device = null;
        #region - Init 1 -
        public formMain()
        {

            


            InitializeComponent();

            if (!BaseSerializer<AppSettings>.LoadFromFile("AppSettings.xml", out m_settings))
            {
                m_settings = new AppSettings()
                {
                    DevAddrt = 0
                };
                BaseSerializer<AppSettings>.SaveToFile("AppSettings.xml", m_settings);
            }

            //Device.DeviceID = m_settings.DevAddrt;

            Trend.SetupGraph(graph);
            UpdateStatus();
            scroller.AutoScroll = true; // HACK: runtime fix designer
            numY.Value = (int)(Trend.Y1 * 1000);
            DEBUG = new formDebugConsole();
            DEBUG.CreateControl();
            // lua
            try
            {
                Lua1(); // Lua init
                DEBUG.SetInterpreter(LuaInterpreter);
            }
            catch (Exception ex)
            {
                DEBUG.Print(ex.GetType().ToString() + ": " + ex.Message, Color.Magenta); // shit happens
                DEBUG.Trace("Hint: Did you forget Microsoft Visual C++ 2010 x86 Redistributable?");
            }
            //
            RegisterControls();
            Title = Text;

            StateSelector.Items.Clear();
            for (int i = 0; i < GsaRM.ZonesCount; i++)
            {
                StateSelector.Items.Add((i + 1));
            }

            if (StateSelector.Items.Count > 0)
                StateSelector.SelectedIndex = 0;


            
        }

        AppSettings m_settings;

        private void formMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DEBUG.Dispose();
                if (LuaInterpreter != null)
                    LuaInterpreter.Dispose();
            }
#if !DEBUG
            catch { }
#endif
            finally { }
        }

        string[][] regs = new string[][]
                {
                    new string[] {"connectEthernet", "ConnectEthernet"},
                    new string[] {"connect", "ConnectSerialNamed"},
                    new string[] {"disconnect", "Disconnect"},
                    new string[] {"msg", "msg"},
                    new string[] {"reread", "ReRead"},
                    new string[] {"update", "DeviceUpdate"},
                    new string[] {"r", "DeviceRead"},
                    new string[] {"w", "DeviceWrite"},
                    new string[] {"poller", "SetPoller"},
                    new string[] {"hex", "HEX"},
                    new string[] {"sleep", "Sleep"},
                    new string[] {"test", "test"},
                    new string[] {"record", "RecordStart"},
                    new string[] {"stop", "RecordStop"},
                    new string[] {"interval", "SetInterval"},
                    //new string[] {"", ""},
                };

        Lua LuaInterpreter = null;

        void Lua1() // native
        {
            LuaInterpreter = new Lua();
            DEBUG.Trace(LuaInterpreter.GetString("_VERSION"));
#if DEBUG
            DEBUG.Trace(" * DEBUG * ");
#endif
            // register functions
            LuaInterpreter.RegisterFunction("print", DEBUG, DEBUG.GetType().GetMethod("LuaPrint"));
            LuaInterpreter.RegisterFunction("trace", DEBUG, DEBUG.GetType().GetMethod("Trace"));
            LuaInterpreter.DoString(
                "do local p=print print=function(...) p({...}) end end"); // HACK: vararg

            //
            var tt = this.GetType();
            for (int i = 0, l = regs.Length; i < l; ++i)
            {
                var method = tt.GetMethod(regs[i][1]);
                if (method != null)
                    LuaInterpreter.RegisterFunction(regs[i][0], this, method);
                else DEBUG.Error(new Exception("Register failed: " + regs[i]));
            }
            //
            LuaInterpreter.DoString(
                "do local f=r r=function(p1,p2) return f(p1 or error('r(start[,count])'),p2 or 1) end end");
        }

        #endregion

        string Title = "";
        formDebugConsole DEBUG = null;

       // GsaRM Device = null;
        
        PointPairList pts = new PointPairList();
        bool errorState = false; // есть ошибки
        uint errorCount = 0; // счетчик ошибок
        uint errorSequentialCount = 0; // счетчик последовательных ошибок
        bool controlsInit = true;

        #region - Menu -

        // FILE

        private void menuFOpen_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Снимок|*.snapshot";
                if(dlg.ShowDialog()==System.Windows.Forms.DialogResult.OK)
                    try
                    {
                        Trend.SnapshotLoad(graph, dlg.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
            }
        }

        private void menuFSave_Click(object sender, EventArgs e)
        {
            string snap=Trend.SnapshotGet(graph);
            using (var dlg = new SaveFileDialog())
            {
                dlg.Filter = "Снимок|*.snapshot";
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    try
                    {
                        File.WriteAllText(dlg.FileName, snap);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
            }
        }

        private void menuFImport_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog()
            {
                Filter = "Параметры|*.ini",
                Title = "Загрузка параметров"
            })
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    try
                    {
                        Device.SettingsLoad(dlg.FileName);
                        controlsInit = true;
                    }
                    catch (Exception ex)
                    {
                        DEBUG.Error(ex);
                        MessageBox.Show(ex.Message, "Ошибка загрузки",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

        }

        private void menuFExport_Click(object sender, EventArgs e)
        {
            using (var dlg = new SaveFileDialog()
            {
                Filter = "Параметры|*.ini",
                Title = "Сохранение параметров"
            })
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    try
                    {
                        Device.SettingsSave(dlg.FileName);
                    }
                    catch (Exception ex)
                    {
                        DEBUG.Error(ex);
                        MessageBox.Show(ex.Message, "Ошибка сохранения",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

        }

        private void menuFTerminate_Click(object sender, EventArgs e)
        {
            Close();
        }

        // CONNECT

        private void menuCSocket_Click(object sender, EventArgs e)
        {
            using (var frm = new formEthernet())
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    try
                    {
                        Disconnect();
                        if (!ConnectEthernet(frm.NetworkLocation))
                            throw new InvalidOperationException
                                ("Неверные параметры соединения.");
                    }
                    catch (Exception ex)
                    {
                        DEBUG.Error(ex);
                        MessageBox.Show(ex.Message, "Ошибка соединения",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
            }
        }

        private void menuCSerialPort_Click(object sender, EventArgs e)
        {
            using (var frm = new formSerial(COM_thing))
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    try
                    {
                        Disconnect();
                        if (!ConnectSerial(COM_thing))
                            throw new InvalidOperationException
                                ("Не удалось подключиться.");
                    }
                    catch (Exception ex)
                    {
                        DEBUG.Error(ex);
                        MessageBox.Show(ex.Message, "Ошибка соединения",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
            }
        }

        void menuCSerialPortItem_Click(object sender, EventArgs e)
        {
            var s = sender as ToolStripMenuItem;
            if (s != null)
            {
                try
                {
                    Disconnect();
                    COM_thing.PortName = s.Text;
                    if (!ConnectSerial(COM_thing))
                        throw new InvalidOperationException
                            ("Не удалось подключиться.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка соединения",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void menuC_Delay_Click(object sender, EventArgs e)
        {
            if (Device == null) return;
            using (var dlg = new formDelay(Device.ioDelayValue))
            {
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Device.ioDelayValue = dlg.Delay;
                }
            }
        }

        private void menuCDisconnect_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        // VIEW

        private void menuV_Panel_Click(object sender, EventArgs e)
        {
            splitter.Panel2Collapsed ^= true;
        }

        private void menuVDebugConsole_Click(object sender, EventArgs e)
        {
            if (!DEBUG.Visible)
            {
                DEBUG.Show(this);
                DEBUG.Activate();
            }
            else DEBUG.Hide();
        }

        private void menuVSettings_Click(object sender, EventArgs e)
        {
            using (var dlg = new formSettings())
            {
                // setup

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // apply

                }
            }
        }

        // WRITE

        private void menuWWrite_Click(object sender, EventArgs e)
        {

        }

        private void menuWHistory_Click(object sender, EventArgs e)
        {

        }

        // TOP

        private void menuFile_DropDownOpening(object sender, EventArgs e)
        {
            menuFImport.Enabled = menuFExport.Enabled = Device != null;
        }

        private void menuConnect_DropDownOpening(object sender, EventArgs e)
        {
            bool linked = Device != null;
            menuCDisconnect.Enabled = linked;
            // serials;
            menuCSocket.Checked = tcp != null;
            menuCSerialPort.Checked = port != null;
            menuC_Delay.Visible = Control.ModifierKeys == Keys.Shift;
            // FILL SERIAL PORTS
            var menu = menuConnect.DropDownItems;
            int idx = menu.IndexOf(menuC_Bottom) + 1;
            while (menu.Count > idx)
                menu.RemoveAt(idx);
            string[] ports = SerialPort.GetPortNames();
            foreach (var p in ports)
                ((ToolStripMenuItem)menu.Add(p, null, menuCSerialPortItem_Click)).Checked
                    = p == COM_thing.PortName && COM_thing.IsOpen;
        }

        private void menuView_DropDownOpening(object sender, EventArgs e)
        {
            menuV_Panel.Checked = !splitter.Panel2Collapsed;
            menuVDebugConsole.Checked = DEBUG.Visible;
        }

        #endregion

        #region * Status - - - -

        /// <summary>
        /// Обновить статусную строку.
        /// </summary>
        void UpdateStatus()
        {
            bool ctlP = Device != null && !controlsInit;
            if (splitter.Panel2.Enabled != ctlP)
                splitter.Panel2.Enabled = ctlP;
            stErrors.Text = "Ошибок: " + errorCount;
            stErrors.DisplayStyle = errorState
                ? ToolStripItemDisplayStyle.ImageAndText
                : ToolStripItemDisplayStyle.Text;
            if (!errorState)
            {
                errorSequentialCount = 0;
                lblDespair.Hide();
            }
        }

        delegate void _msg(string msg);

        /// <summary>
        /// Сообщение в статусной строке.
        /// </summary>
        /// <param name="msg"></param>
        public void msg(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new _msg(msg), message);
                return;
            }

            DEBUG.Print(status.Text = string.Format("{0}> {1}", DateTime.Now.ToLongTimeString(), message));
        }

        /// <summary>
        /// Отметить ошибку.
        /// </summary>
        /// <param name="message"></param>
        public void MarkError(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new _msg(MarkError), message);
                return;
            }
            if (!string.IsNullOrEmpty(message)) msg(message);
            errorState = true;
            ++errorCount;
            if (++errorSequentialCount > 5) lblDespair.Show();
            UpdateStatus();
        }

        #endregion

        #region - Connection -

        Stream connection = null;
        SerialPort port = null;
        SafeNetworkStream tcp = null;

        bool Connect(Stream con)
        {
            try
            {
                errorCount = 0;
                controlsInit = true;

                connection = con;

                errorState = false;
                UpdateStatus();
                msg("Подключено.");
                DEBUG.Trace("[Connect]");

                return true;
            }
            catch (Exception ex)
            {
                MarkError("Ошибка подключения: " + ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connection = null;
            }
            return false;
        }

        bool ConnectEthernet(Uri u)
        {
            try
            {
                switch (u.Scheme.ToLowerInvariant())
                {
                    case "ip":
                        Device = new GsaRM(ModbusSerialMaster.CreateRtu(tcp = new SafeNetworkStream(u)));
                        connectEvents(Device);
                        return Connect(null);

                    case "tcp":
                        Device = new GsaRM(ModbusIpMaster.CreateIp(tcp = new SafeNetworkStream(u)));
                        //Device = new MikeDevice(ModbusIpMaster.CreateIp(new TcpClient(u.Host, u.Port)));
                        connectEvents(Device);
                        return Connect(null);

                    case "udp":
                        Device = new GsaRM(ModbusSerialMaster.CreateRtu(new UdpClient(u.Host, u.Port)));
                        connectEvents(Device);
                        return Connect(null);
                }
            }
            //catch { }
            finally { }
            return false;
        }

        public bool ConnectEthernet(string ur)
        {
            Uri u;
            if (Uri.TryCreate(ur, UriKind.Absolute, out u))
                return ConnectEthernet(u);
            return false;
        }

        bool ConnectSerial(SerialPort cm)
        {
            try
            {
                port = cm;
                if (!port.IsOpen) port.Open();
                Device = new GsaRM(ModbusSerialMaster.CreateRtu(port), true);
                connectEvents(Device);
                return Connect(cm.BaseStream);
            }
            catch { port = null; throw; }
            finally { }
            //return false;
        }

        public bool ConnectSerialNamed(string cm)
        {
            try
            {
                COM_thing.PortName = cm;
                return ConnectSerial(COM_thing);
            }
            catch (Exception ex)
            {
                DEBUG.Error(ex);
            }
            return false;
        }

        public void Disconnect()
        {
            try
            {
                var dev = Device;
                var c = connection;
                var t = tcp;
                var s = port;

                Device = null;
                connection = null;
                tcp = null;
                port = null;


                if (dev != null)
                {
                    //sip.AbortFlag = true;
                    dev.Update -= Device_Update;
                    dev.DebugStatus -= Device_DebugStatus;
                    dev.Dispose();
                    DEBUG.Trace("[Disconnect]");
                }
                else if (t != null) t.Dispose();
                else if (s != null) s.Close();
                else if (c != null) c.Dispose();
                UpdateStatus();

                msg("Отключено.");
            }
            catch (Exception ex)
            {
                DEBUG.Error(ex);
                MarkError("Ошибка отключения: " + ex.Message);
            }
        }

        void connectEvents(GsaRM dev)          //MikeDevice
        {
            dev.Update += Device_Update;
            dev.DebugStatus += new EventHandler<DeviceDebugEventArgs>(Device_DebugStatus);
            GsaRM gsa = dev as GsaRM;
            if( gsa != null )
            {
                gsa.Zone = StateSelector.SelectedIndex;
            }
        }

        private void COM_thing_ErrorReceived(object sender, System.IO.Ports.SerialErrorReceivedEventArgs e)
        {
            try
            {
                COM_thing.DiscardOutBuffer();
                COM_thing.DiscardInBuffer();
            }
            catch { }
        }

        #endregion

        #region - Update -

        DateTime lastAxis = DateTime.MinValue;
        private void poller_Tick(object sender, EventArgs e)
        {
            if (Device != null && !Trend.Blocked)
            {
                SyncControls();
                Device.UpdateAsync();
            }
        }

        void Trend_ScaleReset(object sender, EventArgs e)
        {
            if (Device != null && !InvokeRequired)
            {
                Device.AbortFlag = true;
                poller_Tick(sender, e);
            }
        }

        void Device_Update(object sender, DeviceEventArgs e)
        {
            if (InvokeRequired) Invoke(new Action<DeviceEventArgs>(_dev_update), e);
            else _dev_update(e);
        }

        void Device_DebugStatus(object sender, DeviceDebugEventArgs e)
        {
            if (e == null || e.DebugStatus == null) return;
            try
            {
                if (!InvokeRequired) stStep.Text = e.DebugStatus;
                else Invoke((Action)delegate { stStep.Text = e.DebugStatus; }); // FIXME
            }
            catch { } // safe debug
        }
        bool start_axis = false;
        void _dev_update(DeviceEventArgs e)
        {
            try
            {

                var curves = graph.GraphPane.CurveList;
                
                var linep  = curves["V+"] as LineItem;
                var linem  = curves["V-"] as LineItem;
                var linep2 = curves["S+"] as LineItem;
                var linem2 = curves["S-"] as LineItem;
                var linetst = curves["AA"] as LineItem;
                var linetst2 = curves["BB"] as LineItem;



                graph.IsSynchronizeYAxes = false;
                if (!start_axis)
                {
                    graph.GraphPane.YAxisList.Clear();

                    int axis1 = graph.GraphPane.AddYAxis("Voltage");


                    graph.GraphPane.YAxisList[axis1].Scale.Min = -3.3;
                    graph.GraphPane.YAxisList[axis1].Scale.Max = 3.3;
                    graph.GraphPane.YAxisList[axis1].Scale.MajorStep = 0.2;
                    // graph.GraphPane.YAxisList[axis1].Cross = graph.GraphPane.XAxis.Scale.Max;

                    //graph.GraphPane.YAxisList[axis1].Color = Color.Red;
                    linep.YAxisIndex = axis1;
                    linep2.YAxisIndex = axis1;

                    int axis2 = graph.GraphPane.AddYAxis("Temperature");

                    graph.GraphPane.YAxisList[axis2].Scale.Min = -20;
                    graph.GraphPane.YAxisList[axis2].Scale.Max = 40;
                    graph.GraphPane.YAxisList[axis2].Scale.MajorStep = 0.2;
                    graph.GraphPane.YAxisList[axis2].Color = Color.White;
                    
                    graph.GraphPane.YAxisList[axis2].MajorTic.Color = Color.Red;
                    graph.GraphPane.YAxisList[axis2].MinorTic.Color = Color.Red;


                    linem.YAxisIndex = axis2;
                    linem2.YAxisIndex = axis2;

                    int axis3 = graph.GraphPane.AddYAxis("Humidity");
                    graph.GraphPane.YAxisList[axis3].Scale.Min = 0;
                    graph.GraphPane.YAxisList[axis3].Scale.Max = 100;
                    graph.GraphPane.YAxisList[axis3].Scale.MajorStep = 0.2;
                    graph.GraphPane.YAxisList[axis3].Color = Color.Blue;
                    graph.GraphPane.YAxisList[axis3].MajorTic.Color = Color.Blue;
                    graph.GraphPane.YAxisList[axis3].MinorTic.Color = Color.Blue;
                    graph.GraphPane.YAxisList[axis3].MinorGrid.Color = Color.Red;
                    linetst.YAxisIndex = axis3;
                    linetst2.YAxisIndex = axis3;
                    start_axis = true;
                }
                if (linep == null || linem == null || linep2 == null || linem2 == null || linetst == null || linetst2 == null)
                {
                    MarkError("Ошибка логики графиков.");
                    return;
                }

                if (e.Error != null)
                {
                    MarkError("Ошибка устройства: " + e.Error.Message);
#if DEBUG
                    DEBUG.Trace(e.Error.StackTrace.Split(new char[] { '\r', '\n' }, 2)[0]);
#endif
                    if (RECORDING.IsRunning)
                    {
                        double timeshot = RECORDING.ElapsedMilliseconds / 1000d;
                        var nanopoint = new PointPair(timeshot, double.NaN);
                        // add
                        linep.AddPoint(nanopoint);
                        linem.AddPoint(nanopoint);
                        linep2.AddPoint(nanopoint);
                        linem2.AddPoint(nanopoint);
                        linetst.AddPoint(nanopoint);
                        linetst2.AddPoint(nanopoint);

                    }

                    return;
                }

                if (controlsInit) WriteControls();
                SyncFlags();
                
                var pts = e.Data;
            //    pts[0] = 1;
            //    pts[1] = 2;
                if (pts != null)
                {
                    if (RECORDING.IsRunning)
                    {
                        double timeshot = RECORDING.ElapsedMilliseconds / 1000d;
                        // add
                        linep.AddPoint(timeshot, pts[0]);
                        linem.AddPoint(timeshot, pts[1]);
                        linep2.AddPoint(timeshot, pts[0]);
                        linem2.AddPoint(timeshot, pts[1]);
                        linetst.AddPoint(timeshot, pts[2]);
                        linetst2.AddPoint(timeshot, pts[2]);
                        linem.Line.Style = System.Drawing.Drawing2D.DashStyle.Dot;
                        linem2.Line.Style = System.Drawing.Drawing2D.DashStyle.Dot;
                        linetst.Line.Style = System.Drawing.Drawing2D.DashStyle.Dot;
                        linetst2.Line.Style = System.Drawing.Drawing2D.DashStyle.Dot;
                    }

                    // value hack
                    Trend.textPos.Text = pts[1].ToString("0.0" + "°C T ");
                    
                    Trend.textNeg.Text = pts[2].ToString("0.0" +"'%' RH ");

                    // id hack
                    if (stID.Text != Device.ID)
                    {
                        if (Device.ID != null)
                            msg("Подключено устройство " + Device.ID);
                        stID.Text = Device.ID ?? "???";
                    }

                    if (stMD5.Text != ("MD5:"+Device.MD5))
                    {
                        if (Device.MD5 != null)
                            msg("MD5: " + Device.MD5);
                        stMD5.Text = "MD5: "+Device.MD5 ?? "???";
                    }
                    ushort flR = Device.FlagsReadOnly;
                    // threshold hack


                    double ssdf = Device.getGUIvalue(4);
                    ssdf = Device.getGUIvalue(7);
                    bool tp = (pts[0]) > Device.getGUIvalue(4);
                    bool tn = pts[0] < -1*Device.getGUIvalue(5);
                    bool dp = (pts[0]) > Device.getGUIvalue(7);
                    bool dn = pts[0]  < -1*Device.getGUIvalue(8);
                    lblDangerPositive.Text = dp ? "+ Опасно" : "+ Порог";
                    lblDangerNegative.Text = dn ? "- Опасно" : "- Порог";
                    lblDangerPositive.Visible = tp || dp;
                    lblDangerNegative.Visible = tn || dn;
                    lblReady.Visible = (flR & (int)Mike.aFlagsR.Ready) != 0;
                    try
                    {
                        var vertp = graph.GraphPane.GraphObjList["P+"].Location;
                        vertp.Y = Device.getGUIvalue(4);
                        vertp = graph.GraphPane.GraphObjList["P-"].Location;
                        vertp.Y = -Device.getGUIvalue(5);
                        vertp = graph.GraphPane.GraphObjList["D+"].Location;
                        vertp.Y = Device.getGUIvalue(7);
                        vertp = graph.GraphPane.GraphObjList["D-"].Location;
                        vertp.Y = -Device.getGUIvalue(8);      
                    }
                    catch { }

                    // update axis
                    var now = DateTime.Now;
                    if ((now - lastAxis).TotalSeconds > 2)
                    {
                        lastAxis = now;
                        graph.AxisChange();
                    }

                    if (!Trend.Blocked)
                        //graph.Invalidate();
                        graph.Refresh(); // forced refresh for better visibility
                    stStep.Text = "";
                    errorState = false;
                    UpdateStatus();
                }
                else MarkError("Нет ответа.");
            }
            catch (Exception ex)
            {
                MarkError("Ошибка обновления: " + ex.Message);
            }
        }

        #endregion

        #region * Controls Binding -

        Dictionary<byte, Control> guibinding;
        Dictionary<Control, Control> labelbinding;
        Dictionary<byte, System.Windows.Forms.Label> inputbinding;
        Dictionary<byte, byte> igbinding;

        /// <summary>
        /// Регистрация привязок элементов.
        /// <remarks>Должно соответствовать номерам в Mike.cs</remarks>
        /// </summary>

        void RegisterControls()
        {
            // we love hard-coded values


            //byte numUm_reg = state_select  + 1;
            //int numF_reg = state_select * 12 + 2;
            //int numEcm_reg = state_select * 12 + 3;




            guibinding = new Dictionary<byte, Control>
            {
                //{00, numPower},

                
                {01, numUm},
                {02, numF},
                {03, numEcm},



                {04, numTP},
                {05, numTN},
                {06, numKmax},
                {07, numDP},
                {08, numDN},
                {09, numFilter},
                {15, numK2max},
                {10, Zone_timer},
            };

            labelbinding = new Dictionary<Control, Control>
            {
                {numUm, lblUm},
                {numF, lblF},
                {numEcm, lblEcm},
                {numTP, lblTP},
                {numTN, lblTN},
                {numY, lblY},
                {numKmax, lblKmax},
                {numDP, lblDP},
                {numDN,lblDN},
                {numFilter, lblFilter},
                {numK2max, lblK2max},
                {Zone_timer, Zonelbl},
            };

            inputbinding = new Dictionary<byte,System.Windows.Forms.Label>
            {
                {3, inputUm},
                {6, inputF},
                {4, InputEcm},
                {5, inputT},
            };

            igbinding = new Dictionary<byte, byte>
            {
                {3, 01}, // Um
                {4, 03}, // Ecm
            };

            var nTB = new TrackBar[] { numUm, numF, numEcm, numTP, numTN, numY, numKmax, numDP, numDN, numFilter, numK2max, Zone_timer };
            var bUp = new Button[] { cmdUmUp, cmdFUp, cmdEcmUp, cmdTPUp, cmdTNUp, cmdYUp, cmdKmaxUp, cmdDPUp, cmdDNUp, cmdFilterUp, cmdK2maxUp, cmdZone_timerUp };
            var bDn = new Button[] { cmdUmDown, cmdFDown, cmdEcmDown, cmdTPDown, cmdTNDown, cmdYDown, cmdKmaxDown, cmdDPDown, cmdDNDOwn, cmdFilterDown, cmdK2maxDown, cmdZone_timerDown };

            var settings = Mike.Initialize();
            // event handlers
            //foreach (var c in guibinding)
            foreach(var p in settings)
            {
                Control c;

                //if(guibinding.TryGetValue(p.ID, out c))
                if (guibinding.TryGetValue(GetDevId(p), out c))
                if (c is TrackBar)
                {
                    var tbar = (TrackBar)c;
                    int rawrange = p.Raw1 - p.Raw0;
                    double engrange = p.Eng1 - p.Eng0;
                    tbar.Minimum = p.Raw0;
                    tbar.Maximum = p.Raw1;
                    tbar.TickFrequency = rawrange / 20;
                    tbar.SmallChange = (int) (rawrange / (engrange > 10 ? engrange : engrange * 100));
                    tbar.LargeChange = rawrange / 20;
                    tbar.ValueChanged += new EventHandler(Control_ValueChanged);
                    tbar.Tag = p;
                    // hackery for value input
                    var thisP = p;
                    labelbinding[tbar].Click += new EventHandler(delegate(object se, EventArgs e)
                    {
                        using (var dlg = new formBulbulator())
                        {
                            var pp = Device.getGUIparam(GetDevId(thisP));
                            dlg.MinValue = pp.Eng0;
                            dlg.MaxValue = pp.Eng1;
                            dlg.VALUE = pp.Value;
                            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                pp.Value = dlg.VALUE;
                                tbar.Value = pp.WantedValue;
                            }
                        }
                    });
                }
            }

            // supreme hackery for up/down button trackbar changers
            for (int i = 0, l = nTB.Length; i < l; ++i)
            {
                var thisTB = nTB[i];
                bUp[i].Click += new EventHandler(delegate(object se, EventArgs e)
                {
                    var sca = thisTB.SmallChange;
                    if (Control.ModifierKeys == Keys.Shift) sca /= 10;
                    else if (Control.ModifierKeys == Keys.Control) sca *= 10;
                    thisTB.Value = (int)Math.Min(thisTB.Maximum, 
                        thisTB.Minimum + (thisTB.Value - thisTB.Minimum + sca) / sca * sca );
                    thisTB.Focus();
                });
                bDn[i].Click += new EventHandler(delegate(object se, EventArgs e)
                {
                    var sca = thisTB.SmallChange;
                    if (Control.ModifierKeys == Keys.Shift) sca /= 10;
                    else if (Control.ModifierKeys == Keys.Control) sca *= 10;
                    thisTB.Value = (int)Math.Max(thisTB.Minimum,
                        thisTB.Minimum + (thisTB.Value - thisTB.Minimum - sca)/sca*sca);
                    thisTB.Focus();
                });
            }
            
            // damn spaghetty, FIXME: FATAL FLAW: zero start required
            cmdTPUpSmall.Click += new EventHandler(delegate(object se, EventArgs e)
            {
                var con = numTP;
                var sca = con.SmallChange / 10;
                con.Value = (int)Math.Min(con.Maximum,
                    (Math.Round((double)(con.Value + sca)
                    / (double)sca)) * sca);
                con.Focus();
            });
            cmdTPDownSmall.Click += new EventHandler(delegate(object se, EventArgs e)
            {
                var con = numTP;
                var sca = con.SmallChange / 10;
                con.Value = (int)Math.Max(con.Minimum,
                    (Math.Round((double)(con.Value - sca)
                    / (double)sca)) * sca);
                con.Focus();
            });
            cmdTNUpSmall.Click += new EventHandler(delegate(object se, EventArgs e)
            {
                var con = numTN;
                var sca = con.SmallChange / 10;
                con.Value = (int)Math.Min(con.Maximum,
                    (Math.Round((double)(con.Value + sca)
                    / (double)sca)) * sca);
                con.Focus();
            });
            cmdTNDownSmall.Click += new EventHandler(delegate(object se, EventArgs e)
            {
                var con = numTN;
                var sca = con.SmallChange / 10;
                con.Value = (int)Math.Max(con.Minimum,
                    (Math.Round((double)(con.Value - sca)
                    / (double)sca)) * sca);
                con.Focus();
            });
            cmdDPUpSmall.Click += new EventHandler(delegate(object se, EventArgs e)
            {
                var con = numDP;
                var sca = con.SmallChange / 10;
                con.Value = (int)Math.Min(con.Maximum,
                    (Math.Round((double)(con.Value + sca)
                    / (double)sca)) * sca);
                con.Focus();
            });
            cmdDPDownSmall.Click += new EventHandler(delegate(object se, EventArgs e)
            {
                var con = numDP;
                var sca = con.SmallChange / 10;
                con.Value = (int)Math.Max(con.Minimum,
                    (Math.Round((double)(con.Value - sca)
                    / (double)sca)) * sca);
                con.Focus();
            });
            cmdDNUpSmall.Click += new EventHandler(delegate(object se, EventArgs e)
            {
                var con = numDN;
                var sca = con.SmallChange / 10;
                con.Value = (int)Math.Min(con.Maximum,
                    (Math.Round((double)(con.Value + sca)
                    / (double)sca)) * sca);
                con.Focus();
            });
            cmdDNDownSmall.Click += new EventHandler(delegate(object se, EventArgs e)
            {
                var con = numDN;
                var sca = con.SmallChange / 10;
                con.Value = (int)Math.Max(con.Minimum,
                    (Math.Round((double)(con.Value - sca)
                    / (double)sca)) * sca);
                con.Focus();
            });
        }

        byte GetDevId(DeviceParameter p)
        {
            if( p == null )
                return 0;

            if (Device == null)
                return p.ID;

            return Device.GetUI_Id(p);
        }

        void Control_ValueChanged(object sender, EventArgs e)
        {
            // FIXME: assumptions
            Control lbl = labelbinding[(Control)sender];
            double v = double.NaN;
            var t = sender as TrackBar;
            DeviceParameter dp = null;
            foreach (var a in guibinding)
                if (a.Value == sender)
                { v = Device.getGUIvalue(a.Key); dp = Device.getGUIparam(a.Key); break; }
            lbl.Text = string.Format(lbl.Tag as string ?? "{0}",
                t.Enabled && dp != null ? dp.Decode((ushort)t.Value) : v);
        }

        /// <summary>
        /// Первоначальная инициализация значениями из устройства.
        /// </summary>
        void WriteControls()
        {
            StringBuilder sb = new StringBuilder(), sbt = new StringBuilder();
            foreach (var p in Device.Settings)      
            {
                Control c;
                if (guibinding.TryGetValue(GetDevId(p), out c))
                    try
                    {
                        if (c is TrackBar)
                        {
                            var t = (TrackBar)c;
                            if (p.RawValue < t.Minimum)
                            {
                                t.Value = t.Minimum;
                                if (!(p.Value >= p.Eng0))
                                {
                                    sb.AppendFormat("#{0}={1}<{2}; ", GetDevId(p), p.RawValue, t.Minimum);
                                    sbt.AppendFormat("#{0}={1}<{2} {3}\r\n", GetDevId(p), p.RawValue, t.Minimum, p.Name);
                                }
                            }
                            else if (p.RawValue > t.Maximum)
                            {
                                t.Value = t.Maximum;
                                if (!(p.Value <= p.Eng1))
                                {
                                    sb.AppendFormat("#{0}={1}>{2}; ", GetDevId(p), p.RawValue, t.Maximum);
                                    sbt.AppendFormat("#{0}={1}<{2} {3}\r\n", GetDevId(p), p.RawValue, t.Maximum, p.Name);
                                }
                            }
                            else
                            {
                                t.Value = p.RawValue; // (int)Math.Round(p.Value);
                            }
                            Control_ValueChanged(c, null); // force update
                        }
                    }
                    catch (Exception ex)
                    {
                        sb.AppendFormat("#{0}={1}(!{2}); ", GetDevId(p), p.RawValue, ex.GetType().ToString());
                        sbt.AppendFormat("#{0}={1} {3} (!{2})\r\n", GetDevId(p), p.RawValue, ex.GetType().ToString(), p.Name);
                    }
            }

            // oaking flags
            ushort flW = Device.FlagsWriteable;
            //cmdWrite.Checked = (flW & (int)Mike.aFlagsW.WriteFlash) != 0;
            cmdWrite.Checked = false;
            cmdKmax.Checked = (flW & (int)Mike.aFlagsW.Kmax) != 0;
            cmdFilter.Checked = (flW & (int)Mike.aFlagsW.Filter) != 0;
            cmdLock.Checked = (flW & (int)Mike.aFlagsW.Lock) != 0;
            cmdEnum.Checked = (flW & (int)Mike.aFlagsW.Enum) != 0;
            cmdControl.Checked = (flW & (int)Mike.aFlagsW.Control) != 0;

            // FATAL: solution for out of range
            if (sb.Length > 0)
            {
                MarkError("Неверные исходные значения: " + sb.ToString());
                MessageBox.Show("Неверные исходные значения параметров:\r\n"
                    + sbt.ToString() + "\r\n\r\nДанные значения будут заменены допустимыми.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            controlsInit = false;
            UpdateStatus();
        }

        /// <summary>
        /// Синхронизировать значения параметров с элементами управления.
        /// </summary>
        void SyncControls()
        {
            // enabled trackbars
            foreach (var p in Device.Settings)
            {
                Control c;
                if (guibinding.TryGetValue(GetDevId(p), out c))
                    if (c is TrackBar)
                    {
                        var t = ((TrackBar)c);
                        if (t.Enabled) p.WantedValue = (ushort)t.Value;
                    }
            }

            // flags
            ushort flW = Device.FlagsWriteable;
            //MikeAIG.SetFlag(ref flW, Mike.aFlagsW.Mode, modeAirOut.Checked);
            MikeDevice.SetFlag(ref flW, Mike.aFlagsW.WriteFlash, cmdWrite.Checked);
            //MikeDevice.SetFlag(ref flW, Mike.aFlagsW.SuntSet, shuntOnOff.Checked);
            MikeDevice.SetFlag(ref flW, Mike.aFlagsW.Kmax, cmdKmax.Checked);
            MikeDevice.SetFlag(ref flW, Mike.aFlagsW.Filter, cmdFilter.Checked);
            MikeDevice.SetFlag(ref flW, Mike.aFlagsW.Lock, cmdLock.Checked);
            MikeDevice.SetFlag(ref flW, Mike.aFlagsW.Enum, cmdEnum.Checked);
            MikeDevice.SetFlag(ref flW, Mike.aFlagsW.Control, cmdControl.Checked);
            Device.FlagsWriteable = flW;

        }

        void SyncFlags()
        {
            // status
            ushort flR = Device.FlagsReadOnly;
            //flagAmplifierOK.BackColor = (flR & (int)Mike.aFlagsR.AmplifierOK) != 0 ? Color.LightGreen : Color.White;
            if (lblMaterial.Visible = (flR & (int)Mike.aFlagsR.Iprit) != 0)
                lblMaterial.Text = "Опасно Иприт";
            else if (lblMaterial.Visible =  (flR & (int)Mike.aFlagsR.Phosphor) != 0)
                lblMaterial.Text = "Опасно Фосфор Органика";
            else if (lblMaterial.Visible = (flR & (int)Mike.aFlagsR.IpritThr) != 0)
                lblMaterial.Text = "Порог Иприт";
            else if (lblMaterial.Visible = (flR & (int)Mike.aFlagsR. PhosphorThr) != 0)
                lblMaterial.Text = "Порог Фосфор Органика";
            else lblMaterial.Visible = false;

            // sensors
            //sensorT1.Visible = (flR & (int)Mike.aFlagsR.Temperature_sensor1) != 0;
            foreach (var l in Device.Inputs)
            {
                System.Windows.Forms.Label lab;
                if(inputbinding.TryGetValue(GetDevId(l), out lab))
                {
                    lab.Text = string.Format(lab.Tag as string ?? "{0}", l.Value);
                    if (igbinding.ContainsKey(GetDevId(l)))
                    {
                        double real = Device.getGUIvalue(igbinding[GetDevId(l)]);
                        // FIXME: DUMB USER
                        lab.BackColor = (Math.Abs(l.Value - real) >
                            (GetDevId(l) == 3 ? 0.2 : (real * 0.05))) // Ecm damn exception
                            ? (flashActive ? Color.Red : Color.MistyRose) :
                            Color.FromKnownColor(KnownColor.Window);
                    }
                }
            }


            
            // write success
            flR = Device.FlagsWriteable;

            if ((flR & (int)Mike.aFlagsW.WriteFlash) != 0)
            {
                cmdWrite.Checked = false;
                flR &= unchecked((ushort)~(Mike.aFlagsW.WriteFlash));
                Device.FlagsWriteable = flR;
                msg("Параметры успешно сохранены.");
            }

        }

        private void Control_CheckedChanged(object sender, EventArgs e)
        {
            var rad = sender as RadioButton;
            if (rad != null)
            {
                rad.BackColor = rad.Checked ? Color.FromKnownColor(KnownColor.Highlight)
                    : Color.FromKnownColor(KnownColor.ButtonFace);
                rad.ForeColor = rad.Checked ? Color.FromKnownColor(KnownColor.HighlightText)
                    : Color.FromKnownColor(KnownColor.ControlText);
                return;
            }
            var chk = sender as CheckBox;
            if (chk != null)
            {
                chk.BackColor = chk.Checked ? Color.FromKnownColor(KnownColor.Highlight)
                    : Color.FromKnownColor(KnownColor.ButtonFace);
                chk.ForeColor = chk.Checked ? Color.FromKnownColor(KnownColor.HighlightText)
                    : Color.FromKnownColor(KnownColor.ControlText);
                return;
            }
        }

        /// <summary>
        /// Установить флаг инициализации графических элементов управления 
        /// значениями параметров из устройства.
        /// <remarks>
        /// Следует помнить, что в случае несовпадения значений с заданными
        /// происходит попытка их перезаписи на каждом цикле.
        /// </remarks>
        /// </summary>
        private void cmdREread_Click(object sender, EventArgs e)
        {
            controlsInit = true;
        }


        bool flashActive = false;
        private void flasher_Tick(object sender, EventArgs e)
        {
            lblDangerPositive.BackColor = flashActive ? Color.Yellow : Color.White;
            lblDangerNegative.BackColor = flashActive ? Color.Yellow : Color.White;
            lblMaterial.BackColor = flashActive ? Color.Yellow : Color.White;
            flashActive ^= true;
        }

        private void numY_Scroll(object sender, EventArgs e)
        {
            double v = numY.Value / 1000d;
            var sc = graph.GraphPane.YAxis.Scale;
            if (v > 0)
            {
                Trend.Y0 = -(Trend.Y1 = v);
                sc.Min = Trend.Y0;
                sc.Max = Trend.Y1;
            }
            else
            {
                Trend.Y0 = Trend.Y1 = 0;
                sc.MinAuto = sc.MaxAuto = true;
            }
            lblY.Text = v.ToString("0.000");
        }

        private void lblY_Click(object sender, EventArgs e)
        {
            using (var dlg = new formBulbulator())
            {
                dlg.MinValue = 0;
                dlg.MaxValue = 3.3;
                dlg.VALUE = Trend.Y1;
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    numY.Value = (int)(dlg.VALUE * 1000);
                    numY_Scroll(null, e);
                    graph.Invalidate();
                }
            }
        }

        private void lblDespair_Click(object sender, EventArgs e)
        {
            lblDespair.Hide();
        }

        #endregion

        #region - Lua -

        public void ReRead()
        {
            controlsInit = true;
        }

        public void DeviceUpdate()
        {
            poller_Tick(this, null);
        }

        public object DeviceRead(ushort reg, byte cnt)
        {
            var d = Device;
            if (d == null) return null;
            ushort[] res = d.Master.ReadHoldingRegisters(d.DeviceID, reg, cnt);
            if (res.Length == 1) return res[0];
#if LUA
            var dres = LuaInterpreter.DoString("return{}")[0] as LuaTable; // so much of HACK
            for (int i = 0, l = res.Length; i < l; ++i)
                dres[i + 1] = res[i];
            return dres;
#else
            return res;
#endif
        }

        public void DeviceWrite(ushort reg, ushort va)
        {
            var d = Device;
            if (d == null) return;
           
            d.Master.WriteSingleRegister(d.DeviceID, reg, va);


        }

        public void SetPoller(double s)
        {
            poller.Enabled = s > 0;
            DEBUG.Trace("Poller: " + (s > 0 ? "enabled" : "disabled"));
        }

        public void SetInterval(double s)
        {
            poller.Interval = (int)(s *1000);
            DEBUG.Trace("Poller interval: " + s);
        }

        public string HEX(double s)
        {
            if (double.IsNaN(s)) return ("nan");
            return "0x" + ((int)s).ToString("X4");
        }

        public void Sleep(double s)
        {
            if (!(s > 1)) return ;
            System.Threading.Thread.Sleep((int)s);
        }

        public object test()
        {
            return "Test";
        }

        #endregion

        #region - Main Record Logic Regional Block -

        System.Diagnostics.Stopwatch RECORDING = new System.Diagnostics.Stopwatch();

        /// <summary>
        /// Большая кнопка смены статуса регистрации и отображения графиков.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdStartStop_Click(object sender, EventArgs e)
        {
            if (RECORDING.IsRunning)
            {
                RecordStop();
                cmdStartStop.Text = "Старт";
            }
            else
            {
                RecordStart();
                cmdStartStop.Text = "Стоп";
            }
        }

        public bool RecordStart()
        {
            if (Device == null) return false;

            // clear
            var vp  = graph.GraphPane.CurveList["V+"];
            var vm  = graph.GraphPane.CurveList["V-"];
            var vp2 = graph.GraphPane.CurveList["S+"];
            var vm2 = graph.GraphPane.CurveList["S-"];
            var linetstc = graph.GraphPane.CurveList["AA"]; 
            var linetst2c = graph.GraphPane.CurveList["BB"]; 



            vp.Clear();
            vm.Clear();
            vp2.Clear();
            vm2.Clear();
            linetstc.Clear();
            linetst2c.Clear();

            // start
            RECORDING.Restart();

            return true;
        }

        public void RecordStop()
        {
            // stop
            RECORDING.Stop();
        }


        #endregion
      //  ushort state_select = 0;
        public void StateSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Device == null)
                return;

            Device.Zone = StateSelector.SelectedIndex;

            Device.init = true;
            controlsInit = true;

            WriteControls();
            SyncControls();
            Device.UpdateAsync();

            Disconnect();
            ConnectSerial(COM_thing);


            var d = Device;
            d.Master.Transport.Retries = 4;
            


            ushort timer_reg_num = 0x0a;

            ushort zone_reg_shift = 0x15;

            ushort[] Buf = null;
            try
            {
                d.Master.WriteSingleRegister(d.DeviceID, 0x5C, (ushort)Device.Zone);                ///////// отправляем номер выбранной "зоны" в прибор, если не стоит галка "откл.", никак не влияет на работу прибора.

                //while (Buf == null)
                //{
                //    Buf = d.Master.ReadHoldingRegisters(d.DeviceID, (ushort)(timer_reg_num + (zone_reg_shift * StateSelector.SelectedIndex)), 1);            ///////// читаем выставленное "время зоны" из прибора
                    
                //};
                //Zone_timer.Value = Buf[0];
                //Zone_timer.Refresh();
            }
            catch( Exception ex)
            {
                this.Text = (string.IsNullOrEmpty(ex.Message)) ? "Form1" : ("Form1... Error: " + ex.Message);
            }




            //try
            //{
            //    //(ushort)(timer_reg_num + (zone_reg_shift * StateSelector.SelectedIndex))
            //    ushort[] Buf = d.Master.ReadHoldingRegisters(d.DeviceID, (ushort)(timer_reg_num + (zone_reg_shift * StateSelector.SelectedIndex)), 1);            ///////// читаем выставленное "время зоны" из прибора
            //    Zone_timer.Value = Buf[0];
            //    Zone_timer.Refresh();
                
            //}
            //catch (Exception ex)
            //{
            //    this.Text = (string.IsNullOrEmpty(ex.Message)) ? "Form1" : ("Form1... Error: " + ex.Message);
            //}


            //if (StateSelector.SelectedIndex == 0)
            //{



            //    Device.Zone = 0;
            //   state_select = 0;

            //    numUm_reg = 1;
            //    numF_reg = 2;
            //    numEcm_reg = 3;
            //};




            //if (StateSelector.SelectedIndex == 1)
            //{
            //    Device.Zone = 1;
            //    state_select = 1;
            //    numUm_reg = 13;
            //    numF_reg = 14;
            //    numEcm_reg = 15;
            //};

            //if (StateSelector.SelectedIndex == 2)
            //{
            //    Device.Zone = 2;
            //    state_select = 2;                          
            //    numUm_reg = 25;
            //    numF_reg = 26;
            //    numEcm_reg = 27;
            //};


        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            var d = Device;

            ushort zone_reg = 0x5B;
            ushort zone_ch_on = 0x01;
            ushort zone_ch_off = 0x00;




            if (checkBox1.CheckState == CheckState.Checked)
                try
                {
                    d.Master.WriteSingleRegister(d.DeviceID, zone_reg, zone_ch_on);
                }
                catch( Exception ex)
                {
                    this.Text = (string.IsNullOrEmpty(ex.Message)) ? "Form1" : ("Form1... Error: " + ex.Message);
                }
                
            else
                try
                {
                    d.Master.WriteSingleRegister(d.DeviceID, zone_reg, zone_ch_off);
                }
                catch (Exception ex)
                {
                    this.Text = (string.IsNullOrEmpty(ex.Message)) ? "Form1" : ("Form1... Error: " + ex.Message);
                }



        }

        private void Zone_timer_Scroll(object sender, EventArgs e)
        {

           // Zone_delay.Text = ((float)(Zone_timer.Value) / 2).ToString();

        }

        private void Zone_timer_MouseUp(object sender, MouseEventArgs e)
        {
            //ushort timer_reg_num = 0x0a;

            //ushort zone_reg_shift = 0x15;

            //var d = Device;





            //try
            //{
            //    d.Master.WriteSingleRegister(d.DeviceID, (ushort)(timer_reg_num + (zone_reg_shift * StateSelector.SelectedIndex)), (ushort)Zone_timer.Value);
            //}
            //catch (Exception ex)
            //{
            //    this.Text = (string.IsNullOrEmpty(ex.Message)) ? "Form1" : ("Form1... Error: " + ex.Message);
            //}


        }

        private void scroller_Paint(object sender, PaintEventArgs e)
        {

        }
        
        private void select_addr_SelectedIndexChanged(object sender, EventArgs e)
        {


           
            Device.DeviceID = (byte)(select_addr.SelectedIndex + 1);


        }

        //private void shuntOnOff_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (shuntOnOff.Checked)
        //    {
        //        var d = Device;
        //        if (d == null) return;
        //        try
        //        {
        //            d.Master.WriteSingleRegister(d.DeviceID, 0, 2);
        //        }
        //        catch { }
        //    }
        //    else
        //    {
        //        var d = Device;
        //        if (d == null) return;
        //        try
        //        {
        //            d.Master.WriteSingleRegister(d.DeviceID, 0, 0);
        //        }
        //        catch { }

        //    }
        //}
    }
}
