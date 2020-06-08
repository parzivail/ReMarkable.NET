using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace ReMarkable.NET.Unix
{
    [SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    public sealed class SafeUnixHandle : SafeHandle
    {
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        private SafeUnixHandle()
            : base(new IntPtr(-1), true)
        {
        }

        public override bool IsInvalid
        {
            get { return this.handle == new IntPtr(-1); }
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            return UnsafeNativeMethods.Close(this.handle) != -1;
        }
    }
}
