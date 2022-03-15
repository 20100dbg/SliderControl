using System;
using System.Drawing;
using System.Windows.Forms;

namespace SliderControl
{
    public class Slider
    {
        //todo
        //vérifier la cohérence du nommage (cursor, span, value, etc)

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
        /// Occurs after the cursor is moved
        /// </summary>
        public event SpanMovedEventHandler SpanMoved;
        public delegate void SpanMovedEventHandler(object sender, SpanMovedEventArgs e);

        /// <summary>
        /// Occurs when the cursor is being moved
        /// </summary>
        public event SpanMovingEventHandler SpanMoving;
        public delegate void SpanMovingEventHandler(object sender, SpanMovedEventArgs e);


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


        //Private attributes
        Form parentForm;
        Point previousLocation;
        Control activeControl;
        bool isResizing = false;
        bool isMoving = false;
        int minCusorSize = 5;


        //Constructors
        /// <summary>
        /// Build a Slider control thats sets location, size, minimum and maximum values
        /// </summary>
        /// <param name="parentForm">The form the control must be added to</param>
        public Slider(Form parentForm, Point location, Size size, int minimum, int maximum)
        {
            Init(parentForm, location, size, minimum, maximum, 10);
        }

        /// <summary>
        /// Build a Slider control that sets minimum and maximum values
        /// </summary>
        /// <param name="parentForm">The form the control must be added to</param>
        public Slider(Form parentForm, int minimum, int maximum)
        {
            Point location = new Point(100, 100);
            Size size = new Size(200, 22);

            Init(parentForm, location, size, minimum, maximum, 10);
        }

        /// <summary>
        /// Build a Slider control with default parameters
        /// </summary>
        /// <param name="parentForm">The form the control must be added to</param>
        public Slider(Form parentForm)
        {
            Point location = new Point(100, 100);
            Size size = new Size(200, 22);

            Init(parentForm, location, size, 0, 100, 10);
        }

        private void Init(Form parentForm, Point location, Size size, int minimum, int maximum, int span)
        {
            this.parentForm = parentForm;
            InitControl(location, size);

            Minimum = minimum;
            Maximum = maximum;
            SmallChange = 1;
            LargeChange = 40;

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
            this.p_cursor.Location = new System.Drawing.Point(p_background.Location.X, p_background.Location.Y + 1);
            this.b_stepDown.Location = new System.Drawing.Point(this.p_background.Location.X - 21, this.p_background.Location.Y);
            this.b_stepUp.Location = new System.Drawing.Point(this.p_background.Location.X + this.p_background.Width - 1, this.p_background.Location.Y);
        }

        /// <summary>
        /// Change the slider control's size
        /// </summary>
        /// <param name="newSize">New size to apply to the control</param>
        public void SetSize(Size newSize)
        {
            this.p_background.Size = newSize;
            this.p_cursor.Size = new System.Drawing.Size(20, p_background.Height - 2);
            this.b_stepDown.Size = new System.Drawing.Size(22, p_background.Height);
            this.b_stepUp.Location = new System.Drawing.Point(this.p_background.Location.X + this.p_background.Width - 1, this.p_background.Location.Y);
            this.b_stepUp.Size = new System.Drawing.Size(22, p_background.Height);
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

            int width = WidthToSpan(newSpan);
            if (p_cursor.Width < minCusorSize) width = minCusorSize;
            p_cursor.Width = width;

            SpanResized?.Invoke(this, new SpanResizedEventArgs { NewSize = CurrentSpan });
        }

        /// <summary>
        /// Change the control's value. Must be between minimum and maximum
        /// </summary>
        /// <param name="newValue">Value to set</param>
        public void SetValue(int newValue)
        {
            CurrentValue = newValue;
            if (CurrentValue > Maximum) CurrentValue = Maximum;
            else if (CurrentValue < Minimum) CurrentValue = Minimum;

            int X = ValueToLocX(CurrentValue);
            p_cursor.Location = new Point(p_background.Location.X + X, p_cursor.Location.Y);
            
            SpanMoved?.Invoke(this, new SpanMovedEventArgs { NewValue = CurrentValue });
        }


        //private stuff
        private void p_background_MouseUp(object sender, MouseEventArgs e)
        {
            int wCursor = p_cursor.Width;
            int relX = p_cursor.Location.X - p_background.Location.X;

            if (e.X < relX) Scroll(LargeChange * -1);
            else if (e.X > relX + wCursor) Scroll(LargeChange);
        }

