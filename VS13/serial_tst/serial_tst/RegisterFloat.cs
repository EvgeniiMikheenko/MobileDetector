using common.utils;
using common.utils.CRC_WRK;
using PIDLibrary.Pravate;
using RemoteDevices.Gdu_01.RegistersData;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Runtime.InteropServices;

namespace RemoteDevices.Gdu_01
{
    public class RegisterFloat
    {
        public RegisterFloat()
        {

        }
        //-------------------------------------------------------------------------
        #region Data
        //---------------------------------------------------------------------
        #region Const

        public const int MaxReadRegs = 125;
        public const int MaxWriteRegs = 123;

        public const int DummyVbatIndex = 40;
        public const int DummyMeasureTimeIndex = 43; // #define DUMMY_ADC_MEASURE_TIME_INDEX		(DUMMY_ADC_DATA_START_INDEX + DUMMY_ADC_REGS_COUNT) см. IAR project.h

        public const UInt32 IsStartedMask = 0x00000001;
        public const UInt32 IsBoxTempEnMask = 0x00000002;
        public const UInt32 GenModeMask = 0x0000001C;
        public const UInt32 IsHumEnMask = 0x00000020;
        public const UInt32 HumModeMask = 0x000001C0;
        public const UInt32 IsSourceZeroGasMask = 0x00000200;
        public const UInt32 IsCellEnMask = 0x00000400;
        public const UInt32 CellModeMask = 0x00003800;
        public const UInt32 IsAddEnMask = 0x00004000;
        public const UInt32 AddModeMask = 0x00038000;
        public const UInt32 IsZeroFlowEnMask = 0x00040000;
        public const UInt32 IsCellOutDirMask = 0x00080000;
        public const UInt32 IsGenProgCompleteMask = 0x00100000;
        public const UInt32 IsHumProgCompleteMask = 0x00200000;
        public const UInt32 IsCellProgCompleteMask = 0x00400000;
        public const UInt32 IsAddProgCompleteMask = 0x00800000;
        public const UInt32 IsPressureLowMask = 0x01000000;
        public const UInt32 IsHumErrorMask = 0x02000000;
        public const UInt32 IsPressureErrorMask = 0x04000000;
        public const UInt32 IsUsbSelectedMask = 0x08000000;
        public const UInt32 IsUsetTempFactorsMask = 0x10000000;
        public const UInt32 IsHumidityFactorsMask = 0x20000000;
        public const UInt32 IsEnPressureErrorMask = 0x40000000;

        public const UInt32 NvDataValidMask = 0x80000000;

        #endregion
        //---------------------------------------------------------------------
        #region Pravate data
        //
        RegistersData.MbRegisters m_registers = new RegistersData.MbRegisters();

        bool m_savedDataChange;
        //
        #endregion //Data
        //---------------------------------------------------------------------
        #region Public Fields
        //
        public int RegistersCount
        {
            get
            {
                ushort[] buf = ValueUtil.StructToBuff<ushort, RegistersData.MbRegisters>(m_registers);
                return buf.Length;
            }
        }

        public bool IsSavedDataChange { get { return m_savedDataChange; } }

        public byte DeviceAddress
        {
            get { return (byte)m_registers.NvRegs.DeviceAddress; }
            set
            {
                if (m_registers.NvRegs.DeviceAddress != value)
                    m_savedDataChange = true;

                m_registers.NvRegs.DeviceAddress = value;
            }
        }

        public int Baudrate
        {
            get
            {
                Baudrates br = (Baudrates)m_registers.NvRegs.Baudrate;
                return BaudrateEnumToInt(br);
            }
            set
            {
                ushort old = m_registers.NvRegs.Baudrate;
                Baudrates br = BaudrateIntToEnum(value);

                m_registers.NvRegs.Baudrate = (ushort)br;

                if (m_registers.NvRegs.Baudrate != old)
                    m_savedDataChange = true;
            }
        }

        public StopBits StopBits
        {
            get
            {
                if (m_registers.NvRegs.StopBits == 2)
                    return System.IO.Ports.StopBits.One;

                return System.IO.Ports.StopBits.Two;
            }

            set
            {
                ushort old = m_registers.NvRegs.StopBits;

                if (value == System.IO.Ports.StopBits.Two)
                    m_registers.NvRegs.StopBits = 2;
                else
                    m_registers.NvRegs.StopBits = 1;

                if (m_registers.NvRegs.StopBits != old)
                    m_savedDataChange = true;
            }
        }

        public int ExtAdcMeasureTime
        {
            get
            {
                try
                {
                    return (int)m_registers.Dummy[DummyMeasureTimeIndex];
                }
                catch { return -1; }
            }
        }

        public bool IsNvDataValid
        {
            get
            {
                return (GetWorkFlags() & NvDataValidMask) != 0;
            }
        }
        
        public float Vbat
        {
            get
            {
				if (m_registers.Dummy == null)
					return 0;
				//
				if (DummyVbatIndex >= m_registers.Dummy.Length)
					return 0;
				//
                return ((float)m_registers.Dummy[DummyVbatIndex]) / 1000.0f;
            }
        }

        public CalibrationData CalibrationData { get; set; }
        //
        #endregion //Data
        //---------------------------------------------------------------------
        #endregion //Data
        //-------------------------------------------------------------------------
        #region Public
        //
        public void Parse(ushort[] src, int startIndex, int count)
        {
            ClearRegs();

            if ((src == null) || (src.Length <= 0))
                return;

            ushort[] buf = ValueUtil.StructToBuff<ushort, RegistersData.MbRegisters>(m_registers);

            int end = startIndex + count;
            for (int i = startIndex; i < end; i++)
            {
                if (i >= buf.Length)
                    break;
                if (i >= src.Length)
                    break;

                buf[i] = src[i];
            }

            m_registers = ValueUtil.BufToStruct<RegistersData.MbRegisters, ushort>(buf);
        }

        public void ParseCalibrationData(ushort[] buf)
        {
            ushort[] tBuf = ValueUtil.StructToBuff<ushort, CalibrationData>(CalibrationData);
            Array.Clear(tBuf, 0, tBuf.Length);
            //
            if (buf != null)
            {
                int len = buf.Length;
                if (len > tBuf.Length)
                    len = tBuf.Length;
                //
                Array.Copy(buf, 0, tBuf, 0, len);
            }
            //
            CalibrationData = ValueUtil.BufToStruct<CalibrationData, ushort>(tBuf);
        }

        public T[] GetCalibrationData<T>()
        {
            try { return ValueUtil.StructToBuff<T, CalibrationData>(CalibrationData); }
            catch   { return null; }
        }

        //public byte[] GetInitNvData()
        //{
        //    byte[] buf = ValueUtil.StructToBuff<byte, RegistersData.AppSavedData>(m_registers.NvRegs);
        //    RegistersData.AppSavedData asd = ValueUtil.BufToStruct<RegistersData.AppSavedData, byte>(buf);
        //    // Сбрасываем флаги работы
        //    for (int i = 0; i < ParamValues.WorkFlagsCount; i++)
        //    {
        //        asd.WorkType[i] = 0;
        //    }

        //    buf = ValueUtil.StructToBuff<byte, RegistersData.AppSavedData>(asd);
        //    byte[] tBuf = new byte[buf.Length - sizeof(ushort)];
        //    Array.Copy(buf, 0, tBuf, 0, tBuf.Length);
        //    asd.Crc = CRC.GetCRC16(tBuf);
        //    //
        //    buf = ValueUtil.StructToBuff<byte, RegistersData.AppSavedData>(asd);
        //    //
        //    return buf;
        //}

