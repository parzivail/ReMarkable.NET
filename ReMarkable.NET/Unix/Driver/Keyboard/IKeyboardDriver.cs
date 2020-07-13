using System;
using System.Collections.Generic;
using ReMarkable.NET.Unix.Driver.Generic;

namespace ReMarkable.NET.Unix.Driver.Keyboard
{
    /// <summary>
    ///     Provides an interface through which a physical keyboard can be accessed
    /// </summary>
    public interface IKeyboardDriver
    {
        /// <summary>
        ///     Fired when a resting key is pressed
        /// </summary>
        event EventHandler<KeyPressEventArgs> Pressed;

        /// <summary>
        ///     Fired when a pressed key is released
        /// </summary>
        event EventHandler<KeyEventArgs> Released;

        /// <summary>
        ///     Contains a map of all instantaneous key states
        /// </summary>
        Dictionary<KeyboardKey, ButtonState> KeyStates { get; }
    }
}