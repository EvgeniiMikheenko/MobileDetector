using System;
using System.ComponentModel;
using System.Reflection;

namespace common.utils
{
	public static class EnumDescriptor
	{
		public static string Get(Enum value)
		{
			Type t = value.GetType();
			FieldInfo fi = t.GetField(value.ToString());
			DescriptionAttribute[] da = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
			//
			return (da.Length > 0) ? da[0].Description : value.ToString();
		}

		public static bool ToEnum<T>(string desc, ref object resultValue)
		{
			bool found = false;
			foreach (Enum t in Enum.GetValues(typeof(T)))
			{
				if (!string.Equals(desc, EnumDescriptor.Get(t)))
					continue;
				found = true;
				resultValue = t;
				break;
			}
			//
			return found;
		}

		public static T ToEnum<T>(string desc)
		{
			object result = default(T);
			if (!ToEnum<T>(desc, ref result))
				return default(T);
			//
			return (T)result;
		}
	}
}
