using common.plugins;
using serialPortLib.types;
using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace serialPortLib.Controls
{
	public partial class SerialPortSettings : UserControl, ISerialPortSettingsPage
	{
		public SerialPortSettings()
		{
			InitializeComponent();
			//
			Init();
			this.Load += SerialPortSettings_Load;
		}

		//-------------------------------------------------------------------------
		#region Data
		//
		SerialPortParams m_spParams;
		public SerialPortParams SpParams
		{
			get { return m_spParams; }
			set 
			{
				m_spParams = value;
				m_lockEvents = true;
				SetUiParams();
				m_lockEvents = false;
			}
		}
		
		bool m_lockEvents;

		public bool LogEnable
		{
			get { return ((checkBox1 == null) ? false : checkBox1.Checked); }
			set
			{
				if (checkBox1 == null)
					return;
				//
				m_lockEvents = true;
				checkBox1.Checked = value;
				m_lockEvents = false;
			}
		}

		bool m_NeedUpdateOpenStatus;
		bool m_openStatus;
		//
		#endregion // Data
		//-------------------------------------------------------------------------
		#region Public
		//
		public void SetOpenStatus(bool value)
		{
			try
			{
				m_NeedUpdateOpenStatus = false;
				m_openStatus = value;
				if (value)
				{
					this.BeginInvoke(new Action(() =>
					{
						btnClose.Enabled = true;
						btnOpen.Enabled = false;
					}), null);
					return;
				}
				//
				this.BeginInvoke(new Action(() =>
				{
					btnClose.Enabled = false;
					btnOpen.Enabled = true;
				}), null);
			}
			catch
			{
				m_NeedUpdateOpenStatus = true;
			}
		}
		//
		#endregion // Public
		//-------------------------------------------------------------------------
		#region Private
		//
		void Init()
		{
			m_lockEvents = true;
			FillSerialPortsCombobox();
			FillBaudRateCombobox();
			FillDataBitsCombobox();
			FillParityCombobox();
			FillStopBitsCombobox();
			FillHandshakeCombobox();
			//
			m_lockEvents = false;
		}

		void FillSerialPortsCombobox()
		{
			string[] ports = SerialPort.GetPortNames();
			cbxSerialPorts.Items.Clear();
			if ((ports == null) || (ports.Length <= 0))
				return;
			//
			foreach (string port in ports)
			{
				cbxSerialPorts.Items.Add(port);
			}
		}

		void FillBaudRateCombobox()
		{
			cbxBaudRate.Items.Clear();
			foreach (Baudrates bdr in Enum.GetValues(typeof(Baudrates)))
			{
				cbxBaudRate.Items.Add((int)bdr);
			}
			cbxBaudRate.SelectedIndex = 0;
		}

		void FillDataBitsCombobox()
		{
			cbxDataBits.Items.Clear();
			foreach (DataBits db in Enum.GetValues(typeof(DataBits)))
			{
				cbxDataBits.Items.Add((int)db);
			}
			cbxDataBits.SelectedIndex = 0;
		}

		void FillParityCombobox()
		{
			cbxParity.Items.Clear();
			foreach (Parity pr in Enum.GetValues(typeof(Parity)))
			{
				cbxParity.Items.Add(pr);
			}
			cbxParity.SelectedIndex = 0;
		}

		void FillStopBitsCombobox()
		{
			cbxStopBits.Items.Clear();
			foreach (StopBits pr in Enum.GetValues(typeof(StopBits)))
			{
				cbxStopBits.Items.Add(pr);
			}
			cbxStopBits.SelectedIndex = 0;
		}

		void FillHandshakeCombobox()
		{
			cbxHandshake.Items.Clear();
			foreach (Handshake pr in Enum.GetValues(typeof(Handshake)))
			{
				cbxHandshake.Items.Add(pr);
			}
			cbxHandshake.SelectedIndex = 0;
		}

		void SetUiParams()
		{
			if (m_spParams == null)
				return;
			//
			int selected =  -1;
			for (int i = 0; i < cbxSerialPorts.Items.Count; i++)
			{
				string port = cbxSerialPorts.Items[i].ToString();
				if (!string.Equals(port, m_spParams.Name))
					continue;
				//
				selected = i;
				break;
			}
			cbxSerialPorts.SelectedIndex = selected;
			//
			selected = -1;
			for (int i = 0; i < cbxBaudRate.Items.Count; i++)
			{
				Baudrates bdr = (Baudrates)cbxBaudRate.Items[i];
				if (bdr != m_spParams.Baudrate)
					continue;
				//
				selected = i;
				break;
			}
			cbxBaudRate.SelectedIndex = selected;
			//
			selected = -1;
			for (int i = 0; i < cbxDataBits.Items.Count; i++)
			{
				DataBits db = (DataBits)cbxDataBits.Items[i];
				if (db != m_spParams.DataBits)
					continue;
				//
				selected = i;
				break;
			}
			cbxDataBits.SelectedIndex = selected;
			//
			selected = -1;
			for (int i = 0; i < cbxParity.Items.Count; i++)
			{
				Parity pr = (Parity)cbxParity.Items[i];
				if (pr != m_spParams.Parity)
					continue;
				//
				selected = i;
				break;
			}
			cbxParity.SelectedIndex = selected;
			//
			selected = -1;
			for (int i = 0; i < cbxStopBits.Items.Count; i++)
			{
				StopBits pr = (StopBits)cbxStopBits.Items[i];
				if (pr != m_spParams.StopBits)
					continue;
				//
				selected = i;
				break;
			}
			cbxStopBits.SelectedIndex = selected;
			//
			selected = -1;
			for (int i = 0; i < cbxHandshake.Items.Count; i++)
			{
				Handshake hs = (Handshake)cbxHandshake.Items[i];
				if (hs != m_spParams.Handshake)
					continue;
				//
				selected = i;
				break;
			}
			cbxHandshake.SelectedIndex = selected;
			//
		}
		//
		#endregion //Private
		//-------------------------------------------------------------------------
		#region Events
		//
		void CbxSerialPortsDropDown(object sender, EventArgs e)
		{
			if (m_lockEvents)
				return;
			//
			FillSerialPortsCombobox();
		}

		void CbxSerialPortsSelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_lockEvents)
				return;
			//
			ComboBox cbx = sender as ComboBox;
			if (cbx == null)
				return;
			//
			string port = cbx.SelectedItem.ToString();
			m_spParams.Name = port;
			OnPortNameChange(this, port);
		}

		void CbxBaudRateSelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_lockEvents)
				return;
			//
			ComboBox cbx = sender as ComboBox;
			if (cbx == null)
				return;
			//
			Baudrates bdr = (Baudrates)cbx.SelectedItem;
			m_spParams.Baudrate = bdr;
			OnBaudRateChange(this, bdr);
		}

		void CbxDataBitsSelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_lockEvents)
				return;
			//
			ComboBox cbx = sender as ComboBox;
			if (cbx == null)
				return;
			//
			DataBits db = (DataBits)cbx.SelectedItem;
			m_spParams.DataBits = db;
			OnDataBitsChange(this, db);
		}

		void CbxParitySelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_lockEvents)
				return;
			//
			ComboBox cbx = sender as ComboBox;
			if (cbx == null)
				return;
			//
			Parity pr = (Parity)cbx.SelectedItem;
			m_spParams.Parity = pr;
			OnParityChange(this, pr);
		}

		void CbxStopBitsSelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_lockEvents)
				return;
			//
			ComboBox cbx = sender as ComboBox;
			if (cbx == null)
				return;
			//
			StopBits st = (StopBits)cbx.SelectedItem;
			m_spParams.StopBits = st;
			OnStopBitsChange(this, st);
		}

		void CbxHandshakeSelectedIndexChanged(object sender, EventArgs e)
		{
			if (m_lockEvents)
				return;
			//
			ComboBox cbx = sender as ComboBox;
			if (cbx == null)
				return;
			//
			Handshake st = (Handshake)cbx.SelectedItem;
			m_spParams.Handshake = st;
			OnHandshakeChange(this, st);
		}
		
		void btnOpen_Click(object sender, EventArgs e)
		{
			if (m_lockEvents)
				return;
			//
			bool result = OnOpenStatusChange(this, true);
			//btnOpen.Enabled = !result;
			//btnClose.Enabled = result;
		}

		void btnClose_Click(object sender, EventArgs e)
		{
			if (m_lockEvents)
				return;
			//
			bool result = OnOpenStatusChange(this, false);
			//btnOpen.Enabled = result;
			//btnClose.Enabled = !result;
		}
		//
		public event SetValueEventHandler<string> PortNameChange;
		protected void OnPortNameChange(object sender, string value)
		{
			if (PortNameChange != null)
				PortNameChange(sender, value);
		}

		public event SetValueEventHandler<Baudrates> BaudRateChange;
		protected void OnBaudRateChange(object sender, Baudrates value)
		{
			if (BaudRateChange != null)
				BaudRateChange(sender, value);
		}

		public event SetValueEventHandler<DataBits> DataBitsChange;
		protected void OnDataBitsChange(object sender, DataBits value)
		{
			if (DataBitsChange != null)
				DataBitsChange(sender, value);
		}

		public event SetValueEventHandler<Parity> ParityChange;
		protected void OnParityChange(object sender, Parity value)
		{
			if (ParityChange != null)
				ParityChange(sender, value);
		}

		public event SetValueEventHandler<StopBits> StopBitsChange;
		protected void OnStopBitsChange(object sender, StopBits value)
		{
			if (StopBitsChange != null)
				StopBitsChange(sender, value);
		}

		public event SetValueEventHandler<Handshake> HandshakeChange;
		protected void OnHandshakeChange(object sender, Handshake value)
		{
			if (HandshakeChange != null)
				HandshakeChange(sender, value);
		}

		public event SetValueEventHandler<bool, bool> OpenStatusChange;
		protected bool OnOpenStatusChange(object sender, bool value)
		{
			if (OpenStatusChange != null)
				return OpenStatusChange(sender, value);
			//
			return true;
		}

		public event SetValueEventHandler<bool> LogEnableChange;
		protected void OnLogEnableChange(object sender, bool value)
		{
			if (LogEnableChange != null)
				LogEnableChange(sender, value);
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (m_lockEvents)
				return;
			//
			CheckBox chbx = sender as CheckBox;
			if (chbx == null)
				return;
			//
			OnLogEnableChange(this, chbx.Checked);
		}

		public event SetValueEventHandler<bool> ShowLogDataChange;
		protected void OnShowLogDataChange(object sender, bool value)
		{
			if (ShowLogDataChange != null)
				ShowLogDataChange(sender, value);
		}

		private void chbxShowData_CheckedChanged(object sender, EventArgs e)
		{
			if (m_lockEvents)
				return;
			//
			CheckBox chbx = sender as CheckBox;
			if (chbx == null)
				return;
			//
			OnShowLogDataChange(this, chbx.Checked);
		}

		void SerialPortSettings_Load(object sender, EventArgs e)
		{
			if (!m_NeedUpdateOpenStatus)
				return;
			//
			SetOpenStatus(m_openStatus);
		}
		//
		#endregion //Events
		//-------------------------------------------------------------------------
	}
}
