using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Graphite.Typography;
using ReMarkable.NET.Unix.Driver;
using ReMarkable.NET.Unix.Driver.Battery;

namespace Graphite.Controls
{
    public class BatteryIndicator : Label
    {
        private static readonly char[] BatteryIndicators =
        {
            SegoeUiSymbols.MobBattery0, SegoeUiSymbols.MobBattery1, SegoeUiSymbols.MobBattery2, SegoeUiSymbols.MobBattery3, SegoeUiSymbols.MobBattery4, 
            SegoeUiSymbols.MobBattery5, SegoeUiSymbols.MobBattery6, SegoeUiSymbols.MobBattery7, SegoeUiSymbols.MobBattery8, SegoeUiSymbols.MobBattery9, 
            SegoeUiSymbols.MobBattery10
        };

        private static readonly char[] ChargingBatteryIndicators =
        {
            SegoeUiSymbols.MobBatteryCharging0, SegoeUiSymbols.MobBatteryCharging1, SegoeUiSymbols.MobBatteryCharging2, SegoeUiSymbols.MobBatteryCharging3, SegoeUiSymbols.MobBatteryCharging4, 
            SegoeUiSymbols.MobBatteryCharging5, SegoeUiSymbols.MobBatteryCharging6, SegoeUiSymbols.MobBatteryCharging7, SegoeUiSymbols.MobBatteryCharging8, SegoeUiSymbols.MobBatteryCharging9, 
            SegoeUiSymbols.MobBatteryCharging10
        };

        public BatteryIndicator()
        {
            Poll();
        }

        public void Poll()
        {
            var batteryPercentage = PassiveDevices.Battery.GetPercentage();

            var indicators = PassiveDevices.Battery.GetStatus() != PowerSupplyStatus.Charging ? ChargingBatteryIndicators : BatteryIndicators;
            var batteryIndex = (int)Math.Round((indicators.Length - 1) * batteryPercentage);
            Text = $"{indicators[batteryIndex]}";
        }
    }
}
