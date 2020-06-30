using System;
using System.Reflection;
using ReMarkable.NET.Unix.Driver.Button;
using ReMarkable.NET.Unix.Driver.Digitizer;
using ReMarkable.NET.Unix.Driver.Touchscreen;

namespace ReMarkable.NET.Unix.Driver
{
    public class InputDevices
    {
        public static readonly IPhysicalButtonDriver PhysicalButtons;
        public static readonly ITouchscreenDriver Touchscreen;
        public static readonly IDigitizerDriver Digitizer;

        static InputDevices()
        {
            if (Environment.GetEnvironmentVariable("RM_EMULATOR") != null)
            {
                // Load emulated input devices
                var deviceContainer = Type.GetType("RmEmulator.Devices, RmEmulator");
                if (deviceContainer == null)
                    throw new Exception("Could not load emulation container");

                var buttonsDev = deviceContainer.GetField("PhysicalButtons", BindingFlags.Public | BindingFlags.Static);
                if (buttonsDev == null)
                    throw new Exception("Could not load emulated button device");

                PhysicalButtons = (IPhysicalButtonDriver)buttonsDev.GetValue(buttonsDev);

                var touchscreenDev = deviceContainer.GetField("Touchscreen", BindingFlags.Public | BindingFlags.Static);
                if (touchscreenDev == null)
                    throw new Exception("Could not load emulated touchscreen device");

                Touchscreen = (ITouchscreenDriver)touchscreenDev.GetValue(touchscreenDev);

                var digitizerDev = deviceContainer.GetField("Digitizer", BindingFlags.Public | BindingFlags.Static);
                if (digitizerDev == null)
                    throw new Exception("Could not load emulated digitizer device");

                Digitizer = (IDigitizerDriver)digitizerDev.GetValue(digitizerDev);
            }
            else
            {
                // Load hardware input devices
                var deviceMap = DeviceUtils.GetInputDeviceEventHandlers();

                PhysicalButtons = new HardwarePhysicalButtonDriver(deviceMap["gpio-keys"]);
                Touchscreen = new HardwareTouchscreenDriver(deviceMap["cyttsp5_mt"], 767, 1023, 32);
                Digitizer = new HardwareDigitizerDriver(deviceMap["Wacom I2C Digitizer"], 20967, 15725);
            }
        }
    }
}
