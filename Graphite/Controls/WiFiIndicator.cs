using System;
using Graphite.Symbols;
using Graphite.Typography;
using ReMarkable.NET.Unix.Driver;

namespace Graphite.Controls
{
    public class WiFiIndicator : IconLabel<SegoeMdl2Glyphs>
    {
        private static readonly SegoeMdl2Glyphs[] WiFiIndicators =
        {
            SegoeMdl2Glyphs.Wifi1, SegoeMdl2Glyphs.Wifi2, SegoeMdl2Glyphs.Wifi3, SegoeMdl2Glyphs.Wifi4
        };

        private static readonly SegoeMdl2Glyphs[] WiFiWarningIndicators =
        {
            SegoeMdl2Glyphs.WifiWarning1, SegoeMdl2Glyphs.WifiWarning2, SegoeMdl2Glyphs.WifiWarning3, SegoeMdl2Glyphs.WifiWarning4
        };

        public WiFiIndicator() : base(SymbolAtlases.SegoeMdl2)
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
                    Icon = indicators[batteryIndex];
                    break;
                }
                case WifiStatus.NotConnected:
                    Icon = SegoeMdl2Glyphs.NetworkOffline;
                    break;
                case WifiStatus.Disabled:
                    Icon = SegoeMdl2Glyphs.Airplane;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}