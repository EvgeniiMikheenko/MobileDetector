using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LGAR
{
    class GsaRM : MikeDevice 
    {

        const ushort ZoneRegistersCount = 76;
        public const ushort ZoneSettingsStart = 0;
        public const ushort ZoneSettingsLength = 18; // !!!
        public const ushort ZoneSettingsFlagsOffset = 0;
        public const ushort ZoneSettingsParametersOffset = 0; // !

        public const ushort ZoneInputsStart = 70;
        public const int ZoneInputsLength = 23; // !
        public const ushort ZoneInputsFlagsOffset = 0;
        public const ushort ZoneInputsIDOffset = 6;     //6
        public const ushort ZoneInputsMD5Offset = 12;



        public GsaRM(ModbusMaster mas) : base(mas) { Init(); }


        public GsaRM(ModbusMaster mas, bool SerialTransport) : base(mas, SerialTransport) { Init(); }

        List<Dictionary<byte, byte>> stateId_control;

        ushort m_zone = 0;


        public const int ZonesCount = 3;

        public int Zone
        {
            get { return m_zone; }

            set
            {
                if (stateId_control == null)
                    return;

                if ((value < 0) || ((value - 1) >= stateId_control.Count))
                    return;

                if( m_zone != value )
                {
                    // 
                }
                m_zone = (ushort) value;
            }
        }

        public override byte GetId(DeviceParameter p)
        {
            if (m_zone == 0)
                return base.GetId(p);

            int index = m_zone - 1;

            return stateId_control[index][p.ID];
        }

        public byte GetUI_Id( DeviceParameter p )
        {
            return p.ID;
        }

        void Init()
        {
            stateId_control = new List<Dictionary<byte, byte>>(2);

            stateId_control.Add(new Dictionary<byte, byte>
            {
                //{01, 22},
                //{02, 23},
                //{03, 24},
                //{04, 25},
                //{05, 26},
                //{06, 27},
                //{07, 28},
                //{08, 29},
                //{09, 30},
                //{15, 36},



                { 01, 01},
                {02, 02},
                {03, 03},
                {04, 04},
                {05, 05},
                {06, 06},
                {07, 07},
                {08, 08},
                {09, 09},
                {15, 15},
                {10, 10},

            });

            stateId_control.Add(new Dictionary<byte, byte>
            {
                //{01, 43},
                //{02, 44},
                //{03, 45},
                //{04, 46},
                //{05, 47},
                //{06, 48},
                //{07, 49},
                //{08, 50},
                //{09, 51},
                //{15, 67},

                {01, 01},
                {02, 02},
                {03, 03},
                {04, 04},
                {05, 05},
                {06, 06},
                {07, 07},
                {08, 08},
                {09, 09},
                {15, 15},
                {10, 10},

            });
        }

        protected override void SettingsWrite()
        {
            if (m_zone == 0)
            {
                base.SettingsWrite();
                return;
            }

            //if (settings == null)
            //    return;

            //if (init)
            //    return;


            if (settings != null && !init)
            {
                ushort[] values = new ushort[ZoneSettingsLength - ZoneSettingsStart];
                bool changes = false;
                values[ZoneSettingsFlagsOffset] = FlagsWriteableWanted;
                changes |= (_flagsWriteable ^ FlagsWriteableWanted) != 0;

                // parameters
                foreach (var p in settings) // table
                    if (p != null)
                    {
                        values[ZoneSettingsParametersOffset + GetId(p)] = p.WantedValue;
                        //values[aSettingsParametersOffset + p.ID] = p.WantedValue;
                        changes |= p.RawValue != p.WantedValue; // несовпадение желаемого с действительным
                    }

                if (changes) // весь пакет настроек
                {
                    ioDelay();

                    // master.WriteMultipleRegisters(DeviceID, ZoneSettingsStart, values);
                    master.WriteMultipleRegisters(DeviceID, (ushort)(ZoneSettingsStart + m_zone * 21), values);
                    return;
                }


                //ushort[] values = new ushort[ ZoneRegistersCount ];
                //bool changes = false;

                //byte startAddr = stateId_control[m_zone - 1][0x01];

                //// parameters
                //foreach (var p in settings) // table
                //    if (p != null)
                //    {
                //        if ((p.ID == 0) || ((p.ID >= 10) && (p.ID < 15)))
                //            continue;

                //        if (p.ID > 15)
                //            break;

                //        int index = ( p.ID <= 9 ) ? (p.ID - 1) : (p.ID - 5);
                //        //values[aSettingsParametersOffset + GetId(p)] = p.WantedValue;
                //        values[ index ] = p.WantedValue;
                //        changes |= p.RawValue != p.WantedValue; // несовпадение желаемого с действительным
                //    }

                //if (changes) // весь пакет настроек
                //{
                //    ioDelay();
                //    master.WriteMultipleRegisters(DeviceID, startAddr, values);
                //    return;
                //}
            }
        }

        protected override void SettingsRead()
        {
            if (m_zone == 0)
            {
                base.SettingsRead();
                return;
            }


            if (settings == null)
                return;

            //if (init)
            //    return;


            //ioDelay();
            ////byte startAddr = stateId_control[m_zone - 1][0x01];
            //ushort[] st = master.ReadHoldingRegisters(DeviceID, 0, 75);

            //// settings
            //foreach (var p in settings)
            //    if (p != null) p.RawValue = st[ZoneSettingsParametersOffset + GetId(p)];
            ////if (p != null) p.RawValue = st[aSettingsParametersOffset + p.ID];
            //// settings initialization

            ////if (init) SettingsInit(st);

            //// inputs
            //ioDelay();
            //st = master.ReadHoldingRegisters(DeviceID, ZoneInputsStart, ZoneInputsLength);
            //FlagsReadOnly = st[aInputsFlagsOffset];
            //foreach (var p in inputs)
            //    if (p != null) p.RawValue = p.WantedValue = st[GetId(p)];




            ioDelay();

            ushort[] st = master.ReadHoldingRegisters(DeviceID, (ushort)(ZoneSettingsStart + m_zone * 21), ZoneSettingsLength);
            //ushort[] st = master.ReadHoldingRegisters(DeviceID, ZoneSettingsStart, ZoneSettingsLength);

            // zoom, flags
            _flagsWriteable = st[aSettingsFlagsOffset];

            // settings
            foreach (var p in settings)
              //  if (p != null) p.RawValue = st[aSettingsParametersOffset + GetId( p )];
                if (p != null) p.RawValue = st[aSettingsParametersOffset + p.ID];
            // settings initialization             
            if (init) SettingsInit(st);


            // inputs
            ioDelay();
            st = master.ReadHoldingRegisters(DeviceID, ZoneInputsStart, ZoneInputsLength);
            FlagsReadOnly = st[aInputsFlagsOffset];
            foreach (var p in inputs)
                //if (p != null) p.RawValue = p.WantedValue = st[ p.ID ];
                if (p != null) p.RawValue = p.WantedValue = st[p.ID];

            // id
            var sb = new StringBuilder();
            for (int i = aInputsIDOffset; i < aInputsIDOffset + 6; ++i)
                sb.AppendFormat(i != aInputsIDOffset ? ":{0:X4}" : "{0:X4}", st[i]);
            var sbid = sb.ToString();
            if (ID != sbid)
            {
                // CHANGED!
                ID = sb.ToString();
            }
            var sb2 = new StringBuilder();
            for (int i = aInputsMD5Offset; i < aInputsMD5Offset + 6; ++i)
                sb2.AppendFormat(i != aInputsMD5Offset ? ":{0:X4}" : "{0:X4}", st[i]);
            var sbmd5 = sb2.ToString();
            if (MD5 != sbmd5)
            {
                // CHANGED!
                MD5 = sb2.ToString();
            }






        }
    }
}
