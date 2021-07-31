using System.Collections;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Graphite.Controls
{
    public class Panel : Control, IList<Control>
    {
        private readonly IList<Control> _controls;

        /// <inheritdoc />
        public int Count => _controls.Count;

        /// <inheritdoc />
        public bool IsReadOnly => _controls.IsReadOnly;

        public Panel()
        {
            _controls = new List<Control>();
        }

        /// <inheritdoc />
        public IEnumerator<Control> GetEnumerator()
        {
            return _controls.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_controls).GetEnumerator();
        }

        /// <inheritdoc />
        public virtual void Add(Control item)
        {
            if (item != null)
            {
                item.Parent = this;
                item.Window = Window;
            }
            _controls.Add(item);
        }

        /// <inheritdoc />
        public void Clear()
        {
            _controls.Clear();
        }

        /// <inheritdoc />
        public bool Contains(Control item)
        {
            return _controls.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(Control[] array, int arrayIndex)
        {
            foreach (var item in array)
                if (item != null)
                {
                    item.Parent = this;
                    item.Window = Window;
                }

            _controls.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public bool Remove(Control item)
        {
            return _controls.Remove(item);
        }

        /// <inheritdoc />
        public int IndexOf(Control item)
        {
            return _controls.IndexOf(item);
        }

        /// <inheritdoc />
        public void Insert(int index, Control item)
        {
            if (item != null)
            {
                item.Parent = this;
                item.Window = Window;
            }
            _controls.Insert(index, item);
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            _controls.RemoveAt(index);
        }

        /// <inheritdoc />
        public Control this[int index]
        {
            get => _controls[index];
            set
            {
                if (value != null)
                {
                    value.Parent = this;
                    value.Window = Window;
                }
                _controls[index] = value;
            }
        }

        /// <inheritdoc />
        public override void Draw(Image<Rgb24> buffer)
        {
            if (BackgroundColor != Color.Transparent)
            {
                buffer.Mutate(g => g.Fill(BackgroundColor, Bounds));
            }
            foreach (var control in _controls) control.Draw(buffer);
        }

        public virtual void LayoutControls()
        {
        }
    }
}