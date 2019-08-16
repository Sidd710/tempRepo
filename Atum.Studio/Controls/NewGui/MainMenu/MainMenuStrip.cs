using Atum.Studio.Core.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls.NewGui.MainMenu
{
    public partial class MainMenuStrip : UserControl
    {
        public event EventHandler OnSelectMenuItem;

        private readonly int defaultItemHeight = 48;
        public MainMenuStrip()
        {
            InitializeComponent();

            LoadMenu();
            this.BackColor = BrandingManager.Menu_BackgroundColor;
        }

        private void LoadMenu()
        {
            this.plMenuList.Controls.Clear();
            var summaryHeight = 8;

            var newProjectItem = new MenuItemControl("New Project");
            newProjectItem.Left = 0;
            newProjectItem.Top = summaryHeight;
            newProjectItem.OnClick += newProjectItem_OnClick;
            this.plMenuList.Controls.Add(newProjectItem);
            summaryHeight += defaultItemHeight;

            var newOpentItem = new MenuItemControl("Open...");
            newOpentItem.Left = 0;
            newOpentItem.Top = summaryHeight;
            newOpentItem.OnClick += NewOpenItem_OnClick;
            this.plMenuList.Controls.Add(newOpentItem);
            summaryHeight += defaultItemHeight;

            var savetItem = new MenuItemControl("Save");
            savetItem.Left = 0;
            savetItem.Top = summaryHeight;
            savetItem.OnClick += savetItem_OnClick;
            this.plMenuList.Controls.Add(savetItem);
            summaryHeight += defaultItemHeight;

            var saveAsItem = new MenuItemControl("Save As...");
            saveAsItem.Left = 0;
            saveAsItem.Top = summaryHeight;
            saveAsItem.OnClick += saveAsItem_OnClick;
            this.plMenuList.Controls.Add(saveAsItem);
            //summaryHeight += defaultItemHeight;
        }

        private void newProjectItem_OnClick(object sender, EventArgs e)
        {
            OnSelectMenuItem?.Invoke(sender, e);
        }

        private void savetItem_OnClick(object sender, EventArgs e)
        {
            OnSelectMenuItem?.Invoke(sender, e);
        }

        private void NewOpenItem_OnClick(object sender, EventArgs e)
        {
            OnSelectMenuItem?.Invoke(sender, e);
        }

        private void saveAsItem_OnClick(object sender, EventArgs e)
        {
            OnSelectMenuItem?.Invoke(sender, e);
        }
        
    }
}