        public ushort[] GetSavedData(ref ushort start)
        {
            start = 0;
            m_registers.NvRegs.DeviceAddress = 16;
            m_registers.NvRegs.Baudrate = (ushort)BaudrateIntToEnum(115200);
            m_registers.NvRegs.StopBits = 1;
            ushort[] buf = ValueUtil.StructToBuff<ushort, RegistersData.AppSavedData>(m_registers.NvRegs);
            return buf;
        }

        public bool CompareSavedData(ushort[] src)
        {
            if ((src == null) || (src.Length <= 0))
                return false;

            try
            {
                RegistersData.AppSavedData regs = new RegistersData.AppSavedData();

                ushort[] buf = ValueUtil.StructToBuff<ushort, RegistersData.AppSavedData>(regs);

                for (int i = 0; i < src.Length; i++)
                {
                    if (i >= buf.Length)
                        break;

                    buf[i] = src[i];
                }

                regs = ValueUtil.BufToStruct<RegistersData.AppSavedData, ushort>(buf);
                regs.Crc = m_registers.NvRegs.Crc;
                regs.WorkType = m_registers.NvRegs.WorkType;

                buf = ValueUtil.StructToBuff<ushort, RegistersData.AppSavedData>(regs);
                ushort[] bufRegs = ValueUtil.StructToBuff<ushort, RegistersData.AppSavedData>(m_registers.NvRegs);

                bool state = true;
                for (int i = 0; i < bufRegs.Length; i++)
                {
                    if (bufRegs[i] == buf[i])
                        continue;

                    state = false;
                    break;
                }

                return state;
            }
            catch
            {
                return false;
            }
        }

        public void ClearChangeFlag()
        {
            m_savedDataChange = false;
        }

