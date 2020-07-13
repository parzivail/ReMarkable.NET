using ReMarkable.NET.Unix.Driver.Display;
using SixLabors.ImageSharp;

namespace ReMarkable.NET.Unix.Driver.Digitizer
{
    /// <summary>
    ///     Represents a stylus' complete immediate state
    /// </summary>
    public class StylusState
    {
        /// <summary>
        ///     The proximity of the stylus tool to the digitizer from 0 (touching screen) to 255 (furthest detectable distance,
        ///     around 5mm)
        /// </summary>
        public int Distance { get; }

        /// <summary>
        ///     The position of the stylus tool
        /// </summary>
        public Point Position { get; }

        /// <summary>
        ///     The pressure of the stylus tool
        /// </summary>
        public int Pressure { get; }

        /// <summary>
        ///     The tilt of the stylus tool from -9000 (horizontal one direction) to 9000 (horizontal the opposing direction)
        /// </summary>
        public Point Tilt { get; }

        /// <summary>
        ///     The tool currently employed by the stylus
        /// </summary>
        public StylusTool Tool { get; }

        /// <summary>
        ///     Creates a new <see cref="StylusState" />
        /// </summary>
        /// <param name="tool">The tool currently employed by the stylus</param>
        /// <param name="position">The position of the stylus tool</param>
        /// <param name="pressure">The pressure of the stylus tool</param>
        /// <param name="distance">The proximity of the stylus tool</param>
        /// <param name="tilt">The tilt of the stylus tool</param>
        public StylusState(StylusTool tool, Point position, int pressure, int distance, Point tilt)
        {
            Tool = tool;
            Position = position;
            Pressure = pressure;
            Distance = distance;
            Tilt = tilt;
        }


        /// <summary>
        ///     Maps the position of the tool from virtual digitizer coordinates to display coordinates
        /// </summary>
        /// <param name="digitizer">The digitizer to map coordinates from</param>
        /// <param name="display">The display to map coordinates to</param>
        /// <returns>The tool position as a <see cref="PointF" /> display coordinate</returns>
        public PointF GetDevicePosition(IDigitizerDriver digitizer, IDisplayDriver display)
        {
            return new PointF(Position.X / (float)digitizer.Width * display.VisibleWidth,
                Position.Y / (float)digitizer.Height * display.VisibleHeight);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return
                $"{nameof(Tool)}: {Tool}, {nameof(Position)}: {Position}, {nameof(Pressure)}: {Pressure}, {nameof(Distance)}: {Distance}, {nameof(Tilt)}: {Tilt}";
        }
    }
}