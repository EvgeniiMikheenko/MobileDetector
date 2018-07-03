using common.plugins;
using common.utils.Files;
using Modbus.Device;
using PidLibrary;
using PIDLibrary.Pravate;
using serialPortLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Win32;

namespace SipTest
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			Init();
		}

		//-------------------------------------------------------------------------
		#region Data
		//
        const int UpdateInterval = 500;
		const ushort ReadMaxLength = 125;
        const ushort HostControlReg0PidTempResetBit         = (ushort)(1 << 0);
        const ushort HostControlReg0PidFlowTotalResetBit    = (ushort)(1 << 1);
        const ushort HostControlReg0PidFlowInResetBit       = (ushort)(1 << 2);
        const ushort HostControlReg0PidFactorsTempChangeBit = (ushort)(1 << 3);
        const ushort HostControlReg0PidFactorsFlowTotalChangeBit = (ushort)(1 << 4);
        const ushort HostControlReg0PidFactorsFlowInChangeBit = (ushort)(1 << 5);

		const string ParamFileName = "serialPortparam.xml";
		string m_appDirFullName;
		string m_paramFileFullName;

		byte m_sipAddr = 1;

		BackgroundWorker m_bgwWorker;

		SerialPortParams m_spParams;

		SerialPort m_serialPort;

		int m_readTimeout;
		/// <summary>
		/// Таймаут приема
		/// </summary>
		public int ReadTimeout
		{
			get { return m_readTimeout; }
			set
			{
				m_readTimeout = value;
				UpdateTimeouts();
			}
		}

		int m_writeTimeout;
		/// <summary>
		/// Таймаут передачи
		/// </summary>
		public int WriteTimeout
		{
			get { return m_writeTimeout; }
			set
			{
				m_writeTimeout = value;
				UpdateTimeouts();
			}
		}

		bool m_isInit;

		IModbusSerialMaster mbMaster;

		enum WorkType
		{
			Idle,
			Work
		}

		WorkType m_wrkType;

		int m_tempGraphicIndex = -1;
		int m_spGraphicIndex = -1;
		int m_pwrGraphicIndex = -1;

        List<int> m_t1GraphIndexes = new List<int>(4);
        List<int> m_t2GraphIndexes = new List<int>(4);

        List<int> m_f1GraphIndexes = new List<int>(4);
        List<int> m_f2GraphIndexes = new List<int>(4);

		int m_flowSpIndex = -1;
		int m_flowpvIndex = -1;
		int m_totalFlowSpIndex = -1;
		int m_totalFlowPvIndex = -1;

		SipRegisters m_sipRegs = new SipRegisters();
		ushort m_regStart = 0;
        ushort m_regsCount = 8515;//1281;//8515;

        List<ModbusIOEventArgs> m_ioList = new List<ModbusIOEventArgs>(1024);

        bool m_deviceConnected = false;
        string m_deviceInfo = null;

        bool m_isDeviceWritePasswordNeed = false;
        ushort m_deviseInfoSize = 0;

        const ushort InfoSizeRegNum = 60000;
        const ushort InfoRegNum = 60001;
        const ushort PasswordRegNum = 59950;

        const string DevicePassword = "LGAR MODBUS PASSWORD";

        bool m_isFirstRead = true;

        PidParamsEventArgs Pid1EA = new PidParamsEventArgs();
        PIDFactors m_pidFactors1 = new PIDFactors();

        PidParamsEventArgs PidFlowTotalEA = new PidParamsEventArgs();
        PIDFactors m_pidFactorsFt = new PIDFactors();

        PidParamsEventArgs PidFlowInEA = new PidParamsEventArgs();
        PIDFactors m_pidFactorsFi = new PIDFactors();

        List<float> m_analogData = new List<float>(128);
        int m_sensorsCount = 0;
        int m_analogDataCount = 0;

        List<controls.MGraphic> m_graphsList = new List<controls.MGraphic>();
		//
		#endregion //Data
		//-------------------------------------------------------------------------
		#region Public
		//

		//
		#endregion // Public
		//-------------------------------------------------------------------------
		#region Protected
		//
		protected void UpdateTimeouts()
		{
			if (m_serialPort == null)
				return;
			//
			if (!m_isInit)
				return;
			//
			m_serialPort.WriteTimeout = WriteTimeout;
			m_serialPort.ReadTimeout = ReadTimeout;
		}
		//
		#endregion // Public
		//-------------------------------------------------------------------------
		#region Private
		//
		void Init()
		{
			InitStrings();

			if(!LoadParam(ParamFileName))
			{
				m_spParams = new SerialPortParams();
				SaveParam();
			}

			m_portParams.SpParams = m_spParams;

			m_tempGraphicIndex = m_graphic.CreateGraphic("Температура", Colors.YellowGreen, 2, controls.Graphics.GraphicType.Line);
			m_pwrGraphicIndex = m_graphic.CreateGraphic("Pвых", Colors.Red, 2, controls.Graphics.GraphicType.Polygon);
			m_spGraphicIndex = m_graphic.CreateGraphic("Уставка", Colors.DeepSkyBlue, 2, controls.Graphics.GraphicType.Line);
			m_graphic.YMax = 200;
            m_graphic.XMax = 5400;
            m_graphic.IsCursorEnable = true;
            m_graphic.MoveCursor += m_graphic_MoveCursor;
            m_graphic.SetDiagnosticsNumData(m_tempGraphicIndex);
            m_graphic.ModeHysteresis = 1.0;
            m_graphic.MinModeHoldTimeSec = 300; // 300 sec
            m_graphsList.Add(m_graphic);
            //
            m_t1GraphIndexes.Add( m_T1graphic.CreateGraphic("Raw", Colors.LightGray, 2, controls.Graphics.GraphicType.Line) );
			m_t1GraphIndexes.Add( m_T1graphic.CreateGraphic("Median", Colors.Violet, 2, controls.Graphics.GraphicType.Polygon) );
			m_t1GraphIndexes.Add( m_T1graphic.CreateGraphic("Value", Colors.Yellow, 2, controls.Graphics.GraphicType.Line) );
            m_t1GraphIndexes.Add( m_T1graphic.CreateGraphic("Filter", Colors.YellowGreen, 2, controls.Graphics.GraphicType.Line) );
            m_T1graphic.YMax = 2500;
            m_T1graphic.XMax = 5400;
            m_T1graphic.IsCursorEnable = true;
            m_T1graphic.MoveCursor += m_graphic_MoveCursor;
            m_T1graphic.SetDiagnosticsNumData(m_t1GraphIndexes[2], m_t1GraphIndexes[3]);
            m_T1graphic.ModeHysteresis = 1.0;
            m_T1graphic.MinModeHoldTimeSec = 300; // 300 sec
            m_graphsList.Add(m_T1graphic);
            //
            m_t2GraphIndexes.Add(m_T2graphic.CreateGraphic("Raw", Colors.LightGray, 2, controls.Graphics.GraphicType.Line));
            m_t2GraphIndexes.Add(m_T2graphic.CreateGraphic("Median", Colors.Violet, 2, controls.Graphics.GraphicType.Polygon));
            m_t2GraphIndexes.Add(m_T2graphic.CreateGraphic("Value", Colors.Yellow, 2, controls.Graphics.GraphicType.Line));
            m_t2GraphIndexes.Add(m_T2graphic.CreateGraphic("Filter", Colors.YellowGreen, 2, controls.Graphics.GraphicType.Line));
            m_T2graphic.YMax = 2500;
            m_T2graphic.XMax = 5400;
            m_T2graphic.IsCursorEnable = true;
            m_T2graphic.MoveCursor += m_graphic_MoveCursor;
            m_T2graphic.SetDiagnosticsNumData(m_t2GraphIndexes[2], m_t2GraphIndexes[3]);
            m_T2graphic.ModeHysteresis = 1.0;
            m_T2graphic.MinModeHoldTimeSec = 300; // 300 sec
            m_graphsList.Add(m_T2graphic);
            //
			m_flowpvIndex = m_graphicFlow.CreateGraphic("Flow Pv (inv)", Colors.Red, 2, controls.Graphics.GraphicType.Polygon);
			m_totalFlowPvIndex = m_graphicFlow.CreateGraphic("Total flow Pv", Colors.LightSkyBlue, 2, controls.Graphics.GraphicType.Polygon);
			m_totalFlowSpIndex = m_graphicFlow.CreateGraphic("Total flow Sp", Colors.Yellow, 2, controls.Graphics.GraphicType.Line);
			m_flowSpIndex = m_graphicFlow.CreateGraphic("Flow Sp (inv)", Colors.YellowGreen, 2, controls.Graphics.GraphicType.Line);
			m_graphicFlow.YMax = 1100;
            m_graphicFlow.XMax = 5400;
			//m_graphicFlow.SetGraphicYOffset(m_flowpvIndex, -1000);
			//m_graphicFlow.SetGraphicYOffset(m_flowSpIndex, -1000);
			m_graphicFlow.SetGraphicYInverted(m_flowpvIndex, true);
			m_graphicFlow.SetGraphicYInverted(m_flowSpIndex, true);
            m_graphsList.Add(m_graphicFlow);

            m_f1GraphIndexes.Add(m_F1graphic.CreateGraphic("Raw", Colors.LightGray, 2, controls.Graphics.GraphicType.Line));
            m_f1GraphIndexes.Add(m_F1graphic.CreateGraphic("Median", Colors.Violet, 2, controls.Graphics.GraphicType.Polygon));
            m_f1GraphIndexes.Add(m_F1graphic.CreateGraphic("Value", Colors.Yellow, 2, controls.Graphics.GraphicType.Line));
            m_f1GraphIndexes.Add(m_F1graphic.CreateGraphic("Filter", Colors.YellowGreen, 2, controls.Graphics.GraphicType.Line));
            m_F1graphic.YMax = 2500;
            m_F1graphic.XMax = 5400;
            m_F1graphic.IsCursorEnable = true;
            m_F1graphic.MoveCursor += m_graphic_MoveCursor;
            m_F1graphic.SetDiagnosticsNumData(m_t2GraphIndexes[2], m_t2GraphIndexes[3]);
            m_F1graphic.ModeHysteresis = 1.0;
            m_F1graphic.MinModeHoldTimeSec = 300; // 300 sec
            m_graphsList.Add(m_F1graphic);

            m_f2GraphIndexes.Add(m_F2graphic.CreateGraphic("Raw", Colors.LightGray, 2, controls.Graphics.GraphicType.Line));
            m_f2GraphIndexes.Add(m_F2graphic.CreateGraphic("Median", Colors.Violet, 2, controls.Graphics.GraphicType.Polygon));
            m_f2GraphIndexes.Add(m_F2graphic.CreateGraphic("Value", Colors.Yellow, 2, controls.Graphics.GraphicType.Line));
            m_f2GraphIndexes.Add(m_F2graphic.CreateGraphic("Filter", Colors.YellowGreen, 2, controls.Graphics.GraphicType.Line));
            m_F2graphic.YMax = 2500;
            m_F2graphic.XMax = 5400;
            m_F2graphic.IsCursorEnable = true;
            m_F2graphic.MoveCursor += m_graphic_MoveCursor;
            m_F2graphic.SetDiagnosticsNumData(m_t2GraphIndexes[2], m_t2GraphIndexes[3]);
            m_F2graphic.ModeHysteresis = 1.0;
            m_F2graphic.MinModeHoldTimeSec = 300; // 300 sec
            m_graphsList.Add(m_F2graphic);

			m_graphicData.CreateGraphic("SIP data", Colors.GreenYellow, 2, controls.Graphics.GraphicType.Line);
			m_graphicData.AxisXUnitType = controls.AxisXUnitsType.Direct;
			m_graphicData.AxisXUnitDivider = 50;
            m_graphicData.MoveCursor += m_graphic_MoveCursor;
            m_graphicData.IsCursorEnable = true;
            m_graphsList.Add(m_graphicData);

			m_bgwWorker = new BackgroundWorker();
			m_bgwWorker.WorkerSupportsCancellation = true;
			m_bgwWorker.WorkerReportsProgress = true;
			m_bgwWorker.ProgressChanged += m_bgwWorker_ProgressChanged;
			m_bgwWorker.DoWork += m_bgwWorker_DoWork;
			m_bgwWorker.RunWorkerAsync();

            Pid1EA.PidResetDelegate += Pid1Reset;
            m_pidFactors1.KdChange += m_pidFactors1_KdChange;
            m_pidFactors1.KiChange += m_pidFactors1_KiChange;
            m_pidFactors1.KpChange += m_pidFactors1_KpChange;

            PidFlowTotalEA.PidResetDelegate += PidFlowTotalReset;
            m_pidFactorsFt.KpChange += m_pidFactorsFt_KpChange;
            m_pidFactorsFt.KiChange += m_pidFactorsFt_KiChange;
            m_pidFactorsFt.KdChange += m_pidFactorsFt_KdChange;

            PidFlowInEA.PidResetDelegate += PidFlowInReset;
            m_pidFactorsFi.KpChange += m_pidFactorsFi_KpChange;
            m_pidFactorsFi.KiChange += m_pidFactorsFi_KiChange;
            m_pidFactorsFi.KdChange += m_pidFactorsFi_KdChange;
            
            pidParams.PidFactors = m_pidFactors1;
            pidParamsFlow.PidFactors = m_pidFactorsFt;
            pidParamsFlowIn.PidFactors = m_pidFactorsFi;
		}

        bool LoadParam(string fName)
		{
			SerialPortParams tmp = null;
			if (BaseSerializer<SerialPortParams>.LoadFromFile(fName, out tmp))
			{
				m_spParams = tmp;
				return true;
			}
			return false;
		}

		void SaveParam()
		{
			BaseSerializer<SerialPortParams>.SaveToFile(m_paramFileFullName, m_spParams);
		}

		void InitStrings()
		{
			FileInfo fi = new FileInfo(Assembly.GetExecutingAssembly().Location);
			m_appDirFullName = fi.DirectoryName;
			m_paramFileFullName = m_appDirFullName + "\\" + ParamFileName;
		}

		void CreateSerialPort()
		{
			if (m_serialPort != null)
			{
				try
				{
					ClosePort();
				}
				catch { }
			}
			//
			m_serialPort = new SerialPort();
			UpdateTimeouts();
			UpdateSerialPortParam();
			GC.Collect();
		}

		void UpdateSerialPortParam()
		{
			if (m_serialPort == null)
				return;
			//
			m_serialPort.BaudRate = (int)m_spParams.Baudrate;
			m_serialPort.DataBits = (int)m_spParams.DataBits;
			m_serialPort.Parity = m_spParams.Parity;
			m_serialPort.StopBits = m_spParams.StopBits;
			m_serialPort.Handshake = m_spParams.Handshake;
			//
			if (m_serialPort.PortName != m_spParams.Name)
			{
				bool lastIsOpen = m_serialPort.IsOpen;
				ClosePort();
				string portName = m_serialPort.PortName;
				try
				{
					m_serialPort.PortName = m_spParams.Name;
				}
				catch
				{
					m_serialPort.PortName = portName;
				}
				if (lastIsOpen)
					Open();
			}
		}

		void ClosePort()
		{
			if (m_serialPort == null)
				return;
			//
            m_isFirstRead = true;
			if (m_serialPort.IsOpen)
			{
				try
				{
					if (FindPortName(m_serialPort.PortName))
					{
						m_serialPort.Close();
						m_serialPort.Dispose();
					}
					
					m_serialPort = null;
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Ошибка закрытия порта < " + m_spParams.Name + " >", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		bool Open()
		{
			if (m_serialPort != null)
			{
				ClosePort();
			}
			//
			CreateSerialPort();
			//
			try
			{
				m_serialPort.Open();
				m_serialPort.BaseStream.Flush();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка открытия порта < " + m_spParams.Name + " >", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			//
			return (m_serialPort != null) ? m_serialPort.IsOpen : false;
		}

		bool FindPortName(string name)
		{
			if (string.IsNullOrEmpty(name))
				return false;
			//
			string[] ports = SerialPort.GetPortNames();
			bool found = false;
			for (int i = 0; i < ports.Length; i++)
			{
				if (ports[i] != name)
					continue;
				//
				found = true;
				break;
			}
			//
			return found;
		}

		public bool InitModbus()
		{
            m_ioList.Clear();

			if (!Open())
				return false;

			return InitModbusProtocol(ref mbMaster, ref m_serialPort);
		}

		bool InitModbusProtocol(ref IModbusSerialMaster mb, ref SerialPort sp)
		{
			try
			{
				if ((mb != null) && (sp != null))
				{
					if (!sp.IsOpen)
						sp.Open();
					//
					return true;
				}
				if (sp == null)
				{
					return false;
				}
				mb = ModbusSerialMaster.CreateRtu(sp);
				mb.Transport.ReadTimeout = 700;
				mb.Transport.WriteTimeout = 700;
				return true;
			}
			catch
			{
				try
				{
					if (mb != null)
						mb.Dispose();
					//
					if (sp != null)
						sp.Dispose();
				}
				finally
				{
					mb = null;
					sp = null;
					GC.Collect();
				}
				//
				return false;
			}
		}

		void SetStopBits(StopBits value)
		{
			try
			{
				m_serialPort.StopBits = value;
				m_spParams.StopBits = value;
				SaveParam();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка установки количества стоп бит порта < " + m_spParams.Name + " >", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		void SetParity(Parity value)
		{
			try
			{
				m_serialPort.Parity = value;
				m_spParams.Parity = value;
				SaveParam();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка установки паритета четности порта < " + m_spParams.Name + " >", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		void SetHandshake(Handshake value)
		{
			try
			{
				m_serialPort.Handshake = value;
				m_spParams.Handshake = value;
				SaveParam();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка установки свойства управления потоком порта < " + m_spParams.Name + " >", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		void SetBaudRate(Baudrates val)
		{
			try
			{
				m_serialPort.BaudRate = (int)val;
				m_spParams.Baudrate = val;
				SaveParam();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка установки скорости порта < " + m_spParams.Name + " >", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

        void CheckConnectedDevice()
        {
            if (m_deviceConnected)
                return;

            m_isDeviceWritePasswordNeed = false;
            m_deviceInfo = string.Empty;
            m_deviseInfoSize = 0;

            ushort[] tmp = null;

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    tmp = mbMaster.ReadHoldingRegisters(m_sipAddr, InfoSizeRegNum, 1);
                    m_deviseInfoSize = tmp[0];

                    tmp = mbMaster.ReadHoldingRegisters(m_sipAddr, InfoRegNum, m_deviseInfoSize);
                    m_deviceInfo = ConvertDeviceString(tmp);

                    m_isDeviceWritePasswordNeed = true;
                    break;
                }
                catch (Exception ex)
                {
                    m_isDeviceWritePasswordNeed = false;
                    m_deviceInfo = string.Empty;
                    m_deviseInfoSize = 0;
                }
            }
        }

        bool IsRegionEnable(SipModbusRegionType rt)
        {
            bool isEn = false;
            switch (rt)
            {
                case SipModbusRegionType.Params:
                    isEn = m_readParams;
                    break;
                case SipModbusRegionType.Data:
                    isEn = m_readData;
                    break;
                case SipModbusRegionType.Config:
                    isEn = m_readConfig;
                    break;
                case SipModbusRegionType.PidData:
                    isEn = m_readPidData;
                    break;
                default:
                    break;
            }

            return isEn;
        }

		void ReadModbus()
		{
			if (mbMaster == null)
				return;

			ushort[] buf = new ushort[m_sipRegs.GetBufSize() / 2];
			ushort[] tmp = null;
			int index = 0;
            int rdIndex = 0;
			ushort len = 0;
            ushort count = 0;// m_regsCount;
            ushort start = 0;// m_regStart;
			try
			{
                CheckConnectedDevice();

                //while(count > 0)
                //{
                //    len = (count > ReadMaxLength) ? ReadMaxLength : count;
                //    tmp =  mbMaster.ReadHoldingRegisters(m_sipAddr, start, len);
                //    Array.Copy(tmp, 0, buf, index, len);

                //    count -= len;
                //    start += len;
                //    index += len;
                //}

                //m_sipRegs.Build(buf, m_regStart, m_regsCount);
                ////
                //int pidDataStart = m_sipRegs.GetPidDataOffset();
                //start = (ushort)pidDataStart;
                //int pidDataCount = m_sipRegs.GetPidDataSize();
                //count = (ushort)pidDataCount;
                ////
                //index = 0;
                //len = 0;
                ////
                //while (count > 0)
                //{
                //    len = (count > ReadMaxLength) ? ReadMaxLength : count;
                //    tmp = mbMaster.ReadHoldingRegisters(m_sipAddr, start, len);
                //    Array.Copy(tmp, 0, buf, index, len);

                //    count -= len;
                //    start += len;
                //    index += len;
                //}

                //m_sipRegs.Build(buf, pidDataStart, pidDataCount);

                SipModbusRegion[] readRegions = m_sipRegs.GetReadRegions();
                if(readRegions != null)
                {
                    for (int i = 0; i < readRegions.Length; i++)
                    {
                        count = readRegions[i].Count;
                        start = readRegions[i].StartAddr;
                        index = start;
                        rdIndex = 0;
                        if (!IsRegionEnable(readRegions[i].RegionType))
                            continue;

                        ushort[] readBuf = new ushort[count]; 
                        while (count > 0)
                        {
                            len = (count > ReadMaxLength) ? ReadMaxLength : count;
                            tmp = mbMaster.ReadHoldingRegisters(m_sipAddr, start, len);
                            Array.Copy(tmp, 0, readBuf, rdIndex, len);
                            Array.Copy(tmp, 0, buf, index, len);

                            count -= len;
                            start += len;
                            index += len;
                            rdIndex += len;
                        }

                        m_sipRegs.Build(readBuf, readRegions[i].StartAddr, readRegions[i].Count);
                    }
                }

                if ((m_sipRegs.IsAnalogDataSupport) && (m_readAnalogData))
                {
                    start = (ushort)m_sipRegs.GetAnalogDataOffset();
                    count = 2;
                    tmp = mbMaster.ReadHoldingRegisters(m_sipAddr, start, count);
                    start += 2;
                    m_sensorsCount = tmp[0];
                    m_analogDataCount = tmp[1];
                    //
                    count = (ushort)(tmp[0] * tmp[1] * 2);
                    tmp = mbMaster.ReadHoldingRegisters(m_sipAddr, start, count);
                    byte[] tBuf = new byte[tmp.Length * 2];
                    for (int i = 0, j = 0; i < tmp.Length; i++)
                    {
                        tBuf[j++] = (byte)tmp[i];
                        tBuf[j++] = (byte)(tmp[i] >> 8);
                    }
                    //
                    m_analogData.Clear();
                    index = 0;
                    count = (ushort)(m_sensorsCount * m_analogDataCount);
                    for (int i = 0; i < count; i++)
                    {
                        float f = BitConverter.ToSingle(tBuf, index);
                        m_analogData.Add(f);
                        index += sizeof(float);
                    }
                }
                else
                {
                    m_sensorsCount = 0;
                    m_analogDataCount = 0;
                    if(m_analogData.Count > 0)
                        m_analogData.Clear();
                }

                CheckIOList(buf);
                m_deviceConnected = true;
			}
			catch (Exception ex)
			{
                m_deviceConnected = false;
                m_deviceInfo = null;
                m_isDeviceWritePasswordNeed = false;
                m_deviseInfoSize = 0;
                m_isFirstRead = true;
			}
		}

        void CheckIOList(ushort[] buf)
        {
            if (buf == null)
                return;

            if (m_ioList.Count <= 0)
                return;

            for (int i = 0; i < m_ioList.Count; i++)
            {
                ModbusIOEventArgs e = m_ioList[i];
                if (e == null)
                {
                    m_ioList.RemoveAt(0);
                    i--;
                    continue;
                }

                ushort start = e.StartIndex;
                ushort end = (ushort)(e.StartIndex + e.Count);

                if (e.IOType == ModbusIOType.Read)
                { // Read
                    if ((start > buf.Length) || (end > buf.Length))
                    {
                        if(e.IsCheckResult)
                        {
                            e.IOResult = false;
                            e.OnIOCallBack(e.Sender, e);
                        }
                        continue;
                    }
                        
                    ushort[] tmpBuf = new ushort[e.Count];
                    Array.Copy(buf, start, tmpBuf, 0, e.Count);
                    e.Buf = tmpBuf;
                    e.OnIOCallBack(e.Sender, e);
                    m_ioList.RemoveAt(0);
                    i--;
                    continue;
                }

                if (!e.IsComplete)
                    continue;

                // Write result
                if(!e.IsCheckResult)
                {
                    m_ioList.RemoveAt(i);
                    continue;
                }
                bool valid = true;
                for (int j = 0; j < e.Count; j++)
                {
                    if (buf[j + e.StartIndex] == e.Buf[j])
                        continue;

                    valid = false;
                    break;
                }
                e.IOResult = valid;
                if (valid)
                {
                    e.LastError = "";
                    e.OnIOCallBack(e.Sender, e);
                    m_ioList.RemoveAt(i);
                    continue;
                }

                e.LastError = "Read data is not valid";
            }
        }

        void CheckIOListWrite()
        {
            if (m_ioList.Count <= 0)
                return;

            for (int i = 0; i < m_ioList.Count; i++)
            {
                ModbusIOEventArgs e = m_ioList[i];
                if (e == null)
                {
                    m_ioList.RemoveAt(0);
                    i--;
                    continue;
                }

                if (e.IOType == ModbusIOType.Read)
                    continue;

                if(e.Buf == null)
                {
                    m_ioList.RemoveAt(0);
                    i--;
                    continue;
                }

                ushort start = e.StartIndex;
                
                // Write
                try
                {
                    ushort count = e.Count;
                    ushort len;
                    int index = 0;

                    while (count > 0)
                    {
                        SendPassword();

                        len = (count > ReadMaxLength) ? ReadMaxLength : count;
                        ushort[] wrBuf = new ushort[len];
                        Array.Copy(e.Buf, index, wrBuf, 0, len);

                        mbMaster.WriteMultipleRegisters(e.Addr, start, wrBuf);
                        count -= len;
                        start += len;
                        index += len;
                        e.IOResult = true;
                        e.LastError = "";
                    }

                    e.IsComplete = true;
                }
                catch (Exception ex)
                {
                    if (e.IsCheckResult)
                    {
                        e.IOResult = false;
                        e.LastError = ex.Message;
                        e.OnIOCallBack(e.Sender, e); 
                    }
                }
            }
        }

        bool SendPassword()
        {
            if (!m_isDeviceWritePasswordNeed)
                return true;


            try
            {
                ushort[] pass = GetPasswordBuf();
                mbMaster.WriteMultipleRegisters(m_sipAddr, PasswordRegNum, pass);
                return true;
            }
            catch
            {
                return false;
            }
        }

        ushort[] GetPasswordBuf()
        {
            byte[] uBuf = Encoding.Unicode.GetBytes(DevicePassword);
            Encoding enc = Encoding.GetEncoding(1251);
            byte[] aBuf = Encoding.Convert(Encoding.Unicode, enc, uBuf);

            ushort[] result = new ushort[aBuf.Length];
            for (int i = 0; i < aBuf.Length; i++)
            {
                result[i] = aBuf[i];
            }

            return result;
        }

        string ConvertDeviceString(ushort[] data)
        {
            byte[] result = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                result[i] = (byte)data[i];
            }

            Encoding enc = Encoding.GetEncoding(1252);
            string str = enc.GetString(result);
            return str;
        }

        int m_x = 0;
        int m_tempSetupStartTime;
        int m_modeStartTime;
        bool m_isFirst = true;
		void ShowData()
		{
			double pv = ((double)(m_sipRegs.CpuOut.Temp1 / 10.0) + (double)(m_sipRegs.CpuOut.Temp2 / 10.0)) / 2;
			double sp = (double)(m_sipRegs.CpuIn.T1Setpoint / 10.0);
            double pwr = Math.Round(m_sipRegs.Heater1OutValue / 10.0, 3);// (double)(m_sipRegs.CpuOut.HeaterPower / 10.0);

			double flowIn = (double)(m_sipRegs.CpuOut.FlowIn / 10.0);
			double flowInSp = (double)(m_sipRegs.CpuIn.FlowInSetpoint / 10.0);
			double totalFlow = (double)(m_sipRegs.CpuOut.FlowTotal / 10.0);
			double totalFlowSp = (double)(m_sipRegs.CpuIn.FlowTotalSetpoint / 10.0);

            int tempSetupTime = m_sipRegs.TempSetupTime;
            int modeOkTime = m_sipRegs.ModeOkWorkTime;

			m_graphic.Dispatcher.BeginInvoke(new Action(() =>
			{
				m_graphic.AddPoint(m_spGraphicIndex, sp);
				m_graphic.AddPoint(m_tempGraphicIndex, pv);
				m_graphic.AddPoint(m_pwrGraphicIndex, pwr);

				m_graphicFlow.AddPoint(m_flowpvIndex, flowIn);
				m_graphicFlow.AddPoint(m_flowSpIndex, flowInSp);
				m_graphicFlow.AddPoint(m_totalFlowPvIndex, totalFlow);
				m_graphicFlow.AddPoint(m_totalFlowSpIndex, totalFlowSp);

				List<Point> lst = Convert(m_sipRegs.FpgaIn.Points, 1024);
				m_graphicData.ShowData(lst, 0);

                try
                {
                    if (m_analogData.Count > 0)
                    {
                        int fIndex = 0;
                        int gIndex = 0;
                        m_T1graphic.AddPoint(m_t1GraphIndexes[gIndex++], m_analogData[fIndex++]);
                        m_T1graphic.AddPoint(m_t1GraphIndexes[gIndex++], m_analogData[fIndex++]);
                        m_T1graphic.AddPoint(m_t1GraphIndexes[gIndex++], m_analogData[fIndex++]);
                        m_T1graphic.AddPoint(m_t1GraphIndexes[gIndex++], m_analogData[fIndex++]);
                        //
                        gIndex = 0;
                        m_T2graphic.AddPoint(m_t2GraphIndexes[gIndex++], m_analogData[fIndex++]);
                        m_T2graphic.AddPoint(m_t2GraphIndexes[gIndex++], m_analogData[fIndex++]);
                        m_T2graphic.AddPoint(m_t2GraphIndexes[gIndex++], m_analogData[fIndex++]);
                        m_T2graphic.AddPoint(m_t2GraphIndexes[gIndex++], m_analogData[fIndex++]);
                        //
                        gIndex = 0;
                        m_F1graphic.AddPoint(m_f1GraphIndexes[gIndex++], m_analogData[fIndex++]);
                        m_F1graphic.AddPoint(m_f1GraphIndexes[gIndex++], m_analogData[fIndex++]);
                        m_F1graphic.AddPoint(m_f1GraphIndexes[gIndex++], m_analogData[fIndex++]);
                        m_F1graphic.AddPoint(m_f1GraphIndexes[gIndex++], m_analogData[fIndex++]);
                        //
                        gIndex = 0;
                        m_F2graphic.AddPoint(m_f2GraphIndexes[gIndex++], m_analogData[fIndex++]);
                        m_F2graphic.AddPoint(m_f2GraphIndexes[gIndex++], m_analogData[fIndex++]);
                        m_F2graphic.AddPoint(m_f2GraphIndexes[gIndex++], m_analogData[fIndex++]);
                        m_F2graphic.AddPoint(m_f2GraphIndexes[gIndex++], m_analogData[fIndex++]);
                    }
                }
                catch { }

                //m_pidFactors1.Kp = Math.Round(m_sipRegs.Heater1Kp, 5);
                //m_pidFactors1.Ki =  Math.Round(m_sipRegs.Heater1Ki, 5);
                //m_pidFactors1.Kd =  Math.Round(m_sipRegs.Heater1Kd, 5);

				Pid1EA.Kp =  Math.Round(m_sipRegs.Heater1Kp, 5);
				Pid1EA.Ki =  Math.Round(m_sipRegs.Heater1Ki, 5);
				Pid1EA.Kd =  Math.Round(m_sipRegs.Heater1Kd, 5);
				Pid1EA.PValue =  Math.Round(m_sipRegs.Heater1PValue, 5);
				Pid1EA.IValue =  Math.Round(m_sipRegs.Heater1IValue, 5);
				Pid1EA.DValue =  Math.Round(m_sipRegs.Heater1DValue, 5);
				Pid1EA.Sp = sp;
				Pid1EA.Pv = pv;
				//Pid1EA.Error = m_sipRegs.ErrSumm1;
				Pid1EA.OutValue = Math.Round(m_sipRegs.Heater1OutValue / 10.0, 3);
				pidParams.SetPidValues(Pid1EA);
                //-----------------------------------------------------------------------------

                PidFlowTotalEA.Kp = Math.Round(m_sipRegs.Regs.Config.FlowTotalPidFactors.kP, 5);
                PidFlowTotalEA.Ki = Math.Round(m_sipRegs.Regs.Config.FlowTotalPidFactors.kI, 5);
                PidFlowTotalEA.Kd = Math.Round(m_sipRegs.Regs.Config.FlowTotalPidFactors.kD, 5);
                PidFlowTotalEA.PValue = Math.Round(m_sipRegs.Regs.FlowTotalPidData.pValue, 5);
                PidFlowTotalEA.IValue = Math.Round(m_sipRegs.Regs.FlowTotalPidData.iValue, 5);
                PidFlowTotalEA.DValue = Math.Round(m_sipRegs.Regs.FlowTotalPidData.dValue, 5);
                PidFlowTotalEA.Sp = m_sipRegs.Regs.FlowTotalPidData.sp;
                PidFlowTotalEA.Pv = m_sipRegs.Regs.CPU_OUT.FlowTotal / 10.0;
                //PidFlowTotalEA.Error = m_sipRegs.ErrSumm1;
                PidFlowTotalEA.OutValue = Math.Round(m_sipRegs.Regs.FlowTotalPidData.outValue, 3);
                pidParamsFlow.SetPidValues(PidFlowTotalEA);
                //----------------------------------------------------------------------------
                PidFlowInEA.Kp = Math.Round(m_sipRegs.Regs.Config.FlowInPidFactors.kP, 5);
                PidFlowInEA.Ki = Math.Round(m_sipRegs.Regs.Config.FlowInPidFactors.kI, 5);
                PidFlowInEA.Kd = Math.Round(m_sipRegs.Regs.Config.FlowInPidFactors.kD, 5);
                PidFlowInEA.PValue = Math.Round(m_sipRegs.Regs.FlowInPidData.pValue, 5);
                PidFlowInEA.IValue = Math.Round(m_sipRegs.Regs.FlowInPidData.iValue, 5);
                PidFlowInEA.DValue = Math.Round(m_sipRegs.Regs.FlowInPidData.dValue, 5);
                PidFlowInEA.Sp = m_sipRegs.Regs.FlowInPidData.sp;
                PidFlowInEA.Pv = m_sipRegs.Regs.CPU_OUT.FlowIn / 10.0;
                //PidFlowInEA.Error = m_sipRegs.ErrSumm1;
                PidFlowInEA.OutValue = Math.Round(m_sipRegs.Regs.FlowInPidData.outValue, 3);
                pidParamsFlowIn.SetPidValues(PidFlowInEA);
                //----------------------------------------------------------------------------
                if (!m_isFirst)
                {
                    if (tempSetupTime == 0)
                        m_tempSetupStartTime = m_x;
                    //
                    if (modeOkTime == 0)
                        m_modeStartTime = m_x; 
                }
                else
                {
                    if (tempSetupTime != 0)
                        m_tempSetupStartTime = (-tempSetupTime);
                    //
                    if (modeOkTime != 0)
                        m_modeStartTime = (-modeOkTime); 
                }
                //
                txtTime.Text = "Time= " + SecondsToTimeStr(m_x);
                txtTempSetupTime.Text = "Ttemp= " + SecondsToTimeStr(tempSetupTime);
                txtModeOkWorkTime.Text = "Tmode= " + SecondsToTimeStr(modeOkTime);
                //
                m_x++;
                m_isFirst = false;
                //
                if(m_isFirstRead)
                {
                    m_temp1SldrEventEn = false;
                    m_temp2SldrEventEn = false;
                    m_flowTotalEventEn = false;
                    m_flowInEventEn = false;

                    sldrTemp1.Value = m_sipRegs.Regs.CPU_IN.T1Setpoint / 10.0;
                    sldrTemp2.Value = m_sipRegs.Regs.CPU_IN.T2Setpoint / 10.0;
                    sldrFlowTotal.Value = m_sipRegs.Regs.CPU_IN.FlowTotalSetpoint / 10.0;
                    sldrFlowIn.Value = m_sipRegs.Regs.CPU_IN.FlowInSetpoint / 10.0;

                    m_pidFactors1.Kp = Math.Round(m_sipRegs.Heater1Kp, 5);
                    m_pidFactors1.Ki = Math.Round(m_sipRegs.Heater1Ki, 5);
                    m_pidFactors1.Kd = Math.Round(m_sipRegs.Heater1Kd, 5);
                    
                    m_pidFactorsFt.Kp = Math.Round(m_sipRegs.Regs.Config.FlowTotalPidFactors.kP, 5);
                    m_pidFactorsFt.Ki = Math.Round(m_sipRegs.Regs.Config.FlowTotalPidFactors.kI, 5);
                    m_pidFactorsFt.Kd = Math.Round(m_sipRegs.Regs.Config.FlowTotalPidFactors.kD, 5);

                    m_pidFactorsFi.Kp = Math.Round(m_sipRegs.Regs.Config.FlowInPidFactors.kP, 5);
                    m_pidFactorsFi.Ki = Math.Round(m_sipRegs.Regs.Config.FlowInPidFactors.kI, 5);
                    m_pidFactorsFi.Kd = Math.Round(m_sipRegs.Regs.Config.FlowInPidFactors.kD, 5);

                    m_temp1SldrEventEn = true;
                    m_temp2SldrEventEn = true;
                    m_flowTotalEventEn = true;
                    m_flowInEventEn = true;
                    m_isFirstRead = false;
                }
                //
			}), System.Windows.Threading.DispatcherPriority.Send);
		}

		void Pid1Reset()
		{
            AddRegToWrList(156, HostControlReg0PidTempResetBit, false);
		}

        void PidFlowTotalReset()
        {
            AddRegToWrList(156, HostControlReg0PidFlowTotalResetBit, false);
        }

        void PidFlowInReset()
        {
            AddRegToWrList(156, HostControlReg0PidFlowInResetBit, false);
        }

        string SecondsToTimeStr(int sec)
        {

            if (Math.Abs(sec) < 60)
                return sec.ToString() + " s";
            //
            string result;
            int m, h;
            bool negative = Math.Sign(sec) < 0;
            //
            if(Math.Abs(sec) < 3600)
            {
                m = Math.Abs(sec) / 60;
                sec = Math.Abs(sec) % 60;
                //
                result = string.Format("{0:D2}", m) + ":" + string.Format("{0:D2}", sec);
                if (negative)
                    return "-" + result;
                //
                return result;
            }
            //
            h = Math.Abs(sec) / 3600;

            m = Math.Abs(sec - (h * 3600)) / 60;
            sec = Math.Abs(sec - (h * 3600) - (m * 60)) % 60;
            //
            result = string.Format("{0:D2}", h) + ":" + string.Format("{0:D2}", m) + ":" + string.Format("{0:D2}", sec);
            if (Math.Sign(sec) < 0)
               return "-" + result;
            //
            return result;
        }

		List<Point> Convert(ushort[] buf, int count)
		{
			if (buf == null)
				return null;
			List<Point> lst = new List<Point>(buf.Length);

			for (int i = 0; i < buf.Length; i++)
			{
				if (i >= count)
					break;

				lst.Add(new Point(i, ConvertScale(buf[i], 0, 2500)));
			}

			return lst;
		}

		public static double ConvertScale(ushort raw, double a0, double a1)
		{
			return a0 + (a1 - a0) * raw / ushort.MaxValue; // / RawY1;
		}

		void ShowIOTime(double t, string units)
		{
			txtIOtime.Dispatcher.BeginInvoke(new Action(() =>
			{
				string str = Math.Round(t, 3).ToString();
				if(!string.IsNullOrEmpty(units))
				{
					str += (" " + units);
				}
				txtIOtime.Text = str;
			}), System.Windows.Threading.DispatcherPriority.Send);
		}

        void AddTemp1SpToWrList(double value)
        {
            AddOneRegToWrList(65, value);
        }

        void AddTemp2SpToWrList(double value)
        {
            AddOneRegToWrList(66, value);
        }

        void AddOneRegToWrList(ushort regAddr, double value)
        {
            ushort temp = (ushort)(Math.Round(value * 10, 1));
            AddRegToWrList(regAddr, temp);
        }

        void AddRegToWrList(ushort regAddr, ushort value, bool isCheckResult = true)
        {
            ModbusIOEventArgs e2 = new ModbusIOEventArgs()
            {
                Addr = m_sipAddr,
                Buf = new ushort[] { value },
                Count = 1,
                IOResult = false,
                IOType = ModbusIOType.Write,
                IsCheckResult = isCheckResult,
                Sender = this,
                LastError = "",
                StartIndex = regAddr, 
                IsComplete = false
            };

            m_ioList.Add(e2);
        }

        void AddToWrList(ushort regAddr, ushort[] buf, bool isCheckResult = true)
        {
            if (buf == null)
                return;

            if (buf.Length <= 0)
                return;
            //
            ModbusIOEventArgs e2 = new ModbusIOEventArgs()
            {
                Addr = m_sipAddr,
                Buf = buf,
                Count = (ushort)buf.Length,
                IOResult = false,
                IOType = ModbusIOType.Write,
                IsCheckResult = isCheckResult,
                Sender = this,
                LastError = "",
                StartIndex = regAddr, 
                IsComplete = false
            };

            m_ioList.Add(e2);
        }
        
        void AddFlowTotalSpToWrList(double value)
        {
            AddOneRegToWrList(61, value);
        }

        void AddFlowInSpToWrList(double value)
        {
            AddOneRegToWrList(62, value);
        }
		//
		#endregion // Private
		//-------------------------------------------------------------------------
		#region Events
		//
		bool m_bgwWorkerEn;
		double sp = 45;
		double pv = 0;
		double pwr = 0;
		void m_bgwWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			HiPerfTimer tmr = new HiPerfTimer();
			m_bgwWorkerEn = true;
			int time;
			double tmp;
			while (m_bgwWorkerEn)
			{
				if (m_bgwWorker.CancellationPending)
				{
					e.Cancel = true;
					m_bgwWorkerEn = false;
					break;
				}

				tmr.Start();

				switch (m_wrkType)
				{
					case WorkType.Idle:
						//m_graphic.AddPoint(m_spGraphicIndex, sp);
						//m_graphic.AddPoint(m_tempGraphicIndex, pv);
						//m_graphic.AddPoint(m_pwrGraphicIndex, pwr);

						//pv += 0.1;
						//pwr += 0.01;
						//if (pwr >= 100)
						//	pwr = 100;

						break;
					case WorkType.Work:
						ReadModbus();
						ShowData();
                        CheckIOListWrite();
						break;
					default:
						break;
				}
				tmr.Stop();
				ShowIOTime(tmr.Duration, "s");
                tmp = UpdateInterval - (tmr.Duration * 1000);
				time = (tmp > 20) ? ((int)tmp) : 20;
				Thread.Sleep(time);
			}
		}

		void m_bgwWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			
		}

		private void WpfSerialPortSettings_StopBitsChange(object sender, System.IO.Ports.StopBits value)
		{
			SetStopBits(value);
		}

		private bool WpfSerialPortSettings_OpenStatusChange(object sender, bool value)
		{
			m_wrkType = WorkType.Idle;

			if (value)
			{
				bool result = InitModbus();
				SaveParam();
				if (result)
					m_wrkType = WorkType.Work;
				//
				return result;
			}
			//
			if (m_serialPort == null)
			{
				m_spParams.Name = "";
				return true;
			}
			//
			if (mbMaster != null)
			{
				mbMaster.Dispose();
				mbMaster = null;
			}

			ClosePort();
			return ((m_serialPort == null) ? true : !m_serialPort.IsOpen);
		}

		private void WpfSerialPortSettings_ParityChange(object sender, System.IO.Ports.Parity value)
		{
			SetParity(value);
		}

		private void WpfSerialPortSettings_PortNameChange(object sender, string value)
		{
			if (string.IsNullOrEmpty(value))
				return;
			//
			if (value == m_spParams.Name)
				return;
			//
			if (string.IsNullOrEmpty(value))
				return;
			//
			if (m_serialPort == null)
			{
				CreateSerialPort();
			}
			//
			bool lastIsOpen = m_serialPort.IsOpen;
			ClosePort();

			if (m_serialPort == null)
			{
				CreateSerialPort();
			}

			m_serialPort.PortName = value;
			m_spParams.Name = value;
			//
			if (lastIsOpen)
				Open();
			//
			SaveParam();
		}

		private void WpfSerialPortSettings_ShowLogDataChange(object sender, bool value)
		{
			
		}

		private void WpfSerialPortSettings_HandshakeChange(object sender, System.IO.Ports.Handshake value)
		{
			SetHandshake(value);
		}

		private void WpfSerialPortSettings_LogEnableChange(object sender, bool value)
		{

		}

		private void WpfSerialPortSettings_BaudRateChange(object sender, Baudrates value)
		{
			SetBaudRate(value);
		}

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double w = m_graphic.ActualWidth;
            double h = m_graphic.ActualHeight;
            //
            if (w < (mainGrid.ActualWidth))
                m_graphic.Width = mainGrid.ActualWidth;
            //
            if (h < (mainGrid.RowDefinitions[1].ActualHeight))
                m_graphic.Height = mainGrid.RowDefinitions[1].ActualHeight;
        }

        void GraphWidthChange(controls.MGraphic graph, double delta)
        {
            if (graph == null)
                return;

            double w = graph.ActualWidth;
            
            w += delta;
            if (w < (mainGrid.ActualWidth))
                w = mainGrid.ActualWidth;

            graph.Width = w;
        }

        void GraphHeightChange(controls.MGraphic graph, double delta)
        {
            if (graph == null)
                return;

            double h = graph.ActualHeight;

            h += delta;
            if (h < (mainGrid.RowDefinitions[1].ActualHeight))
                h = mainGrid.RowDefinitions[1].ActualHeight;

            graph.Height = h;
        }

        controls.MGraphic GetSelectedGraphic()
        {
            controls.MGraphic graph = null;
            switch (tabControl.SelectedIndex)
            {
                case 0:
                    graph = m_graphic;
                    break;
                case 1:
                    graph = m_T1graphic;
                    break;
                case 2:
                    graph = m_T2graphic;
                    break;
                case 3:
                    graph = m_graphicFlow;
                    break;
                case 4:
                    graph = m_F1graphic;
                    break;
                case 5:
                    graph = m_F2graphic;
                    break;
                case 6:
                    graph = m_graphicData;
                    break;
                default:

                    break;
            }
            return graph;
        }

        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            controls.MGraphic graph = GetSelectedGraphic();

            if (Keyboard.Modifiers == ModifierKeys.Alt)
            {
                GraphWidthChange(graph, e.Delta);
            }
            else if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                GraphHeightChange(graph, e.Delta);
            }
            else if(Keyboard.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift))
            {
                GraphHeightChange(graph, e.Delta * 10);
            }
            else if (Keyboard.Modifiers == (ModifierKeys.Control | ModifierKeys.Alt))
            {
                GraphWidthChange(graph, e.Delta * 10);
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            controls.MGraphic graph = GetSelectedGraphic();
            if(graph != null)
                graph.SetKeyDown(e.Key);
        }

        void m_graphic_MoveCursor(object sender, string time, double temp)
        {
            txtPos.Text = "X= " + time + ", Y= " + string.Format("{0:F1}", temp);
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.LeftShift)
            //{
            //    if (m_graphic.IsCursorEnable)
            //    {
            //        m_graphic.IsCursorEnable = false;
            //        txtPos.Visibility = System.Windows.Visibility.Hidden;
            //    }
            //}
        }

        private void sldrTemp_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!m_temp1SldrEventEn)
                return;
            //
            AddTemp1SpToWrList(e.NewValue);
        }

        private void sldrTemp2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!m_temp2SldrEventEn)
                return;
            //
            AddTemp2SpToWrList(e.NewValue);
        }

        bool m_temp1SldrEventEn = true;
        private void sldrTemp1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_temp1SldrEventEn = false;
        }

        private void sldrTemp1_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            m_temp1SldrEventEn = true;
            Slider sldr = sender as Slider;
            if(sldr == null)
                return;
            //
            AddTemp1SpToWrList(sldr.Value);
        }

        bool m_temp2SldrEventEn = true;
        private void sldrTemp2_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_temp2SldrEventEn = false;
        }

        private void sldrTemp2_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            m_temp2SldrEventEn = true;
            Slider sldr = sender as Slider;
            if (sldr == null)
                return;
            //
            AddTemp2SpToWrList(sldr.Value);
        }

        bool m_flowTotalEventEn = true;
        private void sldrFlowTotal_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_flowTotalEventEn = false;
        }

        private void sldrFlowTotal_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            m_flowTotalEventEn = true;
            Slider sldr = sender as Slider;
            if (sldr == null)
                return;
            //
            AddFlowTotalSpToWrList(sldr.Value);
        }

        private void sldrFlowTotal_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!m_flowTotalEventEn)
                return;
            
            AddFlowTotalSpToWrList(e.NewValue);
        }

        bool m_flowInEventEn = true;
        private void sldrFlowIn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_flowInEventEn = false;
        }

        private void sldrFlowIn_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            m_flowInEventEn = true;
            Slider sldr = sender as Slider;
            if (sldr == null)
                return;
            //
            AddFlowInSpToWrList(sldr.Value);
        }

        private void sldrFlowIn_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!m_flowInEventEn)
                return;

            AddFlowTotalSpToWrList(e.NewValue);
        }

        void m_pidFactors1_KpChange(object sender, double newValue)
        {
            if (m_isFirstRead)
                return;

            PidFactors_t pf = m_sipRegs.Regs.Config.Heater1PidFactors;
            pf.kP = (float)newValue;
            ushort[] buf = SipRegisters.StructToBuff<PidFactors_t>(pf);
            AddToWrList(8460, buf);
            AddRegToWrList(156, HostControlReg0PidFactorsTempChangeBit, false);
        }

        void m_pidFactors1_KiChange(object sender, double newValue)
        {
            if (m_isFirstRead)
                return;

            PidFactors_t pf = m_sipRegs.Regs.Config.Heater1PidFactors;
            pf.kI = (float)newValue;
            ushort[] buf = SipRegisters.StructToBuff<PidFactors_t>(pf);
            AddToWrList(8460, buf);
            AddRegToWrList(156, HostControlReg0PidFactorsTempChangeBit, false);
        }

        void m_pidFactors1_KdChange(object sender, double newValue)
        {
            if (m_isFirstRead)
                return;

            PidFactors_t pf = m_sipRegs.Regs.Config.Heater1PidFactors;
            pf.kD = (float)newValue;
            ushort[] buf = SipRegisters.StructToBuff<PidFactors_t>(pf);
            AddToWrList(8460, buf);
            AddRegToWrList(156, HostControlReg0PidFactorsTempChangeBit, false);
        }

        void m_pidFactorsFi_KpChange(object sender, double newValue)
        {
            if (m_isFirstRead)
                return;

            PidFactors_t pf = m_sipRegs.Regs.Config.FlowInPidFactors;
            pf.kP = (float)newValue;
            ushort[] buf = SipRegisters.StructToBuff<PidFactors_t>(pf);
            AddToWrList(8466, buf);
            AddRegToWrList(156, HostControlReg0PidFactorsFlowInChangeBit, false);
        }

        void m_pidFactorsFi_KiChange(object sender, double newValue)
        {
            if (m_isFirstRead)
                return;

            PidFactors_t pf = m_sipRegs.Regs.Config.FlowInPidFactors;
            pf.kI = (float)newValue;
            ushort[] buf = SipRegisters.StructToBuff<PidFactors_t>(pf);
            AddToWrList(8466, buf);
            AddRegToWrList(156, HostControlReg0PidFactorsFlowInChangeBit, false);
        }

        void m_pidFactorsFi_KdChange(object sender, double newValue)
        {
            if (m_isFirstRead)
                return;

            PidFactors_t pf = m_sipRegs.Regs.Config.FlowInPidFactors;
            pf.kD = (float)newValue;
            ushort[] buf = SipRegisters.StructToBuff<PidFactors_t>(pf);
            AddToWrList(8466, buf);
            AddRegToWrList(156, HostControlReg0PidFactorsFlowInChangeBit, false);
        }

        void m_pidFactorsFt_KpChange(object sender, double newValue)
        {
            if (m_isFirstRead)
                return;

            PidFactors_t pf = m_sipRegs.Regs.Config.FlowTotalPidFactors;
            pf.kP = (float)newValue;
            ushort[] buf = SipRegisters.StructToBuff<PidFactors_t>(pf);
            AddToWrList(8472, buf);
            AddRegToWrList(156, HostControlReg0PidFactorsFlowTotalChangeBit, false);
        }

        void m_pidFactorsFt_KiChange(object sender, double newValue)
        {
            if (m_isFirstRead)
                return;

            PidFactors_t pf = m_sipRegs.Regs.Config.FlowTotalPidFactors;
            pf.kI = (float)newValue;
            ushort[] buf = SipRegisters.StructToBuff<PidFactors_t>(pf);
            AddToWrList(8472, buf);
            AddRegToWrList(156, HostControlReg0PidFactorsFlowTotalChangeBit, false);
        }

        void m_pidFactorsFt_KdChange(object sender, double newValue)
        {
            if (m_isFirstRead)
                return;

            PidFactors_t pf = m_sipRegs.Regs.Config.FlowTotalPidFactors;
            pf.kD = (float)newValue;
            ushort[] buf = SipRegisters.StructToBuff<PidFactors_t>(pf);
            AddToWrList(8472, buf);
            AddRegToWrList(156, HostControlReg0PidFactorsFlowTotalChangeBit, false);
        }

        bool m_readData = true;
        bool m_readConfig = true;
        bool m_readPidData = true;
        bool m_readAnalogData = true;
        bool m_readParams = true;
        private void chbxReadData_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chbx = sender as CheckBox;
            if (chbx == null)
                return;

            m_readData = (bool)chbx.IsChecked;
        }

        private void chbxReadConfig_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chbx = sender as CheckBox;
            if (chbx == null)
                return;

            m_readConfig = (bool)chbx.IsChecked;
        }

        private void chbxReadPidData_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chbx = sender as CheckBox;
            if (chbx == null)
                return;

            m_readPidData = (bool)chbx.IsChecked;
        }

        private void chbxReadParams_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chbx = sender as CheckBox;
            if (chbx == null)
                return;

            m_readParams = (bool)chbx.IsChecked;
        }

        private void chbxReadAnalogData_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chbx = sender as CheckBox;
            if (chbx == null)
                return;

            m_readAnalogData = (bool)chbx.IsChecked;
        }

		//   
		#endregion // Events
		//-------------------------------------------------------------------------
	}

    public enum ModbusIOType
    {
        Read,
        Write
    }

    public delegate void ModbusIOEventHandler(object sender, ModbusIOEventArgsBase e);
    public class ModbusIOEventArgs : ModbusIOEventArgsBase
    {
        public event ModbusIOEventHandler IOCallBack; 

        public void OnIOCallBack(object sender, ModbusIOEventArgsBase e)
        {
            if (IOCallBack != null)
                IOCallBack(sender, e);
        }

        public object Sender { get; set; }

        public bool IsCheckResult { get; set; }

        public bool IsComplete { get; set; }
    }

    public class ModbusIOEventArgsBase
    {
        static ModbusIOEventArgsBase()
        {
            m_id = 0;
        }

        public ModbusIOEventArgsBase()
        {
            m_id++;
        }

        static int m_id;
        public int Id {  get { return m_id; } }

        public ModbusIOType IOType { get; set; }

        public ushort[] Buf { get; set; }

        public ushort StartIndex { get; set; }

        public ushort Count { get; set; }

        public byte Addr { get; set; }

        public bool IOResult { get; set; }

        public string LastError { get; set; }
    }
}