        public float GetFloatParamValue(ParamTypes pt)
        {
            int index = (int)pt;
            ushort[] buf = GetAllRegs();
            float result = 0;

            switch (pt)
            {
                case ParamTypes.DeviceAddress:
                    result = (float)DeviceAddress;
                    break;
                case ParamTypes.Baudrate:
                    result = (float)Baudrate;
                    break;
                case ParamTypes.Stopbits:
                    result = (float)StopBits;
                    break;
                case ParamTypes.SpBoxTemp:
                    result = m_registers.NvRegs.setPoints.BoxTemp;
                    break;
                case ParamTypes.SpCellTemp:
                    result = m_registers.NvRegs.setPoints.CellTemp;
                    break;
                case ParamTypes.SpOutHum:
                    result = m_registers.NvRegs.setPoints.OutHum;
                    break;
                case ParamTypes.SpGenFlow:
                    result = m_registers.NvRegs.setPoints.GenFlow;
                    break;
                case ParamTypes.SpTestFlow:
                    result = m_registers.NvRegs.setPoints.TestFlow;
                    break;
                case ParamTypes.SpCellFlow:
                    result = m_registers.NvRegs.setPoints.CellFlow;
                    break;
                case ParamTypes.SpZeroFlow:
                    result = m_registers.NvRegs.setPoints.ZeroFlow;
                    break;
                case ParamTypes.SpHumSpeed:
                    result = m_registers.NvRegs.setPoints.OutHumSpeed;
                    break;
                case ParamTypes.SpHumTime:
                    result = m_registers.NvRegs.setPoints.OutHumTime;
                    break;
                case ParamTypes.SpBoxTempSpeed:
                    result = m_registers.NvRegs.setPoints.BoxTempSpeed;
                    break;
                case ParamTypes.SpBoxTempTime:
                    result = m_registers.NvRegs.setPoints.BoxTempTime;
                    break;
                case ParamTypes.SpCellTempSpeed:
                    result = m_registers.NvRegs.setPoints.CellTempSpeed;
                    break;
                case ParamTypes.SpCellTempTime:
                    result = m_registers.NvRegs.setPoints.CellTempTime;
                    break;
                case ParamTypes.SpGenFlowSpeed:
                    result = m_registers.NvRegs.setPoints.GenFlowSpeed;
                    break;
                case ParamTypes.SpGenFlowTime:
                    result = m_registers.NvRegs.setPoints.GenFlowTime;
                    break;
                case ParamTypes.SpCellFlowSpeed:
                    result = m_registers.NvRegs.setPoints.CellFlowSpeed;
                    break;
                case ParamTypes.SpCellFlowTime:
                    result = m_registers.NvRegs.setPoints.CellFlowTime;
                    break;
                case ParamTypes.WorkType:
                    result = GetWorkFlags();
                    break;
                case ParamTypes.NvDummy:
                    result = m_registers.NvRegs.Dummy[0];
                    break;
                case ParamTypes.BoxTempPidKp:
                    result = m_registers.NvRegs.BoxTempPidFactors.kP;
                    break;
                case ParamTypes.BoxTempPidKi:
                    result = m_registers.NvRegs.BoxTempPidFactors.kI;
                    break;
                case ParamTypes.BoxTempPidKd:
                    result = m_registers.NvRegs.BoxTempPidFactors.kD;
                    break;
                case ParamTypes.CellTempPidKp:
                    result = m_registers.NvRegs.CellTempPidFActors.kP;
                    break;
                case ParamTypes.CellTempPidKi:
                    result = m_registers.NvRegs.CellTempPidFActors.kI;
                    break;
                case ParamTypes.CellTempPidKd:
                    result = m_registers.NvRegs.CellTempPidFActors.kD;
                    break;
                case ParamTypes.HumPidKp:
                    result = m_registers.NvRegs.HumPidFactors.kP;
                    break;
                case ParamTypes.HumPidKi:
                    result = m_registers.NvRegs.HumPidFactors.kI;
                    break;
                case ParamTypes.HumPidKd:
                    result = m_registers.NvRegs.HumPidFactors.kD;
                    break;
                case ParamTypes.GenFlowPidKp:
                    result = m_registers.NvRegs.GenFlowPidFactors.kP;
                    break;
                case ParamTypes.GenFlowPidKi:
                    result = m_registers.NvRegs.GenFlowPidFactors.kI;
                    break;
                case ParamTypes.GenFlowPidKd:
                    result = m_registers.NvRegs.GenFlowPidFactors.kD;
                    break;
                case ParamTypes.TestFlowPidKp:
                    result = m_registers.NvRegs.TestFlowPidFactors.kP;
                    break;
                case ParamTypes.TestFlowPidKi:
                    result = m_registers.NvRegs.TestFlowPidFactors.kI;
                    break;
                case ParamTypes.TestFlowPidKd:
                    result = m_registers.NvRegs.TestFlowPidFactors.kD;
                    break;
                case ParamTypes.CellFlowPidKp:
                    result = m_registers.NvRegs.CellFlowPidFactors.kP;
                    break;
                case ParamTypes.CellFlowPidKi:
                    result = m_registers.NvRegs.CellFlowPidFactors.kI;
                    break;
                case ParamTypes.CellFlowPidKd:
                    result = m_registers.NvRegs.CellFlowPidFactors.kD;
                    break;
                case ParamTypes.ZeroFlowPidKp:
                    result = m_registers.NvRegs.ZeroFlowPidFactors.kP;
                    break;
                case ParamTypes.ZeroFlowPidKi:
                    result = m_registers.NvRegs.ZeroFlowPidFactors.kI;
                    break;
                case ParamTypes.ZeroFlowPidKd:
                    result = m_registers.NvRegs.ZeroFlowPidFactors.kD;
                    break;
                //case ParamTypes.GenProgUnitNum:
                //    result = m_registers.NvRegs.genProgramm.PrgIndex;
                //    break;
                //case ParamTypes.GenProgramm:
                //    break;
                //case ParamTypes.HumProgUnitNum:
                //    break;
                //case ParamTypes.HumProgramm:
                //    break;
                //case ParamTypes.CellProgUnitNum:
                //    break;
                //case ParamTypes.CellProgramm:
                //    break;
                //case ParamTypes.AddProgUnitNum:
                //    break;
                //case ParamTypes.AddProgramm:
                //    break;
                case ParamTypes.CRC:
                    result = m_registers.NvRegs.Crc;
                    break;
                case ParamTypes.BoxTemp:
                    result = m_registers.CurValues.BoxTemp;
                    break;
                case ParamTypes.CellTemp:
                    result = m_registers.CurValues.CellTemp;
                    break;
                case ParamTypes.OutHum:
                    result = m_registers.CurValues.OutHum;
                    break;
                case ParamTypes.GenFlow:
                    result = m_registers.CurValues.GenFlow;
                    break;
                case ParamTypes.CellFlow:
                    result = m_registers.CurValues.CellFlow;
                    break;
                case ParamTypes.TestFlow:
                    result = m_registers.CurValues.TestFlow;
                    break;
                case ParamTypes.ZeroFlow:
                    result = m_registers.CurValues.ZeroFlow;
                    break;
                case ParamTypes.Pressure:
                    result = m_registers.CurValues.Pressure;
                    break;
                case ParamTypes.InputHum:
                    result = m_registers.CurValues.InputHum;
                    break;
                case ParamTypes.OutHumSpeed:
                    result = m_registers.CurValues.OutHumSpeed;
                    break;
                case ParamTypes.OutHumTime:
                    result = m_registers.CurValues.OutHumTime;
                    break;
                case ParamTypes.BoxTempSpeed:
                    result = m_registers.CurValues.BoxTempSpeed;
                    break;
                case ParamTypes.BoxTempTime:
                    result = m_registers.CurValues.BoxTempTime;
                    break;
                case ParamTypes.CellTempSpeed:
                    result = m_registers.CurValues.CellTempSpeed;
                    break;
                case ParamTypes.CellTempTime:
                    result = m_registers.CurValues.CellTempTime;
                    break;
                case ParamTypes.GenFlowSpeed:
                    result = m_registers.CurValues.GenFlowSpeed;
                    break;
                case ParamTypes.GenFlowTime:
                    result = m_registers.CurValues.GenFlowTime;
                    break;
                case ParamTypes.CellFlowSpeed:
                    result = m_registers.CurValues.CellFlowSpeed;
                    break;
                case ParamTypes.CellFlowTime:
                    result = m_registers.CurValues.CellFlowTime;
                    break;
                case ParamTypes.AppFlags:
                    result = GetAppFlags();
                    break;
                //case ParamTypes.Dummy:
                //    break;
                //case ParamTypes.DummyExtAdcCh1Volts:
                //    break;
                //case ParamTypes.DummyExtAdcCh1BeforeFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh1AfterFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh2Volts:
                //    break;
                //case ParamTypes.DummyExtAdcCh2BeforeFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh2AfterFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh3Volts:
                //    break;
                //case ParamTypes.DummyExtAdcCh3BeforeFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh3AfterFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh4Volts:
                //    break;
                //case ParamTypes.DummyExtAdcCh4BeforeFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh4AfterFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh5Volts:
                //    break;
                //case ParamTypes.DummyExtAdcCh5BeforeFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh5AfterFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh6Volts:
                //    break;
                //case ParamTypes.DummyExtAdcCh6BeforeFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh6AfterFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh7Volts:
                //    break;
                //case ParamTypes.DummyExtAdcCh7BeforeFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh7AfterFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh8Volts:
                //    break;
                //case ParamTypes.DummyExtAdcCh8BeforeFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh8AfterFilters:
                //    break;
                //case ParamTypes.DummyIntAdcCh1Volts:
                //    break;
                //case ParamTypes.DummyIntAdcCh1BeforeFilters:
                //    break;
                //case ParamTypes.DummyIntAdcCh1AfterFilters:
                //    break;
                //case ParamTypes.DummyIntAdcCh2Volts:
                //    break;
                //case ParamTypes.DummyIntAdcCh2BeforeFilters:
                //    break;
                //case ParamTypes.DummyIntAdcCh2AfterFilters:
                //    break;
                //case ParamTypes.DummyIntAdcCh3Volts:
                //    break;
                //case ParamTypes.DummyIntAdcCh3BeforeFilters:
                //    break;
                //case ParamTypes.DummyIntAdcCh3AfterFilters:
                //    break;
                //case ParamTypes.DummyIntAdcCh4Volts:
                //    break;
                //case ParamTypes.DummyIntAdcCh4BeforeFilters:
                //    break;
                //case ParamTypes.DummyIntAdcCh4AfterFilters:
                //    break;
                //case ParamTypes.DummyIntAdcCh5Volts:
                //    break;
                //case ParamTypes.DummyIntAdcCh5BeforeFilters:
                //    break;
                //case ParamTypes.DummyIntAdcCh5AfterFilters:
                //    break;
                //case ParamTypes.DummyIntAdcVbatVolts:
                //    break;
                //case ParamTypes.DummyIntAdcVbatBeforeFilters:
                //    break;
                //case ParamTypes.DummyIntAdcVbatAfterFilters:
                //    break;
                case ParamTypes.BoxTempPidDataSp:
                    result = m_registers.BoxTempPidData.Sp;
                    break;
                case ParamTypes.BoxTempPidDataPv:
                    result = m_registers.BoxTempPidData.Pv;
                    break;
                case ParamTypes.BoxTempPidDataPval:
                    result = m_registers.BoxTempPidData.Pval;
                    break;
                case ParamTypes.BoxTempPidDataIval:
                    result = m_registers.BoxTempPidData.Ival;
                    break;
                case ParamTypes.BoxTempPidDataDval:
                    result = m_registers.BoxTempPidData.Dval;
                    break;
                case ParamTypes.BoxTempPidDataResult:
                    result = m_registers.BoxTempPidData.Result;
                    break;
                case ParamTypes.CellTempPidDataSp:
                    result = m_registers.CellTempPidData.Sp;
                    break;
                case ParamTypes.CellTempPidDataPv:
                    result = m_registers.CellTempPidData.Pv;
                    break;
                case ParamTypes.CellTempPidDataPval:
                    result = m_registers.CellTempPidData.Pval;
                    break;
                case ParamTypes.CellTempPidDataIval:
                    result = m_registers.CellTempPidData.Ival;
                    break;
                case ParamTypes.CellTempPidDataDval:
                    result = m_registers.CellTempPidData.Dval;
                    break;
                case ParamTypes.CellTempPidDataResult:
                    result = m_registers.CellTempPidData.Result;
                    break;
                case ParamTypes.HumPidDataSp:
                    result = m_registers.HumPidData.Sp;
                    break;
                case ParamTypes.HumPidDataPv:
                    result = m_registers.HumPidData.Pv;
                    break;
                case ParamTypes.HumPidDataPval:
                    result = m_registers.HumPidData.Pval;
                    break;
                case ParamTypes.HumPidDataIval:
                    result = m_registers.HumPidData.Ival;
                    break;
                case ParamTypes.HumPidDataDval:
                    result = m_registers.HumPidData.Dval;
                    break;
                case ParamTypes.HumPidDataResult:
                    result = m_registers.HumPidData.Result;
                    break;
                case ParamTypes.GenFlowPidDataSp:
                    result = m_registers.GenFlowPidData.Sp;
                    break;
                case ParamTypes.GenFlowPidDataPv:
                    result = m_registers.GenFlowPidData.Pv;
                    break;
                case ParamTypes.GenFlowPidDataPval:
                    result = m_registers.GenFlowPidData.Pval;
                    break;
                case ParamTypes.GenFlowPidDataIval:
                    result = m_registers.GenFlowPidData.Ival;
                    break;
                case ParamTypes.GenFlowPidDataDval:
                    result = m_registers.GenFlowPidData.Dval;
                    break;
                case ParamTypes.GenFlowPidDataResult:
                    result = m_registers.GenFlowPidData.Result;
                    break;
                case ParamTypes.TestFlowPidDataSp:
                    result = m_registers.TestFlowPidData.Pv;
                    break;
                case ParamTypes.TestFlowPidDataPv:
                    result = m_registers.TestFlowPidData.Pval;
                    break;
                case ParamTypes.TestFlowPidDataPval:
                    result = m_registers.TestFlowPidData.Ival;
                    break;
                case ParamTypes.TestFlowPidDataIval:
                    result = m_registers.TestFlowPidData.Dval;
                    break;
                case ParamTypes.TestFlowPidDataDval:
                    result = m_registers.TestFlowPidData.Dval;
                    break;
                case ParamTypes.TestFlowPidDataResult:
                    result = m_registers.TestFlowPidData.Result;
                    break;
                case ParamTypes.CellFlowPidDataSp:
                    result = m_registers.CellTempPidData.Sp;
                    break;
                case ParamTypes.CellFlowPidDataPv:
                    result = m_registers.CellTempPidData.Pv;
                    break;
                case ParamTypes.CellFlowPidDataPval:
                    result = m_registers.CellTempPidData.Pval;
                    break;
                case ParamTypes.CellFlowPidDataIval:
                    result = m_registers.CellTempPidData.Ival;
                    break;
                case ParamTypes.CellFlowPidDataDval:
                    result = m_registers.CellTempPidData.Dval;
                    break;
                case ParamTypes.CellFlowPidDataResult:
                    result = m_registers.CellTempPidData.Result;
                    break;
                case ParamTypes.ZeroFlowPidDataSp:
                    result = m_registers.ZeroFlowPidData.Sp;
                    break;
                case ParamTypes.ZeroFlowPidDataPv:
                    result = m_registers.ZeroFlowPidData.Pv;
                    break;
                case ParamTypes.ZeroFlowPidDataPval:
                    result = m_registers.ZeroFlowPidData.Pval;
                    break;
                case ParamTypes.ZeroFlowPidDataIval:
                    result = m_registers.ZeroFlowPidData.Ival;
                    break;
                case ParamTypes.ZeroFlowPidDataDval:
                    result = m_registers.ZeroFlowPidData.Dval;
                    break;
                case ParamTypes.ZeroFlowPidDataResult:
                    result = m_registers.ZeroFlowPidData.Result;
                    break;
                default:
                    break;
            }

            return result;
        }

