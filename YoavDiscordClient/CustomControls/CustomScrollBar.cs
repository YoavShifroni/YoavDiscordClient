using System;
using System.Drawing;
using System.Windows.Forms;

namespace YoavDiscordClient
{
    public class CustomScrollBar : Control
    {
        private int _minimum = 0;
        private int _maximum = 100;
        private int _value = 0;
        private int _largeChange = 10;
        private bool _thumbDragging = false;
        private Rectangle _thumbRect;
        private int _thumbHeight = 20; // Default thumb size
        private int _dragOffsetY = 0;

        // Properties
        public int Minimum
        {
            get => _minimum;
            set
            {
                _minimum = value;
                Invalidate();
            }
        }

        public int Maximum
        {
            get => _maximum;
            set
            {
                _maximum = value;
                Invalidate();
            }
        }

        public int ScrollValue
        {
            get => _value;
            set
            {
                _value = Math.Max(_minimum, Math.Min(value, _maximum));
                Invalidate();
                ScrollValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int LargeChange
        {
            get => _largeChange;
            set
            {
                _largeChange = value;
                UpdateThumbSize();
            }
        }

        // Events
        public event EventHandler ScrollValueChanged;

        public CustomScrollBar()
        {
            this.Width = 15; // Default scrollbar width
            this.BackColor = Color.Gray;
            this.DoubleBuffered = true;

            // Handle mouse events
            this.MouseDown += CustomScrollBar_MouseDown;
            this.MouseMove += CustomScrollBar_MouseMove;
            this.MouseUp += CustomScrollBar_MouseUp;
            this.Resize += CustomScrollBar_Resize;
        }

        private void CustomScrollBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (_thumbRect.Contains(e.Location))
            {
                _thumbDragging = true;
                _dragOffsetY = e.Y - _thumbRect.Y;
            }
        }

        private void CustomScrollBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (_thumbDragging)
            {
                int trackHeight = this.Height - _thumbHeight;
                int newThumbY = Math.Max(0, Math.Min(e.Y - _dragOffsetY, trackHeight));

                // Update scroll value proportionally
                ScrollValue = (int)((float)newThumbY / trackHeight * (_maximum - _minimum));
            }
        }

        private void CustomScrollBar_MouseUp(object sender, MouseEventArgs e)
        {
            _thumbDragging = false;
        }

        private void CustomScrollBar_Resize(object sender, EventArgs e)
        {
            UpdateThumbSize();
        }

        private void UpdateThumbSize()
        {
            int trackHeight = this.Height;
            if (_maximum > 0 && _largeChange > 0)
            {
                // Calculate thumb height based on the visible portion of the content
                _thumbHeight = Math.Max(20, (int)((float)trackHeight * _largeChange / (_maximum - _minimum)));
                _thumbRect = new Rectangle(0, _thumbRect.Y, this.Width, _thumbHeight);
            }
            else
            {
                _thumbHeight = 20; // Default size
            }

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            // Draw scrollbar background
            using (Brush backgroundBrush = new SolidBrush(this.BackColor))
            {
                g.FillRectangle(backgroundBrush, this.ClientRectangle);
            }

            // Draw thumb
            using (Brush thumbBrush = new SolidBrush(Color.DarkSlateGray))
            {
                _thumbRect = new Rectangle(0, (int)((float)_value / (_maximum - _minimum) * (this.Height - _thumbHeight)), this.Width, _thumbHeight);
                g.FillRectangle(thumbBrush, _thumbRect);
            }
        }
    }
}
