using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace lxzh
{
    public partial class ImageProcessBox : Control
    {

        #region Member variable

        private bool bMouseEnter;
        private bool bLockH;
        private bool bLockW;
        private Point ptOriginal;
        private Point ptCurrent;
        private Point ptTempStarPos;
        private Rectangle[] rectDots;
        private Rectangle[] rectBoarders;
        private Rectangle rectClip;

        private Bitmap bmpDark;
        private Pen pen;
        private SolidBrush brush;
        private int dotRadiu = 5;

        #endregion

        #region Properties

        private Image baseImage;
        /// <summary>
        /// 获取或设置用于被操作的图像
        /// </summary>
        [Category("Custom"), Description("获取或设置用于被操作的图像")]
        public Image BaseImage {
            get { return baseImage; }
            set {
                baseImage = value;
                this.BuildBitmap();
            }
        }

        private Color dotColor;
        /// <summary>
        /// 获取或设置操作框点的颜色
        /// </summary>
        [Description("获取或设置操作框点的颜色")]
        [DefaultValue(typeof(Color), "Yellow"), Category("Custom")]
        public Color DotColor {
            get { return dotColor; }
            set { dotColor = value; }
        }

        private Color lineColor;
        /// <summary>
        /// 获取或设置操作框线条的颜色
        /// </summary>
        [Description("获取或设置操作框线条的颜色")]
        [DefaultValue(typeof(Color), "Cyan"), Category("Custom")]
        public Color LineColor {
            get { return lineColor; }
            set { lineColor = value; }
        }

        private Rectangle selectedRect;
        /// <summary>
        /// 获取当前选中的区域
        /// </summary>
        [Browsable(false)]
        public Rectangle SelectedRect {
            get {
                Rectangle rectTemp = selectedRect;
                rectTemp.Width++; rectTemp.Height++;
                return rectTemp;
            }
            set { selectedRect = value; }
        }

        private Size magnifySize;
        /// <summary>
        /// 获取或设置放大图像的原图大小尺寸
        /// </summary>
        [Description("获取或设置放大图像的原图大小尺寸")]
        [DefaultValue(typeof(Size), "15,15"), Category("Custom")]
        public Size MagnifySize {
            get { return magnifySize; }
            set {
                magnifySize = value;
                if (magnifySize.Width < 5) magnifySize.Width = 5;
                if (magnifySize.Width > 20) magnifySize.Width = 20;
                if (magnifySize.Height < 5) magnifySize.Height = 5;
                if (magnifySize.Height > 20) magnifySize.Height = 20;
            }
        }

        private int magnifyTimes;
        /// <summary>
        /// 获取或设置图像放大的倍数
        /// </summary>
        [Description("获取或设置图像放大的倍数")]
        [DefaultValue(7), Category("Custom")]
        public int MagnifyTimes {
            get { return magnifyTimes; }
            set {
                magnifyTimes = value;
                if (magnifyTimes < 3) magnifyTimes = 3;
                if (magnifyTimes > 10) magnifyTimes = 10;
            }
        }

        private bool isDrawOperationDot;
        /// <summary>
        /// 获取或设置是否绘制操作框点
        /// </summary>
        [Description("获取或设置是否绘制操作框点")]
        [DefaultValue(true), Category("Custom")]
        public bool IsDrawOperationDot {
            get { return isDrawOperationDot; }
            set {
                if (value == isDrawOperationDot) return;
                isDrawOperationDot = value;
                this.Invalidate();
            }
        }

        private bool isSetClip;
        /// <summary>
        /// 获取或设置是否限制鼠标操作区域
        /// </summary>
        [Description("获取或设置是否限制鼠标操作区域")]
        [DefaultValue(true), Category("Custom")]
        public bool IsSetClip {
            get { return isSetClip; }
            set { isSetClip = value; }
        }

        private bool isShowInfo;
        /// <summary>
        /// 获取或设置是否绘制信息展示
        /// </summary>
        [Description("获取或设置是否绘制信息展示")]
        [DefaultValue(true), Category("Custom")]
        public bool IsShowInfo {
            get { return isShowInfo; }
            set { isShowInfo = value; }
        }

        private bool autoSizeFromImage;
        /// <summary>
        /// 获取或设置是否根据图像大小自动调整控件尺寸
        /// </summary>
        [Description("获取或设置是否根据图像大小自动调整控件尺寸")]
        [DefaultValue(true), Category("Custom")]
        public bool AutoSizeFromImage {
            get { return autoSizeFromImage; }
            set {
                if (value && this.baseImage != null) {
                    this.Width = this.baseImage.Width;
                    this.Height = this.baseImage.Height;
                }
                autoSizeFromImage = value;
            }
        }

        private bool isDrawed;
        /// <summary>
        /// 获取当前是否绘制的有区域
        /// </summary>
        [Browsable(false)]
        public bool IsDrawed {
            get { return isDrawed; }
        }

        private bool isStartDraw;
        /// <summary>
        /// 获取当前是否开始绘制
        /// </summary>
        [Browsable(false)]
        public bool IsStartDraw {
            get { return isStartDraw; }
        }

        private bool isMoving;
        /// <summary>
        /// 获取当前操作框是否正在移动
        /// </summary>
        [Browsable(false)]
        public bool IsMoving {
            get { return isMoving; }
        }

        private bool canReset;
        /// <summary>
        /// 获取或设置操作框是否锁定
        /// </summary>
        [Browsable(false)]
        public bool CanReset {
            get { return canReset; }
            set {
                canReset = value;
                if (!canReset) this.Cursor = Cursors.Default;
            }
        }
        #endregion

        public ImageProcessBox() {
            InitializeComponent();
            InitMember();

            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        private void InitMember() {
            this.dotColor = Color.Yellow;
            this.lineColor = Color.Cyan;
            this.magnifySize = new Size(15, 15);
            this.magnifyTimes = 7;
            this.isDrawOperationDot = true;
            this.isSetClip = true;
            this.isShowInfo = true;
            this.autoSizeFromImage = true;
            this.canReset = true;
            pen = new Pen(lineColor, 1);
            brush = new SolidBrush(dotColor);
            this.selectedRect = new Rectangle();
            this.ClearDraw();
            dotRadiu = 4;
            int diameter = 2 * dotRadiu + 1;
            rectDots = new Rectangle[8];
            rectBoarders = new Rectangle[8];
            for (int i = 0; i < 8; i++) {
                //rectDots[i] = m_rectBoarders[i] = new Rectangle(-10, -10, 5, 5);
                rectDots[i] = rectBoarders[i] = new Rectangle(-diameter, -diameter, diameter, diameter);
            }
        }
        //貌似析构函数不执行
        ~ImageProcessBox() {
            pen.Dispose();
            brush.Dispose();
            if (this.baseImage != null) {
                bmpDark.Dispose();
                this.baseImage.Dispose();
            }
        }

        public void DeleResource() {
            pen.Dispose();
            brush.Dispose();
            if (this.baseImage != null) {
                bmpDark.Dispose();
                this.baseImage.Dispose();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {        //根据情况是否开始绘制操作框
                if (!this.IsDrawed || this.Cursor != Cursors.Default) {
                    rectClip = this.DisplayRectangle;
                    if (this.baseImage != null) {
                        if (this.isSetClip) {
                            if (e.X > this.baseImage.Width || e.Y > this.baseImage.Height) return;
                            rectClip.Intersect(new Rectangle(0, 0, this.baseImage.Width, this.baseImage.Height));
                        }
                    }
                    Cursor.Clip = RectangleToScreen(rectClip);
                    isStartDraw = true;
                    ptOriginal = e.Location;
                }
            }
            this.Focus();
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            ptCurrent = e.Location;
            bMouseEnter = true;

            #region Process OperationBox

            if (isDrawed && this.canReset) {        //如果已经绘制 并且可以操作选区 判断操作类型
                this.SetCursorStyle(e.Location);
                if (isStartDraw && this.isDrawOperationDot) {
                    isDrawed = false;
                    if (rectBoarders[0].Contains(e.Location)) {
                        ptOriginal.X = this.selectedRect.Right;
                        ptOriginal.Y = this.selectedRect.Bottom;
                    } else if (rectBoarders[1].Contains(e.Location)) {
                        ptOriginal.Y = this.selectedRect.Bottom;
                        bLockW = true;
                    } else if (rectBoarders[2].Contains(e.Location)) {
                        ptOriginal.X = this.selectedRect.X;
                        ptOriginal.Y = this.selectedRect.Bottom;
                    }else if (rectBoarders[3].Contains(e.Location)) {
                        ptOriginal.X = this.selectedRect.Right;
                        bLockH = true;
                    } else if (rectBoarders[4].Contains(e.Location)) {
                        ptOriginal.X = this.selectedRect.X;
                        bLockH = true;
                    } else if (rectBoarders[5].Contains(e.Location)) {
                        ptOriginal.X = this.selectedRect.Right;
                        ptOriginal.Y = this.selectedRect.Y;
                    } else if (rectBoarders[6].Contains(e.Location)) {
                        ptOriginal.Y = this.selectedRect.Y;
                        bLockW = true;
                    } else if (rectBoarders[7].Contains(e.Location)) {
                        ptOriginal = this.selectedRect.Location;
                    } else if (this.selectedRect.Contains(e.Location)) {
                        isMoving = true;
                    }
                }
                base.OnMouseMove(e);
                return;
            }

            #endregion

            #region Calculate the operationbox

            if (isStartDraw) {
                if (isMoving) {     //如果移动选区 只重置 location
                    //Point ptLast = this.selectedRectangle.Location;
                    this.selectedRect.X = ptTempStarPos.X + e.X - ptOriginal.X;
                    this.selectedRect.Y = ptTempStarPos.Y + e.Y - ptOriginal.Y;
                    if (this.selectedRect.X < 0) this.selectedRect.X = 0;
                    if (this.selectedRect.Y < 0) this.selectedRect.Y = 0;
                    if (this.selectedRect.Right > rectClip.Width) this.selectedRect.X = rectClip.Width - this.selectedRect.Width - 1;
                    if (this.selectedRect.Bottom > rectClip.Height) this.selectedRect.Y = rectClip.Height - this.selectedRect.Height - 1;
                } else {            //其他情况 判断是锁定高 还是锁定宽
                    if (Math.Abs(e.X - ptOriginal.X) > 1 || Math.Abs(e.Y - ptOriginal.Y) > 1) {
                        if (!bLockW) {
                            selectedRect.X = ptOriginal.X - e.X < 0 ? ptOriginal.X : e.X;
                            selectedRect.Width = Math.Abs(ptOriginal.X - e.X);
                        }
                        if (!bLockH) {
                            selectedRect.Y = ptOriginal.Y - e.Y < 0 ? ptOriginal.Y : e.Y;
                            selectedRect.Height = Math.Abs(ptOriginal.Y - e.Y);
                        }
                    }
                }
                this.Invalidate();
            }

            #endregion
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e) {
            bMouseEnter = false;
            this.Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {        //如果绘制太小 则视为无效
                if (this.selectedRect.Width >= 4 && this.selectedRect.Height >= 4)
                    isDrawed = true;
                else
                    this.ClearDraw();
                isMoving = bLockH = bLockW = false; //取消锁定
                isStartDraw = false;
                ptTempStarPos = this.selectedRect.Location;
                Cursor.Clip = new Rectangle();
            } //else if (e.Button == MouseButtons.Right)
                //this.ClearDraw();
            this.Invalidate();
            base.OnMouseUp(e);
        }

        private void ImageProcessBox_KeyPress(object sender, KeyPressEventArgs e) {
            Console.WriteLine("OnKeyPress:" + e.KeyChar + "");
        }

        protected override void OnKeyPress(KeyPressEventArgs e){
            Console.WriteLine("override OnKeyPress:" + e.KeyChar + "");
            if (e.KeyChar == 'w')
                Win32.SetCursorPos(MousePosition.X, MousePosition.Y - 1);
            else if (e.KeyChar == 's')
                Win32.SetCursorPos(MousePosition.X, MousePosition.Y + 1);
            else if (e.KeyChar == 'a')
                Win32.SetCursorPos(MousePosition.X - 1, MousePosition.Y);
            else if (e.KeyChar == 'd')
                Win32.SetCursorPos(MousePosition.X + 1, MousePosition.Y);
            base.OnKeyPress(e);
        }

        protected override void OnPaint(PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            if (this.baseImage != null) {
                g.DrawImage(bmpDark, 0, 0);
                g.DrawImage(this.baseImage, this.selectedRect, this.selectedRect, GraphicsUnit.Pixel);
            }
            this.DrawOperationBox(g);
            if (this.baseImage != null && !isDrawed && !isMoving && bMouseEnter && isShowInfo)
                DrawInfo(e.Graphics);
            base.OnPaint(e);
        }
        //绘制操作框
        protected virtual void DrawOperationBox(Graphics g) {

            #region Draw SizeInfo

            string strDrawSize = "(" + this.selectedRect.X + "," + this.selectedRect.Y +
                ") " + (this.selectedRect.Width + 1) + " x " + (this.selectedRect.Height + 1);
            Size seStr = TextRenderer.MeasureText(strDrawSize, this.Font);
            int tempX = this.selectedRect.X;
            int tempY = this.selectedRect.Y - seStr.Height - 3;
            if (!rectClip.IsEmpty)
                if (tempX + seStr.Width >= rectClip.Right) tempX = rectClip.Right-seStr.Width;
            if (tempY <= 0) tempY += seStr.Height + 6;

            brush.Color = Color.FromArgb(250, 40, 40, 40);
            g.FillRectangle(brush, tempX, tempY, seStr.Width, seStr.Height);
            brush.Color = this.ForeColor;
            g.DrawString(strDrawSize, this.Font, brush, tempX, tempY);

            #endregion

            if (!this.isDrawOperationDot) {
                pen.Width = 5; pen.Color = this.lineColor;
                g.DrawRectangle(pen, this.selectedRect);
                return;
            }
            //计算八个顶点位置
            //rectDots[0].Y = rectDots[1].Y = rectDots[2].Y = this.selectedRectangle.Y - 2;
            //rectDots[5].Y = rectDots[6].Y = rectDots[7].Y = this.selectedRectangle.Bottom - 3;
            //rectDots[0].X = rectDots[3].X = rectDots[5].X = this.selectedRectangle.X - 2;
            //rectDots[2].X = rectDots[4].X = rectDots[7].X = this.selectedRectangle.Right - 3;
            //rectDots[3].Y = rectDots[4].Y = this.selectedRectangle.Y + this.selectedRectangle.Height / 2 - 2;
            //rectDots[1].X = rectDots[6].X = this.selectedRectangle.X + this.selectedRectangle.Width / 2 - 2;
            rectDots[0].Y = rectDots[1].Y = rectDots[2].Y = this.selectedRect.Y - dotRadiu;
            rectDots[5].Y = rectDots[6].Y = rectDots[7].Y = this.selectedRect.Bottom - dotRadiu - 1;
            rectDots[0].X = rectDots[3].X = rectDots[5].X = this.selectedRect.X - dotRadiu;
            rectDots[2].X = rectDots[4].X = rectDots[7].X = this.selectedRect.Right - dotRadiu - 1;
            rectDots[3].Y = rectDots[4].Y = this.selectedRect.Y + this.selectedRect.Height / 2 - dotRadiu;
            rectDots[1].X = rectDots[6].X = this.selectedRect.X + this.selectedRect.Width / 2 - dotRadiu;
            //计算八个边界矩形框
            rectBoarders[0].Y = rectBoarders[1].Y = rectBoarders[2].Y = this.selectedRect.Y - dotRadiu;
            rectBoarders[5].Y = rectBoarders[6].Y = rectBoarders[7].Y = this.selectedRect.Bottom - dotRadiu - 1;
            rectBoarders[0].X = rectBoarders[3].X = rectBoarders[5].X = this.selectedRect.X - dotRadiu;
            rectBoarders[2].X = rectBoarders[4].X = rectBoarders[7].X = this.selectedRect.Right - dotRadiu - 1;
            rectBoarders[3].Y = rectBoarders[4].Y = this.selectedRect.Y + dotRadiu + 1;
            rectBoarders[1].X = rectBoarders[6].X = this.selectedRect.X + dotRadiu - 1;
            rectBoarders[1].Width = rectBoarders[6].Width = this.selectedRect.Width - 2 * (dotRadiu - 1);
            rectBoarders[3].Height = rectBoarders[4].Height = this.selectedRect.Height - 2 * (dotRadiu - 1);

            pen.Width = 1; pen.Color = this.lineColor;
            g.DrawRectangle(pen, this.selectedRect);
            brush.Color = this.dotColor;
            foreach (Rectangle rect in rectDots) {
                g.FillEllipse(brush, rect);
                //g.FillRectangle(m_sb, rect);
            }
            //g.DrawEllipse(m_pen, 100, 100, 12, 12);
            if (this.selectedRect.Width <= 10 || this.selectedRect.Height <= 10)
                g.DrawRectangle(pen, this.selectedRect);
        }
        //绘制图像放大信息
        protected virtual void DrawInfo(Graphics g) {

            #region Calculate point

            int tempX = ptCurrent.X + 20;
            int tempY = ptCurrent.Y + 20;
            int tempW = this.magnifySize.Width * this.magnifyTimes + 8;
            int tempH = this.magnifySize.Width * this.magnifyTimes + 12 + this.Font.Height * 3;
            if (!rectClip.IsEmpty) {
                if (tempX + tempW >= this.rectClip.Right) tempX -= tempW + 30;
                if (tempY + tempH >= this.rectClip.Bottom) tempY -= tempH + 30;
            } else {
                if (tempX + tempW >= this.ClientRectangle.Width) tempX -= tempW + 30;
                if (tempY + tempH >= this.ClientRectangle.Height) tempY -= tempH + 30;
            }
            Rectangle tempRectBorder = new Rectangle(tempX + 2, tempY + 2, tempW - 4, this.magnifySize.Width * this.magnifyTimes + 4);

            #endregion

            brush.Color = Color.FromArgb(200, 0, 0, 0);
            g.FillRectangle(brush, tempX, tempY, tempW, tempH);
            pen.Width = 2; pen.Color = Color.White;
            g.DrawRectangle(pen, tempRectBorder);

            #region Draw the magnified image

            using (Bitmap bmpSrc = new Bitmap(this.magnifySize.Width, this.magnifySize.Height, PixelFormat.Format32bppArgb)) {
                using (Graphics gp = Graphics.FromImage(bmpSrc)) {
                    gp.DrawImage(this.baseImage, -(ptCurrent.X - this.magnifySize.Width / 2), -(ptCurrent.Y - this.magnifySize.Height / 2));
                }
                using (Bitmap bmpInfo = ImageProcessBox.MagnifyImage(bmpSrc, this.magnifyTimes)) {
                    g.DrawImage(bmpInfo, tempX + 4, tempY + 4);
                }
            }

            #endregion

            pen.Width = this.magnifyTimes - 2;
            pen.Color = Color.FromArgb(125, 0, 255, 255);
            int tempCenterX = tempX + (tempW + (this.magnifySize.Width % 2 == 0 ? this.magnifyTimes : 0)) / 2;
            int tempCenterY = tempY + 2 + (tempRectBorder.Height + (this.MagnifySize.Height % 2 == 0 ? this.magnifyTimes : 0)) / 2;
            g.DrawLine(pen, tempCenterX, tempY + 4, tempCenterX, tempRectBorder.Bottom - 2);
            g.DrawLine(pen, tempX + 4, tempCenterY, tempX + tempW - 4, tempCenterY);

            #region Draw Info

            brush.Color = this.ForeColor;
            Color clr = ((Bitmap)this.baseImage).GetPixel(ptCurrent.X, ptCurrent.Y);
            g.DrawString((this.selectedRect.Width + 1) + " x "+ (this.selectedRect.Height + 1),
                this.Font, brush, tempX + 2, tempRectBorder.Bottom + 2);
            g.DrawString("RGB:(" + clr.R + "," + clr.G + "," + clr.B + ")",
                this.Font, brush, tempX + 2, tempRectBorder.Bottom + 2 + this.Font.Height);
            g.DrawString("0x" + 
                clr.R.ToString("X").PadLeft(2, '0') +
                clr.G.ToString("X").PadLeft(2, '0') +
                clr.B.ToString("X").PadLeft(2, '0'),
                this.Font, brush, tempX + 2, tempRectBorder.Bottom + 2 + this.Font.Height * 2);
            brush.Color = clr;
            g.FillRectangle(brush, tempX + tempW - 2 - this.Font.Height,         //右下角颜色
                tempY + tempH - 2 - this.Font.Height,
                this.Font.Height,
                this.Font.Height);
            g.DrawRectangle(Pens.Cyan, tempX + tempW - 2 - this.Font.Height,    //右下角颜色边框
                tempY + tempH - 2 - this.Font.Height,
                this.Font.Height,
                this.Font.Height);
            g.FillRectangle(brush, tempCenterX - this.magnifyTimes / 2,          //十字架中间颜色
                tempCenterY - this.magnifyTimes / 2,
                this.magnifyTimes,
                this.magnifyTimes);
            g.DrawRectangle(Pens.Cyan, tempCenterX - this.magnifyTimes / 2,     //十字架中间边框
                tempCenterY - this.magnifyTimes / 2,
                this.magnifyTimes - 1,
                this.magnifyTimes - 1);

            #endregion
        }
        //放大图形
        private static Bitmap MagnifyImage(Bitmap bmpSrc, int times) {
            Bitmap bmpNew = new Bitmap(bmpSrc.Width * times, bmpSrc.Height * times, PixelFormat.Format32bppArgb);
            BitmapData bmpSrcData = bmpSrc.LockBits(new Rectangle(0, 0, bmpSrc.Width, bmpSrc.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData bmpNewData = bmpNew.LockBits(new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            byte[] bySrcData = new byte[bmpSrcData.Height * bmpSrcData.Stride];
            Marshal.Copy(bmpSrcData.Scan0, bySrcData, 0, bySrcData.Length);
            byte[] byNewData = new byte[bmpNewData.Height * bmpNewData.Stride];
            Marshal.Copy(bmpNewData.Scan0, byNewData, 0, byNewData.Length);
            for (int y = 0, lenY = bmpSrc.Height; y < lenY; y++) {
                for (int x = 0, lenX = bmpSrc.Width; x < lenX; x++) {
                    for (int cy = 0; cy < times; cy++) {
                        for (int cx = 0; cx < times; cx++) {
                            byNewData[(x * times + cx) * 4 + ((y * times + cy) * bmpNewData.Stride)] = bySrcData[x * 4 + y * bmpSrcData.Stride];
                            byNewData[(x * times + cx) * 4 + ((y * times + cy) * bmpNewData.Stride) + 1] = bySrcData[x * 4 + y * bmpSrcData.Stride + 1];
                            byNewData[(x * times + cx) * 4 + ((y * times + cy) * bmpNewData.Stride) + 2] = bySrcData[x * 4 + y * bmpSrcData.Stride + 2];
                            byNewData[(x * times + cx) * 4 + ((y * times + cy) * bmpNewData.Stride) + 3] = bySrcData[x * 4 + y * bmpSrcData.Stride + 3];
                        }
                    }
                }
            }
            Marshal.Copy(byNewData, 0, bmpNewData.Scan0, byNewData.Length);
            bmpSrc.UnlockBits(bmpSrcData);
            bmpNew.UnlockBits(bmpNewData);
            return bmpNew;
        }
        //设置鼠标指针样式
        private void SetCursorStyle(Point loc) {
            if (rectBoarders[0].Contains(loc) || rectBoarders[7].Contains(loc))
                this.Cursor = Cursors.SizeNWSE;
            else if (rectBoarders[1].Contains(loc) || rectBoarders[6].Contains(loc))
                this.Cursor = Cursors.SizeNS;
            else if (rectBoarders[2].Contains(loc) || rectBoarders[5].Contains(loc))
                this.Cursor = Cursors.SizeNESW;
            else if (rectBoarders[3].Contains(loc) || rectBoarders[4].Contains(loc))
                this.Cursor = Cursors.SizeWE;
            else if (this.selectedRect.Contains(loc) /*&& this.canReset*/)
                this.Cursor = Cursors.SizeAll;
            else
                this.Cursor = Cursors.Default;
        }

        private void BuildBitmap() {
            if (this.baseImage == null) return;
            bmpDark = new Bitmap(this.baseImage);
            using (Graphics g = Graphics.FromImage(bmpDark)) {
                SolidBrush sb = new SolidBrush(Color.FromArgb(125, 0, 0, 0));
                g.FillRectangle(sb, 0, 0, bmpDark.Width, bmpDark.Height);
                sb.Dispose();
            }
        }
        /// <summary>
        /// 清空所有操作
        /// </summary>
        public void ClearDraw() {
            isDrawed = false;
            this.selectedRect.X = this.selectedRect.Y = -100;
            this.selectedRect.Width = this.selectedRect.Height = 0;
            this.Cursor = Cursors.Default;
            this.Invalidate();
        }
        /// <summary>
        /// 手动设置一个块选中区域
        /// </summary>
        /// <param name="rect">要选中区域</param>
        public void SetSelectRect(Rectangle rect) {
            rect.Intersect(this.DisplayRectangle);
            if (rect.IsEmpty) return;
            rect.Width--; rect.Height--;
            if (this.selectedRect == rect) return;
            this.selectedRect = rect;
            this.Invalidate();
        }
        /// <summary>
        /// 手动设置一个块选中区域
        /// </summary>
        /// <param name="pt">要选中区域的坐标</param>
        /// <param name="se">要选中区域的大小</param>
        public void SetSelectRect(Point pt, Size se) {
            Rectangle rectTemp = new Rectangle(pt, se);
            rectTemp.Intersect(this.DisplayRectangle);
            if (rectTemp.IsEmpty) return;
            rectTemp.Width--; rectTemp.Height--;
            if (this.selectedRect == rectTemp) return;
            this.selectedRect = rectTemp;
            this.Invalidate();
        }
        /// <summary>
        /// 手动设置一个块选中区域
        /// </summary>
        /// <param name="x">要选中区域的x坐标</param>
        /// <param name="y">要选中区域的y坐标</param>
        /// <param name="w">要选中区域的宽度</param>
        /// <param name="h">要选中区域的高度</param>
        public void SetSelectRect(int x, int y, int w, int h) {
            Rectangle rectTemp = new Rectangle(x, y, w, h);
            rectTemp.Intersect(this.DisplayRectangle);
            if (rectTemp.IsEmpty) return;
            rectTemp.Width--; rectTemp.Height--;
            if (this.selectedRect == rectTemp) return;
            this.selectedRect = rectTemp;
            this.Invalidate();
        }
        /// <summary>
        /// 手动设置信息显示的位置
        /// </summary>
        /// <param name="pt">要显示的位置</param>
        public void SetInfoPoint(Point pt) {
            if (ptCurrent == pt) return;
            ptCurrent = pt;
            bMouseEnter = true;
            this.Invalidate();
        }
        /// <summary>
        /// 手动设置信息显示的位置
        /// </summary>
        /// <param name="x">要显示位置的x坐标</param>
        /// <param name="y">要显示位置的y坐标</param>
        public void SetInfoPoint(int x, int y) {
            if (ptCurrent.X == x && ptCurrent.Y == y) return;
            ptCurrent.X = x;
            ptCurrent.Y = y;
            bMouseEnter = true;
            this.Invalidate();
        }
        /// <summary>
        /// 获取操作框内的图像
        /// </summary>
        /// <returns>结果图像</returns>
        public Bitmap GetResultBmp() {
            if (this.baseImage == null) return null;
            Bitmap bmp = new Bitmap(this.selectedRect.Width + 1, this.selectedRect.Height + 1);
            using (Graphics g = Graphics.FromImage(bmp)) {
                g.DrawImage(this.baseImage, -this.selectedRect.X, -this.selectedRect.Y);
            }
            return bmp;
        }
    }
}
