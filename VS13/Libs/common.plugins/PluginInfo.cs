using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace common.plugins
{
	public class PluginInfo<T>
	{
		public PluginInfo(T plugin, AppDomain domain, string[] commonDll)
		{
			Plugin = plugin;
			PluginType = typeof(T);
			Domain = domain;
			CommonDll = commonDll;
		}
		//-------------------------------------------------------------------------
		#region Data
		//
		public Type PluginType { get; set; }

		public T Plugin { get; set; }

		public AppDomain Domain { get; set; }

		public string[] CommonDll { get; set; }
		//
		#endregion //Data
		//-------------------------------------------------------------------------
		#region Public
		//

		//
		#endregion // Public
		//-------------------------------------------------------------------------
		#region Private
		//

		//
		#endregion // Private
		//-------------------------------------------------------------------------
		#region Events
		//

		//
		#endregion // Events
		//-------------------------------------------------------------------------
	}
}
