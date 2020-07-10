using System;
using System.Linq;
using SixLabors.ImageSharp;

namespace Graphite.Controls
{
    public class HorizontalStackPanel : Panel
    {
        /// <inheritdoc />
        public override void Add(Control item)
        {
            if (item != null)
            {
                var leftPos = this.Aggregate(0f, (i, control) => i + control.Bounds.Width);
                item.Location = new PointF(Bounds.X + leftPos, Bounds.Y);
            }

            base.Add(item);
        }
    }
}
