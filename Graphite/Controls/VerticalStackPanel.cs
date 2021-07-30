using System.Linq;
using SixLabors.ImageSharp;

namespace Graphite.Controls
{
    public class VerticalStackPanel : Panel
    {
        /// <inheritdoc />
        public override void Add(Control item)
        {
            base.Add(item);

            if (item == null)
                return;

            var topPos = this.Aggregate(0f, (i, control) => i + control.Bounds.Height);
            item.Location = new PointF(Bounds.X, Bounds.Y + topPos);
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
            float heightToAdd = 0;

            foreach (var control in this)
            {
                control.Location = new PointF(Location.X, Location.Y + heightToAdd);
                heightToAdd += control.Size.Height;
            }
        }
    }
}