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
    }
}