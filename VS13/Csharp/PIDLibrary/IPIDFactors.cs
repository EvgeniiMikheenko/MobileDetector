using PIDLibrary.Pravate;

namespace PIDLibrary
{
	public interface IPIDFactors
	{
		double Kd { get; set; }
		event PIDFactorChangeEventHandler KdChange;
		double Ki { get; set; }
		event PIDFactorChangeEventHandler KiChange;
		double Kp { get; set; }
		event PIDFactorChangeEventHandler KpChange;
	}
}
