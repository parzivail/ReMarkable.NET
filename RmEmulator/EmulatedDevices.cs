using RmEmulator.Devices;

namespace RmEmulator
{
    public class EmulatedDevices
    {
        public static EmulatedDisplayDriver Display;

        public static EmulatedButtonDriver PhysicalButtons;
        public static EmulatedTouchscreenDriver Touchscreen;
        public static EmulatedDigitizerDriver Digitizer;

        public static EmulatedPerformanceMonitor Performance;
        public static EmulatedPowerSupplyMonitor Battery;
        public static EmulatedPowerSupplyMonitor UsbPower;

        public static void Init(EmulatorWindow emulatorWindow)
        {
            Display = new EmulatedDisplayDriver(emulatorWindow, 1404, 1872);

            PhysicalButtons = new EmulatedButtonDriver(emulatorWindow);
            Touchscreen = new EmulatedTouchscreenDriver(emulatorWindow, 767, 1023, 32);
            Digitizer = new EmulatedDigitizerDriver(emulatorWindow, 20967, 15725);

            Performance = new EmulatedPerformanceMonitor();
            Battery = new EmulatedPowerSupplyMonitor();
            UsbPower = new EmulatedPowerSupplyMonitor();
        }
    }
}