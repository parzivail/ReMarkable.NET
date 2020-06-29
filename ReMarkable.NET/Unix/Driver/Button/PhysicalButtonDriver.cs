using System;
using System.Collections.Generic;
using System.Threading;
using ReMarkable.NET.Util;

namespace ReMarkable.NET.Unix.Driver.Button
{
    public sealed class PhysicalButtonDriver : InputDriver
    {
        public event EventHandler<PhysicalButtonEventCode> Pressed;
        public event EventHandler<PhysicalButtonEventCode> Released;

        public Dictionary<PhysicalButtonEventCode, KeyState> ButtonStates;

        internal PhysicalButtonDriver(string devicePath) : base(devicePath)
        {
            ButtonStates = new Dictionary<PhysicalButtonEventCode, KeyState>();
        }

        /// <inheritdoc />
        protected override void DataAvailable(object sender, DataAvailableEventArgs<EvEvent> e)
        {
            var data = e.Data;

            var eventType = (PhysicalButtonEventType)data.Type;

            switch (eventType)
            {
                case PhysicalButtonEventType.Syn:
                    break;
                case PhysicalButtonEventType.Key:
                    var button = (PhysicalButtonEventCode)data.Code;
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
