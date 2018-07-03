/*
 * Сделано в SharpDevelop.
 * Пользователь: n.danilov
 * Дата: 06.06.2013
 * Время: 17:13
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace common.utils
{
	/// <summary>
	/// Description of Logger.
	/// </summary>
	public class ByteParser
	{
		public ByteParser(int columns)
		{
			m_Columns = columns;
		}
		//---------------------------------------------------------------------
		#region	Data
		//
		int m_Columns = 8;
		public int Columns 
		{
			get { return m_Columns; }
			set { m_Columns = value; }
		}
		
		int m_rows = 0;
		public int Rows 
		{
			get { return m_rows; }
			set { m_rows = value; }
		}
		//
		#endregion
		//---------------------------------------------------------------------
		#region	Public
		//
		public string[] Convert(byte[] buf, int index, int count, bool addRowNum)
		{
			if((buf == null) || (buf.Length <= 0))
				return null;
			//
			List<string> lst = new List<string>(1024);
			int column = -1;
			string str = "";
			if (addRowNum)
				str = ConvertToString(m_rows++, 8) + " :\t";
			for (int i = index; i < buf.Length; i++, count--) 
			{
				if (count <= 0)
					break;
				//
				column++;
				if(column >= m_Columns)
				{
					lst.Add(str);
					column = 0;
					if (addRowNum)
						str = ConvertToString(m_rows++, 8) + " :\t" + ConvertToString(buf[i], 2);
					else
						str = ConvertToString(buf[i], 2);
					//
					if(i != buf.Length - 1)
						str += "\t";
				}
				else
				{
					str += (ConvertToString(buf[i], 2));
					if(i != buf.Length - 1)
						str += "\t";
				}
			}
			//
			lst.Add(str);
			//
			return lst.ToArray();
		}
		
		public void Reset()
		{
			m_rows = 0;
		}
		//
		#endregion
		//---------------------------------------------------------------------
		#region	Private
		//
		string ConvertToString(int src, int minLen) 
		{
			string result = System.Convert.ToString(src, 16);
			while(result.Length < minLen)
			{
				result = "0" + result;
			}
			//
			return "0x" + (result).ToUpper();
		}
		//
		#endregion
	}
}
