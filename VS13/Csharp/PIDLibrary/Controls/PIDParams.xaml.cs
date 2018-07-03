using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PidLibrary;

namespace PIDLibrary.Controls
{
	/// <summary>
	/// Логика взаимодействия для PIDParams.xaml
	/// </summary>
	public partial class PIDParams : UserControl
	{
		public PIDParams()
		{
			InitializeComponent();
			SetPidValues(null);
		}
		//-------------------------------------------------------------------------
		#region Data
		//
		IPIDFactors _pidFactors;
		public IPIDFactors PidFactors
		{
			get { return _pidFactors; }
			set 
			{ 
				_pidFactors = value;
				if (_pidFactors != null)
				{
					_pidFactors.KpChange += pidFactors_KpChange;
					_pidFactors.KiChange += pidFactors_KiChange;
					_pidFactors.KdChange += pidFactors_KdChange;
					_lockEvent = true;
					//
					ntbxP.Value = (float)_pidFactors.Kp;
					ntbxI.Value = (float)_pidFactors.Ki;
					ntbxD.Value = (float)_pidFactors.Kd;
					//
					_lockEvent = false;
				}
			}
		}

		bool _lockEvent;

		enum PidParamEdit
		{
			None,
			P,
			I,
			D
		}

		PidParamEdit _pidParamEdit = PidParamEdit.None;

		double param;
		public double Param
		{
			get { return param; }
			set
			{
				param = value;
				_pidParamEdit = SetPidParam(value, _pidParamEdit);
			}
		}

		string _header;
		public string Header
		{
			get { return _header; }
			set 
			{ 
				_header = value;
				groupBox1.Header = _header;
			}
		}

		SimpleFunction _pidResetDelegate;
		public SimpleFunction PidResetDelegate
		{
			get { return _pidResetDelegate; }
			private set 
			{ 
				_pidResetDelegate = value;
				Dispatcher.BeginInvoke(new Action(() =>
				{
					if (_pidResetDelegate == null)
						btnReset.IsEnabled = false;
					else
						btnReset.IsEnabled = true;
				}), System.Windows.Threading.DispatcherPriority.Send);
			}
		}
		void OnPidResetDelegate()
		{
			if (_pidResetDelegate != null)
				_pidResetDelegate();
		}
		//
		#endregion
		//-------------------------------------------------------------------------
		#region Public
		//
		public void SetPidValues(PidParamsEventArgs e)
		{
			if (e == null)
			{
				Dispatcher.BeginInvoke(new Action(() =>
				{
					string str = DigitToString(0, 3);
					txtPvalue.Text = str;
					txtIvalue.Text = str;
					txtDvalue.Text = str;
					txtOutvalue.Text = str;
					txtPvValue.Text = str;
					txtSpvalue.Text = str;
					PidResetDelegate = null;
				}), System.Windows.Threading.DispatcherPriority.Send);
			}
			else
			{
				Dispatcher.BeginInvoke(new Action(() =>
				{
					txtPvalue.Text = DigitToString(e.PValue, 3);
					txtIvalue.Text = DigitToString(e.IValue, 3);
					txtDvalue.Text = DigitToString(e.DValue, 3);
					txtOutvalue.Text = DigitToString(e.OutValue, 3);
					txtPvValue.Text = DigitToString(e.Pv, 3);
					txtSpvalue.Text = DigitToString(e.Sp, 3);
					PidResetDelegate = e.PidResetDelegate;
				}), System.Windows.Threading.DispatcherPriority.Send);
			}
		}
		//
		#endregion
		//-------------------------------------------------------------------------
		#region Private
		//
		PidParamEdit SetPidParam(double value, PidParamEdit ppe)
		{
			if (_pidFactors == null)
				return PidParamEdit.None;
			//
			switch (ppe)
			{
				case PidParamEdit.None:
					break;
				case PidParamEdit.P:
					_pidFactors.Kp = value;
					break;
				case PidParamEdit.I:
					_pidFactors.Ki = value;
					break;
				case PidParamEdit.D:
					_pidFactors.Kd = value;
					break;
				default:
					break;
			}
			//
			return PidParamEdit.None;
		}

		static string DigitToString(double value, int digits)
		{
			if (digits < 0)
				digits = 0;
			//
			string str = (Math.Round(value, digits)).ToString();
			if (digits == 0)
				return str;
			//
			int dotIndex = GetCharIndex(str, '.', ',');
			if (dotIndex > 0)
			{
				int x = (int)value;
				if (x == value)
					str += ',';
				//
				while (dotIndex != (str.Length - (digits + 1)))
				{
					str += '0';
					dotIndex = GetCharIndex(str, '.', ',');
				}
			}
			else
			{
				str += ',';
				for (int i = 0; i < digits; i++)
				{
					str += '0';
				}
			}
			//
			return str;
		}

		static int GetCharIndex(string source, params char[] ch)
		{
			if ((string.IsNullOrEmpty(source)) || (ch == null) || (ch.Length <= 0))
				return -1;
			//
			int result = -1;
			//
			for (int i = 0; i < source.Length; i++)
			{
				bool found = false;
				for (int j = 0; j < ch.Length; j++)
				{
					if (source[i] != ch[j])
						continue;
					//
					found = true;
					break;
				}
				if (!found)
					continue;
				//
				result = i;
				break;
			}
			//
			return result;
		}
		//
		#endregion
		//-------------------------------------------------------------------------
		#region Events
		//
		void pidFactors_KdChange(object sender, double newValue)
		{
			if (_lockEvent)
				return;
			//
			ntbxD.Value = (float)newValue;
		}

		void pidFactors_KiChange(object sender, double newValue)
		{
			if (_lockEvent)
				return;
			//
			ntbxI.Value = (float)newValue;
		}

		void pidFactors_KpChange(object sender, double newValue)
		{
			if (_lockEvent)
				return;
			//
			ntbxP.Value = (float)newValue;
		}

		private void ntbxP_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			_pidParamEdit = PidParamEdit.P;
			OnParamEdit(this, null);
		}

		private void ntbxI_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			_pidParamEdit = PidParamEdit.I;
			OnParamEdit(this, null);
		}

		private void ntbxD_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			_pidParamEdit = PidParamEdit.D;
			OnParamEdit(this, null);
		}

		public event RoutedEventHandler ParamEdit;
		protected void OnParamEdit(object sender, RoutedEventArgs e)
		{
			if (ParamEdit != null)
				ParamEdit(sender, e);
		}

		private void btnReset_Click(object sender, RoutedEventArgs e)
		{
			OnPidResetDelegate();
		}

        private void ntbxP_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                try
                {
                    _lockEvent = true;
                    _pidFactors.Kp = ntbxP.Value;
                    _lockEvent = false;
                }
                catch { }
            }
        }

        private void ntbxI_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    _lockEvent = true;
                    _pidFactors.Ki = ntbxI.Value;
                    _lockEvent = false;
                }
                catch { }
            }
        }

        private void ntbxD_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    _lockEvent = true;
                    _pidFactors.Kd = ntbxD.Value;
                    _lockEvent = false;
                }
                catch { }
            }
        }
		//
		#endregion
		//-------------------------------------------------------------------------
	}
}
