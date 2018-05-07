using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace lxzh
{
    public partial class CaptureForm : Form
    {

        private MouseHook mHook;
        private List<Bitmap> historyLayer;       //记录历史图层

        private bool isStartDraw;
        private Point ptOriginal;
        private Point ptCurrent;
        private Bitmap bmpLayerCurrent;
        private Bitmap bmpLayerShow;
        private Cursor crossCursor;
        public static bool isAlive = false;

        private static int maxHeight = 0;
        private static int maxWidth = 0;

        #region Properties

        private bool isCaptureCursor;
        /// <summary>
        /// 获取或设置是否捕获鼠标
        /// </summary>
        public bool IsCaptureCursor {
            get { return isCaptureCursor; }
            set { isCaptureCursor = value; }
        }
        private bool isFromClipBoard;
        /// <summary>
        /// 获取或设置是否从剪切板获取图像
        /// </summary>
        public bool IsFromClipBoard {
            get { return isFromClipBoard; }
            set { isFromClipBoard = value; }
        }
        /// <summary>
        /// 获取或设置是否显示图像信息
        /// </summary>
        public bool ImgProcessBoxIsShowInfo {
            get { return imageProcessBox.IsShowInfo; }
            set { imageProcessBox.IsShowInfo = value; }
        }
        /// <summary>
        /// 获取或设置操作框点的颜色
        /// </summary>
        public Color ImgProcessBoxDotColor {
            get { return imageProcessBox.DotColor; }
            set { imageProcessBox.DotColor = value; }
        }
        /// <summary>
        /// 获取或设置操作框边框颜色
        /// </summary>
        public Color ImgProcessBoxLineColor {
            get { return imageProcessBox.LineColor; }
            set { imageProcessBox.LineColor = value; }
        }
        /// <summary>
        /// 获取或设置放大图形的原始尺寸
        /// </summary>
        public Size ImgProcessBoxMagnifySize {
            get { return imageProcessBox.MagnifySize; }
            set { imageProcessBox.MagnifySize = value; }
        }
        /// <summary>
        /// 获取或设置放大图像的倍数
        /// </summary>
        public int ImgProcessBoxMagnifyTimes {
            get { return imageProcessBox.MagnifyTimes; }
            set { imageProcessBox.MagnifyTimes = value; }
        }

        #endregion
        public CaptureForm() {
            InitializeComponent();
            this.Location = new Point(0, 0);

            //获取屏幕放大比例
            //Graphics graphics = this.CreateGraphics(); 
            //int dpiX = (int)graphics.DpiX;
            //int dpiY = (int)graphics.DpiY;
            //float scaling=1.0f;
            //if(dpiX==96){
            //    scaling=1.0F;
            //}else if(dpiX==120){
            //    scaling=1.25F;
            //}else if(dpiX==144){
            //    scaling=1.5F;
            //}else if(dpiX==192){
            //    scaling=2.0F;
            //}else{

            //}

            foreach (Screen screen in Screen.AllScreens) {
                maxWidth += screen.Bounds.Width;
                if (screen.Bounds.Height > maxHeight) {
                    maxHeight = screen.Bounds.Height;
                }
            }
            this.Location = Point.Empty;
            this.Size = new Size(maxWidth, maxHeight);

            mHook = new MouseHook();
            this.FormClosing += (s, e) => { mHook.UnLoadHook(); this.DelResource(); };
            imageProcessBox.MouseLeave += (s, e) => this.Cursor = Cursors.Default;
            //后期一些操作历史记录图层
            historyLayer = new List<Bitmap>();
            List<String> items = new List<string>();
            items.AddRange(new String[] { "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22" });
            this.fontBox.SetItems(items);
        }

        private void DelResource() {
            if (bmpLayerCurrent != null) 
                bmpLayerCurrent.Dispose();
            if (bmpLayerShow != null) 
                bmpLayerShow.Dispose();
            historyLayer.Clear();
            imageProcessBox.DeleResource();
            GC.Collect();
        }

        //初始化参数
        private void InitMember() {
            plTool.Height = btnFinish.Bottom + 3;
            plTool.Width = btnFinish.Right + 3;
            plColorBox.Height = colorBox.Height;
            plTool.Paint += (s, e) => e.Graphics.DrawRectangle(Pens.SteelBlue, 0, 0, plTool.Width - 1, plTool.Height - 1);
            plColorBox.Paint += (s, e) => e.Graphics.DrawRectangle(Pens.SteelBlue, 0, 0, plColorBox.Width - 1, plColorBox.Height - 1);

            btnRect.Click += new EventHandler(selectToolButton_Click);
            btnEllipse.Click += new EventHandler(selectToolButton_Click);
            btnArrow.Click += new EventHandler(selectToolButton_Click);
            btnBrush.Click += new EventHandler(selectToolButton_Click);
            btnText.Click += new EventHandler(selectToolButton_Click);
            btnClose.Click += (s, e) => this.Close();
            fontBox.ValueChanged += new EventHandler(fontBox_ValueChanged);
            colorBox.ColorChanged += (s, e) => txtDrawText.ForeColor = e.Color;
        }

        private void CaptureForm_Load(object sender, EventArgs e) {
            this.InitMember();

            imageProcessBox.BaseImage = GetScreen(this.isCaptureCursor,this.isFromClipBoard);
            mHook.SetHook();
            mHook.MHookEvent += new MouseHook.MHookEventHandler(mHook_MHookEvent);
            imageProcessBox.IsDrawOperationDot = false;
            this.BeginInvoke(new MethodInvoker(() => this.Enabled = false));
            //
            //crossCursor = new Cursor(Cursor.Current.Handle);
            //IntPtr customCursorHandle = Win32.LoadCursorFromFile("cross.cur");
            //crossCursor.GetType().InvokeMember("handle", BindingFlags.Public |
            //BindingFlags.NonPublic | BindingFlags.Instance |
            //BindingFlags.SetField, null, crossCursor,
            //new object[] { customCursorHandle });
            crossCursor = new Cursor(global::lxzh.Properties.Resources.cross.GetHicon());
            isAlive = true;
            //timer1.Interval = 500;
            //timer1.Enabled = true;
        }

        private void mHook_MHookEvent(object sender, MHookEventArgs e) {
            //如果窗体禁用 调用控件的方法设置信息显示位置
            if (!this.Enabled)      //貌似Hook不能精确坐标(Hook最先执行,执行完后的坐标可能与执行时传入的坐标发生了变化 猜测是这样) 所以放置了一个timer检测
                imageProcessBox.SetInfoPoint(MousePosition.X, MousePosition.Y);
            //鼠标点下恢复窗体禁用
            if (e.MButton == ButtonStatus.LeftDown || e.MButton == ButtonStatus.RightDown) {
                this.Enabled = true;
                imageProcessBox.IsDrawOperationDot = true;
            }
            #region 右键抬起

            if (e.MButton == ButtonStatus.RightUp) {
                if (!imageProcessBox.IsDrawed) //没有绘制那么退出(直接this.Close右键将传递到下面)
                    this.BeginInvoke(new MethodInvoker(() => this.Close()));
            }
            #endregion

            #region 找寻窗体

            if (!this.Enabled)
                this.FoundAndDrawWindowRect();
            #endregion
        }
        /// <summary>
        /// 将控件捕获的方向键点击事件传到窗体
        /// 需要将窗体的KeyPreview属性设置为true
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessDialogKey(Keys keyData) {
            if (keyData == Keys.Up || keyData == Keys.Down ||
                keyData == Keys.Left || keyData == Keys.Right ||
                keyData == (Keys.Control | Keys.S) || keyData == (Keys.Control | Keys.Z)||keyData==Keys.Escape)
                return false;
            else
                return base.ProcessDialogKey(keyData);
        }

        /// <summary>
        /// 窗体接收到的按键点击事件
        /// 处理方向键事件，移动选框位置
        /// </summary>
        private void CaptureForm_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyValue >= 37 && e.KeyValue <= 40 && !txtDrawText.Visible) {
                Rectangle rect = imageProcessBox.SelectedRect;
                rect.Width -= 1;
                rect.Height -= 1;
                switch (e.KeyData) {
                    case Keys.Left:
                        if (rect.X > 0) {
                            rect.X -= 1;
                            Win32.SetCursorPos(MousePosition.X - 1, MousePosition.Y);
                        }
                        break;
                    case Keys.Up:
                        if (rect.Y > 0) {
                            rect.Y -= 1;
                            Win32.SetCursorPos(MousePosition.X, MousePosition.Y - 1);
                        }
                        break;
                    case Keys.Right:
                        if (rect.Right < this.Width) {
                            rect.X += 1;
                            Win32.SetCursorPos(MousePosition.X + 1, MousePosition.Y);
                        }
                        break;
                    case Keys.Down:
                        if (rect.Bottom < this.Height) {
                            rect.Y += 1;
                            Win32.SetCursorPos(MousePosition.X, MousePosition.Y + 1);
                        }
                        break;
                }
                imageProcessBox.SelectedRect = rect;
                SetToolBarLocation();
                this.Refresh();
            }
            switch (e.KeyData) {
                case Keys.Control | Keys.S:
                    btnSave_Click(sender, e);
                    break;
                case Keys.Control | Keys.Z:
                    btnReset_Click(sender, e);
                    break;
                case Keys.Escape:
                    this.Close();
                    break;
            }
            //base.OnKeyDown(e);
        }

        //工具条前五个按钮绑定的公共事件
        private void selectToolButton_Click(object sender, EventArgs e) {
            LxzhToolButton nowButton = sender as LxzhToolButton;
            plColorBox.Visible = nowButton.IsSelected;
            tBtn_TextFont.Visible = fontBox.Visible = nowButton.Equals(btnText);
            if (plColorBox.Visible) 
                imageProcessBox.CanReset = false;
            else { 
                imageProcessBox.CanReset = historyLayer.Count == 0; 
            }
            this.SetToolBarLocation();
        }

        private void fontBox_ValueChanged(object sender, EventArgs e) {
            int size=10;
            if (!string.IsNullOrEmpty(fontBox.Text)) {
                try {
                    size=Convert.ToInt32(fontBox.Text);
                } catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                }
            }
            Font font=txtDrawText.Font;
            FontFamily fontF=font.FontFamily;
            FontStyle fontS = font.Style;
            txtDrawText.Font = new Font(fontF.Name, size, fontS, GraphicsUnit.Point, ((byte)(134)));
        }

        #region 截图后的一些后期绘制

        private void imageProcessBox_MouseDown(object sender, MouseEventArgs e) {
            if (imageProcessBox.Cursor != Cursors.SizeAll &&
                imageProcessBox.Cursor != Cursors.Default)
                plTool.Visible = false;         //表示改变选取大小 隐藏工具条
            //若果在选区内点击 并且有选择工具
            if (e.Button == MouseButtons.Left && imageProcessBox.IsDrawed && HaveSelectedToolButton()) {
                if (imageProcessBox.SelectedRect.Contains(e.Location)) {
                    if (btnText.IsSelected) {         //如果选择的是绘制文本 弹出文本框
                        Size size = TextRenderer.MeasureText("X", txtDrawText.Font);
                        Point loc = e.Location;
                        if (loc.X + size.Width > imageProcessBox.SelectedRect.Right-1) {
                            loc.X = imageProcessBox.SelectedRect.Right - 1 - size.Width;
                            if (loc.X < imageProcessBox.SelectedRect.Left + 1) {
                                loc.X = imageProcessBox.SelectedRect.Left + 1;
                                size.Width = imageProcessBox.SelectedRect.Width - 2;
                            }
                        }
                        if (loc.Y + size.Height > imageProcessBox.SelectedRect.Bottom - 1) {
                            loc.Y = imageProcessBox.SelectedRect.Bottom - 1 - size.Height;
                            if (loc.Y < imageProcessBox.SelectedRect.Top + 1) {
                                loc.Y = imageProcessBox.SelectedRect.Top + 1;
                                size.Height = imageProcessBox.SelectedRect.Height - 2;
                            }
                        }
                        txtDrawText.Size = size;
                        txtDrawText.Invalidate();
                        txtDrawText.Location = loc;
                        txtDrawText.Visible = true;
                        txtDrawText.Focus();
                        return;
                    }
                    this.Cursor = crossCursor;
                    isStartDraw = true;
                    Cursor.Clip = imageProcessBox.SelectedRect;
                }
            }
            ptOriginal = e.Location;
        }

        private void imageProcessBox_MouseMove(object sender, MouseEventArgs e) {
            ptCurrent = e.Location;
            //根据是否选择有工具决定 鼠标指针样式
            if (imageProcessBox.SelectedRect.Contains(e.Location) && HaveSelectedToolButton() && imageProcessBox.IsDrawed)
                this.Cursor = crossCursor;
            else if (!imageProcessBox.SelectedRect.Contains(e.Location))
                this.Cursor = Cursors.Default;
            Rectangle rect = new Rectangle();
            rect.X = imageProcessBox.SelectedRect.X-2;
            rect.Y = imageProcessBox.SelectedRect.Y-2;
            rect.Width = imageProcessBox.SelectedRect.Width + 6;
            rect.Height = imageProcessBox.SelectedRect.Height + 6;

            if (imageProcessBox.IsStartDraw && plTool.Visible)   //在重置选取的时候 重置工具条位置(成立于移动选取的时候)
                this.SetToolBarLocation();
            if (isStartDraw && bmpLayerShow != null) {        //如果在区域内点下那么绘制相应图形
                using (Graphics g = Graphics.FromImage(bmpLayerShow)) {
                    int tempWidth = 1;
                    if (btnMiddle.IsSelected) tempWidth = 3;
                    if (btnLarge.IsSelected) tempWidth = 5;
                    Pen p = new Pen(colorBox.SelectedColor, tempWidth);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    #region   绘制矩形

                    if (btnRect.IsSelected) {
                        int tempX = e.X - ptOriginal.X > 0 ? ptOriginal.X : e.X;
                        int tempY = e.Y - ptOriginal.Y > 0 ? ptOriginal.Y : e.Y;
                        g.Clear(Color.Transparent);
                        g.DrawRectangle(p, tempX - imageProcessBox.SelectedRect.Left, tempY - imageProcessBox.SelectedRect.Top, Math.Abs(e.X - ptOriginal.X), Math.Abs(e.Y - ptOriginal.Y));
                        imageProcessBox.Invalidate();
                    }

                    #endregion

                    #region    绘制圆形

                    if (btnEllipse.IsSelected) {
                        g.DrawLine(Pens.Red, 0, 0, 200, 200);
                        g.Clear(Color.Transparent);
                        g.DrawEllipse(p, ptOriginal.X - imageProcessBox.SelectedRect.Left, ptOriginal.Y - imageProcessBox.SelectedRect.Top, e.X - ptOriginal.X, e.Y - ptOriginal.Y);
                        imageProcessBox.Invalidate();
                    }

                    #endregion

                    #region    绘制箭头

                    if (btnArrow.IsSelected) {
                        g.Clear(Color.Transparent);
                        int X = imageProcessBox.SelectedRect.Location.X;
                        int Y = imageProcessBox.SelectedRect.Location.Y;
                        float len = (float)Math.Sqrt(Math.Pow(ptCurrent.X - ptOriginal.X, 2) + Math.Pow(ptCurrent.Y - ptOriginal.Y, 2));
                        //float k = (ptCurrent.Y - ptOriginal.Y) * 1.0F / (ptCurrent.X - ptOriginal.X);
                        //float b = ptOriginal.Y - k * ptOriginal.X;
                        float sink = (ptCurrent.Y - ptOriginal.Y) / len;
                        float cosk = (ptCurrent.X - ptOriginal.X) / len;
                        PointF[] points;
                        float h = 10;
                        if (btnMiddle.IsSelected) h = 16;
                        if (btnLarge.IsSelected) h = 22;
                        if (len < h)
                        {
                            points = new PointF[4];
                            points[0].X = points[3].X = ptOriginal.X + h * cosk-X;
                            points[0].Y = points[3].Y = ptOriginal.Y + h * sink-Y;
                            points[1].X = ptOriginal.X - h / 2 * sink - X;
                            points[1].Y = ptOriginal.Y + h / 2 * cosk - Y;
                            points[2].X = ptOriginal.X + h / 2 * sink - X;
                            points[2].Y = ptOriginal.Y - h / 2 * cosk - Y;
                        }
                        else
                        {
                            points = new PointF[8];
                            points[0].X = ptCurrent.X - X;
                            points[0].Y = ptCurrent.Y - Y;
                            points[7] = points[0];
                            float tmpX = ptCurrent.X - h * cosk;
                            float tmpY = ptCurrent.Y - h * sink;
                            points[1].X = tmpX - h / 2 * sink - X;
                            points[1].Y = tmpY + h / 2 * cosk - Y;
                            points[6].X = tmpX + h / 2 * sink - X;
                            points[6].Y = tmpY - h / 2 * cosk - Y;
                            points[2].X = tmpX - h / 4 * sink - X;
                            points[2].Y = tmpY + h / 4 * cosk - Y;
                            points[5].X = tmpX + h / 4 * sink - X;
                            points[5].Y = tmpY - h / 4 * cosk - Y;
                            tmpX = ptOriginal.X;
                            tmpY = ptOriginal.Y;
                            points[3].X = tmpX - h / 50 * sink - X;
                            points[3].Y = tmpY + h / 50 * cosk - Y;
                            points[4].X = tmpX + h / 50 * sink - X;
                            points[4].Y = tmpY - h / 50 * cosk - Y;

                        }
                        //AdjustableArrowCap lineArrow =new AdjustableArrowCap(10, 10, true);
                        //p.StartCap = LineCap.Round;
                        //p.CustomEndCap = lineArrow;
                        //g.DrawLine(p, (Point)((Size)m_ptOriginal - (Size)imageProcessBox.SelectedRectangle.Location),
                        //    (Point)((Size)m_ptCurrent - (Size)imageProcessBox.SelectedRectangle.Location));
                        g.FillPolygon(new SolidBrush(colorBox.SelectedColor), points);
                        //g.DrawPolygon(p, points);
                        imageProcessBox.Invalidate();
                    }
                    #endregion

                    #region    绘制线条
                    if (btnBrush.IsSelected) {
                        Point ptTemp = (Point)((Size)ptOriginal - (Size)imageProcessBox.SelectedRect.Location);
                        p.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                        g.DrawLine(p, ptTemp, (Point)((Size)e.Location - (Size)imageProcessBox.SelectedRect.Location));
                        ptOriginal = e.Location;
                        imageProcessBox.Invalidate();
                    }
                    #endregion
                    p.Dispose();
                }
            }
        }

        private void imageProcessBox_MouseUp(object sender, MouseEventArgs e) {
            if (this.IsDisposed) return;
            if (e.Button == MouseButtons.Right) {   //右键清空绘制
                this.Enabled = false;
                imageProcessBox.ClearDraw();
                imageProcessBox.CanReset = true;
                imageProcessBox.IsDrawOperationDot = false;
                historyLayer.Clear();    //清空历史记录
                bmpLayerCurrent = null;
                bmpLayerShow = null;
                ClearToolBarBtnSelected();
                plTool.Visible = false;
                plColorBox.Visible = false;
            }
            if (!imageProcessBox.IsDrawed) {       //如果没有成功绘制选取 继续禁用窗体
                this.Enabled = false;
                imageProcessBox.IsDrawOperationDot = false;
            } else if (!plTool.Visible) {           //否则显示工具条
                this.SetToolBarLocation();          //重置工具条位置
                plTool.Visible = true;
                bmpLayerCurrent = imageProcessBox.GetResultBmp();    //获取选取图形
                bmpLayerShow = new Bitmap(bmpLayerCurrent.Width, bmpLayerCurrent.Height);
            }
            //如果移动了选取位置 重新获取选取的图形
            if (imageProcessBox.Cursor == Cursors.SizeAll && ptOriginal != e.Location) {
                bmpLayerCurrent.Dispose();
                bmpLayerCurrent = imageProcessBox.GetResultBmp();
            }

            if (!isStartDraw) return;
            Cursor.Clip = Rectangle.Empty;
            isStartDraw = false;
            if (e.Location == ptOriginal && !btnBrush.IsSelected) return;
            this.SetLayer();        //将绘制的图形绘制到历史图层中
        }
        //绘制后期操作
        private void imageProcessBox_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            if (historyLayer.Count > 0)  //绘制保存的历史记录的最后一张图
                g.DrawImage(historyLayer[historyLayer.Count - 1], imageProcessBox.SelectedRect.Location);
            if (bmpLayerShow != null)     //绘制当前正在拖动绘制的图形(即鼠标点下还没有抬起确认的图形)
                g.DrawImage(bmpLayerShow, imageProcessBox.SelectedRect.Location);
        }

        #endregion

        //文本改变时重置文本框大小
        private void txtDrawText_TextChanged(object sender, EventArgs e) {
            if (txtDrawText.Right < imageProcessBox.SelectedRect.Right - 1 || txtDrawText.Bottom < imageProcessBox.SelectedRect.Bottom - 1) {
                int lines = txtDrawText.Lines.Length;
                Size[] sizes = new Size[lines];
                int maxWidth = imageProcessBox.SelectedRect.Right - txtDrawText.Left-1;
                int maxHeight = imageProcessBox.SelectedRect.Bottom - txtDrawText.Top-1;
                Size size = new Size(0,0);
                for (int i = 0; i < lines; i++) {
                    sizes[i] = TextRenderer.MeasureText(txtDrawText.Text, txtDrawText.Font);
                    if (sizes[i].Width >= maxWidth) {
                        size.Width = maxWidth;
                        if (sizes[i].Width / maxWidth * maxWidth == sizes[i].Width)
                            size.Height += sizes[i].Width / maxWidth * sizes[i].Height;
                        else
                            size.Height += (sizes[i].Width / maxWidth + 1) * sizes[i].Height;
                    } else {
                        if (size.Width < sizes[i].Width) {
                            size.Width = sizes[i].Width;
                        }
                        size.Height += sizes[i].Height;
                    }
                }
                if (size.Height > maxHeight)
                    size.Height = maxHeight;
                //if (txtDrawText.Left + se.Width > imageProcessBox.SelectedRectangle.Right) {
                //    se.Width = imageProcessBox.SelectedRectangle.Right - txtDrawText.Left;
                //    se.Height += TextRenderer.MeasureText("X", txtDrawText.Font).Height;
                //}
                //if (txtDrawText.Top + se.Height > imageProcessBox.SelectedRectangle.Bottom) {
                //    se.Height = imageProcessBox.SelectedRectangle.Bottom - txtDrawText.Top;
                //}
                if (size.Width < txtDrawText.Width)
                    size.Width = txtDrawText.Width;
                if (size.Height < txtDrawText.Height)
                    size.Height = txtDrawText.Height;
                txtDrawText.Size = size.IsEmpty ? new Size(50, txtDrawText.Font.Height) : size;
                txtDrawText.Invalidate();
            }
        }
        //文本框失去焦点时 绘制文本
        private void txtDrawText_Validating(object sender, CancelEventArgs e) {
            txtDrawText.Visible = false;
            if (string.IsNullOrEmpty(txtDrawText.Text.Trim())) { 
                txtDrawText.Text = ""; 
                return; 
            }
            using (Graphics g = Graphics.FromImage(bmpLayerCurrent)) {
                SolidBrush sb = new SolidBrush(colorBox.SelectedColor);
                //g.DrawString(txtDrawText.Text, txtDrawText.Font, sb,
                //    txtDrawText.Left - imageProcessBox.SelectedRectangle.Left,
                //    txtDrawText.Top - imageProcessBox.SelectedRectangle.Top);
                Point ptT=txtDrawText.Location;
                Point ptS=imageProcessBox.SelectedRect.Location;
                g.DrawString(txtDrawText.Text, txtDrawText.Font, sb, new RectangleF(ptT.X-ptS.X,ptT.Y-ptS.Y,txtDrawText.Size.Width,txtDrawText.Height));
                sb.Dispose();
                txtDrawText.Text = "";
                this.SetLayer();        //将文本绘制到当前图层并存入历史记录
                imageProcessBox.Invalidate();
            }
        }

        //撤销
        private void btnReset_Click(object sender, EventArgs e) {
            using (Graphics g = Graphics.FromImage(bmpLayerShow)) {
                g.Clear(Color.Transparent);     //清空当前临时显示的图像
            }
            if (historyLayer.Count > 0) {            //删除最后一层
                historyLayer.RemoveAt(historyLayer.Count - 1);
                if (historyLayer.Count > 0)
                    bmpLayerCurrent = historyLayer[historyLayer.Count - 1].Clone() as Bitmap;
                else
                    bmpLayerCurrent = imageProcessBox.GetResultBmp();
                imageProcessBox.Invalidate();
                imageProcessBox.CanReset = historyLayer.Count == 0 && !HaveSelectedToolButton();
            } else {                            //如果没有历史记录则取消本次截图
                this.Enabled = false;
                imageProcessBox.ClearDraw();
                imageProcessBox.IsDrawOperationDot = false;
                plTool.Visible = false;
                plColorBox.Visible = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e) {
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Filter = Util.SUPPORT_EXTENSION_FILTER;
            saveDlg.FilterIndex = 3;
            saveDlg.FileName = Util.GetSavePicPath();
            if (saveDlg.ShowDialog() == DialogResult.OK) {
                switch (saveDlg.FilterIndex) {
                    case 1:
                        bmpLayerCurrent.Clone(new Rectangle(0, 0, bmpLayerCurrent.Width, bmpLayerCurrent.Height),
                            System.Drawing.Imaging.PixelFormat.Format24bppRgb).Save(saveDlg.FileName,
                            System.Drawing.Imaging.ImageFormat.Bmp);
                        this.Close();
                        break;
                    case 2:
                        bmpLayerCurrent.Save(saveDlg.FileName,
                            System.Drawing.Imaging.ImageFormat.Jpeg);
                        this.Close();
                        break;
                    case 3:
                        bmpLayerCurrent.Save(saveDlg.FileName,
                            System.Drawing.Imaging.ImageFormat.Png);
                        this.Close();
                        break;
                }
            }
            //m_bSave = false;
        }
        //将图像保存到剪贴板
        private void tBtn_Finish_Click(object sender, EventArgs e) {
            Clipboard.SetImage(bmpLayerCurrent);
            this.Close();
        }

        private void btnSticky_Click(object sender, EventArgs e) {
            new StickyForm(bmpLayerCurrent.Clone() as Bitmap).Show();
            this.Close();
        }

        private void imageProcessBox_DoubleClick(object sender, EventArgs e) {
            Clipboard.SetImage(bmpLayerCurrent);
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e) {
            //if (!this.Enabled)
            //    imageProcessBox.SetInfoPoint(MousePosition.X, MousePosition.Y);
        }
        //根据鼠标位置找寻窗体并绘制边框
        private void FoundAndDrawWindowRect() {
            Win32.LPPOINT pt = new Win32.LPPOINT();
            pt.X = MousePosition.X; 
            pt.Y = MousePosition.Y;
            IntPtr hWnd = Win32.ChildWindowFromPointEx(Win32.GetDesktopWindow(), pt,
                Win32.CWP_SKIPINVISIBL | Win32.CWP_SKIPDISABLED);
            if (hWnd != IntPtr.Zero) {
                IntPtr hTemp = hWnd;
                while (true) {
                    Win32.ScreenToClient(hTemp, out pt);
                    hTemp = Win32.ChildWindowFromPointEx(hWnd, pt, Win32.CWP_SKIPINVISIBL);
                    if (hTemp == IntPtr.Zero || hTemp == hWnd)
                        break;
                    hWnd = hTemp;
                    pt.X = MousePosition.X; pt.Y = MousePosition.Y; //坐标还原为屏幕坐标
                }
                Win32.LPRECT rect = new Win32.LPRECT();
                Win32.GetWindowRect(hWnd, out rect);
                imageProcessBox.SetSelectRect(new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top));
            }
        }
        //获取桌面图像
        private Bitmap GetScreen(bool bCaptureCursor, bool bFromClipBoard) {
            Bitmap bmp = new Bitmap(maxWidth, maxHeight);
            if (bCaptureCursor)      //是否捕获鼠标
                DrawCurToScreen();

            //做完以上操作 才开始捕获桌面图像
            using (Graphics g = Graphics.FromImage(bmp)) {
                g.CopyFromScreen(0, 0, 0, 0, bmp.Size);
                if (!bFromClipBoard)
                    return bmp;
                using (Image imgClip = Clipboard.GetImage()) {
                    if (imgClip != null) {
                        using (SolidBrush sb = new SolidBrush(Color.FromArgb(150, 0, 0, 0))) {
                            g.FillRectangle(sb, 0, 0, bmp.Width, bmp.Height);
                            g.DrawImage(imgClip,
                                (bmp.Width - imgClip.Width) >> 1,
                                (bmp.Height - imgClip.Height) >> 1,
                                imgClip.Width, imgClip.Height);
                        }
                    }
                }
            }
            return bmp;
        }
        //在桌面绘制鼠标
        public Rectangle DrawCurToScreen() {
            //如果直接将捕获到的鼠标画在bmp上 光标不会反色 指针边框也很浓 也就是说
            //尽管bmp上绘制了图像 绘制鼠标的时候还是以黑色作为鼠标的背景 然后在将混合好的鼠标绘制到图像 会很别扭
            //所以 干脆直接在桌面把鼠标绘制出来再截取桌面
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero)) {   //传入0默认就是桌面 Win32.GetDesktopWindow()也可以
                Win32.PCURSORINFO pci;
                pci.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Win32.PCURSORINFO));
                Win32.GetCursorInfo(out pci);
                if (pci.hCursor != IntPtr.Zero) {
                    Cursor cur = new Cursor(pci.hCursor);
                    Rectangle rectCur = new Rectangle((Point)((Size)MousePosition - (Size)cur.HotSpot), cur.Size);
                    g.CopyFromScreen(0, 0, 0, 0, new Size(maxWidth,maxHeight));
                    //g.CopyFromScreen(rect_cur.Location, rect_cur.Location, rect_cur.Size); //在桌面绘制鼠标前 先在桌面绘制一下当前的桌面图像
                    //如果不绘制当前桌面 那么cur.Draw的时候会是用历史桌面的快照 进行鼠标的混合 那么到时候混出现底色(测试中就是这样的)
                    cur.Draw(g, rectCur);
                    return rectCur;
                }
                return Rectangle.Empty;
            }
        }
        //设置工具条位置
        private void SetToolBarLocation() {
            int plToolX = imageProcessBox.SelectedRect.Right-plTool.Width;
            if (plToolX < 0)
                plToolX = 0;
            plTool.Left = plToolX;
            plColorBox.Left = plToolX;

            int plToolY = imageProcessBox.SelectedRect.Bottom + 3;
            if (plToolY + plTool.Height > this.Height) {
                plToolY = imageProcessBox.SelectedRect.Top - plTool.Height - 6 - imageProcessBox.Font.Height;
                if (plToolY <= 0) {
                    plToolY = 0;
                }
            }
            plTool.Top = plToolY;
            if (plColorBox.Visible) {
                int plColorBoxY = plTool.Bottom + 2;
                int tempHeight = plColorBox.Height + 2;
                if (plColorBoxY + tempHeight > this.Height) {
                    plColorBoxY = plTool.Top - 2 - plColorBox.Height;
                }
                plColorBox.Top = plColorBoxY;
            }
        }
        //确定是否工具条上面有被选中的按钮
        private bool HaveSelectedToolButton() {
            return btnRect.IsSelected || btnEllipse.IsSelected
                || btnArrow.IsSelected || btnBrush.IsSelected
                || btnText.IsSelected;
        }
        //清空选中的工具条上的工具
        private void ClearToolBarBtnSelected() {
            btnRect.IsSelected = btnEllipse.IsSelected = btnArrow.IsSelected =
                btnBrush.IsSelected = btnText.IsSelected = false;
        }
        //设置历史图层
        private void SetLayer() {
            if (this.IsDisposed) return;
            using (Graphics g = Graphics.FromImage(bmpLayerCurrent)) {
                g.DrawImage(bmpLayerShow, 0, 0);
            }
            Bitmap bmpTemp = bmpLayerCurrent.Clone() as Bitmap;
            historyLayer.Add(bmpTemp);
        }

        private void CaptureForm_FormClosed(object sender, FormClosedEventArgs e) {
            isAlive = false;
        }
    }
}
