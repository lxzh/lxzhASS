namespace lxzh
{
    partial class StickyForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StickyForm));
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiOTC = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCTC = new System.Windows.Forms.ToolStripMenuItem();
            this.tss1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiSO = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSC = new System.Windows.Forms.ToolStripMenuItem();
            this.tss2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiClose = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiOTC,
            this.tsmiCTC,
            this.tss1,
            this.tsmiSO,
            this.tsmiSC,
            this.tss2,
            this.tsmiHelp,
            this.tsmiClose});
            this.contextMenuStrip.Name = "contextMenuStrip1";
            this.contextMenuStrip.Size = new System.Drawing.Size(214, 182);
            // 
            // tsmiOTC
            // 
            this.tsmiOTC.Name = "tsmiOTC";
            this.tsmiOTC.Size = new System.Drawing.Size(213, 24);
            this.tsmiOTC.Text = "保存原始图到剪切板";
            this.tsmiOTC.Click += new System.EventHandler(this.tsmiOTC_Click);
            // 
            // tsmiCTC
            // 
            this.tsmiCTC.Name = "tsmiCTC";
            this.tsmiCTC.Size = new System.Drawing.Size(213, 24);
            this.tsmiCTC.Text = "保存当前图到剪切板";
            this.tsmiCTC.Click += new System.EventHandler(this.tsmiCTC_Click);
            // 
            // tss1
            // 
            this.tss1.Name = "tss1";
            this.tss1.Size = new System.Drawing.Size(210, 6);
            // 
            // tsmiSO
            // 
            this.tsmiSO.Name = "tsmiSO";
            this.tsmiSO.Size = new System.Drawing.Size(213, 24);
            this.tsmiSO.Text = "保存原始图";
            this.tsmiSO.Click += new System.EventHandler(this.tsmiSaveOriginal_Click);
            // 
            // tsmiSC
            // 
            this.tsmiSC.Name = "tsmiSC";
            this.tsmiSC.Size = new System.Drawing.Size(213, 24);
            this.tsmiSC.Text = "保存当前图";
            this.tsmiSC.Click += new System.EventHandler(this.tsmiSaveCurrent_Click);
            // 
            // tss2
            // 
            this.tss2.Name = "tss2";
            this.tss2.Size = new System.Drawing.Size(210, 6);
            // 
            // tsmiHelp
            // 
            this.tsmiHelp.Name = "tsmiHelp";
            this.tsmiHelp.Size = new System.Drawing.Size(213, 24);
            this.tsmiHelp.Text = "帮助";
            this.tsmiHelp.Click += new System.EventHandler(this.tsmiHelp_Click);
            // 
            // tsmiClose
            // 
            this.tsmiClose.Name = "tsmiClose";
            this.tsmiClose.Size = new System.Drawing.Size(213, 24);
            this.tsmiClose.Text = "关闭";
            this.tsmiClose.Click += new System.EventHandler(this.tsmiClose_Click);
            // 
            // StickyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 309);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "StickyForm";
            this.TopMost = true;
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem tsmiCTC;
        private System.Windows.Forms.ToolStripMenuItem tsmiSO;
        private System.Windows.Forms.ToolStripSeparator tss1;
        private System.Windows.Forms.ToolStripMenuItem tsmiSC;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp;
        private System.Windows.Forms.ToolStripMenuItem tsmiClose;
        private System.Windows.Forms.ToolStripSeparator tss2;
        private System.Windows.Forms.ToolStripMenuItem tsmiOTC;
    }
}