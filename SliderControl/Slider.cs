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
        //echelle

        //Events
        public event ResizedEventHandler Resized;
        public delegate void ResizedEventHandler(object sender, ResizedEventArgs e);

        public event CursorMovedEventHandler CursorMoved;
        public delegate void CursorMovedEventHandler(object sender, CursorMovedEventArgs e);

        //Properties
        public int CursorStart { get; set; }
        public int CursorEnd { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        public Slider(Form parentForm)
        {
            this.parentForm = parentForm;
            InitControl();
        }

        private Point previousLocation;
        private Control activeControl;
        bool resize = false;
        bool move = false;
        int tailleMin = 10;

        private void p_cursor_MouseDown(object sender, MouseEventArgs e)
        {
            activeControl = sender as Control;
            previousLocation = e.Location;

            if (IsMouseOnBorder(e))
            {
                Cursor.Current = Cursors.SizeWE;
                resize = true;
                move = false;
            }
            else
            {
                move = true;
                resize = false;
            }
        }

        private void p_cursor_MouseUp(object sender, MouseEventArgs e)
        {
            activeControl = null;
            Cursor.Current = Cursors.Arrow;

            if (resize)
            {
                //MAJ période
                //MAJ
                Resized(this, new ResizedEventArgs { NewSize = 0 });
            }
            else if (move)
            {
                //MAJ
                CursorMoved(this, new CursorMovedEventArgs { NewValue = 0 });
            }

            move = resize = false;
        }

        //détecter les bords
        //activer/désactiver curseur redimensionnement

        private void p_cursor_MouseMove(object sender, MouseEventArgs e)
        {
            if (resize)
            {
                UpdateLocation(sender, e);
            }
            else if (move)
            {
                UpdateLocation(sender, e);
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


        /// <summary>
        /// Déplace le curseur de x crans
        /// </summary>
        /// <param name="x">Négatif : déplace vers la gauche, positif vers la droite</param>
        public void Scroll(int x)
        {

        }

        private void UpdateLocation(object sender, MouseEventArgs e)
        {
            if (activeControl == null || activeControl != sender)
                return;

            var location = p_cursor.Location;
            int destX = e.Location.X - previousLocation.X;

            if (p_cursor.Location.X + destX <= p_background.Location.X + b_stepDown.Width) destX = 0;
            if (p_cursor.Location.X + p_cursor.Width + b_stepUp.Width + destX >= p_background.Location.X + p_background.Width) destX = 0;

            if (resize)
            {
                if (e.X > 3)
                {
                    p_cursor.Width += destX;
                }   

                if (p_cursor.Width < tailleMin) p_cursor.Width = tailleMin;
                previousLocation = e.Location;
            }
            else if (move)
            {
                location.Offset(destX, 0);
                activeControl.Location = location;
            }

        }

        /// <summary>
        /// Initialise tous les sous controls nécessaires
        /// </summary>
        private void InitControl()
        {
            this.p_background = new System.Windows.Forms.Panel();
            this.p_cursor = new System.Windows.Forms.Panel();
            this.b_stepDown = new System.Windows.Forms.Button();
            this.b_stepUp = new System.Windows.Forms.Button();
            this.p_background.SuspendLayout();
            // 
            // p_fond
            // 
            this.p_background.BackColor = System.Drawing.SystemColors.ControlLight;
            this.p_background.Controls.Add(this.b_stepUp);
            this.p_background.Controls.Add(this.b_stepDown);
            this.p_background.Location = new System.Drawing.Point(204, 145);
            this.p_background.Name = "p_fond";
            this.p_background.Size = new System.Drawing.Size(342, 22);
            this.p_background.TabIndex = 0;
            // 
            // p_curseur
            // 
            this.p_cursor.BackColor = System.Drawing.SystemColors.ControlDark;
            this.p_cursor.Location = new System.Drawing.Point(239, 145);
            this.p_cursor.Name = "p_curseur";
            this.p_cursor.Size = new System.Drawing.Size(122, 22);
            this.p_cursor.TabIndex = 1;
            this.p_cursor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.p_cursor_MouseDown);
            this.p_cursor.MouseUp += new System.Windows.Forms.MouseEventHandler(this.p_cursor_MouseUp);
            this.p_cursor.MouseMove += p_cursor_MouseMove;
            // 
            // b_moins
            // 
            this.b_stepDown.Location = new System.Drawing.Point(0, 0);
            this.b_stepDown.Name = "b_moins";
            this.b_stepDown.Size = new System.Drawing.Size(22, 22);
            this.b_stepDown.TabIndex = 2;
            this.b_stepDown.Text = "<";
            this.b_stepDown.UseVisualStyleBackColor = true;
            // 
            // b_plus
            // 
            this.b_stepUp.Location = new System.Drawing.Point(this.p_background.Width - 22, 0);
            this.b_stepUp.Name = "b_plus";
            this.b_stepUp.Size = new System.Drawing.Size(22, 22);
            this.b_stepUp.TabIndex = 3;
            this.b_stepUp.Text = ">";
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
