using System;

namespace ReMarkable.NET.Unix.Driver.Touchscreen
{
    /// <summary>
    ///     Provides an interface through which to access the device's touchscreen
    /// </summary>
    public interface ITouchscreenDriver
    {
        /// <summary>
        ///     Fired when a finger which is in contact with the screen moves
        /// </summary>
        event EventHandler<FingerState> Moved;

        /// <summary>
        ///     Fired when a finger contacts the screen
        /// </summary>
        event EventHandler<FingerState> Pressed;

        /// <summary>
        ///     Fired when a finger is removed from the screen
        /// </summary>
        event EventHandler<FingerState> Released;

        /// <summary>
        ///     The vertical resolution of the device
        /// </summary>
        int Height { get; }

        /// <summary>
        ///     The maximum number of fingers the device can track simultaneously
        /// </summary>
        int MaxFingers { get; }

        /// <summary>
        ///     The horizontal resolution of the device
        /// </summary>
        int Width { get; }
    }
}