using System;
using ReMarkable.NET.Unix.Driver.Display;
using ReMarkable.NET.Util;

namespace ReMarkable.NET.Unix.Driver
{
    public class OutputDevices
    {
        public static readonly IDisplayDriver Display;

        static OutputDevices()
        {
#if DEBUG
            // Load emulated input devices
            var deviceContainer = Type.GetType("RmEmulator.EmulatedDevices, RmEmulator");
            if (deviceContainer != null)
            {
                deviceContainer.ReadStaticField("Display", out Display);

                return;
            }
#endif
            // Load hardware output devices
            Display = new HardwareDisplayDriver("/dev/fb0");
        }
    }
}
