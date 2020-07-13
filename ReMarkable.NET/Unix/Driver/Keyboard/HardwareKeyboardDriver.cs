using System;
using System.Collections.Generic;
using ReMarkable.NET.Unix.Driver.Generic;

namespace ReMarkable.NET.Unix.Driver.Keyboard
{
    /// <summary>
    ///     Provides methods for monitoring a physical keyboard attached to the device
    /// </summary>
    public sealed class HardwareKeyboardDriver : UnixInputDriver, IKeyboardDriver
    {
        /// <inheritdoc />
        public event EventHandler<KeyPressEventArgs> Pressed;

        /// <inheritdoc />
        public event EventHandler<KeyEventArgs> Released;

        /// <inheritdoc />
        public Dictionary<KeyboardKey, ButtonState> KeyStates { get; }

        /// <summary>
        ///     Creates a new <see cref="HardwareKeyboardDriver" />
        /// </summary>
        /// <param name="device">The device event stream to poll for new events</param>
        public HardwareKeyboardDriver(string device) : base(device)
        {
            KeyStates = new Dictionary<KeyboardKey, ButtonState>();
        }

        /// <inheritdoc />
        protected override void DataAvailable(object sender, DataAvailableEventArgs<EvEvent> e)
        {
            var data = e.Data;

            var eventType = (KeyboardEventType)data.Type;

            switch (eventType)
            {
                case KeyboardEventType.Syn:
                case KeyboardEventType.Msc:
                case KeyboardEventType.Led:
                    break;
                case KeyboardEventType.Key:
                {
                    var key = (KeyboardKey)data.Code;
                    var state = (ButtonState)data.Value;

                    switch (state)
                    {
                        case ButtonState.Released:
                            Released?.Invoke(this, new KeyEventArgs(key));
                            break;
                        case ButtonState.Pressed:
                        case ButtonState.Repeat:
                            Pressed?.Invoke(this, new KeyPressEventArgs(key, state == ButtonState.Repeat));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(state), state, state.GetType().Name);
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventType), eventType, eventType.GetType().Name);
            }
        }
    }
}