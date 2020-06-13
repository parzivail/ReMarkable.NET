using System;
using System.Collections.Generic;
using System.Text;
using ReMarkable.NET.Unix.Driver.Button;
using ReMarkable.NET.Unix.Driver.Digitizer;

namespace ReMarkable.NET.Unix.Driver
{
    public class InputDevices
    {
        public static readonly PhysicalButtonDriver PhysicalButtons;
        public static readonly DigitizerDriver Digitizer;

        static InputDevices()
        {
            var deviceMap = DeviceUtils.GetInputDeviceEventHandlers();

            PhysicalButtons = new PhysicalButtonDriver(deviceMap["gpio-keys"]);
            Digitizer = new DigitizerDriver(deviceMap["Wacom I2C Digitizer"]);
        }
    }
}
