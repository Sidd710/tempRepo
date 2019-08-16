using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Models;
using System.Diagnostics;

namespace Atum.Studio.Controls.Docking
{
    public partial class DockPanelBase : Form
    {
        internal static Action<int, int> OnDockPanelResized;

        private bool _autoHide;
        private Form _mainForm;

        private bool _dragging;
        private Point _draggingOffset;

        public string Title { get; set; }
        public Image ToolstripIconMouseOut { get; set; }
        public Image ToolstripIconMouseOver { get; set; }

        public int DockingTop { get; set; }
        public int DockingHeight { get; set; }

        public DockToolstripItem ToolstripItem { get; set; }
        public bool AutoHide
        {
            get
            {
                return this._autoHide;
            }
            set
            {
                this._autoHide = value;

                if (this._autoHide)
                {
                    this.picAutoHide.Image = Properties.Resources.DockPane_AutoHide;
                }
                else
                {
                    this.picAutoHide.Image = Properties.Resources.DockPane_Dock;
                }
            }
        }

        public DockPanelBase()
        {
            InitializeComponent();

            //branding
            this.plTitle.BackColor = BrandingManager.DockPanelTitleBackground;

            this.AutoHide = true;

            this.Visible = false;
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.None;
          //  this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        public DockToolstripItem CreateToolStripButton(Panel parentPanel)
        {
            var topPosition = 5;
            foreach (Control control in parentPanel.Controls)
            {
                if (control is DockToolstripItem)
                {
                    var pictureBox = control as PictureBox;
                    topPosition = pictureBox.Top + pictureBox.Height + 5;
                }
            }
            this.ToolstripItem = new DockToolstripItem();
            this.ToolstripItem.Top = topPosition;
            this.ToolstripItem.Left = parentPanel.Width - this.ToolstripIconMouseOut.Width - 5;
            this.ToolstripItem.ToolstripIconMouseOut = this.ToolstripIconMouseOut;
            this.ToolstripItem.ToolstripIconMouseOver = this.ToolstripIconMouseOver;
            this.ToolstripItem.BackgroundImage = this.ToolstripIconMouseOut;
            this.ToolstripItem.Height = this.ToolstripIconMouseOut.Height;
            this.ToolstripItem.Width = this.Icon.Width + 5;
            this.ToolstripItem.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);

            this.ToolstripItem.Click += imageButton_Click;

            return this.ToolstripItem;
        }

        void imageButton_Click(object sender, EventArgs e)
        {
            //reset parent 
            this.ShowPanel(frmStudioMain.SceneControl.ParentForm);

        }

        internal void InitialiseLargeIcons(Panel parentPanel)
        {
            var userProfile = UserProfileManager.UserProfiles[0];
            var topPosition = 5;

            foreach (Control control in parentPanel.Controls)
            {
                if (control is DockToolstripItem)
                {
                    var pictureBox = control as PictureBox;

                    pictureBox.Height = userProfile.Settings_Studio_UseLargeToolbarIcons ? pictureBox.BackgroundImage.Height * 2 : pictureBox.BackgroundImage.Height;
                    pictureBox.Width = userProfile.Settings_Studio_UseLargeToolbarIcons ? pictureBox.BackgroundImage.Width * 2 : pictureBox.BackgroundImage.Width;
                    pictureBox.Top = topPosition;
                    pictureBox.Left = 5;
                    topPosition = pictureBox.Top + pictureBox.Height + 5;
                }
            }
        }

        private void DockPanelBase_Load(object sender, EventArgs e)
        {
            this.lblTitle.Text = this.Title;
            this.lblTitle.Font = new Font(FontFamily.GenericSansSerif, 12);

            this.plTitle.Height = this.lblTitle.Height + 6;
            this.plContent.Top = this.plTitle.Height;
        }

