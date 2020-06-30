using System;

namespace ReMarkable.NET.Unix.Driver.Touchscreen
{
    public interface ITouchscreenDriver
    {
        /// <summary>
        /// Fired when a finger contacts the screen
        /// </summary>
        event EventHandler<Finger> Pressed;

        /// <summary>
        /// Fired when a finger is removed from the screen
        /// </summary>
        event EventHandler<Finger> Released;

        /// <summary>
        /// Fired when a finger which is in contact with the screen moves
        /// </summary>
        event EventHandler<Finger> Moved;

        /// <summary>
        /// The horizontal resolution of the device
        /// </summary>
        int Width { get; }

        /// <summary>
        /// The vertical resolution of the device
        /// </summary>
        int Height { get; }

        /// <summary>
        /// The maximum number of fingers the device can track simultaneously
        /// </summary>
        int MaxFingers { get; }
    }
}