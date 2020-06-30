using System;
using ReMarkable.NET.Util;

namespace ReMarkable.NET.Unix.Driver.Touchscreen
{
    public sealed class HardwareTouchscreenDriver : UnixInputDriver, ITouchscreenDriver
    {
        public event EventHandler<Finger> Pressed;
        public event EventHandler<Finger> Released;
        public event EventHandler<Finger> Moved;

        public int Width { get; }
        public int Height { get; }
        public int MaxFingers { get; }

        public Finger[] Fingers;

        private int _slot;
        private int _x;
        private int _y;

        internal HardwareTouchscreenDriver(string devicePath, int width, int height, int maxFingers) : base(devicePath)
        {
            Width = width;
            Height = height;
            MaxFingers = maxFingers;
            Fingers = new Finger[maxFingers];
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
                            default:
                                throw new ArgumentOutOfRangeException();
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
                    throw new ArgumentOutOfRangeException();
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
                        Fingers[_slot].PreviousDeviceX = Fingers[_slot].DeviceX;
                        Fingers[_slot].PreviousRawX = Fingers[_slot].RawX;

                        float pos = Width - 1 - value;
                        _x = (int)(pos / Width * OutputDevices.Display.VisibleWidth);

                        Fingers[_slot].DeviceX = _x;
                        Fingers[_slot].RawX = value;
                    }
                    break;
                case TouchscreenEventAbsCode.MultiTouchPositionY:
                    {
                        Fingers[_slot].PreviousDeviceY = Fingers[_slot].DeviceY;
                        Fingers[_slot].PreviousRawY = Fingers[_slot].RawY;

                        float pos = Height - 1 - value;
                        _y = (int)(pos / Height * OutputDevices.Display.VisibleHeight);

                        Fingers[_slot].DeviceY = _y;
                        Fingers[_slot].RawY = value;
                    }
                    break;
                case TouchscreenEventAbsCode.MultiTouchToolType:
                    break;
                case TouchscreenEventAbsCode.MultiTouchTrackingId:
                    {
                        if (value == -1)
                            Fingers[_slot].Status = FingerStatus.Up;
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
                    throw new ArgumentOutOfRangeException(nameof(code), code, null);
            }
        }
    }
}
