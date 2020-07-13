using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace ReMarkable.NET.Unix.Stream
{
    /// <summary>
    ///     Defines P/Invoke methods for opening and closing Unix file streams
    /// </summary>
    internal static class NativeUnixStreamMethods
    {
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [DllImport("libc", EntryPoint = "close", SetLastError = true)]
        public static extern int Close(IntPtr handle);

        [DllImport("libc", EntryPoint = "open", SetLastError = true)]
        [SuppressMessage("Globalization", "CA2101:Specify marshaling for P/Invoke string arguments", Justification =
            "Specifying a marshaling breaks rM compatability")]
        public static extern SafeUnixHandle Open(string path, uint flags, UnixFileMode mode);
    }
}