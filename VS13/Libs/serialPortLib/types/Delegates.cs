using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace serialPortLib.types
{
	public delegate void SetValueEventHandler<T>(object sender, T value);

	public delegate TResult SetValueEventHandler<T, TResult>(object sender, T value);
}