        public void SetParamValue(ParamTypes pt, object value)
        {
            switch (pt)
            {
                case ParamTypes.DeviceAddress:
                    byte addr = Convert.ToByte(value);
                    DeviceAddress = addr;
					break;
                case ParamTypes.Baudrate:
                    int br = Convert.ToInt32(value);
                    Baudrate = br;
                    break;
                case ParamTypes.Stopbits:
                    StopBits = (System.IO.Ports.StopBits)Convert.ToInt32(value);
                    break;
                case ParamTypes.SpBoxTemp:
                    SetFloat(ref m_registers.NvRegs.setPoints.BoxTemp, value, true);
                    break;
                case ParamTypes.SpCellTemp:
                    SetFloat(ref m_registers.NvRegs.setPoints.CellTemp, value, true);
                    break;
                case ParamTypes.SpOutHum:
                    SetFloat(ref m_registers.NvRegs.setPoints.OutHum, value, true);
                    break;
                case ParamTypes.SpGenFlow:
                    SetFloat(ref m_registers.NvRegs.setPoints.GenFlow, value, true);
                    break;
                case ParamTypes.SpTestFlow:
                    SetFloat(ref m_registers.NvRegs.setPoints.TestFlow, value, true);
                    break;
                case ParamTypes.SpCellFlow:
                    SetFloat(ref m_registers.NvRegs.setPoints.CellFlow, value, true);
                    break;
                case ParamTypes.SpZeroFlow:
                    SetFloat(ref m_registers.NvRegs.setPoints.ZeroFlow, value, true);
                    break;
                case ParamTypes.SpHumSpeed:
                    SetFloat(ref m_registers.NvRegs.setPoints.OutHumSpeed, value, true);
                    break;
                case ParamTypes.SpHumTime:
                    SetUshort(ref m_registers.NvRegs.setPoints.OutHumTime, value, true);
                    break;
                case ParamTypes.SpBoxTempSpeed:
                    SetFloat(ref m_registers.NvRegs.setPoints.BoxTempSpeed, value, true);
                    break;
                case ParamTypes.SpBoxTempTime:
                    SetUshort(ref m_registers.NvRegs.setPoints.BoxTempTime, value, true);
                    break;
                case ParamTypes.SpCellTempSpeed:
                    SetFloat(ref m_registers.NvRegs.setPoints.CellTempSpeed, value, true);
                    break;
                case ParamTypes.SpCellTempTime:
                    SetUshort(ref m_registers.NvRegs.setPoints.CellTempTime, value, true);
                    break;
                case ParamTypes.SpGenFlowSpeed:
                    SetFloat(ref m_registers.NvRegs.setPoints.GenFlowSpeed, value, true);
                    break;
                case ParamTypes.SpGenFlowTime:
                    SetUshort(ref m_registers.NvRegs.setPoints.GenFlowTime, value, true);
                    break;
                case ParamTypes.SpCellFlowSpeed:
                    SetFloat(ref m_registers.NvRegs.setPoints.CellFlowSpeed, value, true);
                    break;
                case ParamTypes.SpCellFlowTime:
                    SetUshort(ref m_registers.NvRegs.setPoints.CellFlowTime, value, true);
                    break;
                case ParamTypes.WorkType:
                    UInt32 flags = Convert.ToUInt32(value);
                    UInt32 flagsOld = GetWorkFlags();
                    SetWorkFlags(flags);
                    if (flags != flagsOld)
                        m_savedDataChange = true;
                    break;
                //case ParamTypes.NvDummy:
                //    m_registers.NvRegs.Dummy[0];
                //    break;
                case ParamTypes.BoxTempPidKp:
                    SetFloat(ref m_registers.NvRegs.BoxTempPidFactors.kP, value, true);
                    break;
                case ParamTypes.BoxTempPidKi:
                    SetFloat(ref m_registers.NvRegs.BoxTempPidFactors.kI, value, true);
                    break;
                case ParamTypes.BoxTempPidKd:
                    SetFloat(ref m_registers.NvRegs.BoxTempPidFactors.kD, value, true);
                    break;
                case ParamTypes.CellTempPidKp:
                    SetFloat(ref m_registers.NvRegs.CellTempPidFActors.kP, value, true);
                    break;
                case ParamTypes.CellTempPidKi:
                    SetFloat(ref m_registers.NvRegs.CellTempPidFActors.kI, value, true);
                    break;
                case ParamTypes.CellTempPidKd:
                    SetFloat(ref m_registers.NvRegs.CellTempPidFActors.kD, value, true);
                    break;
                case ParamTypes.HumPidKp:
                    SetFloat(ref m_registers.NvRegs.HumPidFactors.kP, value, true);
                    break;
                case ParamTypes.HumPidKi:
                    SetFloat(ref m_registers.NvRegs.HumPidFactors.kI, value, true);
                    break;
                case ParamTypes.HumPidKd:
                    SetFloat(ref m_registers.NvRegs.HumPidFactors.kD, value, true);
                    break;
                case ParamTypes.GenFlowPidKp:
                    SetFloat(ref m_registers.NvRegs.GenFlowPidFactors.kP, value, true);
                    break;
                case ParamTypes.GenFlowPidKi:
                    SetFloat(ref m_registers.NvRegs.GenFlowPidFactors.kI, value, true);
                    break;
                case ParamTypes.GenFlowPidKd:
                    SetFloat(ref m_registers.NvRegs.GenFlowPidFactors.kD, value, true);
                    break;
                case ParamTypes.TestFlowPidKp:
                    SetFloat(ref m_registers.NvRegs.TestFlowPidFactors.kP, value, true);
                    break;
                case ParamTypes.TestFlowPidKi:
                    SetFloat(ref m_registers.NvRegs.TestFlowPidFactors.kI, value, true);
                    break;
                case ParamTypes.TestFlowPidKd:
                    SetFloat(ref m_registers.NvRegs.TestFlowPidFactors.kD, value, true);
                    break;
                case ParamTypes.CellFlowPidKp:
                    SetFloat(ref m_registers.NvRegs.CellFlowPidFactors.kP, value, true);
                    break;
                case ParamTypes.CellFlowPidKi:
                    SetFloat(ref m_registers.NvRegs.CellFlowPidFactors.kI, value, true);
                    break;
                case ParamTypes.CellFlowPidKd:
                    SetFloat(ref m_registers.NvRegs.CellFlowPidFactors.kD, value, true);
                    break;
                case ParamTypes.ZeroFlowPidKp:
                    SetFloat(ref m_registers.NvRegs.ZeroFlowPidFactors.kP, value, true);
                    break;
                case ParamTypes.ZeroFlowPidKi:
                    SetFloat(ref m_registers.NvRegs.ZeroFlowPidFactors.kI, value, true);
                    break;
                case ParamTypes.ZeroFlowPidKd:
                   SetFloat(ref m_registers.NvRegs.ZeroFlowPidFactors.kD, value, true);
                    break;
                //case ParamTypes.GenProgUnitNum:
                //    m_registers.NvRegs.genProgramm.PrgIndex;
                //    break;
                //case ParamTypes.GenProgramm:
                //    break;
                //case ParamTypes.HumProgUnitNum:
                //    break;
                //case ParamTypes.HumProgramm:
                //    break;
                //case ParamTypes.CellProgUnitNum:
                //    break;
                //case ParamTypes.CellProgramm:
                //    break;
                //case ParamTypes.AddProgUnitNum:
                //    break;
                //case ParamTypes.AddProgramm:
                //    break;
                case ParamTypes.CRC:
                    UInt16 crc = Convert.ToUInt16(value);
                    UInt16 crcOld = m_registers.NvRegs.Crc;
                    m_registers.NvRegs.Crc = crc;
                    break;
                case ParamTypes.BoxTemp:
                    SetFloat(ref m_registers.CurValues.BoxTemp, value);
                    break;
                case ParamTypes.CellTemp:
                    SetFloat(ref m_registers.CurValues.CellTemp, value);
                    break;
                case ParamTypes.OutHum:
                    SetFloat(ref m_registers.CurValues.OutHum, value);
                    break;
                case ParamTypes.GenFlow:
                    SetFloat(ref m_registers.CurValues.GenFlow, value);
                    break;
                case ParamTypes.CellFlow:
                    SetFloat(ref m_registers.CurValues.CellFlow, value);
                    break;
                case ParamTypes.TestFlow:
                    SetFloat(ref m_registers.CurValues.TestFlow, value);
                    break;
                case ParamTypes.ZeroFlow:
                    SetFloat(ref m_registers.CurValues.ZeroFlow, value);
                    break;
                case ParamTypes.Pressure:
                    SetFloat(ref m_registers.CurValues.Pressure, value);
                    break;
                case ParamTypes.InputHum:
                    SetFloat(ref m_registers.CurValues.InputHum, value);
                    break;
                case ParamTypes.OutHumSpeed:
                    SetFloat(ref m_registers.CurValues.OutHumSpeed, value);
                    break;
                case ParamTypes.OutHumTime:
                    SetUshort(ref m_registers.CurValues.OutHumTime, value);
                    break;
                case ParamTypes.BoxTempSpeed:
                    SetFloat(ref m_registers.CurValues.BoxTempSpeed, value);
                    break;
                case ParamTypes.BoxTempTime:
                    SetUshort(ref m_registers.CurValues.BoxTempTime, value);
                    break;
                case ParamTypes.CellTempSpeed:
                    SetFloat(ref m_registers.CurValues.CellTempSpeed, value);
                    break;
                case ParamTypes.CellTempTime:
                    SetUshort(ref m_registers.CurValues.CellTempTime, value);
                    break;
                case ParamTypes.GenFlowSpeed:
                    SetFloat(ref m_registers.CurValues.GenFlowSpeed, value);
                    break;
                case ParamTypes.GenFlowTime:
                    SetUshort(ref m_registers.CurValues.GenFlowTime, value);
                    break;
                case ParamTypes.CellFlowSpeed:
                    SetFloat(ref m_registers.CurValues.CellFlowSpeed, value);
                    break;
                case ParamTypes.CellFlowTime:
                    SetUshort(ref m_registers.CurValues.CellFlowTime, value);
                    break;
                //case ParamTypes.AppFlags:
                //    break;
                //case ParamTypes.Dummy:
                //    break;
                //case ParamTypes.DummyExtAdcCh1Volts:
                //    break;
                //case ParamTypes.DummyExtAdcCh1BeforeFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh1AfterFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh2Volts:
                //    break;
                //case ParamTypes.DummyExtAdcCh2BeforeFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh2AfterFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh3Volts:
                //    break;
                //case ParamTypes.DummyExtAdcCh3BeforeFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh3AfterFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh4Volts:
                //    break;
                //case ParamTypes.DummyExtAdcCh4BeforeFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh4AfterFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh5Volts:
                //    break;
                //case ParamTypes.DummyExtAdcCh5BeforeFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh5AfterFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh6Volts:
                //    break;
                //case ParamTypes.DummyExtAdcCh6BeforeFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh6AfterFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh7Volts:
                //    break;
                //case ParamTypes.DummyExtAdcCh7BeforeFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh7AfterFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh8Volts:
                //    break;
                //case ParamTypes.DummyExtAdcCh8BeforeFilters:
                //    break;
                //case ParamTypes.DummyExtAdcCh8AfterFilters:
                //    break;
                //case ParamTypes.DummyIntAdcCh1Volts:
                //    break;
                //case ParamTypes.DummyIntAdcCh1BeforeFilters:
                //    break;
                //case ParamTypes.DummyIntAdcCh1AfterFilters:
                //    break;
                //case ParamTypes.DummyIntAdcCh2Volts:
                //    break;
                //case ParamTypes.DummyIntAdcCh2BeforeFilters:
                //    break;
                //case ParamTypes.DummyIntAdcCh2AfterFilters:
                //    break;
                //case ParamTypes.DummyIntAdcCh3Volts:
                //    break;
                //case ParamTypes.DummyIntAdcCh3BeforeFilters:
                //    break;
                //case ParamTypes.DummyIntAdcCh3AfterFilters:
                //    break;
                //case ParamTypes.DummyIntAdcCh4Volts:
                //    break;
                //case ParamTypes.DummyIntAdcCh4BeforeFilters:
                //    break;
                //case ParamTypes.DummyIntAdcCh4AfterFilters:
                //    break;
                //case ParamTypes.DummyIntAdcCh5Volts:
                //    break;
                //case ParamTypes.DummyIntAdcCh5BeforeFilters:
                //    break;
                //case ParamTypes.DummyIntAdcCh5AfterFilters:
                //    break;
                //case ParamTypes.DummyIntAdcVbatVolts:
                //    break;
                //case ParamTypes.DummyIntAdcVbatBeforeFilters:
                //    break;
                //case ParamTypes.DummyIntAdcVbatAfterFilters:
                //    break;
                case ParamTypes.BoxTempPidDataSp:
                    SetFloat(ref m_registers.BoxTempPidData.Sp, value);
                    break;
                case ParamTypes.BoxTempPidDataPv:
                    SetFloat(ref m_registers.BoxTempPidData.Pv, value);
                    break;
                case ParamTypes.BoxTempPidDataPval:
                    SetFloat(ref m_registers.BoxTempPidData.Pval, value);
                    break;
                case ParamTypes.BoxTempPidDataIval:
                    SetFloat(ref m_registers.BoxTempPidData.Ival, value);
                    break;
                case ParamTypes.BoxTempPidDataDval:
                    SetFloat(ref m_registers.BoxTempPidData.Dval, value);
                    break;
                case ParamTypes.BoxTempPidDataResult:
                    SetFloat(ref m_registers.BoxTempPidData.Result, value);
                    break;
                case ParamTypes.CellTempPidDataSp:
                    SetFloat(ref m_registers.CellTempPidData.Sp, value);
                    break;
                case ParamTypes.CellTempPidDataPv:
                    SetFloat(ref m_registers.CellTempPidData.Pv, value);
                    break;
                case ParamTypes.CellTempPidDataPval:
                    SetFloat(ref m_registers.CellTempPidData.Pval, value);
                    break;
                case ParamTypes.CellTempPidDataIval:
                    SetFloat(ref m_registers.CellTempPidData.Ival, value);
                    break;
                case ParamTypes.CellTempPidDataDval:
                    SetFloat(ref m_registers.CellTempPidData.Dval, value);
                    break;
                case ParamTypes.CellTempPidDataResult:
                    SetFloat(ref m_registers.CellTempPidData.Result, value);
                    break;
                case ParamTypes.HumPidDataSp:
                    SetFloat(ref m_registers.HumPidData.Sp, value);
                    break;
                case ParamTypes.HumPidDataPv:
                    SetFloat(ref m_registers.HumPidData.Pv, value);
                    break;
                case ParamTypes.HumPidDataPval:
                    SetFloat(ref m_registers.HumPidData.Pval, value);
                    break;
                case ParamTypes.HumPidDataIval:
                    SetFloat(ref m_registers.HumPidData.Ival, value);
                    break;
                case ParamTypes.HumPidDataDval:
                    SetFloat(ref m_registers.HumPidData.Dval, value);
                    break;
                case ParamTypes.HumPidDataResult:
                    SetFloat(ref m_registers.HumPidData.Result, value);
                    break;
                case ParamTypes.GenFlowPidDataSp:
                    SetFloat(ref m_registers.GenFlowPidData.Sp, value);
                    break;
                case ParamTypes.GenFlowPidDataPv:
                    SetFloat(ref m_registers.GenFlowPidData.Pv, value);
                    break;
                case ParamTypes.GenFlowPidDataPval:
                    SetFloat(ref m_registers.GenFlowPidData.Pval, value);
                    break;
                case ParamTypes.GenFlowPidDataIval:
                    SetFloat(ref m_registers.GenFlowPidData.Ival, value);
                    break;
                case ParamTypes.GenFlowPidDataDval:
                    SetFloat(ref m_registers.GenFlowPidData.Dval, value);
                    break;
                case ParamTypes.GenFlowPidDataResult:
                    SetFloat(ref m_registers.GenFlowPidData.Result, value);
                    break;
                case ParamTypes.TestFlowPidDataSp:
                    SetFloat(ref m_registers.TestFlowPidData.Pv, value);
                    break;
                case ParamTypes.TestFlowPidDataPv:
                    SetFloat(ref m_registers.TestFlowPidData.Pval, value);
                    break;
                case ParamTypes.TestFlowPidDataPval:
                    SetFloat(ref m_registers.TestFlowPidData.Ival, value);
                    break;
                case ParamTypes.TestFlowPidDataIval:
                    SetFloat(ref m_registers.TestFlowPidData.Dval, value);
                    break;
                case ParamTypes.TestFlowPidDataDval:
                    SetFloat(ref m_registers.TestFlowPidData.Dval, value);
                    break;
                case ParamTypes.TestFlowPidDataResult:
                    SetFloat(ref m_registers.TestFlowPidData.Result, value);
                    break;
                case ParamTypes.CellFlowPidDataSp:
                    SetFloat(ref m_registers.CellTempPidData.Sp, value);
                    break;
                case ParamTypes.CellFlowPidDataPv:
                    SetFloat(ref m_registers.CellTempPidData.Pv, value);
                    break;
                case ParamTypes.CellFlowPidDataPval:
                    SetFloat(ref m_registers.CellTempPidData.Pval, value);
                    break;
                case ParamTypes.CellFlowPidDataIval:
                    SetFloat(ref m_registers.CellTempPidData.Ival, value);
                    break;
                case ParamTypes.CellFlowPidDataDval:
                    SetFloat(ref m_registers.CellTempPidData.Dval, value);
                    break;
                case ParamTypes.CellFlowPidDataResult:
                    SetFloat(ref m_registers.CellTempPidData.Result, value);
                    break;
                case ParamTypes.ZeroFlowPidDataSp:
                    SetFloat(ref m_registers.ZeroFlowPidData.Sp, value);
                    break;
                case ParamTypes.ZeroFlowPidDataPv:
                    SetFloat(ref m_registers.ZeroFlowPidData.Pv, value);
                    break;
                case ParamTypes.ZeroFlowPidDataPval:
                    SetFloat(ref m_registers.ZeroFlowPidData.Pval, value);
                    break;
                case ParamTypes.ZeroFlowPidDataIval:
                    SetFloat(ref m_registers.ZeroFlowPidData.Ival, value);
                    break;
                case ParamTypes.ZeroFlowPidDataDval:
                    SetFloat(ref m_registers.ZeroFlowPidData.Dval, value);
                    break;
                case ParamTypes.ZeroFlowPidDataResult:
                    SetFloat(ref m_registers.ZeroFlowPidData.Result, value);
                    break;
                default:
                    break;
            }
        }

