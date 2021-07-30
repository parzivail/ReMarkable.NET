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

        public override PointF Location
        {
            get => base.Location;
            set
            {
                base.Location = value;
                RecalculateChildPositions();
            }
        }

        private void RecalculateChildPositions()
        {
            float widthToAdd = 0;

            foreach (var control in this)
            {
                control.Location = new PointF(Location.X + widthToAdd, Location.Y);
                widthToAdd += control.Size.Width;
            }
        }
    }
}
