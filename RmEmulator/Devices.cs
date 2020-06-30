using ReMarkable.NET.Unix.Driver.Button;
using ReMarkable.NET.Unix.Driver.Digitizer;
using ReMarkable.NET.Unix.Driver.Display;
using ReMarkable.NET.Unix.Driver.Touchscreen;
using RmEmulator.Drivers;

namespace RmEmulator
{
    public class Devices
    {
        public static EmulatedDisplayDriver Display;
        public static EmulatedButtonDriver PhysicalButtons;
        public static EmulatedTouchscreenDriver Touchscreen;
        public static EmulatedDigitizerDriver Digitizer;

        public static void Init(EmulatorWindow emulatorWindow)
        {
            Display = new EmulatedDisplayDriver(emulatorWindow, 1404, 1872);

            PhysicalButtons = new EmulatedButtonDriver(emulatorWindow);
            Touchscreen = new EmulatedTouchscreenDriver(emulatorWindow, 767, 1023, 32);
            Digitizer = new EmulatedDigitizerDriver(emulatorWindow, 20967, 15725);
        }
    }
}