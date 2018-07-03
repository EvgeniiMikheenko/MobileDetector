using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PidLibrary
{
	public class PidParamsEventArgs
	{
		
		//-------------------------------------------------------------------------
		#region Data
		//
		double _pv;
		public double Pv
		{
			get { return _pv; }
			set { _pv = value; }
		}

		double _sp;
		public double Sp
		{
			get { return _sp; }
			set { _sp = value; }
		}

		double _kp;
		public double Kp
		{
			get { return _kp; }
			set { _kp = value; }
		}

		double _ki;
		public double Ki
		{
			get { return _ki; }
			set { _ki = value; }
		}

		double _kd;
		public double Kd
		{
			get { return _kd; }
			set { _kd = value; }
		}

		double _pValue;
		public double PValue
		{
			get { return _pValue; }
			set { _pValue = value; }
		}

		double _iValue;
		public double IValue
		{
			get { return _iValue; }
			set { _iValue = value; }
		}

		double _dValue;
		public double DValue
		{
			get { return _dValue; }
			set { _dValue = value; }
		}

		double _error;
		public double Error
		{
			get 
			{ 
				return (_sp - _pv); 
			}
		}

		double _outValue;
		public double OutValue
		{
			get { return _outValue; }
			set { _outValue = value; }
		}

		SimpleFunction _pidResetDelegate;
		public SimpleFunction PidResetDelegate
		{
			get { return _pidResetDelegate; }
			set { _pidResetDelegate = value; }
		}
		//
		#endregion
		//-------------------------------------------------------------------------
		#region Public
		//
		public void ResetPid()
		{
			if (_pidResetDelegate != null)
				_pidResetDelegate();
		}
		//
		#endregion
		//-------------------------------------------------------------------------
		#region Private
		//

		//
		#endregion
		//-------------------------------------------------------------------------
		#region Events
		//

		//
		#endregion
		//-------------------------------------------------------------------------
	}

	public delegate void SimpleFunction();
}
