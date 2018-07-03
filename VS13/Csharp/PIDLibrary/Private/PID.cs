using System;
using System.Threading;

namespace PIDLibrary.Pravate
{
	public class PID : IPID
	{

		#region Fields

		//Gains
		private double kp;
		private double ki;
		private double kd;

		//Running Values
		private DateTime lastUpdate;
		private double lastPV;
		private double errSum;

		//Reading/Writing Values
		private GetDouble readPV;
		private GetDouble readSP;
		private SetDouble writeOV;

		//Max/Min Calculation
		private double pvMax;
		private double pvMin;
		private double outMax;
		private double outMin;

		//Threading and Timing
		private double computeHz = 1.0f;
		public double ComputeHz
		{
			get { return computeHz; }
			set 
			{ 
				computeHz = value;
				if (computeHz < 0.1)
					computeHz = 0.1;
			}
		}

		private Thread runThread;

		//
		bool _integralCorrectorEn = true;
		public bool IntegralCorrectorEn
		{
			get { return _integralCorrectorEn; }
			set { _integralCorrectorEn = value; }
		}

		#endregion

		#region Properties

		public double PGain
		{
			get { return kp; }
			set { kp = value; }
		}

		public double IGain
		{
			get { return ki; }
			set { ki = value; }
		}

		public double DGain
		{
			get { return kd; }
			set { kd = value; }
		}

		public double PVMin
		{
			get { return pvMin; }
			set { pvMin = value; }
		}

		public double PVMax
		{
			get { return pvMax; }
			set { pvMax = value; }
		}

		public double OutMin
		{
			get { return outMin; }
			set { outMin = value; }
		}

		public double OutMax
		{
			get { return outMax; }
			set { outMax = value; }
		}

		public bool PIDOK
		{
			get { return runThread != null; }
		}

		#endregion

		#region Construction / Deconstruction
		/// <summary>
		/// Конструктор PID регулятора
		/// </summary>
		/// <param name="pG">Пропорциональный коэффициент</param>
		/// <param name="iG">Интегральный коэффициент</param>
		/// <param name="dG">Дифференциальный коэффициент</param>
		/// <param name="pMax">Ограничение максимального текущего значения</param>
		/// <param name="pMin">Ограничение минимального текущего значения</param>
		/// <param name="oMax">Ограничение максимального выходного значения</param>
		/// <param name="oMin">Ограничение минимального выходного значения</param>
		/// <param name="pvFunc">Функция-делегат чтения текущего значения</param>
		/// <param name="spFunc">Функция-делегат чтения значения уставки</param>
		/// <param name="outFunc">Функция-делегат записи выходного значения</param>
		public PID(	double pG, double iG, double dG,
								double pMax, double pMin, 
								double oMax, double oMin,
								GetDouble pvFunc, 
								GetDouble spFunc, 
								SetDouble outFunc)
		{
			kp = pG;
			ki = iG;
			kd = dG;
			pvMax = pMax;
			pvMin = pMin;
			outMax = oMax;
			outMin = oMin;
			readPV = pvFunc;
			readSP = spFunc;
			writeOV = outFunc;
		}

