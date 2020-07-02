using SixLabors.ImageSharp;

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

        public PointF GetDevicePosition()
        {
            var digitizer = InputDevices.Digitizer;
            var display = OutputDevices.Display;

            return new PointF(Position.X / (float)digitizer.Width * display.VisibleWidth, Position.Y / (float)digitizer.Height * display.VisibleHeight);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{nameof(Tool)}: {Tool}, {nameof(Position)}: {Position}, {nameof(Pressure)}: {Pressure}, {nameof(Distance)}: {Distance}, {nameof(Tilt)}: {Tilt}";
        }
    }
}