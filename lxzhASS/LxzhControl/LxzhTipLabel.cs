using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace lxzh {
    /// <summary>
    /// 带提示信息的标签控件
    /// </summary>
    public partial class LxzhTipLabel : Label {
        private string tipInfo;
        /// <summary>
        /// 提示信息
        /// </summary>
        public string TipInfo {
            get { return tipInfo; }
            set { tipInfo = value; }
        }

        public LxzhTipLabel() {
            InitializeComponent();
        }

        private void LxzhTipLabel_MouseHover(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(tipInfo)) {
                toolTip.Show(tipInfo, this);
            }
        }

        private void LxzhTipLabel_MouseLeave(object sender, EventArgs e) {
            toolTip.Hide(this);
        }
    }
}
