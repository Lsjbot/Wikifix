namespace Wikifix
{
    partial class Form1
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
            this.Quitbutton = new System.Windows.Forms.Button();
            this.LB_wiki = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.Testbutton = new System.Windows.Forms.Button();
            this.InfoboxWDbutton = new System.Windows.Forms.Button();
            this.Replacebutton = new System.Windows.Forms.Button();
            this.Contribtestbutton = new System.Windows.Forms.Button();
            this.Vandalbutton = new System.Windows.Forms.Button();
            this.Deletebutton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.Redirectbutton = new System.Windows.Forms.Button();
            this.CB_onlylogged = new System.Windows.Forms.CheckBox();
            this.Revertbutton = new System.Windows.Forms.Button();
            this.synonymbutton = new System.Windows.Forms.Button();
            this.TB_resumeat = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Imagebutton = new System.Windows.Forms.Button();
            this.Categorybutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Quitbutton
            // 
            this.Quitbutton.Location = new System.Drawing.Point(579, 541);
            this.Quitbutton.Name = "Quitbutton";
            this.Quitbutton.Size = new System.Drawing.Size(75, 50);
            this.Quitbutton.TabIndex = 0;
            this.Quitbutton.Text = "Quit";
            this.Quitbutton.UseVisualStyleBackColor = true;
            this.Quitbutton.Click += new System.EventHandler(this.Quitbutton_Click);
            // 
            // LB_wiki
            // 
            this.LB_wiki.FormattingEnabled = true;
            this.LB_wiki.Location = new System.Drawing.Point(30, 43);
            this.LB_wiki.Name = "LB_wiki";
            this.LB_wiki.Size = new System.Drawing.Size(120, 212);
            this.LB_wiki.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Wiki to fix";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(30, 270);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(354, 321);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // Testbutton
            // 
            this.Testbutton.Location = new System.Drawing.Point(579, 459);
            this.Testbutton.Name = "Testbutton";
            this.Testbutton.Size = new System.Drawing.Size(75, 23);
            this.Testbutton.TabIndex = 4;
            this.Testbutton.Text = "Test login";
            this.Testbutton.UseVisualStyleBackColor = true;
            this.Testbutton.Click += new System.EventHandler(this.Testbutton_Click);
            // 
            // InfoboxWDbutton
            // 
            this.InfoboxWDbutton.Location = new System.Drawing.Point(524, 24);
            this.InfoboxWDbutton.Name = "InfoboxWDbutton";
            this.InfoboxWDbutton.Size = new System.Drawing.Size(130, 41);
            this.InfoboxWDbutton.TabIndex = 5;
            this.InfoboxWDbutton.Text = "Infoxbox tawo WD";
            this.InfoboxWDbutton.UseVisualStyleBackColor = true;
            this.InfoboxWDbutton.Click += new System.EventHandler(this.InfoboxWDbutton_Click);
            // 
            // Replacebutton
            // 
            this.Replacebutton.Location = new System.Drawing.Point(524, 71);
            this.Replacebutton.Name = "Replacebutton";
            this.Replacebutton.Size = new System.Drawing.Size(130, 48);
            this.Replacebutton.TabIndex = 6;
            this.Replacebutton.Text = "Replace-wikilink     (modify source first!)";
            this.Replacebutton.UseVisualStyleBackColor = true;
            this.Replacebutton.Click += new System.EventHandler(this.Replacebutton_Click);
            // 
            // Contribtestbutton
            // 
            this.Contribtestbutton.Location = new System.Drawing.Point(579, 488);
            this.Contribtestbutton.Name = "Contribtestbutton";
            this.Contribtestbutton.Size = new System.Drawing.Size(75, 35);
            this.Contribtestbutton.TabIndex = 7;
            this.Contribtestbutton.Text = "Test usercontrib";
            this.Contribtestbutton.UseVisualStyleBackColor = true;
            this.Contribtestbutton.Click += new System.EventHandler(this.Contribtestbutton_Click);
            // 
            // Vandalbutton
            // 
            this.Vandalbutton.Location = new System.Drawing.Point(524, 125);
            this.Vandalbutton.Name = "Vandalbutton";
            this.Vandalbutton.Size = new System.Drawing.Size(130, 39);
            this.Vandalbutton.TabIndex = 8;
            this.Vandalbutton.Text = "Revert IP vandal";
            this.Vandalbutton.UseVisualStyleBackColor = true;
            this.Vandalbutton.Click += new System.EventHandler(this.Vandalbutton_Click);
            // 
            // Deletebutton
            // 
            this.Deletebutton.Location = new System.Drawing.Point(524, 170);
            this.Deletebutton.Name = "Deletebutton";
            this.Deletebutton.Size = new System.Drawing.Size(130, 46);
            this.Deletebutton.TabIndex = 9;
            this.Deletebutton.Text = "Delete articles from list";
            this.Deletebutton.UseVisualStyleBackColor = true;
            this.Deletebutton.Click += new System.EventHandler(this.Deletebutton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Redirectbutton
            // 
            this.Redirectbutton.Location = new System.Drawing.Point(524, 222);
            this.Redirectbutton.Name = "Redirectbutton";
            this.Redirectbutton.Size = new System.Drawing.Size(130, 48);
            this.Redirectbutton.TabIndex = 10;
            this.Redirectbutton.Text = "Redirect articles from list";
            this.Redirectbutton.UseVisualStyleBackColor = true;
            this.Redirectbutton.Click += new System.EventHandler(this.Redirectbutton_Click);
            // 
            // CB_onlylogged
            // 
            this.CB_onlylogged.AutoSize = true;
            this.CB_onlylogged.Checked = true;
            this.CB_onlylogged.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_onlylogged.Location = new System.Drawing.Point(524, 270);
            this.CB_onlylogged.Name = "CB_onlylogged";
            this.CB_onlylogged.Size = new System.Drawing.Size(118, 17);
            this.CB_onlylogged.TabIndex = 11;
            this.CB_onlylogged.Text = "Only logged failures";
            this.CB_onlylogged.UseVisualStyleBackColor = true;
            // 
            // Revertbutton
            // 
            this.Revertbutton.Location = new System.Drawing.Point(445, 222);
            this.Revertbutton.Name = "Revertbutton";
            this.Revertbutton.Size = new System.Drawing.Size(75, 48);
            this.Revertbutton.TabIndex = 12;
            this.Revertbutton.Text = "Revert wrong moves";
            this.Revertbutton.UseVisualStyleBackColor = true;
            this.Revertbutton.Click += new System.EventHandler(this.Revertbutton_Click);
            // 
            // synonymbutton
            // 
            this.synonymbutton.Location = new System.Drawing.Point(524, 293);
            this.synonymbutton.Name = "synonymbutton";
            this.synonymbutton.Size = new System.Drawing.Size(130, 44);
            this.synonymbutton.TabIndex = 13;
            this.synonymbutton.Text = "Make synonym categories";
            this.synonymbutton.UseVisualStyleBackColor = true;
            this.synonymbutton.Click += new System.EventHandler(this.synonymbutton_Click);
            // 
            // TB_resumeat
            // 
            this.TB_resumeat.Location = new System.Drawing.Point(356, 86);
            this.TB_resumeat.Name = "TB_resumeat";
            this.TB_resumeat.Size = new System.Drawing.Size(162, 20);
            this.TB_resumeat.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(398, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Resume at:";
            // 
            // Imagebutton
            // 
            this.Imagebutton.Location = new System.Drawing.Point(524, 345);
            this.Imagebutton.Name = "Imagebutton";
            this.Imagebutton.Size = new System.Drawing.Size(130, 40);
            this.Imagebutton.TabIndex = 16;
            this.Imagebutton.Text = "Image suggestions";
            this.Imagebutton.UseVisualStyleBackColor = true;
            this.Imagebutton.Click += new System.EventHandler(this.Imagebutton_Click);
            // 
            // Categorybutton
            // 
            this.Categorybutton.Location = new System.Drawing.Point(524, 391);
            this.Categorybutton.Name = "Categorybutton";
            this.Categorybutton.Size = new System.Drawing.Size(130, 41);
            this.Categorybutton.TabIndex = 17;
            this.Categorybutton.Text = "Category handling";
            this.Categorybutton.UseVisualStyleBackColor = true;
            this.Categorybutton.Click += new System.EventHandler(this.Categorybutton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 624);
            this.Controls.Add(this.Categorybutton);
            this.Controls.Add(this.Imagebutton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TB_resumeat);
            this.Controls.Add(this.synonymbutton);
            this.Controls.Add(this.Revertbutton);
            this.Controls.Add(this.CB_onlylogged);
            this.Controls.Add(this.Redirectbutton);
            this.Controls.Add(this.Deletebutton);
            this.Controls.Add(this.Vandalbutton);
            this.Controls.Add(this.Contribtestbutton);
            this.Controls.Add(this.Replacebutton);
            this.Controls.Add(this.InfoboxWDbutton);
            this.Controls.Add(this.Testbutton);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LB_wiki);
            this.Controls.Add(this.Quitbutton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Quitbutton;
        private System.Windows.Forms.ListBox LB_wiki;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button Testbutton;
        private System.Windows.Forms.Button InfoboxWDbutton;
        private System.Windows.Forms.Button Replacebutton;
        private System.Windows.Forms.Button Contribtestbutton;
        private System.Windows.Forms.Button Vandalbutton;
        private System.Windows.Forms.Button Deletebutton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button Redirectbutton;
        private System.Windows.Forms.CheckBox CB_onlylogged;
        private System.Windows.Forms.Button Revertbutton;
        private System.Windows.Forms.Button synonymbutton;
        public System.Windows.Forms.TextBox TB_resumeat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Imagebutton;
        private System.Windows.Forms.Button Categorybutton;
    }
}