		/// <summary>
		/// Деструктор
		/// </summary>
		~PID()
		{
			Disable();
			readPV = null;
			readSP = null;
			writeOV = null;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Запуск процесса вычисления
		/// </summary>
		public void Enable()
		{
			if (runThread != null)
				return;

			Reset();

			runThread = new Thread(new ThreadStart(Run));
			runThread.IsBackground = true;
			runThread.Name = "PID Processor";
			runThread.Start();
		}

		/// <summary>
		/// Остановка процесса вычисления
		/// </summary>
		public void Disable()
		{
			if (runThread == null)
				return;

			runThread.Abort();
			runThread = null;
			Reset();
		}

		bool _isFirstStart = false;
		/// <summary>
		/// Сброс PID регулятора
		/// </summary>
		public void Reset()
		{
			errSum = 0.0f;
			lastUpdate = DateTime.Now;
			_isFirstStart = true;
			_pTerm = 0;
			_iTerm = 0;
			_dTerm = 0;
			OutValues = 0;
			_integralCorrectorEn = true;
		}

		#endregion

		#region Private Methods

		private double ScaleValue(double value, double valuemin, double valuemax, double scalemin, double scalemax)
		{
			double vPerc = (value - valuemin) / (valuemax - valuemin);
			double bigSpan = vPerc * (scalemax - scalemin);

			double retVal = scalemin + bigSpan;

			return retVal;
		}

		private double Clamp(double value, double min, double max)
		{
			if (value > max)
				return max;
			if (value < min)
				return min;
			return value;
		}

		double _pTerm;
		public double PTerm
		{
			get { return _pTerm; }
			//set { pTerm = value; }
		}
		
		double _iTerm;
		public double ITerm
		{
			get { return _iTerm; }
			//set { iTerm = value; }
		}
		
		double _dTerm;
		public double DTerm
		{
			get { return _dTerm; }
			//set { dTerm = value; }
		}

		public double SetPoint { get; set; }

		public double ProcessValue { get; set; }

		public double OutValues { get; set; }

		private void Compute()
		{
			if (readPV == null || readSP == null || writeOV == null)
				return;

			double pv = readPV();
			double sp = readSP();
			//
			ProcessValue = pv;
			SetPoint = sp;
			//We need to scale the pv to +/- 100%, but first clamp it
			pv = Clamp(pv, pvMin, pvMax);
			pv = ScaleValue(pv, pvMin, pvMax, -1.0f, 1.0f);

			//We also need to scale the setpoint
			sp = Clamp(sp, pvMin, pvMax);
			sp = ScaleValue(sp, pvMin, pvMax, -1.0f, 1.0f);

			//Now the error is in percent...
			double err = sp - pv;

			double pTerm = err * kp;
			double iTerm = 0.0f;
			double dTerm = 0.0f;
			double i_tmp = 0.0f;
			double partialSum = 0.0f;
			//
			if (_isFirstStart)
			{
				_isFirstStart = false;
				lastPV = pv;
				lastUpdate = DateTime.Now;
			}
			//
			DateTime nowTime = DateTime.Now;
			//
			if (pTerm < 1.0f)
			{
				if (lastUpdate != null)
				{
					double dT = (nowTime - lastUpdate).TotalSeconds;

					//Compute the integral if we have to...
					if (pv >= pvMin && pv <= pvMax)
					{
						i_tmp = dT * err;
						partialSum = errSum + i_tmp;
						iTerm = ki * partialSum;
					}

					if (dT != 0.0f)
						dTerm = kd * (lastPV - pv) / dT;
				}
			}
			//
			lastUpdate = nowTime;
			errSum = partialSum;
			lastPV = pv;
			//
			//Now we have to scale the output value to match the requested scale
			double outReal = pTerm + iTerm + dTerm;
			double out_tmp = outReal;
			outReal = Clamp(outReal, -1.0f, 1.0f);
			outReal = ScaleValue(outReal, -1.0f, 1.0f, outMin, outMax);
			if (outReal >= outMax)
			{
				errSum -= i_tmp;
			}
			else if (_integralCorrectorEn)
			{
				if ((outReal < 0))
				{
					double kitmp = (ki == 0) ? 1 : ki;
					double kptmp = (kp == 0) ? 1 : kp;
					errSum += ((0.0f - out_tmp) / kitmp / kp);
				}
			} 
			//
			_pTerm = Clamp(pTerm, -1.0f, 1.0f);
			_pTerm = ScaleValue(_pTerm, -1.0f, 1.0f, outMin, outMax) / (outMax / 100);
			//
			_iTerm = Clamp(iTerm, -1.0f, 1.0f);
			_iTerm = ScaleValue(_iTerm, -1.0f, 1.0f, outMin, outMax) / (outMax / 100);
			//
			_dTerm = Clamp(dTerm, -1.0f, 1.0f);
			_dTerm = ScaleValue(_dTerm, -1.0f, 1.0f, outMin, outMax) / (outMax / 100);
			//Write it out to the world
			OutValues = outReal;
			writeOV(outReal);
		}

		#endregion

		#region Threading

		private void Run()
		{

			while (true)
			{
				try
				{
					int sleepTime = (int)(1000 / computeHz);
					Thread.Sleep(sleepTime);
					Compute();
				}
				catch
				{

				}
			}

		}

		#endregion

	}
}
