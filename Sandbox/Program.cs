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
            // var img = new Image<Rgb24>(400, 200, Color.White);
            //
            // var fonts = new FontCollection();
            //
            // using var memStream = new MemoryStream(EmbeddedResources.Lato_Black);
            // var fontFamily = fonts.Install(memStream);
            // var font = fontFamily.CreateFont(52);
            //
            // img.Mutate(ctx => ctx.DrawText("Hello, World!", font, Color.Black, new PointF(10, 10)));
            
            var img = Image.Load<Rgb24>(EmbeddedResources.display_test);

            OutputDevices.Display.Framebuffer.Write(img, new Rectangle(0, 0, img.Width, img.Height), new Point(0, 0));

            OutputDevices.Display.Refresh(new Rectangle(0, 0, img.Width, img.Height), WaveformMode.Gc16Fast);
        }
    }
}
