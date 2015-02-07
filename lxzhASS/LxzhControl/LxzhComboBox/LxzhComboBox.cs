using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace lxzh {
    [ToolboxItem(true)]
    public partial class LxzhComboBox : PictureBox {
        [Browsable(true)]
        public new string Text {
            get { return base.Text; }
            set {
                if (base.Text != value) {
                    base.Text = value; 
                    if (ValueChanged != null) {
                        ValueChanged(this, new EventArgs());
                    }
                }
            }
        }
        [Browsable(true)]
        public new Font Font {
            get { return base.Font; }
            set { base.Font = value; }
        }
        private SolidBrush backBrush;
        private Color backColor = Color.White;
        /// <summary>
        /// 重写背景颜色属性
        /// </summary>
        public new Color BackColor {
            get { return backColor; }
            set {
                base.BackColor = Color.Transparent;
                backColor = value;
                backBrush = new SolidBrush(backColor);
                this.Invalidate();
            }
        }

        private Pen penEnter;
        private Pen penLeave;

        private bool mouseEnter = false;

        private bool isPopup = false;
        [Browsable(false), DefaultValue(false)]
        public bool IsPopup {
            get { return isPopup; }
            set { isPopup = value; }
        }

        private int radiu = 2;
        [Browsable(true), DefaultValue(2)]
        public int Radiu {
            get { return radiu; }
            set { radiu = value; }
        }

        protected Popup dropDown;

        private LxzhListControl dropDownControl;
        [Browsable(false)]
        public LxzhListControl DropDownControl {
            get { return dropDownControl; }
            set { dropDownControl = value; }
        }

        /// <summary>
        /// 滚动条
        /// </summary>
        private LxzhScrollBar scrollBar;

        [Browsable(false)]
        public List<LxzhComboBoxItem> ComboBoxItems {
            get { return dropDownControl.ComboBoxItems; }
        }

        private List<String> items;
        /// <summary>This property is not relevant for this class.</summary>
        [Browsable(true)]
        public List<String> Items {
            get { return items; }
            set { items = value; }
        }

        private int itemHeight = 20;
        /// <summary>This property is not relevant for this class.</summary>
        [Browsable(true), DefaultValue(20)]
        public int ItemHeight {
            get { return itemHeight; }
            set { itemHeight = value; }
        }

        private ControlProperties ctrlProperties;

        [Description("The properties that will be assigned to the checkboxes as default values.")]
        [Browsable(true)]
        public ControlProperties CtrlProperties {
            get { return ctrlProperties; }
            set { ctrlProperties = value; controlProperties_PropertyChanged(this, EventArgs.Empty); }
        }

        public event EventHandler ItemsChanged;

        public event EventHandler ValueChanged;

        public LxzhComboBox() {
            InitializeControlStyle();
            InitializeComponent();
            ctrlProperties = new ControlProperties();
            ctrlProperties.PropertyChanged += new EventHandler(controlProperties_PropertyChanged);
            
            dropDownControl = new LxzhListControl(this);
            this.ForeColor = dropDownControl.ForeColor;

            dropDown = new Popup(dropDownControl);
            dropDown.PopupHide += new EventHandler(dropDown_PopupHide);
            dropDown.Resizable = true;
            scrollBar = dropDownControl.ScrollBar;

            scrollBar.Scroll += new EventHandler(scrollBar_Scroll);
            scrollBar.ValueChanged += new EventHandler(scrollBar_ValueChanged);
            penEnter = new Pen(new SolidBrush(System.Drawing.SystemColors.Highlight), 1.4F);
            penLeave = new Pen(new SolidBrush(Color.White), 1.4F);
            items = new List<string>();
            base.Text = string.Empty;
        }

        public void SetItems(List<string> value) {
            if (value == null)
                throw new NullReferenceException("数据集为空值");
            items = value;
            int count = items.Count;
            dropDownControl.ItemCount = count;
            if (count > 10) {
                dropDownControl.ShowItemCount = 10;
                int itemHeight = 17;
                dropDownControl.Index = 0;
                scrollBar.Maximum = count * itemHeight;
                scrollBar.LargeChange = dropDownControl.ShowItemCount * dropDownControl.ShowItemCount * itemHeight / count;
                scrollBar.SmallChange = dropDownControl.ShowItemCount * itemHeight / count;
            } else {
                dropDownControl.ShowItemCount = count;
            }
            if (ItemsChanged != null) {
                ItemsChanged(this, new EventArgs());
            }
        }

        private void InitializeControlStyle() {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        private void controlProperties_PropertyChanged(object sender, EventArgs e) {
            foreach (LxzhComboBoxItem Item in ComboBoxItems)
                Item.ApplyProperties(CtrlProperties);
        }

        private void scrollBar_Scroll(object sender, EventArgs e) {

            //throw new NotImplementedException();
        }

        private void scrollBar_ValueChanged(object sender, EventArgs e) {
            int index = scrollBar.Value/scrollBar.SmallChange;
            if (index != dropDownControl.Index) {
                dropDownControl.Index = index;
                dropDownControl.Refresh();
            }
        }

        private void dropDown_PopupHide(object sender, EventArgs e) {
            isPopup = false;
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe) {

            Graphics g = pe.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(1, 1, this.Width - 2, this.Height - 2);
            GraphicsPath path = Util.GetRoundedRectPath(rect, radiu);
            g.FillPath(backBrush, path);

            g.DrawString(Text, this.Font, new SolidBrush(this.ForeColor), new PointF(3F, 3.5F));

            if (isPopup || (!isPopup && mouseEnter)) {
                g.DrawPath(penEnter, path);
                //g.DrawRectangle(penEnter, rect);
            } else {
                g.DrawPath(penLeave, path);
            }
            if (isPopup) {
                g.DrawImage(global::lxzh.Properties.Resources.arrowlist.Clone(new Rectangle(33, 0, 11, 11), System.Drawing.Imaging.PixelFormat.Format32bppPArgb), new Rectangle(this.Width - 18, 6, 11, 11));
            } else {
                g.DrawImage(global::lxzh.Properties.Resources.arrowlist.Clone(new Rectangle(11, 0, 11, 11), System.Drawing.Imaging.PixelFormat.Format32bppPArgb), new Rectangle(this.Width - 18, 6, 11, 11));
            }

            //Util.SetWindowRegion(this, radiu);
            //base.OnPaint(pe);
        }

        protected override void OnResize(EventArgs e) {
            Size Size = new Size(Width, DropDownControl.Height);
            dropDown.Size = Size;
            base.OnResize(e);
        }

        protected override void OnMouseEnter(EventArgs e) {
            mouseEnter = true;
            base.OnMouseEnter(e);
            this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e) {
            mouseEnter = false;
            base.OnMouseLeave(e);
            this.Invalidate();
        }

        protected override void OnClick(EventArgs e) {
            if (dropDown != null) {
                TimeSpan TimeSpan = DateTime.Now.Subtract(dropDown.LastClosedTimeStamp);
                if (TimeSpan.TotalMilliseconds > 200 && !isPopup)
                    ShowDropDown();
            }
            base.OnClick(e);
        }

        public void ShowDropDown() {
            if (dropDown != null) {
                isPopup = true;
                dropDown.Height = dropDownControl.ShowItemCount * ItemHeight + 4;
                dropDown.Show(this, this.ClientRectangle);
                this.Refresh();
            }
        }

        public void HideDropDown() {
            if (dropDown != null) {
                dropDown.Hide();
                isPopup = false;
            }
        }
    }
}
