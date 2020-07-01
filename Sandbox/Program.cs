using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml.Xsl;
using Microsoft.VisualBasic;
using ReMarkable.NET.Unix.Driver;
using ReMarkable.NET.Unix.Driver.Battery;
using ReMarkable.NET.Unix.Driver.Display.EinkController;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Console = System.Console;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = PassiveDevices.Performance;

            Console.WriteLine($"Cores: {p.NumberOfCores}");
            Console.WriteLine($"Processors: {p.NumberOfProcessors}");
            Console.WriteLine($"RAM: {p.TotalMemory}");
            Console.WriteLine($"Swap: {p.TotalSwap}");
        }
    }
}
