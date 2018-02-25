using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lxzh
{
    public partial class PinForm : Form
    {
        private string codeTxt=null;
        public string CodeTxt {
            get { return codeTxt; }
            set { codeTxt = value; }
        }

        private ToolStripMenuItem preOpacityCheckItem = null;

        public PinForm()
        {
            InitializeComponent();
        }

        const int LXZH_HTLEFT = 10;
        const int LXZH_HTRIGHT = 11;
        const int LXZH_HTTOP = 12;
        const int LXZH_HTTOPLEFT = 13;
        const int LXZH_HTTOPRIGHT = 14;
        const int LXZH_HTBOTTOM = 15;
        const int LXZH_HTBOTTOMLEFT = 0x10;
        const int LXZH_HTBOTTOMRIGHT = 17;

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0084:
                    base.WndProc(ref m);
                    Point vPoint = new Point((int)m.LParam & 0xFFFF,
                        (int)m.LParam >> 16 & 0xFFFF);
                    vPoint = PointToClient(vPoint);
                    if (vPoint.X <= 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)LXZH_HTTOPLEFT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)LXZH_HTBOTTOMLEFT;
                        else m.Result = (IntPtr)LXZH_HTLEFT;
                    else if (vPoint.X >= ClientSize.Width - 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)LXZH_HTTOPRIGHT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)LXZH_HTBOTTOMRIGHT;
                        else m.Result = (IntPtr)LXZH_HTRIGHT;
                    else if (vPoint.Y <= 5)
                        m.Result = (IntPtr)LXZH_HTTOP;
                    else if (vPoint.Y >= ClientSize.Height - 5)
                        m.Result = (IntPtr)LXZH_HTBOTTOM;
                    break;
                case 0x0201:                //鼠标左键按下的消息   
                    m.Msg = 0x00A1;         //更改消息为非客户区按下鼠标   
                    m.LParam = IntPtr.Zero; //默认值   
                    m.WParam = new IntPtr(2);//鼠标放在标题栏内   
                    base.WndProc(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        private void PinForm_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(codeTxt)) {
                rtbCode.Text = codeTxt;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsmiTopMost_Click(object sender, EventArgs e)
        {
            this.TopMost = !this.TopMost;
            tsmiTopMost.Checked = this.TopMost;
        }

        private void tsmiWordWrap_Click(object sender, EventArgs e)
        {
            rtbCode.WordWrap = !rtbCode.WordWrap;
            tsmiWordWrap.Checked = rtbCode.WordWrap;
        }

        private void tsmiOpacity_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            string name = item.Name;
            int opacity = 100;
            int.TryParse(name.Replace("tsmiOpacity", ""), out opacity);
            if (preOpacityCheckItem != null)
            {
                preOpacityCheckItem.Checked = false;
                if (preOpacityCheckItem != item)
                {
                    preOpacityCheckItem = item;
                    preOpacityCheckItem.Checked = true;
                }
                else
                {
                    preOpacityCheckItem = null;
                }
            }
            else
            {
                preOpacityCheckItem = item;
                preOpacityCheckItem.Checked = true;
            }
            this.Opacity = opacity/100.0D;
        }
    }
}
