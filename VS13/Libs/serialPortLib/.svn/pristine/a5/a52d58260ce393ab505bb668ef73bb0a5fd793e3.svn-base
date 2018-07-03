using common;
using common.plugins;
using common.utils;
using common.utils.Files;
using common.utils.Timers;
using serialPortLib.Controls;
using serialPortLib.types;
using System;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Transport;

namespace serialPortLib
{
    public class SerialPortDevice : TransportBase, ITransport, IDisposable
    {
		public SerialPortDevice()
		{
			Transport = TransportType.SerialPort;
			connectedUri = new Uri("pack://application:,,,/serialPortLib;component/Resources/connected.png", UriKind.RelativeOrAbsolute);
			m_connectedBitmapImage = new BitmapImage(connectedUri);
			disconnectedUri = new Uri("pack://application:,,,/serialPortLib;component/Resources/disconnected.png", UriKind.RelativeOrAbsolute);
			m_disconnectedBitmapImage = new BitmapImage(disconnectedUri);
            DefaultLogFileName = "SerialPort.log";
			Init();
			//
			m_isInit = true;
			WriteTimeout = 5000;
			ReadTimeout = 5000;
		}
		//-------------------------------------------------------------------------
		#region Data
		//
		SerialPort m_serialPort;

		SerialPortParams m_spParams;

		const string ParamFileName = "serialPortparam.xml";
		string m_appDirFullName;
		string m_paramFileFullName;
		//
		readonly WpfSerialPortSettings m_wpfSettingsPage = new WpfSerialPortSettings();
		readonly SerialPortSettings m_SettingsPage = new SerialPortSettings();
		ISerialPortSettingsPage[] m_settingPages;
		//
		bool m_portReleaseFinish = true;
		bool m_portReleaseStart;
		//
		const int RingBufferSize = 1048576;
		CRingBuffer<byte> m_rxBuf;
		//
		double m_rdTimout = 0;
		readonly HiPerfTimer m_rdTimeoutTmr = new HiPerfTimer();
		//
		LogForm m_logForm;
		bool m_logFormCreateFinish;
		//
		bool m_showLogData = false;
		ByteParser m_ByteConverter = new ByteParser(4);
		//
		#endregion //Data
		//-------------------------------------------------------------------------
		#region Public
		//
		public void Dispose()
		{
			Close();
		}

