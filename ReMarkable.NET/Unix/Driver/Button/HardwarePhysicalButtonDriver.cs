using System;
using System.Collections.Generic;
using ReMarkable.NET.Unix.Driver.Generic;

namespace ReMarkable.NET.Unix.Driver.Button
{
    /// <summary>
    ///     Provides methods for monitoring the physical buttons installed in the device
    /// </summary>
    public sealed class HardwarePhysicalButtonDriver : UnixInputDriver, IPhysicalButtonDriver
    {
        /// <inheritdoc />
        public event EventHandler<PhysicalButton> Pressed;

        /// <inheritdoc />
        public event EventHandler<PhysicalButton> Released;

        /// <inheritdoc />
        public Dictionary<PhysicalButton, ButtonState> ButtonStates { get; }

        /// <summary>
        ///     Creates a new <see cref="HardwarePhysicalButtonDriver" />
        /// </summary>
        /// <param name="devicePath">The device event stream to poll for new events</param>
        public HardwarePhysicalButtonDriver(string devicePath) : base(devicePath)
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
                            throw new ArgumentOutOfRangeException(nameof(buttonState), buttonState,
                                buttonState.GetType().Name);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventType), eventType, eventType.GetType().Name);
            }
        }
    }
}