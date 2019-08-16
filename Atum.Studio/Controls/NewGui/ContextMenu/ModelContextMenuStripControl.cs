using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atum.Studio.Core.Managers;

namespace Atum.Studio.Controls.NewGui.ContextMenu
{
    public partial class ModelContextMenuStripControl : UserControl
    {
        public event EventHandler OnSelectMenuItem;

        private readonly int defaultItemHeight = 48;
        public ModelContextMenuStripControl()
        {
            InitializeComponent();
            LoadMenu();
            this.BackColor = BrandingManager.Menu_BackgroundColor;
        }

        public void SetLocation(Point point)
        {
            this.Location = point;
        }
        private void LoadMenu()
        {
            this.plMenuList.Controls.Clear();
            var summaryHeight = 0;

            var undoAction = new ContextMenuItem("Undo");
            undoAction.Left = 0;
            undoAction.Top = summaryHeight;
            undoAction.OnClick += undoModel_OnClick;
            this.plMenuList.Controls.Add(undoAction);
            summaryHeight += defaultItemHeight;

            var deleteModel = new ContextMenuItem("Delete Model");
            deleteModel.Left = 0;
            deleteModel.Top = summaryHeight;
            deleteModel.OnClick += deleteModel_OnClick;
            this.plMenuList.Controls.Add(deleteModel);
            summaryHeight += defaultItemHeight;
            this.Height = summaryHeight;


        }
        private void undoModel_OnClick(object sender, EventArgs e)
        {
            this.Close();
            OnSelectMenuItem?.Invoke(sender, e);
        }

        private void deleteModel_OnClick(object sender, EventArgs e)
        {
            this.Close();
            OnSelectMenuItem?.Invoke(sender, e);
        }

        public bool ShowOnMouseUp { get; set; }

        public new void Show()
        {
            if (ShowOnMouseUp)
            {
                if (!frmStudioMain.SceneControl.Controls.Contains(this))
                {
                    frmStudioMain.SceneControl.Controls.Add(this);
                }

                Point pt = frmStudioMain.SceneControl.PointToClient(Control.MousePosition);
                this.SetLocation(pt);
                this.BringToFront();

                frmStudioMain.SceneControl.Render(); //prevent render issues
            }
        }

        public void Close(bool forceRender = false)
        {
            this.ShowOnMouseUp = false;
            if (frmStudioMain.SceneControl.Controls.Contains(this))
            {
                frmStudioMain.SceneControl.Controls.Remove(this);
                SceneControlToolbarManager.Redraw(frmStudioMain.SceneControl);
            }

            if (forceRender)
            {
                frmStudioMain.SceneControl.Render();
            }

        }
    }
}
