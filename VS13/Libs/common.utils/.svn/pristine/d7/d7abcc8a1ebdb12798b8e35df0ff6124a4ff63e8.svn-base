

namespace common.utils.Timers
{
	public class CTOn
	{
		public CTOn()
		{
			hTimer = new HiPerfTimer();
			hTimer.Reset();
			hTimer.Start();
		}
		//-------------------------------------------------------------------------
		#region Data
		//
		bool _enable;
		public bool Enable
		{
			get { return _enable; }
		}
		//
		bool _outValue;
		public bool OutValue
		{
			get { return _outValue; }
		}

		HiPerfTimer hTimer;
		long startTime;
		double timeLeft;
		public double TimeLeft
		{
			get { return timeLeft; }
		}
		//
		#endregion
		//-------------------------------------------------------------------------
		#region Public
		//
		public bool Run(bool enable, int msec)
		{
			if (!enable)
			{
				_outValue = false;
				hTimer.Reset();
				startTime = hTimer.GetStartTime();
				timeLeft = 0;
				_enable = false;
				return false;
			}
			//
			if (_outValue)
				return true;
			//
			timeLeft = hTimer.GetTimeLeftMs(startTime);
			if (!_enable)
			{
				_enable = enable;
				if (startTime <= 0)
					startTime = hTimer.GetStartTime();
				return false;
			}
			_enable = enable;
			//
			if (timeLeft >= msec)
				_outValue = true;
			//
			return _outValue;
		}

		public void Reset()
		{
			_outValue = false;
			hTimer.Reset();
			startTime = hTimer.GetStartTime();
			timeLeft = 0;
			_enable = false;
		}

		public override string ToString()
		{
			return timeLeft.ToString();
		}
		//
		#endregion
		//-------------------------------------------------------------------------
		#region Private
		//

		//
		#endregion
		//-------------------------------------------------------------------------
	}
}
