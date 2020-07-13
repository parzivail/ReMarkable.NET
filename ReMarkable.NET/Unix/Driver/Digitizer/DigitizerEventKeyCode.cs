namespace ReMarkable.NET.Unix.Driver.Digitizer
{
    /// <summary>
    ///     Defines the possible event codes the digitizer can raise through the KEY event
    /// </summary>
    public enum DigitizerEventKeyCode
    {
        /// <summary>
        ///     Reports a transition to the stylus nib tool
        /// </summary>
        ToolPen = 320,

        /// <summary>
        ///     Reports a transition to the stylus "eraser" tool
        /// </summary>
        ToolRubber = 321,

        /// <summary>
        ///     Reports that the stylus has pressed the screen
        /// </summary>
        Touch = 330,

        /// <summary>
        ///     Reports a press of the first stylus button
        /// </summary>
        Stylus = 331,

        /// <summary>
        ///     Reports a press of the second stylus button
        /// </summary>
        Stylus2 = 332
    }
}