using System.Runtime.InteropServices;

namespace ReMarkable.NET.Util
{
    static class BufferUtil
    {
        public static T ToStruct<T>(this byte[] buffer) where T : struct
        {
            var temp = new T();
            var size = Marshal.SizeOf(temp);
            var ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(buffer, 0, ptr, size);

            var ret = (T)Marshal.PtrToStructure(ptr, temp.GetType());
            Marshal.FreeHGlobal(ptr);

            return ret;
        }
    }
}
