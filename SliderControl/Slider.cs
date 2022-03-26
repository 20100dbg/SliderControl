using System;
using System.Drawing;
using System.Windows.Forms;

namespace SliderControl
{
    public class Slider
    {
        //Events
        /// <summary>
        /// Occurs after the slider's span is resized
        /// </summary>
        public event SpanResizedEventHandler SpanResized;
        public delegate void SpanResizedEventHandler(object sender, SpanResizedEventArgs e);

        /// <summary>
        /// Occurs when the slider's span is being resized
        /// </summary>
        public event SpanResizingEventHandler SpanResizing;
        public delegate void SpanResizingEventHandler(object sender, SpanResizedEventArgs e);

        /// <summary>
        /// Occurs after the span is moved
        /// </summary>
        public event SpanMovedEventHandler SpanMoved;
        public delegate void SpanMovedEventHandler(object sender, SpanMovedEventArgs e);

        /// <summary>
        /// Occurs when the span is being moved
        /// </summary>
        public event SpanMovingEventHandler SpanMoving;
        public delegate void SpanMovingEventHandler(object sender, SpanMovedEventArgs e);

        public enum SpanStates { None, IsMoving, IsResizing };

        //Properties
        /// <summary>
        /// Gets the current value of the cursor
        /// </summary>
        public int CurrentValue { get; private set; }

        /// <summary>
        /// Gets the current span of the cursor
        /// </summary>
        public int CurrentSpan { get; private set; }

        /// <summary>
        /// Gets or sets the minimum value
        /// </summary>
        public int Minimum { get; set; }

        /// <summary>
        /// Gets or sets the maximum value
        /// </summary>
        public int Maximum { get; set; }

        /// <summary>
        /// Gets or sets the amount of change when clicking on buttons
        /// </summary>
        public int SmallChange { get; set; }

        /// <summary>
        /// Gets or sets the amount of change when clicking on the background
        /// </summary>
        public int LargeChange { get; set; }

        /// <summary>
        /// Gets the current SpanState
        /// </summary>
        public SpanStates SpanState { get; private set; }


        //Private attributes
        //Form parentForm;
        Control.ControlCollection parent;
        Point previousLocation;
        Control activeControl;
        int minCusorSize = 5;
        string version = "1.0.1";

        //Constructors
        /// <summary>
        /// Build a Slider control thats sets location, size, minimum and maximum values
        /// </summary>
        /// <param name="parentForm">The form the control must be added to</param>
        public Slider(Control.ControlCollection parent, Point location, Size size, int minimum, int maximum)
        {
            Init(parent, location, size, minimum, maximum, 10);
        }

        /// <summary>
        /// Build a Slider control that sets minimum and maximum values
        /// </summary>
        /// <param name="parentForm">The form the control must be added to</param>
        public Slider(Control.ControlCollection parent, int minimum, int maximum)
        {
            Point location = new Point(100, 100);
            Size size = new Size(200, 22);

            Init(parent, location, size, minimum, maximum, 10);
        }

        /// <summary>
        /// Build a Slider control with default parameters
        /// </summary>
        /// <param name="parentForm">The form the control must be added to</param>
        public Slider(Control.ControlCollection parent)
        {
            Point location = new Point(100, 100);
            Size size = new Size(200, 22);

            Init(parent, location, size, 0, 100, 10);
        }

        private void Init(Control.ControlCollection parent, Point location, Size size, int minimum, int maximum, int span)
        {
            this.parent = parent;
            InitControl(location, size);

            SpanState = SpanStates.None;
            Minimum = minimum;
            Maximum = maximum;
            SmallChange = 1;
            LargeChange = (int)(Maximum * 0.1);

            SetSpan(span);
            SetValue(0);
        }


        //public methods
        /// <summary>
        /// Change the slider control's location
        /// </summary>
        /// <param name="newLocation">Point object to new location</param>
        public void SetLocation(Point newLocation)
        {
            this.p_background.Location = newLocation;
            this.p_span.Location = new Point(p_background.Location.X, p_background.Location.Y + 1);
            this.b_stepDown.Location = new Point(this.p_background.Location.X - 21, this.p_background.Location.Y);
            this.b_stepUp.Location = new Point(this.p_background.Location.X + this.p_background.Width - 1, this.p_background.Location.Y);
        }

