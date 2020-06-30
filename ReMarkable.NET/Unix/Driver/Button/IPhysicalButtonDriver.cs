using System;
using System.Collections.Generic;

namespace ReMarkable.NET.Unix.Driver.Button
{
    public interface IPhysicalButtonDriver
    {
        /// <summary>
        /// Fired when a resting button is pressed
        /// </summary>
        event EventHandler<PhysicalButton> Pressed;

        /// <summary>
        /// Fired when a pressed button is released
        /// </summary>
        event EventHandler<PhysicalButton> Released;

        /// <summary>
        /// Contains a map of all instantaneous button states
        /// </summary>
        Dictionary<PhysicalButton, ButtonState> ButtonStates { get; }
    }
}