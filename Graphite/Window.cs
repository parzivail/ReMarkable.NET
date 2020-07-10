using System;
using System.Collections.Generic;
using System.Linq;
using Graphite.Controls;
using Graphite.Util;
using ReMarkable.NET.Unix.Driver.Digitizer;
using ReMarkable.NET.Unix.Driver.Touchscreen;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Graphite
{
    public class Window
    {
        private readonly Stack<Page> _pages;

        public int Width { get; }
        public int Height { get; }

        public bool CanGoBack => _pages.Count > 0;

        public event EventHandler<WindowUpdateEventArgs> Update;

        public Image<Rgb24> Buffer { get; }

        public Window(int width, int height)
        {
            Width = width;
            Height = height;

            Buffer = new Image<Rgb24>(Width, Height);
            Buffer.Mutate(g => g.Clear(Color.White));

            _pages = new Stack<Page>();
        }

        public void ConsumePress(Finger finger)
        {
            var eligibleControls = GetControlsAtPoint(new PointF(finger.DeviceX, finger.DeviceY));
            foreach (var control in eligibleControls)
                control.OnPress(finger);
        }

        public void ConsumeRelease(Finger finger)
        {
            var eligibleControls = GetControlsAtPoint(new PointF(finger.DeviceX, finger.DeviceY));
            foreach (var control in eligibleControls)
                control.OnRelease(finger);
        }

        public void ConsumeMove(Finger finger)
        {
            var eligibleControls = GetControlsAtPoint(new PointF(finger.DeviceX, finger.DeviceY));
            foreach (var control in eligibleControls)
                control.OnMove(finger);
        }

        public void ConsumePress(StylusState stylus, DigitizerEventKeyCode code)
        {
            var eligibleControls = GetControlsAtPoint(stylus.GetDevicePosition());
            foreach (var control in eligibleControls)
                control.OnPress(stylus, code);
        }

        public void ConsumeRelease(StylusState stylus, DigitizerEventKeyCode code)
        {
            var eligibleControls = GetControlsAtPoint(stylus.GetDevicePosition());
            foreach (var control in eligibleControls)
                control.OnRelease(stylus, code);
        }

        public void ConsumeMove(StylusState stylus)
        {
            var eligibleControls = GetControlsAtPoint(stylus.GetDevicePosition());
            foreach (var control in eligibleControls)
                control.OnMove(stylus);
        }

        private List<Control> GetControlsAtPoint(PointF position)
        {
            var page = GetCurretPage();

            if (page == null)
                return new List<Control>();

            var children = GetChildControls(page.Content);

            if (children.All(control => !control.Bounds.Contains(position)))
                return new List<Control>();

            return children
                .Where(control => control.BoundsContains(position))
                .GroupBy(control => control.Layer)
                .OrderByDescending(controls => controls.Key)
                .First()
                .ToList();
        }

        private static List<Control> GetChildControls(Panel parent)
        {
            var controls = new List<Control>();

            foreach (var control in parent)
            {
                controls.Add(control);
                if (control is Panel p)
                    controls.AddRange(GetChildControls(p));
            }

            return controls;
        }

        public Page GetCurretPage()
        {
            return _pages.Count > 0 ? _pages.Peek() : null;
        }

        public Page CreatePage<T>() where T : Panel, new()
        {
            return new Page(this, Width, Height, new T());
        }

        public void ShowPage(Page page)
        {
            _pages.Push(page);
            Refresh(new Rectangle(0, 0, Width, Height));
        }

        public void ShowPreviousPage()
        {
            _pages.Pop();
            Refresh(new Rectangle(0, 0, Width, Height));
        }

        public void Refresh(RectangleF rectangle)
        {
            var currentPage = GetCurretPage();

            if (currentPage == null)
                return;

            currentPage.Draw(Buffer);
            Update?.Invoke(this, new WindowUpdateEventArgs(Buffer, rectangle.GetContainingIntRect()));
        }
    }

    public enum ToolType
    {
        Finger,
        Stylus
    }
}