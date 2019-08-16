using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atum.Studio.Core.Models;
using Atum.Studio.Resources;

namespace Atum.Studio.Controls.SceneControlActionPanel
{
    public partial class SceneControlModelSupportPropertiesHandle : UserControl
    {
        private bool _stateOpen;
        private SupportCone _supportCone;

        internal SupportCone DataSource
        {
            get
            {
                return this._supportCone;
            }
            set
            {
                this._supportCone = value;
            }
        }

        public SceneControlModelSupportPropertiesHandle()
        {
            InitializeComponent();
        }

        private void SceneControlModelSupportPropertiesHandle_Click(object sender, EventArgs e)
        {
            this._stateOpen = !this._stateOpen;

            if (this._stateOpen)
            {
                if (frmStudioMain.SceneControl.InvokeRequired)
                {
                    frmStudioMain.SceneControl.Invoke(new MethodInvoker
                    (
                        delegate
                        {
                            AddMenuToSceneControl();
                        }
                    ));
                }
                else
                {
                    AddMenuToSceneControl();
                }

                this.BackgroundImage = Properties.Resources.toolbar_combobo_left;
            }
            else
            {
                CloseState();
            }
        }


        private void AddMenuToSceneControl()
        {
            var supportPropertiesControl = new SceneControlModelSupportProperties();
            supportPropertiesControl.DataSource = this._supportCone;
            frmStudioMain.SceneControl.Controls.Add(supportPropertiesControl);
            supportPropertiesControl.Left = this.Left + this.Width;

			if (supportPropertiesControl.Left >= frmStudioMain.SceneControl.Width - supportPropertiesControl.Width - 28 - 16 - 60)
            {
                supportPropertiesControl.Left = this.Left - supportPropertiesControl.Width;
            }
			
            if (this.Top >= frmStudioMain.SceneControl.Height - supportPropertiesControl.Height - 28 - 16 - 60)
            {
                supportPropertiesControl.Top = this.Top - supportPropertiesControl.Height + this.Height;
            }
            else
            {
                supportPropertiesControl.Top = this.Top;
                supportPropertiesControl.BringToFront();
            }
            supportPropertiesControl.Visible = true;
        }

        internal void CloseState()
        {
            this.BackgroundImage = Properties.Resources.toolbar_combobo_right;

            RemoveMenuFromSceneControl();

            this._stateOpen = false;
        }

        internal void RemoveMenuFromSceneControl()
        {
            if (frmStudioMain.SceneControl.InvokeRequired)
            {
                frmStudioMain.SceneControl.Invoke(
                new MethodInvoker(delegate
                {
                    RemoveMenuFromSceneControl();
                }));
            }
            else
            {
                var controls = frmStudioMain.SceneControl.Controls.OfType<SceneControlModelSupportProperties>().ToList();
                foreach (var handle in controls)
                {
                    frmStudioMain.SceneControl.Controls.Remove(handle);
                }
            }
            frmStudioMain.SceneControl.Render();
        }
    }
}
