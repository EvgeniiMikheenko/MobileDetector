using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modbus;
using Modbus.Device;
using System.Net.Sockets;
using System.Threading;
using ZedGraph;
using System.IO;

namespace LGAR
{
    public class MikeDevice: IDisposable
    {
        protected volatile ModbusMaster master;
        Thread poll;

        public static string _DEVICE_HEAD = "AIG2";

        protected IList<DeviceParameter> settings = null, inputs = null; // списки параметров

        public byte DeviceID = 1; // номер устройства = 0 т.к теперь можно втыкать приборы с любыми адресами, 0 - описан в протаколе.
        public bool init = true;  // необходима инициализация параметров
        bool serial = false; // последовательный порт требует задержек

        /// <summary>
        /// Задержка перед выполнением запроса ModBus.
        /// </summary>
        public int ioDelayValue = 100; // ms

        #region * Mike -



        public const ushort aSettingsStart = 0;
        public const ushort aSettingsLength = 22; // !!!
        public const ushort aSettingsFlagsOffset = 0;
        public const ushort aSettingsParametersOffset = 0; // !

        public const ushort aInputsStart = 34;      //30
        public const int aInputsLength = 43; // !
        public const ushort aInputsFlagsOffset = 0;
        public const ushort aInputsIDOffset = 6;
        public const ushort aInputsMD5Offset = 12;


        //public const ushort aSettingsStart = 0;
        //public const ushort aSettingsLength = 90; // !!!
        //public const ushort aSettingsFlagsOffset = 0;
        //public const ushort aSettingsParametersOffset = 0; // !

        //public const ushort aInputsStart = 71;
        //public const int aInputsLength = 6; // !
        //public const ushort aInputsFlagsOffset = 0;
        //public const ushort aInputsIDOffset = 76;
        //public const ushort aInputsMD5Offset = 82;

        #endregion

        #region - Construct -

        public MikeDevice(ModbusMaster mas)
        {
            master = mas;
            master.Transport.Retries = 0;
            // init
            settings = Mike.Initialize();
            inputs = Mike.InitializeInputs();

            // thread
            poll = new Thread(_poll) { Name = "Device Poll", IsBackground = true };
            poll.Start();
        }

        public MikeDevice(ModbusMaster mas, bool SerialTransport)
            : this(mas) { serial = SerialTransport; }

        /// <summary>
        /// Уничтожить.
        /// </summary>
        public void Dispose()
        {
            try
            {
                var m = master;
                master = null;
                if(_update != null) _update.Set();
                Thread.Sleep(0);
                //if (poll.IsAlive) poll.Join(10);
                //if(poll.IsAlive) poll.Abort();
                if (m != null) m.Dispose();
            }
            catch { }

        }

#endregion

        #region * Some properties -

        /// <summary>
        /// Список параметров.
        /// </summary>
        public IList<DeviceParameter> Settings { get { return settings; } }

        public DeviceParameter getGUIparam(int idx)
        {
            foreach (var a in settings)
                if (GetId( a ) == idx) return a;
            return null;
        }
        public double getGUIvalue(int idx)
        {
            foreach (var a in settings)
                //if (a.ID == idx) return a.Value;
                if (GetId( a ) == idx) return a.Value;
            return double.NaN;
        }

        public virtual byte GetId(DeviceParameter p)
        {
            if (p == null)
                return 0;

            return p.ID;
        }

        /// <summary>
        /// Список входных параметров.
        /// </summary>
        public IList<DeviceParameter> Inputs { get { return inputs; } }

        public double getInput(int idx)
        {
            foreach (var a in inputs)
                //if (a.ID == idx) return a.Value;
                if (GetId( a ) == idx) return a.Value;
            return double.NaN;
        }

        /// <summary>
        /// Записываемые флаги.
        /// </summary>
        public ushort FlagsWriteable
        {
            get { return _flagsWriteable; }
            set { FlagsWriteableWanted = value; }
        }
        protected ushort FlagsWriteableWanted;
        protected ushort _flagsWriteable;

        /// <summary>
        /// Флаги только для чтения.
        /// </summary>
        public ushort FlagsReadOnly { get; protected set; }

