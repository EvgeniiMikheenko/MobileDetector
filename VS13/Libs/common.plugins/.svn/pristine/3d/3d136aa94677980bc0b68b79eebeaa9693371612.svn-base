using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace common.plugins
{
	[Serializable]
	public class LibraryLoader<T> //: MarshalByRefObject
	{
		//
		Assembly m_assembly;
		string m_lastError;
		//
		public bool LoadLibrary(string fName)
		{
			if (!File.Exists(fName))
				return false;
			//
			try
			{
				m_assembly = Assembly.LoadFrom(fName);
				m_lastError = "";
				return true;
			}
			catch (Exception e)
			{
				m_lastError = e.Message;
				return false;
			}
		}
		//
		public T[] GetPlugins()
		{
			if(m_assembly == null)
				return null;
			//
			Type[] types = m_assembly.GetTypes();
			if((types == null) || (types.Length <= 0))
				return null;
			//
			List<T> lst = new List<T>(types.Length);
			for (int i = 0; i < types.Length; i++)
			{
				try
				{
					T t = (T)Activator.CreateInstance(types[i]);
					lst.Add(t);
					m_lastError = "";
				}
				catch(Exception ex)
				{
					m_lastError = ex.Message;
				}
			}
			//
			return lst.ToArray();
		}
		//
		public string GetLastError()
		{
			return m_lastError;
		}
		//

	}
}
