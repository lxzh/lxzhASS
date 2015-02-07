using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace lxzh {
    /// <summary>
    /// Í¸Ã÷ÎÄ±¾¿ò
    /// </summary>
    public class LxzhAlphaTextBox : TextBox {
        #region private variables
        private uPictureBox pictureBox;
        private bool upToDate = false;
        private bool caretUpToDate = false;
        private Bitmap bitmap;
        private Bitmap alphaBitmap;

        private int fontHeight = 10;

        private System.Windows.Forms.Timer timer;

        private bool caretState = true;
        private bool paintedFirstTime = false;

        private Color backColor = Color.White;
        private int backAlpha = 10;

        protected override CreateParams CreateParams {
            get {
                CreateParams prams = base.CreateParams;
                if (Win32.LoadLibrary("msftedit.dll") != IntPtr.Zero) {
                    prams.ExStyle |= 0x020; // transparent 
                    prams.ClassName = "RICHEDIT50W";
                }
                return prams;
            }
        }
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        #endregion // end private variables

        #region public property overrides
        [DefaultValue(BorderStyle.None)]
        public new BorderStyle BorderStyle {
            get { return base.BorderStyle; }
            set {
                if(this.paintedFirstTime)
                    this.SetStyle(ControlStyles.UserPaint, false);

                base.BorderStyle = value;

                if(this.paintedFirstTime)
                    this.SetStyle(ControlStyles.UserPaint, true);

                this.bitmap = null;
                this.alphaBitmap = null;
                upToDate = false;
                this.Invalidate();
            }
        }

        public new Color BackColor {
            get {
                return Color.FromArgb(base.BackColor.R, base.BackColor.G, base.BackColor.B);;
            }
            set {
                backColor = value;
                base.BackColor = value;
                upToDate = false;
            }
        }
        public override bool Multiline {
            get { return base.Multiline; }
            set {
                if(this.paintedFirstTime)
                    this.SetStyle(ControlStyles.UserPaint, false);

                base.Multiline = value;

                if(this.paintedFirstTime)
                    this.SetStyle(ControlStyles.UserPaint, true);

                this.bitmap = null;
                this.alphaBitmap = null;
                upToDate = false;
                this.Invalidate();
            }
        }

        #endregion    //end public property overrides

        #region public methods and overrides
        public LxzhAlphaTextBox() {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            // TODO: Add any initialization after the InitializeComponent call
            
            this.BackColor = backColor;
            base.BorderStyle = BorderStyle.None;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, false);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);

            pictureBox = new uPictureBox();
            this.Controls.Add(pictureBox);
            pictureBox.Dock = DockStyle.Fill;
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            this.bitmap = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);//(this.Width,this.Height);
            this.alphaBitmap = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);//(this.Width,this.Height);
            upToDate = false;
            this.Invalidate();
        }

        //Some of these should be moved to the WndProc later
        protected override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);
            upToDate = false;
            this.Invalidate();
        }

        protected override void OnKeyUp(KeyEventArgs e) {
            base.OnKeyUp(e);
            upToDate = false;
            this.Invalidate();
        }

        protected override void OnKeyPress(KeyPressEventArgs e) {
            base.OnKeyPress(e);
            upToDate = false;
            this.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            base.OnMouseUp(e);
            this.Invalidate();
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs gfbevent) {
            base.OnGiveFeedback(gfbevent);
            upToDate = false;
            this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e) {
            Point ptCursor = Cursor.Position;

            Form f = this.FindForm();
            ptCursor = f.PointToClient(ptCursor);
            if(!this.Bounds.Contains(ptCursor))
                base.OnMouseLeave(e);
        }

        protected override void OnChangeUICues(UICuesEventArgs e) {
            base.OnChangeUICues(e);
            upToDate = false;
            this.Invalidate();
        }

        protected override void OnGotFocus(EventArgs e) {
            base.OnGotFocus(e);
            caretUpToDate = false;
            upToDate = false;
            this.Invalidate();

            timer = new System.Windows.Forms.Timer(this.components);
            timer.Interval = (int)Win32.GetCaretBlinkTime(); //  usually around 500;

            timer.Tick += new EventHandler(myTimer1_Tick);
            timer.Enabled = true;
        }

        protected override void OnLostFocus(EventArgs e) {
            base.OnLostFocus(e);
            caretUpToDate = false;
            upToDate = false;
            this.Invalidate();

            timer.Dispose();
        }

        protected override void OnFontChanged(EventArgs e) {
            if(this.paintedFirstTime)
                this.SetStyle(ControlStyles.UserPaint, false);
            base.OnFontChanged(e);
            if(this.paintedFirstTime)
                this.SetStyle(ControlStyles.UserPaint, true);
            fontHeight = GetFontHeight();

            upToDate = false;
            this.Invalidate();
        }

        protected override void OnTextChanged(EventArgs e) {
            base.OnTextChanged(e);
            upToDate = false;
            this.Invalidate();
        }

        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);

            // need to rewrite as a big switch
            if(m.Msg == Win32.WM_PAINT) {
                paintedFirstTime = true;

                if(!upToDate || !caretUpToDate)
                    GetBitmaps();
                upToDate = true;
                caretUpToDate = true;

                if(pictureBox.Image != null) pictureBox.Image.Dispose();
                pictureBox.Image = (Image)alphaBitmap.Clone();
            } else if(m.Msg == Win32.WM_HSCROLL || m.Msg == Win32.WM_VSCROLL) {
                upToDate = false;
                this.Invalidate();
            } else if(m.Msg == Win32.WM_LBUTTONDOWN
                  || m.Msg == Win32.WM_RBUTTONDOWN
                  || m.Msg == Win32.WM_LBUTTONDBLCLK
                //  || m.Msg == win32.WM_MOUSELEAVE  ///****
                  ) {
                upToDate = false;
                this.Invalidate();
            } else if(m.Msg == Win32.WM_MOUSEMOVE) {
                if(m.WParam.ToInt32() != 0)  //shift key or other buttons
				{
                    upToDate = false;
                    this.Invalidate();
                }
            }
            //System.Diagnostics.Debug.WriteLine("Pro: " + m.Msg.ToString("X"));
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing) {
            if(disposing) {
                //this.BackColor = Color.Pink;
                if(components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion		//end public method and overrides

        #region private functions and classes

        private int GetFontHeight() {
            Graphics g = this.CreateGraphics();
            SizeF sf_font = g.MeasureString("X", this.Font);
            g.Dispose();
            return (int)sf_font.Height;
        }

        private void GetBitmaps() {
            if(bitmap == null
                || alphaBitmap == null
                || bitmap.Width != Width
                || bitmap.Height != Height
                || alphaBitmap.Width != Width
                || alphaBitmap.Height != Height) {
                bitmap = null;
                alphaBitmap = null;
            }
            if(bitmap == null) {
                bitmap = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);//(Width,Height);
                upToDate = false;
            }

            if(!upToDate) {
                //Capture the TextBox control window

                this.SetStyle(ControlStyles.UserPaint, false);

                Win32.CaptureWindow(this, ref bitmap);

                this.SetStyle(ControlStyles.UserPaint, true);
                this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                this.BackColor = Color.FromArgb(backAlpha, backColor);
            }	

            Rectangle r2 = new Rectangle(0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height);
            ImageAttributes tempImageAttr = new ImageAttributes();

            //Found the color map code in the MS Help

            ColorMap[] tempColorMap = new ColorMap[1];
            tempColorMap[0] = new ColorMap();
            tempColorMap[0].OldColor = Color.FromArgb(255, backColor);
            tempColorMap[0].NewColor = Color.FromArgb(backAlpha, backColor);

            tempImageAttr.SetRemapTable(tempColorMap);

            if(alphaBitmap != null)
                alphaBitmap.Dispose();

            alphaBitmap = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);//(Width,Height);
            Graphics tempGraphics1 = Graphics.FromImage(alphaBitmap);
            tempGraphics1.DrawImage(bitmap, r2, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, GraphicsUnit.Pixel, tempImageAttr);
            tempGraphics1.Dispose();

            if(this.Focused && (this.SelectionLength == 0)) {
                Graphics tempGraphics2 = Graphics.FromImage(alphaBitmap);
                if(caretState) {
                    //Draw the caret
                    Point caret = this.findCaret();
                    Pen p = new Pen(this.ForeColor, 3);
                    tempGraphics2.DrawLine(p, caret.X, caret.Y + 0, caret.X, caret.Y + fontHeight);
                    tempGraphics2.Dispose();
                }
            }
        }

        private Point findCaret() {
            /*  Find the caret translated from code at 
             * http://www.vb-helper.com/howto_track_textbox_caret.html
             * and 
             * http://www.microbion.co.uk/developers/csharp/textpos2.htm
             * Changed to EM_POSFROMCHAR
             * This code still needs to be cleaned up and debugged
             * */
            Point pointCaret = new Point(0);
            int i_char_loc = this.SelectionStart;
            IntPtr pi_char_loc = new IntPtr(i_char_loc);

            int i_point = Win32.SendMessage(this.Handle, Win32.EM_POSFROMCHAR, pi_char_loc, IntPtr.Zero);
            pointCaret = new Point(i_point);
            if(i_char_loc == 0) {
                pointCaret = new Point(0);
            } else if(i_char_loc >= this.Text.Length) {
                pi_char_loc = new IntPtr(i_char_loc - 1);
                i_point = Win32.SendMessage(this.Handle, Win32.EM_POSFROMCHAR, pi_char_loc, IntPtr.Zero);
                pointCaret = new Point(i_point);

                Graphics g = this.CreateGraphics();
                String t1 = this.Text.Substring(this.Text.Length - 1, 1) + "X";
                SizeF sizet1 = g.MeasureString(t1, this.Font);
                SizeF sizex = g.MeasureString("X", this.Font);
                g.Dispose();
                int xoffset = (int)(sizet1.Width - sizex.Width);
                pointCaret.X = pointCaret.X + xoffset;

                if(i_char_loc == this.Text.Length) {
                    String slast = this.Text.Substring(Text.Length - 1, 1);
                    if(slast == "\n") {
                        pointCaret.X = 1;
                        pointCaret.Y = pointCaret.Y + fontHeight;
                    }
                }
            }
            return pointCaret;
        }

        private void myTimer1_Tick(object sender, EventArgs e) {
            //Timer used to turn caret on and off for focused control
            caretState = !caretState;
            caretUpToDate = false;
            this.Invalidate();
        }

        private class uPictureBox : PictureBox {
            public uPictureBox() {
                this.SetStyle(ControlStyles.Selectable, false);
                this.SetStyle(ControlStyles.UserPaint, true);
                this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                this.SetStyle(ControlStyles.DoubleBuffer, true);

                this.Cursor = null;
                this.Enabled = true;
                this.SizeMode = PictureBoxSizeMode.Normal;
            }

            //uPictureBox
            protected override void WndProc(ref Message m) {
                if(m.Msg == Win32.WM_LBUTTONDOWN
                    || m.Msg == Win32.WM_RBUTTONDOWN
                    || m.Msg == Win32.WM_LBUTTONDBLCLK
                    || m.Msg == Win32.WM_MOUSELEAVE
                    || m.Msg == Win32.WM_MOUSEMOVE) {
                    //Send the above messages back to the parent control
                    Win32.PostMessage(this.Parent.Handle, (uint)m.Msg, m.WParam, m.LParam);
                } else if(m.Msg == Win32.WM_LBUTTONUP) {
                    //??  for selects and such
                    this.Parent.Invalidate();
                }
                base.WndProc(ref m);
            }
        }   // End uPictureBox Class

        #endregion  // end private functions and classes

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            components = new System.ComponentModel.Container();
        }
        #endregion

        #region New Public Properties

        [
        Category("Appearance"),
        Description("The alpha value used to blend the control's background. Valid values are 0 through 255."),
        Browsable(true),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
        ]
        public int BackAlpha {
            get { return backAlpha; }
            set {
                int v = value;
                if(v > 255)
                    v = 255;
                backAlpha = v;
                upToDate = false;
                Invalidate();
            }
        }
        #endregion
    }
}