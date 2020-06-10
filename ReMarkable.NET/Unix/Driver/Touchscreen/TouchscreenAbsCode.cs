using System;
using System.Collections.Generic;
using System.Text;

namespace ReMarkable.NET.Unix.Driver.Touchscreen
{
    public enum TouchscreenAbsCode
    {
        Distance = 25,
        MultiTouchSlot = 47,
        MultiTouchTouchMajor = 48,
        MultiTouchTouchMinor = 49,
        MultiTouchOrientation = 52,
        MultiTouchPositionX = 53,
        MultiTouchPositionY = 54,
        MultiTouchToolType = 55,
        MultiTouchTrackingId = 57,
        MultiTouchPressure = 58
    }
}
