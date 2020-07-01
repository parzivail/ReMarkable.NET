using System;
using ReMarkable.NET.Unix.Driver.Digitizer;

namespace RmEmulator.Devices
{
    public class EmulatedDigitizerDriver : IDigitizerDriver
    {
        private readonly EmulatorWindow _window;

        public event EventHandler<StylusTool> ToolChanged;
        public event EventHandler<DigitizerEventKeyCode> Pressed;
        public event EventHandler<DigitizerEventKeyCode> Released;
        public event EventHandler<StylusState> StylusUpdate;

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