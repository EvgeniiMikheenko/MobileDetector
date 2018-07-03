
using System.Collections.Generic;
using System;
using System.Xml.Serialization;

namespace common
{
	[Serializable]
	public class CRingBuffer<T>
	{
		public CRingBuffer()
		{
			buf = new T[defaultBufLen];
			//
			Clear();
		}
		
		public CRingBuffer(int length)
		{
			if (length > 0)
				_capacity = length;
			else
				_capacity = defaultBufLen;
			//
			buf = new T[_capacity];
			//
			Clear();
		}
		//-------------------------------------------------------------------------
		#region Data
		//
		#region Const
		//
		const int defaultBufLen = 64;
		//
		#endregion
		//
		T[] buf;
		
		int zeroIndex;
		int wrIndex;
		bool enableRing;

		int lehgth;
		public int Lehgth
		{
			get { return lehgth; }
		}

		int _capacity;
		public int Capacity
		{
			get { return _capacity; }
			//set { _capacity = value; }
		}

		public bool IsFull
		{
			get { return (lehgth == _capacity);}
		}

		public T StartValue
		{
			get
			{
				return buf[zeroIndex];
			}
		}

		public T EndValue
		{
			get
			{
				int i = wrIndex;
				if (i < 0)
					i = 0;
				else if (i >= buf.Length)
					i = buf.Length - 1;
				//
				if ((i < 0) || (i >= buf.Length))
					return default(T);
				//
				return buf[i];
			}
		}
		//
		#endregion
		//-------------------------------------------------------------------------
		#region Public
		//
		public T this[int index]
		{
			get
			{
				if (index >= buf.Length)
					return default(T);
				//
				return buf[GetIndex(index)];
			}
		}

		public void Put(T value)
		{
			wrIndex++;
			if (wrIndex >= buf.Length)
				wrIndex = 0;
			//
			//
			buf[wrIndex] = value;
			//
			if (lehgth < buf.Length)
				lehgth++;
			else
				enableRing = true;
			//
			if (enableRing)
			{
				zeroIndex++;
				if (zeroIndex >= buf.Length)
					zeroIndex = 0;
			}
		}

		public void PutRange(T[] src, int index, int count)
		{
			if (src == null)
				return;
			//
			for (int i = index; i < src.Length; i++, count--)
			{
				if (count <= 0)
					break;
				//
				Put(src[i]);
			}
		}

		public bool Get(ref T result)
		{
			if (lehgth <= 0)
			{
				return false;
			}
			//
			result = buf[zeroIndex];
			zeroIndex++;
			lehgth--;
			if (zeroIndex >= buf.Length)
			{
				zeroIndex = 0;
			}
			//
			return true;
		}

		public int GetRange(ref T[] dst, int index, int count)
		{
			if (lehgth <= 0)
			{
				return 0;
			}
			//
			int cnt = 0;
			//
			for (int i = index; i < dst.Length; i++, count--)
			{
				if (count <= 0)
					break;
				//
				T value = default(T);
				if (!Get(ref value))
					break;
				//
				dst[i] = value;
				cnt++;
			}
			//
			return cnt;
		}

		public void Clear()
		{
			for (int i = 0; i < buf.Length; i++)
			{
				buf[i] = default(T);
			}
			//
			zeroIndex = 0;
			wrIndex = buf.Length;
			enableRing = false;
			lehgth = 0;
		}

		public List<T> GetList()
		{
			List<T> lst = new List<T>();
			for (int i = 0; i < Lehgth; i++)
			{
				lst.Add(buf[i]);
			}
			//
			return lst;
		}

		public void Set(int index, T value)
		{
			index = GetIndex(index);
			if ((index < 0) || (index >= buf.Length))
				return;
			//
			buf[index] = value;
		}
		//
		#endregion
		//-------------------------------------------------------------------------
		#region Private
		//
		int GetIndex(int index)
		{
			int i = zeroIndex + index;
			if (i < buf.Length)
				return i;
			//
			return i - buf.Length;
		}
		//
		#endregion
		//-------------------------------------------------------------------------
	}
}