        /// <summary>
        /// Change the slider control's size
        /// </summary>
        /// <param name="newSize">New size to apply to the control</param>
        public void SetSize(Size newSize)
        {
            this.p_background.Size = newSize;
            this.p_span.Size = new Size(20, p_background.Height - 2);
            this.b_stepDown.Size = new Size(22, p_background.Height);
            this.b_stepUp.Location = new Point(this.p_background.Location.X + this.p_background.Width - 1, this.p_background.Location.Y);
            this.b_stepUp.Size = new Size(22, p_background.Height);
        }

        /// <summary>
        /// Change the control's span size. Must be between minimum and maximum
        /// </summary>
        /// <param name="newSpan">New span to use</param>
        public void SetSpan(int newSpan)
        {
            CurrentSpan = newSpan;
            if (CurrentSpan > Maximum) CurrentSpan = Maximum;
            else if (CurrentSpan < Minimum) CurrentSpan = Minimum;

            int width = SpanToWidth(newSpan);
            if (p_span.Width < minCusorSize) width = minCusorSize;
            p_span.Width = width;

            SpanResized?.Invoke(this, new SpanResizedEventArgs { NewSize = CurrentSpan });
        }

        /// <summary>
        /// Change the control's value. Must be between minimum and maximum
        /// </summary>
        /// <param name="newValue">Value to set</param>
        public void SetValue(int newValue)
        {
            CurrentValue = newValue;
            if (CurrentValue > Maximum - CurrentSpan) CurrentValue = Maximum - CurrentSpan;
            else if (CurrentValue < Minimum) CurrentValue = Minimum;

            int X = ValueToLocX(CurrentValue);
            p_span.Location = new Point(p_background.Location.X + X, p_span.Location.Y);

            SpanMoved?.Invoke(this, new SpanMovedEventArgs { NewValue = CurrentValue });
        }


        //private stuff
        private void p_background_MouseUp(object sender, MouseEventArgs e)
        {
            int wSpan = p_span.Width;
            int relX = p_span.Location.X - p_background.Location.X;

            if (e.X < relX) Scroll(LargeChange * -1);
            else if (e.X > relX + wSpan) Scroll(LargeChange);
        }

        private void p_span_MouseMove(object sender, MouseEventArgs e)
        {
            if (SpanState == SpanStates.IsMoving || SpanState == SpanStates.IsResizing)
            {
                if (activeControl == null || activeControl != sender) return;
                UpdateLocation(e.Location);
            }
            else
                Cursor.Current = (IsMouseOnBorder(e)) ? Cursors.SizeWE : Cursors.Arrow;
        }

        private void p_span_MouseDown(object sender, MouseEventArgs e)
        {
            activeControl = sender as Control;
            previousLocation = e.Location;

            bool flag = IsMouseOnBorder(e);
            Cursor.Current = (flag) ? Cursors.SizeWE : Cursors.Arrow;
            SpanState = (flag) ? SpanStates.IsResizing : SpanStates.IsMoving;
        }

        private void p_span_MouseUp(object sender, MouseEventArgs e)
        {
            activeControl = null;
            Cursor.Current = Cursors.Arrow;

            if (SpanState == SpanStates.IsResizing)
            {
                CurrentSpan = WidthToSpan(p_span.Width);
                SpanResized?.Invoke(this, new SpanResizedEventArgs { NewSize = CurrentSpan });
            }
            else if (SpanState == SpanStates.IsMoving)
            {
                int rel = p_span.Location.X - p_background.Location.X;
                CurrentValue = LocXToValue(rel);
                SpanMoved?.Invoke(this, new SpanMovedEventArgs { NewValue = CurrentValue });
            }
            SpanState = SpanStates.None;
        }

        private void b_stepUp_Click(object sender, EventArgs e)
        {
            Scroll(SmallChange);
        }

        private void b_stepDown_Click(object sender, EventArgs e)
        {
            Scroll(SmallChange * -1);
        }

        private bool IsMouseOnBorder(MouseEventArgs e)
        {
            return (e.X >= p_span.Width - 2);
        }

