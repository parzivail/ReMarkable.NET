using System;
using System.Collections.Generic;
using ReMarkable.NET.Calibration;
using ReMarkable.NET.Unix.Driver.Generic;

namespace ReMarkable.NET.Unix.Driver.Digitizer
{
    /// <summary>
    ///     Provides an interface through which to access the device's digitizer
    /// </summary>
    public interface IDigitizerDriver
    {
        /// <summary>
        ///     Fired when the stylus makes physical contact with the device
        /// </summary>
        event EventHandler<DigitizerEventKeyCode> Pressed;

        /// <summary>
        ///     Fired when the stylus breaks physical contact with the device
        /// </summary>
        event EventHandler<DigitizerEventKeyCode> Released;

        /// <summary>
        ///     Fired when the stylus changes state
        /// </summary>
        event EventHandler<StylusState> StylusUpdate;

        /// <summary>
        ///     Fired when the stylus tool changes
        /// </summary>
        event EventHandler<StylusTool> ToolChanged;

        /// <summary>
        ///     The instantaneous states of the stylus tools and buttons
        /// </summary>
        Dictionary<DigitizerEventKeyCode, ButtonState> ButtonStates { get; }

        /// <summary>
        /// The currently applicable touchscreen calibrator
        /// </summary>
        public TouchscreenCalibrator Calibrator { get; set; }

        /// <summary>
        ///     The vertical resolution of the device
        /// </summary>
        int Height { get; }

        /// <summary>
        ///     The instantaneous state of the stylus
        /// </summary>
        StylusState State { get; }

        /// <summary>
        ///     The horizontal resolution of the device
        /// </summary>
        int Width { get; }
    }
}