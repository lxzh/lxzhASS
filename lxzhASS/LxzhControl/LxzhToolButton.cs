using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace lxzh
{
    [Designer(typeof(ToolButtonDesigner))]
    public partial class LxzhToolButton : Control
    {
        public LxzhToolButton() {
            InitializeComponent();
        }

        private Image btnImage;
        public Image BtnImage {
            get { return btnImage; }
            set {
                btnImage = value;
                this.Invalidate();
            }
        }

        private bool isSelectedBtn;
        public bool IsSelectedBtn {
            get { return isSelectedBtn; }
            set { 
                isSelectedBtn = value;
                if (!isSelectedBtn) this.isSingleSelectedBtn = false;
            }
        }

        private bool isSingleSelectedBtn;
        public bool IsSingleSelectedBtn {
            get { return isSingleSelectedBtn; }
            set { 
                isSingleSelectedBtn = value;
                if (isSingleSelectedBtn) this.isSelectedBtn = true;
            }
        }

        private bool isSelected;
        public bool IsSelected {
            get { return isSelected; }
            set {
                //if (!this.isSelectedBtn) return;
                if (value == isSelected) return;
                isSelected = value;
                this.Invalidate();
            }
        }
        private String tipText;

        [Description("get or set text would be shown on the button when the point of your mouse is hovering on it!\n获取或设置鼠标指针悬停在按钮上时要显示的文本")]
        [Category("ToolButton")]
        [RefreshProperties(RefreshProperties.All)]
        public String TipText {
            get {
                return tipText;
            }
            set {
                if (value != null) {
                    if (value.Length < 1) {
                        tipText = Text;
                    } else {
                        tipText = value;
                    }
                } else {
                    tipText = Text;
                }
            }
        }

        public override string Text {
            get {
                return base.Text;
            }
            set {
                base.Text = value;
                Size se = TextRenderer.MeasureText(this.Text, this.Font);
                this.Width = se.Width + 22;
            }
        }

        private bool bMouseEnter;

        protected override void OnMouseEnter(EventArgs e) {
            bMouseEnter = true;
            this.Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e) {
            bMouseEnter = false;
            this.Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnClick(EventArgs e) {
            if (this.isSelectedBtn) {
                if (this.isSelected) {
                    if (!this.isSingleSelectedBtn) {
                        this.isSelected = false;
                        this.Invalidate();
                    }
                } else {
                    this.isSelected = true; this.Invalidate();
                    for (int i = 0, len = this.Parent.Controls.Count; i < len; i++) {
                        if (this.Parent.Controls[i] is LxzhToolButton && this.Parent.Controls[i] != this) {
                            if (((LxzhToolButton)(this.Parent.Controls[i])).isSelected)
                                ((LxzhToolButton)(this.Parent.Controls[i])).IsSelected = false;
                        }
                    }
                }
            }
            this.Focus();
            base.OnClick(e);
        }

        protected override void OnDoubleClick(EventArgs e) {
            this.OnClick(e);
            base.OnDoubleClick(e);
        }

        protected override void OnPaint(PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.SmoothingMode = SmoothingMode.HighQuality;
            if (bMouseEnter) {
                ttInfo.SetToolTip(this, tipText);
                Rectangle rect = new Rectangle(1, 1, this.Width-2, this.Height-2);//this.Left-10,this.Top-10,this.Width-10,this.Height-10);                 
                GraphicsPath FormPath = GetRoundedRectPath(rect, 2);
                g.FillPath(Brushes.LightBlue, FormPath);
                g.FillRectangle(new SolidBrush(Color.FromArgb(255, 154, 219, 255)), this.ClientRectangle);
                g.DrawPath(new Pen(new SolidBrush(Color.FromArgb(255, 42, 135, 228)), 1.4F), FormPath);
                //g.DrawRectangle(Pens.DarkCyan, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
            }
            if (this.btnImage == null)
                g.DrawImage(global::lxzh.Properties.Resources.none, new Rectangle(2, 2, 18, 18));
            else
                g.DrawImage(this.btnImage, new Rectangle(2, 2, 18, 18));
            g.DrawString(this.Text, this.Font, Brushes.Black, 22, (this.Height - this.Font.Height) / 2+2);
            if (this.isSelected)
                g.DrawRectangle(Pens.DarkCyan, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
            //SetWindowRegion();
            base.OnPaint(e);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified) {
            base.SetBoundsCore(x, y, TextRenderer.MeasureText(this.Text, this.Font).Width + 22, 22, specified);
        }

        //绘制圆角
        private void SetWindowRegion() {
            System.Drawing.Drawing2D.GraphicsPath FormPath;
            FormPath = new System.Drawing.Drawing2D.GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);//this.Left-10,this.Top-10,this.Width-10,this.Height-10);                 
            FormPath = GetRoundedRectPath(rect, 2);
            this.Region = new Region(FormPath);
        }

        //private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius) {
        //    int diameter = radius;
        //    Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
        //    GraphicsPath path = new GraphicsPath();
        //    //   左上角   
        //    path.AddArc(arcRect, 180, 90);
        //    //   右上角   
        //    arcRect.X = rect.Right - diameter;
        //    path.AddArc(arcRect, 270, 90);
        //    //   右下角   
        //    arcRect.Y = rect.Bottom - diameter;
        //    path.AddArc(arcRect, 0, 90);
        //    //   左下角   
        //    arcRect.X = rect.Left;
        //    path.AddArc(arcRect, 90, 90);
        //    path.CloseFigure();
        //    return path;
        //}

        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius) {
            GraphicsPath roundedRect = new GraphicsPath();
            roundedRect.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
            roundedRect.AddLine(rect.X + radius, rect.Y, rect.Right - radius * 2, rect.Y);
            roundedRect.AddArc(rect.X + rect.Width - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
            roundedRect.AddLine(rect.Right, rect.Y + radius * 2, rect.Right, rect.Y + rect.Height - radius * 2);
            roundedRect.AddArc(rect.X + rect.Width - radius * 2, rect.Y + rect.Height - radius * 2, radius * 2, radius * 2, 0, 90);
            roundedRect.AddLine(rect.Right - radius * 2, rect.Bottom, rect.X + radius * 2, rect.Bottom);
            roundedRect.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            roundedRect.AddLine(rect.X, rect.Bottom - radius * 2, rect.X, rect.Y + radius * 2);
            roundedRect.CloseFigure();
            return roundedRect;
        } 
    }
}
