using Atum.Studio.Core.Engines.MagsAI;
using Atum.Studio.Core.Events;
using Atum.Studio.Core.Helpers;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Shapes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static Atum.Studio.Core.Enums;

namespace Atum.Studio.Controls.MagsAI
{
    public partial class frmMagsAIWizard : Form
    {

        public frmMagsAIWizard()
        {
            InitializeComponent();

            this.magsAIWelcomeTab1.ModelLoaded += MagsAIWelcomeTab1_ModelLoaded;
            MagsAIEngine.ModelProgressChanged += MagsAIEngine_ModelProgressChanged;

            this.tbWizard.SelectedIndexChanged += TbWizard_SelectedIndexChanged;
            this.Icon = BrandingManager.MainForm_Icon;

        }

        private void MagsAIEngine_ModelProgressChanged(object sender, Core.Events.MagsAIProgressEventArgs e)
        {
            magsAIOrientationTabPanel1.GLControl.Render();

            this.magsAIPreview1.Progress = e;

            if (e.Percentage == 100)
            {
                foreach (var model in ObjectView.Objects3D)
                {
                    if (model is STLModel3D && !(model is GroundPane))
                    {
                        var stlModel = model as STLModel3D;
                        stlModel.SupportBasement = !stlModel.SupportBasement;
                        stlModel.SupportBasement = this.magsAIMaterialTabPanel1.UseSupportBasement;
                    }
                }
            }
        }

        private void MagsAIWelcomeTab1_ModelLoaded(object sender, EventArgs e)
        {
            if (this.tbWizard.InvokeRequired)
            {
                this.tbWizard.Invoke(new MethodInvoker(delegate
                {
                    MagsAIWelcomeTab1_ModelLoaded(sender, e);
                }));
            }
            else
            {
                this.SelectNextTabPanel();
            }
        }

