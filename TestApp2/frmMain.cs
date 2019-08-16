using Atum.Studio.Core.Managers;
using Examples.TextureLoaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace TestApp2
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(382, 92);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(382, 188);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new System.Drawing.Point(59, 92);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(200, 144);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // frmMain
            // 
            this.ClientSize = new System.Drawing.Size(654, 523);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "frmMain";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);

        }

        int _selectedIndex = 0;
        public void frmMain_Load(object sender, EventArgs e)
        {
            var test = new Atum.PrinterClient.Core.Menu.LCDMenu();
            test.MenuItems.Add(new Atum.PrinterClient.Core.Menu.MenuItemInfo(Atum.PrinterClient.Core.Menu.typeMenuItem.Selectable, "test"));
           // test.MenuItems.Add(new Atum.PrinterClient.Core.Menu.MenuItemInfo(Atum.PrinterClient.Core.Menu.typeMenuItem.Selectable, "test2"));
            //test.MenuItems.Add(new Atum.PrinterClient.Core.Menu.MenuItemInfo(Atum.PrinterClient.Core.Menu.typeMenuItem.Selectable, "test3"));
           // test.MenuItems.Add(new Atum.PrinterClient.Core.Menu.MenuItemInfo(Atum.PrinterClient.Core.Menu.typeMenuItem.Selectable, "test4"));
           // test.MenuItems.Add(new Atum.PrinterClient.Core.Menu.MenuItemInfo(Atum.PrinterClient.Core.Menu.typeMenuItem.Selectable, "test5"));
            test.MenuItems.Add(new Atum.PrinterClient.Core.Menu.MenuItemInfo(Atum.PrinterClient.Core.Menu.typeMenuItem.EnterEsc, "enter"));

            foreach(var item in test.MenuItems)
            {
                var label = new Label() { Text = item.Text, Tag = item };
                this.flowLayoutPanel1.Controls.Add(label);
            }

            test.MenuItems[_selectedIndex].Selected = true;

            RefreshControls();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var lastMenuItem = this.flowLayoutPanel1.Controls[this.flowLayoutPanel1.Controls.Count - 1].Tag as Atum.PrinterClient.Core.Menu.MenuItemInfo;

            if (_selectedIndex > 0)
            {
                _selectedIndex--;
            }
            else if (lastMenuItem.Type != Atum.PrinterClient.Core.Menu.typeMenuItem.NotSelectable)
            {
                _selectedIndex = this.flowLayoutPanel1.Controls.Count - 2;
            }
            else
            {
                _selectedIndex = this.flowLayoutPanel1.Controls.Count - 1;
            }

            RefreshControls();
        }

        public void RefreshControls()
        {
            //reset selection
            foreach(Control control in this.flowLayoutPanel1.Controls)
            {
                control.Visible = false;
                control.BackColor = Color.White;
                var menuItem = control.Tag as Atum.PrinterClient.Core.Menu.MenuItemInfo;
                menuItem.Selected = false;
            }

            var selectedControl = this.flowLayoutPanel1.Controls[_selectedIndex];
            selectedControl.Visible = true;
            selectedControl.BackColor = Color.Red;

            var lastMenuItem = this.flowLayoutPanel1.Controls[this.flowLayoutPanel1.Controls.Count - 1].Tag as Atum.PrinterClient.Core.Menu.MenuItemInfo;

            if (_selectedIndex == 0)
            {
                this.flowLayoutPanel1.Controls[_selectedIndex + 1].Visible = true;
                if (_selectedIndex - 2 >= 0)
                {
                    this.flowLayoutPanel1.Controls[_selectedIndex + 2].Visible = true;
                }
            }
            else if (_selectedIndex == this.flowLayoutPanel1.Controls.Count - 2 && lastMenuItem.Type != Atum.PrinterClient.Core.Menu.typeMenuItem.NotSelectable)
            {
                this.flowLayoutPanel1.Controls[_selectedIndex - 1].Visible = true;
                if (_selectedIndex - 2 >= 0)
                {
                    this.flowLayoutPanel1.Controls[_selectedIndex - 2].Visible = true;
                }
            }
            else if (_selectedIndex == this.flowLayoutPanel1.Controls.Count - 3 && lastMenuItem.Type != Atum.PrinterClient.Core.Menu.typeMenuItem.NotSelectable)
                {
                this.flowLayoutPanel1.Controls[_selectedIndex - 1].Visible = true;
                this.flowLayoutPanel1.Controls[_selectedIndex + 1].Visible = true;
            }
            else
            {
                this.flowLayoutPanel1.Controls[_selectedIndex - 1].Visible = true;
                this.flowLayoutPanel1.Controls[_selectedIndex + 1].Visible = true;
            }

            
            if (lastMenuItem.Type != Atum.PrinterClient.Core.Menu.typeMenuItem.NotSelectable)
            {
                this.flowLayoutPanel1.Controls[this.flowLayoutPanel1.Controls.Count - 1].Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var lastMenuItem = this.flowLayoutPanel1.Controls[this.flowLayoutPanel1.Controls.Count - 1].Tag as Atum.PrinterClient.Core.Menu.MenuItemInfo;

            if (lastMenuItem.Type != Atum.PrinterClient.Core.Menu.typeMenuItem.NotSelectable && _selectedIndex == this.flowLayoutPanel1.Controls.Count - 2) 
            {
                _selectedIndex = 0;
            }
            else if (_selectedIndex < this.flowLayoutPanel1.Controls.Count - 1)
            {
                _selectedIndex++;
            }
            else
            {
                _selectedIndex = 0;
            }

            RefreshControls();
        }
    }
}

