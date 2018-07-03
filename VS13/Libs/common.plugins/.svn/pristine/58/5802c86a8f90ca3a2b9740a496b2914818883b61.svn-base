using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Media.Imaging;
using Transport;

namespace common.plugins
{
	public class TransportBase : /* MarshalByRefObject, */ IConnectEventSupport
	{
		public TransportBase()
		{
			InitAssyncBackgroundWorker();
		}
		//-------------------------------------------------------------------------
		#region Data
		//
		TransportType m_transport = TransportType.Undefined;
		/// <summary>
		/// Тип транспорта
		/// </summary>
		public TransportType Transport
		{
			get { return m_transport; }
			protected set { m_transport = value; }
		}

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

		/// <summary>
		/// Состояние подключения
		/// </summary>
		public bool IsConnected { get; protected set; }
		//
		protected System.Windows.Controls.Grid m_wpfParent = null;
		protected System.Windows.Forms.Panel m_WfParent = null;
		//
		protected bool m_IsError = false;
		//
		protected Uri connectedUri;
		protected BitmapImage m_connectedBitmapImage;
		public BitmapImage ConnectedBitmapImage
		{
			get
			{
				return m_connectedBitmapImage;
			}
		}
		//
		protected Uri disconnectedUri;
		protected BitmapImage m_disconnectedBitmapImage;
		public BitmapImage DisconnectedBitmapImage
		{
			get
			{
				return m_disconnectedBitmapImage;
			}
		}
		//
		Queue<AssyncIOEventArgs> m_assyncIOqueue = new Queue<AssyncIOEventArgs>(1024);
		protected object m_syncObj = new object();
		BackgroundWorker m_bgwAssync;
		//
		protected bool m_isInit;
		//
		public string Name
		{
			get { return ToString(); }
		}

        public string DefaultLogFileName { get; set; }

		bool m_loggerEnable;
		public bool IsLoggerEn 
		{
			get { return m_loggerEnable; }
			protected set
			{
				m_loggerEnable = value;
				if (m_loggerEnable)
				{
					CreateLogWnd();
                    CheckFileName(DefaultLogFileName + ".log");
					return;
				}
				//
				ReleaseLogWnd();
                CheckFileName(null);
			}
		}

		protected string m_logFileName;
        protected StringFileLogger mLogger;
		//
		#endregion //Data
		//-------------------------------------------------------------------------
		#region Public
		//
		/// <summary>
		/// Функция асинхронной записи. Добавляет в очередь операцию записи, и не дожидаясь окончания
		/// возвращает управление.
		/// Результат записи возвращается в событии AssyncWriteResultEvent.
		/// </summary>
		/// <param name="buf">Массив для пердачи.</param>
		/// <param name="index">Индекс в массиве, с которого начинаеися передача.</param>
		/// <param name="count">Количество байт для передачи.</param>
		/// <param name="id">Идентификатор транзакции</param>
		public void AssyncWrite(byte[] buf, int index, int count, uint id)
		{
			m_assyncIOqueue.Enqueue(new AssyncIOEventArgs(buf, index, count, id, IOType.Write));
		}

		/// <summary>
		/// Функция асинхронного чтения. Добавляет в очередь операцию чтения, и не дожидаясь окончания
		/// чтения возвращает управление.
		/// Результат чтения возвращается в событии AssyncReadResultEvent.
		/// </summary>
		/// <param name="count">Число байт для чтения</param>
		/// <param name="id">Идентификатор транзакции</param>
		public void AssyncRead(int count, uint id)
		{
			m_assyncIOqueue.Enqueue(new AssyncIOEventArgs(null, 0, count, id, IOType.Read));
		}

		public void EnableLogger(string fName)
		{
			CheckFileName(fName);
			IsLoggerEn = true;
		}
		//
		#endregion // Public
		//-------------------------------------------------------------------------
		#region Private
		//
		void InitAssyncBackgroundWorker()
		{
			m_bgwAssync = new BackgroundWorker();
			m_bgwAssync.WorkerSupportsCancellation = true;
			m_bgwAssync.DoWork += m_bgwAssync_DoWork;
			m_bgwAssync.RunWorkerAsync();
		}

