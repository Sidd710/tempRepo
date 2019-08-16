using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TestApp
{
    public partial class menuItemTable : Form
    {
        public menuItemTable()
        {
            InitializeComponent();

            this.BackColor = Color.White;
            this.StartPosition = FormStartPosition.Manual;
            this.SuspendLayout();
            var amountOfPanels = 20;
            var panelSizeX =  this.Width / amountOfPanels;
            var panelSizeY = panelSizeX;
            var panelPadding = 2;
            panelSizeX = panelSizeY = panelSizeX - panelPadding;

            for (var xItemIndex = 0; xItemIndex < amountOfPanels; xItemIndex++)
            {
                for (var yItemIndex = 0; yItemIndex < amountOfPanels; yItemIndex++)
                {
                    var panel = new MenuItemTableCell();
                    panel.Location = new Point((xItemIndex * panelSizeX) + (xItemIndex * panelPadding), (yItemIndex * panelSizeX) + (yItemIndex * panelPadding));
                    panel.Size = new Size(panelSizeX, panelSizeX);
                    panel.BackColor = Color.White;
                    panel.ColumnIndex = xItemIndex;
                    panel.RowIndex = yItemIndex;
                    panel.MouseHover += panel_MouseHover;
                    panel.MouseMove += panel_MouseHover;
                    panel.BorderStyle = BorderStyle.FixedSingle;
                    this.plTable.Controls.Add(panel);
                }
            }

            this.ResumeLayout();
        }

        void panel_MouseHover(object sender, EventArgs e)
        {
            MenuItemTableCell hoveredPanel = null;
            if (sender != null && sender is MenuItemTableCell){
                hoveredPanel = sender as MenuItemTableCell;
            }

            if (hoveredPanel != null)
            {
                this.UpdateSelection(hoveredPanel);
            }
        }

        void UpdateControls()
        {

        }

        void UpdateSelection(MenuItemTableCell menuItemTableCell)
        {
            if (menuItemTableCell != null)
            {
                var selectedRowIndex = menuItemTableCell.RowIndex;
                var selectedColumnIndex = menuItemTableCell.ColumnIndex;

                this.label1.Text = string.Format("X: {0} - Y: {1} Clones", selectedRowIndex + 1, selectedColumnIndex + 1);

                foreach (MenuItemTableCell menuItemCell in this.plTable.Controls)
                {
                    if (menuItemCell.RowIndex <= selectedRowIndex &&
                        menuItemCell.ColumnIndex <= selectedColumnIndex &&
                        !menuItemCell.Selected)
                    {
                        menuItemCell.Selected = true;
                    }
                    else if (menuItemCell.RowIndex > selectedRowIndex && menuItemCell.Selected)
                    {
                        menuItemCell.Selected = false;
                    }
                    else if (menuItemCell.ColumnIndex > selectedColumnIndex && menuItemCell.Selected)
                    {
                        menuItemCell.Selected = false;
                    }
                }
            }
        }

    }

    public class MenuItemTableCell: Panel
    {
        private bool _selected;

        public int ColumnIndex { get; set; }
        public int RowIndex { get; set; }
        public bool Selected
        {
            get
            {
                return this._selected;
            }
            set
            {
                this.BackColor = value ? Color.Orange : Color.White;

                this._selected = value;
            }
        }

    }
}
