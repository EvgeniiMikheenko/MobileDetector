using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace types
{
	public delegate T GetValue<T>();
	public delegate void SetValue<T>(T value);
}