        /// <summary>
        /// Установить бит массива флагов.
        /// </summary>
        /// <param name="flag">Битовый массив.</param>
        /// <param name="mask">Маска флага.</param>
        /// <param name="value">Значение.</param>
        /// <returns></returns>
        public static ushort SetFlag(ushort flag, ushort mask, bool value)
        {
            if (value) return (ushort)(flag | mask);
            return (ushort)(flag & ~mask & ushort.MaxValue);
        }
        public static void SetFlag(ref ushort flag, Mike.aFlagsW mask, bool value)
        {
            flag = SetFlag(flag, (ushort)mask, value);
        }

        /// <summary>
        /// Флаг прерывания цикла.
        /// </summary>
        public volatile bool AbortFlag = false;

        public ModbusMaster Master
        {
            get { return master; }
        }

        public string ID { get; protected  set; }
        public string MD5 { get; protected set; }

        #endregion

        #region * Settings -

        System.Diagnostics.Stopwatch delayer = new System.Diagnostics.Stopwatch();

        /// <summary>
        /// Задержка после запросов, чтобы переходник проср*лся.
        /// </summary>
        public void ioDelay()
        {
            long toWait = ioDelayValue - delayer.ElapsedMilliseconds;
            if (ioDelayValue > 0)
            {
                delayer.Restart();
                if(toWait > 0) Thread.Sleep((int)toWait + 1);
            }
        }

        /// <summary>
        /// Чтение текущих значений параметров из устройства.
        /// </summary>

