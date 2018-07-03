using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SipTest
{
	public class SipRegisters
	{

		//-------------------------------------------------------------------------
		#region Data
		//
		const int FpfaOutIndex = 0;
		const int CpuInIndex = 61;
		const int CrcIndex = 126;
		const int CpuOutIndex = 127;
		const int FpgaInIndex = 190;

		const int Kp1Index = 0;
		const int Ki1Index = 1;
		const int Kd1Index = 2;
		const int P1ValueIndex = 3;
		const int I1ValueIndex = 4;
		const int D1ValueIndex = 5;
		const int ErrSumm1Index = 6;
		const int Pwr1Index = 7;

        const int CpuOutDummyTempSetupTimeIndex         = 7;
        const int CpuOutDummyModeOkWorkTime             = 8;
        const int CpuOutDummy1AppConfigIndex            = 9;

		//FpgaOut m_fpgaOut = new FpgaOut();
		//CpuIn m_cpuIn = new CpuIn();
		//ushort m_crc = 0xFFFF;
		//CpuOut m_cpuOut = new CpuOut();
		//FpgaIn m_fpgaIn = new FpgaIn();

        //
        SipRegisters_t m_regs = new SipRegisters_t();
        public SipRegisters_t Regs
        {
            get
            {
                return m_regs;
            }
        }

		public FpgaOut FpgaOut
		{
			get { return m_regs.FPGA_OUT; }
			//set { m_fpgaOut = value; }
		}
		public CpuIn CpuIn
		{
			get { return m_regs.CPU_IN; }
			//set { m_cpuIn = value; }
		}
		public ushort Crc
		{
			get { return m_regs.CRC1; }
			//set { m_crc = value; }
		}
		public CpuOut CpuOut
		{
			get { return m_regs.CPU_OUT; }
			//set { m_cpuOut = value; }
		}
		public FpgaIn FpgaIn
		{
			get { return m_regs.FPGA_IN; }
			//set { m_fpgaIn = value; }
		}

		public double Heater1Kp 
		{ 
			get 
			{ 
                //if(m_regs.CPU_OUT.Dummy1 == null)
                //    return 0;

                //if (m_regs.CPU_OUT.Dummy1.Length < Kp1Index)
                //    return 0;

                return (double)m_regs.Config.Heater1PidFactors.kP;
			} 
		}
		public double Heater1Ki 
		{ 
			get 
			{
                //if (m_regs.CPU_OUT.Dummy1 == null)
                //    return 0;

                //if (m_regs.CPU_OUT.Dummy1.Length < Kp1Index)
                //    return 0;

                return (double)m_regs.Config.Heater1PidFactors.kI;
			} 
		}
		public double Heater1Kd 
		{ 
			get 
			{ 
                //if(m_cpuOut.Dummy1 == null)
                //    return 0;

                //if (m_cpuOut.Dummy1.Length < Kp1Index)
                //    return 0;

                return (double)m_regs.Config.Heater1PidFactors.kD; 
			} 
		}
		public double Heater1PValue 
		{ 
			get 
			{ 
                //if(m_cpuOut.Dummy1 == null)
                //    return 0;

                //if (m_cpuOut.Dummy1.Length < Kp1Index)
                //    return 0;

                return (double)m_regs.Heater1PidData.pValue;
			} 
		}
		public double Heater1IValue 
		{ 
			get 
			{ 
                //if(m_cpuOut.Dummy1 == null)
                //    return 0;

                //if (m_cpuOut.Dummy1.Length < Kp1Index)
                //    return 0;

                return (double)m_regs.Heater1PidData.iValue;
			} 
		}
		public double Heater1DValue 
		{ 
			get 
			{ 
                //if(m_cpuOut.Dummy1 == null)
                //    return 0;

                //if (m_cpuOut.Dummy1.Length < Kp1Index)
                //    return 0;

                return (double)m_regs.Heater1PidData.dValue;
			} 
		}
		public double Heater1ErrSumm 
		{ 
			get 
			{ 
                //if(m_cpuOut.Dummy1 == null)
                //    return 0;

                //if (m_cpuOut.Dummy1.Length < Kp1Index)
                //    return 0;

                return (double)m_regs.Heater1PidData.errSumm;
			} 
		}
		public double Heater1OutValue 
		{ 
			get 
			{ 
                //if(m_cpuOut.Dummy1 == null)
                //    return 0;

                //if (m_cpuOut.Dummy1.Length < Kp1Index)
                //    return 0;

                return (double)m_regs.Heater1PidData.outValue;
			} 
		}
		
        public int TempSetupTime
        {
            get 
            {
                try
                {
                    return (int)m_regs.CPU_OUT.Dummy1[CpuOutDummyTempSetupTimeIndex];
                }
                catch { return 0; }
            }
        }

        public int ModeOkWorkTime
        {
            get 
            {
                try
                {
                    return (int)m_regs.CPU_OUT.Dummy1[CpuOutDummyModeOkWorkTime];
                }
                catch { return 0; }
            }
        }
        
        public bool IsAnalogDataSupport
        {
            get { return (m_regs.CPU_OUT.Dummy1[CpuOutDummy1AppConfigIndex] & (1 << 3)) != 0; }
        }
        //
		#endregion //Data
		//-------------------------------------------------------------------------
		#region Public
		//
		public void Build(ushort[] buf, int regStart, int count)
		{
			if((buf == null) || (buf.Length <= 0))
			{
				return;
			}
			//
            ushort[] tBuf = StructToBuff<SipRegisters_t>(m_regs);
            //
            int end = regStart + count;
            for (int i = regStart, j = 0; i < end; i++, j++)
            {
                if (i >= tBuf.Length)
                    break;
                //
                tBuf[i] = buf[j];
            }
            //
            m_regs = BufToStruct<SipRegisters_t>(tBuf);
		}

		public int GetBufSize()
		{
            int result = Marshal.SizeOf(m_regs);
			return result;
		}

		public static T BufToStruct<T>(ushort[] buf) where T : struct
		{
			GCHandle handle = GCHandle.Alloc(buf, GCHandleType.Pinned);		// Выделить память

			IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buf, 0);	// и взять адрес
			T ret = (T)Marshal.PtrToStructure(ptr, typeof(T));				// создать структуру
			handle.Free();													// Освобождить дескриптор

			return ret;
		}

		public static ushort[] StructToBuff<T>(T value) where T : struct
		{

			ushort[] buf = new ushort[Marshal.SizeOf(value) / 2];

			GCHandle handle = GCHandle.Alloc(buf, GCHandleType.Pinned);		// Выделить память
			IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buf, 0);	// и взять адрес
			Marshal.StructureToPtr(value, ptr, true);						// копировать в массив
			handle.Free();													// Освобождить дескриптор

			return buf;

		}

        public int GetPidDataOffset()
        {
            int result = Marshal.SizeOf(m_regs);
            result -= (Marshal.SizeOf(m_regs.Heater1PidData) * 3);
            return result / 2;
        }

        public int GetPidDataSize()
        {
            return (Marshal.SizeOf(m_regs.Heater1PidData) * 3) / 2;
        }

        public SipModbusRegion[] GetReadRegions()
        {
            List<SipModbusRegion> lst = new List<SipModbusRegion>(16);

            lst.Add(new SipModbusRegion() { Name = "Params", StartAddr = 0, Count = 257, RegionType = SipModbusRegionType.Params });
            lst.Add(new SipModbusRegion() { Name = "Data", StartAddr = 258, Count = 1024, RegionType = SipModbusRegionType.Data });
            lst.Add(new SipModbusRegion() { Name = "Config + Config CRC", StartAddr = 8449, Count = 29, RegionType = SipModbusRegionType.Config });
            lst.Add(new SipModbusRegion() { Name = "Pids data", StartAddr = (ushort)GetPidDataOffset(), Count = (ushort)GetPidDataSize(), RegionType = SipModbusRegionType.PidData });

            //lst.Add(new SipModbusRegion() { Name = "Data", StartAddr = 0, Count = 190 });

            return lst.ToArray();
        }
        
        public int GetAnalogDataOffset()
        {
            return (Marshal.SizeOf(m_regs) / 2);
        }
        //
		#endregion // Public
		//-------------------------------------------------------------------------
		#region Protected
		//

		//
		#endregion // Public
		//-------------------------------------------------------------------------
		#region Private
		//

		//
		#endregion // Private
		//-------------------------------------------------------------------------
		#region Events
		//

		//   
		#endregion // Events
		//-------------------------------------------------------------------------
	}

    public enum SipModbusRegNums
    {
        FpgaOutNum                      = 0,
        CpuInNum                        = 61,
        CRCNum                          = 126,
        CpuOutNum                       = 127,
        FpgaInNum                       = 190,
        ConfigNum                       = 8449,
        ConfigCRCNum                    = 8478,
        Heater1PidDataNum               = 8479,
        FlowInPidDataNum                = 8491,
        FlowTotalPidDataNum             = 8503
    }

    public enum CpuOutDummy1Regs
    {
        HubState,
        HubLocalCounter,
        HubPwr1,
        HubPwr2,
        WaEnableValue,
        HubPowerPeriod,
        HubFlags,
        TempSetupTime,
        ModeOkSetTime,
        AppConfig,
        AppFlags0,
        AppFlags1,
        HostAppControl0,
        HostAppControl1
    }

    public enum SipModbusRegionType
    {
        Params,
        Data,
        Config,
        PidData,
        AnalogData
    }
    public struct SipModbusRegion
    {
        public string Name;
        public ushort StartAddr;
        public ushort Count;
        public SipModbusRegionType RegionType;
    }

	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
	public struct FpgaOut
	{
		public ushort PointScale;
		public ushort FpgaFlags;
		public ushort HvImpulseLength;
		public ushort HvPolarityLength;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
		public ushort[] PeakParamTable;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
		public ushort[] Dummy;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
	public struct CpuIn
	{
		public ushort FlowTotalSetpoint;
		public ushort FlowInSetpoint;
		public ushort PumpLevel;
		public ushort ValveLevel;
		public ushort T1Setpoint;
		public ushort T2Setpoint;
		public ushort HvSetpoint;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)]
		public ushort[] Dummy;
		public ushort FlowSetPeriod;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public ushort[] Dummy1;
		public ushort Substance;
		public ushort RtcSec;
		public ushort RtcMin;
		public ushort RtcHour;
		public ushort RtcDay;
		public ushort RtcMonth;
		public ushort RtcYear;
		public ushort RtcCtl;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public ushort[] Dummy2;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
	public struct CpuOut
	{
		public ushort Flags;
		public ushort Temp1;
		public ushort Temp2;
		public ushort HeaterPower;
		public ushort TempCase;
		public ushort AthmPressure;
		public ushort ValveLevel;
		public ushort PumpTotalFlow;
		public ushort HvValue;
		public ushort PumpTimeLow;
		public ushort PumpTimeHigh;
		public ushort Humidity;
		public ushort FlowTotal;
		public ushort FlowIn;
		public ushort DeviceMode;
		public ushort Substance;
		public ushort FirmwareVersion;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 46)]
		public ushort[] Dummy1;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
	public struct PeaksTable
	{
		public ushort Time;
		public ushort Value;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
	public struct FpgaIn
	{
		public ushort FlasgFpga2;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public PeaksTable[] PeaksFoundTable;
		public ushort FlagsFpga;
		public ushort PeakCount;
		public ushort Shelf;
		public ushort Integral;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
		public ushort[] Dummy1;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8192)]
		public ushort[] Points;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct PidFactors_t
    {
        public float kP;
        public float kI;
        public float kD;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct PidData_t
    {
        public float pValue;
        public float iValue;
        public float dValue;
        public float errSumm;
        public float outValue;
        public float sp;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct DeviceConfig
    {
        public ushort UsartBaudrate;
        public ushort UsartStopBits;
        public ushort MbAddress;
        public ushort WAYlimit;
        public ushort WAYlimitInReperY;
        public ushort WAMinDYforUp;
        public ushort WAOnTimeForUp;
        public ushort WAOffTimeForUp;
        public ushort WAMinDYforDown;
        public ushort WAOnTimeForDown;
        public ushort WAOffTimeForDown;
        //
        public PidFactors_t Heater1PidFactors;
        public PidFactors_t FlowInPidFactors;
        public PidFactors_t FlowTotalPidFactors;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct SipRegisters_t
    {
        public FpgaOut FPGA_OUT;
        public CpuIn CPU_IN;
        public ushort CRC1;
        public CpuOut CPU_OUT;
        public FpgaIn FPGA_IN;
        //
        public DeviceConfig Config;
        public ushort ConfigCRC;
        //
        public PidData_t Heater1PidData;
        public PidData_t FlowInPidData;
        public PidData_t FlowTotalPidData;
        //
    }
}
