using System;
using System.Collections.Generic;
using System.Text;
using ReMarkable.NET.Util;

namespace ReMarkable.NET.Calibration.Builtin
{
    public class BuiltinStylusCalibrations
    {
        public static readonly TouchscreenCalibration ReMarkableMarker = StylusCalibrations.DataMarker.ToStruct<TouchscreenCalibration>();
        public static readonly TouchscreenCalibration ThinkpadX220Stylus = StylusCalibrations.DataX220.ToStruct<TouchscreenCalibration>();
        public static readonly TouchscreenCalibration FujitsuLifebookStylus = StylusCalibrations.DataLifebook.ToStruct<TouchscreenCalibration>();
    }
}
