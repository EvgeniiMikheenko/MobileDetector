using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LGAR
{
    public class DeviceParameter: ICloneable
    {
    /// <summary>
    /// Класс параметра устройства.
    /// </summary>
        byte id = byte.MaxValue;
        string name = null;
        ushort rawValue;
        ushort wValue;
        ushort raw0 = ushort.MinValue, raw1 = ushort.MaxValue;
        double eng0 = double.NaN , eng1 = double.NaN;
        bool scaled = false;
        double k = double.NaN;
        string format = null;

        const string defaultFormat = "G5";

        public DeviceParameter(byte Id, string Name)
        {
            id = Id;
            name = Name;
        }

        public DeviceParameter(byte Id, string Name, ushort Raw0, ushort Raw1)
            : this(Id, Name)
        {
            if (raw0 < raw1) { raw0 = Raw0; raw1 = Raw1; }
        }

        public DeviceParameter(byte Id, string Name, double Eng0, double Eng1)
            : this(Id, Name, ushort.MinValue, ushort.MaxValue, Eng0, Eng1) { }



        public DeviceParameter(byte Id, string Name, double K)
            : this(Id, Name, ushort.MinValue * K, ushort.MaxValue * K) { }




        public DeviceParameter(byte Id, string Name, ushort Raw0, ushort Raw1, double Eng0, double Eng1)
            : this(Id, Name, Raw0, Raw1)
        {
            if (!double.IsNaN(Eng0) && Eng0 != Eng1)
            {
                eng0 = Eng0; eng1 = Eng1;
                if (raw0 < raw1)
                {
                    k = (eng1 - eng0) / (raw1 - raw0);
                    if (Eng0 > Eng1) // swap
                    {
                        eng0 = Eng1;
                        eng1 = Eng0;
                    }
                    scaled = !double.IsNaN(k);
                }
            }
        }

        /// <summary>
        /// Клонировать объект.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new DeviceParameter(id, name, raw0, raw1, eng0, eng1) { format = format };
        }

        /// <summary>
        /// Номер параметра.
        /// </summary>
        public byte ID { get { return id; } }
        /// <summary>
        /// Наименование параметра.
        /// </summary>
        public string Name { get { return name; } }

        /// <summary>
        /// Сырое значение.
        /// </summary>
        public ushort RawValue
        {
            get { return rawValue; }
            set { rawValue = value; }
        }

        /// <summary>
        /// Желаемое значение.
        /// </summary>
        /// <param name="value"></param>
        public ushort WantedValue
        {
            set { wValue = value; }
            get { return wValue; }
        }

        /// <summary>
        /// Флаг применения масштаба к сырым значениям.
        /// </summary>
        public bool Scaled { get { return scaled; } }

        public ushort Raw0 { get { return raw0; } }
        public ushort Raw1 { get { return raw1; } }
        public double Eng0 { get { return eng0; } }
        public double Eng1 { get { return eng1; } }

        /// <summary>
        /// Формат вывода.
        /// </summary>
        public string StringFormat
        {
            get { return format; }
            set {
                (1.0).ToString(value); // check validity
                format = value; 
            }
        }

        /// <summary>
        /// Значение.
        /// </summary>
        public double Value
        {
            get { return Decode(RawValue); }
            set { wValue = Encode(value); }
        }

        /// <summary>
        /// Текстовое значение.
        /// </summary>
        public string StringValue
        {
            get { return Value.ToString(format ?? defaultFormat); }
            set { Value = double.Parse(value); }
        }

        public virtual double Decode(ushort raw)
        {
            return scaled ? (raw - raw0) * k + eng0 : raw;
        }

        public virtual ushort Encode(double value)
        {
            if (scaled)
                if (value >= eng0 && value <= eng1)
                    return unchecked((ushort)((value - eng0) / k + raw0));
                else throw new ArgumentOutOfRangeException(name ?? "#" + id,
                string.Format("Значение вне допустимого диапазона: {0} .. {1}", eng0, eng1));
            if (raw0 < raw1)
                if (value >= raw0 && value <= raw1)
                    return (ushort)value;
                else throw new ArgumentOutOfRangeException(name ?? "#" + id,
                string.Format("Значение вне допустимого диапазона: {0} .. {1}", raw0, raw1));
            return (ushort)value;
        }

        public override string ToString()
        {
            return double.IsNaN(Value) ? null : StringValue;
        }

    }


}
