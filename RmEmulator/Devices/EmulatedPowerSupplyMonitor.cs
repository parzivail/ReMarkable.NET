using ReMarkable.NET.Unix.Driver.Power;

namespace RmEmulator.Devices
{
    public class EmulatedPowerSupplyMonitor : IPowerSupplyMonitor
    {
        public float GetPercentage()
        {
            return 0.75f;
        }

        public float GetChargeFull()
        {
            return 3.387f;
        }

        public float GetChargeFullDesign()
        {
            return 3;
        }

        public float GetChargeNow()
        {
            return 3.387f;
        }

        public float GetVoltageNow()
        {
            return 4.184f;
        }

        public float GetCurrentNow()
        {
            return 0.61f;
        }

        public float GetTemperature()
        {
            return 230;
        }

        public bool IsOnline()
        {
            return false;
        }

        public bool IsPresent()
        {
            return true;
        }

        public PowerSupplyStatus GetStatus()
        {
            return PowerSupplyStatus.Charging;
        }
    }
}