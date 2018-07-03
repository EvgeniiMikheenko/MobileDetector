using PIDLibrary.Pravate;

namespace PIDLibrary
{
	public class PidConstructor : IPidConstructor
	{
		public IPID Create(double kp, double i, double d, double pMax, double pMin, double oMax, double oMin, GetDouble GetCurrentValueFunc, GetDouble GetSetPointFunc, SetDouble SetProcessValue)
		{
			IPID ipid = new PID(kp, i, d, pMax, pMin, oMax, oMin, GetCurrentValueFunc, GetSetPointFunc, SetProcessValue);
			//
			return ipid;
		}
	}
}
