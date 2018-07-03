using System;
using System.Runtime.InteropServices;

namespace common.utils
{
    public static class ValueUtil
    {
        public static Tret BufToStruct<Tret, Tparams>(Tparams[] buf) where Tret : struct
        {
            GCHandle handle = GCHandle.Alloc(buf, GCHandleType.Pinned);		// Выделить память

            IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buf, 0);	// и взять адрес
            Tret ret = (Tret)Marshal.PtrToStructure(ptr, typeof(Tret));		// создать структуру
            handle.Free();													// Освобождить дескриптор

            return ret;
        }

        public static Tret[] StructToBuff<Tret, Tparams>(Tparams value) where Tparams : struct
        {
            Tret tmp = default(Tret);
            Tret[] buf = new Tret[Marshal.SizeOf(value) / Marshal.SizeOf(tmp)];

            GCHandle handle = GCHandle.Alloc(buf, GCHandleType.Pinned);		// Выделить память
            IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buf, 0);	// и взять адрес
            Marshal.StructureToPtr(value, ptr, true);						// копировать в массив
            handle.Free();													// Освобождить дескриптор

            return buf;

        }

    }
}
