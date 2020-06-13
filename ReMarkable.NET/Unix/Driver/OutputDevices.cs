using System;
using System.Collections.Generic;
using System.Text;
using ReMarkable.NET.Unix.Driver.Display;

namespace ReMarkable.NET.Unix.Driver
{
    public class OutputDevices
    {
        public static readonly DisplayDriver Display;

        static OutputDevices()
        {
            Display = new DisplayDriver("/dev/fb0");
        }
    }
}
