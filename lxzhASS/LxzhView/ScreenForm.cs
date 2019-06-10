using System;
using System.Drawing;
using System.Windows.Forms;

namespace lxzh
{
    public partial class ScreenForm : Form
    {
        private Rectangle rect;

        public ScreenForm()
        {
            InitializeComponent();
        }
        public ScreenForm(Rectangle rect) {
            InitializeComponent();
            this.rect = rect;
        }

        private void ScreenForm_Load(object sender, EventArgs e)
        {
            if (rect.Left < 0) {
                rect.Width += rect.Left;
                rect.X = 0;
            }
            if (rect.Top < 0) {
                rect.Height += rect.Top;
                rect.Y = 0;
            }
            Bitmap bmp = new Bitmap(rect.Width, rect.Height);
            Graphics g = Graphics.FromImage(bmp);
            Console.WriteLine(string.Format("rect({0},{1},{2},{3})",rect.Left, rect.Top, rect.Width,rect.Height));
            g.CopyFromScreen(rect.Left, rect.Top, 0, 0, new Size(rect.Width, rect.Height));

            string filename = Util.GetSavePicPath();
            bmp.Save(filename);
            Util.SaveRectInfo(rect);
            Clipboard.SetImage(bmp);

            //Toast toast = new Toast(2, string.Format("已截图保存至：\n{0}", filename));
            //toast.StartPos = ContentAlignment.BottomRight;
            //toast.Show();
        }
    }
}
