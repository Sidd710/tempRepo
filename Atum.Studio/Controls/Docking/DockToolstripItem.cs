using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Atum.Studio.Controls.Docking
{
    public class DockToolstripItem: PictureBox
    {
        public bool Selected { get; set; }
        public Image ToolstripIconMouseOut { get; set; }
        public Image ToolstripIconMouseOver { get; set; }

        public DockToolstripItem()
        {
            this.BackgroundImageLayout = ImageLayout.Zoom;
            this.DoubleBuffered = true;
            this.MouseHover += imageButton_MouseHover;
            this.MouseLeave += imageButton_MouseLeave;
            this.Click += toolstripButton_Click;
        }

        public void SelectButton()
        {
            this.Selected = true;
            this.BackgroundImage = this.ToolstripIconMouseOver;
        }

        public void ResetHighlight(){
            this.BackgroundImage = this.ToolstripIconMouseOut;
            this.Selected = false;
        }

        void imageButton_MouseLeave(object sender, EventArgs e)
        {
            if (!this.Selected)
            {
                this.ResetHighlight();
            }
        }

        void imageButton_MouseHover(object sender, EventArgs e)
        {
            var pictureBox = sender as PictureBox;
            if (pictureBox.BackgroundImage != this.ToolstripIconMouseOver)
            {
                pictureBox.BackgroundImage = this.ToolstripIconMouseOver;
            }
        }

        void toolstripButton_Click(object sender, EventArgs e)
        {
            foreach (Control control in ((PictureBox)sender).Parent.Controls)
            {
                if (control is DockToolstripItem)
                {
                    var dockButton = control as DockToolstripItem;
                    dockButton.ResetHighlight();
                }
            }

            this.Selected = true;
            this.imageButton_MouseHover(this, null);

        }
    }
}
