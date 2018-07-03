
namespace PIDLibrary
{
	public interface IPID
	{
		double PGain { get; set; }

		double IGain { get; set; }

		double DGain { get; set; }

		double PVMax { get; set; }

		double PVMin { get; set; }

		double OutMax { get; set; }

		double OutMin { get; set; }

		bool PIDOK { get; }

		void Enable();

		void Disable();

		void Reset();

		double PTerm { get; }

		double ITerm { get; }

		double DTerm { get; }

		bool IntegralCorrectorEn { get; set; }

		double SetPoint { get; set; }

		double ProcessValue { get; set; }

		double OutValues { get; set; }
	}
}
