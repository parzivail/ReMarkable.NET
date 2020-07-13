using System.IO;

namespace ReMarkable.NET.Unix.Driver.Power
{
    /// <summary>
    ///     Provides methods for reading power supply information from the installed hardware
    /// </summary>
    public sealed class HardwarePowerSupplyMonitor : IPowerSupplyMonitor
    {
        /// <summary>
        ///     The base device path to read information from
        /// </summary>
        public string Device { get; }

        /// <summary>
        ///     Creates a new <see cref="HardwarePowerSupplyMonitor" />
        /// </summary>
        /// <param name="device">The base device path to read information from</param>
        public HardwarePowerSupplyMonitor(string device)
        {
            Device = device;
        }

        /// <inheritdoc />
        public float GetChargeFull()
        {
            if (!TryReadAttr("charge_full", out var value))
                return 0;

            return DeviceUtils.MicroToBaseUnit(int.Parse(value));
        }

        /// <inheritdoc />
        public float GetChargeFullDesign()
        {
            if (!TryReadAttr("charge_full_design", out var value))
                return 0;

            return DeviceUtils.MicroToBaseUnit(int.Parse(value));
        }

        /// <inheritdoc />
        public float GetChargeNow()
        {
            if (!TryReadAttr("charge_now", out var value))
                return 0;

            return DeviceUtils.MicroToBaseUnit(int.Parse(value));
        }

        /// <inheritdoc />
        public float GetCurrentNow()
        {
            if (!TryReadAttr("current_now", out var value))
                return 0;

            return DeviceUtils.MicroToBaseUnit(int.Parse(value));
        }

        /// <inheritdoc />
        public float GetPercentage()
        {
            if (!TryReadAttr("capacity", out var capacity))
                return 0;

            return int.Parse(capacity) / 100f;
        }

        /// <inheritdoc />
        public PowerSupplyStatus GetStatus()
        {
            if (!TryReadAttr("status", out var value))
                return PowerSupplyStatus.Unknown;

            return value switch
            {
                "Charging\n" => PowerSupplyStatus.Charging,
                "Discharging\n" => PowerSupplyStatus.Discharging,
                "NotCharging\n" => PowerSupplyStatus.NotCharging,
                "FULL\n" => PowerSupplyStatus.Full,
                _ => PowerSupplyStatus.Unknown
            };
        }

        /// <inheritdoc />
        public float GetTemperature()
        {
            if (!TryReadAttr("temp", out var value))
                return 0;

            // tenths of a degree C
            return int.Parse(value) / 10f;
        }

        /// <inheritdoc />
        public float GetVoltageNow()
        {
            if (!TryReadAttr("voltage_now", out var value))
                return 0;

            return DeviceUtils.MicroToBaseUnit(int.Parse(value));
        }

        /// <inheritdoc />
        public bool IsOnline()
        {
            if (!TryReadAttr("online", out var value))
                return false;

            return int.Parse(value) == 1;
        }

        /// <inheritdoc />
        public bool IsPresent()
        {
            if (!TryReadAttr("present", out var value))
                return false;

            return int.Parse(value) == 1;
        }

        /// <summary>
        ///     Attempts to read an attribute file as a file located in the directory specified by <see cref="Device" />
        /// </summary>
        /// <param name="attr">The attribute file to read</param>
        /// <param name="value">The value as read from the file</param>
        /// <returns>True if the attribute exists and the read was successful, false otherwise</returns>
        private bool TryReadAttr(string attr, out string value)
        {
            value = null;

            var file = Path.Combine(Device, attr);
            if (!File.Exists(file))
                return false;

            value = File.ReadAllText(file);
            return true;
        }
    }
}