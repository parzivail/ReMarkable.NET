using System.Runtime.InteropServices;
using ReMarkable.NET.Unix.Driver.Display.Framebuffer;
using ReMarkable.NET.Unix.Stream;

namespace ReMarkable.NET.Unix.Driver.Display.EinkController
{
    /// <summary>
    ///     Provides methods for interacting with the device subsystem from userspace
    /// </summary>
    internal class DisplayIoctl
    {
        /// <summary>
        ///     The IOCTL handle for <see cref="FbUpdateData" /> payloads
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="request"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("libc", EntryPoint = "ioctl", SetLastError = true)]
        public static extern int Ioctl(SafeUnixHandle handle, IoctlDisplayCommand request, ref FbUpdateData data);

        /// <summary>
        ///     The IOCTL handle for <see cref="FbVarScreenInfo" /> payloads
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="request"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("libc", EntryPoint = "ioctl", SetLastError = true)]
        public static extern int Ioctl(SafeUnixHandle handle, IoctlDisplayCommand request, ref FbVarScreenInfo data);

        /// <summary>
        ///     The IOCTL handle for <see cref="FbFixedScreenInfo" /> payloads
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="request"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("libc", EntryPoint = "ioctl", SetLastError = true)]
        public static extern int Ioctl(SafeUnixHandle handle, IoctlDisplayCommand request, ref FbFixedScreenInfo data);
    }
}