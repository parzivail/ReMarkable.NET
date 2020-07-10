using System;
using System.Collections.Generic;
using System.Text;
using Graphite.Symbols;
using Graphite.Util;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Graphite.Controls
{
    public class IconLabel<T> : Control where T : Enum
    {
        private readonly SymbolAtlas<T> _atlas;
        private T _icon;
        private int _iconSize;

        public T Icon
        {
            get => _icon;
            set => RedrawWithChange(() => _icon = value);
        }

        public int IconSize
        {
            get => _iconSize;
            set => RedrawWithChange(() => _iconSize = value);
        }

        public IconLabel(SymbolAtlas<T> atlas, int iconSize = 32)
        {
            _atlas = atlas;
            _iconSize = iconSize;

            Size = new SizeF(_iconSize, _iconSize);
        }

        protected override RectangleF GetMinimumRedrawRect()
        {
            var iconRect = new RectangleF(0, 0, IconSize, IconSize);
            iconRect.Align(Bounds, TextAlign);
            return iconRect;
        }

        public override void Draw(Image<Rgb24> buffer)
        {
            var rect = GetMinimumRedrawRect();
            var icon = _atlas.GetIcon(IconSize, Icon);
            buffer.Mutate(g => g.DrawImage(icon, rect.Location.ToInteger(), 1));
        }
    }
}
