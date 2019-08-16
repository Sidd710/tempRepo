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
    public partial class SupportConeContextMenuStripControl : UserControl
    {
        public event EventHandler OnSelectMenuItem;

        private readonly int defaultItemHeight = 48;
        public SupportConeContextMenuStripControl()
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

            var deleteSupportCone = new ContextMenuItem("Delete Support Cone");
            deleteSupportCone.Left = 0;
            deleteSupportCone.Top = summaryHeight;
            deleteSupportCone.OnClick += deleteSupportCone_OnClick;
            this.plMenuList.Controls.Add(deleteSupportCone);
            summaryHeight += defaultItemHeight;
            this.Height = summaryHeight;


        }
        private void undoModel_OnClick(object sender, EventArgs e)
        {
            this.Close();
            OnSelectMenuItem?.Invoke(sender, e);
        }

        private void deleteSupportCone_OnClick(object sender, EventArgs e)
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
