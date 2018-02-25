namespace lxzh
{
    partial class PinForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PinForm));
            this.rtbCode = new System.Windows.Forms.RichTextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiTopMost = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiWordWrap = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpacity = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpacity100 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpacity80 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpacity60 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpacity40 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpacity20 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpacity10 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtbCode
            // 
            this.rtbCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbCode.Location = new System.Drawing.Point(5, 33);
            this.rtbCode.Name = "rtbCode";
            this.rtbCode.Size = new System.Drawing.Size(500, 500);
            this.rtbCode.TabIndex = 0;
            this.rtbCode.Text = "";
            this.rtbCode.WordWrap = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Image = global::lxzh.Properties.Resources.close;
            this.btnClose.Location = new System.Drawing.Point(480, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(28, 28);
            this.btnClose.TabIndex = 1;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiTopMost,
            this.tsmiWordWrap,
            this.tsmiOpacity});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(178, 88);
            // 
            // tsmiTopMost
            // 
            this.tsmiTopMost.Checked = true;
            this.tsmiTopMost.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsmiTopMost.Name = "tsmiTopMost";
            this.tsmiTopMost.Size = new System.Drawing.Size(177, 28);
            this.tsmiTopMost.Text = "置顶(&T)";
            this.tsmiTopMost.Click += new System.EventHandler(this.tsmiTopMost_Click);
            // 
            // tsmiWordWrap
            // 
            this.tsmiWordWrap.Name = "tsmiWordWrap";
            this.tsmiWordWrap.Size = new System.Drawing.Size(177, 28);
            this.tsmiWordWrap.Text = "自动换行(&A)";
            this.tsmiWordWrap.Click += new System.EventHandler(this.tsmiWordWrap_Click);
            // 
            // tsmiOpacity
            // 
            this.tsmiOpacity.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiOpacity100,
            this.tsmiOpacity80,
            this.tsmiOpacity60,
            this.tsmiOpacity40,
            this.tsmiOpacity20,
            this.tsmiOpacity10});
            this.tsmiOpacity.Name = "tsmiOpacity";
            this.tsmiOpacity.Size = new System.Drawing.Size(177, 28);
            this.tsmiOpacity.Text = "透明度(&O)";
            // 
            // tsmiOpacity100
            // 
            this.tsmiOpacity100.Name = "tsmiOpacity100";
            this.tsmiOpacity100.Size = new System.Drawing.Size(129, 28);
            this.tsmiOpacity100.Text = "100%";
            this.tsmiOpacity100.Click += new System.EventHandler(this.tsmiOpacity_Click);
            // 
            // tsmiOpacity80
            // 
            this.tsmiOpacity80.Name = "tsmiOpacity80";
            this.tsmiOpacity80.Size = new System.Drawing.Size(129, 28);
            this.tsmiOpacity80.Text = "80%";
            this.tsmiOpacity80.Click += new System.EventHandler(this.tsmiOpacity_Click);
            // 
            // tsmiOpacity60
            // 
            this.tsmiOpacity60.Name = "tsmiOpacity60";
            this.tsmiOpacity60.Size = new System.Drawing.Size(129, 28);
            this.tsmiOpacity60.Text = "60%";
            this.tsmiOpacity60.Click += new System.EventHandler(this.tsmiOpacity_Click);
            // 
            // tsmiOpacity40
            // 
            this.tsmiOpacity40.Name = "tsmiOpacity40";
            this.tsmiOpacity40.Size = new System.Drawing.Size(129, 28);
            this.tsmiOpacity40.Text = "40%";
            this.tsmiOpacity40.Click += new System.EventHandler(this.tsmiOpacity_Click);
            // 
            // tsmiOpacity20
            // 
            this.tsmiOpacity20.Name = "tsmiOpacity20";
            this.tsmiOpacity20.Size = new System.Drawing.Size(129, 28);
            this.tsmiOpacity20.Text = "20%";
            this.tsmiOpacity20.Click += new System.EventHandler(this.tsmiOpacity_Click);
            // 
            // tsmiOpacity10
            // 
            this.tsmiOpacity10.Name = "tsmiOpacity10";
            this.tsmiOpacity10.Size = new System.Drawing.Size(129, 28);
            this.tsmiOpacity10.Text = "10%";
            this.tsmiOpacity10.Click += new System.EventHandler(this.tsmiOpacity_Click);
            // 
            // PinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 538);
            this.ContextMenuStrip = this.contextMenuStrip;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.rtbCode);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PinForm";
            this.ShowIcon = false;
            this.Text = "PinForm";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Load += new System.EventHandler(this.PinForm_Load);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbCode;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem tsmiTopMost;
        private System.Windows.Forms.ToolStripMenuItem tsmiWordWrap;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpacity;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpacity100;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpacity80;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpacity60;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpacity40;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpacity20;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpacity10;
    }
}