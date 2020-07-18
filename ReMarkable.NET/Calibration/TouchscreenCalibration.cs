using System.Runtime.InteropServices;

namespace ReMarkable.NET.Calibration
{
    [StructLayout(LayoutKind.Sequential, Size = 24)]
    public struct TouchscreenCalibration
    {
        public float Kx1;
        public float Kx2;
        public float Kx3;
        public float Ky1;
        public float Ky2;
        public float Ky3;
    }
}