        private void p_cursor_MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizing || isMoving)
            {
                if (activeControl == null || activeControl != sender) return;
                UpdateLocation(e.Location);
            }
            else
                Cursor.Current = (IsMouseOnBorder(e)) ? Cursors.SizeWE : Cursors.Arrow;
        }

        private void p_cursor_MouseDown(object sender, MouseEventArgs e)
        {
            activeControl = sender as Control;
            previousLocation = e.Location;

            bool flag = IsMouseOnBorder(e);
            Cursor.Current = (flag) ? Cursors.SizeWE : Cursors.Arrow;
            isResizing = flag;
            isMoving = !flag;
        }

        private void p_cursor_MouseUp(object sender, MouseEventArgs e)
        {
            activeControl = null;
            Cursor.Current = Cursors.Arrow;

            if (isResizing)
            {
                CurrentSpan = SpanToWidth(p_cursor.Width);
                SpanResized?.Invoke(this, new SpanResizedEventArgs { NewSize = CurrentSpan });
            }
            else if (isMoving)
            {
                int rel = p_cursor.Location.X - p_background.Location.X;
                CurrentValue = LocXToValue(rel);
                SpanMoved?.Invoke(this, new SpanMovedEventArgs { NewValue = CurrentValue });
            }
            isMoving = isResizing = false;
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
            return (e.X >= p_cursor.Width - 2);
        }

        private void UpdateLocation(Point loc)
        {
            int diff = loc.X - previousLocation.X;

            if (isResizing)
            {
                if (p_cursor.Width + diff <= p_background.Width - (p_cursor.Location.X - p_background.Location.X))
                {
                    p_cursor.Width += diff;
                    if (p_cursor.Width < minCusorSize) p_cursor.Width = minCusorSize;
                    previousLocation = loc;

                    CurrentSpan = SpanToWidth(p_cursor.Width);
                    SpanResizing?.Invoke(this, new SpanResizedEventArgs { NewSize = CurrentSpan });
                }
            }
            else if (isMoving)
            {
                Point newLoc = new Point(p_cursor.Location.X + diff, p_cursor.Location.Y);

                if (newLoc.X < p_background.Location.X)
                    p_cursor.Location = new Point(p_background.Location.X, p_cursor.Location.Y);
                else if (newLoc.X + p_cursor.Width > p_background.Location.X + p_background.Width)
                    p_cursor.Location = new Point(p_background.Location.X + p_background.Width - p_cursor.Width + 1, p_cursor.Location.Y);
                else
                {
                    int rel = p_cursor.Location.X - p_background.Location.X;

                    CurrentValue = LocXToValue(rel);
                    if (CurrentValue > Maximum) CurrentValue = Maximum;
                    else if (CurrentValue < Minimum) CurrentValue = Minimum;

                    SpanMoving?.Invoke(this, new SpanMovedEventArgs { NewValue = CurrentValue });
                    p_cursor.Location = newLoc;
                }
            }
        }

        private void Scroll(int val)
        {
            SetValue(CurrentValue + val);
        }

        private int LocXToValue(int X)
        {
            int bg = p_background.Width - p_cursor.Width;
            if (bg == 0) bg = 1;
            return X * Maximum / bg;
        }

        private int ValueToLocX(int val)
        {
            int bg = p_background.Width - p_cursor.Width;
            if (bg == 0) bg = 1;
            return val * bg / Maximum;
        }

        private int WidthToSpan(int width)
        {
            return width * p_background.Width / Maximum;
        }

        private int SpanToWidth(int span)
        {
            return span * Maximum / p_background.Width;
        }

        /// <summary>
        /// Initialize subcontrols
        /// </summary>
        private void InitControl(Point spawnPoint, Size backgroundSize)
        {
            this.p_background = new System.Windows.Forms.Panel();
            this.p_cursor = new System.Windows.Forms.Panel();
            this.b_stepDown = new System.Windows.Forms.Button();
            this.b_stepUp = new System.Windows.Forms.Button();
            this.p_background.SuspendLayout();
            // 
            // p_background
            // 
            this.p_background.BackColor = System.Drawing.SystemColors.ControlLight;
            this.p_background.Location = spawnPoint;
            this.p_background.Name = "p_background";
            this.p_background.Size = backgroundSize;
            this.p_background.MouseUp += p_background_MouseUp;
            this.p_background.TabIndex = 0;
            // 
            // p_cursor
            // 
            this.p_cursor.BackColor = System.Drawing.SystemColors.ControlDark;
            this.p_cursor.Location = new System.Drawing.Point(p_background.Location.X, p_background.Location.Y + 1);
            this.p_cursor.Name = "p_cursor";
            this.p_cursor.Size = new System.Drawing.Size(20, p_background.Height - 2);
            this.p_cursor.TabIndex = 1;
            this.p_cursor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.p_cursor_MouseDown);
            this.p_cursor.MouseUp += new System.Windows.Forms.MouseEventHandler(this.p_cursor_MouseUp);
            this.p_cursor.MouseMove += p_cursor_MouseMove;
            // 
            // b_stepDown
            // 
            this.b_stepDown.Location = new System.Drawing.Point(this.p_background.Location.X - 21, this.p_background.Location.Y);
            this.b_stepDown.Name = "b_stepDown";
            this.b_stepDown.Size = new System.Drawing.Size(22, backgroundSize.Height);
            this.b_stepDown.TabIndex = 2;
            this.b_stepDown.Text = "<";
            this.b_stepDown.Click += b_stepDown_Click;
            this.b_stepDown.UseVisualStyleBackColor = true;
            // 
            // b_stepUp
            // 
            this.b_stepUp.Location = new System.Drawing.Point(this.p_background.Location.X + this.p_background.Width - 1, this.p_background.Location.Y);
            this.b_stepUp.Name = "b_stepUp";
            this.b_stepUp.Size = new System.Drawing.Size(22, backgroundSize.Height);
            this.b_stepUp.TabIndex = 3;
            this.b_stepUp.Text = ">";
            this.b_stepUp.Click += b_stepUp_Click;
            this.b_stepUp.UseVisualStyleBackColor = true;

            parentForm.Controls.Add(this.p_cursor);
            parentForm.Controls.Add(this.p_background);
            parentForm.Controls.Add(this.b_stepDown);
            parentForm.Controls.Add(this.b_stepUp);
            this.p_background.ResumeLayout(false);
        }

        private Panel p_background;
        private Panel p_cursor;
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
