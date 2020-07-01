using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml.Xsl;
using Microsoft.VisualBasic;
using ReMarkable.NET.Unix.Driver;
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
            // var stamp = new Image<Rgb24>(240, 70, Color.White);
            //
            // var fonts = new FontCollection();
            //
            // using var memStream = new MemoryStream(EmbeddedResources.Lato_Black);
            // var fontFamily = fonts.Install(memStream);
            // var font = fontFamily.CreateFont(36);
            //
            // stamp.Mutate(ctx => ctx.DrawText("Hello, World!", font, Color.Black, new PointF(10, 10)));

            var img = Image.Load<Rgb24>(EmbeddedResources.display_test);

            var stamp = new Image<Rgb24>(150, 150);
            stamp.Mutate(g => g.Clear(Color.White).DrawLines(Color.Black, 3, new PointF(10, 10), new PointF(120, 10), new PointF(10, 120)));

            OutputDevices.Display.Framebuffer.Write(img, new Rectangle(0, 0, img.Width, img.Height), new Point(0, 0));
            OutputDevices.Display.Refresh(new Rectangle(0, 0, img.Width, img.Height), WaveformMode.Auto);

            InputDevices.Touchscreen.Pressed += (s, f) =>
            {
                var x = f.DeviceX - stamp.Width / 2;
                var y = f.DeviceY - stamp.Height / 2;
                OutputDevices.Display.Framebuffer.Write(stamp, new Rectangle(0, 0, stamp.Width, stamp.Height), new Point(x, y));
                OutputDevices.Display.Refresh(new Rectangle(x, y, stamp.Width, stamp.Height), WaveformMode.Auto);
            };

            while (true) { }
        }
    }
}
