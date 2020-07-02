using System;
using ReMarkable.NET.Unix.Driver.Digitizer;
using SixLabors.ImageSharp;

namespace RmEmulator.Devices
{
    public class EmulatedDigitizerDriver : IDigitizerDriver
    {
        private readonly EmulatorWindow _window;

        public event EventHandler<StylusTool> ToolChanged;
        public event EventHandler<DigitizerEventKeyCode> Pressed;
        public event EventHandler<DigitizerEventKeyCode> Released;
        public event EventHandler<StylusState> StylusUpdate;

        public StylusState State { get; } = new StylusState(StylusTool.None, Point.Empty, 0, 0, Point.Empty);
        public int Width { get; }
        public int Height { get; }

        public EmulatedDigitizerDriver(EmulatorWindow window, int width, int height)
        {
            _window = window;
            Width = width;
            Height = height;
        }
    }
}