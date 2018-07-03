﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace common.utils.Files
{
	public class BinarySerializer<T>
	{
		//-------------------------------------------------------------------------
		public static bool SaveToFile(string fullFName, T serializedObject)
		{
			bool result = false;
			FileStream fs = null;
			//
			try
			{
				fs = new FileStream(fullFName, FileMode.Create);
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(fs, serializedObject);
				result = true;
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (fs != null)
					fs.Close();
			}
			//
			return result;
		}

		public static bool LoadFromFile(string fullFName, out T obj)
		{
			bool result = false;
			obj = default(T);
			FileStream fs = null;
			try
			{
				fs = new FileStream(fullFName, FileMode.Open);
				BinaryFormatter formatter = new BinaryFormatter();
				obj = (T)formatter.Deserialize(fs);
				//
				result = true;
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (fs != null)
					fs.Close();
			}
			//
			return result;
		}

		public static bool GetBytes(T serializedObject, ref byte[] buf)
		{
			bool result = false;
			MemoryStream mem = null;
			//
			try
			{
				mem = new MemoryStream();
				XmlSerializer ser = new XmlSerializer(typeof(T));
				ser.Serialize(mem, serializedObject);
				buf = mem.ToArray();
				result = true;
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (mem != null)
					mem.Close();
			}
			//
			return result;
		}

		public static bool FromBytes(byte[] bytes, ref T resultObject)
		{
			MemoryStream mem = null;
			bool result = true;
			try
			{
				mem = new MemoryStream(bytes);
				XmlSerializer ser = new XmlSerializer(typeof(T));
				resultObject = (T)ser.Deserialize(mem);
				result = true;
			}
			catch
			{
				result = false;
				resultObject = default(T);
			}
			finally
			{
				if (mem != null)
					mem.Close();
			}
			return result;
		}
		//-------------------------------------------------------------------------
	}
}