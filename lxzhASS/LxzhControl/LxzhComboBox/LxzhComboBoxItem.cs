using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace lxzh {
    [ToolboxItem(false)]
    public partial class LxzhComboBoxItem : Label {
        #region CONSTRUCTOR

        /// <param name="owner">A reference to the LxzhComboBox.</param>
        /// <param name="text">A reference to the item in the ComboBox.Items that this object is extending.</param>
        public LxzhComboBoxItem(LxzhComboBox owner, string text)
            : base() {
            InitializeComponent();
            DoubleBuffered = true;
            lxzhComboBox = owner;
            Text = text;
        }
        #endregion

        #region PRIVATE PROPERTIES

        private bool isSelected = false;

        public bool IsSelected {
            get { return isSelected; }
            set { isSelected = value; }
        }

        /// <summary>
        /// A reference to the LxzhComboBox.
        /// </summary>
        private LxzhComboBox lxzhComboBox;

        #endregion

        #region PROTECTED MEMBERS

        protected override void OnClick(EventArgs e) {
            if (lxzhComboBox.Items != null) {
                lxzhComboBox.Text = this.Text;
                lxzhComboBox.HideDropDown();
                lxzhComboBox.Refresh();
            }
            base.OnClick(e);
        }

        protected override void OnMouseEnter(EventArgs e) {
            this.BackColor = System.Drawing.SystemColors.Highlight;
            this.ForeColor = Color.White;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e) {
            this.BackColor = Color.White;
            this.ForeColor = Color.Black;
            base.OnMouseLeave(e);
        }

        #endregion

        #region HELPER MEMBERS

        internal void ApplyProperties(ControlProperties properties) {
            this.AutoEllipsis = properties.AutoEllipsis;
            this.AutoSize = properties.AutoSize;
            this.FlatStyle = properties.FlatStyle;
            this.ForeColor = properties.ForeColor;
            this.RightToLeft = properties.RightToLeft;
            this.TextAlign = properties.TextAlign;
        }

        #endregion
    }
}
