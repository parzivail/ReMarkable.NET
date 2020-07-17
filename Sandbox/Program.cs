using System;
using System.Collections.Generic;
using System.Threading;
using Graphite;
using Graphite.Controls;
using Graphite.Util;
using ReMarkable.NET.Unix.Driver;
using ReMarkable.NET.Unix.Driver.Button;
using ReMarkable.NET.Unix.Driver.Digitizer;
using ReMarkable.NET.Unix.Driver.Display.EinkController;
using ReMarkable.NET.Util;
using Sandbox.Pages;
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
            var logger = Lumberjack.CreateLogger("Sandbox");

            var threadLock = new ManualResetEventSlim();

            var screen = OutputDevices.Display;

            //            var w = new Window(screen.VisibleWidth, screen.VisibleHeight);
            //            w.Update += WindowUpdate;
            //
            //            InputDevices.Digitizer.Pressed += (sender, code) => w.ConsumePress(InputDevices.Digitizer.State, code);
            //            InputDevices.Digitizer.Released += (sender, code) => w.ConsumeRelease(InputDevices.Digitizer.State, code);
            //            InputDevices.Digitizer.StylusUpdate += (sender, state) => w.ConsumeMove(state);
            //
            //            InputDevices.Touchscreen.Pressed += (sender, finger) => w.ConsumePress(finger);
            //            InputDevices.Touchscreen.Released += (sender, finger) => w.ConsumeRelease(finger);
            //            InputDevices.Touchscreen.Moved += (sender, finger) => w.ConsumeMove(finger);
            //
            //            var mainPage = w.CreatePage<MainPage>();
            //
            //            logger.Info("Showing main page");
            //            w.ShowPage(mainPage);

            var buf = new Image<Rgb24>(screen.VisibleWidth, screen.VisibleHeight);
            buf.Mutate(context => context.SetGraphicsOptions(options => options.Antialias = false).Clear(Color.White));

            var white = buf.Clone();

            screen.Draw(buf, buf.Bounds(), Point.Empty, waveformMode: WaveformMode.Du);

            InputDevices.PhysicalButtons.Pressed += (sender, button) =>
            {
                switch (button)
                {
                    case PhysicalButton.Left:
                        screen.Draw(white, white.Bounds(), Point.Empty, waveformMode: WaveformMode.Du);
                        break;
                    case PhysicalButton.Right:
                        screen.Draw(buf, buf.Bounds(), Point.Empty, waveformMode: WaveformMode.Du);
                        break;
                }
            };

            var lastPos = PointF.Empty;
            InputDevices.Digitizer.StylusUpdate += (sender, state) =>
            {
                var thisPos = state.GetDevicePosition(InputDevices.Digitizer, screen);
                if (state.Pressure > 0 && (Math.Abs(lastPos.X - thisPos.X) >= 1 || Math.Abs(lastPos.Y - thisPos.Y) >= 1))
                {
                    var rect = Rectangle.Union(new Rectangle((int)lastPos.X, (int)lastPos.Y, 1, 1), new Rectangle((int)thisPos.X, (int)thisPos.Y, 1, 1));

                    buf.Mutate(context => context.DrawLines(Color.Black, 2, lastPos, thisPos));
                    screen.Draw(buf, rect, rect.Location, waveformMode: WaveformMode.Du, displayTemp: DisplayTemp.RemarkableDraw);

                    lastPos = thisPos;
                }

            };

            threadLock.Wait();
        }

        private static void WindowUpdate(object sender, WindowUpdateEventArgs e)
        {
            OutputDevices.Display.Draw(e.Buffer, e.UpdatedArea, e.UpdatedArea.Location);
        }
    }
}
