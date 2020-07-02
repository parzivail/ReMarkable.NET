using System;
using Graphite.Typography;
using Graphite.Util;
using ReMarkable.NET.Unix.Driver.Digitizer;
using ReMarkable.NET.Unix.Driver.Touchscreen;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Graphite
{
    public abstract class Control
    {
        public event EventHandler<Finger> FingerPress; 
        public event EventHandler<Finger> FingerRelease;
        public event EventHandler<Finger> FingerMove;

        public event EventHandler<StylusPressEventArgs> StylusPress; 
        public event EventHandler<StylusPressEventArgs> StylusRelease;
        public event EventHandler<StylusState> StylusMove; 

        public Control Parent { get; protected internal set; }
        public Window Window { get; protected internal set; }

        public RectangleF Bounds { get; set; }

        public int Layer { get; set; }

        public Color ForegroundColor { get; set; } = Color.Black;
        public Color BackgroundColor { get; set; } = Color.White;

        public Font Font { get; set; } = Fonts.SegoeUi.CreateFont(36);

        public string Text { get; set; }

        public abstract void Draw(Image<Rgb24> buffer);

        internal void DrawString(Image<Rgb24> buffer, string s, RectangleF layoutRectangle)
        {
            var size = TextMeasurer.Measure(s, new RendererOptions(Font)).ToRectangle();
            size.CenterIn(layoutRectangle);

            buffer.Mutate(g => g.DrawText(s, Font, ForegroundColor, size.Location));
        }

        public virtual bool BoundsContains(PointF point)
        {
            return Bounds.Contains(point);
        }

        internal virtual void OnPress(Finger finger)
        {
            FingerPress?.Invoke(this, finger);
        }

        internal virtual void OnRelease(Finger finger)
        {
            FingerRelease?.Invoke(this, finger);
        }

        internal virtual void OnMove(Finger finger)
        {
            FingerMove?.Invoke(this, finger);
        }

        internal virtual void OnPress(StylusState stylus, DigitizerEventKeyCode code)
        {
            StylusPress?.Invoke(this, new StylusPressEventArgs(stylus, code));
        }

        internal virtual void OnRelease(StylusState stylus, DigitizerEventKeyCode code)
        {
            StylusRelease?.Invoke(this, new StylusPressEventArgs(stylus, code));
        }

        internal virtual void OnMove(StylusState stylus)
        {
            StylusMove?.Invoke(this, stylus);
        }
    }
}
