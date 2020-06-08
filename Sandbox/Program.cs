using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using ReMarkable.NET.IO;
using ReMarkable.NET.Unix;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            const int fbWidth = 1408;
            const int fbHeight = 1920;

            const int width = 1404;
            const int height = 1872;

            /*
             * | Device                  | Description             |
             * | /dev/input/event0       | Stylus events           |
             * | /dev/input/event1       | Touch events            |
             * | /dev/input/event2       | Physical button events  |
             */

//            using var fs = File.OpenRead("/dev/input/event0");
//
//            var buf = new byte[16];
//
//            while (true)
//            {
//                var numRead = fs.Read(buf, 0, 16);
//                if (numRead != 16) Console.WriteLine("Read buffer underflow");
//
//                var e = FromBuffer<EvEvent>(buf);
//
//                Console.WriteLine($"[{e.TimeWholeSeconds}.{e.TimeFractionMicroseconds / 1000000f}] type={e.Type} code={e.Code}: {e.Value}");
//            }

            using var bw = new BinaryWriter(File.Open("/dev/fb0", FileMode.Open));

            for (int i = 0; i < fbWidth * fbHeight; i++)
            {
                bw.Write(Color(0xFF, 0xFF, 0xFF));
            }

            using var handle = UnsafeNativeMethods.Open("/dev/fb0", 0, UnixFileMode.ReadWrite);

            var data = new FbUpdateData
            {
                UpdateRegion = new FbRect
                {
                    X = 0,
                    Y = 0,
                    Width = width,
                    Height = height
                },
                WaveformMode = WaveformMode.Du,
                DisplayTemp = DisplayTemp.Papyrus,
                UpdateMode = UpdateMode.Full,
                UpdateMarker = (uint)DateTime.Now.Ticks,
                DitherMode = 0,
                QuantBit = 0,
                Flags = 0
            };

            if (UnsafeNativeMethods.Ioctl(handle, IoctlCommand.SendUpdate, ref data) == -1)
            {
                throw new UnixIOException();
            }
        }

        static T FromBuffer<T>(byte[] buffer) where T : struct
        {
            var temp = new T();
            var size = Marshal.SizeOf(temp);
            var ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(buffer, 0, ptr, size);

            var ret = (T)Marshal.PtrToStructure(ptr, temp.GetType());
            Marshal.FreeHGlobal(ptr);

            return ret;
        }

        private static short Color(byte r, byte g, byte b)
        {
            return (short)(((r & 0xF8) << 8) + ((g & 0xFC) << 3) + (b >> 3));
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct EvEvent
    {
        public uint TimeWholeSeconds;
        public uint TimeFractionMicroseconds;
        public ushort Type;
        public ushort Code;
        public int Value;
    }
}
