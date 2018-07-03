using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace types
{
	public class PropertyEventArgs<T>
	{

		public PropertyEventArgs(T oldValue, T newValue)
		{
			OldValue = oldValue;
			NewValue = newValue;
		}

		//-------------------------------------------------------------------------
		#region Data
		//
		public T OldValue { get; set; }
		public T NewValue { get; set; }
		//
		#endregion
		//-------------------------------------------------------------------------
	}
}