        private void UpdateLocation(Point loc)
        {
            int diff = loc.X - previousLocation.X;
            if (diff == 0) return;

            if (SpanState == SpanStates.IsResizing)
            {
                if (p_span.Width + diff <= p_background.Width - p_span.Location.X - p_background.Location.X)
                {
                    p_span.Width += diff;
                    if (p_span.Width < minCusorSize) p_span.Width = minCusorSize;
                    previousLocation = loc;

                    CurrentSpan = WidthToSpan(p_span.Width);
                    SpanResizing?.Invoke(this, new SpanResizedEventArgs { NewSize = CurrentSpan });
                }
            }
            else if (SpanState == SpanStates.IsMoving)
            {
                Point newLoc = new Point(p_span.Location.X + diff, p_span.Location.Y);

                if (newLoc.X < p_background.Location.X)
                    p_span.Location = new Point(p_background.Location.X, p_span.Location.Y);
                else if (newLoc.X + p_span.Width > p_background.Location.X + p_background.Width)
                    p_span.Location = new Point(p_background.Location.X + p_background.Width - p_span.Width, p_span.Location.Y);
                else
                {
                    int rel = p_span.Location.X - p_background.Location.X;

                    CurrentValue = LocXToValue(rel);
                    if (CurrentValue > Maximum) CurrentValue = Maximum;
                    else if (CurrentValue < Minimum) CurrentValue = Minimum;

                    p_span.Location = newLoc;
                    SpanMoving?.Invoke(this, new SpanMovedEventArgs { NewValue = CurrentValue });
                }
            }
        }

        private void Scroll(int val)
        {
            SetValue(CurrentValue + val);
        }

        private int LocXToValue(int X)
        {
            int bg = p_background.Width;
            if (bg == 0) bg = 1;
            return X * Maximum / bg;
        }

        private int ValueToLocX(int val)
        {
            int bg = p_background.Width;
            if (bg == 0) bg = 1;
            return val * bg / Maximum;
        }

        private int SpanToWidth(int width)
        {
            return width * p_background.Width / Maximum;
        }

        private int WidthToSpan(int span)
        {
            return span * Maximum / p_background.Width;
        }

        /// <summary>
        /// Initialize subcontrols
        /// </summary>
        private void InitControl(Point spawnPoint, Size backgroundSize)
        {
            this.p_background = new Panel();
            this.p_span = new Panel();
            this.b_stepDown = new Button();
            this.b_stepUp = new Button();
            this.p_background.SuspendLayout();
            // 
            // p_background
            // 
            this.p_background.BackColor = SystemColors.ControlLight;
            this.p_background.Location = spawnPoint;
            this.p_background.Name = "p_background";
            this.p_background.Size = backgroundSize;
            this.p_background.MouseUp += p_background_MouseUp;
            this.p_background.TabIndex = 0;
            // 
            // p_span
            // 
            this.p_span.BackColor = SystemColors.ControlDark;
            this.p_span.Location = new Point(p_background.Location.X, p_background.Location.Y + 1);
            this.p_span.Name = "p_span";
            this.p_span.Size = new Size(20, p_background.Height - 2);
            this.p_span.TabIndex = 1;
            this.p_span.MouseDown += new MouseEventHandler(this.p_span_MouseDown);
            this.p_span.MouseUp += new MouseEventHandler(this.p_span_MouseUp);
            this.p_span.MouseMove += p_span_MouseMove;
            // 
            // b_stepDown
            // 
            this.b_stepDown.Location = new Point(this.p_background.Location.X - 21, this.p_background.Location.Y);
            this.b_stepDown.Name = "b_stepDown";
            this.b_stepDown.Size = new Size(22, backgroundSize.Height);
            this.b_stepDown.TabIndex = 2;
            this.b_stepDown.Text = "<";
            this.b_stepDown.Click += b_stepDown_Click;
            this.b_stepDown.UseVisualStyleBackColor = true;
            // 
            // b_stepUp
            // 
            this.b_stepUp.Location = new Point(this.p_background.Location.X + this.p_background.Width - 1, this.p_background.Location.Y);
            this.b_stepUp.Name = "b_stepUp";
            this.b_stepUp.Size = new Size(22, backgroundSize.Height);
            this.b_stepUp.TabIndex = 3;
            this.b_stepUp.Text = ">";
            this.b_stepUp.Click += b_stepUp_Click;
            this.b_stepUp.UseVisualStyleBackColor = true;

            parent.Add(this.p_span);
            parent.Add(this.p_background);
            parent.Add(this.b_stepDown);
            parent.Add(this.b_stepUp);
            this.p_background.ResumeLayout(false);
        }

        private Panel p_background;
        private Panel p_span;
        private Button b_stepUp;
        private Button b_stepDown;
    }

    public class SpanResizedEventArgs : EventArgs
    {
        public int NewSize { get; set; }
    }

    public class SpanMovedEventArgs : EventArgs
    {
        public int NewValue { get; set; }
    }
}
