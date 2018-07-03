
namespace PIDLibrary
{
	public interface IPidConstructor
	{
		IPID Create(double kp, double i, double d, double pMax, double pMin, double oMax, double oMin, GetDouble GetCurrentValueFunc, GetDouble GetSetPointFunc, SetDouble SetProcessValue);
	}
}
