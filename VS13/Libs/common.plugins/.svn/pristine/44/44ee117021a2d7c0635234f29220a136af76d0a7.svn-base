/*
 * Сделано в SharpDevelop.
 * Пользователь: n.danilov
 * Дата: 07.06.2013
 * Время: 10:11
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;
using System.IO.Ports;

namespace common.plugins
{
	/// <summary>
	/// Description of SerialPortParams.
	/// </summary>
	[Serializable]
	public class SerialPortParams
	{
		public SerialPortParams()
		{
			Name = "COM1";
			Baudrate = Baudrates.Bps19200;
			DataBits = DataBits.DataBits8;
			Parity = Parity.None;
			StopBits = StopBits.One;
			Handshake = Handshake.None;
		}

		public SerialPortParams(SerialPortParams src)
		{
			if (src == null)
			{
				Name = "COM1";
				Baudrate = Baudrates.Bps19200;
				DataBits = DataBits.DataBits8;
				Parity = Parity.None;
				StopBits = StopBits.One;
				Handshake = Handshake.None;
				return;
			}
			//
			Name = src.Name;
			Baudrate = src.Baudrate;
			DataBits = src.DataBits;
			Parity = src.Parity;
			StopBits = src.StopBits;
			Handshake = src.Handshake;
		}
		//-------------------------------------------------------------------------
		#region Data
		//
		public string Name { get; set; }
		public Baudrates Baudrate { get; set; }
		public DataBits DataBits { get; set; }
		public Parity Parity { get; set; }
		public StopBits StopBits { get; set; }
		public Handshake Handshake { get; set; }
		//
		#endregion // Data
		//-------------------------------------------------------------------------
		#region Public
		//
		
		//
		#endregion // Public
		//-------------------------------------------------------------------------
		#region Private
		//
		
		//
		#endregion //Private
		//-------------------------------------------------------------------------
		#region Events
		//
		
		//
		#endregion //Events
		//-------------------------------------------------------------------------
	}
	//
	public enum Baudrates : int
	{
		Bps75 = 75,
		Bps110 = 110,
		Bps134 = 134,
		Bps150 = 150,
		Bps300 = 300,
		Bps600 = 600,
		Bps1200 = 1200,
		Bps1800 = 1800,
		Bps2400 = 2400,
		Bps4800 = 4800,
		Bps7200 = 7200,
		Bps9600 = 9600,
		Bps14400 = 14400,
		Bps19200 = 19200,
		Bps38400 = 38400,
		Bps57600 = 57600,
		Bps115200 = 115200,
		Bps12800 = 128000
	}
	
	public enum DataBits : int
	{
		DataBits4 = 4,
		DataBits5 = 5,
		DataBits6 = 6,
		DataBits7 = 7,
		DataBits8 = 8
	}
}
