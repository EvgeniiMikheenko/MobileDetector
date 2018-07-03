using System.Runtime.InteropServices;

namespace common.devices
{
    /// <summary>
    /// Структура для записи даты и времени в регистрах модбас
    /// </summary>
    [StructLayout( LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Unicode )]
    public struct DateTimeRegisters
    {
        /// <summary>
        /// Год
        /// </summary>
        public ushort Year;

        /// <summary>
        /// Месяц
        /// </summary>
        public ushort Month;

        /// <summary>
        /// День
        /// </summary>
        public ushort Day;

        /// <summary>
        /// Часы
        /// </summary>
        public ushort Hour;

        /// <summary>
        /// Минуты
        /// </summary>
        public ushort Minutes;

        /// <summary>
        /// Секунды
        /// </summary>
        public ushort Seconds;
    }
}
