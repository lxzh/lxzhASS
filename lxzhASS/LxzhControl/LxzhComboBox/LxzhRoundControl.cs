using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace lxzh {
    public partial class LxzhRoundControl : UserControl {
        public LxzhRoundControl() {
            InitializeControlStyle();
            InitializeComponent();
            base.BackColor = Color.Transparent;
        }
        private int radiu = 1;
        [Browsable(true), DefaultValue(1), Description("是否绘制边框")]
        public int Radiu {
            get { return radiu; }
            set { 
                radiu = value;
                if (radiu < 1)
                    radiu = 1;
                else if (radiu > this.Width / 2 || radiu > this.Height / 2) {
                    radiu = Math.Min(this.Width,this.Height)/2;
                }
                this.Refresh();
            }
        }

        private Color backColor=SystemColors.Control;
        [Browsable(true),DefaultValue(typeof(SystemColors), "Control")]
        public new Color BackColor {
            get { return backColor; }
            set {
                base.BackColor = Color.Transparent;
                backColor = value;
                this.Refresh();
            }
        }

        private Color borderColor = SystemColors.Highlight;
        [Browsable(true), DefaultValue(typeof(SystemColors), "Highlight"),Description("边框颜色")]
        public Color BorderColor {
            get { return borderColor; }
            set {
                borderColor = value;
                this.Refresh();
            }
        }

        private int borderThickness = 1;
        [Browsable(true), DefaultValue(1),Description("边框厚度")]
        public int BorderThickness {
            get { return borderThickness; }
            set {
                borderThickness = value;
                if (borderThickness < 0)
                    borderThickness = 0;
                else if (borderThickness > Math.Min(this.Width, this.Height) / 5) {
                    borderThickness = Math.Min(this.Width, this.Height) / 5;
                }
                this.Refresh();
            }
        }

        private bool drawBorder = true;
        [Browsable(true), DefaultValue(true), Description("是否绘制边框")]
        public bool DrawBorder {
            get { return drawBorder; }
            set { 
                drawBorder = value;
                this.Refresh();
            }
        }

        private void InitializeControlStyle() {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        protected override void OnPaint(PaintEventArgs e) {
            GraphicsPath path = Util.GetRoundedRectPath(new RectangleF(borderThickness / 2f, borderThickness / 2f, this.Width - borderThickness - 1F, this.Height - borderThickness - 1F), radiu);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillPath(new SolidBrush(backColor), path);
            if (drawBorder) {
                Pen pen = new Pen(new SolidBrush(borderColor), borderThickness);
                g.DrawPath(pen, path);
                pen.Dispose();
            }
            path.Dispose();
        }

        private void LxzhPanel_SizeChanged(object sender, EventArgs e) {
            this.Refresh();
        }
    }
}
