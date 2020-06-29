using System.Drawing;

namespace ReMarkable.NET.Unix.Driver.Digitizer
{
    public class StylusState
    {
        public StylusTool Tool { get; }
        public Point Position { get; }
        public int Pressure { get; }
        public int Distance { get; }
        public Point Tilt { get; }

        public StylusState(StylusTool tool, Point position, int pressure, int distance, Point tilt)
        {
            Tool = tool;
            Position = position;
            Pressure = pressure;
            Distance = distance;
            Tilt = tilt;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{nameof(Tool)}: {Tool}, {nameof(Position)}: {Position}, {nameof(Pressure)}: {Pressure}, {nameof(Distance)}: {Distance}, {nameof(Tilt)}: {Tilt}";
        }
    }
}