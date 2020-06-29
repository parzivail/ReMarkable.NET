namespace ReMarkable.NET.Unix.Driver.Touchscreen
{
    public struct Finger
    {
        public int DeviceX { get; set; }
        public int DeviceY { get; set; }
        public int RawX { get; set; }
        public int RawY { get; set; }
        public int Pressure { get; set; }
        public FingerStatus Status { get; set; }
        public int Id { get; set; }
    }

    public enum FingerStatus
    {
        Untracked,
        Down,
        Up,
        Moving
    }
}
