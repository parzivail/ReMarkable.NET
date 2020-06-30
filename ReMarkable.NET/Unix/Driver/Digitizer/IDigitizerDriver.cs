using System;

namespace ReMarkable.NET.Unix.Driver.Digitizer
{
    public interface IDigitizerDriver
    {
        /// <summary>
        /// Fired when the stylus tool changes
        /// </summary>
        event EventHandler<StylusTool> ToolChanged;

        /// <summary>
        /// Fired when the stylus makes physical contact with the device
        /// </summary>
        event EventHandler<DigitizerEventKeyCode> Pressed;

        /// <summary>
        /// Fired when the stylus breaks physical contact with the device
        /// </summary>
        event EventHandler<DigitizerEventKeyCode> Released;

        /// <summary>
        /// Fired when the stylus changes state
        /// </summary>
        event EventHandler<StylusState> StylusUpdate;

        /// <summary>
        /// The horizontal resolution of the device
        /// </summary>
        int Width { get; }

        /// <summary>
        /// The vertical resolution of the device
        /// </summary>
        int Height { get; }
    }
}