using System.IO;
using Graphite.Resources;
using SixLabors.Fonts;

namespace Graphite.Typography
{
    public class Fonts
    {
        public static readonly FontFamily SegoeUi;
        public static readonly FontFamily SegoeUiLight;
        public static readonly FontFamily SegoeUiSemilight;
        public static readonly FontFamily SegoeUiSemibold;
        public static readonly FontFamily SegoeMdl2;

        static Fonts()
        {
            var fonts = new FontCollection();

            InstallFont(fonts, FontResources.segoeui); // Segoe UI
            InstallFont(fonts, FontResources.segoeuib); // Segoe UI - Bold

            InstallFont(fonts, FontResources.segoeuil); // Segoe UI Light
            InstallFont(fonts, FontResources.segoeuisl); // Segoe UI Semilight
            InstallFont(fonts, FontResources.seguisb); // Segoe UI Semibold
            InstallFont(fonts, FontResources.segoemdl2); // Material Icons

            SegoeUi = fonts.Find("Segoe UI");
            SegoeUiLight = fonts.Find("Segoe UI Light");
            SegoeUiSemilight = fonts.Find("Segoe UI Semilight");
            SegoeUiSemibold = fonts.Find("Segoe UI Semibold");
            SegoeMdl2 = fonts.Find("Segoe MDL2 Assets");
        }

        private static void InstallFont(IFontCollection fonts, byte[] font)
        {
            using var ms = new MemoryStream(font);
            fonts.Install(ms);
        }
    }
}
