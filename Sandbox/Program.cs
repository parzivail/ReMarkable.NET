using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml.Xsl;
using Graphite;
using Graphite.Controls;
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
            var screen = OutputDevices.Display;

            var w = new Window(screen.VisibleWidth, screen.VisibleHeight);
            w.Update += WindowUpdate;

            var mainPage = w.CreatePage();
            mainPage.Content.Add(new Button
            {
                Bounds = new Rectangle(50, 50, 200, 100)
            });

            w.Forward(mainPage);

            while (true) { }
        }

        private static void WindowUpdate(object sender, WindowUpdateEventArgs e)
        {
            OutputDevices.Display.Framebuffer.Write(e.Buffer, e.UpdatedArea, e.UpdatedArea.Location);
            OutputDevices.Display.Refresh(e.UpdatedArea, WaveformMode.Auto);
        }
    }
}
