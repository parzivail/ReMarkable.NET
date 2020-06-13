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

        /// <summary>
        /// The IOCTL handle for <see cref="FbUpdateData"/> payloads
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="request"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("libc", EntryPoint = "ioctl", SetLastError = true)]
        public static extern int Ioctl(SafeUnixHandle handle, IoctlDisplayCommand request, ref FbUpdateData data);

        /// <summary>
        /// The IOCTL handle for <see cref="FbVarScreenInfo"/> payloads
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="request"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("libc", EntryPoint = "ioctl", SetLastError = true)]
        public static extern int Ioctl(SafeUnixHandle handle, IoctlDisplayCommand request, ref FbVarScreenInfo data);

        /// <summary>
        /// The IOCTL handle for <see cref="FbFixedScreenInfo"/> payloads
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="request"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("libc", EntryPoint = "ioctl", SetLastError = true)]
        public static extern int Ioctl(SafeUnixHandle handle, IoctlDisplayCommand request, ref FbFixedScreenInfo data);
    }
}
