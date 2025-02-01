using System.IO;
using System.Linq;
using SixLabors.Fonts;

namespace Graphite.Typography
{
    public class Fonts
    {
        public static readonly FontFamily SegoeUi;
        public static readonly FontFamily SegoeUiLight;
        public static readonly FontFamily SegoeUiSemilight;
        public static readonly FontFamily SegoeUiSemibold;

        static Fonts()
        {
            var fonts = new FontCollection();

            InstallFont(fonts, EmbeddedFonts.segoeui); // Segoe UI
            InstallFont(fonts, EmbeddedFonts.segoeuib); // Segoe UI - Bold

            InstallFont(fonts, EmbeddedFonts.segoeuil); // Segoe UI Light
            InstallFont(fonts, EmbeddedFonts.segoeuisl); // Segoe UI Semilight
            InstallFont(fonts, EmbeddedFonts.seguisb); // Segoe UI Semibold

SegoeUi = fonts.Get("Segoe UI");
            SegoeUiLight = fonts.Get("Segoe UI Light");
            SegoeUiSemilight = fonts.Get("Segoe UI Semilight");
            SegoeUiSemibold = fonts.Get("Segoe UI Semibold");
        }

        private static void InstallFont(IFontCollection fonts, byte[] font)
        {
            using var ms = new MemoryStream(font);
            fonts.Add(ms);
        }
    }
}
