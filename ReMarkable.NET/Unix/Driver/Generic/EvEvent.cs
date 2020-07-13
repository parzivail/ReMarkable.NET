using System.Runtime.InteropServices;

namespace ReMarkable.NET.Unix.Driver.Generic
{
    /// <summary>
    ///     Defines the interchange format for an input device event
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct EvEvent
    {
        /// <summary>
        ///     The epoch timestamp whole seconds component
        /// </summary>
        [FieldOffset(0)] public uint TimeWholeSeconds;

        /// <summary>
        ///     The epoch timestamp fractional seconds component, in microseconds
        /// </summary>
        [FieldOffset(4)] public uint TimeFractionMicroseconds;

        /// <summary>
        ///     The application-specific event type
        /// </summary>
        [FieldOffset(8)] public ushort Type;

        /// <summary>
        ///     The application-specific event code
        /// </summary>
        [FieldOffset(10)] public ushort Code;

        /// <summary>
        ///     The application-specific event value
        /// </summary>
        [FieldOffset(12)] public int Value;
    }
}