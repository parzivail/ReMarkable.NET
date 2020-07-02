using System.IO;
using Graphite.Resources;
using SixLabors.Fonts;

namespace Graphite.Typography
{
    public class Fonts
    {
        public static FontFamily SegoeUi;
        public static FontFamily SegoeUiLight;
        public static FontFamily SegoeUiSemilight;
        public static FontFamily SegoeMdl2Assets;

        static Fonts()
        {
            var fonts = new FontCollection();

            InstallFont(fonts, FontResources.segoeui); // Segoe UI
            InstallFont(fonts, FontResources.segoeuib); // Segoe UI - Bold

            InstallFont(fonts, FontResources.segoeuil); // Segoe UI Light
            InstallFont(fonts, FontResources.segoeuisl); // Segoe UI Semilight
            InstallFont(fonts, FontResources.segoemdl2); // Segoe MDL2 Assets

            SegoeUi = fonts.Find("Segoe UI");
            SegoeUiLight = fonts.Find("Segoe UI Light");
            SegoeUiSemilight = fonts.Find("Segoe UI Semilight");
            SegoeMdl2Assets = fonts.Find("Segoe MDL2 Assets");
        }

        private static void InstallFont(IFontCollection fonts, byte[] font)
        {
            using var ms = new MemoryStream(font);
            fonts.Install(ms);
        }
    }
}
