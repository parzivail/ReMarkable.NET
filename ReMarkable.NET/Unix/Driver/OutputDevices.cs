using System;
using System.Reflection;
using ReMarkable.NET.Unix.Driver.Display;

namespace ReMarkable.NET.Unix.Driver
{
    public class OutputDevices
    {
        public static readonly IDisplayDriver Display;

        static OutputDevices()
        {
            if (Environment.GetEnvironmentVariable("RM_EMULATOR") != null)
            {
                // Load emulated output devices
                var deviceContainer = Type.GetType("RmEmulator.Devices, RmEmulator");
                if (deviceContainer == null)
                    throw new Exception("Could not load emulation container");

                var displayDev = deviceContainer.GetField("Display", BindingFlags.Public | BindingFlags.Static);
                if (displayDev == null)
                    throw new Exception("Could not load emulated display device");

                Display = (IDisplayDriver)displayDev.GetValue(displayDev);
            }
            else
            {
                // Load hardware output devices
                Display = new HardwareDisplayDriver("/dev/fb0");
            }
        }
    }
}
