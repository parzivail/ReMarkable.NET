using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using ReMarkable.NET.IO;

namespace ReMarkable.NET.Unix
{
    public static class UnsafeNativeMethods
    {
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [DllImport("libc", EntryPoint = "close", SetLastError = true)]
        public static extern int Close(IntPtr handle);

        /// <summary>
        /// The IOCTL handle for <see cref="FbUpdateData"/> payloads
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="request"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("libc", EntryPoint = "ioctl", SetLastError = true)]
        public static extern int Ioctl(SafeUnixHandle handle, IoctlCommand request, ref FbUpdateData data);

        [DllImport("libc", EntryPoint = "open", SetLastError = true)]
        public static extern SafeUnixHandle Open(string path, uint flag, UnixFileMode mode);

        internal static string Strerror(int error)
        {
            return $"strerror: {error}";
//            try
//            {
//                var buffer = new StringBuilder(256);
//                var result = Strerror(error, buffer, (ulong)buffer.Capacity);
//                return (result != -1) ? buffer.ToString() : null;
//            }
//            catch (EntryPointNotFoundException)
//            {
//                return null;
//            }
        }

        [DllImport("libc", EntryPoint = "strerror", SetLastError = true)]
        private static extern int Strerror(int error, [Out] StringBuilder buffer, ulong length);
    }

    public enum UnixFileMode
    {
        ReadOnly = 0,
        WriteOnly = 1,
        ReadWrite = 2
    }
}
