using System;
using ReMarkable.NET.Unix.Driver.Digitizer;

namespace Graphite
{
    public class StylusPressEventArgs : EventArgs
    {
        public StylusState Stylus { get; }
        public DigitizerEventKeyCode Code { get; }

        public StylusPressEventArgs(StylusState stylus, DigitizerEventKeyCode code)
        {
            Stylus = stylus;
            Code = code;
        }
    }
}