        internal void ShowPanel(Form parent)
        {
            this._mainForm = parent;
            try
            {

                frmStudioMain.NAVIGATIONPANEL.EnablePanel();
                frmStudioMain.MODELPROPERTIESPANEL.EnablePanel();
                frmStudioMain.SUPPORTPROPERTIESPANEL.EnablePanel();

                if (frmStudioMain.NAVIGATIONPANEL.InvokeRequired)
                {
                    frmStudioMain.NAVIGATIONPANEL.Invoke(new MethodInvoker(delegate
                    {
                        if (frmStudioMain.NAVIGATIONPANEL.AutoHide)
                        {
                            frmStudioMain.NAVIGATIONPANEL.Visible = false;
                            frmStudioMain.NAVIGATIONPANEL.ToolstripItem.ResetHighlight();
                        }
                        else
                        {

                        }
                    }));
                }
                else
                {
                    if (frmStudioMain.NAVIGATIONPANEL.AutoHide)
                    {
                        frmStudioMain.NAVIGATIONPANEL.Visible = false;
                        frmStudioMain.NAVIGATIONPANEL.ToolstripItem.ResetHighlight();
                    }
                    else
                    {

                    }
                }

                if (frmStudioMain.PRINTJOBPROPERTIESPANEL.InvokeRequired)
                {
                    frmStudioMain.PRINTJOBPROPERTIESPANEL.Invoke(new MethodInvoker(delegate
                    {
                        if (frmStudioMain.PRINTJOBPROPERTIESPANEL.AutoHide)
                        {
                            frmStudioMain.PRINTJOBPROPERTIESPANEL.Visible = false;
                            frmStudioMain.PRINTJOBPROPERTIESPANEL.ToolstripItem.ResetHighlight();
                        }
                        else
                        {
                     
                        }
                    }));
                }
                else
                {
                    if (frmStudioMain.PRINTJOBPROPERTIESPANEL.AutoHide)
                    {
                        frmStudioMain.PRINTJOBPROPERTIESPANEL.Visible = false;
                        frmStudioMain.PRINTJOBPROPERTIESPANEL.ToolstripItem.ResetHighlight();
                    }
                    else
                    {
                     
                    }
                }

                if (frmStudioMain.MODELPROPERTIESPANEL.InvokeRequired)
                {
                    frmStudioMain.MODELPROPERTIESPANEL.Invoke(new MethodInvoker(delegate
                    {
                        if (frmStudioMain.MODELPROPERTIESPANEL.AutoHide)
                        {
                            frmStudioMain.MODELPROPERTIESPANEL.Visible = false;
                            frmStudioMain.MODELPROPERTIESPANEL.ToolstripItem.ResetHighlight();
                        }
                        else
                        {
                        }
                    }));
                }
                else
                {
                    if (frmStudioMain.MODELPROPERTIESPANEL.AutoHide)
                    {
                        frmStudioMain.MODELPROPERTIESPANEL.Visible = false;
                        frmStudioMain.MODELPROPERTIESPANEL.ToolstripItem.ResetHighlight();
                    }
                    else
                    {
                    }
                }


                if (frmStudioMain.SUPPORTPROPERTIESPANEL.InvokeRequired)
                {
                    frmStudioMain.SUPPORTPROPERTIESPANEL.Invoke(new MethodInvoker(delegate
                    {
                        if (frmStudioMain.SUPPORTPROPERTIESPANEL.AutoHide)

                        {
                            frmStudioMain.SUPPORTPROPERTIESPANEL.Visible = false;
                            frmStudioMain.SUPPORTPROPERTIESPANEL.ToolstripItem.ResetHighlight();
                        }
                    }));
                }
                else
                {
                    if (frmStudioMain.SUPPORTPROPERTIESPANEL.AutoHide)
                    {
                        frmStudioMain.SUPPORTPROPERTIESPANEL.Visible = false;
                        frmStudioMain.SUPPORTPROPERTIESPANEL.ToolstripItem.ResetHighlight();
                    }
                }

                if (!this.Visible)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            this.Show(this.Parent);
                            this.Visible = true;
                            this.ToolstripItem.SelectButton();
                        }));
                    }
                    else
                    {
                        this.Show(parent);
                        this.Visible = true;
                        this.ToolstripItem.SelectButton();
                        //     this.Focus();
                    }
                }

                //navigation dock
                frmStudioMain.NAVIGATIONPANEL.Height = frmStudioMain.NAVIGATIONPANEL.MinimumSize.Height;

                //printjob dock
                frmStudioMain.PRINTJOBPROPERTIESPANEL.Height = frmStudioMain.PRINTJOBPROPERTIESPANEL.MinimumSize.Height;
                if (frmStudioMain.NAVIGATIONPANEL.Visible || !frmStudioMain.NAVIGATIONPANEL.AutoHide)
                {
                    frmStudioMain.PRINTJOBPROPERTIESPANEL.Location = new Point(frmStudioMain.NAVIGATIONPANEL.Left, frmStudioMain.NAVIGATIONPANEL.Top + frmStudioMain.NAVIGATIONPANEL.MinimumSize.Height);
                }

                //model dock
                frmStudioMain.MODELPROPERTIESPANEL.Height = frmStudioMain.MODELPROPERTIESPANEL.MinimumSize.Height;
                if (frmStudioMain.PRINTJOBPROPERTIESPANEL.Visible || !frmStudioMain.PRINTJOBPROPERTIESPANEL.AutoHide)
                {
                    frmStudioMain.MODELPROPERTIESPANEL.Location = new Point(frmStudioMain.PRINTJOBPROPERTIESPANEL.Left, frmStudioMain.PRINTJOBPROPERTIESPANEL.Top + frmStudioMain.PRINTJOBPROPERTIESPANEL.MinimumSize.Height);
                }
                
                //support dock
                int modelTop = 0;
                int supportTop = frmStudioMain.NAVIGATIONPANEL.Top;
                int printjobTop = 0;

                if (frmStudioMain.NAVIGATIONPANEL.Visible || !frmStudioMain.NAVIGATIONPANEL.AutoHide)
                {
                    supportTop += frmStudioMain.NAVIGATIONPANEL.MinimumSize.Height;
                }

                printjobTop = supportTop;

                if (frmStudioMain.PRINTJOBPROPERTIESPANEL.Visible || !frmStudioMain.PRINTJOBPROPERTIESPANEL.AutoHide)
                {
                    supportTop += frmStudioMain.PRINTJOBPROPERTIESPANEL.MinimumSize.Height;
                }
                else
                {

                }

                modelTop = supportTop;

                if (frmStudioMain.MODELPROPERTIESPANEL.Visible || !frmStudioMain.MODELPROPERTIESPANEL.AutoHide)
                {
                    supportTop += frmStudioMain.MODELPROPERTIESPANEL.MinimumSize.Height;
                }

                frmStudioMain.PRINTJOBPROPERTIESPANEL.Location = new Point(frmStudioMain.NAVIGATIONPANEL.Left, printjobTop);
                frmStudioMain.MODELPROPERTIESPANEL.Location = new Point(frmStudioMain.NAVIGATIONPANEL.Left, modelTop);
                frmStudioMain.SUPPORTPROPERTIESPANEL.Location = new Point(frmStudioMain.NAVIGATIONPANEL.Left, supportTop);
                frmStudioMain.SUPPORTPROPERTIESPANEL.Height = frmStudioMain.SUPPORTPROPERTIESPANEL.MinimumSize.Height;

                //expand docking to bottom
                if (frmStudioMain.SUPPORTPROPERTIESPANEL.Visible || !frmStudioMain.SUPPORTPROPERTIESPANEL.AutoHide){
                    if (frmStudioMain.SUPPORTPROPERTIESPANEL.Top < this.DockingHeight - this.DockingTop)
                    {
                        frmStudioMain.SUPPORTPROPERTIESPANEL.Height = this.DockingHeight - frmStudioMain.SUPPORTPROPERTIESPANEL.Top + this.DockingTop;
                    }
                }
                else if (frmStudioMain.MODELPROPERTIESPANEL.Visible || !frmStudioMain.MODELPROPERTIESPANEL.AutoHide)
                {
                    if (frmStudioMain.MODELPROPERTIESPANEL.Top < this.DockingHeight - this.DockingTop)
                    {
                        frmStudioMain.MODELPROPERTIESPANEL.Height = this.DockingHeight - frmStudioMain.MODELPROPERTIESPANEL.Top + this.DockingTop;
                    }
                }
                else if (frmStudioMain.PRINTJOBPROPERTIESPANEL.Visible || !frmStudioMain.PRINTJOBPROPERTIESPANEL.AutoHide)
                {
                    if (frmStudioMain.PRINTJOBPROPERTIESPANEL.Top < this.DockingHeight - this.DockingTop)
                    {
                        frmStudioMain.PRINTJOBPROPERTIESPANEL.Height = this.DockingHeight - frmStudioMain.PRINTJOBPROPERTIESPANEL.Top + this.DockingTop;
                    }
                }
                else if (frmStudioMain.NAVIGATIONPANEL.Visible || !frmStudioMain.NAVIGATIONPANEL.AutoHide)
                {
                    if (frmStudioMain.NAVIGATIONPANEL.Top < this.DockingHeight - this.DockingTop)
                    {
                        frmStudioMain.NAVIGATIONPANEL.Height = this.DockingHeight - frmStudioMain.NAVIGATIONPANEL.Top + this.DockingTop;
                    }
                }

                //disable panel if selected item is not the type selected
                if (ObjectView.SelectedModel is SupportCone && frmStudioMain.SUPPORTPROPERTIESPANEL.Visible)
                {
                    frmStudioMain.MODELPROPERTIESPANEL.DisablePanel();
                    frmStudioMain.SUPPORTPROPERTIESPANEL.EnablePanel();
                }
                else if (ObjectView.SelectedModel is STLModel3D && frmStudioMain.MODELPROPERTIESPANEL.Visible)
                {
                    frmStudioMain.MODELPROPERTIESPANEL.EnablePanel();
                    frmStudioMain.SUPPORTPROPERTIESPANEL.DisablePanel();
                }

            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
            }

        }

        internal void EnablePanel()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate { this.Enabled = true; }));
            }
            else
            {
                this.Enabled = true;
            }
        }

        internal void DisablePanel()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate { this.Enabled = false; }));
            }
            else
            {
                this.Enabled = false;
            }
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Hide();

            if (this._mainForm != null)
            {
                this._mainForm.Focus();
            }
            this.ToolstripItem.ResetHighlight();
        }

        protected override bool ShowWithoutActivation
        {
            get
            {
                return true;
            }
        }

        private void plTitle_MouseMove(object sender, MouseEventArgs e)
        {
            this.SetFocusToDockPanel();
        }

        private void lblTitle_MouseMove(object sender, MouseEventArgs e)
        {
            this.SetFocusToDockPanel();
        }

        public void SetFocusToDockPanel()
        {
            if (!this.Focused) { }
            this.Focus();

            if (this._dragging)
            {
         //       this.Location = new Point(Cursor.Position.X + this._draggingOffset.X, Cursor.Position.Y + this._draggingOffset.Y);
            }

        }

        private void plContent_MouseMove(object sender, MouseEventArgs e)
        {
            this.SetFocusToDockPanel();
        }

        private void plTitle_MouseDown(object sender, MouseEventArgs e)
        {
            this._dragging = true;
            this.Cursor = Cursors.Hand;
            this._draggingOffset = new Point(this.lblTitle.Location.X - e.Location.X, this.lblTitle.Location.Y - e.Location.Y);
        }

        private void plTitle_MouseUp(object sender, MouseEventArgs e)
        {
            this._dragging = false;
            this.Cursor = Cursors.Default;
        }

        private void lblTitle_MouseDown(object sender, MouseEventArgs e)
        {
            this._dragging = true;
            this.Cursor = Cursors.Hand;
            this._draggingOffset = new Point(this.lblTitle.Location.X - e.Location.X, this.lblTitle.Location.Y - e.Location.Y);
        }

        private void lblTitle_MouseUp(object sender, MouseEventArgs e)
        {
            this._dragging = false;
            this.Cursor = Cursors.Default;
        }

        private void picAutoHide_Click(object sender, EventArgs e)
        {
            this.AutoHide = !this.AutoHide;

            UserProfileManager.SaveDocking(this);

            if (this.AutoHide)
            {
                this.Visible = false;
                
                if (frmStudioMain.SUPPORTPROPERTIESPANEL.Visible)
                {
                    frmStudioMain.SUPPORTPROPERTIESPANEL.ShowPanel((Form)this.Parent);
                }
                else if (frmStudioMain.MODELPROPERTIESPANEL.Visible)
                {
                    frmStudioMain.MODELPROPERTIESPANEL.ShowPanel((Form)this.Parent);
                }
                else if (frmStudioMain.PRINTJOBPROPERTIESPANEL.Visible)
                {
                    frmStudioMain.PRINTJOBPROPERTIESPANEL.ShowPanel((Form)this.Parent);
                }
            }
        }

        private void DockPanelBase_KeyDown(object sender, KeyEventArgs e)
        {
            //if (this.DockKeyDown != null) { this.DockKeyDown(sender, e); }
        }

        private const int cGrip = 25;      // Grip size
        private const int cCaption = 32;   // Caption bar height;

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    Rectangle rc = new Rectangle(this.ClientSize.Width - cGrip, this.ClientSize.Height - cGrip, cGrip, cGrip);
        //    ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, rc);
        //    rc = new Rectangle(0, 0, this.ClientSize.Width, cCaption);
        //    e.Graphics.FillRectangle(Brushes.DarkBlue, rc);
        //}

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {  // Trap WM_NCHITTEST
                Point pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);
                pos = this.PointToClient(pos);
                if (pos.Y < cCaption)
                {
                    m.Result = (IntPtr)2;  // HTCAPTION
                    return;
                }
                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip)
                {
                    m.Result = (IntPtr)17; // HTBOTTOMRIGHT
                    return;
                }
            }
            base.WndProc(ref m);
        }

        private bool _mouseDown = false;
        private int _currentDockPositionRight = 0;
        private int _currentEndDragPositionX = 0;

        private void plGripSize_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseDown = true;
            _currentEndDragPositionX = Control.MousePosition.X;
            _currentDockPositionRight = this.Location.X + this.Width;
        }

        private void plGripSize_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown)
            {
                if (_currentEndDragPositionX != Control.MousePosition.X)
                {
                    this.Size = new Size((_currentDockPositionRight - _currentEndDragPositionX), this.Size.Height);
                    this.Location = new Point(_currentEndDragPositionX, this.Location.Y);

                    _currentEndDragPositionX = Control.MousePosition.X;

                    OnDockPanelResized?.Invoke(this.Location.X, this.Width);
                }
                
            }
        }

        private void plGripSize_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
        }
    }

}


