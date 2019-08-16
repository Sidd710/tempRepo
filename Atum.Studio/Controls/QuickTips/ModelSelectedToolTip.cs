using Atum.Studio.Controls.OpenGL;
using System;
using System.Windows.Forms;

namespace Atum.Studio.Controls.QuickTips
{
    class ModelSelectedToolTip: Panel
    {
        public event EventHandler<EventArgs> btnMoveClicked;
        public event EventHandler<EventArgs> btnRotateClicked;

        private SceneGLControl _parentControl;
        
        private Timer _showDelay = new Timer();
        
        private int _previousMousePositionX;
        private int _previousMousePositionY;

        private Button btnMove { get; set; }
        private Button btnRotate { get; set; }

        internal ModelSelectedToolTip(SceneGLControl parentControl)
        {
            this.Visible = false;
            
            this._parentControl = parentControl;
            this._showDelay.Interval = 500;
            this._showDelay.Tick += _showDelay_Tick;
            this._showDelay.Start();
            
            this.Size = new System.Drawing.Size(40, 24);
            this.Margin = new Padding();
            this.Padding = new Padding();

            this.btnMove = new Button();
            this.btnMove.Size = new System.Drawing.Size(24, 24);
            this.btnMove.BackgroundImage = Properties.Resources.Add;
            this.btnMove.BackgroundImageLayout = ImageLayout.Zoom;
            this.btnMove.Text = string.Empty;
            this.btnMove.Click += btnMove_Click;
            this.Controls.Add(this.btnMove);

            this.btnRotate = new Button();
            this.btnRotate.Left = this.btnMove.Width;
            this.btnRotate.Top = 0;
            this.btnRotate.Size = new System.Drawing.Size(24, 24);
            this.btnRotate.BackgroundImage = Properties.Resources.orbit_24x24;
            this.btnRotate.BackgroundImageLayout = ImageLayout.Zoom;
            this.btnRotate.Text = string.Empty;
            this.btnRotate.Click += btnRotate_Click;
            this.Controls.Add(this.btnRotate);

        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            this.btnMoveClicked?.Invoke(sender, e);
        }

        private void btnRotate_Click(object sender, EventArgs e)
        {
            this.btnRotateClicked?.Invoke(sender, e);
        }

        private void _showDelay_Tick(object sender, System.EventArgs e)
        {
            if (_parentControl != null)
            {
                if (!Visible)
                {
                    if (_parentControl.CurrentMousePositionX == _previousMousePositionX && _parentControl.CurrentMousePositionY == _previousMousePositionY && !_parentControl.LeftMouseDown && !_parentControl.RightMouseDown)
                    {
                        this.Left = _previousMousePositionX + 1;
                        this.Top = _previousMousePositionY - 10;
                        this.Show();
                    }
                }
            }

            this._previousMousePositionX = _parentControl.CurrentMousePositionX;
            this._previousMousePositionY = _parentControl.CurrentMousePositionY;
        }
    }


}
