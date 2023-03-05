namespace Wikifix
{
    partial class FormImageSuggestions
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
            this.button1 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.CB_IW = new System.Windows.Forms.CheckBox();
            this.CB_suggestions = new System.Windows.Forms.CheckBox();
            this.CB_pages = new System.Windows.Forms.CheckBox();
            this.TB_offset = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RB_gallerylog = new System.Windows.Forms.RadioButton();
            this.RB_talk = new System.Windows.Forms.RadioButton();
            this.RB_separate = new System.Windows.Forms.RadioButton();
            this.RB_gallery = new System.Windows.Forms.RadioButton();
            this.RB_nothing = new System.Windows.Forms.RadioButton();
            this.CB_skipillustrated = new System.Windows.Forms.CheckBox();
            this.RB_file = new System.Windows.Forms.RadioButton();
            this.CB_fromfile = new System.Windows.Forms.CheckBox();
            this.TB_loops = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(658, 97);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(119, 47);
            this.button1.TabIndex = 0;
            this.button1.Text = "Add images";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(36, 76);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(351, 246);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // CB_IW
            // 
            this.CB_IW.AutoSize = true;
            this.CB_IW.Location = new System.Drawing.Point(479, 115);
            this.CB_IW.Name = "CB_IW";
            this.CB_IW.Size = new System.Drawing.Size(90, 17);
            this.CB_IW.TabIndex = 2;
            this.CB_IW.Text = "From interwiki";
            this.CB_IW.UseVisualStyleBackColor = true;
            // 
            // CB_suggestions
            // 
            this.CB_suggestions.AutoSize = true;
            this.CB_suggestions.Location = new System.Drawing.Point(479, 138);
            this.CB_suggestions.Name = "CB_suggestions";
            this.CB_suggestions.Size = new System.Drawing.Size(128, 17);
            this.CB_suggestions.TabIndex = 3;
            this.CB_suggestions.Text = "From suggestions API";
            this.CB_suggestions.UseVisualStyleBackColor = true;
            // 
            // CB_pages
            // 
            this.CB_pages.AutoSize = true;
            this.CB_pages.Checked = true;
            this.CB_pages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_pages.Location = new System.Drawing.Point(479, 161);
            this.CB_pages.Name = "CB_pages";
            this.CB_pages.Size = new System.Drawing.Size(101, 17);
            this.CB_pages.TabIndex = 4;
            this.CB_pages.Text = "From pages API";
            this.CB_pages.UseVisualStyleBackColor = true;
            // 
            // TB_offset
            // 
            this.TB_offset.Location = new System.Drawing.Point(497, 186);
            this.TB_offset.Name = "TB_offset";
            this.TB_offset.Size = new System.Drawing.Size(100, 20);
            this.TB_offset.TabIndex = 5;
            this.TB_offset.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(453, 189);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Offset:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RB_file);
            this.groupBox1.Controls.Add(this.RB_gallerylog);
            this.groupBox1.Controls.Add(this.RB_talk);
            this.groupBox1.Controls.Add(this.RB_separate);
            this.groupBox1.Controls.Add(this.RB_gallery);
            this.groupBox1.Controls.Add(this.RB_nothing);
            this.groupBox1.Location = new System.Drawing.Point(479, 290);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(253, 155);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "What to do with images:";
            // 
            // RB_gallerylog
            // 
            this.RB_gallerylog.AutoSize = true;
            this.RB_gallerylog.Location = new System.Drawing.Point(10, 108);
            this.RB_gallerylog.Name = "RB_gallerylog";
            this.RB_gallerylog.Size = new System.Drawing.Size(135, 17);
            this.RB_gallerylog.TabIndex = 4;
            this.RB_gallerylog.Text = "Add as gallery in log file";
            this.RB_gallerylog.UseVisualStyleBackColor = true;
            // 
            // RB_talk
            // 
            this.RB_talk.AutoSize = true;
            this.RB_talk.Location = new System.Drawing.Point(10, 89);
            this.RB_talk.Name = "RB_talk";
            this.RB_talk.Size = new System.Drawing.Size(106, 17);
            this.RB_talk.TabIndex = 3;
            this.RB_talk.Text = "Add on talk page";
            this.RB_talk.UseVisualStyleBackColor = true;
            // 
            // RB_separate
            // 
            this.RB_separate.AutoSize = true;
            this.RB_separate.Location = new System.Drawing.Point(10, 66);
            this.RB_separate.Name = "RB_separate";
            this.RB_separate.Size = new System.Drawing.Size(160, 17);
            this.RB_separate.TabIndex = 2;
            this.RB_separate.Text = "Add as separate pix in article";
            this.RB_separate.UseVisualStyleBackColor = true;
            // 
            // RB_gallery
            // 
            this.RB_gallery.AutoSize = true;
            this.RB_gallery.Location = new System.Drawing.Point(10, 43);
            this.RB_gallery.Name = "RB_gallery";
            this.RB_gallery.Size = new System.Drawing.Size(133, 17);
            this.RB_gallery.TabIndex = 1;
            this.RB_gallery.Text = "Add as gallery in article";
            this.RB_gallery.UseVisualStyleBackColor = true;
            // 
            // RB_nothing
            // 
            this.RB_nothing.AutoSize = true;
            this.RB_nothing.Checked = true;
            this.RB_nothing.Location = new System.Drawing.Point(10, 20);
            this.RB_nothing.Name = "RB_nothing";
            this.RB_nothing.Size = new System.Drawing.Size(203, 17);
            this.RB_nothing.TabIndex = 0;
            this.RB_nothing.TabStop = true;
            this.RB_nothing.Text = "List suggestions locally, no wiki action";
            this.RB_nothing.UseVisualStyleBackColor = true;
            // 
            // CB_skipillustrated
            // 
            this.CB_skipillustrated.AutoSize = true;
            this.CB_skipillustrated.Location = new System.Drawing.Point(479, 240);
            this.CB_skipillustrated.Name = "CB_skipillustrated";
            this.CB_skipillustrated.Size = new System.Drawing.Size(130, 17);
            this.CB_skipillustrated.TabIndex = 8;
            this.CB_skipillustrated.Text = "Skip illustrated articles";
            this.CB_skipillustrated.UseVisualStyleBackColor = true;
            // 
            // RB_file
            // 
            this.RB_file.AutoSize = true;
            this.RB_file.Location = new System.Drawing.Point(10, 131);
            this.RB_file.Name = "RB_file";
            this.RB_file.Size = new System.Drawing.Size(93, 17);
            this.RB_file.TabIndex = 5;
            this.RB_file.TabStop = true;
            this.RB_file.Text = "Save list to file";
            this.RB_file.UseVisualStyleBackColor = true;
            // 
            // CB_fromfile
            // 
            this.CB_fromfile.AutoSize = true;
            this.CB_fromfile.Location = new System.Drawing.Point(479, 92);
            this.CB_fromfile.Name = "CB_fromfile";
            this.CB_fromfile.Size = new System.Drawing.Size(65, 17);
            this.CB_fromfile.TabIndex = 9;
            this.CB_fromfile.Text = "From file";
            this.CB_fromfile.UseVisualStyleBackColor = true;
            // 
            // TB_loops
            // 
            this.TB_loops.Location = new System.Drawing.Point(497, 212);
            this.TB_loops.Name = "TB_loops";
            this.TB_loops.Size = new System.Drawing.Size(100, 20);
            this.TB_loops.TabIndex = 10;
            this.TB_loops.Text = "10";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(456, 216);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Loops";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // FormImageSuggestions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 468);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TB_loops);
            this.Controls.Add(this.CB_fromfile);
            this.Controls.Add(this.CB_skipillustrated);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TB_offset);
            this.Controls.Add(this.CB_pages);
            this.Controls.Add(this.CB_suggestions);
            this.Controls.Add(this.CB_IW);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button1);
            this.Name = "FormImageSuggestions";
            this.Text = "FormImageSuggestions";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.CheckBox CB_IW;
        private System.Windows.Forms.CheckBox CB_suggestions;
        private System.Windows.Forms.CheckBox CB_pages;
        private System.Windows.Forms.TextBox TB_offset;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton RB_gallery;
        private System.Windows.Forms.RadioButton RB_nothing;
        private System.Windows.Forms.RadioButton RB_separate;
        private System.Windows.Forms.RadioButton RB_talk;
        private System.Windows.Forms.RadioButton RB_gallerylog;
        private System.Windows.Forms.CheckBox CB_skipillustrated;
        private System.Windows.Forms.RadioButton RB_file;
        private System.Windows.Forms.CheckBox CB_fromfile;
        private System.Windows.Forms.TextBox TB_loops;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}