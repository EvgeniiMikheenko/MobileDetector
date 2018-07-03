using common.plugins;
using System.ComponentModel;

namespace Transport
{
	public interface ITransport
	{
		/// <summary>
		/// Тип транспорта
		/// </summary>
		TransportType Transport { get; }

		/// <summary>
		/// Таймаут приема
		/// </summary>
		int ReadTimeout { get; set; }

		/// <summary>
		/// Таймаут передачи
		/// </summary>
		int WriteTimeout { get; set; }

		/// <summary>
		/// Функция передачи массива байт
		/// </summary>
		/// <param name="buf">Массив для пердачи.</param>
		/// <param name="index">Индекс в массиве, с которого начинаеися передача.</param>
		/// <param name="count">Количество байт для передачи.</param>
		/// <param name="result">Ссылка на строку, в которую записывается результат передачи.</param>
		/// <returns>Возвращает количество переданных байт.</returns>
		int Write(byte[] buf, int index, int count, ref string result);

		/// <summary>
		/// Функция чтения 
		/// </summary>
		/// <param name="dst">Ссылка на массив, в который записываются принятые байты.</param>
		/// <param name="index">Индекс в массиве, с которого начинается запись в массив.</param>
		/// <param name="count">Число байт для чтения.</param>
		/// <param name="result">Ссылка на строку, в которую записывается результат передачи.</param>
		/// <returns>Возвращает число прочитанных байт.</returns>
		int Read(ref byte[] dst, int index, int count, ref string result);

		/// <summary>
		/// Функция асинхронной записи. Добавляет в очередь операцию записи, и не дожидаясь окончания
		/// возвращает управление.
		/// Результат записи возвращается в событии AssyncWriteResultEvent.
		/// </summary>
		/// <param name="buf">Массив для пердачи.</param>
		/// <param name="index">Индекс в массиве, с которого начинаеися передача.</param>
		/// <param name="count">Количество байт для передачи.</param>
		/// <param name="id">Идентификатор транзакции</param>
		void AssyncWrite(byte[] buf, int index, int count, uint id);

		/// <summary>
		/// Функция асинхронного чтения. Добавляет в очередь операцию чтения, и не дожидаясь окончания
		/// чтения возвращает управление.
		/// Результат чтения возвращается в событии AssyncReadResultEvent.
		/// </summary>
		/// <param name="count">Число байт для чтения</param>
		/// <param name="id">Идентификатор транзакции</param>
		void AssyncRead(int count, uint id);

		/// <summary>
		/// Событие, возвращающее результат выполнения операции асинхронного чтения.
		/// </summary>
		event TransportReadEventHandler AssyncReadResultEvent;

		/// <summary>
		/// Событие, возвращающее результат выполнения операции асинхронной записи.
		/// </summary>
		event TransportWriteEventHandler AssyncWriteResultEvent;

		/// <summary>
		/// Функция отображает страницу настроек для приложения WPF
		/// </summary>
		/// <param name="parent">Grid, на котором отображается страница настроек</param>
		void ShowWPFSettingsPage(System.Windows.Controls.Grid parent);

		/// <summary>
		/// Функция отображает страницу настроек для приложения Windows Forms
		/// </summary>
		/// <param name="parent">Panel, на которой отображается страница настроек</param>
		void ShowSettingsPage(System.Windows.Forms.Panel parent);

		void Close();

		string ToString();

		string Name { get; }

		bool Open(object param);

		object GetParams();

		void ResetRxBuffer();
	}
	//
	public enum TransportType
	{
		[Description("Неопределенный тип")]
		Undefined,

		[Description("Последовательный порт")]
		SerialPort,

		[Description("Интерфейс USB")]
		USB
	}
}
