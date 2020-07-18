using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using Graphite;
using Graphite.Controls;
using Graphite.Util;
using ReMarkable.NET.Calibration;
using ReMarkable.NET.Calibration.Builtin;
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
        private static readonly Dictionary<PointF, PointF> CalibrationPoints = new Dictionary<PointF, PointF>();
        private static PointF[] _referencePoints;
        private static int _currentPointIdx;
        private static Image<Rgb24> _frame;

        static void Main(string[] args)
        {
            var logger = Lumberjack.CreateLogger("Sandbox");

            var threadLock = new ManualResetEventSlim();

            var screen = OutputDevices.Display;

            // var w = new Window(screen.VisibleWidth, screen.VisibleHeight);
            // w.Update += WindowUpdate;
            //
            // InputDevices.Digitizer.Pressed += (sender, code) => w.ConsumePress(InputDevices.Digitizer.State, code);
            // InputDevices.Digitizer.Released += (sender, code) => w.ConsumeRelease(InputDevices.Digitizer.State, code);
            // InputDevices.Digitizer.StylusUpdate += (sender, state) => w.ConsumeMove(state);
            //
            // InputDevices.Touchscreen.Pressed += (sender, finger) => w.ConsumePress(finger);
            // InputDevices.Touchscreen.Released += (sender, finger) => w.ConsumeRelease(finger);
            // InputDevices.Touchscreen.Moved += (sender, finger) => w.ConsumeMove(finger);
            //
            // var mainPage = w.CreatePage<MainPage>();
            //
            // logger.Info("Showing main page");
            // w.ShowPage(mainPage);

            var center = new PointF(screen.VisibleWidth / 2f, screen.VisibleHeight / 2f);
            var margin = 50;

            _referencePoints = new[]
            {
                new PointF(margin, margin), new PointF(center.X, margin), new PointF(screen.VisibleWidth - margin, margin),
                new PointF(margin, center.Y), new PointF(center.X, center.Y), new PointF(screen.VisibleWidth - margin, center.Y),
                new PointF(margin, screen.VisibleHeight - margin), new PointF(center.X, screen.VisibleHeight - margin), new PointF(screen.VisibleWidth - margin, screen.VisibleHeight - margin),
            };

            _frame = new Image<Rgb24>(screen.VisibleWidth, screen.VisibleHeight);
            _frame.Mutate(context => context
                .SetGraphicsOptions(options => options.Antialias = false)
                .Clear(Color.White)
            // .DrawPolygon(Color.Black, 1, Square(5, center, 45))
            // .DrawPolygon(Color.Black, 1, Square(1, center))
            );
            OutputDevices.Display.Draw(_frame, _frame.Bounds(), Point.Empty, waveformMode: WaveformMode.Auto);

            // InputDevices.Digitizer.Calibrator.Calibration = BuiltinStylusCalibrations.FujitsuLifebookStylus;

            StylusState prevState = null;
            var time = DateTime.Now;
            InputDevices.Digitizer.StylusUpdate += (sender, state) =>
            {
                if (prevState == null)
                {
                    prevState = state;
                    return;
                }

                if (((prevState.Pressure < 10 && state.Pressure >= 10) || _currentPointIdx == -1) && time <= DateTime.Now)
                {
                    StylusPress(state);
                    time = DateTime.Now + TimeSpan.FromMilliseconds(50);
                }

                prevState = state;
            };

            OutputDevices.Display.Draw(_frame.Clone(DrawCalibrationMarker), _frame.Bounds(), Point.Empty, waveformMode: WaveformMode.Auto);

            threadLock.Wait();
        }

        private static void StylusPress(StylusState state)
        {
            if (_currentPointIdx == -1)
            {
                Console.WriteLine($"{state.Tilt.X}\t{state.Tilt.Y}\t{state.DevicePosition.X}\t{state.DevicePosition.Y}");
                return;
            }

            var currentPoint = _referencePoints[_currentPointIdx];
            CalibrationPoints[currentPoint] = state.Position;

            _currentPointIdx++;

            if (_currentPointIdx == _referencePoints.Length)
            {
                _currentPointIdx = -1;
                OutputDevices.Display.Draw(_frame.Clone(context =>
                        context.DrawPolygon(Color.Black, 2, Square(10, _referencePoints[4], 45))
                        .DrawPolygon(Color.Black, 1, Square(1, _referencePoints[4]))
                    ), _frame.Bounds(), Point.Empty, waveformMode: WaveformMode.Auto);
            }
            else
                OutputDevices.Display.Draw(_frame.Clone(DrawCalibrationMarker), _frame.Bounds(), Point.Empty, waveformMode: WaveformMode.Auto);
        }

        private static void DrawCalibrationMarker(IImageProcessingContext context)
        {
            var currentPoint = _referencePoints[_currentPointIdx];
            context.DrawPolygon(Color.Black, 2, Square(10, currentPoint, 45))
                .DrawPolygon(Color.Black, 1, Square(1, currentPoint));
        }

        private static PointF[] Square(int size, PointF center, float angle = 0)
        {
            return new[]
            {
                Rotate(new PointF(center.X - size / 2f, center.Y - size / 2f), center, angle),
                Rotate(new PointF(center.X + size / 2f, center.Y - size / 2f), center, angle),
                Rotate(new PointF(center.X + size / 2f, center.Y + size / 2f), center, angle),
                Rotate(new PointF(center.X - size / 2f, center.Y + size / 2f), center, angle),
                Rotate(new PointF(center.X - size / 2f, center.Y - size / 2f), center, angle)
            };
        }

        private static PointF Rotate(PointF p, PointF c, float angle)
        {
            return new PointF((float)Math.Cos(angle / 180 * Math.PI) * (p.X - c.X) - (float)Math.Sin(angle / 180 * Math.PI) * (p.Y - c.Y) + c.X,
                (float)Math.Sin(angle / 180 * Math.PI) * (p.X - c.X) + (float)Math.Cos(angle / 180 * Math.PI) * (p.Y - c.Y) + c.Y);
        }

        private static void WindowUpdate(object sender, WindowUpdateEventArgs e)
        {
            OutputDevices.Display.Draw(e.Buffer, e.UpdatedArea, e.UpdatedArea.Location, displayTemp: DisplayTemp.RemarkableDraw);
        }
    }
}
