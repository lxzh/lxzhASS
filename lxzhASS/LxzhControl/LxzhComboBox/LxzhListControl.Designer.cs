namespace lxzh {
    partial class LxzhListControl {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            this.ScrollBar = new lxzh.LxzhScrollBar();
            this.SuspendLayout();
            // 
            // ScrollBar
            // 
            this.ScrollBar.BackColor = System.Drawing.Color.Transparent;
            this.ScrollBar.BorderColor = System.Drawing.SystemColors.Highlight;
            this.ScrollBar.Delta = 0;
            this.ScrollBar.DrawBorder = false;
            this.ScrollBar.Location = new System.Drawing.Point(59, 0);
            this.ScrollBar.Name = "ScrollBar";
            this.ScrollBar.Size = new System.Drawing.Size(16, 200);
            this.ScrollBar.TabIndex = 0;
            // 
            // LxzhListControl
            // 
            this.Controls.Add(this.ScrollBar);
            this.Name = "LxzhListControl";
            this.Size = new System.Drawing.Size(76, 200);
            this.ResumeLayout(false);

        }

        #endregion

        protected internal LxzhScrollBar ScrollBar;

    }
}
