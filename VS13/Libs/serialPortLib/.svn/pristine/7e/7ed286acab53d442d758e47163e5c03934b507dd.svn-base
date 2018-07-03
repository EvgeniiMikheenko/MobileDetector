using common.plugins;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace serialPortLib.types
{
	internal interface ISerialPortSettingsPage
	{
		SerialPortParams SpParams { get; set; }
		bool LogEnable { get; set; }
		//
		event SetValueEventHandler<string> PortNameChange;
		event SetValueEventHandler<Baudrates> BaudRateChange;
		event SetValueEventHandler<DataBits> DataBitsChange;
		event SetValueEventHandler<Parity> ParityChange;
		event SetValueEventHandler<StopBits> StopBitsChange;
		event SetValueEventHandler<Handshake> HandshakeChange;
		event SetValueEventHandler<bool, bool> OpenStatusChange;
		event SetValueEventHandler<bool> LogEnableChange;
		event SetValueEventHandler<bool> ShowLogDataChange;
		//
		void SetOpenStatus(bool value);
	}
}
