using System;
using System.Xml.Serialization;

namespace PIDLibrary.Pravate
{
	[Serializable]
	public class PIDFactors : IPIDFactors
	{
		public PIDFactors()
		{
			this.kp = 1.0;
			this.ki = 1.0;
			this.kd = 1.0;
		}

		public PIDFactors(double kp, double ki, double kd)
		{
			this.kp = kp;
			this.ki = ki;
			this.kd = kd;
		}
		//-------------------------------------------------------------------------
		double kp;
		[XmlElement(DataType = "double")]
		public double Kp
		{
			get { return kp; }
			set
			{
				double old = kp;
				kp = value;
				if (kp != old)
					OnKpChange(this, kp);
			}
		}

		double ki;
		[XmlElement(DataType = "double")]
		public double Ki
		{
			get { return ki; }
			set 
			{
				double old = ki;
				ki = value;
				if (ki != old)
					OnKiChange(this, ki);
			}
		}

		double kd;
		[XmlElement(DataType = "double")]
		public double Kd
		{
			get { return kd; }
			set
			{
				double old = kd;
				kd = value;
				if (kd != old)
					OnKdChange(this, kd);
			}
		}
		//-------------------------------------------------------------------------
		public event PIDFactorChangeEventHandler KpChange;
		protected void OnKpChange(object sender, double newValue)
		{
			if (KpChange != null)
				KpChange(sender, newValue);
		}

		public event PIDFactorChangeEventHandler KiChange;
		protected void OnKiChange(object sender, double newValue)
		{
			if (KiChange != null)
				KiChange(sender, newValue);
		}

		public event PIDFactorChangeEventHandler KdChange;
		protected void OnKdChange(object sender, double newValue)
		{
			if (KdChange != null)
				KdChange(sender, newValue);
		}
		//-------------------------------------------------------------------------
	}

	public delegate void PIDFactorChangeEventHandler(object sender, double newValue);
}
