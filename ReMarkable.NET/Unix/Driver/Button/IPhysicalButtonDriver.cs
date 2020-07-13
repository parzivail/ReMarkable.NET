using System;
using System.Collections.Generic;
using ReMarkable.NET.Unix.Driver.Generic;

namespace ReMarkable.NET.Unix.Driver.Button
{
    /// <summary>
    ///     Provides an interface through which to access the device's physical buttons
    /// </summary>
    public interface IPhysicalButtonDriver
    {
        /// <summary>
        ///     Fired when a resting button is pressed
        /// </summary>
        event EventHandler<PhysicalButton> Pressed;

        /// <summary>
        ///     Fired when a pressed button is released
        /// </summary>
        event EventHandler<PhysicalButton> Released;

        /// <summary>
        ///     Contains a map of all instantaneous button states
        /// </summary>
        Dictionary<PhysicalButton, ButtonState> ButtonStates { get; }
    }
}