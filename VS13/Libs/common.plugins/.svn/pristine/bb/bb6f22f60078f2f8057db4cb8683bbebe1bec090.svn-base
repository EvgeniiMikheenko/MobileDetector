using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace common.plugins
{
	public class TransportReadEventArgs : TransportEventArgs
	{
		public TransportReadEventArgs()
		{
			ID = uint.MaxValue;
			Data = null;
			Status = false;
			Result = string.Empty;
		}

		public TransportReadEventArgs(uint id, byte[] buf, bool status, string result)
		{
			ID = id;
			Data = buf;
			Status = status;
			Result = result;
		}
		//
		public byte[] Data { get; set; }

		public int ResultReadCount { get; set; }
	}
}
