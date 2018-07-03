using common.plugins;
using serialPortLib.types;
using System;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;

namespace serialPortLib.Controls
{
	/// <summary>
	/// Логика взаимодействия для SerialPortSettings.xaml
	/// </summary>
	public partial class WpfSerialPortSettings : UserControl, ISerialPortSettingsPage
	{
		public WpfSerialPortSettings()
		{
			InitializeComponent();
			//
			Init();
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
			get { return ((chbxLogEnable == null) ? false : ((bool)chbxLogEnable.IsChecked)); }
			set
			{
				if (chbxLogEnable == null)
					return;
				//
				m_lockEvents = true;
				chbxLogEnable.IsChecked = value;
				m_lockEvents = false;
			}
		}
		//
		#endregion
		//-------------------------------------------------------------------------
		#region Public
		//
		public void SetOpenStatus(bool value)
		{
			if (value)
			{
				this.Dispatcher.BeginInvoke(new Action(() =>
				{
					btnClose.IsEnabled = true;
					btnOpen.IsEnabled = false;
				}), System.Windows.Threading.DispatcherPriority.Send);
				return;
			}
			//
			this.Dispatcher.BeginInvoke(new Action(() =>
			{
				btnClose.IsEnabled = false;
				btnOpen.IsEnabled = true;
			}), System.Windows.Threading.DispatcherPriority.Send);
		}
		//
		#endregion
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
			int selected = -1;
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
		#endregion
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

		void CbxSerialPortsSelectedIndexChanged(object sender, SelectionChangedEventArgs e)
		{
			if (m_lockEvents)
				return;
			//
			ComboBox cbx = sender as ComboBox;
			if (cbx == null)
				return;
			//
			if (cbx.SelectedItem == null)
				return;
			//
			string port = cbx.SelectedItem.ToString();
			OnPortNameChange(this, port);
		}

		void CbxBaudRateSelectedIndexChanged(object sender, SelectionChangedEventArgs e)
		{
			if (m_lockEvents)
				return;
			//
			ComboBox cbx = sender as ComboBox;
			if (cbx == null)
				return;
			//
			if (cbx.SelectedItem == null)
				return;
			//
			Baudrates bdr = (Baudrates)cbx.SelectedItem;
			OnBaudRateChange(this, bdr);
		}

		void CbxDataBitsSelectedIndexChanged(object sender, SelectionChangedEventArgs e)
		{
			if (m_lockEvents)
				return;
			//
			ComboBox cbx = sender as ComboBox;
			if (cbx == null)
				return;
			//
			if (cbx.SelectedItem == null)
				return;
			//
			DataBits db = (DataBits)cbx.SelectedItem;
			OnDataBitsChange(this, db);
		}

		void CbxParitySelectedIndexChanged(object sender, SelectionChangedEventArgs e)
		{
			if (m_lockEvents)
				return;
			//
			ComboBox cbx = sender as ComboBox;
			if (cbx == null)
				return;
			//
			if (cbx.SelectedItem == null)
				return;
			//
			Parity pr = (Parity)cbx.SelectedItem;
			OnParityChange(this, pr);
		}

		void CbxStopBitsSelectedIndexChanged(object sender, SelectionChangedEventArgs e)
		{
			if (m_lockEvents)
				return;
			//
			ComboBox cbx = sender as ComboBox;
			if (cbx == null)
				return;
			//
			if (cbx.SelectedItem == null)
				return;
			//
			StopBits st = (StopBits)cbx.SelectedItem;
			OnStopBitsChange(this, st);
		}

		void CbxHandshakeSelectedIndexChanged(object sender, SelectionChangedEventArgs e)
		{
			if (m_lockEvents)
				return;
			//
			ComboBox cbx = sender as ComboBox;
			if (cbx == null)
				return;
			//
			if (cbx.SelectedItem == null)
				return;
			//
			Handshake st = (Handshake)cbx.SelectedItem;
			OnHandshakeChange(this, st);
		}

		void btnOpen_Click(object sender, RoutedEventArgs e)
		{
			if (m_lockEvents)
				return;
			//
			bool result = OnOpenStatusChange(this, true);
			btnOpen.IsEnabled = !result;
			btnClose.IsEnabled = result;
		}

		void btnClose_Click(object sender, RoutedEventArgs e)
		{
			if (m_lockEvents)
				return;
			//
			bool result = OnOpenStatusChange(this, false);
			btnOpen.IsEnabled = result;
			btnClose.IsEnabled = !result;
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

		private void chbxLogEnable_Click(object sender, RoutedEventArgs e)
		{
			if (m_lockEvents)
				return;
			//
			CheckBox chbx = sender as CheckBox;
			if (chbx == null)
				return;
			//
			OnLogEnableChange(this, (bool)chbx.IsChecked);
		}

		public event SetValueEventHandler<bool> ShowLogDataChange;
		protected void OnShowLogDataChange(object sender, bool value)
		{
			if (ShowLogDataChange != null)
				ShowLogDataChange(sender, value);
		}

		private void chbxShowData_Click(object sender, RoutedEventArgs e)
		{
			if (m_lockEvents)
				return;
			//
			CheckBox chbx = sender as CheckBox;
			if (chbx == null)
				return;
			//
			OnShowLogDataChange(this, (bool)chbx.IsChecked);
		}
		//
		#endregion //Events
		//---------------------------------------------------------------------------
	}
}
