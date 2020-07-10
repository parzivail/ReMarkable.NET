using System;
using System.Collections.Generic;
using System.Text;
using Graphite;
using Graphite.Controls;
using Graphite.Typography;
using ReMarkable.NET.Unix.Driver;
using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace Sandbox.Pages
{
    class MainPage : Panel
    {
        /// <inheritdoc />
        public override void LayoutControls()
        {
            var bottomToolbar = new HorizontalStackPanel
            {
                Size = new SizeF(Size.Width, 40)
            };
            bottomToolbar.Location = new PointF(0, Bounds.Height - bottomToolbar.Size.Height);
            
            bottomToolbar.Add(new WiFiIndicator
            {
                Size = new SizeF(50, 40),
                TextAlign = RectAlign.Top | RectAlign.Center
            });
            bottomToolbar.Add(new BatteryIndicator
            {
                Size = new SizeF(50, 40),
                TextAlign = RectAlign.Top | RectAlign.Center
            });
            bottomToolbar.Add(new Label
            {
                Text = $"{PassiveDevices.Battery.GetPercentage()*100:F0}%",
                Font = Fonts.SegoeUi.CreateFont(28),
                TextAlign = RectAlign.Top | RectAlign.Center,
                Size = new SizeF(80, 40)
            });

            Add(bottomToolbar);
        }
    }
}
