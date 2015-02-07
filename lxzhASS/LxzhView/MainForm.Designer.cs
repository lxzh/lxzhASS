namespace lxzh
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.niSetting = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsSetting = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.ttInfo = new System.Windows.Forms.ToolTip(this.components);
            this.lblName = new System.Windows.Forms.Label();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.cmsSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // niSetting
            // 
            this.niSetting.ContextMenuStrip = this.cmsSetting;
            this.niSetting.Icon = ((System.Drawing.Icon)(resources.GetObject("niSetting.Icon")));
            this.niSetting.Text = "截图";
            this.niSetting.Visible = true;
            // 
            // cmsSetting
            // 
            this.cmsSetting.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSetting,
            this.tsmiExit});
            this.cmsSetting.Name = "cmsSetting";
            this.cmsSetting.Size = new System.Drawing.Size(109, 52);
            this.cmsSetting.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsSetting_ItemClicked);
            // 
            // tsmiSetting
            // 
            this.tsmiSetting.Image = ((System.Drawing.Image)(resources.GetObject("tsmiSetting.Image")));
            this.tsmiSetting.Name = "tsmiSetting";
            this.tsmiSetting.Size = new System.Drawing.Size(108, 24);
            this.tsmiSetting.Text = "设置";
            // 
            // tsmiExit
            // 
            this.tsmiExit.Image = global::lxzh.Properties.Resources.exit;
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new System.Drawing.Size(108, 24);
            this.tsmiExit.Text = "退出";
            // 
            // ttInfo
            // 
            this.ttInfo.IsBalloon = true;
            // 
            // lblName
            // 
            this.lblName.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.lblName.ForeColor = System.Drawing.Color.NavajoWhite;
            this.lblName.Location = new System.Drawing.Point(2, 21);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(218, 45);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "一键截图V1.0";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAuthor
            // 
            this.lblAuthor.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.lblAuthor.ForeColor = System.Drawing.Color.NavajoWhite;
            this.lblAuthor.Location = new System.Drawing.Point(2, 66);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(218, 18);
            this.lblAuthor.TabIndex = 2;
            this.lblAuthor.Text = "By lxzh";
            this.lblAuthor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(220, 87);
            this.Controls.Add(this.lblAuthor);
            this.Controls.Add(this.lblName);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.cmsSetting.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon niSetting;
        private System.Windows.Forms.ContextMenuStrip cmsSetting;
        private System.Windows.Forms.ToolStripMenuItem tsmiSetting;
        private System.Windows.Forms.ToolStripMenuItem tsmiExit;
        private System.Windows.Forms.ToolTip ttInfo;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblAuthor;
    }
}

