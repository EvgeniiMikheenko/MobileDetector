using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace common.utils.Files
{
	public class BaseSerializer<T>
	{
		//---------------------------------------------------------------------------
		static bool CheckFile(string xmlFileName)
		{
			if (string.IsNullOrEmpty(xmlFileName))
				return false;
			//
			if (!File.Exists(xmlFileName))
				return false;
			//
			return true;
		}

		public static bool LoadFromFile(string xmlFileName, out T loadedSettings)
		{
			loadedSettings = default(T);
			//
			if (!CheckFile(xmlFileName))
				return false;
			//
			FileStream fs = null;
			try
			{
				fs = new FileStream(xmlFileName, FileMode.Open);
				XmlSerializer sr = new XmlSerializer(typeof(T));
				T result = (T)sr.Deserialize(fs);
				fs.Close();
				loadedSettings = result;
				return true;
			}
			catch
			{
				if (fs != null)
					fs.Close();
				loadedSettings = default(T);
				return false;
			}
		}

		public static bool SaveToFile(string xmlFileName, T savedSettings)
		{
			TextWriter tw = null;
			try
			{
				XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
				namespaces.Add("", null);
				tw = new StreamWriter(xmlFileName, false);
				XmlSerializer sr = new XmlSerializer(typeof(T));
				sr.Serialize(tw, savedSettings, namespaces);
				//
				tw.Close();
				return true;
			}
			catch
			{
				if (tw != null)
					tw.Close();
				//
				return false;
			}
		}

		//---------------------------------------------------------------------------
	}
}
