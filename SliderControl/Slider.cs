using System;
using System.Drawing;
using System.Windows.Forms;

namespace SliderControl
{
    internal class Slider
    {
        Form parentForm;

        //constructeurs
        //largeur du controle
        //valeur min/max

        //Events
        public event ResizedEventHandler Resized;
        public delegate void ResizedEventHandler(object sender, ResizedEventArgs e);

        public event CursorMovedEventHandler CursorMoved;
        public delegate void CursorMovedEventHandler(object sender, CursorMovedEventArgs e);


        //Properties
        public int CurrentValue { get; set; }
        public int CurrentSpan { get; set; }
        public int Minimum { get; set; }
        public int Maximum { get; set; }
        public int SmallChange { get; set; }
        public int LargeChange { get; set; }
        

        public Slider(Form parentForm)
        {
            this.parentForm = parentForm;

            Point spawnPoint = new Point(500, 500);
            Size backgroundSize = new Size(400, 22);

            InitControl(spawnPoint, backgroundSize);

            Minimum = 0;
            Maximum = 100;
            SmallChange = 1;
            LargeChange = 10;
            
            CurrentValue = 0;
            CurrentSpan = 10;
        }

        private Point previousLocation;
        private Control activeControl;
        bool isResizing = false;
        bool isMoving = false;
        int minCusorSize = 10;

        private int XtoValue(int width)
        {
            int bg = p_background.Width - p_cursor.Width;
            int x = width * Maximum / bg;
            return x;
        }

        private int ValueToX(int val)
        {
            int bg = p_background.Width - p_cursor.Width;
            int x = val * bg / Maximum;
            return x;
        }

        private int SizeToSpan(int width)
        {
            int bg = p_background.Width - p_cursor.Width;// + b_stepDown.Width + b_stepUp.Width;
            int x = width * Maximum / bg;
            return x;
        }

        private void p_cursor_MouseDown(object sender, MouseEventArgs e)
        {
            activeControl = sender as Control;
            previousLocation = e.Location;

            if (IsMouseOnBorder(e))
            {
                Cursor.Current = Cursors.SizeWE;
                isResizing = true;
                isMoving = false;
            }
            else
            {
                isMoving = true;
                isResizing = false;
            }
        }

        private void p_cursor_MouseUp(object sender, MouseEventArgs e)
        {
            activeControl = null;
            Cursor.Current = Cursors.Arrow;

            if (isResizing)
            {
                CurrentSpan = SizeToSpan(p_cursor.Width);
                Resized?.Invoke(this, new ResizedEventArgs { NewSize = CurrentSpan });
            }
            else if (isMoving)
            {
                int rel = p_cursor.Location.X - p_background.Location.X + b_stepDown.Width;
                CurrentValue = XtoValue(rel);
                CursorMoved?.Invoke(this, new CursorMovedEventArgs { NewValue = CurrentValue });
            }

            isMoving = isResizing = false;
        }

