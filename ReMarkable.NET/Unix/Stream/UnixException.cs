using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace ReMarkable.NET.Unix.Stream
{
    /// <summary>
    ///     Wraps a Unix exception in a class that makes the exception friendlier to read
    /// </summary>
    [Serializable]
    internal class UnixException : ExternalException
    {
        private static readonly string[] ErrorMessages =
        {
            "EPERM: Operation not permitted",
            "ENOENT: No such file or directory",
            "ESRCH: No such process",
            "EINTR: Interrupted system call",
            "EIO: Input/output error",
            "ENXIO: No such device or address",
            "E2BIG: Argument list too long",
            "ENOEXEC: Exec format error",
            "EBADF: Bad file descriptor",
            "ECHILD: No child processes",
            "EAGAIN: Resource temporarily unavailable",
            "ENOMEM: Cannot allocate memory",
            "EACCES: Permission denied",
            "EFAULT: Bad address",
            "ENOTBLK: Block device required",
            "EBUSY: Device or resource busy",
            "EEXIST: File exists",
            "EXDEV: Invalid cross-device link",
            "ENODEV: No such device",
            "ENOTDIR: Not a directory",
            "EISDIR: Is a directory",
            "EINVAL: Invalid argument",
            "ENFILE: Too many open files in system",
            "EMFILE: Too many open files",
            "ENOTTY: Inappropriate ioctl for device",
            "ETXTBSY: Text file busy",
            "EFBIG: File too large",
            "ENOSPC: No space left on device",
            "ESPIPE: Illegal seek",
            "EROFS: Read-only file system",
            "EMLINK: Too many links",
            "EPIPE: Broken pipe",
            "EDOM: Numerical argument out of domain",
            "ERANGE: Numerical result out of range",
            "EDEADLK: Resource deadlock avoided",
            "ENAMETOOLONG: File name too long",
            "ENOLCK: No locks available",
            "ENOSYS: Function not implemented",
            "ENOTEMPTY: Directory not empty",
            "ELOOP: Too many levels of symbolic links",
            "Invalid error code",
            "ENOMSG: No message of desired type",
            "EIDRM: Identifier removed",
            "ECHRNG: Channel number out of range",
            "EL2NSYNC: Level 2 not synchronized",
            "EL3HLT: Level 3 halted",
            "EL3RST: Level 3 reset",
            "ELNRNG: Link number out of range",
            "EUNATCH: Protocol driver not attached",
            "ENOCSI: No CSI structure available",
            "EL2HLT: Level 2 halted",
            "EBADE: Invalid exchange",
            "EBADR: Invalid request descriptor",
            "EXFULL: Exchange full",
            "ENOANO: No anode",
            "EBADRQC: Invalid request code",
            "EBADSLT: Invalid slot",
            "Invalid error code",
            "EBFONT: Bad font file format",
            "ENOSTR: Device not a stream",
            "ENODATA: No data available",
            "ETIME: Timer expired",
            "ENOSR: Out of streams resources",
            "ENONET: Machine is not on the network",
            "ENOPKG: Package not installed",
            "EREMOTE: Object is remote",
            "ENOLINK: Link has been severed",
            "EADV: Advertise error",
            "ESRMNT: Srmount error",
            "ECOMM: Communication error on send",
            "EPROTO: Protocol error",
            "EMULTIHOP: Multihop attempted",
            "EDOTDOT: RFS specific error",
            "EBADMSG: Bad message",
            "EOVERFLOW: Value too large for defined data type",
            "ENOTUNIQ: Name not unique on network",
            "EBADFD: File descriptor in bad state",
            "EREMCHG: Remote address changed",
            "ELIBACC: Can not access a needed shared library",
            "ELIBBAD: Accessing a corrupted shared library",
            "ELIBSCN: .lib section in a.out corrupted",
            "ELIBMAX: Attempting to link in too many shared libraries",
            "ELIBEXEC: Cannot exec a shared library directly",
            "EILSEQ: Invalid or incomplete multibyte or wide character",
            "ERESTART: Interrupted system call should be restarted",
            "ESTRPIPE: Streams pipe error",
            "EUSERS: Too many users",
            "ENOTSOCK: Socket operation on non-socket",
            "EDESTADDRREQ: Destination address required",
            "EMSGSIZE: Message too long",
            "EPROTOTYPE: Protocol wrong type for socket",
            "ENOPROTOOPT: Protocol not available",
            "EPROTONOSUPPORT: Protocol not supported",
            "ESOCKTNOSUPPORT: Socket type not supported",
            "EOPNOTSUPP: Operation not supported",
            "EPFNOSUPPORT: Protocol family not supported",
            "EAFNOSUPPORT: Address family not supported by protocol",
            "EADDRINUSE: Address already in use",
            "EADDRNOTAVAIL: Cannot assign requested address",
            "ENETDOWN: Network is down",
            "ENETUNREACH: Network is unreachable",
            "ENETRESET: Network dropped connection on reset",
            "ECONNABORTED: Software caused connection abort",
            "ECONNRESET: Connection reset by peer",
            "ENOBUFS: No buffer space available",
            "EISCONN: Transport endpoint is already connected",
            "ENOTCONN: Transport endpoint is not connected",
            "ESHUTDOWN: Cannot send after transport endpoint shutdown",
            "ETOOMANYREFS: Too many references: cannot splice",
            "ETIMEDOUT: Connection timed out",
            "ECONNREFUSED: Connection refused",
            "EHOSTDOWN: Host is down",
            "EHOSTUNREACH: No route to host",
            "EALREADY: Operation already in progress",
            "EINPROGRESS: Operation now in progress",
            "ESTALE: Stale file handle",
            "EUCLEAN: Structure needs cleaning",
            "ENOTNAM: Not a XENIX named type file",
            "ENAVAIL: No XENIX semaphores available",
            "EISNAM: Is a named type file",
            "EREMOTEIO: Remote I/O error",
            "EDQUOT: Disk quota exceeded",
            "ENOMEDIUM: No medium found",
            "EMEDIUMTYPE: Wrong medium type",
            "ECANCELED: Operation canceled",
            "ENOKEY: Required key not available",
            "EKEYEXPIRED: Key has expired",
            "EKEYREVOKED: Key has been revoked",
            "EKEYREJECTED: Key was rejected by service",
            "EOWNERDEAD: Owner died",
            "ENOTRECOVERABLE: State not recoverable",
            "ERFKILL: Operation not possible due to RF-kill",
            "EHWPOISON: Memory page has hardware error"
        };

        public int NativeErrorCode { get; }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        public UnixException()
            : this(Marshal.GetLastWin32Error())
        {
        }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        public UnixException(int error)
            : this(error, GetErrorMessage(error))
        {
        }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        public UnixException(string message)
            : this(Marshal.GetLastWin32Error(), message)
        {
        }

        public UnixException(int error, string message)
            : base(message)
        {
            NativeErrorCode = error;
        }

        public UnixException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected UnixException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            NativeErrorCode = info.GetInt32("NativeErrorCode");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));

            info.AddValue("NativeErrorCode", NativeErrorCode);
            base.GetObjectData(info, context);
        }

        private static string GetErrorMessage(int error)
        {
            return $"{ErrorMessages[error - 1]} ({error})";
        }
    }
}