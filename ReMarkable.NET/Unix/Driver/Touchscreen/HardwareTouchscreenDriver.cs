using System;
using ReMarkable.NET.Unix.Driver.Generic;
using SixLabors.ImageSharp;

namespace ReMarkable.NET.Unix.Driver.Touchscreen
{
    /// <summary>
    ///     Provides a set of methods for monitoring the device's physical touchscreen
    /// </summary>
    public sealed class HardwareTouchscreenDriver : UnixInputDriver, ITouchscreenDriver
    {
        /// <inheritdoc />
        public event EventHandler<FingerState> Moved;

        /// <inheritdoc />
        public event EventHandler<FingerState> Pressed;

        /// <inheritdoc />
        public event EventHandler<FingerState> Released;

        /// <summary>
        ///     The status of each finger arranged according to their slot index
        /// </summary>
        public FingerState[] Fingers;

        /// <summary>
        ///     Temporary finger position accumulated for event dispatch
        /// </summary>
        private Point _position = Point.Empty;

        /// <summary>
        ///     Temporary finger slot index accumulated for event dispatch
        /// </summary>
        private int _slot;

        /// <inheritdoc />
        public int Height { get; }

        /// <inheritdoc />
        public int MaxFingers { get; }

        /// <inheritdoc />
        public int Width { get; }

        /// <summary>
        /// Whether the width value should be left or right biased
        /// </summary>
        private readonly bool invertWidth;

        public HardwareTouchscreenDriver(string devicePath, int width, int height, int maxFingers, bool shouldInvertWidth = true) : base(devicePath)
        {
            Width = width;
            Height = height;
            MaxFingers = maxFingers;
            Fingers = new FingerState[maxFingers];
            invertWidth = shouldInvertWidth;
        }

        /// <inheritdoc />
        protected override void DataAvailable(object sender, DataAvailableEventArgs<EvEvent> e)
        {
            var data = e.Data;

            var eventType = (TouchscreenEventType)data.Type;

            switch (eventType)
            {
                case TouchscreenEventType.Syn:
                {
                    switch (Fingers[_slot].Status)
                    {
                        case FingerStatus.Down:
                            Pressed?.Invoke(this, Fingers[_slot]);
                            break;
                        case FingerStatus.Up:
                            Released?.Invoke(this, Fingers[_slot]);
                            break;
                        case FingerStatus.Moving:
                            Moved?.Invoke(this, Fingers[_slot]);
                            break;
                        case FingerStatus.Untracked:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("Status", Fingers[_slot].Status,
                                Fingers[_slot].Status.GetType().Name);
                    }

                    Fingers[_slot].Status = Fingers[_slot].Status switch
                    {
                        FingerStatus.Up => FingerStatus.Untracked,
                        FingerStatus.Down => FingerStatus.Moving,
                        _ => Fingers[_slot].Status
                    };

                    _slot = 0;
                    break;
                }
                case TouchscreenEventType.Absolute:
                    ProcessAbsoluteTouch((TouchscreenEventAbsCode)data.Code, data.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventType), eventType, eventType.GetType().Name);
            }
        }

        private void ProcessAbsoluteTouch(TouchscreenEventAbsCode code, int value)
        {
            switch (code)
            {
                case TouchscreenEventAbsCode.Distance:
                    break;
                case TouchscreenEventAbsCode.MultiTouchSlot:
                {
                    _slot = value;
                    if (_slot >= Fingers.Length)
                        _slot = Fingers.Length - 1; //sink
                }
                    break;
                case TouchscreenEventAbsCode.MultiTouchTouchMajor:
                    break;
                case TouchscreenEventAbsCode.MultiTouchTouchMinor:
                    break;
                case TouchscreenEventAbsCode.MultiTouchOrientation:
                    break;
                case TouchscreenEventAbsCode.MultiTouchPositionX:
                {
                    Fingers[_slot].PreviousDevicePosition.X = Fingers[_slot].DevicePosition.X;
                    Fingers[_slot].PreviousRawPosition.X = Fingers[_slot].RawPosition.X;

                    float pos = invertWidth ? Width - 1 - value : value;
                    _position.X = (int)(pos / Width * OutputDevices.Display.VisibleWidth);

                    Fingers[_slot].DevicePosition.X = _position.X;
                    Fingers[_slot].RawPosition.X = value;
                }
                    break;
                case TouchscreenEventAbsCode.MultiTouchPositionY:
                {
                    Fingers[_slot].PreviousDevicePosition.Y = Fingers[_slot].DevicePosition.Y;
                    Fingers[_slot].PreviousRawPosition.Y = Fingers[_slot].RawPosition.Y;

                    float pos = Height - 1 - value;
                    _position.Y = (int)(pos / Height * OutputDevices.Display.VisibleHeight);

                    Fingers[_slot].DevicePosition.Y = _position.Y;
                    Fingers[_slot].RawPosition.Y = value;
                }
                    break;
                case TouchscreenEventAbsCode.MultiTouchToolType:
                    break;
                case TouchscreenEventAbsCode.MultiTouchTrackingId:
                {
                    if (value == -1)
                    {
                        Fingers[_slot].Status = FingerStatus.Up;
                    }
                    else if (Fingers[_slot].Status == FingerStatus.Untracked)
                    {
                        Fingers[_slot].Id = value;
                        Fingers[_slot].Status = FingerStatus.Down;
                    }
                }
                    break;
                case TouchscreenEventAbsCode.MultiTouchPressure:
                {
                    Fingers[_slot].PreviousPressure = Fingers[_slot].Pressure;
                    Fingers[_slot].Pressure = value;
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(code), code, code.GetType().Name);
            }
        }
    }
}