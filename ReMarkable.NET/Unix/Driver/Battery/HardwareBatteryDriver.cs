using System.IO;

namespace ReMarkable.NET.Unix.Driver.Battery
{
    class HardwareBatteryDriver : IPowerSupplyDriver
    {
        public string Device { get; }

        public HardwareBatteryDriver(string device)
        {
            Device = device;
        }

        private bool TryReadAttr(string attr, out string value)
        {
            value = null;

            var file = Path.Combine(Device, attr);
            if (!File.Exists(file))
                return false;

            value = File.ReadAllText(file);
            return true;
        }

        public float GetPercentage()
        {
            if (!TryReadAttr("capacity", out var capacity))
                return 0;

            return int.Parse(capacity) / 100f;
        }

        public float GetChargeFull()
        {
            if (!TryReadAttr("charge_full", out var value))
                return 0;

            return DeviceUtils.MicroToBaseUnit(int.Parse(value));
        }

        public float GetChargeFullDesign()
        {
            if (!TryReadAttr("charge_full_design", out var value))
                return 0;

            return DeviceUtils.MicroToBaseUnit(int.Parse(value));
        }

        public float GetChargeNow()
        {
            if (!TryReadAttr("charge_now", out var value))
                return 0;

            return DeviceUtils.MicroToBaseUnit(int.Parse(value));
        }

        public float GetVoltageNow()
        {
            if (!TryReadAttr("voltage_now", out var value))
                return 0;

            return DeviceUtils.MicroToBaseUnit(int.Parse(value));
        }

        public float GetCurrentNow()
        {
            if (!TryReadAttr("current_now", out var value))
                return 0;

            return DeviceUtils.MicroToBaseUnit(int.Parse(value));
        }

        public float GetTemperature()
        {
            if (!TryReadAttr("temp", out var value))
                return 0;

            // tenths of a degree C
            return int.Parse(value) * 10;
        }

        public bool IsOnline()
        {
            if (!TryReadAttr("online", out var value))
                return false;

            return int.Parse(value) == 1;
        }

        public bool IsPresent()
        {
            if (!TryReadAttr("present", out var value))
                return false;

            return int.Parse(value) == 1;
        }

        public PowerSupplyStatus GetStatus()
        {
            if (!TryReadAttr("status", out var value))
                return PowerSupplyStatus.Unknown;

            return value switch
            {
                "Charging" => PowerSupplyStatus.Charging,
                "Discharging" => PowerSupplyStatus.Discharging,
                "NotCharging" => PowerSupplyStatus.NotCharging,
                "FULL" => PowerSupplyStatus.Full,
                _ => PowerSupplyStatus.Unknown
            };
        }
    }
}
