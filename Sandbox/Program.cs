using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.VisualBasic;
using ReMarkable.NET.Unix.Driver;
using ReMarkable.NET.Unix.Driver.Display.EinkController;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var img = new Image<Rgb24>(400, 200, Color.White);

            var fonts = new FontCollection();

            using var memStream = new MemoryStream(EmbeddedResources.Lato_Black);
            var fontFamily = fonts.Install(memStream);
            var font = fontFamily.CreateFont(52);
            
            img.Mutate(ctx => ctx.DrawText("Hello, World!", font, Color.Black, new PointF(10, 10)));
            
            OutputDevices.Display.Framebuffer.Write(img, new Rectangle(0, 0, 400, 200), new Point(150, 200));

            OutputDevices.Display.Refresh(new Rectangle(150, 200, 400, 200), WaveformMode.Gc16Fast);
        }
    }
}
