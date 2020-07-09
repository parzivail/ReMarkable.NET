using System;
using Graphite.Typography;
using ReMarkable.NET.Unix.Driver;

namespace Graphite.Controls
{
    public class WiFiIndicator : Label
    {
        private static readonly char[] WiFiIndicators =
        {
            SegoeUiSymbols.MobWifi1, SegoeUiSymbols.MobWifi2, SegoeUiSymbols.MobWifi3, SegoeUiSymbols.MobWifi4
        };

        private static readonly char[] WiFiWarningIndicators =
        {
            SegoeUiSymbols.MobWifiWarning1, SegoeUiSymbols.MobWifiWarning2, SegoeUiSymbols.MobWifiWarning3, SegoeUiSymbols.MobWifiWarning4
        };

        public WiFiIndicator()
        {
            Poll();
        }

        public void Poll()
        {
            var qualLink = PassiveDevices.Wireless.GetLinkQuality();
            var qualLevel = PassiveDevices.Wireless.GetSignalStrength();

            var wifiStatus = qualLevel == 0 ? WifiStatus.NotConnected : WifiStatus.Connected;
            var wifiStrength = qualLink;

            switch (wifiStatus)
            {
                case WifiStatus.Connected:
                case WifiStatus.NoInternet:
                {
                    var indicators = wifiStatus switch
                    {
                        WifiStatus.Connected => WiFiIndicators,
                        WifiStatus.NoInternet => WiFiWarningIndicators,
                        _ => throw new ArgumentOutOfRangeException(nameof(wifiStatus), wifiStatus, "Nested switch shouldn't need this branch")
                    };

                    var batteryIndex = (int) Math.Round((indicators.Length - 1) * wifiStrength);
                    Text = $"{indicators[batteryIndex]}";
                    break;
                }
                case WifiStatus.NotConnected:
                    Text = $"{SegoeUiSymbols.NetworkOffline}";
                    break;
                case WifiStatus.Disabled:
                    Text = $"{SegoeUiSymbols.MobAirplane}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}