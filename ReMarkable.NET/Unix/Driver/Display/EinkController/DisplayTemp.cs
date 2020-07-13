namespace ReMarkable.NET.Unix.Driver.Display.EinkController
{
    public enum DisplayTemp : uint
    {
        Ambient = 0x1000,
        Papyrus = 0x1001,
        RemarkableDraw = 0x0018,
        Max = 0xFFFF
    }
}