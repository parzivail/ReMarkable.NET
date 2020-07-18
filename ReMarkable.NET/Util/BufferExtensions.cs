using System;
using System.IO;
using System.Runtime.InteropServices;
using NLog.LayoutRenderers.Wrappers;

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

        public static byte[] ToByteArray<T>(this T data) where T : struct
        {
            var bufferSize = Marshal.SizeOf(data);
            var byteArray = new byte[bufferSize];

            var handle = Marshal.AllocHGlobal(bufferSize);
            try
            {
                Marshal.StructureToPtr(data, handle, true);
                Marshal.Copy(handle, byteArray, 0, bufferSize);

            }
            finally
            {
                Marshal.FreeHGlobal(handle);
            }
            return byteArray;
        }

        public static void SaveStruct<T>(this Stream stream, T data) where T : struct
        {
            stream.Write(data.ToByteArray());
        }

        public static T LoadStruct<T>(this Stream stream, int size) where T : struct
        {
            var buf = new byte[size];
            if (stream.Read(buf, 0, size) != size)
                throw new InvalidDataException("Unable to read struct, insufficient data");

            return buf.ToStruct<T>();
        }
    }
}