		protected virtual void CheckConnection()
		{

		}

		protected virtual bool CheckReaderIsOpen()
		{
			return false;
		}

		protected virtual bool AssyncRead(AssyncIOEventArgs ioArgs)
		{
			if (ioArgs == null)
				return false;
			//
			return true;
		}

		protected virtual bool CheckWriterIsOpen()
		{
			return false;
		}

		protected virtual bool AssyncWrite(AssyncIOEventArgs ioArgs)
		{
			if (ioArgs == null)
				return false;
			//
			return true;
		}

		protected virtual void UpdateTimeouts() { }

		protected void CheckFileName(string fName)
		{
			try
			{
				ReleaseFileLogger();
				//
				if (!string.IsNullOrEmpty(fName))
				{
					m_logFileName = fName;
					CreateFileLogger();
				}
				else
				{
					m_logFileName = null;
				}
				//

			}
			catch
			{
				ReleaseFileLogger();
				m_logFileName = null;
			}
		}

		protected virtual void CreateFileLogger() 
        {
            try
            {
                mLogger = new StringFileLogger(m_logFileName);
            }
            catch
            {
                mLogger = null;
            }
        }

		protected virtual void ReleaseFileLogger() 
        {
            if (mLogger != null)
            {
                mLogger.Dispose();
                mLogger = null;
            }
        }

		protected virtual void CreateLogWnd(){ }

		protected virtual void ReleaseLogWnd() { }

        protected virtual void LogToFile(params string[] strs)
        {
            if (mLogger == null)
                return;
            //
            mLogger.Log(strs);
        }
		//
		#endregion // Private
		//-------------------------------------------------------------------------
		#region Events
		//
		public event TransportReadEventHandler AssyncReadResultEvent;
		protected void OnAssyncReadResultEvent(object sender, TransportReadEventArgs e)
		{
			if (AssyncReadResultEvent != null)
				AssyncReadResultEvent(sender, e);
		}

		public event TransportWriteEventHandler AssyncWriteResultEvent;
		protected void OnAssyncWriteResultEvent(object sender, TransportWriteEventArgs e)
		{
			if (AssyncWriteResultEvent != null)
				AssyncWriteResultEvent(sender, e);
		}

		public event EventHandler Connected;
		protected void OnConnected(object sender, EventArgs e)
		{
			if (Connected != null)
				Connected(sender, e);
		}

		public event EventHandler Disconnected;
		protected void OnDisconnected(object sender, EventArgs e)
		{
			if (Disconnected != null)
				Disconnected(sender, e);
		}

		bool m_bgwEnable;
		void m_bgwAssync_DoWork(object sender, DoWorkEventArgs e)
		{
			m_bgwEnable = true;
			AssyncIOEventArgs ioArgs = null;
			while (m_bgwEnable)
			{
				if (m_bgwAssync.CancellationPending)
				{
					e.Cancel = true;
					break;
				}
				//
				if (!m_isInit)
				{
					Thread.Sleep(10);
					continue;
				}
				//
				CheckConnection();
				//
				if ((m_assyncIOqueue.Count <= 0))
				{ // очередь пуста
					Thread.Sleep(10);
					continue;
				}
				// В очереди есть данные
				ioArgs = m_assyncIOqueue.Dequeue();
				if (ioArgs == null)
					continue;
				//
				lock (m_syncObj)
				{
					switch (ioArgs.IoType)
					{
						case IOType.Undefined:
							break;
						case IOType.Read:
							if (ioArgs.Count <= 0)
								break;
							//
							if (!CheckReaderIsOpen())
								break;
							//
							AssyncRead(ioArgs);
							Thread.Sleep(5);
							//
							break;
						case IOType.Write:
							if (ioArgs.Buf == null)
								break;
							//
							if (!CheckWriterIsOpen())
								break;
							//
							AssyncWrite(ioArgs);
							Thread.Sleep(5);
							//
							break;
						default:
							break;
					}
				}
			}
		}
		//
		#endregion // Events
		//-------------------------------------------------------------------------
	}
}
