using System;
using OpenToolkit.Windowing.Common;
using OpenToolkit.Windowing.Common.Input;
using ReMarkable.NET.Unix.Driver.Touchscreen;
using SixLabors.ImageSharp;

namespace RmEmulator.Devices
{
    public class EmulatedTouchscreenDriver : ITouchscreenDriver
    {
        private readonly EmulatorWindow _window;

        public event EventHandler<FingerState> Pressed;
        public event EventHandler<FingerState> Released;
        public event EventHandler<FingerState> Moved;

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

        private FingerState CreateFinger(FingerStatus status)
        {
            var mouse = _window.MouseState;
            var mouseLast = _window.LastMouseState;

            var x = mouse.X / _window.Size.X;
            var y = mouse.Y / _window.Size.Y;

            var devX = x * EmulatedDevices.Display.VisibleWidth;
            var devY = y * EmulatedDevices.Display.VisibleHeight;

            var rawX = x * Width;
            var rawY = y * Height;

            var xLast = mouseLast.X / _window.Size.X;
            var yLast = mouseLast.Y / _window.Size.Y;

            var devXLast = xLast * EmulatedDevices.Display.VisibleWidth;
            var devYLast = yLast * EmulatedDevices.Display.VisibleHeight;

            var rawXLast = xLast * Width;
            var rawYLast = yLast * Height;
            
            var finger = new FingerState
            {
                Status = status,
                Id = _trackingId,
                DevicePosition = new Point((int)devX, (int)devY),
                RawPosition = new Point((int)rawX, (int)rawY),
                PreviousDevicePosition = new Point((int)devXLast, (int)devYLast),
                PreviousRawPosition = new Point((int)rawXLast, (int)rawYLast),
                Pressure = 128,
                PreviousPressure = 128
            };
            return finger;
        }
    }
}