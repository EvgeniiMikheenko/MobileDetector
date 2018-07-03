using System;
using System.Runtime.InteropServices;

namespace common.devices
{
    public static partial class ParamValues
    {
        public const int UsmDummy1RegistersCount = 64;
        public const int UsmDummy2RegistersCount = 64;
        public const int UsmDummy3RegistersCount = 64;
        public const int UsmDummy4RegistersCount = 64;
        public const int UsmHardwareVersionUIntCount = 4;
        public const int UsmConfigDummyRegistersCount = 16;
    }
}


namespace common.devices.USM
{
    [StructLayout( LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Unicode )]
    public struct MbUsmRegisters
    {
        /// <summary>
        /// Температура
        /// </summary>
        public UInt32 Temperature;
         
        /// <summary>
        /// Влажность
        /// </summary>
        public UInt32 Humidity;

        /// <summary>
        /// Атмосферное давление
        /// </summary>
        public UInt32 Pressure;

        /// <summary>
        /// Скорость ветра
        /// </summary>
        public float WindSpeed;

        /// <summary>
        /// Направление ветра
        /// </summary>
        public float WindDir;
        //
        /// <summary>
        /// Дата и время
        /// </summary>
        public DateTimeRegisters DateTime;
        // 
        /// <summary>
        /// Резерв 1
        /// </summary>
        [MarshalAs( UnmanagedType.ByValArray, SizeConst = ParamValues.UsmDummy1RegistersCount )]
        public ushort[] Dummy1;
        //---------------------------------------------------------------------
        //
        /// <summary>
        /// Усредненная температура
        /// </summary>
        public UInt32 TemperatureAvg;

        /// <summary>
        /// Усредненная влажность
        /// </summary>
        public UInt32 HumidityAvg;

        /// <summary>
        /// Усредненное давление
        /// </summary>
        public UInt32 PressureAvg;

        /// <summary>
        /// Усредненная скорость ветра
        /// </summary>
        public float WindSpeedAvg;

        /// <summary>
        /// Усредненное направление ветра
        /// </summary>
        public float WindDirAvg;

        /// <summary>
        /// Время усреднения
        /// </summary>
        public UInt32 AverageTime;
        //
        /// <summary>
        /// Резерв 2
        /// </summary>
        [MarshalAs( UnmanagedType.ByValArray, SizeConst = ParamValues.UsmDummy2RegistersCount )]
        public ushort[] Dummy2;
        //---------------------------------------------------------------------
        //
        /// <summary>
        /// Время пролета УЗ в прямом направлении для пары 1
        /// </summary>
        public float Pair1AvgTime;

        /// <summary>
        /// Время пролета УЗ в обратном направлении для пары 1
        /// </summary>
        public float Pair1Time;

        /// <summary>
        /// Время пролета УЗ в прямом направлении для пары 2
        /// </summary>
        public float Pair2AvgTime;

        /// <summary>
        /// Время пролета УЗ в обратном направлении для пары 2
        /// </summary>
        public float Pair2Time;
        //
        /// <summary>
        /// Резерв 3
        /// </summary>
        [MarshalAs( UnmanagedType.ByValArray, SizeConst = ParamValues.UsmDummy3RegistersCount )]
        public ushort[] Dummy3;
        //---------------------------------------------------------------------
        /// <summary>
        /// Версия ПО
        /// </summary>
        //public UInt32 FirmwareVersion;

        ///// <summary>
        ///// Версия плат (нескольких)
        ///// </summary>
        //[MarshalAs( UnmanagedType.ByValArray, SizeConst = ParamValues.UsmHardwareVersionUIntCount )]
        //public UInt32[] HardwareVersion;

        /// <summary>
        /// MD5 прошивки
        /// </summary>
        //[MarshalAs( UnmanagedType.ByValArray, SizeConst = ParamValues.FirmwareMD5RegistersCount )]
        //public ushort[] MD5;

        /// <summary>
        /// Напряжение питания
        /// </summary>
        public UInt32 VccSysVoltage;



        public UInt32 Soft_Version;

        public UInt32 HardVersion1;
        public UInt32 HardVersion2;
        public UInt32 HardVersion3;
        public UInt32 HardVersion4;
        /// <summary>
        /// Флаги
        /// </summary>
        public ushort ReadFlags;
        public ushort WriteFlags;

        /// <summary>
        /// Параметры чтения журнала
        /// </summary>
        //public JornalData Jornal;
        //---------------------------------------------------------------------
        /// <summary>
        /// Конфигурация прибора
        /// </summary>
        public MbUsmConfigRegisters Config;
        //
        /// <summary>
        /// Резерв 4
        /// </summary>
       // [MarshalAs( UnmanagedType.ByValArray, SizeConst = ParamValues.UsmDummy4RegistersCount )]
        //public ushort[] Dummy4;
    }

    [StructLayout( LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Unicode )]
    public struct MbUsmConfigRegisters
    {
        /// <summary>
        /// Модбас адрес
        /// </summary>
        public ushort MbAddress;

        /// <summary>
        /// Скорость порта
        /// </summary>
        public ushort Baudrate;

        /// <summary>
        /// Количество стопбит
        /// </summary>
        public ushort Stopbits;

        /// <summary>
        /// Время усреднения
        /// </summary>
        public ushort AverageTime;

        /// <summary>
        /// Угол ориентации прибора
        /// </summary>
        public float Angle;

        /// <summary>
        /// Расстояние между первой парой датчиков
        /// </summary>
        public ushort Pair1Distance;

        /// <summary>
        /// Расстояние между второй парой датчиков
        /// </summary>
        public ushort Pair2Distance;

        /// <summary>
        /// Широта (GPS)
        /// </summary>
        public UInt32 Latitude;

        /// <summary>
        /// Долгота (GPS)
        /// </summary>
        public UInt32 Longitude;


        public ushort time_error_first;

        public ushort time_error_second;

        public ushort Crc;
        /// <summary>
        /// Строка описания местоположения прибора
        /// </summary>
        //[MarshalAs( UnmanagedType.ByValArray, SizeConst = ParamValues.LocationDescriptionRegistersCount )]
        //public ushort[] LocationDescription;

        /// <summary>
        /// Флаги
        /// </summary>
       // public ushort Flags;

        /// <summary>
        /// Резерв
        /// </summary>
       // [MarshalAs( UnmanagedType.ByValArray, SizeConst = ParamValues.UsmConfigDummyRegistersCount )]
       // public ushort[] Dummy1;

        /// <summary>
        /// Контрольная сумма
        /// </summary>
       // public ushort Crc;
    }

}