        private void TbWizard_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbWizard.SelectedTab == this.tbMagsAIOrientation)
            {
                var stlModel = ObjectView.Objects3D[1] as STLModel3D;
                stlModel.Triangles.UpdateConnections();
                this.magsAIOrientationTabPanel1.GLControl.Render();
            }
            else if (tbWizard.SelectedTab == this.tbPreview)
            {
                this.magsAIPreview1.GLControl.Render();

                var stlModel = ObjectView.Objects3D[1] as STLModel3D;
                Task.Run(() => MagsAIEngine.Calculate(stlModel, this.magsAIMaterialTabPanel1.SelectedMaterial, this.magsAIMaterialTabPanel1.SelectedPrinter));
            }
        }


        private void SelectPreviousTabPanel()
        {
            this.btnDebug.Visible = false;
            if (this.tbWizard.InvokeRequired)
            {
                this.tbWizard.Invoke(new MethodInvoker(delegate
                {
                    this.SelectPreviousTabPanel();
                }));
            }
            else
            {
                var selectedIndex = this.tbWizard.SelectedIndex;
                if (selectedIndex > 0)
                {
                    this.tbWizard.SelectedIndex -= 1;
                    this.btnNext.Text = "Next";
                }
            }
        }

        private void SelectNextTabPanel()
        {
            this.btnDebug.Visible = false;
            if (this.tbWizard.InvokeRequired)
            {
                this.tbWizard.Invoke(new MethodInvoker(delegate
                {
                    this.SelectNextTabPanel();
                }));
            }
            else
            {
                var selectedIndex = this.tbWizard.SelectedIndex;
                if (selectedIndex < this.tbWizard.TabCount - 1)
                {
                    this.tbWizard.SelectedIndex += 1;
                }
                else
                {
                    this.btnNext.Text = "Finished";
                    this.btnDebug.Visible = true;
                }
            }
        }

        private void tbWizard_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == this.tbPreview)
            {

                this.magsAIPreview1.GLControl = this.magsAIOrientationTabPanel1.GLControl;
                ToolbarActionsManager.Update(MainFormToolStripActionType.btnSelectPressed, this.magsAIPreview1.GLControl, null);

                this.magsAIPreview1.GLControl.Render();

                foreach (var model in ObjectView.Objects3D)
                {
                    if (model is STLModel3D && !(model is GroundPane))
                    {
                        var stlModel = model as STLModel3D;

                        if (this.magsAIOrientationTabPanel1.OrientationType == MagsAIOrientationTabPanel.TypeOfOrientation.SelectedTriangles)
                        {
                            //calc new model normal
                            var autorotationNormal = stlModel.Triangles.CalcSelectedOrientationTrianglesNormal();

                            float zAngle; float yAngle;
                            VectorHelper.CalcRotationAnglesYZFromVector(autorotationNormal, false, out zAngle, out yAngle);

                            //rotate model 
                            stlModel.Rotate(0, 0, stlModel.RotationAngleZ - zAngle, RotationEventArgs.TypeAxis.Z);
                            stlModel.UpdateDefaultCenter();
                            stlModel.Rotate(0, stlModel.RotationAngleY - yAngle, 0, RotationEventArgs.TypeAxis.Y);
                            stlModel.UpdateDefaultCenter();
                            stlModel.UpdateBoundries();
                        }
                        else
                        {
                            stlModel.Triangles.ClearSelectedOrientationTriangles();
                        }

                        if (this.magsAIMaterialTabPanel1.LiftModelOnSupport)
                        {
                            stlModel.MoveTranslationZ = 5f;
                            stlModel.LiftModelOnSupport();
                        }
                    }
                }
            }
            else if (e.TabPage == this.tbMagsAIOrientation)
            {
                //ToolStripActionsManager.Update(MainFormToolStripActionType.btnLayFlatPressed, frmStudioMain.SceneControl);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (this.tbWizard.SelectedTab == this.tbPreview)
            {
                this.magsAIPreview1.RemoveGLControl();
                this.DialogResult = DialogResult.OK;

                this.Close();
            }
            else
            {
                this.SelectNextTabPanel();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.SelectPreviousTabPanel();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            //this.tbWizard.SelectedTab = this.tbPreview;

            if (this.magsAIOrientationTabPanel1.GLControl != null)
            {
                this.magsAIOrientationTabPanel1.RemoveGLControl();
            }

            if (this.magsAIPreview1.GLControl != null)
            {
                this.magsAIPreview1.RemoveGLControl();
            }

            //this.Close();
            this.DialogResult = DialogResult.OK;
        }

        private void frmMagsAIWizard_Shown(object sender, EventArgs e)
        {
            this.magsAIOrientationTabPanel1.GLControl = frmStudioMain.SceneControl;
            this.splitContainer1.SplitterDistance = this.splitContainer1.Width;
        }

        private void btnDebug_Click(object sender, EventArgs e)
        {
            if (this.magsAIPreview1.GLControl != null)
            {
                this.splitContainer1.SplitterDistance = (int)((this.splitContainer1.Width - (400 * DisplayManager.GetResolutionScaleFactor())));
                this.txtSavePath.Text = DAL.ApplicationSettings.Settings.RoamingLoggingPath;
                this.btnDebug.Visible = false;

                this.magsAIPreview1.GLControl.Render();
            }
        }

        
        private void btnCreateScreenshot_Click(object sender, EventArgs e)
        {
            var magsAICommentControl = new MAGSAIDebugCommentControl(this.magsAIPreview1);
            magsAICommentControl.Width = (int)((this.flowLayoutPanel1.Width - 10));
            this.flowLayoutPanel1.Controls.Add(magsAICommentControl);
        }

        //private void btnSaveMagsAIDebugInformation_Click(object sender, EventArgs e)
        //{
        //    var loggingPath = Path.Combine(DAL.ApplicationSettings.Settings.RoamingLoggingPath);
        //    if (!Directory.Exists(loggingPath))
        //    {
        //        Directory.CreateDirectory(loggingPath);
        //    }

        //    var magsAIComments = new List<MAGSAIDebugComment>();
        //    foreach (MAGSAIDebugCommentControl magsAICommentControl in this.flowLayoutPanel1.Controls)
        //    {
        //        if (magsAICommentControl != null)
        //        {
        //            var magsAICommentItem = magsAICommentControl.DataSource;
        //            if (!string.IsNullOrEmpty(magsAICommentItem.Comment) || magsAICommentItem.ScreenshotAsByteArray != null)
        //            {
        //                magsAIComments.Add(magsAICommentItem);
        //            }
        //        }
        //    }

        //    Core.Engines.ExportEngine.ExportCurrentWorkspace(false, magsAIComments);
        //}

        private void txtSavePath_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var savePathLink = sender as LinkLabel;
            Process.Start(@"" + savePathLink.Text + @"");
        }
    }
}
