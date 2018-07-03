using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace common.plugins
{
	public delegate void TransportReadEventHandler(object sender, TransportReadEventArgs e);

	public delegate void TransportWriteEventHandler(object sender, TransportWriteEventArgs e);
}
