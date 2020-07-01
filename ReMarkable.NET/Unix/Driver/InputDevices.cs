using System;
using ReMarkable.NET.Unix.Driver.Button;
using ReMarkable.NET.Unix.Driver.Digitizer;
using ReMarkable.NET.Unix.Driver.Touchscreen;
using ReMarkable.NET.Util;

namespace ReMarkable.NET.Unix.Driver
{
    public class InputDevices
    {
        public static readonly IPhysicalButtonDriver PhysicalButtons;
        public static readonly ITouchscreenDriver Touchscreen;
        public static readonly IDigitizerDriver Digitizer;

        static InputDevices()
        {
#if DEBUG
            // Load emulated input devices
            var deviceContainer = Type.GetType("RmEmulator.EmulatedDevices, RmEmulator");
            if (deviceContainer != null)
            {
                deviceContainer.ReadStaticField("PhysicalButtons", out PhysicalButtons);
                deviceContainer.ReadStaticField("Touchscreen", out Touchscreen);
                deviceContainer.ReadStaticField("Digitizer", out Digitizer);

                return;
            }
#endif
            // Load hardware input devices
            var deviceMap = DeviceUtils.GetInputDeviceEventHandlers();

            PhysicalButtons = new HardwarePhysicalButtonDriver(deviceMap["gpio-keys"]);
            Touchscreen = new HardwareTouchscreenDriver(deviceMap["cyttsp5_mt"], 767, 1023, 32);
            Digitizer = new HardwareDigitizerDriver(deviceMap["Wacom I2C Digitizer"], 20967, 15725);
        }
    }
}
