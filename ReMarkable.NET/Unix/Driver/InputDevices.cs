using System;
using System.Collections.Generic;
using System.Text;
using ReMarkable.NET.Unix.Driver.Button;
using ReMarkable.NET.Unix.Driver.Digitizer;
using ReMarkable.NET.Unix.Driver.Touchscreen;

namespace ReMarkable.NET.Unix.Driver
{
    public class InputDevices
    {
        public static readonly PhysicalButtonDriver PhysicalButtons;
        public static readonly TouchscreenDriver TouchscreenDriver;
        public static readonly DigitizerDriver Digitizer;

        static InputDevices()
        {
            var deviceMap = DeviceUtils.GetInputDeviceEventHandlers();

            PhysicalButtons = new PhysicalButtonDriver(deviceMap["gpio-keys"]);
            TouchscreenDriver = new TouchscreenDriver(deviceMap["cyttsp5_mt"], 767, 1023, 31);
            Digitizer = new DigitizerDriver(deviceMap["Wacom I2C Digitizer"]);
        }
    }
}
