using System;
using ReMarkable.NET.Unix.Driver.Performance;
using ReMarkable.NET.Unix.Driver.Power;
using ReMarkable.NET.Unix.Driver.Wireless;
using ReMarkable.NET.Util;

namespace ReMarkable.NET.Unix.Driver
{
    /// <summary>
    ///     Provides a static listing of passive, monitorable devices. When referenced on a tablet, will contain instances of
    ///     hardware drivers. When run in debug and with the RmEmulator assembly loaded, will contain instances of emulated
    ///     drivers.
    /// </summary>
    public class PassiveDevices
    {
        /// <summary>
        ///     Holds an instance of a power supply monitor for the battery
        /// </summary>
        public static readonly IPowerSupplyMonitor Battery;

        /// <summary>
        ///     Holds an instance of a performance monitor
        /// </summary>
        public static readonly IPerformanceMonitor Performance;

        /// <summary>
        ///     Holds an instance of a power supply monitor for USB power
        /// </summary>
        public static readonly IPowerSupplyMonitor UsbPower;

        /// <summary>
        ///     Holds an instance of a wireless network monitor
        /// </summary>
        public static readonly IWirelessMonitor Wireless;

        /// <summary>
        ///     Loads the appropriate passive devices
        /// </summary>
        static PassiveDevices()
        {
#if DEBUG
            // Load emulated input devices
            var deviceContainer = Type.GetType("RmEmulator.EmulatedDevices, RmEmulator");
            if (deviceContainer != null)
            {
                deviceContainer.ReadStaticField("Performance", out Performance);
                deviceContainer.ReadStaticField("Battery", out Battery);
                deviceContainer.ReadStaticField("UsbPower", out UsbPower);
                deviceContainer.ReadStaticField("Wireless", out Wireless);

                return;
            }
#endif

            Performance = new HardwarePeformanceMonitor();
            Battery = new HardwarePowerSupplyMonitor("/sys/class/power_supply/bq27441-0");
            UsbPower = new HardwarePowerSupplyMonitor("/sys/class/power_supply/imx_usb_charger");
            Wireless = new HardwareWirelessMonitor();
        }
    }
}