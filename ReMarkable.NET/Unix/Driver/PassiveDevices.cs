using System;
using System.Collections.Generic;
using System.Text;
using ReMarkable.NET.Unix.Driver.Battery;
using ReMarkable.NET.Unix.Driver.Performance;
using ReMarkable.NET.Util;

namespace ReMarkable.NET.Unix.Driver
{
    public class PassiveDevices
    {
        public static readonly IPerformanceMonitor Performance;
        public static readonly IPowerSupplyMonitor Battery;
        public static readonly IPowerSupplyMonitor UsbPower;

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

                return;
            }
#endif

            Performance = new HardwarePeformanceMonitor();
            Battery = new HardwareBatteryMonitor("/sys/class/power_supply/bq27441-0");
            UsbPower = new HardwareBatteryMonitor("/sys/class/power_supply/imx_usb_charger");
        }
    }
}