        public ushort[] GetAllRegs()
        {
            return ValueUtil.StructToBuff<ushort, RegistersData.MbRegisters>(m_registers);
        }

        public UInt32 GetWorkFlags()
        {
			if (m_registers.NvRegs.WorkType == null)
				return 0;
			//
            return (UInt32)((m_registers.NvRegs.WorkType[1] << 16) + m_registers.NvRegs.WorkType[0]);
        }

        public void SetWorkFlags(UInt32 value)
        {
            UInt32 last = GetWorkFlags();
            //
            m_registers.NvRegs.WorkType[0] = (ushort)value;
            m_registers.NvRegs.WorkType[1] = (ushort)(value >> 16);
            //
            //if (last != value)
            m_savedDataChange = true;
        }

        public UInt32 GetAppFlags()
        {
			if (m_registers.AppFlags == null)
				return 0;
			//
            return (UInt32)((m_registers.AppFlags[1] << 16) | (m_registers.AppFlags[0]));
        }

        public ushort[] SetPidFactors(PIDFactors pf, PidTypes pt)
        {
            ushort[] buf = ValueUtil.StructToBuff<ushort, AppSavedData>(m_registers.NvRegs);
            AppSavedData sd = ValueUtil.BufToStruct<AppSavedData, ushort>(buf);

            bool isChange = false;

            switch (pt)
            {
                case PidTypes.BoxTempPid:
                    isChange = SetPidFactors(pf, ref sd.BoxTempPidFactors, ParamTypes.BoxTempPidKp, ParamTypes.BoxTempPidKi, ParamTypes.BoxTempPidKd);
                    break;
                case PidTypes.HumPid:
                    isChange = SetPidFactors(pf, ref sd.HumPidFactors, ParamTypes.HumPidKp, ParamTypes.HumPidKi, ParamTypes.HumPidKd);
                    break;
                case PidTypes.CelTemplPid:
                    isChange = SetPidFactors(pf, ref sd.CellTempPidFActors, ParamTypes.CellTempPidKp, ParamTypes.CellTempPidKi, ParamTypes.CellTempPidKd);
                    break;
                case PidTypes.CellFlowPid:
                    isChange = SetPidFactors(pf, ref sd.CellFlowPidFactors, ParamTypes.CellFlowPidKp, ParamTypes.CellFlowPidKi, ParamTypes.CellFlowPidKd);
                    break;
                case PidTypes.GenFlowPid:
                    isChange = SetPidFactors(pf, ref sd.GenFlowPidFactors, ParamTypes.GenFlowPidKp, ParamTypes.GenFlowPidKi, ParamTypes.GenFlowPidKd);
                    break;
                case PidTypes.TestFlowPid:
                    isChange = SetPidFactors(pf, ref sd.TestFlowPidFactors, ParamTypes.TestFlowPidKp, ParamTypes.TestFlowPidKi, ParamTypes.TestFlowPidKd);
                    break;
                case PidTypes.ZeroFlowPid:
                    isChange = SetPidFactors(pf, ref sd.ZeroFlowPidFactors, ParamTypes.ZeroFlowPidKp, ParamTypes.ZeroFlowPidKi, ParamTypes.ZeroFlowPidKd);
                    break;
                default:
                    break;
            }

            if (!isChange)
                return null;

            buf = ValueUtil.StructToBuff<ushort, AppSavedData>(sd);
            m_registers.NvRegs = ValueUtil.BufToStruct<AppSavedData, ushort>(buf);
            m_savedDataChange = true;
            return buf;
        }

