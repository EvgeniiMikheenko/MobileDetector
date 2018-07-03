using common.plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Transport;

namespace common.plagins
{
	public class PluginLoader<T> : IDisposable
	{
		public PluginLoader(string pluginsDir, string[] commonDll)
		{
			m_pluginsDir = pluginsDir;
			CommonDll = commonDll;
			LoadPlugins();
		}
		//-------------------------------------------------------------------------
		#region Data
		//
		readonly string m_pluginsDir;
		string m_appDir;
		//
		readonly string[] CommonDll;

		readonly List<PluginInfo<T>> m_pluginList = new List<PluginInfo<T>>(16);
		//
		public T[] AvailablePlugins
		{
			get
			{
				if ((m_pluginList == null) || (m_pluginList.Count < 0))
					return default(T[]);
				//
				List<T> lst = new List<T>(m_pluginList.Count);
				//
				for (int i = 0; i < m_pluginList.Count; i++)
				{
					if (m_pluginList[i] == null)
						continue;
					//
					lst.Add(m_pluginList[i].Plugin);
				}
				//
				return lst.ToArray();
			}
		}
		//
		#endregion //Data
		//-------------------------------------------------------------------------
		#region Public
		//
		public void Dispose()
		{
			Clear();
		}

		public void LoadPlugins()
		{
			Clear();
			//
			if (!Directory.Exists(m_pluginsDir))
			{
				GC.Collect();
				return;
			}
			//
			try
			{
				FileInfo fi = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
				m_appDir = fi.DirectoryName;
				//
				foreach (string f in Directory.GetFiles(m_pluginsDir, "*.dll"))
				{
					AppDomain domain = DomainHelper.CreateDomain(m_appDir, m_pluginsDir);
					//
					T[] types = LoadFrom(domain, f);
					if ((types == null) || (types.Length <= 0))
					{
						DomainHelper.UnloadDomain(domain);
						continue;
					}
					//
					for (int i = 0; i < types.Length; i++)
					{
						PluginInfo<T> pi = new PluginInfo<T>(types[i], domain, CommonDll);
						m_pluginList.Add(pi);
					}
				}
			}
			catch
			{ // ошибки доступа к файлам
				
			}
			finally
			{
				GC.Collect();
			}
		}

		public void Clear()
		{
			if ((m_pluginList == null) || (m_pluginList.Count <= 0))
				return;
			//
			while (m_pluginList.Count > 0)
			{
				PluginInfo<T> pi = m_pluginList[0];
				if (pi == null)
					continue;
				//
				IDisposable ids = pi.Plugin as IDisposable;
				if (ids != null)
					ids.Dispose();
				//
				DomainHelper.UnloadDomain(pi.Domain);
				m_pluginList.RemoveAt(0);
			}
		}
		//
		#endregion // Public
		//-------------------------------------------------------------------------
		#region Private
		//
		T[] LoadFrom(AppDomain domain, string fName)
		{
			if (string.IsNullOrEmpty(fName))
				return null;
			//
			if (!File.Exists(fName))
				return null;
			//
			try
			{
				LibraryLoader<T> loader = (LibraryLoader<T>)domain.CreateInstanceAndUnwrap(typeof(LibraryLoader<T>).Assembly.FullName, typeof(LibraryLoader<T>).FullName);
				if (CommonDll != null)
				{
					for (int i = 0; i < CommonDll.Length; i++)
					{
						if (!File.Exists(CommonDll[i]))
							continue;
						//
						loader.LoadLibrary(CommonDll[i]);
					}
				}
				loader.LoadLibrary(fName);
				T[] types = loader.GetPlugins();
				return types;
			}
			catch
			{
				return null;
			}
		}
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
