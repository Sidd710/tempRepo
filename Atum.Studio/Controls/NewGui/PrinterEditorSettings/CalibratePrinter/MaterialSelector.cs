using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atum.DAL.Materials;
using System.Drawing.Drawing2D;

namespace Atum.Studio.Controls.NewGui.PrinterEditorSettings.CalibratePrinter
{
    class MaterialSelector : ComboBox
    {
        public MaterialSelector()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        // Draws the items into the ColorSelector object
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            if (e.Index > -1)
            {
                var item = new DropDownMaterialItem((Material)Items[e.Index]);
                e.Graphics.DrawString(item.Value.DisplayName, e.Font, new SolidBrush(e.ForeColor), e.Bounds.Left + 28, e.Bounds.Top + 6);
                e.Graphics.FillEllipse(new SolidBrush(Color.White), e.Bounds.Left + 6, e.Bounds.Top + 6, 16, 16);
                e.Graphics.FillEllipse(new SolidBrush(item.Value.ModelColor), e.Bounds.Left + 8, e.Bounds.Top + 8, 12, 12);
            }

            //base.OnDrawItem(e);
        }
    }

    public class DropDownMaterialItem
    {
        public Material Value
        {
            get { return _value; }
            set { this._value = value; }
        }
        private Material _value;

        public DropDownMaterialItem() : this(new Material())
        { }

        public DropDownMaterialItem(Material value)
        {
            _value = value;
          //  this.img = new Bitmap(16, 16);
            //Graphics g = Graphics.FromImage(img);
            //Brush b = new SolidBrush(Color.FromName(val));
            //g.DrawRectangle(Pens.Red, 0, 0, 16, 16);
            //g.FillRectangle(b, 1, 1, img.Width - 1, img.Height - 1);
        }

        public override string ToString()
        {
            return this._value.DisplayName;
        }
    }
}
