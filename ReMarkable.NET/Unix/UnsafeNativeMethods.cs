using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using ReMarkable.NET.Unix.Driver.Display.EinkController;
using ReMarkable.NET.Unix.Driver.Display.Framebuffer;

namespace ReMarkable.NET.Unix
{
    internal static class UnsafeNativeMethods
    {
        [DllImport("libc", EntryPoint = "open", SetLastError = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA2101:Specify marshaling for P/Invoke string arguments", Justification = "Specifying a marshaling breaks rM compatability")]
        public static extern SafeUnixHandle Open(string path, uint flags, UnixFileMode mode);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [DllImport("libc", EntryPoint = "close", SetLastError = true)]
        public static extern int Close(IntPtr handle);
    }
}
