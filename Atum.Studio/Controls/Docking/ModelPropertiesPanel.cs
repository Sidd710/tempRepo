using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.Managers;
using OpenTK;
using Atum.Studio.Core.ModelView;

namespace Atum.Studio.Controls.Docking
{
    public partial class ModelPropertiesPanel : DockPanelBase
    {
        internal event Action<object> ValueChanged;
        internal event EventHandler btnRemoveModel_Clicked;
        internal event Action<STLModel3D> btnMirrorModel_Clicked;
        internal event Action<STLModel3D> btnRepairFaces_Clicked;
        internal event EventHandler btnAddManualSupport_Clicked;
        internal event EventHandler btnAddManualGridSupport_Clicked;

        private STLModel3D _datasource;


        public ModelPropertiesPanel()
        {
            InitializeComponent();

            this.ToolstripIconMouseOver = BrandingManager.DockPanelModelPropertiesMouseOver;
           
            this.pgModel.MouseMove += pgModel_MouseMove;
            this.pgModel.SetParent(this);

            foreach (ToolStripItem button in this.toolStrip1.Items)
            {
                button.MouseMove += button_MouseMove;
            }
        }

        void button_MouseMove(object sender, MouseEventArgs e)
        {
            base.SetFocusToDockPanel();
        }


        void pgModel_MouseMove(object sender, MouseEventArgs e)
        {
            base.SetFocusToDockPanel();
        }

        public STLModel3D DataSource
        {
            get
            {
                return this._datasource;
            }
            set
            {
                this._datasource = value;

                if (this._datasource != null)
                {
                    //disable some categories
                    //this.pgModel.HiddenProperties = new string[] {"AxisLocked", "ScaleFactorX", "ScaleFactorY", "ScaleFactorZ" };
                    if (this.pgModel.InvokeRequired)
                    {
                        this.pgModel.Invoke(new MethodInvoker(delegate { this.pgModel.Enabled = true; })); 
                    }
                        else{
                            this.pgModel.Enabled = true;
                        }

                    this.pgModel.Enabled = true;
                    this.pgModel.HiddenProperties = new string[] { "ScaleFactorY", "ScaleFactorZ", "MoveTranslation", "PreviousScaleFactorX", "PreviousScaleFactorY" };
                    this.pgModel.RenamedProperties = new Dictionary<string, string>();
                    this.pgModel.RenamedProperties.Add("ScaleFactorX", "Factor");
                    this.pgModel.SelectedObject = this._datasource;
                }
                else
                {
                    //this.pgModel.SelectedObject = new STLModel3D();
                    this.pgModel.Enabled = false;
                }
            }
        }

        private void pgModel_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            object obj = new { PanelObject = this, EventArgs = e };
            this.ValueChanged?.Invoke(obj);

            if (e.ChangedItem.Label == "All Axis Locked")
            {
                if ((bool)e.ChangedItem.Value)
                {
                    this.pgModel.RenamedProperties.Clear();
                    this.pgModel.RenamedProperties.Add("ScaleFactorX", "Factor");
                    this.pgModel.HiddenProperties = new string[] { "ScaleFactorY", "ScaleFactorZ" };
                }
                else
                {
                    this.pgModel.HiddenProperties = new string[] { };
                    this.pgModel.RenamedProperties.Clear();
                    this.pgModel.RenamedProperties.Add("ScaleFactorX", "Factor-X");
                }

                this.pgModel.RefreshProperties();
                this.pgModel.Refresh();
            }
            else if (e.ChangedItem.Label == "Translation-X")
            {
            }
            else if (e.ChangedItem.Label == "Translation-Y")
            {
            }
            else if (e.ChangedItem.Label == "Translation-Z")
            {
            }
            else if (e.ChangedItem.Label == "Factor")
            {
            }
            else if (e.ChangedItem.Label == "Factor-X")
            {
            }
            else if (e.ChangedItem.Label == "Factor-Y")
            {
            }
            else if (e.ChangedItem.Label == "Factor-Z")
            {
            }
            else if (e.ChangedItem.Label == "Support Basement")
            {
                //var movement = UserProfileManager.UserProfile.SupportEngine_Basement_Thickness;
                //if ((bool)e.ChangedItem.Value != (bool)e.OldValue)
                //{
                //    if ((bool)e.ChangedItem.Value == false) movement = -movement;
                //    var movementVector = new Vector3(0, 0, movement);

                //    foreach (var object3d in ObjectView.Objects3D)
                //    {
                //        if (object3d is STLModel3D)
                //        {
                //            var stlModel = object3d as STLModel3D;
                //            if (stlModel.Selected)
                //            {
                //                if (stlModel.BottomPoint >= 0)
                //                {
                //                    if (movement == -1f && !(bool)e.OldValue)
                //                    {
                //                        break;
                //                    }

                //                    stlModel.MoveModelWithTranslationZ(movementVector);
                //                    stlModel.UpdateBoundries();
                //                    stlModel.UpdateSelectionboxText();
                //                    stlModel.UpdateBinding();
                //                }
                //            }
                //        }
                //    }

                //    this.ValueChanged?.Invoke(obj);
                //}
                
            }
        }

        private void btnSupportRemove_Click(object sender, EventArgs e)
        {
            this.btnRemoveModel_Clicked?.Invoke(sender, e); 
            this.Hide();
        }

        private void btnRepairFaces_Click(object sender, EventArgs e)
        {
            this.btnRepairFaces_Clicked?.Invoke(this._datasource); 
        }

        private void btnAddManualSupport_Click(object sender, EventArgs e)
        {
            this.btnAddManualSupport_Clicked?.Invoke(sender, e); 
        }

        private void toolStrip1_MouseMove(object sender, MouseEventArgs e)
        {
            base.SetFocusToDockPanel();
        }

        private void btnAddManualSupport2Points_Click(object sender, EventArgs e)
        {
            //this.btnAddManualSupport2Points_Clicked?.Invoke(this._datasource); 
        }

        private void btnAddGridSupport_Click(object sender, EventArgs e)
        {
            this.btnAddManualGridSupport_Clicked?.Invoke(sender, e); 
        }

        private void btnMirrorModel_Click(object sender, EventArgs e)
        {
            this.btnMirrorModel_Clicked?.Invoke(this._datasource);
        }

    }
}
