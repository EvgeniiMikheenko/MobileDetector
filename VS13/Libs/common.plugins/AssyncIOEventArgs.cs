using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace common.plugins
{
	public class AssyncIOEventArgs
	{
		public AssyncIOEventArgs()
		{
			Buf = null;
			Index = 0;
			Count = 0;
			ID = uint.MinValue;
			IoType = IOType.Undefined;
		}

		public AssyncIOEventArgs(byte[] buf, int index, int count, uint id, IOType ioType)
		{
			Buf = buf;
			Index = index;
			Count = count;
			ID = id;
			IoType = ioType;
		}
		//
		public IOType IoType { get; set; }
		public byte[] Buf { get; set; }
		public int Index { get; set; }
		public int Count { get; set; }
		public uint ID { get; set; }
	}

	public enum IOType
	{
		Undefined,
		Read,
		Write
	}
}
