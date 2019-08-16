using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Atum.Studio.Controls
{
    public partial class BasePopup : Form
    {
        private string _btnOKText = "OK";
        public string btnOKText { get { return this._btnOKText; } set { this._btnOKText = value; this.btnOK.Text = this._btnOKText; } }
        public bool btnCancelHidden { get; set; }

        public BasePopup()
        {
            InitializeComponent();

            if (DAL.OS.OSProvider.IsOSX)
            {
                this.plFooter.BackColor = Color.Red;
                this.plFooter.Height += 20;
            }
        }

        internal virtual void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void plFooter_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.DarkGray), new Point(0, 0), new Point(this.plFooter.Width, 0));
        }
    }
}
