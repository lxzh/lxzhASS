using System;
using System.Drawing;
using System.Windows.Forms;

namespace lxzh.IBASadd
{
    class FormExt
    {
        static public double MouseEnter { get; set; } = 0.8;
        static public double MouseLeave { get; set; } = 0.5;
        /// <summary>
             /// 为窗体添加拖动效果
             /// </summary>
             /// <param name="Obj">窗体对象(this)</param>
        static public void AddMoveEvent(Object Obj)
        {
            //Object Obj = type.Assembly.CreateInstance(type.ToString());
            Point mouseOff = new Point();
            bool leftFlag = false;
            ((ContainerControl)Obj).MouseDown += (sender, e) => {
                if (e.Button == MouseButtons.Left)
                {
                    mouseOff = new Point(-e.X, -e.Y);
                    leftFlag = true;
                }
            };
            ((ContainerControl)Obj).MouseMove += (sender, e) =>
            {
                if (leftFlag)
                {
                    Point mouseSet = Control.MousePosition;
                    mouseSet.Offset(mouseOff.X, mouseOff.Y);  //calculate the last point
                    ((ContainerControl)sender).Location = mouseSet;
                }
            };
            ((ContainerControl)Obj).MouseUp += (sender, e) =>
            {
                if (leftFlag)
                {
                    leftFlag = false;
                }
            };
        }

        /// <summary>
        /// 设置窗体透明
        /// </summary>
        /// <param name="Obj">窗体对象(this)</param>
        /// <param name="mouseEnter">鼠标进入窗体时的透明度(默认为负数)</param>
        /// <param name="mouseLeave">鼠标离开窗体时的透明度(默认为负数)</param>
        static public void TransparentForm(Object Obj, double mouseEnter, double mouseLeave)
        {
            if (mouseEnter > 0)
            {
                MouseEnter = mouseEnter;
            }
            if (mouseLeave > 0)
            {
                MouseLeave = mouseLeave;
            }
            ((ContainerControl)Obj).MouseEnter += (sender, e) =>
            {
                ((Form)sender).Opacity = MouseEnter;
            };
            ((ContainerControl)Obj).MouseLeave += (sender, e) =>
            {
                ((Form)sender).Opacity = MouseLeave;
            };
            TransparentForm(Obj);
        }
        /// <summary>
        /// 设置窗体透明（仅透明）
        /// </summary>
        /// <param name="Obj">窗体对象(this)</param>
        static public void TransparentForm(Object Obj)
        {
            ((ContainerControl)Obj).BackColor = Color.FromArgb(116, 253, 107);
            ((Form)Obj).Opacity = MouseEnter;
        }
    }
}