		public void Close()
		{
			if (m_serialPort == null)
				return;
			//
			if (m_serialPort.IsOpen)
			{
				try
				{
					Log("Закрытие порта < " + m_serialPort.PortName + " >.");
					Log("\tПоиск порта < " + m_serialPort.PortName + " >.");
					if (FindPortName(m_serialPort.PortName))
					{
						Log("\t\tПорт найден.");
						m_serialPort.Close();
						m_serialPort.Dispose();
						Log("Порт закрыт успешно.");
					}
					else if(!m_portReleaseStart)
					{
						Log("\t\tПорт не найден.");
						Log("\t\tЗапуск фонового потока для закрытия порта при следующем подключении.");
						BackgroundWorker bgwReleaseSerialPort = new BackgroundWorker();
						bgwReleaseSerialPort.WorkerSupportsCancellation = true;
						bgwReleaseSerialPort.DoWork += bgwReleaseSerialPort_DoWork;
						bgwReleaseSerialPort.RunWorkerAsync(m_serialPort);
					}
					//
					if (!IsConnected)
						return;
					//
					m_serialPort = null;
					IsConnected = false;
					Log("Генерация события отключения устройства <Disconnect>");
					OnDisconnected(this, null);
				}
				catch (Exception ex)
				{
					IsConnected = false;
					Log("Ошибка закрытия порта < " + m_spParams.Name + " >", "\t" + ex.Message);
					MessageBox.Show(ex.Message, "Ошибка закрытия порта < " + m_spParams.Name + " >", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				//
				for (int i = 0; i < m_settingPages.Length; i++)
				{
					m_settingPages[i].SetOpenStatus(IsConnected);
				}
			}
		}

		public bool Open(bool isShowErrorMsg)
		{
			if(m_serialPort != null)
			{
				Close();
			}
			//
			if (!m_portReleaseFinish)
				return false;
			//
			CreateSerialPort();
			//
			try
			{
				Log("Открытие порта < " + m_serialPort.PortName + " >.");
				m_serialPort.Open();
                m_serialPort.BaseStream.Flush();
				Log("Порт открыт успешно.");
				IsConnected = true;
				m_IsError = false;
				Log("Генерация события подключения устройства <Сonnect>");
				OnConnected(this, null);
			}
			catch (Exception ex)
			{

				IsConnected = false;
				Log("Ошибка открытия порта < " + m_spParams.Name + " >", "\t" + ex.Message);
				if (isShowErrorMsg)
					MessageBox.Show(ex.Message, "Ошибка открытия порта < " + m_spParams.Name + " >", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			//
			for (int i = 0; i < m_settingPages.Length; i++)
			{
				m_settingPages[i].SetOpenStatus(IsConnected);
			}
			//
			return IsConnected;
		}

		public override string ToString()
		{
			return ((m_serialPort == null) ? "" : ("Последовательный порт < " + m_serialPort.PortName + " >"));
		}

		/// <summary>
		/// Функция передачи массива байт
		/// </summary>
		/// <param name="buf">Массив для пердачи.</param>
		/// <param name="index">Индекс в массиве, с которого начинаеися передача.</param>
		/// <param name="count">Количество байт для передачи.</param>
		/// <param name="result">Ссылка на строку, в которую записывается результат передачи.</param>
		/// <returns>Возвращает количество переданных байт.</returns>
		public int Write(byte[] buf, int index, int count, ref string result)
		{
			result = "";
			//
			Log("Старт записи...");
			if (!IsConnected)
			{
				Log("\tОшибка, порт закрыт...");
				result = m_spParams.Name + " закрыт.";
				return 0;
			}
			//
            HiPerfTimer tmr = new HiPerfTimer();
            double time = 0;
            bool err = false;
			try
			{
				lock (m_syncObj)
				{
                    tmr.Start();
					m_serialPort.Write(buf, index, count);
					m_serialPort.BaseStream.Flush();
                    tmr.Stop();
                    time = tmr.Duration * 1000;
                    if (WriteTimeout != System.IO.Ports.SerialPort.InfiniteTimeout)
                    {
                        if ((time + 10) > WriteTimeout)
                        {
                            err = true;
                            throw new TimeoutException("Ошибка: Истекло время передачи ( " + WriteTimeout.ToString() + " мс. )");
                        }
                    }
					result = count.ToString() + " bytes written.";
					m_IsError = false;
					Log("\tЗаписано " + count.ToString() + " Байт.");
					Log(buf, index, count);
				}
				//
				return count;
			}
			catch (Exception ex)
			{
				result = ex.Message;
                if(!err)
				    m_IsError = true;
                //
                Log(ex.Message);
				return 0;
			}
		}

		/// <summary>
		/// Функция чтения 
		/// </summary>
		/// <param name="dst">Ссылка на массив, в который записываются принятые байты.</param>
		/// <param name="index">Индекс в массиве, с которого начинается запись в массив.</param>
		/// <param name="count">Число байт для чтения.</param>
		/// <param name="result">Ссылка на строку, в которую записывается результат передачи.</param>
		/// <returns>Возвращает число прочитанных байт.</returns>
		public int Read(ref byte[] dst, int index, int count, ref string result)
		{
			if (!IsConnected)
			{
				Log("\tОшибка, порт закрыт...");
				result = m_spParams.Name + " закрыт.";
				return 0;
			}
			//
			result = "";
			//
			int uiTransmitted;
			//
			try
			{
				lock (m_syncObj)
				{
					Log("Старт чтения порта ...");
					int available = m_rxBuf.Lehgth;
					if (available >= count)
					{ // в буфере есть все данные
						Log("\tВ буфере есть все данные...");
						if(dst == null)
							dst = new byte[count];
						uiTransmitted = m_rxBuf.GetRange(ref dst, index, count);
						result = uiTransmitted.ToString() + " bytes read.";
						Log("Чтение завершено успешно.");
						Log(dst, index, uiTransmitted);
					}
					else
					{ // в буфере либо нет ничего, либо не достаточно данных
						Log("\tВ буфере не все данные (" + m_rxBuf.Lehgth.ToString() + " Б. ), ждем завершения приема...");
						m_rdTimeoutTmr.Stop();
						m_rdTimout = 0;
						int waitCnt = count - m_rxBuf.Lehgth;
						while (m_rdTimout < ReadTimeout)
						{
							m_rdTimeoutTmr.Start();
							Thread.Sleep(1);
							//byte[] rBuf = new byte[waitCnt];
							//int rdLen = 0;
							//try
							//{
							//	rdLen = m_serialPort.Read(rBuf, 0, waitCnt);
							//	if (rdLen > 0)
							//	{
							//		m_rxBuf.PutRange(rBuf, 0, rdLen);
							//	}
							//}
							//catch { }
							m_rdTimeoutTmr.Stop();
							m_rdTimout += ((m_rdTimeoutTmr.Duration * 1000));
							if (m_rxBuf.Lehgth >= waitCnt)
								break;
						}
						//
						if (count > m_rxBuf.Lehgth)
						{ // Истек таймаут и не все данные приняты
							if (m_rxBuf.Lehgth == 0)
							{
								Log("\tИстек таймаут ожидания, устройство не отвечает.");
								uiTransmitted = 0;
							}
							else
							{
								Log("\tИстек таймаут ожидания, данные приняты не полностью.");
								Log("\tПринято < " + m_rxBuf.Lehgth.ToString() + " Байт.>");
								Log("\tКопируем то, что пришло.");
								if (dst == null)
									dst = new byte[m_rxBuf.Lehgth];
								uiTransmitted = m_rxBuf.GetRange(ref dst, index, m_rxBuf.Lehgth);
								Log(dst, index, uiTransmitted);
							}
							result = "Таймаут ожидания < " + ReadTimeout.ToString() + "мс. > ( принято :" + uiTransmitted.ToString() + " bytes. )";
						}
						else
						{ // Все данные приняты
							if (dst == null)
								dst = new byte[count];
							uiTransmitted = m_rxBuf.GetRange(ref dst, index, count);
							result = uiTransmitted.ToString() + " bytes read.";
							Log("Чтение завершено успешно ( " + count.ToString() + " Б. )");
							Log(dst, index, uiTransmitted);
						}
					}
				}
				//
				m_IsError = false;
				return uiTransmitted;
			}
			catch (Exception ex)
			{
				m_IsError = true;
				result = ex.Message;
				return 0;
			}
		}

		/// <summary>
		/// Функция отображает страницу настроек для приложения WPF
		/// </summary>
		/// <param name="parent">Grid, на котором отображается страница настроек</param>
		public void ShowWPFSettingsPage(System.Windows.Controls.Grid parent)
		{
			m_wpfParent = parent;
			if (parent == null)
				return;
			//
			m_wpfParent.Children.Clear();
			m_wpfParent.Children.Add(m_wpfSettingsPage);
			System.Windows.Controls.Grid.SetColumn(m_wpfSettingsPage, 0);
			System.Windows.Controls.Grid.SetColumnSpan(m_wpfSettingsPage, 1);
			//
			System.Windows.Controls.Grid.SetRow(m_wpfSettingsPage, 0);
			System.Windows.Controls.Grid.SetRowSpan(m_wpfSettingsPage, 1);
			//
			m_wpfSettingsPage.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
			m_wpfSettingsPage.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
			m_wpfSettingsPage.Margin = new System.Windows.Thickness(0, 0, 0, 0);
		}

		/// <summary>
		/// Функция отображает страницу настроек для приложения Windows Forms
		/// </summary>
		/// <param name="parent">Panel, на которой отображается страница настроек</param>
		public void ShowSettingsPage(System.Windows.Forms.Panel parent)
		{
			m_SettingsPage.Parent = parent;
		}

		public bool Open(object param)
		{
			if (param == null)
				return false;
			//
			SerialPortParams spp = param as SerialPortParams;
			if (spp == null)
				return false;
			//
			Close();
			m_portReleaseStart = false;
			m_portReleaseFinish = true;
			m_spParams = new SerialPortParams(spp);
			CreateSerialPort();
			//
			UpdateUI();
			return Open(true);
		}

		public object GetParams()
		{
			SerialPortParams spp = new SerialPortParams(m_spParams);
			return spp;
		}

		public void ResetRxBuffer()
		{
			if (m_rxBuf == null)
				return;
			//
			m_rxBuf.Clear();
		}
		//
		#endregion // Public
		//-------------------------------------------------------------------------
		#region Private
		//
		void Init()
		{
			m_rxBuf = new CRingBuffer<byte>(RingBufferSize);
			InitSettingsPages();
			InitStrings();
			//
			if (!LoadParam(m_paramFileFullName))
			{
				m_spParams = new SerialPortParams();
				SaveParam();
			}
			//
			CreateSerialPort();
			UpdateUI();
			UpdateSerialPortParam();
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
			FileInfo fi = new FileInfo(System.Windows.Forms.Application.ExecutablePath);
			m_appDirFullName = fi.DirectoryName;
			m_paramFileFullName = m_appDirFullName + "\\" + ParamFileName;
		}

		void InitSettingsPages()
		{
			m_settingPages = new ISerialPortSettingsPage[2];
			m_settingPages[0] = m_wpfSettingsPage;
			m_settingPages[1] = m_SettingsPage;
			//
			for (int i = 0; i < m_settingPages.Length; i++)
			{
				m_settingPages[i].PortNameChange += SerialPortDevice_PortNameChange;
				m_settingPages[i].BaudRateChange += SerialPortDevice_BaudRateChange;
				m_settingPages[i].DataBitsChange += SerialPortDevice_DataBitsChange;
				m_settingPages[i].HandshakeChange += SerialPortDevice_HandshakeChange;
				m_settingPages[i].OpenStatusChange += SerialPortDevice_OpenStatusChange;
				m_settingPages[i].ParityChange += SerialPortDevice_ParityChange;
				m_settingPages[i].StopBitsChange += SerialPortDevice_StopBitsChange;
				m_settingPages[i].LogEnableChange += SerialPortDevice_LogEnableChange;
				m_settingPages[i].ShowLogDataChange += SerialPortDevice_ShowLogDataChange;
			}
		}

		void SetBaudRate(Baudrates val)
		{
			try
			{
				m_serialPort.BaudRate = (int)val;
				m_spParams.Baudrate = val;
				UpdateUI();
				SaveParam();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка установки скорости порта < " + m_spParams.Name + " >", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		void UpdateUI()
		{
			for (int i = 0; i < m_settingPages.Length; i++)
			{
				m_settingPages[i].SpParams = m_spParams;
			}
		}

		void SetDataBits(DataBits value)
		{
			try
			{
				Log("Изменение настроек порта: < DataBits = " + ((int)value).ToString() + " >");
				m_serialPort.DataBits = (int)value;
				m_spParams.DataBits = value;
				UpdateUI();
				SaveParam();
				Log("\tВыполнено успешно");
			}
			catch (Exception ex)
			{
				Log("\tОшибка...");
				Log("\t" + ex.Message + "...");
				MessageBox.Show(ex.Message, "Ошибка установки количества бит данных порта < " + m_spParams.Name + " >", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		void SetHandshake(Handshake value)
		{
			try
			{
				Log("Изменение настроек порта: < Handshake = " + value.ToString() + " >");
				m_serialPort.Handshake = value;
				m_spParams.Handshake = value;
				UpdateUI();
				SaveParam();
				Log("\tВыполнено успешно");
			}
			catch (Exception ex)
			{
				Log("\tОшибка...");
				Log("\t" + ex.Message + "...");
				MessageBox.Show(ex.Message, "Ошибка установки свойства управления потоком порта < " + m_spParams.Name + " >", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		void SetParity(Parity value)
		{
			try
			{
				Log("Изменение настроек порта: < Parity = " + value.ToString() + " >");
				m_serialPort.Parity = value;
				m_spParams.Parity = value;
				UpdateUI();
				SaveParam();
				Log("\tВыполнено успешно");
			}
			catch (Exception ex)
			{
				Log("\tОшибка...");
				Log("\t" + ex.Message + "...");
				MessageBox.Show(ex.Message, "Ошибка установки паритета четности порта < " + m_spParams.Name + " >", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		void SetStopBits(StopBits value)
		{
			try
			{
				Log("Изменение настроек порта: < StopBits = " + value.ToString() + " >");
				m_serialPort.StopBits = value;
				m_spParams.StopBits = value;
				UpdateUI();
				SaveParam();
				Log("\tВыполнено успешно");
			}
			catch (Exception ex)
			{
				Log("\tОшибка...");
				Log("\t" + ex.Message + "...");
				MessageBox.Show(ex.Message, "Ошибка установки количества стоп бит порта < " + m_spParams.Name + " >", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
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
				Close();
				string portName = m_serialPort.PortName;
				try
				{
					m_serialPort.PortName = m_spParams.Name;
				}
				catch
				{
					m_serialPort.PortName = portName;
				}
				if(lastIsOpen)
					Open(true);
			}
		}

		protected override void UpdateTimeouts()
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

		protected override void CheckConnection()
		{
			if (!m_IsError)
				return;
			//
			Close();
			Open(false);
		}

		protected override bool CheckReaderIsOpen()
		{
			if (m_serialPort == null)
				return false;
			//
			return m_serialPort.IsOpen;
		}

		protected override bool CheckWriterIsOpen()
		{
			if (m_serialPort == null)
				return false;
			//
			return m_serialPort.IsOpen;
		}

		protected override bool AssyncRead(AssyncIOEventArgs ioArgs)
		{
			if (!base.AssyncRead(ioArgs))
				return false;
			//
            TransportReadEventArgs rdEa;
            try
            {
                byte[] readBuffer = new byte[ioArgs.Count];
                int uiTransmitted = m_serialPort.Read(readBuffer, 0, ioArgs.Count);
                byte[] rdBuf = readBuffer;
                string result = "Чтение прошло успешно";
                if (uiTransmitted != ioArgs.Count)
                {
                    rdBuf = new byte[uiTransmitted];
                    Array.Copy(readBuffer, 0, rdBuf, 0, uiTransmitted);
                    result = "Прочитано не полностью";
                }
                m_IsError = false;
                rdEa = new TransportReadEventArgs(ioArgs.ID, rdBuf, true, result);
            }
            catch (Exception exRd)
            {
                m_IsError = true;
                rdEa = new TransportReadEventArgs(ioArgs.ID, null, false, exRd.Message);
            }
            //
            OnAssyncReadResultEvent((ITransport)this, rdEa);
            //
            return !m_IsError;
			//return false;
		}

		protected override bool AssyncWrite(AssyncIOEventArgs ioArgs)
		{
			if (!base.AssyncWrite(ioArgs))
				return false;
			//
			TransportWriteEventArgs wrEa;
			try
			{
				m_serialPort.Write(ioArgs.Buf, ioArgs.Index, ioArgs.Count);
				string result = ioArgs.Count.ToString() + " bytes written.";
				m_IsError = false;
				wrEa = new TransportWriteEventArgs(ioArgs.ID, false,result);
			}
			catch (Exception exWr)
			{
				m_IsError = true;
				wrEa = new TransportWriteEventArgs(ioArgs.ID, false, exWr.Message);
			}
			//
			OnAssyncWriteResultEvent((ITransport)this, wrEa);
			//
			return !m_IsError;
		}

		bool FindPortName(string name)
		{
			if(string.IsNullOrEmpty(name))
				return false;
			//
			string[] ports = SerialPort.GetPortNames();
			bool found = false;
			for (int i = 0; i < ports.Length; i++)
			{
				if(ports[i] != name)
					continue;
				//
				found = true;
				break;
			}
			//
			return found;
		}

		void CreateSerialPort()
		{
			if (m_serialPort != null)
			{
				try
				{
					//m_serialPort.DataReceived -= m_serialPort_DataReceived;
                    Close();
				}
				catch {}
			}
			//
			m_serialPort = new SerialPort();
            m_rxBuf.Clear();
			UpdateTimeouts();
			UpdateSerialPortParam();
            m_serialPort.DataReceived += m_serialPort_DataReceived;
            m_serialPort.ErrorReceived += m_serialPort_ErrorReceived;
            GC.Collect();
		}

        protected override void CreateLogWnd()
		{
			m_logFormCreateFinish = false;
			m_logForm = new LogForm();
			m_logForm.Shown += m_logForm_Shown;
			int w = m_logForm.Width;
			int h = m_logForm.Height;
			int top = m_logForm.Top;
			int left = m_logForm.Left;
			//
			int monitorH = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
			int monitorW = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
			//
			m_logForm.Left = monitorW - w - 10;
			m_logForm.Top = 10;
			//
			m_logForm.Show();
		}

		protected override void ReleaseLogWnd()
		{
			if (m_logForm == null)
				return;
			//
			m_logFormCreateFinish = false;
			m_logForm.CloseWindow();
			m_logForm = null;
			GC.Collect();
		}

		void Log(params string[] src)
		{
			if (!IsLoggerEn)
				return;
			//
            LogToFile(src);
            //
			if (!m_logFormCreateFinish)
				return;
			//
			m_logForm.Log(src);
		}

		void Log(byte[] buf, int index, int count)
		{
            string[] strs = Convert(buf, index, count);
            if(!m_showLogData)
                LogToFile(strs);
            //
            if ((buf == null) || (buf.Length <= 0) || (!m_showLogData))
				return;
			//
			Log(strs);
		}

        string[] Convert(byte[] buf, int index, int count)
        {
            if ((buf == null) || (buf.Length <= 0))
                return null;
            //
            string[] strs = m_ByteConverter.Convert(buf, index, count, false);
            for (int i = 0; i < strs.Length; i++)
            {
                strs[i] = "\t\t" + strs[i];
            }
            //
            return strs;
        }
		//
		#endregion // Private
		//-------------------------------------------------------------------------
		#region Events
		//
		void SerialPortDevice_PortNameChange(object sender, string value)
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
			Close();
			if (m_serialPort == null)
			{
				CreateSerialPort();
			}
			m_serialPort.PortName = value;
			m_spParams.Name = value;
			//
			if (lastIsOpen)
				Open(true);
			//
			UpdateUI();
			SaveParam();
		}

		void SerialPortDevice_BaudRateChange(object sender, Baudrates value)
		{
			SetBaudRate(value);
		}

		void SerialPortDevice_DataBitsChange(object sender, DataBits value)
		{
			SetDataBits(value);
		}

		void SerialPortDevice_HandshakeChange(object sender, Handshake value)
		{
			SetHandshake(value);
		}

		bool SerialPortDevice_OpenStatusChange(object sender, bool value)
		{
			if (value)
			{
				m_portReleaseStart = false;
				m_portReleaseFinish = true;
				bool result = Open(true);
				UpdateUI();
				SaveParam();
				return result;
			}
			//
			if (m_serialPort == null)
			{
				m_spParams.Name = "";
				UpdateUI();
				return true;
			}
			//
			Close();
			return ((m_serialPort == null) ? true : !m_serialPort.IsOpen);
		}

		void SerialPortDevice_ParityChange(object sender, Parity value)
		{
			SetParity(value);
		}

		void SerialPortDevice_StopBitsChange(object sender, StopBits value)
		{
			SetStopBits(value);
		}

		void bgwReleaseSerialPort_DoWork(object sender, DoWorkEventArgs e)
		{
			SerialPort sp = e.Argument as SerialPort;
			if (sp == null)
				return;
			//
			try
			{
				Log("\t\t\tПоток запущен.");
				m_portReleaseStart = true;
				m_portReleaseFinish = false;
				sp.Close();
				sp.Dispose();
			}
			catch
			{

			}
			m_portReleaseStart = false;
			m_portReleaseFinish = true;
		}

		void m_serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			int byteCount = m_serialPort.BytesToRead;
			while (byteCount > 0)
			{
				m_rdTimout = 0;
				byte[] buf = new byte[byteCount];
				try
				{
					int rdLen = m_serialPort.Read(buf, 0, byteCount);
					m_rxBuf.PutRange(buf, 0, rdLen);
					//
					DataRecivedEventArgs drea = new DataRecivedEventArgs(buf, rdLen);
					OnDataRecived(this, drea);
					//
					Thread.Sleep(0);
					byteCount = m_serialPort.BytesToRead;
				}
				catch
				{
					byteCount = 0;
					break;
				}
			}
		}

        void m_serialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            Log("Ошибка приема < " + e.ToString() + " >");
        }

		void SerialPortDevice_LogEnableChange(object sender, bool value)
		{
			IsLoggerEn = value;
		}

		void m_logForm_Shown(object sender, EventArgs e)
		{
			m_logFormCreateFinish = true;
			m_logForm.ClearLog();
		}

		void SerialPortDevice_ShowLogDataChange(object sender, bool value)
		{
			m_showLogData = value;
		}

		public event DataRecivedEventHandler DataRecived;
		protected void OnDataRecived(object sender, DataRecivedEventArgs e)
		{
			if (DataRecived != null)
				DataRecived(sender, e);
		}
		//
		#endregion // Events
		//-------------------------------------------------------------------------
    }

	public class DataRecivedEventArgs
	{
		public DataRecivedEventArgs()
		{

		}

		public DataRecivedEventArgs(byte[] buf, int count)
		{
			Data = buf;
			Count = count;
		}
		public byte[] Data { get; set; }
		public int Count { get; set; }
	}

	public delegate void DataRecivedEventHandler(object sender, DataRecivedEventArgs e);
}
