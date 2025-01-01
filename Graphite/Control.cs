﻿using System;
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
        private RectangleF _bounds;
        private Font _font = Fonts.SegoeUiSemibold.CreateFont(32);
        private Color _foregroundColor = Color.Black;
        private Color _backgroundColor = Color.Transparent;
        private string _text;
        private RectAlign _textAlign = RectAlign.Left | RectAlign.Top;

        public event EventHandler<FingerState> FingerPress;
        public event EventHandler<FingerState> FingerRelease;
        public event EventHandler<FingerState> FingerMove;

        public event EventHandler<StylusPressEventArgs> StylusPress;
        public event EventHandler<StylusPressEventArgs> StylusRelease;
        public event EventHandler<StylusState> StylusMove;

        public Control Parent { get; protected internal set; }
        public Window Window { get; protected internal set; }

        public RectangleF Bounds
        {
            get => _bounds;
            set => RedrawWithChange(() => _bounds = value);
        }

        public virtual PointF Location
        {
            get => Bounds.Location;
            set => RedrawWithChange(() => _bounds.Location = value);
        }

        public SizeF Size
        {
            get => Bounds.Size;
            set => RedrawWithChange(() => _bounds.Size = value);
        }

        public int Layer { get; set; }

        public Color ForegroundColor
        {
            get => _foregroundColor;
            set => RedrawWithChange(() => _foregroundColor = value);
        }

        public Color BackgroundColor
        {
            get => _backgroundColor;
            set => RedrawWithChange(() => _backgroundColor = value);
        }

        public Font Font
        {
            get => _font;
            set => RedrawWithChange(() => _font = value);
        }

        public string Text
        {
            get => _text;
            set => RedrawWithChange(() => _text = value);
        }

        public RectAlign TextAlign
        {
            get => _textAlign;
            set => RedrawWithChange(() => _textAlign = value);
        }

        public abstract void Draw(Image<Rgb24> buffer);

        /// <summary>
        /// Redraws the control with the smallest rectangle that
        /// encompasses the visual change provided by the lambda
        /// including removing artifacts from the previous value
        /// </summary>
        /// <param name="action">The visual change to update</param>
        protected void RedrawWithChange(Action action)
        {
            var oldMinimumRect = GetMinimumRedrawRect();
            action?.Invoke();
            var newMinimumRect = GetMinimumRedrawRect();

            var redrawRect = oldMinimumRect.GetSmallestContaining(newMinimumRect);
            Window?.Refresh(redrawRect);
        }

        protected void DrawString(Image<Rgb24> buffer, string s, RectangleF layoutRectangle, RectAlign align = RectAlign.Center | RectAlign.Middle)
        {
            if (s == null)
                return;

            var strSize = MeasureString(s, layoutRectangle, align);

            buffer.Mutate(g => g.DrawText(s, Font, ForegroundColor, strSize.Location));
        }

        protected void DrawStringWithIcon(Image<Rgb24> buffer, char icon, float iconPadding, string s, RectangleF layoutRectangle)
        {
            //            var iconSize = icon == 0 ? RectangleF.Empty : TextMeasurer.Measure(icon.ToString()).ToRectangle();
            //            var strSize = TextMeasurer.Measure(s, rendererOptions).ToRectangle();
            //
            //            if (s != null && !s.Contains('\n'))
            //                strSize.Height = Font.Size;
            //
            //            iconSize.Width += iconPadding;
            //
            //            iconSize.Align(layoutRectangle, RectAlign.Middle);
            //            strSize.Align(layoutRectangle, RectAlign.Middle);
            //
            //            var combinedLeft = layoutRectangle.Left + (layoutRectangle.Width - (iconSize.Width + strSize.Width)) / 2;
            //
            //            iconSize.Location = new PointF(combinedLeft, iconSize.Top);
            //            strSize.Location = new PointF(combinedLeft + iconSize.Width, strSize.Top);
            //
            //            buffer.Mutate(g =>
            //            {
            //                g.DrawText(textGraphicsOptions, s, Font, ForegroundColor, strSize.GetContainingIntRect().Location);
            //
            //                if (icon != 0)
            //                    g.DrawText(textGraphicsOptions, icon.ToString(), Font, ForegroundColor, iconSize.GetContainingIntRect().Location);
            //            });
            // TODO
        }

        protected RectangleF MeasureString(string s, RectangleF layoutRectangle, RectAlign align)
        {
            if (s == null)
                return RectangleF.Empty;

            var strSize = TextMeasurer.MeasureAdvance(s, new TextOptions(Font)).ToRectangle();
            strSize.Align(layoutRectangle, align);
            return strSize.GetContainingIntRect();
        }

        protected void DrawBounds(IImageProcessingContext g)
        {
            g.Draw(ForegroundColor, 1, Bounds);
        }

        protected virtual RectangleF GetMinimumRedrawRect()
        {
            return Bounds;
        }

        public virtual bool BoundsContains(PointF point)
        {
            return Bounds.Contains(point);
        }

        internal virtual void OnPress(FingerState fingerState)
        {
            FingerPress?.Invoke(this, fingerState);
        }

        internal virtual void OnRelease(FingerState fingerState)
        {
            FingerRelease?.Invoke(this, fingerState);
        }

        internal virtual void OnMove(FingerState fingerState)
        {
            FingerMove?.Invoke(this, fingerState);
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
