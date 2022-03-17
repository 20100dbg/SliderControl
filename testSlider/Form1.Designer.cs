namespace testSlider
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.num_value = new System.Windows.Forms.NumericUpDown();
            this.num_span = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.num_largeChange = new System.Windows.Forms.NumericUpDown();
            this.num_smallChange = new System.Windows.Forms.NumericUpDown();
            this.b_apply = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.num_value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_span)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_largeChange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_smallChange)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(396, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(396, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(396, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "label3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(396, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "label4";
            // 
            // num_value
            // 
            this.num_value.Location = new System.Drawing.Point(338, 191);
            this.num_value.Name = "num_value";
            this.num_value.Size = new System.Drawing.Size(64, 20);
            this.num_value.TabIndex = 4;
            // 
            // num_span
            // 
            this.num_span.Location = new System.Drawing.Point(338, 217);
            this.num_span.Name = "num_span";
            this.num_span.Size = new System.Drawing.Size(64, 20);
            this.num_span.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(297, 193);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Value";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(296, 219);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Span";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(257, 284);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "LargeChange";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(263, 258);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "SmallChange";
            // 
            // num_largeChange
            // 
            this.num_largeChange.Location = new System.Drawing.Point(338, 282);
            this.num_largeChange.Name = "num_largeChange";
            this.num_largeChange.Size = new System.Drawing.Size(64, 20);
            this.num_largeChange.TabIndex = 10;
            // 
            // num_smallChange
            // 
            this.num_smallChange.Location = new System.Drawing.Point(338, 256);
            this.num_smallChange.Name = "num_smallChange";
            this.num_smallChange.Size = new System.Drawing.Size(64, 20);
            this.num_smallChange.TabIndex = 9;
            // 
            // b_apply
            // 
            this.b_apply.Location = new System.Drawing.Point(441, 304);
            this.b_apply.Name = "b_apply";
            this.b_apply.Size = new System.Drawing.Size(75, 23);
            this.b_apply.TabIndex = 13;
            this.b_apply.Text = "Apply";
            this.b_apply.UseVisualStyleBackColor = true;
            this.b_apply.Click += new System.EventHandler(this.b_apply_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 333);
            this.Controls.Add(this.b_apply);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.num_largeChange);
            this.Controls.Add(this.num_smallChange);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.num_span);
            this.Controls.Add(this.num_value);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.num_value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_span)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_largeChange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_smallChange)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown num_value;
        private System.Windows.Forms.NumericUpDown num_span;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown num_largeChange;
        private System.Windows.Forms.NumericUpDown num_smallChange;
        private System.Windows.Forms.Button b_apply;
    }
}