        public ModuleStatus GetPidStatus(PidTypes pt)
        {
            UInt32 flags = GetWorkFlags();
            ModuleStatus result = ModuleStatus.Off;
            switch (pt)
            {
                case PidTypes.BoxTempPid:
                    result = ((flags & IsBoxTempEnMask) == 0) ? ModuleStatus.Off : ModuleStatus.On;
                    break;
                case PidTypes.HumPid:

                    if ((flags & IsHumErrorMask) != 0)
                        result = ModuleStatus.Error;
                    else
                        result = ((flags & IsHumEnMask) == 0) ? ModuleStatus.Off : ModuleStatus.On;

                    break;
                case PidTypes.CelTemplPid:
                    result = ((flags & IsCellEnMask) == 0) ? ModuleStatus.Off : ModuleStatus.On;
                    break;
                case PidTypes.CellFlowPid:
                    result = ((flags & IsCellEnMask) == 0) ? ModuleStatus.Off : ModuleStatus.On;
                    break;
                case PidTypes.GenFlowPid:
                    result = ((flags & IsStartedMask) == 0) ? ModuleStatus.Off : ModuleStatus.On;
                    break;
                case PidTypes.TestFlowPid:
                    result = ((flags & IsAddEnMask) == 0) ? ModuleStatus.Off : ModuleStatus.On;
                    break;
                case PidTypes.ZeroFlowPid:
                    result = ((flags & IsZeroFlowEnMask) == 0) ? ModuleStatus.Off : ModuleStatus.On;
                    break;
                default:
                    break;
            }

            return result;
        }

