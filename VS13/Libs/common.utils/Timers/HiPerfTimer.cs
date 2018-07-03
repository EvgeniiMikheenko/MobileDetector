using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;

namespace common.utils.Timers
{
	public class HiPerfTimer
	{
		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceFrequency(out long lpFrequency);

		private long startTime, stopTime;
		private long freq;

		bool isStarted;
		public bool IsStarted
		{
			get { return isStarted; }
		}

		// Constructor
		public HiPerfTimer()
		{
			startTime = 0;
			stopTime = 0;

			if (QueryPerformanceFrequency(out freq) == false)
			{
				// high-performance counter not supported 
				throw new Win32Exception();
			}
		}

		// Start the timer
		public void Start()
		{
			// lets do the waiting threads there work
			Thread.Sleep(0);

			QueryPerformanceCounter(out startTime);
			isStarted = true;
		}

		// Stop the timer
		public void Stop()
		{
			QueryPerformanceCounter(out stopTime);
			isStarted = false;
		}

		// Returns the duration of the timer (in seconds)
		public double Duration
		{
			get
			{
				return (double)(stopTime - startTime) / (double)freq;
			}
		}

		public void Reset()
		{
			QueryPerformanceCounter(out startTime);
		}

		public long GetStartTime()
		{
			long t = 0;
			QueryPerformanceCounter(out t);
			return t;
		}

		public double GetTimeLeftMs(long startTime)
		{
			long t = 0;
			long f = 0;
			QueryPerformanceCounter(out t);
			if (QueryPerformanceFrequency(out f))
				return (((double)(t - startTime) / (double)f) * 1000);
			//
			return 0;
		}

		public override string ToString()
		{
			return Duration.ToString() + " сек.";
		}
	}
}
