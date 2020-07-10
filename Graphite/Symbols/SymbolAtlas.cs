using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Graphite.Symbols
{
    public class SymbolAtlas<T> where T : Enum
    {
        private readonly Dictionary<int, Image<Rgba32>> _pages;

        public IEnumerable<int> AvailableSizes => _pages.Keys;

        public SymbolAtlas(params SymbolAtlasPage[] pages)
        {
            _pages = pages.ToDictionary(page => page.GlyphSize, page => page.Page);
        }

        public Image<Rgba32> GetIcon(int size, T glyph)
        {
            if (!_pages.TryGetValue(size, out var page))
                throw new ArgumentOutOfRangeException(nameof(size), size, $"No atlas defined for given size, expected one of [{string.Join(", ", AvailableSizes)}]");

            var atlasWidth = page.Width / size;
            var atlasHeight = page.Height / size;

            var glyphIdx = Convert.ToInt32(glyph);
            var glyphX = glyphIdx % atlasWidth * size;
            var glyphY = glyphIdx / atlasWidth * size;

            return page.Clone(g => g.Crop(new Rectangle(glyphX, glyphY, size, size)));
        }
    }

    public class SymbolAtlasPage
    {
        public Image<Rgba32> Page { get; }
        public int GlyphSize { get; set; }

        public SymbolAtlasPage(byte[] imagePage, int glyphSize)
        {
            Page = Image.Load(imagePage);
            GlyphSize = glyphSize;
        }
    }
}
