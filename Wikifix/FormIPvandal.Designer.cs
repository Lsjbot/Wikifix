namespace Wikifix
{
    partial class FormIPvandal
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.Quitbutton = new System.Windows.Forms.Button();
            this.LB_IP = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.OKbutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(41, 42);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(314, 331);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // Quitbutton
            // 
            this.Quitbutton.Location = new System.Drawing.Point(609, 358);
            this.Quitbutton.Name = "Quitbutton";
            this.Quitbutton.Size = new System.Drawing.Size(120, 46);
            this.Quitbutton.TabIndex = 1;
            this.Quitbutton.Text = "Cancel";
            this.Quitbutton.UseVisualStyleBackColor = true;
            this.Quitbutton.Click += new System.EventHandler(this.Quitbutton_Click);
            // 
            // LB_IP
            // 
            this.LB_IP.FormattingEnabled = true;
            this.LB_IP.Location = new System.Drawing.Point(609, 115);
            this.LB_IP.Name = "LB_IP";
            this.LB_IP.Size = new System.Drawing.Size(120, 121);
            this.LB_IP.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(606, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Undoing edits by:";
            // 
            // OKbutton
            // 
            this.OKbutton.Location = new System.Drawing.Point(609, 284);
            this.OKbutton.Name = "OKbutton";
            this.OKbutton.Size = new System.Drawing.Size(120, 54);
            this.OKbutton.TabIndex = 4;
            this.OKbutton.Text = "Confirm";
            this.OKbutton.UseVisualStyleBackColor = true;
            this.OKbutton.Click += new System.EventHandler(this.OKbutton_Click);
            // 
            // FormIPvandal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.OKbutton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LB_IP);
            this.Controls.Add(this.Quitbutton);
            this.Controls.Add(this.richTextBox1);
            this.Name = "FormIPvandal";
            this.Text = "FormIPvandal";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button Quitbutton;
        private System.Windows.Forms.ListBox LB_IP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button OKbutton;
    }
}