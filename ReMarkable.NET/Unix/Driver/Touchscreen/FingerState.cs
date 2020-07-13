using SixLabors.ImageSharp;

namespace ReMarkable.NET.Unix.Driver.Touchscreen
{
    /// <summary>
    ///     Represents a finger's complete immediate state
    /// </summary>
    public struct FingerState
    {
        /// <summary>
        ///     The finger's current display position
        /// </summary>
        public Point DevicePosition;

        /// <summary>
        ///     The finger's current device position
        /// </summary>
        public Point RawPosition;

        /// <summary>
        ///     The finger's current pressure from 0-255
        /// </summary>
        public int Pressure { get; set; }

        /// <summary>
        ///     The finger's previous display position
        /// </summary>
        public Point PreviousDevicePosition;

        /// <summary>
        ///     The finger's previous device position
        /// </summary>
        public Point PreviousRawPosition;

        /// <summary>
        ///     The finger's previous pressure from 0-255
        /// </summary>
        public int PreviousPressure { get; set; }

        /// <summary>
        ///     The finger's current status
        /// </summary>
        public FingerStatus Status { get; set; }

        /// <summary>
        ///     The finger's tracking ID
        /// </summary>
        public int Id { get; set; }
    }
}