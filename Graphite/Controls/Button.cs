using Graphite.Typography;
using Graphite.Util;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Graphite.Controls
{
    public class Button : Control
    {
        private char _icon;
        private float _iconPadding;

        public char Icon
        {
            get => _icon;
            set => RedrawWithChange(() => _icon = value);
        }

        public float IconPadding
        {
            get => _iconPadding;
            set => RedrawWithChange(() => _iconPadding = value);
        }

        /// <inheritdoc />
        public override void Draw(Image<Rgb24> buffer)
        {
            buffer.Mutate(DrawBounds);
            DrawStringWithIcon(buffer, Icon, Text, Bounds);
        }

        protected void DrawStringWithIcon(Image<Rgb24> buffer, char icon, string s, RectangleF layoutRectangle)
        {
            var rendererOptions = new RendererOptions(Font) { FallbackFontFamilies = new[] { Fonts.SegoeMdl2 } };
            var textGraphicsOptions = new TextGraphicsOptions(new GraphicsOptions(), new TextOptions { FallbackFonts = { Fonts.SegoeMdl2 } });

            var iconSize = icon == 0 ? RectangleF.Empty : TextMeasurer.Measure(icon.ToString(), rendererOptions).ToRectangle();
            var strSize = TextMeasurer.Measure(s, rendererOptions).ToRectangle();

            if (s != null && !s.Contains('\n'))
                strSize.Height = Font.Size;

            iconSize.Width += IconPadding;

            iconSize.CenterInVertically(layoutRectangle);
            strSize.CenterInVertically(layoutRectangle);

            var combinedLeft = layoutRectangle.Left + (layoutRectangle.Width - (iconSize.Width + strSize.Width)) / 2;

            iconSize.Location = new PointF(combinedLeft, iconSize.Top);
            strSize.Location = new PointF(combinedLeft + iconSize.Width, strSize.Top);

            buffer.Mutate(g =>
            {
                g.DrawText(textGraphicsOptions, s, Font, ForegroundColor, strSize.Location);

                if (icon != 0)
                    g.DrawText(textGraphicsOptions, icon.ToString(), Font, ForegroundColor, iconSize.Location);
            });
        }

        private void DrawBounds(IImageProcessingContext g)
        {
            g.Draw(ForegroundColor, 4, Bounds.Inflated(-2, -2));
        }
    }
}
