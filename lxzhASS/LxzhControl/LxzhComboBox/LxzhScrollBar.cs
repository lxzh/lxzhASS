using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Threading;

namespace lxzh {
    public partial class LxzhScrollBar : LxzhRoundControl {

        private Rectangle rectBtnUp = new Rectangle(1, 1, 14, 14);
        private Rectangle rectBtnDown;
        private Rectangle rectBtnScroll;
        private Rectangle rectScrollBar;
        private ArrowState upArrowState = ArrowState.Hide;
        private ArrowState downArrowState = ArrowState.Hide;
        private ScrollBarState barState = ScrollBarState.NoBack;
        private MouseSection mousePosSection = MouseSection.Out;

        private bool isMouseDown = false;
        private bool isMouseDrag = false;
        private Point mouseDownPos;
        private Point preBarPosition;

        public new event EventHandler Scroll = null;
        public event EventHandler ValueChanged = null;

        private int minimum = 0;
        [Browsable(true), DefaultValue(0)]
        public int Minimum {
            get { return minimum; }
            set { 
                minimum = value;
                if (minimum > value)
                    this.value = minimum;
                setBarHeight();
                this.Invalidate();
            }
        }

        private int maximum = 100;
        [Browsable(true), DefaultValue(100)]
        public int Maximum {
            get { return maximum; }
            set { 
                maximum = value;
                if (maximum < this.value) {
                    this.value = maximum;
                }
                setBarHeight();
                this.Invalidate();
            }
        }

        private int value = 0;
        [Browsable(true), DefaultValue(0)]
        public int Value {
            get { return this.value; }
            set { 
                this.value = value;
                if (this.value > maximum) {
                    this.value = maximum;
                } else if (this.value < minimum) {
                    this.value = minimum;
                }
                int y = (int)((this.value - minimum)*1F / (maximum - minimum) * (rectScrollBar.Height - rectBtnScroll.Height) + rectScrollBar.Y);
                rectBtnScroll.Y = y;
                this.Invalidate();
            }
        }
        private int delta;
        /// <summary>
        /// 滚动条改变值
        /// </summary>
        [Browsable(false)]
        public int Delta {
            get { return delta; }
            set { delta = value; }
        }

        private int largeChange = 3;
        [Browsable(true), DefaultValue(3)]
        public int LargeChange {
            get { return largeChange; }
            set { 
                largeChange = value;
                //if (largeChange > maximum - minimum) {
                //    largeChange = maximum - minimum;
                //}
                //if (largeChange < smallChange) {
                //    largeChange = smallChange;
                //}
                setBarHeight();
                this.Invalidate();
            }
        }

        private int smallChange = 1;
        [Browsable(true), DefaultValue(1)]
        public int SmallChange {
            get { return smallChange; }
            set { 
                smallChange = value;
                if (smallChange > largeChange) {
                    smallChange = largeChange;
                }
            }
        }

        public new Size Size {
            get { return base.Size; }
            set {
                Size size = value;
                size.Width = 16;
                if (size.Height < 48) {
                    size.Height = 48;
                }
                base.Size = size;
                this.Refresh();
            }
        }

        [Browsable(true)]
        public new Point Location {
            get { return base.Location; }
            set { base.Location = value; }
        }

        private Color arrowBtnBorderColor = Color.Gray;
        [Browsable(true), DefaultValue(typeof(Color), "Gray"), Description("箭头边框颜色")]
        public Color ArrowBtnBorderColor {
            get { return arrowBtnBorderColor; }
            set {
                arrowBtnBorderColor = value;
                penArrowBorder = new Pen(new SolidBrush(arrowBtnBorderColor), 1.4F);
                this.Refresh();
            }
        }

        private Pen penArrowBorder = new Pen(new SolidBrush(Color.Gray), 1.4F);

        public LxzhScrollBar() {
            InitializeComponent();

        }

