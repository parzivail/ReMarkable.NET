namespace Graphite.Symbols
{
    public class SymbolAtlases
    {
        public static readonly SymbolAtlas<SegoeMdl2Glyphs> SegoeMdl2;

        static SymbolAtlases()
        {
            SegoeMdl2 = new SymbolAtlas<SegoeMdl2Glyphs>(
                    new SymbolAtlasPage(EmbeddedSymbols.SegoeMdl2_16, 16),
                    new SymbolAtlasPage(EmbeddedSymbols.SegoeMdl2_32, 32),
                    new SymbolAtlasPage(EmbeddedSymbols.SegoeMdl2_64, 64)
                );
        }
    }
}