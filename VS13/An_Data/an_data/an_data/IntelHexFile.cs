using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Utils.IO.Files
{
    public class IntelHexFile : IDisposable
    {
        public IntelHexFile()
        {

        }

        public IntelHexFile(string fName)
        {
            Load(fName);
        }
        //-------------------------------------------------------------------------
        #region Data
        //
        const char StartSeparator = ':';
        //
        FileStream m_FStream;
        StreamReader m_Reader;
        //
        string m_FileName;
        public string FileName
        {
            get { return m_FileName; }
            //set { m_FileName = value; }
        }

		public string ShortFileName { get; private set; }

        bool m_IsOpen;
        public bool IsOpen
        {
            get { return m_IsOpen; }
            //set { m_IsOpen = value; }
        }
        //
        enum RowIndexes : int
        {
            SeparatorIndex  = 0,
            ByteCountIndex  = 1,
            AddressIndex    = 3,
            RowTypeIndex    = 7,
            DataIndex       = 9
        }

        enum RowType : byte
        {
            DataRow                     = 0x00,
            EndOfFile                   = 0x01,
            SegmentAddressRecord        = 0x02,
            StartSegmentAddressRecord   = 0x03,
            ExtendedAddressRecord       = 0x04,
            StartLinearAddressRecord    = 0x05,
            Undefined
        }

        List<string> m_Rows;

        long m_FileSize;
        public long FileSize
        {
            get { return m_FileSize; }
            //set { m_FileSize = value; }
        }

        int m_DataByteCount;
        public int DataByteCount
        {
            get { return m_DataByteCount; }
            //set { m_DataByteCount = value; }
        }

        DateTime m_CreateDate;

        public DateTime CreateDate
        {
            get { return m_CreateDate; }
           // set { m_CreateDate = value; }
        }

        string m_LastError;
        public string LastError
        {
            get { return m_LastError; }
            //set { m_LastError = value; }
        }

        bool m_IsCheckSumError;

		public bool IsError
		{
			get { return (!string.IsNullOrEmpty(m_LastError)); }
		}
        //
        #endregion //Data
        //-------------------------------------------------------------------------
        #region Public
        //
        public bool Load(string fName)
        {
            if (string.IsNullOrEmpty(fName))
            {
                m_LastError = "Неверное имя файла";
                return false;
            }
            //
            if (!File.Exists(fName))
            {
                m_LastError = "Файл не существует";
                return false;
            }
            //
            Close();
            //
            try
            {
                FileInfo fi = new FileInfo(fName);
                m_FStream = new FileStream(fName, FileMode.Open , FileAccess.ReadWrite);
                m_Reader = new StreamReader(m_FStream);
                m_Rows = new List<string>(102400);
                //
                bool start = false;
                bool eofFound = false;
                string row;
                bool result = true;
                for (int k = 0; ; k++ )
                {
                    row = m_Reader.ReadLine();
                    if (row == null)
                        break;
                    //
                    if (!start)
                    {
                        if (row.Length <= 0)
                            continue;
                        //
                        start = true;
                    }
                    //
                    RowType rt = GetRowType(row);
                    if (rt == RowType.EndOfFile)
                        eofFound = true;
                    else if (rt == RowType.Undefined)
                    {
                        result = false;
                        //
                        if (m_IsCheckSumError)
                            m_LastError = "Ошибка контрольной суммы в строке " + (k + 1).ToString();
                        else
                            m_LastError = "Ошибка в строке " + (k + 1).ToString();
                        //
                        break;
                    }
                    //
                    m_Rows.Add(row);
                    if (eofFound)
                        break;
                }
                //
                if (!result)
                {
                    Close();
                    return false;
                }
                //
                m_FileName = fName;
                m_IsOpen = true;
                m_FileSize = fi.Length;
                m_DataByteCount = GetDataSize();
                m_CreateDate = fi.CreationTime;
                m_LastError = "";
				ShortFileName = fi.Name;
                //
                return m_IsOpen;
            }
            catch (Exception ex)
            {
                m_LastError = ex.Message;
                Close();
                return false;
            }
        }

        public void Close()
        {
            m_FileSize = 0;
            m_DataByteCount = 0;
            m_IsOpen = false;
            m_FileName = "";
            //
            if (m_Rows != null)
            {
                m_Rows.Clear();
                m_Rows = null;
            }
            //
            if (m_Reader != null)
            {
                try
                {
                    m_Reader.Close();
                    m_Reader.Dispose();
                }
                catch { }
                m_Reader = null;
            }
            //
            if (m_FStream != null)
            {
                try
                {
                    m_FStream.Close();
                    m_FStream.Dispose();
                }
                catch { }
                m_FStream = null;
            }
        }

        public void Dispose()
        {
            Close();
        }

        public byte[] GetData()
        {
            if (!m_IsOpen)
            {
                m_LastError = "Файл не открыт";
                return null;
            }
            //
            if (m_Rows == null)
            {
                m_LastError = "Формат файла не верный";
                return null;
            }
            //
            List<byte> lst = new List<byte>(m_DataByteCount);
            //
            bool start = false;
            bool result = true;
            for (int i = 0; i < m_Rows.Count; i++)
            {
                RowType rt = GetRowType(m_Rows[i]);
                if (!start)
                {
                    if (rt != RowType.DataRow)
                        continue;
                    //
                    start = true;
                }
                //
                if (rt == RowType.EndOfFile)
                    break;
                //
                if (rt == RowType.Undefined)
                {
                    result = false;
                    //
                    if(m_IsCheckSumError)
                        m_LastError = "Ошибка контрольной суммы в строке " + (i + 1).ToString();
                    else
                        m_LastError = "Ошибка в строке " + (i + 1).ToString();
                    //
                    break;
                }
                //
                if (rt != RowType.DataRow)
                    continue;
                //
                int count = (int)GetRowByteCount(m_Rows[i]);
                int index = (int)RowIndexes.DataIndex;
                try
                {
                    for (int j = 0; j < count; j++)
                    {
                        string str = m_Rows[i].Substring(index, 2);
                        byte b = ToByte(str);
                        lst.Add(b);
                        index += 2;
                    }
                }
                catch (Exception ex)
                {
                    m_LastError = ex.Message;
                    result = false;
                    break;
                }
            }
            //
            if (!result)
                return null;
            //
            return lst.ToArray();
        }

        public int Read(byte[] dst, int offset, int count)
        {
            if (!m_IsOpen)
                return 0;
            //
            try
            {
                m_FStream.Seek(0, SeekOrigin.Begin);
                return m_FStream.Read(dst, offset, count);
            }
            catch (Exception ex)
            {
                m_LastError = ex.Message;
                return 0;
            }
        }

		public UInt32 GetStartAddr()
		{
			if (!m_IsOpen)
				return 0;
			//
			UInt32 result = 0;
			bool baseAddrFound = false;
			byte b1, b2;
			string str;

			for (int i = 0; i < m_Rows.Count; i++)
			{
				RowType rt = GetRowType(m_Rows[i]);

				if(!baseAddrFound)
				{
					if (rt != RowType.ExtendedAddressRecord)
						continue;
					//
					str = m_Rows[i].Substring((int)RowIndexes.DataIndex);
					b1 = ToByte(str);
					str = str.Substring(2);
					b2 = ToByte(str);
					result = (UInt32)((b1 << 24) | (b2 << 16));
					//
					baseAddrFound = true;
					continue;
				}
				//
				if (rt != RowType.DataRow)
					continue;
				//
				str = m_Rows[i].Substring((int)RowIndexes.AddressIndex);
				b1 = ToByte(str);
				str = str.Substring(2);
				b2 = ToByte(str);
				result |= (UInt32)((b1 << 8) | (b2));
				//
				break;
			}


			//
			return result;
		}

		public int GetDataSize()
		{
			if (!m_IsOpen)
				return 0;
			//
			if (m_Rows == null)
				return 0;
			//
			int size = 0;
			for (int i = 0; i < m_Rows.Count; i++)
			{
				RowType rt = GetRowType(m_Rows[i]);
				if (rt != RowType.DataRow)
					continue;
				//
				string str = m_Rows[i].Substring((int)RowIndexes.ByteCountIndex, 2);
				size += (int)ToByte(str);
			}
			//
			return size;
		}
        //
        #endregion // Public
        //-------------------------------------------------------------------------
        #region Private
        //
        byte ToByte(string src)
        {
            if (string.IsNullOrEmpty(src))
                return 0x00;
            //
            string str = "";
            if (src.Length < 2)
                str = src;
            else
                str = src.Substring(0, 2);
            //
            try
            {
                byte b = Convert.ToByte(str, 16);
                return b;
            }
            catch 
            {
                return 0x00;
            }
        }

        RowType GetRowType(string row)
        {
            if (string.IsNullOrEmpty(row))
                return RowType.Undefined;
            //
            if (row.Length < (int)RowIndexes.DataIndex)
                return RowType.Undefined;
            //
            if (row[(int)RowIndexes.SeparatorIndex] != StartSeparator)
                return RowType.Undefined;
            //
            m_IsCheckSumError = !CHeckRowCheckSum(row);
            if (m_IsCheckSumError)
                return RowType.Undefined;
            //
            string str = row.Substring((int)RowIndexes.RowTypeIndex);
            byte b = ToByte(str);
            //
            return (RowType)b;
        }

        bool CHeckRowCheckSum(string row)
        {
            if (string.IsNullOrEmpty(row))
                return false;
            //
            bool result = true;
            byte checkSum = 0;
            for (int i = 1; i < row.Length; i += 2)
            {
                if ((i + 1) >= row.Length)
                {
                    result = false;
                    break;
                }
                //
                string str = row.Substring(i, 2);
                byte b = ToByte(str);
                checkSum += b;
            }
            if (!result)
                return false;
            //
            return (checkSum == 0);
        }

        byte GetRowByteCount(string row)
        {
            RowType rt = GetRowType(row);
            if (rt == RowType.Undefined)
                return 0;
            //
            string str = row.Substring((int)RowIndexes.ByteCountIndex, 2);
            return ToByte(str);
        }
        //
        #endregion // Private
        //-------------------------------------------------------------------------
    }
}