        protected override void OnClick(EventArgs e) {
            base.OnClick(e);
            Point mPoint = PointToClient(Cursor.Position);
            int step = 0;
            if (upArrowState == ArrowState.MouseDown) {
                step = smallChange * -1;
            } else if (downArrowState == ArrowState.MouseDown) {
                step = smallChange;
            } else if (barState == ScrollBarState.MouseDown) {

            } else if (rectScrollBar.Contains(mPoint)) {
                if (mPoint.Y < rectBtnScroll.Y) {
                    step = largeChange * -1;
                } else if (mPoint.Y > rectBtnScroll.Bottom) {
                    step = largeChange;
                }
            }
            if (step != 0)
                increaseBar(step);
        }

        protected override void OnMouseEnter(EventArgs e) {
            base.OnMouseEnter(e);
            Point mPoint = MousePosition;
            setMousePos();
            updateState();
            this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e) {
            base.OnMouseLeave(e);
            mousePosSection = MouseSection.Out;
            setMousePos();
            updateState();
            this.Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            Console.WriteLine("OnMouseDown");
            base.OnMouseDown(e);
            mouseDownPos = Cursor.Position;
            preBarPosition = rectBtnScroll.Location;
            isMouseDown = true;
            setMousePos();
            updateState();
            new Thread(new ThreadStart(DealMouseDownThread)).Start();
            //timScroll.Start();
            this.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            Console.WriteLine("OnMouseUp");
            base.OnMouseUp(e);
            isMouseDown = false;
            isMouseDrag = false;
            MouseSection preSection = mousePosSection;
            setMousePos();
            updateState();
            if (preSection != mousePosSection) {

            }
            this.Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            MouseSection preSection = mousePosSection;
            if (isMouseDown) {
                Point mPoint = PointToClient(Cursor.Position);
                if (isMouseDrag || rectBtnScroll.Contains(mPoint)) {
                    isMouseDrag = true;
                    moveBtnScroll();
                }
            } else {
                setMousePos();
                updateState();
                if (preSection != mousePosSection) {
                    this.Refresh();
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e) {
            base.OnMouseWheel(e);
            increaseBar(e.Delta / -120 * smallChange);
        }

        protected override void OnResize(EventArgs e) {
            Size newSize = this.Size;
            newSize.Width = 16;
            this.Size = newSize;
            rectBtnDown = new Rectangle(1, this.Height - 15, 14, 14);
            rectScrollBar = new Rectangle(2, 15, 12, this.Height - 30);
            if (Util.isZeroRect(rectBtnScroll)) {
                rectBtnScroll = new Rectangle(rectScrollBar.Location, rectScrollBar.Size);
            } else {
                //rectBtnScroll.Location = rectScrollBar.Location;
                setBarHeight();
            }
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            drawUpArrow(g);
            drawDownArrow(g);
            drawScrollBar(g);
        }

        private void LxzhScrollBar_Load(object sender, EventArgs e) {
            timScroll_Tick(sender, e);
        }

        private void DealMouseDownThread() {
            Control.CheckForIllegalCrossThreadCalls = false;
            //此处很关键，增添短暂的暂停，可避免MouseDown事件后再触发Click事件导致的重复增加
            Thread.Sleep(200);
            Point mousePos = PointToClient(Cursor.Position);
            MouseEventArgs e;
            while (isMouseDown) {
                e = new MouseEventArgs(MouseButtons.Left, 1, mousePos.X, mousePos.Y, 0);
                OnClick(e);
                Thread.Sleep(100);
            }
        }

        private void timScroll_Tick(object sender, EventArgs e) {
            if (isMouseDown) {
                Point mPoint = PointToClient(Cursor.Position);
                int step = 0;
                if (upArrowState == ArrowState.MouseDown) {
                    step = smallChange * -1;
                } else if (downArrowState == ArrowState.MouseDown) {
                    step = smallChange;
                } else if (barState == ScrollBarState.MouseDown) {

                } else if (rectScrollBar.Contains(mPoint)) {
                    if (mPoint.Y < rectBtnScroll.Y) {
                        step = largeChange * -1;
                    } else if (mPoint.Y > rectBtnScroll.Bottom) {
                        step = largeChange;
                    }
                }
                if (step != 0) 
                    increaseBar(step);
                else
                    timScroll.Stop();
            } else {
                timScroll.Stop();
            }
        }

        private void setBarHeight() {
            int barHeight = (int)(rectScrollBar.Height * rectScrollBar.Height * 1F / maximum);
            if (barHeight < 16)
                barHeight = 16;
            else if (barHeight > rectScrollBar.Height)
                barHeight = rectScrollBar.Height;
            rectBtnScroll.Height = barHeight;
        }

        private void setMousePos() {
            Point mPoint = PointToClient(Cursor.Position); ;
            if (this.ClientRectangle.Contains(mPoint)) {
                if (mPoint.Y <= rectBtnUp.Bottom) {
                    mousePosSection = MouseSection.UpArrow;
                } else if (mPoint.Y >= rectBtnDown.Y) {
                    mousePosSection = MouseSection.DownArrow;
                } else if (mPoint.Y >= rectBtnScroll.Y && mPoint.Y <= rectBtnScroll.Bottom) {
                    mousePosSection = MouseSection.BtnBar;
                } else {
                    mousePosSection = MouseSection.ScrollBar;
                }
            } else {
                mousePosSection = MouseSection.Out;
            }
        }

        private void updateState() {
            switch (mousePosSection) {
                case MouseSection.UpArrow:
                    if (isMouseDown) {
                        upArrowState = ArrowState.MouseDown;
                    } else {
                        upArrowState = ArrowState.MouseHover;
                    }
                    downArrowState = ArrowState.NoBorder;
                    barState = ScrollBarState.MouseHover;
                    break;
                case MouseSection.DownArrow:
                    upArrowState = ArrowState.NoBorder;
                    if (isMouseDown) {
                        downArrowState = ArrowState.MouseDown;
                    } else {
                        downArrowState = ArrowState.MouseHover;
                    }
                    barState = ScrollBarState.MouseHover;
                    break;
                case MouseSection.BtnBar:
                    upArrowState = ArrowState.NoBorder;
                    downArrowState = ArrowState.NoBorder;
                    if (isMouseDown) {
                        barState = ScrollBarState.MouseDown;
                    } else {
                        barState = ScrollBarState.MouseHover;
                    }
                    break;
                case MouseSection.ScrollBar:
                    upArrowState = ArrowState.NoBorder;
                    downArrowState = ArrowState.NoBorder;
                    barState = ScrollBarState.MouseHover;
                    break;
                case MouseSection.Out:
                    if (!isMouseDown) {
                        upArrowState = ArrowState.Hide;
                        downArrowState = ArrowState.Hide;
                        barState = ScrollBarState.NoBack;
                    } else {

                    }
                    break;
            }
        }

        /// <summary>
        /// 拖动鼠标移动滚动条
        /// </summary>
        private void moveBtnScroll() {
            int delta = Cursor.Position.Y - mouseDownPos.Y;
            if (delta != 0) {
                int y = preBarPosition.Y + delta;
                if (y < rectBtnUp.Bottom) {
                    y = rectBtnUp.Bottom;
                } else if (y + rectBtnScroll.Height > rectBtnDown.Y) {
                    y = rectBtnDown.Y - rectBtnScroll.Height;
                } else {
                    delta = y - preBarPosition.Y;
                    if (delta / smallChange * smallChange != delta) {
                        delta = delta / smallChange * smallChange;
                    }
                    y = preBarPosition.Y + delta;
                }

                if (rectBtnScroll.Y != y) {
                    rectBtnScroll.Y = y;
                    value = rectBtnScroll.Y - 15;
                    if (ValueChanged != null)
                        ValueChanged(this, new EventArgs());
                    this.Invalidate();
                }
            }
        }
        /// <summary>
        /// 鼠标点击、鼠标滚轮滚动 滑动滚动条
        /// </summary>
        /// <param name="step"></param>
        private void increaseBar(int step) {
            int y = rectBtnScroll.Y + step;
            if (y < rectBtnUp.Bottom) {
                y = rectBtnUp.Bottom;
            } else if (y + rectBtnScroll.Height > rectBtnDown.Y) {
                y = rectBtnDown.Y - rectBtnScroll.Height;
            }
            if (rectBtnScroll.Y != y) {
                rectBtnScroll.Y = y;
                value = rectBtnScroll.Y - 15;
                setMousePos();
                updateState();
                this.Invalidate();
                delta = step;
                if (ValueChanged != null)
                    ValueChanged(this, new EventArgs());
                if (Scroll != null)
                    Scroll(this, new EventArgs());
            }
            preBarPosition = rectBtnScroll.Location;
        }

        private void drawUpArrow(Graphics g) {
            int arrowState = (int)upArrowState;
            if (arrowState > 0) {
                RectangleF rect = new RectangleF(rectBtnUp.X + 1.5F, rectBtnUp.Y + 1.5F, 11F, 11F);
                
                g.DrawImage(global::lxzh.Properties.Resources.arrowlist.Clone(new Rectangle(33, 0, 11, 11), PixelFormat.Format32bppPArgb), rect);
                arrowState--;
                if (arrowState > 0) {
                    GraphicsPath path = Util.GetRoundedRectPath(rectBtnUp, 1);
                    g.DrawPath(penArrowBorder, path);
                    arrowState--;
                    if (arrowState > 0) {
                        g.DrawImage(global::lxzh.Properties.Resources.arrowlist.Clone(new Rectangle(22, 0, 11, 11), PixelFormat.Format32bppPArgb), rect);
                    }
                }
            }
        }

        private void drawDownArrow(Graphics g) {
            int arrowState = (int)downArrowState;
            if (arrowState > 0) {
                RectangleF rect = new RectangleF(rectBtnDown.X + 1.5F, rectBtnDown.Y + 2.5F, 11F, 11F);
                g.DrawImage(global::lxzh.Properties.Resources.arrowlist.Clone(new Rectangle(11, 0, 11, 11), PixelFormat.Format32bppPArgb), rect);
                arrowState--;
                if (arrowState > 0) {
                    GraphicsPath path = Util.GetRoundedRectPath(rectBtnDown, 1);
                    g.DrawPath(penArrowBorder, path);
                    arrowState--;
                    if (arrowState > 0) {
                        g.DrawImage(global::lxzh.Properties.Resources.arrowlist.Clone(new Rectangle(0, 0, 11, 11), PixelFormat.Format32bppPArgb), rect);
                    }
                    path.Dispose();
                }
            }
        }

        private void drawScrollBar(Graphics g) {
            GraphicsPath path = Util.GetRoundedRectPath(rectScrollBar, 6);
            if (barState == ScrollBarState.NoBack) {
                g.FillPath(new SolidBrush(Color.White), path);
            } else {
                g.FillPath(new SolidBrush(Color.Gainsboro), path);
            }
            path = Util.GetRoundedRectPath(rectBtnScroll, 6);
            if (barState == ScrollBarState.NoBack) {
                g.FillPath(new SolidBrush(Color.Gainsboro), path);
            } else if (barState != ScrollBarState.MouseDown) {
                g.FillPath(new SolidBrush(Color.Silver), path);
            } else {
                g.FillPath(new SolidBrush(Color.DarkGray), path);
            }
            path.Dispose();
        }
    }
    /// <summary>
    /// 按钮状态
    /// </summary>

    public enum ArrowState {
        Hide = 0,//隐藏
        NoBorder = 1,//无边框
        MouseDown = 2,//鼠标点击
        MouseHover = 3//鼠标悬停
    }
    /// <summary>
    /// 滚动条状态
    /// </summary>
    public enum ScrollBarState {
        NoBack,//无背景
        MouseDown,//鼠标点击
        MouseHover//鼠标悬停
    }
    /// <summary>
    /// 鼠标位置
    /// </summary>
    public enum MouseSection {
        UpArrow,//上按钮区
        BtnBar,//滚动条按钮区
        DownArrow,//下按钮去
        ScrollBar,//滚动条区
        Out//滚动条以外
    }
}
