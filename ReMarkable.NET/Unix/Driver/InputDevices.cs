using System;
using ReMarkable.NET.Unix.Driver.Button;
using ReMarkable.NET.Unix.Driver.Digitizer;
using ReMarkable.NET.Unix.Driver.Touchscreen;
using ReMarkable.NET.Util;

namespace ReMarkable.NET.Unix.Driver
{
    /// <summary>
    ///     Provides a static listing of available input devices. When referenced on a tablet, will contain instances of
    ///     hardware drivers. When run in debug and with the RmEmulator assembly loaded, will contain instances of emulated
    ///     drivers.
    /// </summary>
    public static class InputDevices
    {
        /// <summary>
        ///     Holds an instance of a digitizer driver
        /// </summary>
        public static readonly IDigitizerDriver Digitizer;

        /// <summary>
        ///     Holds an instance of a physical button driver
        /// </summary>
        public static readonly IPhysicalButtonDriver PhysicalButtons;

        /// <summary>
        ///     Holds an instance of a touchscreen driver
        /// </summary>
        public static readonly ITouchscreenDriver Touchscreen;

        /// <summary>
        ///     Loads the appropriate input devices
        /// </summary>
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
            
            if (deviceMap.ContainsKey("30370000.snvs:snvs-powerkey"))
            {
                // rM2
                PhysicalButtons = new HardwarePhysicalButtonDriver(deviceMap["30370000.snvs:snvs-powerkey"]);
                Touchscreen = new HardwareTouchscreenDriver(deviceMap["cyttsp5_mt"], 767, 1023, 32);
                Digitizer = new HardwareDigitizerDriver(deviceMap["Wacom I2C Digitizer"], 20967, 15725);
            }
            else
            {
                PhysicalButtons = new HardwarePhysicalButtonDriver(deviceMap["gpio-keys"]);
                Touchscreen = new HardwareTouchscreenDriver(deviceMap["cyttsp5_mt"], 767, 1023, 32);
                Digitizer = new HardwareDigitizerDriver(deviceMap["Wacom I2C Digitizer"], 20967, 15725);
            }
        }
    }
}
