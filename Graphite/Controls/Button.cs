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
            DrawStringWithIcon(buffer, Icon, IconPadding, Text, Bounds);
        }
    }
}
