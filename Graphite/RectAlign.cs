using System;

namespace Graphite
{
    [Flags]
    public enum RectAlign
    {
        /// <summary>
        /// Horizontal left
        /// </summary>
        Left = 1,

        /// <summary>
        /// Horizontal center
        /// </summary>
        Center = 2,

        /// <summary>
        /// Horizontal right
        /// </summary>
        Right = 4,

        /// <summary>
        /// Vertical top
        /// </summary>
        Top = 8,

        /// <summary>
        /// Vertical center
        /// </summary>
        Middle = 16,

        /// <summary>
        /// Vertical bottom
        /// </summary>
        Bottom = 32
    }
}