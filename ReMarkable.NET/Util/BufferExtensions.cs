using System.Runtime.InteropServices;

namespace ReMarkable.NET.Util
{
    /// <summary>
    ///     Provides extension methods that operate on byte arrays as buffers
    /// </summary>
    public static class BufferExtensions
    {
        /// <summary>
        ///     Parses a struct from a byte stream
        /// </summary>
        /// <typeparam name="T">The struct type to read</typeparam>
        /// <param name="buffer">The buffer from which to read the struct</param>
        /// <returns>A populated struct of type <typeparamref name="T" /></returns>
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