        public object GetParamValue(ParamTypes pt, ref Type type)
        {
            object result;
            switch (pt)
            {
                case ParamTypes.DeviceAddress:
                    result = DeviceAddress;
                    type = typeof(byte);
                    break;
                case ParamTypes.Baudrate:
                    result = Baudrate;
                    type = typeof(int);
                    break;
                case ParamTypes.Stopbits:
                    result = StopBits;
                    type = typeof(System.IO.Ports.StopBits);
                    break;
                case ParamTypes.WorkType:
                    result = GetWorkFlags();
                    type = typeof(ushort);
                    break;
                case ParamTypes.NvDummy:
                    result = m_registers.NvRegs.Dummy[0];
                    type = typeof(ushort);
                    break;
                case ParamTypes.CRC:
                    result = m_registers.NvRegs.Crc;
                    type = typeof(ushort);
                    break;
                case ParamTypes.AppFlags:
                    result = GetAppFlags();
                    type = typeof(UInt32);
                    break;
                case ParamTypes.Dummy:
                    result = m_registers.Dummy[0];
                    type = typeof(ushort);
                    break;
                default:
                    type = typeof(float);
                    result = GetFloatParamValue(pt);
                    break;
            }
            return result;
        }

        public ushort[] SetSpData(List<ParamDesc> src)
        {
            for (int i = 0; i < src.Count; i++)
            {
                ParamTypes pt = EnumDescriptor.ToEnum<ParamTypes>(src[i].Param);
                switch (pt)
                {
                    case ParamTypes.SpBoxTemp:
                        SetFloat(ref m_registers.NvRegs.setPoints.BoxTemp, src[i].Value, true);
                        break;
                    case ParamTypes.SpCellTemp:
                        SetFloat(ref m_registers.NvRegs.setPoints.CellTemp, src[i].Value, true);
                        break;
                    case ParamTypes.SpOutHum:
                        SetFloat(ref m_registers.NvRegs.setPoints.OutHum, src[i].Value, true);
                        break;
                    case ParamTypes.SpGenFlow:
                        SetFloat(ref m_registers.NvRegs.setPoints.GenFlow, src[i].Value, true);
                        break;
                    case ParamTypes.SpTestFlow:
                        SetFloat(ref m_registers.NvRegs.setPoints.TestFlow, src[i].Value, true);
                        break;
                    case ParamTypes.SpCellFlow:
                        SetFloat(ref m_registers.NvRegs.setPoints.CellFlow, src[i].Value, true);
                        break;
                    case ParamTypes.SpZeroFlow:
                        SetFloat(ref m_registers.NvRegs.setPoints.ZeroFlow, src[i].Value, true);
                        break;
                    default:
                        break;
                }
            }

            if (!m_savedDataChange)
                return null;

            return ValueUtil.StructToBuff<ushort, AppSavedData>(m_registers.NvRegs); ;
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
        void ClearRegs()
        {
            ushort[] buf = ValueUtil.StructToBuff<ushort, RegistersData.MbRegisters>(m_registers);
            for (int i = 0; i < buf.Length; i++)
            {
                buf[i] = 0;
            }

            m_registers = ValueUtil.BufToStruct<RegistersData.MbRegisters, ushort>(buf);
        }

        //int BaudrateEnumToInt(Baudrates bd)
        //{
        //    int result = 115200;
        //    switch (bd)
        //    {
        //        case Baudrates.Bps9600:
        //            result = 9600;
        //            break;
        //        case Baudrates.Bps14400:
        //            result = 14400;
        //            break;
        //        case Baudrates.Bps19200:
        //            result = 19200;
        //            break;
        //        case Baudrates.Bps38400:
        //            result = 38400;
        //            break;
        //        case Baudrates.Bps57600:
        //            result = 57600;
        //            break;
        //        case Baudrates.Bps115200:
        //            result = 115200;
        //            break;
        //        default:
        //            result = 115200;
        //            break;
        //    }

        //    return result;
        //}

        //Baudrates BaudrateIntToEnum(int br)
        //{
        //    if (br == 9600)
        //        return Baudrates.Bps9600;

        //    if (br == 14400)
        //        return Baudrates.Bps14400;

        //    if (br == 19200)
        //        return Baudrates.Bps19200;

        //    if (br == 38400)
        //        return Baudrates.Bps38400;

        //    if (br == 57600)
        //        return Baudrates.Bps57600;

        //    return Baudrates.Bps115200;
        //}

        void SetFloat(ref float var, object value, bool isNvData = false)
        {
            float fp = Convert.ToSingle(value);
            float fpOld = var;
            var = fp;
            if (!isNvData)
                return;
            //
            if (fp != fpOld)
                m_savedDataChange = true;
        }

        void SetUshort(ref ushort var, object value, bool isNvData = false)
        {
            ushort fp = Convert.ToUInt16(value);
            ushort fpOld = var;
            var = fp;
            if (!isNvData)
                return;
            //
            if (fp != fpOld)
                m_savedDataChange = true;
        }

        bool SetPidFactors(PIDLibrary.Pravate.PIDFactors pf, ref PidFactors pfDst, ParamTypes ptKp, ParamTypes ptKi, ParamTypes ptKd)
        {
            if (pf == null)
                return false;

            bool isChange = false;
            if (pfDst.kP != pf.Kp)
            {
                pfDst.kP = (float)pf.Kp;
                isChange = true;
            }

            if (pfDst.kI != pf.Kp)
            {
                pfDst.kI = (float)pf.Kp;
                isChange = true;
            }

            if (pfDst.kD != pf.Kd)
            {
                pfDst.kD = (float)pf.Kd;
                isChange = true;
            }

            return isChange;
        }

        //
        #endregion // Private
        //-------------------------------------------------------------------------
        #region Events
        //

        //   
        #endregion // Events
        //-------------------------------------------------------------------------
    }

