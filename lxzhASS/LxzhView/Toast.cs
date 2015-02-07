using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace lxzh
{
    public partial class Toast : Form
    {
        private int livingTime=3;
        private string diaplayText = "";
        private DateTime startTime;
        private int formHeight;
        private int formWidth;
        private int line=1;
        private ContentAlignment startPos=ContentAlignment.MiddleCenter;

        public ContentAlignment StartPos
        {
            get { return startPos; }
            set { startPos = value; }
        }

        public void setTime(int time)
        {
            this.livingTime = time*1000;
        }
        public void setStr(string str)
        {
            this.diaplayText = str;
        }
        public Toast()
        {
            InitializeComponent();
            timer.Enabled = true;
        }
        public Toast(int time, string str)
        {
            InitializeComponent();
            timer.Enabled = true;
            this.livingTime = time*1000;
            this.diaplayText = str;
        }

        public void initForm()
        {
            while (diaplayText.Contains("\n "))
            {
                diaplayText = diaplayText.Replace("\n ", "\n");
            }
            string[] textArray = diaplayText.Split('\n');
            line = textArray.Count();           
            Font stringFont = new Font("微软雅黑", 12);
            Graphics g = lblDisplayText.CreateGraphics();
            int width = 0;
            for (int i = 0; i < line; i++)
            {
                width=(int)(g.MeasureString(textArray[i], stringFont).Width*0.6) ;
                if (width > this.formWidth)
                    this.formWidth = width;
                else continue;
            }
            if (line > 10)
            {
                this.formHeight = 10 * (int)g.MeasureString(textArray[0], stringFont).Height;
                FileStream fs=File.Create("ToastMessage.txt");
                fs.Write(System.Text.Encoding.Default.GetBytes(diaplayText), 0, diaplayText.Length);
                fs.Close();
                Process.Start("NOTEPAD.exe ",System.Environment.CurrentDirectory+"\\ToastMessage.txt");
            }
            else this.formHeight = line * (int)g.MeasureString(textArray[0], stringFont).Height;
            this.Width += formWidth;
            this.Height += formHeight;
            setStartPos();
        }

        private void setStartPos()
        {
            Rectangle rect = Screen.PrimaryScreen.Bounds;
            switch (startPos)
            {
                case ContentAlignment.TopLeft:
                    this.Left = 0;
                    this.Top = 0;
                    break;
                case ContentAlignment.MiddleLeft:
                    this.Left = 0;
                    this.Top = (rect.Height-this.Height)/2;
                    break;
                case ContentAlignment.BottomLeft:
                    this.Left = 0;
                    this.Top = rect.Height-this.Height;
                    break;
                case ContentAlignment.TopCenter:
                    this.Left = (rect.Width - this.Width) / 2;
                    this.Top = 0;
                    break;
                case ContentAlignment.MiddleCenter:
                    this.Left = (rect.Width - this.Width) / 2;
                    this.Top = (rect.Height - this.Height) / 2;
                    break;
                case ContentAlignment.BottomCenter:
                    this.Left = (rect.Width - this.Width) / 2;
                    this.Top = rect.Height - this.Height;
                    break;
                case ContentAlignment.TopRight:
                    this.Left = rect.Width-this.Width;
                    this.Top = 0;
                    break;
                case ContentAlignment.MiddleRight:
                    this.Left = rect.Width - this.Width;
                    this.Top = (rect.Height - this.Height) / 2;
                    break;
                case ContentAlignment.BottomRight:
                    this.Left = rect.Width - this.Width;
                    this.Top = rect.Height - this.Height;
                    break;
            }
        }

        //绘制圆角
        public void SetWindowRegion()
        {
            System.Drawing.Drawing2D.GraphicsPath FormPath;
            FormPath = new System.Drawing.Drawing2D.GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);//this.Left-10,this.Top-10,this.Width-10,this.Height-10);                 
            FormPath = GetRoundedRectPath(rect, 30);
            this.Region = new Region(FormPath);
        }

        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            int diameter = radius;
            Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
            GraphicsPath path = new GraphicsPath();
            //   左上角   
            path.AddArc(arcRect, 180, 90);
            //   右上角   
            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270, 90);
            //   右下角   
            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);
            //   左下角   
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void Toast_Load(object sender, EventArgs e)
        {
            initForm();
            Screen screen = Screen.PrimaryScreen;
            //this.Left = (screen.Bounds.Width - this.Width)/2;
            //this.Top = (screen.Bounds.Height - this.Height) / 2;
            this.Region = null;
            SetWindowRegion();         
            startTime = DateTime.Now;
            timer_Tick(sender, e);
            this.lblDisplayText.Text = diaplayText;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if ((DateTime.Now - startTime).TotalMilliseconds >= livingTime)
            {
                timer.Stop();
                quit();     
            }
        }

        public void quit()
        {
            for (int i = 0; i < 50; i++)
            {
                Thread.Sleep(10);
                this.Opacity -= 0.02;
            }
            this.Close();
        }
    }
}
