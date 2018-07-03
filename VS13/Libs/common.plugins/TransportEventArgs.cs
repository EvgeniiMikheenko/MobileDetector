using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace common.plugins
{
	public class TransportEventArgs
	{
		public TransportEventArgs()
		{
			ID = uint.MaxValue;
			Status = false;
			Result = string.Empty;
		}

		public TransportEventArgs(uint id, bool status, string result)
		{
			ID = id;
			Status = status;
			Result = result;
		}
		//
		public uint ID { get; set; }
		public bool Status { get; set; }
		public string Result { get; set; }
	}
}
