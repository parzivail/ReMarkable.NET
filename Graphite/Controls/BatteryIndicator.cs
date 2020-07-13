using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Graphite.Symbols;
using Graphite.Typography;
using ReMarkable.NET.Unix.Driver;
using ReMarkable.NET.Unix.Driver.Power;

namespace Graphite.Controls
{
    public class BatteryIndicator : IconLabel<SegoeMdl2Glyphs>
    {
        private static readonly SegoeMdl2Glyphs[] BatteryIndicators =
        {
            SegoeMdl2Glyphs.VerticalBattery0, SegoeMdl2Glyphs.VerticalBattery1, SegoeMdl2Glyphs.VerticalBattery2, SegoeMdl2Glyphs.VerticalBattery3, SegoeMdl2Glyphs.VerticalBattery4,
            SegoeMdl2Glyphs.VerticalBattery5, SegoeMdl2Glyphs.VerticalBattery6, SegoeMdl2Glyphs.VerticalBattery7, SegoeMdl2Glyphs.VerticalBattery8, SegoeMdl2Glyphs.VerticalBattery9, 
            SegoeMdl2Glyphs.VerticalBattery10
        };

        private static readonly SegoeMdl2Glyphs[] ChargingBatteryIndicators =
        {
            SegoeMdl2Glyphs.VerticalBatteryCharging0, SegoeMdl2Glyphs.VerticalBatteryCharging1, SegoeMdl2Glyphs.VerticalBatteryCharging2, SegoeMdl2Glyphs.VerticalBatteryCharging3, SegoeMdl2Glyphs.VerticalBatteryCharging4, 
            SegoeMdl2Glyphs.VerticalBatteryCharging5, SegoeMdl2Glyphs.VerticalBatteryCharging6, SegoeMdl2Glyphs.VerticalBatteryCharging7, SegoeMdl2Glyphs.VerticalBatteryCharging8, SegoeMdl2Glyphs.VerticalBatteryCharging9, 
            SegoeMdl2Glyphs.VerticalBatteryCharging10
        };

        public BatteryIndicator() : base(SymbolAtlases.SegoeMdl2)
        {
            Poll();
        }

        public void Poll()
        {
            var batteryPercentage = PassiveDevices.Battery.GetPercentage();

            var indicators = PassiveDevices.Battery.GetStatus() != PowerSupplyStatus.Charging ? ChargingBatteryIndicators : BatteryIndicators;
            var batteryIndex = (int)Math.Round((indicators.Length - 1) * batteryPercentage);
            Icon = indicators[batteryIndex];
        }
    }
}
