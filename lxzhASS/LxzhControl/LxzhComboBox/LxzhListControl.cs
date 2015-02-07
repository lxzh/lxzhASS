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
    /// <summary>
    /// This ListControl that pops up to the User. It contains the lxzhItems. 
    /// The items are docked DockStyle.Top in this control.
    /// </summary>
    [ToolboxItem(false)]
    public partial class LxzhListControl : LxzhRoundControl {

        /// <summary>
        /// Simply a reference to the lxzh.
        /// </summary>
        private LxzhComboBox lxzhComboBox;
        private int margin = 2;
        private int index = 0;

        public int Index {
            get { return index; }
            set { index = value; }
        }
        private int itemCount = 0;

        public int ItemCount {
            get { return itemCount; }
            set { itemCount = value; }
        }
        private int showItemCount = 0;

        public int ShowItemCount {
            get { return showItemCount; }
            set { showItemCount = value; }
        }

        private int itemHeight = 20;

        public int ItemHeight {
            get { return itemHeight; }
        }
        private int itemWidth;
        private int showHeight;

        private bool showScrollBar;
        //当前鼠标选中项索引
        private int nowIndex = -1;
        //前一个鼠标训中项索引
        private int preIndex = -1;

        /// <summary>
        /// A Typed list of ComboBoxCheckBoxItems.
        /// </summary>
        private List<LxzhComboBoxItem> comboBoxItems;

        public List<LxzhComboBoxItem> ComboBoxItems {
            get { return comboBoxItems; }
        }

        public LxzhListControl(LxzhComboBox owner)
            : base() {
            InitializeComponent();
            DoubleBuffered = true;
            lxzhComboBox = owner;
            lxzhComboBox.ItemsChanged += new EventHandler(ListItem_Changed);
            ScrollBar.Scroll += new EventHandler(scrollBar_Scroll);
            comboBoxItems = new List<LxzhComboBoxItem>();
            BackColor = Color.Transparent;
            AutoScroll = false;
            ResizeRedraw = true;
            // if you don't set this, a Resize operation causes an error in the base class.
            MinimumSize = new Size(1, 1);
            MaximumSize = new Size(500, 500);
        }

        private new event EventHandler Scroll;

        //public event EventHandler ItemsChanged; 
        /// <summary>
        /// Prescribed by the Popup control to enable Resize operations.
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m) {
            if ((Parent as Popup).ProcessResizing(ref m)) {
                return;
            }
            base.WndProc(ref m);
        }

        protected override void OnPaint(PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            SolidBrush brush=new SolidBrush(ForeColor);
            for (int i = index,j=0; j < showItemCount; i++,j++) {
                string text = lxzhComboBox.Items[i];
                //SizeF txtSize = TextRenderer.MeasureText(text, this.Font);
                Point textPoint=new Point(margin+1,j*itemHeight+margin);
                g.DrawString(text, this.Font, brush, textPoint);
                if (j == nowIndex) {
                    Rectangle rect = new Rectangle(new Point(margin,textPoint.Y), new Size(itemWidth, itemHeight));
                    Pen pen=new Pen(new SolidBrush(BorderColor),BorderThickness);
                    g.FillRectangle(new SolidBrush(Color.FromArgb(100, BorderColor)), rect);
                    g.DrawRectangle(pen, rect);
                }
            }
            Console.WriteLine("nowIndex:" + nowIndex);
            base.OnPaint(e);
        }

        private void scrollBar_Scroll(object sender, EventArgs e) {
            if (Scroll != null) {
                Scroll(sender, e);
            }
        }

        private void ListItem_Changed(object sender, EventArgs e) {

            itemHeight = lxzhComboBox.ItemHeight;
            itemWidth = this.Width - margin * 2;
            showHeight = showItemCount * itemHeight;

            showScrollBar = showItemCount < lxzhComboBox.Items.Count;
            ScrollBar.Visible = showScrollBar;

            if (showScrollBar) {
                itemWidth -= 16;
                //ScrollBar.Maximum
            }
        }

        protected override void OnMouseEnter(EventArgs e) {
            base.OnMouseEnter(e);
            Point point = PointToClient(Cursor.Position);
            if (point.X <= this.Width - 16 - margin) {
                nowIndex = getMouseHoverItemIndex();
                //Console.WriteLine("OnMouseEnter");
                this.Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e) {
            base.OnMouseLeave(e);
            //Point point = PointToClient(Cursor.Position);
            //Rectangle rect = new Rectangle(this.Width - 16 - margin-1, 0, 16 + margin+1, this.Height);
            //if (!rect.Contains(point)) {
            //    nowIndex = -1;
            //    preIndex = -1;
            //    Console.WriteLine("OnMouseLeave"); 
            //    this.Invalidate();
            //}
        }

        protected override void OnClick(EventArgs e) {
            base.OnClick(e);
            lxzhComboBox.Text = lxzhComboBox.Items[nowIndex+index];
            lxzhComboBox.HideDropDown();
            lxzhComboBox.Refresh();
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            Point point = PointToClient(Cursor.Position);
            if (point.X <= this.Width - 16 - margin) {
                nowIndex = getMouseHoverItemIndex();
                if (preIndex != nowIndex) {
                    this.Invalidate();
                    preIndex = nowIndex;
                }
            }
            Console.WriteLine("OnMouseMove");
        }

        protected override void OnVisibleChanged(EventArgs e) {
            nowIndex=findItemIndex();
            base.OnVisibleChanged(e);
        }

        protected override void OnSizeChanged(EventArgs e) {
            base.OnSizeChanged(e);
            ScrollBar.Location = new Point(this.Width - 16 - margin - 1, margin);
            ScrollBar.Size = new Size(16, this.Height - margin * 2);
        }

        private int getMouseHoverItemIndex() {
            int index = -1;
            Point point = PointToClient(Cursor.Position);
            Rectangle rect=new Rectangle(0,margin,this.Width-16-margin,itemHeight);
            for (int i = 0; i < showItemCount; i++) {
                if (rect.Contains(point)) {
                    index=i;
                    break;
                }
                rect.Y += itemHeight;
            }
            rect = new Rectangle(this.Width - 16 - margin, 0, 16 + margin, this.Height);
            if (rect.Contains(point)) {
                index = nowIndex;
            }
            Console.WriteLine("Index:" + index);
            return index;
        }

        private int findItemIndex() {
            if (lxzhComboBox == null || lxzhComboBox.Items == null)
                return -1;
            for (int i = index,j=0; i < showItemCount; i++,j++) {
                if (lxzhComboBox.Items[i].Equals(lxzhComboBox.Text)) {
                    return j;
                }
            }
            return -1;
        }

        private void Clear() {
            foreach (Control con in Controls) {
                if (con.Name != ScrollBar.Name) {
                    Controls.Remove(con);
                }
            }
        }
    }
}