    //namespace RegistersData
    //{
    //    [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Unicode)]
    //    internal struct AppParamRegs
    //    {
    //        public float BoxTemp;
    //        public float CellTemp;
    //        public float OutHum;
    //        public float GenFlow;
    //        public float TestFlow;
    //        public float CellFlow;
    //        public float ZeroFlow;
    //        public float OutHumSpeed;
    //        public ushort OutHumTime;
    //        public float BoxTempSpeed;
    //        public ushort BoxTempTime;
    //        public float CellTempSpeed;
    //        public ushort CellTempTime;
    //        public float GenFlowSpeed;
    //        public ushort GenFlowTime;
    //        public float CellFlowSpeed;
    //        public ushort CellFlowTime;
    //    }

    //    [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Unicode)]
    //    internal struct PidFactors
    //    {
    //        public float kP;
    //        public float kI;
    //        public float kD;
    //    }

    //    [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Unicode)]
    //    internal struct PidData
    //    {
    //        public float Sp;
    //        public float Pv;
    //        public float Pval;
    //        public float Ival;
    //        public float Dval;
    //        public float Result;
    //    }

    //    /// <summary>
    //    /// Регистры устройства, сохраняемые в энергонезависимой памяти
    //    /// </summary>
    //    [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Unicode)]
    //    internal struct AppSavedData
    //    {
    //        public ushort DeviceAddress;
    //        public ushort Baudrate;
    //        public ushort StopBits;
    //        public AppParamRegs setPoints;
    //        [MarshalAs(UnmanagedType.ByValArray, SizeConst = ParamValues.WorkFlagsCount)]
    //        public ushort[] WorkType;
    //        [MarshalAs(UnmanagedType.ByValArray, SizeConst = ParamValues.NvDummyCount)]
    //        public ushort[] Dummy;
    //        public PidFactors BoxTempPidFactors;
    //        public PidFactors CellTempPidFActors;
    //        public PidFactors HumPidFactors;
    //        public PidFactors GenFlowPidFactors;
    //        public PidFactors TestFlowPidFactors;
    //        public PidFactors CellFlowPidFactors;
    //        public PidFactors ZeroFlowPidFactors;

    //        public ParamValues.GenProgramm_t genProgramm;
    //        public ParamValues.HumProgramm_t humProgramm;
    //        public ParamValues.CellProgramm_t cellProgramm;
    //        public ParamValues.AddProgramm_t addProgramm;

    //        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = ParamValues.TemperatureFactorsRegsCount)]
    //        //public ushort[]                 temperatureFactors;

    //        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = ParamValues.HumidityFactorsRegsCount)]
    //        //public ushort[]                 humidityFactors;

    //        public ushort Crc;
    //    }

    //    /// <summary>
    //    /// Регистры устройства, с текущими измеренными параметрами
    //    /// </summary>
    //    [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Unicode)]
    //    internal struct CurSensorValues
    //    {
    //        /// <summary>
    //        /// Температура внутри корпуса прибора
    //        /// </summary>
    //        public float BoxTemp;

    //        /// <summary>
    //        /// Температура ячейки
    //        /// </summary>
    //        public float CellTemp;

    //        /// <summary>
    //        /// Влажность выходного потока
    //        /// </summary>
    //        public float OutHum;

    //        /// <summary>
    //        /// Основной поток
    //        /// </summary>
    //        public float GenFlow;

    //        /// <summary>
    //        /// Поток ячейки
    //        /// </summary>
    //        public float CellFlow;

    //        /// <summary>
    //        /// 
    //        /// </summary>
    //        public float TestFlow;

    //        /// <summary>
    //        /// Поток нулевого газа
    //        /// </summary>
    //        public float ZeroFlow;

    //        /// <summary>
    //        /// Давление входного воздуха
    //        /// </summary>
    //        public float Pressure;

    //        /// <summary>
    //        /// Обходной поток 
    //        /// </summary>
    //        //public ushort BypassFlow;

    //        /// <summary>
    //        /// Влажность входного потока
    //        /// </summary>
    //        public float InputHum;

    //        public float OutHumSpeed;
    //        public ushort OutHumTime;
    //        public float BoxTempSpeed;
    //        public ushort BoxTempTime;
    //        public float CellTempSpeed;
    //        public ushort CellTempTime;
    //        public float GenFlowSpeed;
    //        public ushort GenFlowTime;
    //        public float CellFlowSpeed;
    //        public ushort CellFlowTime;
    //    }

    //    /// <summary>
    //    /// Все регисты устройства
    //    /// </summary>
    //    [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Unicode)]
    //    internal struct MbRegisters
    //    {
    //        public AppSavedData NvRegs;
    //        public CurSensorValues CurValues;
    //        [MarshalAs(UnmanagedType.ByValArray, SizeConst = ParamValues.AppFlagsCount)]
    //        public ushort[] AppFlags;
    //        [MarshalAs(UnmanagedType.ByValArray, SizeConst = ParamValues.DummyCount)]
    //        public ushort[] Dummy;
    //        public PidData BoxTempPidData;
    //        public PidData CellTempPidData;
    //        public PidData HumPidData;
    //        public PidData GenFlowPidData;
    //        public PidData TestFlowPidData;
    //        public PidData CellFlowPidData;
    //        public PidData ZeroFlowPidData;
    //    }

    //    public enum Baudrates : int
    //    {
    //        Bps9600 = 0,
    //        Bps14400 = 1,
    //        Bps19200 = 2,
    //        Bps38400 = 3,
    //        Bps57600 = 4,
    //        Bps115200 = 5
    //    }

    //    [Serializable]
    //    [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Unicode)]
    //    public struct RegressionFactors
    //    {
    //        public float A;
    //        public float B;
    //        public float C;
    //        public float D;
    //    }

    //    [Serializable]
    //    [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Unicode)]
    //    public struct FilterFactors
    //    {
    //        public float A;
    //        public float B;
    //    }

    //    [Serializable]
    //    [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Unicode)]
    //    public struct CalibrationData
    //    {
    //        [MarshalAs(UnmanagedType.ByValArray, SizeConst = ParamValues.AdcValuesCount)]
    //        public RegressionFactors[] IntAdcFactors;

    //        [MarshalAs(UnmanagedType.ByValArray, SizeConst = ParamValues.ExtAdcChannelsCount)]
    //        public RegressionFactors[] ExtAdcFactors;

    //        [MarshalAs(UnmanagedType.ByValArray, SizeConst = ParamValues.AdcValuesCount)]
    //        public FilterFactors[] IntAdcFilterFactors;
           
    //        [MarshalAs(UnmanagedType.ByValArray, SizeConst = ParamValues.ExtAdcChannelsCount)]
    //        public FilterFactors[] ExtAdcFilterFactors;

    //        public UInt32 Crc32;
    //    }
    //}
}
