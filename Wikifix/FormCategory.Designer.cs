namespace Wikifix
{
    partial class FormCategory
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
            this.button1 = new System.Windows.Forms.Button();
            this.TBfrom = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TBto = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.movebutton = new System.Windows.Forms.Button();
            this.movetreebutton = new System.Windows.Forms.Button();
            this.TBoldending = new System.Windows.Forms.TextBox();
            this.TBnewending = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.subdivisionButton = new System.Windows.Forms.Button();
            this.Heyobutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(58, 44);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(233, 181);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // Quitbutton
            // 
            this.Quitbutton.Location = new System.Drawing.Point(692, 390);
            this.Quitbutton.Name = "Quitbutton";
            this.Quitbutton.Size = new System.Drawing.Size(96, 39);
            this.Quitbutton.TabIndex = 1;
            this.Quitbutton.Text = "Quit";
            this.Quitbutton.UseVisualStyleBackColor = true;
            this.Quitbutton.Click += new System.EventHandler(this.Quitbutton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(692, 80);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 35);
            this.button1.TabIndex = 2;
            this.button1.Text = "List articles in FROM category";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TBfrom
            // 
            this.TBfrom.Location = new System.Drawing.Point(447, 31);
            this.TBfrom.Name = "TBfrom";
            this.TBfrom.Size = new System.Drawing.Size(174, 20);
            this.TBfrom.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(356, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "FROM category:";
            // 
            // TBto
            // 
            this.TBto.Location = new System.Drawing.Point(447, 63);
            this.TBto.Name = "TBto";
            this.TBto.Size = new System.Drawing.Size(174, 20);
            this.TBto.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(372, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "TO category:";
            // 
            // movebutton
            // 
            this.movebutton.Location = new System.Drawing.Point(692, 33);
            this.movebutton.Name = "movebutton";
            this.movebutton.Size = new System.Drawing.Size(96, 41);
            this.movebutton.TabIndex = 7;
            this.movebutton.Text = "Move all articles";
            this.movebutton.UseVisualStyleBackColor = true;
            this.movebutton.Click += new System.EventHandler(this.movebutton_Click);
            // 
            // movetreebutton
            // 
            this.movetreebutton.Location = new System.Drawing.Point(670, 121);
            this.movetreebutton.Name = "movetreebutton";
            this.movetreebutton.Size = new System.Drawing.Size(118, 36);
            this.movetreebutton.TabIndex = 8;
            this.movetreebutton.Text = "Move tree to new ending";
            this.movetreebutton.UseVisualStyleBackColor = true;
            this.movetreebutton.Click += new System.EventHandler(this.movetreebutton_Click);
            // 
            // TBoldending
            // 
            this.TBoldending.Location = new System.Drawing.Point(447, 111);
            this.TBoldending.Name = "TBoldending";
            this.TBoldending.Size = new System.Drawing.Size(100, 20);
            this.TBoldending.TabIndex = 9;
            // 
            // TBnewending
            // 
            this.TBnewending.Location = new System.Drawing.Point(447, 137);
            this.TBnewending.Name = "TBnewending";
            this.TBnewending.Size = new System.Drawing.Size(100, 20);
            this.TBnewending.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(380, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Old ending:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(380, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "New ending:";
            // 
            // subdivisionButton
            // 
            this.subdivisionButton.Location = new System.Drawing.Point(670, 163);
            this.subdivisionButton.Name = "subdivisionButton";
            this.subdivisionButton.Size = new System.Drawing.Size(118, 38);
            this.subdivisionButton.TabIndex = 13;
            this.subdivisionButton.Text = "Make subdivision categories";
            this.subdivisionButton.UseVisualStyleBackColor = true;
            this.subdivisionButton.Click += new System.EventHandler(this.subdivisionButton_Click);
            // 
            // Heyobutton
            // 
            this.Heyobutton.Location = new System.Drawing.Point(670, 207);
            this.Heyobutton.Name = "Heyobutton";
            this.Heyobutton.Size = new System.Drawing.Size(118, 39);
            this.Heyobutton.TabIndex = 14;
            this.Heyobutton.Text = "Move Heyograpiya to subcategories";
            this.Heyobutton.UseVisualStyleBackColor = true;
            this.Heyobutton.Click += new System.EventHandler(this.Heyobutton_Click);
            // 
            // FormCategory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Heyobutton);
            this.Controls.Add(this.subdivisionButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TBnewending);
            this.Controls.Add(this.TBoldending);
            this.Controls.Add(this.movetreebutton);
            this.Controls.Add(this.movebutton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TBto);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TBfrom);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Quitbutton);
            this.Controls.Add(this.richTextBox1);
            this.Name = "FormCategory";
            this.Text = "FormCategory";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button Quitbutton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox TBfrom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBto;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button movebutton;
        private System.Windows.Forms.Button movetreebutton;
        private System.Windows.Forms.TextBox TBoldending;
        private System.Windows.Forms.TextBox TBnewending;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button subdivisionButton;
        private System.Windows.Forms.Button Heyobutton;
    }
}