using System;
using System.Drawing;
using System.IO;
using ReMarkable.NET.Unix;
using ReMarkable.NET.Unix.Driver;
using ReMarkable.NET.Unix.Driver.Button;
using ReMarkable.NET.Unix.Driver.Digitizer;
using ReMarkable.NET.Unix.Ioctl;
using ReMarkable.NET.Unix.Ioctl.Display;

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

            using var digitizerDriver = InputDevices.Digitizer;

            digitizerDriver.ToolChanged += (sender, tool) => Console.WriteLine($"Current tool: {tool}");
            digitizerDriver.Pressed += (sender, code) => Console.WriteLine($"Pressed: {code}");
            digitizerDriver.Released += (sender, code) => Console.WriteLine($"Released: {code}");
            digitizerDriver.StylusUpdate += (sender, state) => Console.WriteLine($"Update: {state}");

            while (true)
            {
                
            }

            using var bw = new BinaryWriter(File.Open("/dev/fb0", FileMode.Open));

            for (int i = 0; i < fbWidth * fbHeight; i++)
            {
                bw.Write(Color(0xFF, 0xFF, 0xFF));
            }

            // using var handle = UnsafeNativeMethods.Open("/dev/fb0", 0, UnixFileMode.ReadWrite);
            //
            // var data = new FbUpdateData
            // {
            //     UpdateRegion = new FbRect
            //     {
            //         X = 0,
            //         Y = 0,
            //         Width = width,
            //         Height = height
            //     },
            //     WaveformMode = WaveformMode.Du,
            //     DisplayTemp = DisplayTemp.Papyrus,
            //     UpdateMode = UpdateMode.Full,
            //     UpdateMarker = (uint)DateTime.Now.Ticks,
            //     DitherMode = 0,
            //     QuantBit = 0,
            //     Flags = 0
            // };
            //
            // if (UnsafeNativeMethods.Ioctl(handle, IoctlDisplayCommand.SendUpdate, ref data) == -1)
            // {
            //     throw new UnixIOException();
            // }
        }

        private static short Color(byte r, byte g, byte b)
        {
            return (short)(((r & 0xF8) << 8) + ((g & 0xFC) << 3) + (b >> 3));
        }
    }
}
