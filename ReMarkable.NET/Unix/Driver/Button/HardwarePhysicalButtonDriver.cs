using System;
using System.Collections.Generic;
using ReMarkable.NET.Util;

namespace ReMarkable.NET.Unix.Driver.Button
{
    public sealed class HardwarePhysicalButtonDriver : UnixInputDriver, IPhysicalButtonDriver
    {
        public Dictionary<PhysicalButton, ButtonState> ButtonStates { get; }

        public event EventHandler<PhysicalButton> Pressed;
        public event EventHandler<PhysicalButton> Released;
        
        internal HardwarePhysicalButtonDriver(string devicePath) : base(devicePath)
        {
            ButtonStates = new Dictionary<PhysicalButton, ButtonState>();
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
                    var button = (PhysicalButton)data.Code;
                    var buttonState = (ButtonState)data.Value;

                    ButtonStates[button] = ButtonState.Pressed;

                    switch (buttonState)
                    {
                        case ButtonState.Released:
                            Released?.Invoke(null, button);
                            break;
                        case ButtonState.Pressed:
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
