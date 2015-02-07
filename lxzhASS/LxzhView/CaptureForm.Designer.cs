namespace lxzh
{
    partial class CaptureForm
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
            lxzh.ControlProperties controlProperties1 = new lxzh.ControlProperties();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CaptureForm));
            this.plTool = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnOut = new lxzh.LxzhToolButton();
            this.btnFinish = new lxzh.LxzhToolButton();
            this.btnClose = new lxzh.LxzhToolButton();
            this.btnSave = new lxzh.LxzhToolButton();
            this.btnReset = new lxzh.LxzhToolButton();
            this.btnText = new lxzh.LxzhToolButton();
            this.btnBrush = new lxzh.LxzhToolButton();
            this.btnArrow = new lxzh.LxzhToolButton();
            this.btnEllipse = new lxzh.LxzhToolButton();
            this.btnRect = new lxzh.LxzhToolButton();
            this.plColorBox = new System.Windows.Forms.Panel();
            this.fontBox = new lxzh.LxzhComboBox();
            this.btnSmall = new lxzh.LxzhToolButton();
            this.btnLarge = new lxzh.LxzhToolButton();
            this.btnMiddle = new lxzh.LxzhToolButton();
            this.colorBox = new lxzh.LxzhColorBox();
            this.tBtn_TextFont = new lxzh.LxzhToolButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.txtDrawText = new lxzh.LxzhTextInput();
            this.imageProcessBox = new lxzh.ImageProcessBox();
            this.plTool.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.plColorBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fontBox)).BeginInit();
            this.SuspendLayout();
            // 
            // plTool
            // 
            this.plTool.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(238)))), ((int)(((byte)(255)))));
            this.plTool.Controls.Add(this.pictureBox1);
            this.plTool.Controls.Add(this.btnOut);
            this.plTool.Controls.Add(this.btnFinish);
            this.plTool.Controls.Add(this.btnClose);
            this.plTool.Controls.Add(this.btnSave);
            this.plTool.Controls.Add(this.btnReset);
            this.plTool.Controls.Add(this.btnText);
            this.plTool.Controls.Add(this.btnBrush);
            this.plTool.Controls.Add(this.btnArrow);
            this.plTool.Controls.Add(this.btnEllipse);
            this.plTool.Controls.Add(this.btnRect);
            this.plTool.Font = new System.Drawing.Font("宋体", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.plTool.Location = new System.Drawing.Point(16, 104);
            this.plTool.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.plTool.Name = "plTool";
            this.plTool.Size = new System.Drawing.Size(284, 31);
            this.plTool.TabIndex = 1;
            this.plTool.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::lxzh.Properties.Resources.separator;
            this.pictureBox1.Location = new System.Drawing.Point(196, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1, 17);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // btnOut
            // 
            this.btnOut.BtnImage = global::lxzh.Properties.Resources._out;
            this.btnOut.IsSelected = false;
            this.btnOut.IsSelectedBtn = false;
            this.btnOut.IsSingleSelectedBtn = false;
            this.btnOut.Location = new System.Drawing.Point(148, 4);
            this.btnOut.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnOut.Name = "btnOut";
            this.btnOut.Size = new System.Drawing.Size(22, 22);
            this.btnOut.TabIndex = 11;
            this.btnOut.TipText = "桌面显示";
            this.btnOut.Click += new System.EventHandler(this.tBtn_Out_Click);
            // 
            // btnFinish
            // 
            this.btnFinish.BtnImage = global::lxzh.Properties.Resources.ok;
            this.btnFinish.IsSelected = false;
            this.btnFinish.IsSelectedBtn = false;
            this.btnFinish.IsSingleSelectedBtn = false;
            this.btnFinish.Location = new System.Drawing.Point(224, 4);
            this.btnFinish.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(57, 22);
            this.btnFinish.TabIndex = 8;
            this.btnFinish.Text = "完成";
            this.btnFinish.TipText = "完成截图";
            this.btnFinish.Click += new System.EventHandler(this.tBtn_Finish_Click);
            // 
            // btnClose
            // 
            this.btnClose.BtnImage = global::lxzh.Properties.Resources.close;
            this.btnClose.IsSelected = false;
            this.btnClose.IsSelectedBtn = false;
            this.btnClose.IsSingleSelectedBtn = false;
            this.btnClose.Location = new System.Drawing.Point(200, 4);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(22, 22);
            this.btnClose.TabIndex = 7;
            this.btnClose.TipText = "退出截图";
            // 
            // btnSave
            // 
            this.btnSave.BtnImage = global::lxzh.Properties.Resources.save;
            this.btnSave.IsSelected = false;
            this.btnSave.IsSelectedBtn = false;
            this.btnSave.IsSingleSelectedBtn = false;
            this.btnSave.Location = new System.Drawing.Point(172, 4);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(22, 22);
            this.btnSave.TabIndex = 6;
            this.btnSave.TipText = "保存Ctrl+S";
            this.btnSave.Click += new System.EventHandler(this.tBtn_Save_Click);
            // 
            // btnReset
            // 
            this.btnReset.BtnImage = global::lxzh.Properties.Resources.cancel;
            this.btnReset.IsSelected = false;
            this.btnReset.IsSelectedBtn = false;
            this.btnReset.IsSingleSelectedBtn = false;
            this.btnReset.Location = new System.Drawing.Point(124, 4);
            this.btnReset.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(22, 22);
            this.btnReset.TabIndex = 5;
            this.btnReset.TipText = "撤销编辑Ctrl+Z";
            this.btnReset.Click += new System.EventHandler(this.tBtn_Reset_Click);
            // 
            // btnText
            // 
            this.btnText.BtnImage = global::lxzh.Properties.Resources.text;
            this.btnText.IsSelected = false;
            this.btnText.IsSelectedBtn = true;
            this.btnText.IsSingleSelectedBtn = false;
            this.btnText.Location = new System.Drawing.Point(100, 4);
            this.btnText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnText.Name = "btnText";
            this.btnText.Size = new System.Drawing.Size(22, 22);
            this.btnText.TabIndex = 4;
            this.btnText.TipText = "文字工具";
            // 
            // btnBrush
            // 
            this.btnBrush.BtnImage = global::lxzh.Properties.Resources.brush;
            this.btnBrush.IsSelected = false;
            this.btnBrush.IsSelectedBtn = true;
            this.btnBrush.IsSingleSelectedBtn = false;
            this.btnBrush.Location = new System.Drawing.Point(76, 4);
            this.btnBrush.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnBrush.Name = "btnBrush";
            this.btnBrush.Size = new System.Drawing.Size(22, 22);
            this.btnBrush.TabIndex = 3;
            this.btnBrush.TipText = "画刷工具";
            // 
            // btnArrow
            // 
            this.btnArrow.BtnImage = global::lxzh.Properties.Resources.arrow;
            this.btnArrow.IsSelected = false;
            this.btnArrow.IsSelectedBtn = true;
            this.btnArrow.IsSingleSelectedBtn = false;
            this.btnArrow.Location = new System.Drawing.Point(52, 4);
            this.btnArrow.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnArrow.Name = "btnArrow";
            this.btnArrow.Size = new System.Drawing.Size(22, 22);
            this.btnArrow.TabIndex = 2;
            this.btnArrow.TipText = "箭头工具";
            // 
            // btnEllipse
            // 
            this.btnEllipse.BtnImage = global::lxzh.Properties.Resources.ellips;
            this.btnEllipse.IsSelected = false;
            this.btnEllipse.IsSelectedBtn = true;
            this.btnEllipse.IsSingleSelectedBtn = false;
            this.btnEllipse.Location = new System.Drawing.Point(28, 4);
            this.btnEllipse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnEllipse.Name = "btnEllipse";
            this.btnEllipse.Size = new System.Drawing.Size(22, 22);
            this.btnEllipse.TabIndex = 1;
            this.btnEllipse.TipText = "椭圆工具";
            // 
            // btnRect
            // 
            this.btnRect.BtnImage = global::lxzh.Properties.Resources.rect;
            this.btnRect.IsSelected = false;
            this.btnRect.IsSelectedBtn = true;
            this.btnRect.IsSingleSelectedBtn = false;
            this.btnRect.Location = new System.Drawing.Point(4, 4);
            this.btnRect.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnRect.Name = "btnRect";
            this.btnRect.Size = new System.Drawing.Size(22, 22);
            this.btnRect.TabIndex = 0;
            this.btnRect.TipText = "矩形工具";
            // 
            // plColorBox
            // 
            this.plColorBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(238)))), ((int)(((byte)(255)))));
            this.plColorBox.Controls.Add(this.fontBox);
            this.plColorBox.Controls.Add(this.btnSmall);
            this.plColorBox.Controls.Add(this.btnLarge);
            this.plColorBox.Controls.Add(this.btnMiddle);
            this.plColorBox.Controls.Add(this.colorBox);
            this.plColorBox.Controls.Add(this.tBtn_TextFont);
            this.plColorBox.Location = new System.Drawing.Point(16, 142);
            this.plColorBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.plColorBox.Name = "plColorBox";
            this.plColorBox.Size = new System.Drawing.Size(265, 40);
            this.plColorBox.TabIndex = 2;
            this.plColorBox.Visible = false;
            // 
            // fontBox
            // 
            this.fontBox.BackColor = System.Drawing.Color.White;
            controlProperties1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.fontBox.CtrlProperties = controlProperties1;
            this.fontBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.fontBox.Items = ((System.Collections.Generic.List<string>)(resources.GetObject("fontBox.Items")));
            this.fontBox.Location = new System.Drawing.Point(35, 7);
            this.fontBox.Name = "fontBox";
            this.fontBox.Size = new System.Drawing.Size(57, 22);
            this.fontBox.TabIndex = 6;
            this.fontBox.TabStop = false;
            this.fontBox.Text = "10";
            // 
            // btnSmall
            // 
            this.btnSmall.BtnImage = global::lxzh.Properties.Resources.small;
            this.btnSmall.IsSelected = true;
            this.btnSmall.IsSelectedBtn = true;
            this.btnSmall.IsSingleSelectedBtn = true;
            this.btnSmall.Location = new System.Drawing.Point(5, 7);
            this.btnSmall.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSmall.Name = "btnSmall";
            this.btnSmall.Size = new System.Drawing.Size(22, 22);
            this.btnSmall.TabIndex = 4;
            this.btnSmall.TipText = "";
            // 
            // btnLarge
            // 
            this.btnLarge.BtnImage = global::lxzh.Properties.Resources.large;
            this.btnLarge.IsSelected = false;
            this.btnLarge.IsSelectedBtn = true;
            this.btnLarge.IsSingleSelectedBtn = true;
            this.btnLarge.Location = new System.Drawing.Point(66, 7);
            this.btnLarge.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnLarge.Name = "btnLarge";
            this.btnLarge.Size = new System.Drawing.Size(22, 22);
            this.btnLarge.TabIndex = 3;
            this.btnLarge.TipText = "";
            // 
            // btnMiddle
            // 
            this.btnMiddle.BtnImage = global::lxzh.Properties.Resources.middle;
            this.btnMiddle.IsSelected = false;
            this.btnMiddle.IsSelectedBtn = true;
            this.btnMiddle.IsSingleSelectedBtn = true;
            this.btnMiddle.Location = new System.Drawing.Point(35, 7);
            this.btnMiddle.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnMiddle.Name = "btnMiddle";
            this.btnMiddle.Size = new System.Drawing.Size(22, 22);
            this.btnMiddle.TabIndex = 2;
            this.btnMiddle.TipText = "";
            // 
            // colorBox
            // 
            this.colorBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(238)))), ((int)(((byte)(255)))));
            this.colorBox.Location = new System.Drawing.Point(99, 0);
            this.colorBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.colorBox.Name = "colorBox";
            this.colorBox.Size = new System.Drawing.Size(165, 35);
            this.colorBox.TabIndex = 0;
            this.colorBox.Text = "colorBox1";
            // 
            // tBtn_TextFont
            // 
            this.tBtn_TextFont.BtnImage = global::lxzh.Properties.Resources.text;
            this.tBtn_TextFont.IsSelected = false;
            this.tBtn_TextFont.IsSelectedBtn = true;
            this.tBtn_TextFont.IsSingleSelectedBtn = false;
            this.tBtn_TextFont.Location = new System.Drawing.Point(5, 7);
            this.tBtn_TextFont.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tBtn_TextFont.Name = "tBtn_TextFont";
            this.tBtn_TextFont.Size = new System.Drawing.Size(22, 22);
            this.tBtn_TextFont.TabIndex = 5;
            this.tBtn_TextFont.TipText = "";
            this.tBtn_TextFont.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // txtDrawText
            // 
            this.txtDrawText.BackColor = System.Drawing.Color.White;
            this.txtDrawText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDrawText.Location = new System.Drawing.Point(137, 12);
            this.txtDrawText.Multiline = true;
            this.txtDrawText.Name = "txtDrawText";
            this.txtDrawText.Size = new System.Drawing.Size(163, 43);
            this.txtDrawText.TabIndex = 5;
            this.txtDrawText.Visible = false;
            this.txtDrawText.TextChanged += new System.EventHandler(this.txtDrawText_TextChanged);
            this.txtDrawText.Validating += new System.ComponentModel.CancelEventHandler(this.txtDrawText_Validating);
            // 
            // imageProcessBox
            // 
            this.imageProcessBox.BackColor = System.Drawing.Color.Transparent;
            this.imageProcessBox.BaseImage = null;
            this.imageProcessBox.CanReset = true;
            this.imageProcessBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.imageProcessBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageProcessBox.DotColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(255)))));
            this.imageProcessBox.Font = new System.Drawing.Font("微软雅黑", 6.6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.imageProcessBox.ForeColor = System.Drawing.Color.White;
            this.imageProcessBox.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(255)))));
            this.imageProcessBox.Location = new System.Drawing.Point(0, 0);
            this.imageProcessBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.imageProcessBox.Name = "imageProcessBox";
            this.imageProcessBox.SelectedRect = new System.Drawing.Rectangle(-100, -100, 7, 7);
            this.imageProcessBox.Size = new System.Drawing.Size(484, 309);
            this.imageProcessBox.TabIndex = 0;
            this.imageProcessBox.Text = "imageProcessBox";
            this.imageProcessBox.DoubleClick += new System.EventHandler(this.imageProcessBox_DoubleClick);
            this.imageProcessBox.Paint += new System.Windows.Forms.PaintEventHandler(this.imageProcessBox_Paint);
            this.imageProcessBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imageProcessBox_MouseMove);
            this.imageProcessBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imageProcessBox_MouseDown);
            this.imageProcessBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.imageProcessBox_MouseUp);
            // 
            // CaptureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 309);
            this.Controls.Add(this.txtDrawText);
            this.Controls.Add(this.plColorBox);
            this.Controls.Add(this.plTool);
            this.Controls.Add(this.imageProcessBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "CaptureForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "FrmCapture";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FrmCapture_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmCapture_KeyDown);
            this.plTool.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.plColorBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fontBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel plTool;
        private LxzhToolButton btnEllipse;
        private LxzhToolButton btnRect;
        private LxzhToolButton btnArrow;
        private LxzhToolButton btnBrush;
        private LxzhToolButton btnText;
        private LxzhToolButton btnFinish;
        private LxzhToolButton btnClose;
        private LxzhToolButton btnSave;
        private LxzhToolButton btnReset;
        private ImageProcessBox imageProcessBox;
        private System.Windows.Forms.Panel plColorBox;
        private LxzhColorBox colorBox;
        private LxzhToolButton btnSmall;
        private LxzhToolButton btnLarge;
        private LxzhToolButton btnMiddle;
        private System.Windows.Forms.Timer timer1;
        private LxzhToolButton btnOut;
        private System.Windows.Forms.PictureBox pictureBox1;
        private LxzhToolButton tBtn_TextFont;
        private LxzhComboBox fontBox;
        private LxzhTextInput txtDrawText;

    }
}