        private void p_cursor_MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizing || isMoving)
            {
                if (activeControl == null || activeControl != sender)
                    return;

                UpdateLocation(e.Location);
            }
            else
            {
                //détecter les bords
                if (IsMouseOnBorder(e))
                    Cursor.Current = Cursors.SizeWE;
                else
                    Cursor.Current = Cursors.Arrow;
            }
        }

        private bool IsMouseOnBorder(MouseEventArgs e)
        {
            return (e.X < 2 || e.X >= p_cursor.Width - 3);
        }

        private void UpdateLocation(Point loc)
        {
            var location = p_cursor.Location;
            int destX = loc.X - previousLocation.X;

            //if (p_cursor.Location.X + destX <= p_background.Location.X + b_stepDown.Width + 2) destX = 0;
            //if (p_cursor.Location.X + p_cursor.Width + b_stepUp.Width + 2 + destX >= p_background.Location.X + p_background.Width) destX = 0;


            int relX = p_cursor.Location.X - p_background.Location.X;

            if (relX + destX + 1 < b_stepDown.Width)
                p_cursor.Location = new Point(p_background.Location.X + b_stepDown.Width - 1, p_background.Location.Y + 1);
            else if (relX + destX - 1 > p_background.Width - p_cursor.Width - b_stepUp.Width)
                p_cursor.Location = new Point(p_background.Location.X + p_background.Width - p_cursor.Width - b_stepDown.Width + 1, p_background.Location.Y + 1);
            else
            {
                if (isResizing)
                {
                    if (loc.X > 3)
                    {
                        p_cursor.Width += destX;
                    }

                    if (p_cursor.Width < minCusorSize) p_cursor.Width = minCusorSize;
                    previousLocation = loc;
                }
                else if (isMoving)
                {
                    location.Offset(destX, 0);
                    activeControl.Location = location;
                }
            }

        }

        private void Scroll(int x)
        {
            CurrentValue += x;
            int w = ValueToX(CurrentValue);

            CursorMoved?.Invoke(this, new CursorMovedEventArgs { NewValue = CurrentValue });

            var loc = p_cursor.Location;
            loc.Offset(w, 0);
            p_cursor.Location = loc;
        }

        private void Resize(int newSpan)
        {
            CurrentSpan = newSpan;
            p_cursor.Width = SizeToSpan(newSpan);
        }

        private void P_background_MouseUp(object sender, MouseEventArgs e)
        {
            int wCursor = p_cursor.Width;
            int relX = p_cursor.Location.X - p_background.Location.X;

            if (e.X < relX) Scroll(LargeChange * -1);
            if (e.X > relX + wCursor) Scroll(LargeChange);
        }

        private void B_stepUp_Click(object sender, EventArgs e)
        {
            Scroll(SmallChange);
        }

        private void B_stepDown_Click(object sender, EventArgs e)
        {
            Scroll(SmallChange * -1);
        }

        /// <summary>
        /// Initialise tous les sous controls nécessaires
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
            this.p_background.Controls.Add(this.b_stepUp);
            this.p_background.Controls.Add(this.b_stepDown);
            this.p_background.Location = spawnPoint;
            this.p_background.Name = "p_background";
            this.p_background.Size = backgroundSize;
            this.p_background.MouseUp += P_background_MouseUp;
            this.p_background.TabIndex = 0;
            // 
            // p_cursor
            // 
            this.p_cursor.BackColor = System.Drawing.SystemColors.ControlDark;
            this.p_cursor.Location = new System.Drawing.Point(spawnPoint.X + 22, spawnPoint.Y + 1);
            this.p_cursor.Name = "p_cursor";
            this.p_cursor.Size = new System.Drawing.Size(122, 20);
            this.p_cursor.TabIndex = 1;
            this.p_cursor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.p_cursor_MouseDown);
            this.p_cursor.MouseUp += new System.Windows.Forms.MouseEventHandler(this.p_cursor_MouseUp);
            this.p_cursor.MouseMove += p_cursor_MouseMove;
            // 
            // b_stepDown
            // 
            this.b_stepDown.Location = new System.Drawing.Point(0, 0);
            this.b_stepDown.Name = "b_stepDown";
            this.b_stepDown.Size = new System.Drawing.Size(22, 22);
            this.b_stepDown.TabIndex = 2;
            this.b_stepDown.Text = "<";
            this.b_stepDown.Click += B_stepDown_Click;
            this.b_stepDown.UseVisualStyleBackColor = true;
            // 
            // b_stepUp
            // 
            this.b_stepUp.Location = new System.Drawing.Point(this.p_background.Width - 22, 0);
            this.b_stepUp.Name = "b_stepUp";
            this.b_stepUp.Size = new System.Drawing.Size(22, 22);
            this.b_stepUp.TabIndex = 3;
            this.b_stepUp.Text = ">";
            this.b_stepUp.Click += B_stepUp_Click;
            this.b_stepUp.UseVisualStyleBackColor = true;

            parentForm.Controls.Add(this.p_cursor);
            parentForm.Controls.Add(this.p_background);
            this.p_background.ResumeLayout(false);
        }


        private System.Windows.Forms.Panel p_background;
        private System.Windows.Forms.Panel p_cursor;
        private System.Windows.Forms.Button b_stepUp;
        private System.Windows.Forms.Button b_stepDown;
    }

    public class ResizedEventArgs : EventArgs
    {
        public int NewSize { get; set; }
    }

    public class CursorMovedEventArgs : EventArgs
    {
        public int NewValue { get; set; }
    }

}
