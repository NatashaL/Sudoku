namespace Sudoku
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.saveScore = new System.Windows.Forms.Button();
            this.name = new System.Windows.Forms.TextBox();
            this.thanks = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(101, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Congratulations!";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Enter your name:";
            // 
            // saveScore
            // 
            this.saveScore.Location = new System.Drawing.Point(24, 116);
            this.saveScore.Name = "saveScore";
            this.saveScore.Size = new System.Drawing.Size(107, 23);
            this.saveScore.TabIndex = 3;
            this.saveScore.Text = "Save my highscore";
            this.saveScore.UseVisualStyleBackColor = true;
            this.saveScore.Click += new System.EventHandler(this.saveScore_Click);
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(24, 83);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(231, 20);
            this.name.TabIndex = 4;
            this.name.TextChanged += new System.EventHandler(this.name_TextChanged);
            // 
            // thanks
            // 
            this.thanks.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.thanks.Location = new System.Drawing.Point(148, 116);
            this.thanks.Name = "thanks";
            this.thanks.Size = new System.Drawing.Size(107, 23);
            this.thanks.TabIndex = 5;
            this.thanks.Text = "No thanks.";
            this.thanks.UseVisualStyleBackColor = true;
            this.thanks.Click += new System.EventHandler(this.thanks_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(83, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "You made it in the top 5!";
            // 
            // Form2
            // 
            this.AcceptButton = this.saveScore;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.thanks;
            this.ClientSize = new System.Drawing.Size(284, 161);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.thanks);
            this.Controls.Add(this.name);
            this.Controls.Add(this.saveScore);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button saveScore;
        private System.Windows.Forms.Button thanks;
        public System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label label3;

    }
}