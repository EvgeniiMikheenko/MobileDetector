using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace common.plugins
{
	public class TransportWriteEventArgs : TransportEventArgs
	{
		public TransportWriteEventArgs()
		{
			ID = uint.MaxValue;
			Status = false;
			Result = string.Empty;
		}

		public TransportWriteEventArgs(uint id, bool status, string result)
		{
			ID = id;
			Status = status;
			Result = result;
		}
	}
}
