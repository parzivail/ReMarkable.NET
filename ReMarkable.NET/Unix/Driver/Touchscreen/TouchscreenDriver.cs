using System;
using ReMarkable.NET.Util;

namespace ReMarkable.NET.Unix.Driver.Touchscreen
{
    public sealed class TouchscreenDriver : InputDriver
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

        internal TouchscreenDriver(string devicePath, int width, int height, int maxFingers) : base(devicePath)
        {
            Width = width;
            Height = height;
            MaxFingers = maxFingers;
            Fingers = new Finger[maxFingers + 1];
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
                        case FingerStatus.Untracked:
                            // ???
                            break;
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
                        if (_slot >= MaxFingers)
                            _slot = MaxFingers; //sink
                        Fingers[_slot].Status =
                            Fingers[_slot].Status != FingerStatus.Untracked
                            ? FingerStatus.Moving
                            : FingerStatus.Down;
                        break;
                    }
                case TouchscreenEventAbsCode.MultiTouchTouchMajor:
                    break;
                case TouchscreenEventAbsCode.MultiTouchTouchMinor:
                    break;
                case TouchscreenEventAbsCode.MultiTouchOrientation:
                    break;
                case TouchscreenEventAbsCode.MultiTouchPositionX:
                    {
                        float pos = Width - 1 - value;
                        _x = (int)(pos / Width * OutputDevices.Display.VisibleWidth);
                        Fingers[_slot].DeviceX = _x;
                        Fingers[_slot].RawX = value;
                        break;
                    }
                case TouchscreenEventAbsCode.MultiTouchPositionY:
                    {
                        float pos = Height - 1 - value;
                        _y = (int)(pos / Height * OutputDevices.Display.VisibleHeight);
                        Fingers[_slot].DeviceY = _y;
                        Fingers[_slot].RawY = value;
                        break;
                    }
                case TouchscreenEventAbsCode.MultiTouchToolType:
                    break;
                case TouchscreenEventAbsCode.MultiTouchTrackingId:
                    {
                        if (value == -1)
                        {
                            if (Fingers[_slot].Status != FingerStatus.Untracked)
                                Fingers[_slot].Status = FingerStatus.Up;
                        }
                        else
                        {
                            if (_slot == 0)
                                Fingers[_slot].Status = FingerStatus.Down;

                            if (Fingers[_slot].Status != FingerStatus.Untracked)
                                Fingers[_slot].Id = value;
                        }
                        break;
                    }
                case TouchscreenEventAbsCode.MultiTouchPressure:
                    if (Fingers[_slot].Status != FingerStatus.Untracked)
                        Fingers[_slot].Pressure = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(code), code, null);
            }
        }
    }
}
