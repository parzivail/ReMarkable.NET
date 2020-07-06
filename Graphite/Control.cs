using System;
using System.ComponentModel;
using Graphite.Typography;
using Graphite.Util;
using ReMarkable.NET.Unix.Driver.Digitizer;
using ReMarkable.NET.Unix.Driver.Touchscreen;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Drawing.Processing.Processors.Text;
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

        public Font Font { get; set; } = Fonts.SegoeUiSemibold.CreateFont(36);

        public string Text { get; set; }
        public char Icon { get; set; }
        public float IconPadding { get; set; }

        public abstract void Draw(Image<Rgb24> buffer);

        internal void DrawStringWithIcon(Image<Rgb24> buffer, string icon, string s, RectangleF layoutRectangle)
        {
            var rendererOptions = new RendererOptions(Font) { FallbackFontFamilies = new[] { Fonts.SegoeMdl2 } };
            var textGraphicsOptions = new TextGraphicsOptions(new GraphicsOptions(), new TextOptions { FallbackFonts = { Fonts.SegoeMdl2 } });

            var iconSize = TextMeasurer.Measure(icon, rendererOptions).ToRectangle();
            var strSize = TextMeasurer.Measure(s, rendererOptions).ToRectangle();

            if (!s.Contains('\n'))
                strSize.Height = Font.Size;

            iconSize.Width += IconPadding;

            iconSize.CenterInVertically(layoutRectangle);
            strSize.CenterInVertically(layoutRectangle);

            var combinedLeft = layoutRectangle.Left + (layoutRectangle.Width - (iconSize.Width + strSize.Width)) / 2;

            iconSize.Location = new PointF(combinedLeft, iconSize.Top);
            strSize.Location = new PointF(combinedLeft + iconSize.Width, strSize.Top);

            buffer.Mutate(g => g
                .DrawText(textGraphicsOptions, icon, Font, ForegroundColor, iconSize.Location)
                .DrawText(textGraphicsOptions, s, Font, ForegroundColor, strSize.Location)
            );
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
