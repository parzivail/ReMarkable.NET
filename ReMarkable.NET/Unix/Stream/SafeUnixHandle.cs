using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace ReMarkable.NET.Unix.Stream
{
    /// <summary>
    ///     Provides a safe handle to a device that can be have IOCTL commands issued to it
    /// </summary>
    [SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    internal sealed class SafeUnixHandle : SafeHandle
    {
        public override bool IsInvalid => handle == new IntPtr(-1);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        private SafeUnixHandle() : base(new IntPtr(-1), true)
        {
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            return NativeUnixStreamMethods.Close(handle) != -1;
        }
    }
}