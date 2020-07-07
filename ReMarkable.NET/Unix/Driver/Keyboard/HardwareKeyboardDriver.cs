using System;
using System.Collections.Generic;
using System.Text;
using ReMarkable.NET.Unix.Driver.Generic;
using ReMarkable.NET.Util;

namespace ReMarkable.NET.Unix.Driver.Keyboard
{
    public sealed class HardwareKeyboardDriver : UnixInputDriver, IKeyboardDriver
    {
        public event EventHandler<KeyPressEventArgs> Pressed;

        public event EventHandler<KeyEventArgs> Released;

        public Dictionary<KeyboardKey, ButtonState> KeyStates { get; }

        public HardwareKeyboardDriver(string device) : base(device)
        {
        }

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
