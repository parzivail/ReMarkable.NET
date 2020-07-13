namespace ReMarkable.NET.Unix.Driver.Display.EinkController
{
    public enum WaveformMode : uint
    {
        /// <summary>
        ///     Screen goes to white (clears)
        /// </summary>
        Init = 0x0,

        /// <summary>
        ///     Basically A2 (so partial refresh shouldnt be possible here)
        /// </summary>
        Glr16 = 0x4,

        /// <summary>
        ///     Official -- and enables Regal D Processing
        /// </summary>
        Gld16 = 0x5,

        /// <summary>
        ///     [Direct Update] Grey->white/grey->black  -- remarkable uses this for drawing
        /// </summary>
        Du = 0x1,

        /// <summary>
        ///     High fidelity (flashing)
        /// </summary>
        Gc16 = 0x2,

        /// <summary>
        ///     Medium fidelity  -- remarkable uses this for UI
        /// </summary>
        Gc16Fast = 0x3,

        /// <summary>
        ///     Medium fidelity from white transition
        /// </summary>
        Gl16Fast = 0x6,

        /// <summary>
        ///     Medium fidelity 4 level of gray direct update
        /// </summary>
        Du4 = 0x7,

        /// <summary>
        ///     Ghost compensation waveform
        /// </summary>
        Reagl = 0x8,

        /// <summary>
        ///     Ghost compensation waveform with dithering
        /// </summary>
        Reagld = 0x9,

        /// <summary>
        ///     2-bit from white transition
        /// </summary>
        Gl4 = 0xA,

        /// <summary>
        ///     High fidelity for black transition
        /// </summary>
        Gl16Inv = 0xB,

        /// <summary>
        ///     Official
        /// </summary>
        Auto = 257
    }
}