
using System;
namespace common.utils.Strings
{
	public class StringUtil
	{
		/// <summary>
		/// Поиск символов в строке
		/// </summary>
		/// <param name="source">Строка для поиска</param>
		/// <param name="ch">Символы, которые нужно найти</param>
		/// <returns>True - если хотя бы один символ найден, и  false в противном случае</returns>
		public static bool FindChar(string source, params char[] ch)
		{
			if ((string.IsNullOrEmpty(source)) || (ch == null) || (ch.Length <= 0))
				return false;
			//
			bool result = false;
			//
			for (int i = 0; i < source.Length; i++)
			{
				bool found = false;
				for (int j = 0; j < ch.Length; j++)
				{
					if (source[i] != ch[j])
						continue;
					//
					found = true;
					break;
				}
				if (!found)
					continue;
				//
				result = true;
				break;
			}
			//
			return result;
		}

		public static int GetCharIndex(string source, params char[] ch)
		{
			if ((string.IsNullOrEmpty(source)) || (ch == null) || (ch.Length <= 0))
				return -1;
			//
			int result = -1;
			//
			for (int i = 0; i < source.Length; i++)
			{
				bool found = false;
				for (int j = 0; j < ch.Length; j++)
				{
					if (source[i] != ch[j])
						continue;
					//
					found = true;
					break;
				}
				if (!found)
					continue;
				//
				result = i;
				break;
			}
			//
			return result;
		}

		public static string DigitToString(double value, int digits)
		{
			if (digits < 0)
				digits = 0;
			//
			string str = (Math.Round(value, digits)).ToString();
			if (digits == 0)
				return str;
			//
			int dotIndex = GetCharIndex(str, '.', ',');
			if (dotIndex > 0)
			{
				int x = (int)value;
				if (x == value)
					str += ',';
				//
				while (dotIndex != (str.Length - (digits + 1)))
				{
					str += '0';
					dotIndex = GetCharIndex(str, '.', ',');
				}
			}
			else
			{
				str += ',';
				for (int i = 0; i < digits; i++)
				{
					str += '0';
				}
			}
			//
			return str;
		}

		public string ConvertToHexString(int src, int minLen)
		{
			string result = System.Convert.ToString(src, 16);
			while (result.Length < minLen)
			{
				result = "0" + result;
			}
			//
			return "0x" + (result).ToUpper();
		}

        public static string DateToString()
        {
          DateTime dt = DateTime.Now;
          string dayStr = ((dt.Day >= 10) ? dt.Day.ToString() : ("0" + dt.Day.ToString()));
          string monthStr = ((dt.Month >= 10) ? dt.Month.ToString() : ("0" + dt.Month.ToString()));
          //
          string hoursStr = ((dt.Hour >= 10) ? dt.Hour.ToString() : ("0" + dt.Hour.ToString()));
          string minStr = ((dt.Minute >= 10) ? dt.Minute.ToString() : ("0" + dt.Minute.ToString()));
          string secStr = ((dt.Second >= 10) ? dt.Second.ToString() : ("0" + dt.Second.ToString()));
          //
          string date = dayStr + "_" + monthStr + "_" + dt.Year.ToString();
          string time = hoursStr + "_" + minStr + "_" + secStr;
          return date + "__" + time;
        }
	}
}
