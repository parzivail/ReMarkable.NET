using System;
using System.Collections.Generic;
using System.Threading;
using ReMarkable.NET.Util;

namespace ReMarkable.NET.Unix.Driver.Button
{
    public sealed class PhysicalButtonDriver : InputDriver
    {
        public event EventHandler<PhysicalButtonCode> Pressed;
        public event EventHandler<PhysicalButtonCode> Released;

        public Dictionary<PhysicalButtonCode, KeyState> ButtonStates;

        public PhysicalButtonDriver() : base("/dev/input/event2")
        {
            ButtonStates = new Dictionary<PhysicalButtonCode, KeyState>();
        }

        /// <inheritdoc />
        protected override void DataAvailable(object sender, DataAvailableEventArgs<EvEvent> e)
        {
            var data = e.Data;

            var eventType = (PhysicalButtonType)data.Type;

            switch (eventType)
            {
                case PhysicalButtonType.Syn:
                    break;
                case PhysicalButtonType.Key:
                    var button = (PhysicalButtonCode)data.Code;
                    var buttonState = (KeyState)data.Value;

                    ButtonStates[button] = KeyState.Pressed;

                    switch (buttonState)
                    {
                        case KeyState.Released:
                            Released?.Invoke(null, button);
                            break;
                        case KeyState.Pressed:
                            Pressed?.Invoke(null, button);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