        ushort ref_null;
        int sign = 1;
        protected virtual void SettingsRead()
        {
            ioDelay();
            ushort[] st = master.ReadHoldingRegisters(DeviceID, aSettingsStart, aSettingsLength);

            // zoom, flags
            _flagsWriteable = st[aSettingsFlagsOffset];
            ref_null = st[6];
            // settings
            foreach (var p in settings)
                //if (p != null) p.RawValue = st[aSettingsParametersOffset + GetId( p )];
                if (p != null) p.RawValue = st[aSettingsParametersOffset + p.ID];
            // settings initialization
            if (init) SettingsInit(st);

            // inputs
            ioDelay();


            st = master.ReadHoldingRegisters(DeviceID, aInputsStart, aInputsLength);
            FlagsReadOnly = st[aInputsFlagsOffset];






            short[] st1 = new short[st.Length];
            for (int i = 0; i < st.Length; i++ )
             st1[i] = (short)st[i];

            if (st1[2] < 0)
            {
                sign = -1;
                st[2] = (ushort)(st1[2] * sign);
            }

            else
                sign = 1;




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

        /// <summary>
        /// Инициализация настроек значениями из устройства.
        /// </summary>
        /// <param name="st"></param>
        protected void SettingsInit(ushort[] st)
        {
            FlagsWriteableWanted = _flagsWriteable = st[aSettingsFlagsOffset];

            // table
            foreach (var p in settings)
                if (p != null) p.WantedValue = p.RawValue;

            init = false;
        }

        /// <summary>
        /// Запись желаемых параметров в устройство.
        /// </summary>
        protected virtual void SettingsWrite()
        {
            if (settings != null && !init)
            {
                ushort[] values = new ushort[aSettingsLength - aSettingsStart];
                bool changes = false;
                values[aSettingsFlagsOffset] = FlagsWriteableWanted;
                changes |= (_flagsWriteable ^ FlagsWriteableWanted) != 0;

                // parameters
                foreach (var p in settings) // table
                    if (p != null)
                    {
                        values[aSettingsParametersOffset + GetId( p )] = p.WantedValue;
                        //values[aSettingsParametersOffset + p.ID] = p.WantedValue;
                        changes |= p.RawValue != p.WantedValue; // несовпадение желаемого с действительным
                    }

                if (changes) // весь пакет настроек
                {
                    ioDelay();
                    master.WriteMultipleRegisters(DeviceID, aSettingsStart, values);
                    return;
                }
            }

            // default
            //ioDelay();
            //master.WriteSingleRegister(DeviceID, aSettingsStart, wzoom);
        }

        /// <summary>
        /// Сохранить настройки в файл.
        /// </summary>
        /// <param name="filename"></param>
        public void SettingsLoad(string filename)
        {
            if (settings == null || init)
                throw new InvalidOperationException
                ("Устройство не готово к загрузке настроек.");

            var phash = new Dictionary<byte, DeviceParameter>(settings.Count);

            //foreach (var p in settings) if (p != null) phash[p.ID] = p;
            foreach (var p in settings) if (p != null) phash[GetId(p)] = p;

            // FIXME: проверка ошибок
            using (var read = File.OpenText(filename))
            {
                string head = read.ReadLine();
                if (head != _DEVICE_HEAD)
                    throw new InvalidDataException("Неверный формат данных.");

                while (!read.EndOfStream)
                {
                    string[] ss = read.ReadLine().Split(' ');
                    if (ss[0] == "I")
                    {
                        phash[byte.Parse(ss[1])].Value = double.Parse(ss[2],
                            System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
            }

            SettingsWrite();
        }

        /// <summary>
        /// Загрузить настройки из файла.
        /// </summary>
        /// <param name="filename"></param>
        public void SettingsSave(string filename)
        {
            if (settings == null)
                throw new InvalidOperationException
                ("Нет доступных значений настроек для сохранения.");

            // values?
            ushort[] values = new ushort[settings.Count];
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(_DEVICE_HEAD);

            // settings
            foreach (var p in settings)
                //sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                //                    "# {3}\r\nI {0} {1} {2:X4}\r\n", p.ID, p.Value, p.RawValue, p.Name);

                sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                    "# {3}\r\nI {0} {1} {2:X4}\r\n", GetId(p), p.Value, p.RawValue, p.Name);

            File.WriteAllText(filename, sb.ToString());
        }

        #endregion
        
        #region * Update =

        AutoResetEvent _update = null;

        /// <summary>
        /// Поток фонового общения с устройством.
        /// </summary>
        /// <param name="e"></param>
        void _poll(object e)
        {
            _update = new AutoResetEvent(false);

            try
            {
                while (_update.WaitOne())
                {
                    if (master == null) break;
                    AbortFlag = false;

                    try
                    {
                        OnDebugStatus("W");
                        SettingsWrite(); // --> [DEVICE]
                        if (AbortFlag) continue;

                        OnDebugStatus("R");
                        SettingsRead(); // <-- [DEVICE]
                        if (AbortFlag) continue;

                    }
                    catch (Exception ex)
                    {
                        if (master != null)
                            OnUpdate(this, new DeviceEventArgs() { Error = ex });
                        else return;
                        continue;
                    }

                    if (master == null) break;
                    OnDebugStatus("UPD");
                    Thread.Sleep(1);
                    // Invoke
                    OnUpdate(this, new DeviceEventArgs()
                    {
                        
                        Data = new double[] { inputs[0].Value - ((float)ref_null/1000f), sign*inputs[1].Value, inputs[2].Value, ((float)ref_null / 1000f) },
                    });
                }
            }
            finally
            { _update.Close(); }

        }

        /// <summary>
        /// Событие обновления данных.
        /// <remarks>Вызов из другого потока.</remarks>
        /// </summary>
        public event EventHandler<DeviceEventArgs> Update = null;

        /// <summary>
        /// Отладочный статус.
        /// </summary>
        public event EventHandler<DeviceDebugEventArgs> DebugStatus = null;

        /// <summary>
        /// Вызвать обработчик события обновления данных.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnUpdate(object sender, DeviceEventArgs e)
        {
            var upd = Update;
            if (upd != null)
                try
                {
                    upd.Invoke(sender, e); // blocking call
                }
                catch { }
        }

        private void OnDebugStatus(string status)
        {
            if (DebugStatus != null)
                DebugStatus.BeginInvoke(this, new DeviceDebugEventArgs() { DebugStatus = status },
                    null, null);
        }

        /// <summary>
        /// Запустить фоновое обновление данных.
        /// </summary>
        public void UpdateAsync()
        {
            var u = _update;
            if (u != null) u.Set();
        }

        #endregion

    }

    /// <summary>
    /// Класс данных события.
    /// </summary>
    public class DeviceEventArgs : EventArgs
    {
        /// <summary>
        /// Исключение при наличии ошибки.
        /// </summary>
        public Exception Error = null;
        /// <summary>
        /// Точки.
        /// </summary>
        public double[] Data = null;

    }

    /// <summary>
    /// Класс отладочного события.
    /// </summary>
    public class DeviceDebugEventArgs : EventArgs
    {
        public string DebugStatus;
    }
}
