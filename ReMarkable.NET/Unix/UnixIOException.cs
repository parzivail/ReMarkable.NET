using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace ReMarkable.NET.Unix
{
    [Serializable]
    internal class UnixIoException : ExternalException
    {
        private readonly int _nativeErrorCode;

        public int NativeErrorCode => _nativeErrorCode;

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        public UnixIoException()
            : this(Marshal.GetLastWin32Error())
        {
        }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        public UnixIoException(int error)
            : this(error, GetErrorMessage(error))
        {
        }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        public UnixIoException(string message)
            : this(Marshal.GetLastWin32Error(), message)
        {
        }

        public UnixIoException(int error, string message)
            : base(message)
        {
            _nativeErrorCode = error;
        }

        public UnixIoException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected UnixIoException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _nativeErrorCode = info.GetInt32("NativeErrorCode");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue("NativeErrorCode", _nativeErrorCode);
            base.GetObjectData(info, context);
        }

        private static string GetErrorMessage(int error)
        {
            var errorDescription = UnsafeNativeMethods.Strerror(error);
            return errorDescription ?? $"Unknown error (0x{error:x})";
        }
    }
}
