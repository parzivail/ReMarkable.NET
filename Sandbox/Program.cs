using System;
using System.Collections.Generic;
using System.Threading;
using Graphite;
using Graphite.Controls;
using ReMarkable.NET.Unix.Driver;
using ReMarkable.NET.Util;
using Sandbox.Pages;
using SixLabors.ImageSharp;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = Lumberjack.CreateLogger("Sandbox");

            var threadLock = new ManualResetEventSlim();

            var screen = OutputDevices.Display;

            var w = new Window(screen.VisibleWidth, screen.VisibleHeight);
            w.Update += WindowUpdate;

            InputDevices.Digitizer.Pressed += (sender, code) => w.ConsumePress(InputDevices.Digitizer.State, code);
            InputDevices.Digitizer.Released += (sender, code) => w.ConsumeRelease(InputDevices.Digitizer.State, code);
            InputDevices.Digitizer.StylusUpdate += (sender, state) => w.ConsumeMove(state);

            InputDevices.Touchscreen.Pressed += (sender, finger) => w.ConsumePress(finger);
            InputDevices.Touchscreen.Released += (sender, finger) => w.ConsumeRelease(finger);
            InputDevices.Touchscreen.Moved += (sender, finger) => w.ConsumeMove(finger);

            var mainPage = w.CreatePage<MainPage>();

            logger.Info("Showing main page");
            w.ShowPage(mainPage);

            threadLock.Wait();
        }

        private static void WindowUpdate(object sender, WindowUpdateEventArgs e)
        {
            OutputDevices.Display.Draw(e.Buffer, e.UpdatedArea, e.UpdatedArea.Location);
        }
    }
}
