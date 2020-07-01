using System;
using OpenToolkit.Windowing.Common;
using OpenToolkit.Windowing.Common.Input;
using ReMarkable.NET.Unix.Driver.Touchscreen;

namespace RmEmulator.Drivers
{
    public class EmulatedTouchscreenDriver : ITouchscreenDriver
    {
        private readonly EmulatorWindow _window;

        public event EventHandler<Finger> Pressed;
        public event EventHandler<Finger> Released;
        public event EventHandler<Finger> Moved;

        public int Width { get; }
        public int Height { get; }
        public int MaxFingers { get; }

        private int _trackingId;

        public EmulatedTouchscreenDriver(EmulatorWindow window, int width, int height, int maxFingers)
        {
            _window = window;
            Width = width;
            Height = height;
            MaxFingers = maxFingers;
        }

        public void ConsumeMouseDown(MouseButtonEventArgs obj)
        {
            if (obj.Button != MouseButton.Left) return;

            _trackingId++;
            Pressed?.Invoke(this, CreateFinger(FingerStatus.Down));
        }

        public void ConsumeMouseUp(MouseButtonEventArgs obj)
        {
            if (obj.Button == MouseButton.Left)
                Released?.Invoke(this, CreateFinger(FingerStatus.Up));
        }

        public void ConsumeMouseMove(MouseMoveEventArgs obj)
        {
            if (_window.MouseState.IsButtonDown(MouseButton.Left))
                Moved?.Invoke(this, CreateFinger(FingerStatus.Moving));
        }

        private Finger CreateFinger(FingerStatus status)
        {
            var mouse = _window.MouseState;
            var mouseLast = _window.LastMouseState;

            var x = mouse.X / _window.Size.X;
            var y = mouse.Y / _window.Size.Y;

            var devX = x * Devices.Display.VisibleWidth;
            var devY = y * Devices.Display.VisibleHeight;

            var rawX = x * Width;
            var rawY = y * Height;

            var xLast = mouseLast.X / _window.Size.X;
            var yLast = mouseLast.Y / _window.Size.Y;

            var devXLast = xLast * Devices.Display.VisibleWidth;
            var devYLast = yLast * Devices.Display.VisibleHeight;

            var rawXLast = xLast * Width;
            var rawYLast = yLast * Height;
            
            var finger = new Finger
            {
                Status = status,
                Id = _trackingId,
                DeviceX = (int)devX,
                DeviceY = (int)devY,
                RawX = (int)rawX,
                RawY = (int)rawY,
                PreviousDeviceX = (int)devXLast,
                PreviousDeviceY = (int)devYLast,
                PreviousRawX = (int)rawXLast,
                PreviousRawY = (int)rawYLast,
                Pressure = 128,
                PreviousPressure = 128
            };
            return finger;
        }
    }
}