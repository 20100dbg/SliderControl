using System;
using System.Drawing;
using System.Windows.Forms;

namespace SliderControl
{
    internal class Slider
    {
        Form parentForm;

        public Slider(Form parentForm)
        {
            this.parentForm = parentForm;
            InitControl();
        }

        private Point previousLocation;
        private Control activeControl;
        bool resize = false;
        bool move = false;

        private void p_curseur_MouseDown(object sender, MouseEventArgs e)
        {
            activeControl = sender as Control;
            previousLocation = e.Location;
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            activeControl = null;

            if (resize)
            {
                //MAJ période
                //MAJ
            }
            else if (move)
            {
                //MAJ
            }
        }


        private void p_curseur_SizeChanged(object sender, EventArgs e)
        {
            
        }

        //détecter les bords
        //activer/désactiver curseur redimensionnement

        private void P_curseur_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateLocation(sender, e);
            
            //else
            {
                //détecter les bords
                if ((e.X == p_curseur.Location.X || e.X == p_curseur.Location.X + p_curseur.Size.Width) &&
                    e.Y >= p_curseur.Location.Y && e.Y <= p_curseur.Location.Y + p_curseur.Size.Height)
                {
                    Cursor.Current = Cursors.SizeWE;
                }
                else
                    Cursor.Current = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Déplace le curseur de x crans
        /// </summary>
        /// <param name="x">Négatif : déplace vers la gauche, positif vers la droite</param>
        public void Scroll(int x)
        {

        }

        private void SetLocationX(int X)
        {
            var location = p_curseur.Location;
            location.Offset(X, 0);
            p_curseur.Location = location;
        }

        private void UpdateLocation(object sender, MouseEventArgs e)
        {
            if (activeControl == null || activeControl != sender)
                return;

            var location = p_curseur.Location;
            int destX = e.Location.X - previousLocation.X;
            
            //if (e.Location.X < p_fond.Location.X) destX = 0;
            //if (destX > p_fond.Size.Width) destX = p_fond.Size.Width;

            location.Offset(destX, 0);
            activeControl.Location = location;
        }

        /// <summary>
        /// Initialise tous les sous controls nécessaires
        /// </summary>
        private void InitControl()
        {
            this.p_fond = new System.Windows.Forms.Panel();
            this.p_curseur = new System.Windows.Forms.Panel();
            this.b_moins = new System.Windows.Forms.Button();
            this.b_plus = new System.Windows.Forms.Button();
            this.p_fond.SuspendLayout();
            this.parentForm.MouseUp += OnMouseUp;
            // 
            // p_fond
            // 
            this.p_fond.BackColor = System.Drawing.SystemColors.ControlLight;
            this.p_fond.Controls.Add(this.b_plus);
            this.p_fond.Controls.Add(this.b_moins);
            this.p_fond.Location = new System.Drawing.Point(204, 145);
            this.p_fond.Name = "p_fond";
            this.p_fond.Size = new System.Drawing.Size(342, 22);
            this.p_fond.TabIndex = 0;
            // 
            // p_curseur
            // 
            this.p_curseur.BackColor = System.Drawing.SystemColors.ControlDark;
            this.p_curseur.Location = new System.Drawing.Point(239, 145);
            this.p_curseur.Name = "p_curseur";
            this.p_curseur.Size = new System.Drawing.Size(122, 22);
            this.p_curseur.TabIndex = 1;
            this.p_curseur.SizeChanged += new System.EventHandler(this.p_curseur_SizeChanged);
            this.p_curseur.MouseDown += new System.Windows.Forms.MouseEventHandler(this.p_curseur_MouseDown);
            this.p_curseur.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            this.p_curseur.MouseMove += P_curseur_MouseMove;
            // 
            // b_moins
            // 
            this.b_moins.Location = new System.Drawing.Point(0, 0);
            this.b_moins.Name = "b_moins";
            this.b_moins.Size = new System.Drawing.Size(22, 22);
            this.b_moins.TabIndex = 2;
            this.b_moins.Text = "<";
            this.b_moins.UseVisualStyleBackColor = true;
            // 
            // b_plus
            // 
            this.b_plus.Location = new System.Drawing.Point(this.p_fond.Width - 22, 0);
            this.b_plus.Name = "b_plus";
            this.b_plus.Size = new System.Drawing.Size(22, 22);
            this.b_plus.TabIndex = 3;
            this.b_plus.Text = ">";
            this.b_plus.UseVisualStyleBackColor = true;

            parentForm.Controls.Add(this.p_curseur);
            parentForm.Controls.Add(this.p_fond);
            this.p_fond.ResumeLayout(false);
        }


        private System.Windows.Forms.Panel p_fond;
        private System.Windows.Forms.Panel p_curseur;
        private System.Windows.Forms.Button b_plus;
        private System.Windows.Forms.Button b_moins;
    }
}
