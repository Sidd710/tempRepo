using Atum.DAL.ApplicationSettings;
using Atum.DAL.Hardware;
using Atum.DAL.Managers;
using Atum.DAL.Print;
using Atum.Studio.Controls;
using Atum.Studio.Controls.NewGui;
using Atum.Studio.Controls.NewGui.ExportControl;
using Atum.Studio.Controls.NewGui.MainMenu;
using Atum.Studio.Controls.NewGui.Preference;
using Atum.Studio.Controls.NewGui.SupportContact;
using Atum.Studio.Controls.OpenGL;
using Atum.Studio.Controls.SceneControlActionPanel;
using Atum.Studio.Controls.SceneControlToolTips;
using Atum.Studio.Core;
using Atum.Studio.Core.Engines;
using Atum.Studio.Core.Engines.PackingEngine;
using Atum.Studio.Core.Events;
using Atum.Studio.Core.Helpers;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.Managers.UndoRedo;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.Models.Defaults;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Plugins;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Structs;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Atum.Studio.Core.Enums;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio
{
    public partial class frmStudioMain : Form
    {
        string _initialModelFileName = "cube.stl";

        private bool _disableOpenTK = false;

        internal static SceneGLControl SceneControl = null;
        private static ExportUserControl ExportControl { get; set; }

        public frmStudioMain()
        {
            InitializeComponent();

            //start default print manager
            MaterialManager.Start(true);
            MainFormManager.DragDropSTLFile += DragDrop_STLFile;
            MainFormManager.DragDropAPFFile += DragDrop_APFFile;
            MainFormManager.MainMenuOpened += frmMainMenuFlyout_OnSelectMenuItem;

            AutoSaveManager.Start();
            CachedProfileManager.Start();

            ExportEngine.ExportToAPF_Progress += ExportEngine_ExportToAPFProgress;
            ExportEngine.ExportToAPF_Completed += ExportEngine_ExportToAPFCompleted;

            ConnectivityManager.Start();
            ConnectivityManager.InternetConnected += ConnectivityManager_InternetConnected;

            PluginManager.Start();


            //performance settings manager
            PerformanceSettingsManager.Start();

            //license request
            LicenseClientManager.Start();

            RenderEngine.RenderToPrintjobCompleted += GeneratePrintJobAsync_Completed;
            RenderEngine.RenderToPrintjobCanceled += renderEngine_RenderCanceled;

            SceneControlToolbarManager.OnEscKeyPressed += SceneControlToolbarManager_OnEscKeyPressed;

            FontManager.LoadDefaultFonts();

            ToolbarActionsManager.Start();

            SceneActionControlManager.Initialize(this.plWorkSpace);
            SceneControlToolbarManager.Initialize(this.plWorkSpace);

            SupportEngine.GridSupportAdded += SupportEngine_GridSupportAdded;

            SceneView.Init();

            RegistryManager.GetRegistryProfileSettings();
        }


        #region MainForm Events

        private void frmStudioMain_Load(object sender, EventArgs e)
        {
            Application.Idle += Application_Idle;

            SetButtonLocations();

            try
            {
                DisplayManager.Start();
                AddGLControl();
                //XYZAxis.CreateXYZAxis();

                //                QuickTipManager.Initialize(SceneControl);


                this.Icon = BrandingManager.MainForm_Icon;
                this.Text = string.Format(BrandingManager.MainForm_Title, string.Empty, string.Empty, string.Empty).Replace(".", string.Empty);
                this.ShowIcon = true;

                MainFormManager.HotKeyDPressed += SceneControl_HotKeyDPressed;
                MainFormManager.HotKeyFPressed += SceneControl_HotKeyFPressed;
                MainFormManager.HotKeyGPressed += SceneControl_HotKeyGPressed;
                MainFormManager.HotKeyMPressed += SceneControl_HotKeyMPressed;
                MainFormManager.HotKeyOPressed += SceneControl_HotKeyOPressed;
                MainFormManager.HotKeyPPressed += SceneControl_HotKeyPPressed;
                MainFormManager.HotKeyQPressed += SceneControl_HotKeyQPressed;
                MainFormManager.HotKeyRPressed += SceneControl_HotKeyRPressed;
                MainFormManager.HotKeySPressed += SceneControl_HotKeySPressed;
                MainFormManager.HotKeyXPressed += SceneControl_HotKeyXPressed;
                MainFormManager.HotKeyZPressed += SceneControl_HotKeyZPressed;
                MainFormManager.HotKeyDelPressed += SceneControl_HotKeyDeletePressed;
                MainFormManager.HotKeyCtlCPressed += SceneControl_HotKeyCtlCPressed;
                MainFormManager.HotKeyCtlPPressed += SceneControl_HotKeyCtlPPressed;
                MainFormManager.HotKeyCtlZPressed += SceneControl_HotKeyCtlZPressed;

                //events
                ObjectView.GroundSupportDistanceProcessing += new EventHandler(ObjectView_GroundSupportDistanceProcessing);
                ObjectView.SupportXYDistanceProcessing += new EventHandler(ObjectView_SupportXYDistanceProcessing);
                ObjectView.SupportSurfaceProcessing += new EventHandler(ObjectView_SupportSurfaceProcessing);
                //ObjectView.InternalSupportProcessing += new EventHandler(ObjectView_InternalSupportProcessing);
                ObjectView.SelectedModelChanged += ObjectView_SelectedModelChanged;
                //ObjectView.CrossSupportProcessing += new EventHandler(ObjectView_CrossSupportProcessing);
                //ObjectView.ZSupportProcessing += new EventHandler(ObjectView_ZSupportProcessing);
                ObjectView.RotationProcessing += new EventHandler<RotationEventArgs>(ObjectView_RotationProcessing);
                ObjectView.ScaleProcessing += new EventHandler<ScaleEventArgs>(ObjectView_ScaleProcessing);
                ObjectView.ModelAdded += ObjectView_ModelAdded;
                ObjectView.ModelRemoved += ObjectView_ModelRemoved;

                SceneActionControlManager.MoveTranslationChanged += SceneActionControlManager_MoveTranslationChanged;
                SceneActionControlManager.RotationChanged += SceneActionControlManager_RotationChanged;
                SceneActionControlManager.ScalingChanged += SceneActionControlManager_ScaleChanged;
                SceneActionControlManager.CloneModels += SceneActionControlManager_CloneModels;

                if (!this._disableOpenTK) ObjectView.BindingSupported = (SceneControl.Context as IGraphicsContextInternal).GetAddress("glGenBuffers") == IntPtr.Zero ? false : true;

                if (File.Exists(_initialModelFileName))
                {
                    var selectedMaterial = PrintJobManager.SelectedMaterialSummary;
                    if (selectedMaterial.Material != null)
                    {
                        var modelColor = string.IsNullOrEmpty(selectedMaterial.Material.DisplayName) ? Color.CornflowerBlue : selectedMaterial.Material.ModelColor;
                        ObjectView.Create(new[] { _initialModelFileName }, modelColor, ObjectView.BindingSupported, this);
                    }

                    SceneControl.Render();
                }


                //var t = new OrientationGizmo(4, OrientationGizmo.SideSelectionType.None, OrientationGizmo.SideSelectionType.None, true);
                //t.BindModel();
                //t.UpdateBinding();
                //ObjectView.AddModel(t, true);

                //   this.btnViewSelect.PerformClick();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message.Replace("Atum.", string.Empty));
            }

            //check if there are plugins that need to initialize on startup
            foreach (var plugin in PluginManager.LoadedPlugins)
            {
                if (plugin.HasInitializeMethod)
                {
                    plugin.Initialize(this);
                }

                plugin.PluginLoaded += Plugin_PluginLoaded;
            }

            // Printer Wizard
            //PRINTJOBPROPERTIESPANEL.CheckInitialPrinter();
        }

        private void Application_Idle(Object sender, EventArgs e)
        {
            if (SceneControl != null)
            {
                while (SceneControl.IsIdle)
                {
                    SceneControl.Render();
                }
            }
        }

        private void ObjectView_ModelRemoved(object sender, EventArgs e)
        {
            if (SceneView.CurrentViewMode == SceneView.ViewMode.Duplicate)
            {
                //find and update all clone references
                var duplicateControl = SceneControl.Controls.OfType<SceneControlModelDuplicate>().FirstOrDefault();
                if (duplicateControl != null)
                {
                    duplicateControl.RefreshCurrentDuplicates();
                }
            }

            SceneControlToolbarManager.PrintJobPropertiesToolbar.ResetPrintJobModelDimensions();
        }

        private void ObjectView_SelectedModelChanged(object sender, EventArgs e)
        {
            var selectedModel = sender as STLModel3D;

            foreach (var availableObject in ObjectView.Objects3D)
            {
                if (availableObject is STLModel3D)
                {
                    var availableModel = availableObject as STLModel3D;
                    if (availableModel != selectedModel)
                    {
                        availableModel.Selected = false;
                    }
                }
            }

            if (SceneView.CurrentViewMode == SceneView.ViewMode.MoveTranslation)
            {
                SceneView.MoveTranslation3DGizmo.UpdateControl(SceneViewSelectedMoveTranslationAxisType.NoAxisSelected, true);
            }

            SceneControlToolbarManager.UpdateModelDimensions(selectedModel.Width, selectedModel.Depth, selectedModel.Height);
        }

        private void SetButtonLocations()
        {
            var plSize = plTopMenu.Size;
            var btnPrepareX = (plSize.Width / 2) - btnPrepare.Width;
            var btnPrepareY = (plSize.Height / 2) - (btnPrepare.Height / 2) + 1;

            btnPrepare.Location = new Point(btnPrepareX, btnPrepareY);
            var btnExportX = (plSize.Width / 2);
            btnExport.Location = new Point(btnExportX, btnPrepareY);
            btnExport.BackColor = BrandingManager.Button_BackgroundColor_LightDark;
            btnPrepare.BackColor = BrandingManager.Button_HighlightColor;

            var btnIconsTop = (plSize.Height / 2) - (btnSettings.Height / 2) + 1;
            btnOpenFileMenu.Top = btnInfo.Top = btnSettings.Top = btnAddObject.Top = btnIconsTop;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            return MainFormManager.ProcessKeyPress(ref msg, keyData);
        }

        private void frmStudioMain_Shown(object sender, EventArgs e)
        {
            PrinterManager.Start();

            ProcessDroppedAPFFiles();
            ProcessDroppedSTLFiles();

            ObjectView.Init();

            SceneControlToolbarManager.SelectedPrinterChanged += SceneControlToolbarManager_SelectedPrinterChanged;
            SceneView.UpdateGroundPane(PrintJobManager.SelectedPrinter);

            SceneControlToolbarManager.Reinitialize(frmStudioMain.SceneControl);
            SetButtonLocations();

            ProgressBarManager.InitialiseMain();


        }

        private void SceneControlToolbarManager_SelectedPrinterChanged(object sender, AtumPrinter e)
        {
            SceneView.UpdateGroundPane((AtumPrinter)sender);
        }


        private void frmStudioMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ObjectView.Objects3D.Count > 1)
            {
                using (var messageBox = new frmMessageBox("Operator Station", string.Format("Save changes to project {0} before quiting?", PrintJobManager.PrintjobName), MessageBoxButtons.YesNoCancel, MessageBoxDefaultButton.Button2))
                {
                    var messageBoxDialogResult = messageBox.ShowDialog();
                    if (messageBoxDialogResult == DialogResult.Yes)
                    {
                        MaterialManager.Stop();

                        e.Cancel = true;
                        saveProjectOnExit(e);
                    }
                    else if (messageBoxDialogResult == DialogResult.No)
                        {
                            MaterialManager.Stop();
                        }
                    else if (messageBoxDialogResult == DialogResult.OK)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
            }

        }

        void DragDrop_APFFile()
        {
            if (MainFormManager.DroppedProjectFiles != null && MainFormManager.DroppedProjectFiles.Count > 0)
            {
                //process selected files
                var cleanProject = true;

                if (ObjectView.Objects3D.Count > 1)
                {
                    using (var frmMessageBox = new frmMessageBox("Clean current project?", "The current project will not be saved. " + Environment.NewLine + Environment.NewLine + "Are you sure you want to clean the current project?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button3))
                        if (frmMessageBox.ShowDialog() == DialogResult.No)
                        {
                            cleanProject = false;
                        }
                }

                if (cleanProject)
                {
                    for (var modelIndex = ObjectView.Objects3D.Count - 1; modelIndex > 0; modelIndex--)
                    {
                        if (!(ObjectView.Objects3D[modelIndex] is GroundPane))
                        {
                            ObjectView.Objects3D.RemoveAt(modelIndex);
                        }
                    }

                    this.Text = this.Text.Split('-')[0].Trim();
                    //PRINTJOBPROPERTIESPANEL.PrintJobName = "NO_NAME";
                }

                var projectFile = MainFormManager.DroppedProjectFiles[0];
                MainFormManager.DroppedProjectFiles.Remove(projectFile);
                ImportEngine.ImportWorkspaceFileAsync(this, projectFile);
            }
        }

        void DragDrop_STLFile()
        {
            var selectedMaterial = PrintJobManager.SelectedMaterialSummary.Material;

            if (MainFormManager.DroppedSTLFiles != null && MainFormManager.DroppedSTLFiles.Count > 0)
            {
                var droppedSTLFile = MainFormManager.DroppedSTLFiles[0];
                MainFormManager.DroppedSTLFiles.Remove(droppedSTLFile);
                ImportSTLFile(droppedSTLFile, selectedMaterial);
            }
        }

        #endregion


        void renderEngine_RenderCanceled(object sender, EventArgs e)
        {
            using (var frmMessageBox = new frmMessageBox("No model(s) found", "No model(s) found", MessageBoxButtons.OK, MessageBoxDefaultButton.Button2))
            {
                frmMessageBox.ShowDialog();
            }

            //enable all controls
            ProcessActionEnd();
        }

        #region StatusBar events

        void ConnectivityManager_InternetConnected(bool obj)
        {
            //    if (UserProfileManager.UserProfiles[0].Settings_Studio_AutoUpdateNotification)
            //    {
            SoftwareUpdateManager.NewUpdateAvailable += SoftwareUpdateManager_NewUpdateAvailable;
            SoftwareUpdateManager.Start();
            //    }
        }

        #endregion

        #region Menustrip Events

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            // var selectedMaterial = PRINTJOBPROPERTIESPANEL.SelectedMaterial;
            if (sender != null && !(sender is string))
            {

                using (var popup = new OpenFileDialog())
                {
                    popup.Filter = "(*.stl)|*.stl";
                    popup.Multiselect = false;
                    if (popup.ShowDialog() == DialogResult.OK)
                    {
                        filePath = popup.FileName;
                    }
                }
            }
            else if (sender != null && sender is string)
            {
                filePath = (string)sender;
            }


            if (!string.IsNullOrEmpty(filePath))
            {
                //   ImportSTLFile(filePath, selectedMaterial);
            }

        }

        private void clearProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cleanProject = true;

            if (ObjectView.Objects3D.Count > 1)
            {
                using (var frmMessageBox = new frmMessageBox("New project", "Any unsaved changes to the current project will be lost." + Environment.NewLine + Environment.NewLine + "Are you sure you want to reset the Build Platform?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button3))
                {
                    var dialogResult = frmMessageBox.ShowDialog();
                    if (dialogResult == DialogResult.No)
                    {
                        cleanProject = false;
                    }
                }
            }

            if (cleanProject)
            {
                for (var modelIndex = ObjectView.Objects3D.Count - 1; modelIndex > 0; modelIndex--)
                {
                    if (!(ObjectView.Objects3D[modelIndex] is GroundPane))
                    {
                        ObjectView.Objects3D.RemoveAt(modelIndex);
                    }
                }

                this.Text = this.Text.Split('-')[0].Trim();
                PrintJobManager.PrintjobName = "Untitled";
                PrintJobManager.ProjectSaveFilePath = string.Empty;
                SceneControlToolbarManager.PrintJobProperties.ResetPrintJobModelDimensions();
            }
        }

        private void saveProjectOnExit(FormClosingEventArgs e)
        {

            //no async!
            if (!string.IsNullOrEmpty(PrintJobManager.ProjectSaveFilePath))
            {
                BwExportToAPF_DoWork(null, null);
            }
            else
            {
                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.AddExtension = true;
                    saveDialog.DefaultExt = ".apf";
#if LOCTITE
                saveDialog.Filter = "Loctite Project File(*.apf)|*.apf";
#else
                    saveDialog.Filter = "Atum Project File(*.apf)|*.apf";
#endif
                    if (saveDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(saveDialog.FileName))
                    {
                        PrintJobManager.ProjectSaveFilePath = saveDialog.FileName;

                        BwExportToAPF_DoWork(null, null);
                    }
                }
            }

            e.Cancel = false;
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(PrintJobManager.ProjectSaveFilePath))
            {
                var bwExportToAPF = new BackgroundWorker();
                bwExportToAPF.DoWork += BwExportToAPF_DoWork;
                bwExportToAPF.RunWorkerAsync();
            }
            else
            {
                saveAsProjectToolStripMenuItem_Click(null, null);
            }
        }

        private void saveAsProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.AddExtension = true;
                saveDialog.FileName = PrintJobManager.PrintjobName + ".apf";
                saveDialog.DefaultExt = ".apf";
#if LOCTITE
                saveDialog.Filter = "Loctite Project File(*.apf)|*.apf";
#else
                saveDialog.Filter = "Atum Project File(*.apf)|*.apf";
#endif
                if (saveDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(saveDialog.FileName))
                {
                    PrintJobManager.ProjectSaveFilePath = saveDialog.FileName;
                    var projectName = (new FileInfo(saveDialog.FileName)).Name.Substring(0, (new FileInfo(saveDialog.FileName)).Name.LastIndexOf('.'));
                    if (projectName.Length > 16)
                    {
                        projectName = projectName.Substring(0, 16);
                    }

                    if (PrintJobManager.PrintjobName == "Untitled")
                    {
                        PrintJobManager.PrintjobName = projectName;
                    }

                    var bwExportToAPF = new BackgroundWorker();
                    bwExportToAPF.DoWork += BwExportToAPF_DoWork;
                    bwExportToAPF.RunWorkerCompleted += BwExportToAPF_RunWorkerCompleted;
                    bwExportToAPF.RunWorkerAsync();

                }
            }
            //export objects to xml

        }

        private void BwExportToAPF_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void BwExportToAPF_DoWork(object sender, DoWorkEventArgs e)
        {
            SceneControl.Render();
            ExportEngine.ExportCurrentWorkspace();
        }

        private void viewAnnotationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserProfileManager.UserProfiles[0].SelectionOptions_Enable_Annotations = (sender as ToolStripMenuItem).Checked;
        }


        private void importProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //process selected files
            var cleanProject = true;

            if (ObjectView.Objects3D.Count > 1)
            {
                var dialogResult = new frmMessageBox("Clean current project?", "The current project will not be saved. " + Environment.NewLine + Environment.NewLine + "Are you sure you want to clean the current project?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button3).ShowDialog();
                if (dialogResult == DialogResult.No)
                {
                    cleanProject = false;
                }
            }

            if (cleanProject)
            {
                ObjectView.Objects3D.Clear();

                this.Text = this.Text.Split('-')[0].Trim();
                //PRINTJOBPROPERTIESPANEL.PrintJobName = "NO_NAME";
            }

            ImportEngine.ImportWorkspaceFileAsync(this);
        }

        void SoftwareUpdateManager_NewUpdateAvailable(object sender, EventArgs e)
        {
            var updateAvailableMenuItem = new ToolStripMenuItem("New version available!");
            updateAvailableMenuItem.Click += updateAvailableMenuItem_Click;

            if (this.menuStrip1.InvokeRequired)
            {
                this.menuStrip1.Invoke(new MethodInvoker(delegate
                {
                    this.menuStrip1.Items.Add(updateAvailableMenuItem);
                }));
            }
            else
            {
                this.menuStrip1.Items.Add(updateAvailableMenuItem);
            }


        }

        void updateAvailableMenuItem_Click(object sender, EventArgs e)
        {
            SoftwareUpdateManager.ShowUpdateDialog();
        }

        void ExportEngine_ExportToAPFProgress(int currentValue, int maxValue)
        {

            var percentage = (int)(((float)currentValue / (float)maxValue) * 100);
            if (percentage >= 100)
            {
                percentage = 99;
            }

            ProgressBarManager.UpdateMainPercentage(percentage);
        }

        void ExportEngine_ExportToAPFCompleted()
        {
            ProgressBarManager.UpdateMainPercentage(100);
        }

        #endregion

        private void AddGLControl()
        {
            SceneControl = new Controls.OpenGL.SceneGLControl(this.plWorkSpace);
            SceneControl.InvalidOpenGLVersion += SceneControl_InvalidOpenGLVersion;
            SceneControl.RenderCompleted += SceneControl_RenderCompleted;



            //SceneControl.SelectedObjectChanged += SceneControl_SelectedObjectChanged;
            SceneControl.RotationAxisXChanged += SceneControl_RotationAxisXChanged;
            SceneControl.RotationAxisYChanged += SceneControl_RotationAxisYChanged;
            SceneControl.RotationAxisZChanged += SceneControl_RotationAxisZChanged;
            SceneControl.MoveTranslationCompleted += SceneControl_MoveTranslationCompleted;

            //SceneControl.ToolTipSelectedModelMoveClicked += SceneControl_ToolTipSelectedModelMoveClicked;
            //SceneControl.ToolTipSelectedModelRotateClicked += SceneControl_ToolTipSelectedModelRotateClicked;

            //StatusbarManager.Initialize(this.statusStrip1, SceneControl);
        }

        private void SceneControl_InvalidOpenGLVersion(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SceneControl_RenderCompleted(object sender, EventArgs e)
        {
        }

        #region HOTKEYS

        private void SceneControl_HotKeyEscPressed(object sender, EventArgs e)
        {
            //ObjectView.RemoveSelection();

            //ToolStripActionsManager.Update(MainFormToolStripActionType.btnSelectPressed, SceneControl);
        }

        private void SceneControl_HotKeyCtlCPressed(object sender, EventArgs e)
        {
            var selectedModel = ObjectView.SelectedModel;
            if (ObjectView.SelectedModel != null)
            {
                var cloneModel = (STLModel3D)selectedModel.Clone(false);
                cloneModel.BindModel();
                ObjectView.AddModel(cloneModel);

                ToolbarActionsManager.Update(MainFormToolStripActionType.btnSelectPressed, frmStudioMain.SceneControl, null);
            }
        }

        private void SceneControl_HotKeyCtlPPressed(object sender, EventArgs e)
        {
            this.btnExport_Click(null, null);
        }

        private void SceneControl_HotKeyCtlZPressed(object sender, EventArgs e)
        {
            if (SceneView.CurrentViewMode != SceneView.ViewMode.MagsAI)
            {
                ProcessUndoOperation();
            }
        }

        private void SceneControl_HotKeySPressed(object sender, EventArgs e)
        {
            var modelToolbar = SceneControlToolbarManager.Toolbars.OfType<SceneControlModelActionsToolbar>().First();
            modelToolbar.ResetBackgroundColors();
            ToolbarActionsManager.Update(MainFormToolStripActionType.btnSelectPressed, SceneControl, modelToolbar.ModelSupportButton);
        }

        private void SceneControl_HotKeyXPressed(object sender, EventArgs e)
        {
            var modelToolbar = SceneControlToolbarManager.Toolbars.OfType<SceneControlModelActionsToolbar>().First();
            modelToolbar.ResetBackgroundColors();
            ToolbarActionsManager.Update(MainFormToolStripActionType.btnScalePressed, SceneControl, modelToolbar.ModelScaleButton);
            modelToolbar.SelectButton(modelToolbar.ModelScaleButton);
        }

        private void SceneControl_HotKeyFPressed(object sender, EventArgs e)
        {
            var modelToolbar = SceneControlToolbarManager.Toolbars.OfType<SceneControlModelActionsToolbar>().First();
            modelToolbar.SelectButton(modelToolbar.ModelSupportButton);
        }

        private void SceneControl_HotKeyDeletePressed(object sender, EventArgs e)
        {
            if (ObjectView.SelectedObject is SupportCone)
            {
                SceneControl_RemoveSupportConeClicked(ObjectView.SelectedObject as SupportCone);
            }
            else if (ObjectView.SelectedObject is STLModel3D)
            {
                SceneControl_RemoveModelClicked(ObjectView.SelectedObject as STLModel3D);

            }
            else if (ObjectView.SelectedObject is TriangleSurfaceInfo)
            {
                SceneControl_RemoveSurfaceSupportClicked(ObjectView.SelectedObject as TriangleSurfaceInfo);

            }

            MemoryHelpers.ForceGCCleanup();
        }

        private void SceneControl_HotKeyZPressed(object sender, EventArgs e)
        {
            var cameraToolbar = SceneControlToolbarManager.Toolbars.OfType<SceneControlCameraActionsToolbar>().First();
            ToolbarActionsManager.Update(MainFormToolStripActionType.btnZoomPressed, SceneControl, cameraToolbar.CameraActionZoom);
        }

        private void SceneControl_HotKeyOPressed(object sender, EventArgs e)
        {
            var cameraToolbar = SceneControlToolbarManager.Toolbars.OfType<SceneControlCameraActionsToolbar>().First();
            ToolbarActionsManager.Update(MainFormToolStripActionType.btnOrbitPressed, SceneControl, cameraToolbar.CameraActionOrbit);
        }

        private void SceneControl_HotKeyGPressed(object sender, EventArgs e)
        {
            var modelToolbar = SceneControlToolbarManager.Toolbars.OfType<SceneControlModelActionsToolbar>().First();
            modelToolbar.SelectButton(new PictureBox() { Name = "gridsupport" });
            ToolbarActionsManager.Update(MainFormToolStripActionType.btnModelActionMagsAIGridSupport, SceneControl, modelToolbar.ModelSupportButton);
        }

        private void SceneControl_HotKeyQPressed(object sender, EventArgs e)
        {
            var modelToolbar = SceneControlToolbarManager.Toolbars.OfType<SceneControlModelActionsToolbar>().First();
            modelToolbar.SelectButton(new PictureBox() { Name = "manualsupport" });
            ToolbarActionsManager.Update(MainFormToolStripActionType.btnModelActionMagsAIManualSupport, SceneControl, modelToolbar.ModelSupportButton);
        }

        private void SceneControl_HotKeyRPressed(object sender, EventArgs e)
        {
            SceneControlToolbarManager.ModelActionsToolbar.SelectButton(SceneControlToolbarManager.ModelActionsToolbar.ModelRotateButton);
        }

        private void SceneControl_HotKeyPPressed(object sender, EventArgs e)
        {
            var cameraToolbar = SceneControlToolbarManager.Toolbars.OfType<SceneControlCameraActionsToolbar>().First();
            ToolbarActionsManager.Update(MainFormToolStripActionType.btnPanPressed, SceneControl, cameraToolbar.CameraActionPan);
        }

        private void SceneControl_HotKeyDPressed(object sender, EventArgs e)
        {
            SceneControlToolbarManager.ModelActionsToolbar.SelectButton(SceneControlToolbarManager.ModelActionsToolbar.ModelDuplicateButton);
        }

        private void SceneControl_HotKeyMPressed(object sender, EventArgs e)
        {
            SceneControlToolbarManager.ModelActionsToolbar.SelectButton(SceneControlToolbarManager.ModelActionsToolbar.ModelMoveButton);
        }

        #endregion

        #region SceneControl events

        private void SceneControl_MoveTranslationCompleted(Vector3Class deltaMoveTranslation)
        {
            var selectedObject = ObjectView.SelectedObject;
            if (selectedObject is SupportCone)
            {
                var selectedSupportCone = selectedObject as SupportCone;

                //selectedSupportCone.MoveTranslation += deltaMoveTranslation;
                selectedSupportCone.UpdateSupportHeight();

                SceneView.MoveTranslation3DGizmo.UpdateControl(Enums.SceneViewSelectedMoveTranslationAxisType.NoAxisSelected, true);

                var translateManualSupportCone = new TranslateManualSupportCone() { ModelIndex = selectedSupportCone.Model.Index, MoveTranslation = -deltaMoveTranslation };

                var selectedSupportConeIndex = 0;
                foreach (var supportCone in selectedSupportCone.Model.SupportStructure)
                {
                    if (supportCone.Position == selectedSupportCone.Position)
                    {
                        translateManualSupportCone.SupportConeIndex = selectedSupportConeIndex;
                        break;
                    }

                    selectedSupportConeIndex++;
                }


                SupportConeMoveTranslate(translateManualSupportCone, false);
            }
            else if (selectedObject is STLModel3D && !(selectedObject is GroundPane))
            {
                var selectedModel = ObjectView.SelectedModel;
                selectedModel.DisableSupportDrawing = false;

                selectedModel.UpdateBoundries();

                var deltaMovement = new Vector3Class(0, 0, -selectedModel.BottomPoint + deltaMoveTranslation.Z); //find delta between lowest point (ground) and deltamovetranslation
                selectedModel.Triangles.UpdateWithMoveTranslation(deltaMovement);
                selectedModel.UpdateBoundries();
                selectedModel.UpdateBinding();

                selectedModel.PreviewMoveTranslation = new Vector3Class();

                var translateModel = new TranslateModel() { IsUndo = true, ModelIndex = selectedModel.Index, TotalMoveTranslation = -deltaMoveTranslation };
                ModelMoveTranslate(translateModel, false);

                //selectedModel.UpdateSelectionboxText();
                //SceneActionControlManager.UpdateMoveTranslationValues(selectedModel.MoveTranslation, false);
            }
        }

        #endregion

        #region Rotation Gizmo

        void RotationProcess(ActionModel actionModel, bool isUndoAction)
        {
            var selectedMaterialSummary = PrintJobManager.SelectedMaterialSummary;

            var rotationEvent = (RotationEventArgs)actionModel.Arguments[0];
            if (isUndoAction)
            {

                //save current state
                var tmpPath = Path.Combine(Settings.RoamingAutoSavePath, Guid.NewGuid().ToString() + ".tmp");
                actionModel.Arguments.Add(tmpPath);
                ExportEngine.ExportSelectedModelProperties(tmpPath, actionModel.ModelIndex);

                var selectedModel = ObjectView.SelectedModel;

                if (selectedModel is STLModel3D)
                {
                    try
                    {
                        var stlModel = selectedModel as STLModel3D;

                        switch (rotationEvent.Axis)
                        {
                            case RotationEventArgs.TypeAxis.X:
                                stlModel.Rotate(rotationEvent.RotationAngle, stlModel.RotationAngleY, stlModel.RotationAngleZ, rotationEvent.Axis);
                                stlModel.UpdateBoundries();
                                stlModel.UpdateDefaultCenter();
                                stlModel.PreviewRotationX = 0;

                                break;
                            case RotationEventArgs.TypeAxis.Y:
                                stlModel.Rotate(stlModel.RotationAngleX, rotationEvent.RotationAngle, stlModel.RotationAngleZ, rotationEvent.Axis);
                                stlModel.UpdateBoundries();
                                stlModel.UpdateDefaultCenter();
                                stlModel.PreviewRotationY = 0;

                                break;
                            case RotationEventArgs.TypeAxis.Z:
                                stlModel.Rotate(stlModel.RotationAngleX, stlModel.RotationAngleY, rotationEvent.RotationAngle, rotationEvent.Axis);
                                stlModel.UpdateBoundries();
                                //stlModel.UpdateDefaultCenter();
                                stlModel.PreviewRotationZ = 0;

                                break;
                        }

                        stlModel.Triangles.UpdateConnections();

                        if (rotationEvent.Axis != RotationEventArgs.TypeAxis.Z)
                        {
                            stlModel.SupportStructure.Clear();
                            stlModel.UpdateBoundries();
                            //move to center position
                            stlModel.UpdateDefaultCenter();
                            stlModel.Triangles.CalcHorizontalSurfaces(stlModel.MoveTranslationZ);
                            stlModel.Triangles.CalcFlatSurfaces();

                            stlModel.LiftModelOnSupport();

                            //if (stlModel.ZSupport && selectedMaterialSummary != null) SupportEngine.CreateZSupport(stlModel, selectedMaterialSummary.Material);
                            //if (stlModel.ZSupport && selectedMaterialSummary != null) SupportEngine.CreateSurfaceSupport(null, stlModel, selectedMaterialSummary.Material);
                            //if (stlModel.ZSupport && selectedMaterialSummary != null) ModelWithGroundDistanceFix(stlModel);
                        }

                        stlModel.UpdateBoundries();
                        //if (stlModel.ZSupport)
                        //{
                        //    stlModel.UpdateSupportBasement();
                        //}
                        //stlModel.UpdateSelectionboxText();

                        //  stlModel.Triangles.ClearConnections();
                        stlModel.UpdateBinding();
                        stlModel.Triangles.UpdateSelectedOrientationTriangles(selectedModel);


                        if (SceneView.Rotation3DGizmo != null)
                        {
                            switch (rotationEvent.Axis)
                            {
                                case RotationEventArgs.TypeAxis.X:
                                    SceneView.Rotation3DGizmo.UpdateControl(Enums.SceneViewSelectedRotationAxisType.X);
                                    break;
                                case RotationEventArgs.TypeAxis.Y:
                                    SceneView.Rotation3DGizmo.UpdateControl(Enums.SceneViewSelectedRotationAxisType.Y);
                                    break;
                                case RotationEventArgs.TypeAxis.Z:
                                    SceneView.Rotation3DGizmo.UpdateControl(Enums.SceneViewSelectedRotationAxisType.Z);
                                    break;
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        //        LoggingManager.WriteToLog("RotationProcessingAsync", "Exception", exc);
                    }
                }
            }
            else if (!isUndoAction)
            {
                //deserial undo model
                var undoModel = ImportEngine.ImportModelUndoFileAsync((string)actionModel.Arguments[1], actionModel.ModelIndex);
                ObjectView.SelectObjectByIndex(actionModel.ModelIndex);
                var selectedModel = ObjectView.SelectedModel;
                selectedModel.RevertFromUndo(undoModel);
            }

        }

        private void SceneControl_RotationAxisXChanged(float rotationXDegrees)
        {
            var selectedModel = ObjectView.SelectedModel;
            //SceneActionControlManager.UpdateRotationValues(selectedModel.RotationAngleX + rotationXDegrees, selectedModel.RotationAngleY, selectedModel.RotationAngleZ);
            SceneView.Rotation3DGizmo.UpdateSelectedAngleOverlay(selectedModel, RotationEventArgs.TypeAxis.X);
        }

        private void SceneControl_RotationAxisYChanged(float rotationYDegrees)
        {
            var selectedModel = ObjectView.SelectedModel;
            // StatusbarManager.UpdateActionMessage("Rotation angle Y (Δ degrees): " + string.Format("{0:0.00}", rotationYDegrees));
            SceneView.Rotation3DGizmo.UpdateSelectedAngleOverlay(selectedModel, RotationEventArgs.TypeAxis.Y);

            //SceneActionControlManager.UpdateRotationValues(selectedModel.RotationAngleX, selectedModel.RotationAngleY + rotationYDegrees, selectedModel.RotationAngleZ);
            //QuickTipManager.UpdateText("Selected Rotation-Axis: Y" + Environment.NewLine + "Rotation angle: " + string.Format("{0:0.00}", rotationYDegrees) + "°");
        }

        private void SceneControl_RotationAxisZChanged(float rotationZDegrees)
        {
            // StatusbarManager.UpdateActionMessage("Rotation angle Z (Δ degrees): " + string.Format("{0:0.00}", rotationZDegrees));

            var selectedModel = ObjectView.SelectedModel;
            SceneView.Rotation3DGizmo.UpdateSelectedAngleOverlay(selectedModel, RotationEventArgs.TypeAxis.Z);

            //SceneActionControlManager.UpdateRotationValues(selectedModel.RotationAngleX, selectedModel.RotationAngleY, selectedModel.RotationAngleZ + rotationZDegrees);
            // QuickTipManager.UpdateText("Selected Rotation-Axis: Z" + Environment.NewLine + "Rotation angle: " + string.Format("{0:0.00}", rotationZDegrees) + "°");
        }

        #endregion

        private void SceneActionControlManager_ScalingChanged(object sender, ScaleEventArgs e)
        {
            var selectedModel = ObjectView.SelectedModel;
            if (selectedModel != null && selectedModel is STLModel3D)
            {
                var stlModel = selectedModel as STLModel3D;
                switch (e.Axis)
                {
                    case ScaleEventArgs.TypeAxis.ALL:
                        stlModel.Scale(e.ScaleFactor, e.ScaleFactor, e.ScaleFactor, e.Axis, true, true);
                        break;
                    case ScaleEventArgs.TypeAxis.X:
                        stlModel.Scale(e.ScaleFactor, stlModel.ScaleFactorY, stlModel.ScaleFactorZ, e.Axis, true, true);
                        break;
                    case ScaleEventArgs.TypeAxis.Y:
                        stlModel.Scale(stlModel.ScaleFactorX, e.ScaleFactor, stlModel.ScaleFactorZ, e.Axis, true, true);
                        break;
                    case ScaleEventArgs.TypeAxis.Z:
                        stlModel.Scale(stlModel.ScaleFactorX, stlModel.ScaleFactorY, e.ScaleFactor, e.Axis, true, true);
                        break;
                }

                stlModel.UpdateBinding();
                SceneControl.Render();
            }
        }

        void SceneControl_RemoveSupportConeClicked(SupportCone supportCone)
        {
            if (supportCone != null)
            {

                if (supportCone.IsSurfaceSupport)
                {
                    var selectedSurface = supportCone.SupportSurface;
                    var selectedModel = selectedSurface.Model as STLModel3D;
                    selectedSurface.SupportStructure.Clear();
                    selectedSurface.UpdateBoundries(selectedModel.Triangles);
                    selectedSurface.SupportDistance = 0;
                }
                else
                {
                    var selectedModel = supportCone.Model;

                    var supportConeSelectedIndex = selectedModel.SupportStructure.FindIndex(s => s.Position == supportCone.Position);
                    var undoAction = new AddManualSupportCone() { ModelIndex = selectedModel.Index, SupportCone = supportCone, SupportConeIndex = supportConeSelectedIndex };
                    UndoRedoManager.GetInstance.PushReverseAction(x => RevertRemoveManualSupportCone(x, true), undoAction);

                    selectedModel.SupportStructure.Remove(supportCone);
                    SceneActionControlManager.RemoveSupportPropertiesHandle();

                    if (selectedModel != null)
                    {
                        selectedModel.UpdateSupportBasement();
                    }
                }
            }

            SceneControl.Render();
        }

        void SceneControl_RemoveModelClicked(STLModel3D stlModel)
        {
            if (ObjectView.SelectedModel == stlModel)
            {
                if (new frmMessageBox("Remove selected model", "Are you sure you want to remove the selected model?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button3).ShowDialog() == DialogResult.Yes)
                {
                    SceneView.Reset3DGizmos();

                    //save object to undo file
                    var tmpPath = Path.Combine(Settings.RoamingAutoSavePath, Guid.NewGuid().ToString() + ".tmp");
                    ExportEngine.ExportSelectedModelProperties(tmpPath, stlModel.Index);

                    var addModel = new AddModel() { ModelIndex = stlModel.Index, UndoFilePath = tmpPath, IsUndo = true };

                    UndoRedoManager.GetInstance.PushReverseAction(x => RevertRemoveModel(x, true), addModel);

                    stlModel.Remove();
                    ToolbarActionsManager.Update(MainFormToolStripActionType.btnSelectPressed, frmStudioMain.SceneControl, null);

                    SceneControl.Render();
                }
            }
        }

        void SceneControl_RemoveSurfaceSupportClicked(TriangleSurfaceInfo surface)
        {
            var selectedModel = ObjectView.SelectedModel;
            if (selectedModel != null)
            {
                var selectedSurfaceIndex = 0;
                foreach (var flatSurface in selectedModel.Triangles.FlatSurfaces)
                {
                    if (flatSurface == surface)
                    {
                        var workspaceFlatSurface = new WorkspaceSTLModelFlatSurface();
                        foreach (var surfaceSupportCone in flatSurface.SupportStructure)
                        {
                            workspaceFlatSurface.SupportStructure.Add(new WorkspaceSTLModelSupport()
                            {
                                BottomHeight = surfaceSupportCone.BottomHeight,
                                BottomRadius = surfaceSupportCone.BottomRadius,
                                Color = ColorTranslator.ToHtml(surfaceSupportCone.Color),
                                MiddleRadius = surfaceSupportCone.MiddleRadius,
                                Position = surfaceSupportCone.Position.ToStruct(),
                                SlicesCount = surfaceSupportCone._slicesCount,
                                TopHeight = surfaceSupportCone.TopHeight,
                                TopRadius = surfaceSupportCone.TopRadius,
                                TotalHeight = surfaceSupportCone.TotalHeight,
                                MoveTranslation = surfaceSupportCone.MoveTranslation.ToStruct(),
                                RotationAngleX = surfaceSupportCone.RotationAngleX,
                                RotationAngleZ = surfaceSupportCone.RotationAngleZ,
                                SupportPenetrationEnabled = surfaceSupportCone.PenetrationEnabled,
                                BottomWidthCorrection = surfaceSupportCone.BottomWidthCorrection
                            });
                        }

                        var undoGridSupport = new AddGridSupport() { ModelIndex = selectedModel.Index, IsFlatSurface = true, SelectedSurfaceIndex = selectedSurfaceIndex, Surface = workspaceFlatSurface };
                        UndoRedoManager.GetInstance.PushReverseAction(x => RevertGridSupport(x, true), undoGridSupport);

                        flatSurface.SupportStructure.Clear();
                        break;
                    }

                    selectedSurfaceIndex++;
                }

                foreach (var horizontalSurface in selectedModel.Triangles.HorizontalSurfaces)
                {
                    if (horizontalSurface == surface)
                    {
                        var workspaceHorizontalSurface = new WorkspaceSTLModelHorizontalSurface();
                        foreach (var surfaceSupportCone in horizontalSurface.SupportStructure)
                        {
                            workspaceHorizontalSurface.SupportStructure.Add(new WorkspaceSTLModelSupport()
                            {
                                BottomHeight = surfaceSupportCone.BottomHeight,
                                BottomRadius = surfaceSupportCone.BottomRadius,
                                Color = ColorTranslator.ToHtml(surfaceSupportCone.Color),
                                MiddleRadius = surfaceSupportCone.MiddleRadius,
                                Position = surfaceSupportCone.Position.ToStruct(),
                                SlicesCount = surfaceSupportCone._slicesCount,
                                TopHeight = surfaceSupportCone.TopHeight,
                                TopRadius = surfaceSupportCone.TopRadius,
                                TotalHeight = surfaceSupportCone.TotalHeight,
                                MoveTranslation = surfaceSupportCone.MoveTranslation.ToStruct(),
                                RotationAngleX = surfaceSupportCone.RotationAngleX,
                                RotationAngleZ = surfaceSupportCone.RotationAngleZ,
                                SupportPenetrationEnabled = surfaceSupportCone.PenetrationEnabled,
                                BottomWidthCorrection = surfaceSupportCone.BottomWidthCorrection
                            });
                        }

                        var undoGridSupport = new AddGridSupport() { ModelIndex = selectedModel.Index, IsHorizontalSurface = true, SelectedSurfaceIndex = selectedSurfaceIndex, Surface = workspaceHorizontalSurface };
                        UndoRedoManager.GetInstance.PushReverseAction(x => RevertGridSupport(x, true), undoGridSupport);

                        horizontalSurface.SupportStructure.Clear();
                        break;
                    }
                }
            }

            frmStudioMain.SceneControl.Render();

        }

        private void SceneActionControlManager_CloneModels(object sender, CloneModelsEventArgs e)
        {
            CloneModels(e);

            if (e.ModelFootprints != null && e.ModelFootprints.Count > 0)
            {
                foreach (var modelFootprint in e.ModelFootprints)
                {
                    FootPrintActionModel footPrintActionModel = new FootPrintActionModel()
                    {
                        Amount = modelFootprint.Value,
                        ModelFootprint = modelFootprint.Key
                    };
                    UndoRedoManager.GetInstance.PushReverseAction(x => UpdateDuplicatesAction(x, true), footPrintActionModel);
                }
            }
        }

        private void UpdateDuplicatesAction(FootPrintActionModel footPrintActionModel, bool isUndoAction)
        {
            int amount = footPrintActionModel.Amount;
            ModelFootprint modelFootprint = footPrintActionModel.ModelFootprint;
            if (modelFootprint != null)
            {
                var tmpPackModelsRequest = PackingHelper.CreatePackModelsRequest(null);
                tmpPackModelsRequest.ModelFootprints.First(s => s.Model == modelFootprint.Model).RequestedCloneCount = amount;

                var solutions = PackingHelper.CalculatePackingSolutions(tmpPackModelsRequest);
                if (solutions.BestSolution != null)
                {
                    var amountOfPackagedItems = solutions.BestSolution.PackedItems.Count(s => s.ModelFootprint.Model == modelFootprint.Model);
                    if (amountOfPackagedItems < amount)
                    {
                        tmpPackModelsRequest = PackingHelper.CreatePackModelsRequest(null);

                        solutions = PackingHelper.CalculatePackingSolutions(tmpPackModelsRequest);
                    }
                }


                Dictionary<ModelFootprint, int> modelFootprints = new Dictionary<ModelFootprint, int>();
                modelFootprints.Add(modelFootprint, amount);

                var cloneModelsEventArgs = new CloneModelsEventArgs()
                {
                    PackModelsRequest = tmpPackModelsRequest,
                    PackagingSolutions = solutions,
                    ModelFootprints = modelFootprints
                };

                CloneModels(cloneModelsEventArgs);
            }
        }

        private void CloneModels(CloneModelsEventArgs e)
        {
            var packModelsRequest = e.PackModelsRequest;
            var solutions = e.PackagingSolutions;

            foreach (var modelFootprint in packModelsRequest.ModelFootprints)
            {
                modelFootprint.Model.LinkedClones.Clear();
                // Remove movetranslation
                modelFootprint.Model.MoveTranslation = new Vector3Class(0, 0, modelFootprint.Model.MoveTranslation.Z);
            }

            // Correct for origin not in center but left-bottom 
            var translationCorrection = new Vector3Class(-0.5f * packModelsRequest.BuildPlatform.SizeX, -0.5f * packModelsRequest.BuildPlatform.SizeY, 0);
            // Recenter group on buildplatform
            if (solutions.BestSolution != null)
            {
                if (solutions.BestSolution.PackedItems.Count > 0)
                {
                    float totalWidth = solutions.BestSolution.PackedItems.Max(i => i.TotalPositionX);
                    float totalHeight = solutions.BestSolution.PackedItems.Max(i => i.TotalPositionY);

                    translationCorrection += new Vector3Class(0.5f * (packModelsRequest.BuildPlatform.SizeX - totalWidth), 0.5f * (packModelsRequest.BuildPlatform.SizeY - totalHeight), 0);

                    //first process the single instance objects
                    for (var modelIndex = 0; modelIndex < solutions.BestSolution.FootprintCloneCount.Length; modelIndex++)
                    {
                        if (solutions.BestSolution.FootprintCloneCount[modelIndex] == 1)
                        {
                            //find modeltype
                            var modelFootprint = solutions.BestSolution.Footprints[modelIndex];
                            var modelPackedItem = solutions.BestSolution.PackedItems.First(s => s.ModelFootprint == modelFootprint);
                            var modelTranslation = new Vector3Class(modelPackedItem.GetTranslation()) + new Vector3Class(0.5f * modelPackedItem.TotalSizeX, 0.5f * modelPackedItem.TotalSizeY, 0) + translationCorrection;
                            modelTranslation.Z = modelPackedItem.ModelFootprint.Model.MoveTranslation.Z;
                            modelPackedItem.ModelFootprint.Model.MoveTranslation = modelTranslation;

                            if (modelPackedItem.RotateModel)
                            {
                                modelPackedItem.ModelFootprint.Model._rotationAngleZ = 0;
                                modelPackedItem.ModelFootprint.Model.Rotate(0, 0, 90, RotationEventArgs.TypeAxis.Z, false, false);
                                modelPackedItem.ModelFootprint.Model.UpdateBinding();
                            }
                        }
                    }

                    for (var modelIndex = 0; modelIndex < solutions.BestSolution.FootprintCloneCount.Length; modelIndex++)
                    {
                        if (solutions.BestSolution.FootprintCloneCount[modelIndex] > 1)
                        {
                            //find modeltype
                            var modelFootprint = solutions.BestSolution.Footprints[modelIndex];
                            foreach (var packedItem in solutions.BestSolution.PackedItems.Where(s => s.ModelFootprint == modelFootprint))
                            {
                                LinkedClone linkedClone = new LinkedClone()
                                {
                                    Translation = new Vector3Class(packedItem.GetTranslation()) + new Vector3Class(0.5f * packedItem.TotalSizeX, 0.5f * packedItem.TotalSizeY, 0) + translationCorrection,
                                    Rotate = packedItem.RotateModel,
                                    SizeX = packedItem.SizeX,
                                    SizeY = packedItem.SizeY
                                };
                                packedItem.ModelFootprint.Model.LinkedClones.Add(linkedClone);
                            }
                        }
                    }
                }
            }

            SceneControl.Render();
        }

        #region SceneActionManager
        private void SceneActionControlManager_RotationChanged(object sender, RotationEventArgs e)
        {
            var selectedModel = ObjectView.SelectedModel;
            if (selectedModel != null)
            {
                switch (e.Axis)
                {
                    case RotationEventArgs.TypeAxis.X:
                        selectedModel.RotationAngleX = e.RotationAngle;
                        break;
                    case RotationEventArgs.TypeAxis.Y:
                        selectedModel.RotationAngleY = e.RotationAngle;
                        break;
                    case RotationEventArgs.TypeAxis.Z:
                        selectedModel.RotationAngleZ = e.RotationAngle;
                        break;
                }
            }
        }

        private void SceneActionControlManager_ScaleChanged(object sender, ScaleEventArgs e)
        {
            var selectedModel = ObjectView.SelectedModel;
            if (selectedModel != null)
            {
                switch (e.Axis)
                {
                    case ScaleEventArgs.TypeAxis.ALL:
                        selectedModel.Scale(e.ScaleFactor, e.ScaleFactor, e.ScaleFactor, ScaleEventArgs.TypeAxis.ALL, true, true);
                        break;

                }

                selectedModel.UpdateBinding();
                SceneControlToolbarManager.UpdateModelDimensions(selectedModel.Width, selectedModel.Depth, selectedModel.Height);
                frmStudioMain.SceneControl.Render();
            }
        }

        private void SceneActionControlManager_MoveTranslationChanged(object sender, MovementEventArgs e)
        {
            var selectedObject = ObjectView.SelectedObject;

            var moveTranslation = new Vector3Class();
            if (selectedObject is SupportCone)
            {
                var selectedSupportCone = selectedObject as SupportCone;
                switch (e.MoveAxisType)
                {
                    case SceneViewSelectedMoveTranslationAxisType.X:
                        moveTranslation.X = -(selectedSupportCone.MoveTranslationX - e.MoveTranslationX);
                        break;
                    case SceneViewSelectedMoveTranslationAxisType.Y:
                        moveTranslation.Y = -(selectedSupportCone.MoveTranslationY - e.MoveTranslationY);
                        break;
                }

                SceneControl_MoveTranslationCompleted(moveTranslation);
            }
            else if (selectedObject is STLModel3D && !(selectedObject is GroundPane))
            {
                var selectedModel = selectedObject as STLModel3D;
                if (selectedModel != null)
                {
                    switch (e.MoveAxisType)
                    {
                        case SceneViewSelectedMoveTranslationAxisType.X:
                            moveTranslation.X = -(selectedModel.MoveTranslationX - e.MoveTranslationX);
                            break;
                        case SceneViewSelectedMoveTranslationAxisType.Y:
                            moveTranslation.Y = -(selectedModel.MoveTranslationY - e.MoveTranslationY);
                            break;
                        case SceneViewSelectedMoveTranslationAxisType.Z:
                            moveTranslation.Z = -(selectedModel.MoveTranslationZ - e.MoveTranslationZ);
                            break;
                    }

                    SceneControl_MoveTranslationCompleted(moveTranslation);
                }
            }

        }

        #endregion

        private void PRINTJOBPROPERTIESPANEL_MaterialIndexChanged(object sender, EventArgs e)
        {
            if (SceneControl != null)
            {
                SceneControl.Render();
            }
        }

        private void Plugin_PluginLoaded(IPlugin plugin)
        {
            if (plugin.HasStatusStripIcon)
            {
            }
        }

        void ObjectView_ModelAdded(object addedModel, EventArgs e)
        {
            MemoryHelpers.ForceGCCleanup();

            var modelToolbar = SceneControlToolbarManager.Toolbars.OfType<SceneControlModelActionsToolbar>().First();
            modelToolbar.SelectMAGSAIButton();

            if (SceneControlToolbarManager.PrintjobName == "Untitled")
            {
                if (((STLModel3D)addedModel).FileName != null)
                {
                    var fileName = ((STLModel3D)addedModel).FileName;

                    if (fileName.Contains("."))
                    {
                        SceneControlToolbarManager.PrintjobName = fileName.Substring(0, fileName.LastIndexOf("."));
                    }
                    else
                    {
                        SceneControlToolbarManager.PrintjobName = fileName;
                    }
                }
            }


            if (!(addedModel is BasicCorrectionModel))
            {
                var addedSTLModel = addedModel as STLModel3D;

                //add to undo queue
                var removeModelAction = new RemoveModel() { ModelIndex = addedSTLModel.Index, IsUndo = true };
                UndoRedoManager.GetInstance.PushReverseAction(x => RevertAddModel(x, true), removeModelAction);
            }
        }


        #region Async events
        #region SupportXYProcessing
        void ObjectView_SupportXYDistanceProcessing(object sender, EventArgs e)
        {
            ProcessActionStart();

            //show wait dialog
            var t = new BackgroundWorker();

            t.DoWork += new DoWorkEventHandler(SupportXYDistanceProcessingAsync);
            t.RunWorkerCompleted += new RunWorkerCompletedEventHandler(SupportXYDistanceProcessingAsync_Completed);
            t.RunWorkerAsync();

        }

        void SupportXYDistanceProcessingAsync(object sender, DoWorkEventArgs e)
        {
            var t = sender as BackgroundWorker;

            var selectedMaterialSummary = PrintJobManager.SelectedMaterialSummary;
            var object3d = ObjectView.SelectedModel;
            if (object3d is STLModel3D)
            {
                try
                {
                    var stlModel = object3d as STLModel3D;
                    stlModel.SupportStructure.Clear();
                    foreach (var surface in stlModel.Triangles.HorizontalSurfaces)
                    {
                        surface.SupportStructure.Clear();
                    }
                    stlModel.Triangles.HorizontalSurfaces.SupportStructure.Clear();
                    //if (selectedMaterialSummary != null) SupportEngine.CreateZSupport(stlModel, selectedMaterialSummary.Material);
                    if (selectedMaterialSummary != null) SupportEngine.CreateSurfaceSupport(null, stlModel, selectedMaterialSummary.Material, false);
                    if (selectedMaterialSummary != null) ModelWithGroundDistanceFix(stlModel);
                }
                catch (Exception exc)
                {
                    LoggingManager.WriteToLog("SupportXYDistanceProcessingAsync", "Exception", exc);
                }
            }
        }

        private void SupportXYDistanceProcessingAsync_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            ProcessActionEnd();

            SceneControl.Render();

        }

        #endregion

        #region SupportSurfaceXYProcessing
        void ObjectView_SupportSurfaceProcessing(object sender, EventArgs e)
        {
            ProcessActionStart();

            //show wait dialog
            var t = new BackgroundWorker();

            t.DoWork += new DoWorkEventHandler(SupportSurfaceProcessingAsync);
            t.RunWorkerCompleted += new RunWorkerCompletedEventHandler(SupportSurfaceProcessingAsync_Completed);
            t.RunWorkerAsync(sender);

        }

        void SupportSurfaceProcessingAsync(object sender, DoWorkEventArgs e)
        {
            var selectedMaterialSummary = PrintJobManager.SelectedMaterialSummary;
            var t = sender as BackgroundWorker;
            try
            {
                var surface = e.Argument as TriangleSurfaceInfo;

                //change support cones in selected horizontal surface
                surface.SupportStructure.Clear();
                var selectedModel = surface.Model;
                if (selectedModel is STLModel3D)
                {
                    var stlModel = selectedModel as STLModel3D;
                    stlModel.Triangles.SurfacePoints.Clear();
                    SupportEngine.CreateSurfaceSupport(surface, stlModel, selectedMaterialSummary.Material);

                    //select first support cone to enable supportpanel
                    if (surface.SupportStructure.Count == 0)
                    {
                        surface.SupportStructure.Add(new SupportCone() { Selected = true });
                    }
                    surface.Selected = true;
                    surface.SupportStructure[0].Selected = true;

                }

            }
            catch (Exception exc)
            {
                LoggingManager.WriteToLog("SupportSurfaceProcessingAsync", "Exception", exc);
            }
        }

        private void SupportSurfaceProcessingAsync_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            ProcessActionEnd();

            SceneControl.Render();
        }

        #endregion
        #region GroundSupportDistance
        void ObjectView_GroundSupportDistanceProcessing(object sender, EventArgs e)
        {
            ProcessActionStart();

            var t = new BackgroundWorker();

            t.DoWork += new DoWorkEventHandler(GroundSupportDistanceAsync);
            t.RunWorkerCompleted += new RunWorkerCompletedEventHandler(GroundSupportDistanceAsync_Completed);
            t.RunWorkerAsync();
        }

        private void ModelWithGroundDistanceFix(STLModel3D stlModel)
        {
            //add surface structure if model lowest point = translation-z
            var selectedMaterialSummary = PrintJobManager.SelectedMaterialSummary;
            var lowestPoint = 5000f;
            TriangleSurfaceInfo lowestSurface = null;
            foreach (var surface in stlModel.Triangles.HorizontalSurfaces)
            {
                if (surface.BottomPoint == stlModel.MoveTranslationZ)
                {
                    surface.UpdateBoundries(stlModel.Triangles);
                    if (surface.BottomPoint < lowestPoint)
                    {
                        lowestPoint = surface.BottomPoint;
                        lowestSurface = surface;
                    }
                }
            }

            //check if there are support cone to bottom
            if (stlModel.GetBottomSupportConesCount() == 0)
            {
                SupportEngine.CreateSurfaceSupport(lowestSurface, stlModel, selectedMaterialSummary.Material, true);
            }
        }

        private void GroundSupportDistanceAsync(object sender, DoWorkEventArgs e)
        {
            var selectedMaterialSummary = PrintJobManager.SelectedMaterialSummary;
            var t = sender as BackgroundWorker;

            var object3d = ObjectView.SelectedModel;
            if (object3d is STLModel3D)
            {
                try
                {
                    var stlModel = object3d as STLModel3D;
                    stlModel.SupportStructure.Clear();
                    //if (!stlModel.ZSupport) stlModel.Triangles.ClearSupportStructure();
                    stlModel.LiftModelOnSupport();
                    //if (stlModel.ZSupport && selectedMaterialSummary != null) SupportEngine.CreateZSupport(stlModel, selectedMaterialSummary.Material);
                    if ((stlModel.BottomPoint > 0 || stlModel.MoveTranslationZ > 0) && selectedMaterialSummary != null)
                    {
                        stlModel.Triangles.CalcHorizontalSurfaces(stlModel.MoveTranslationZ);
                        SupportEngine.CreateSurfaceSupport(null, stlModel, selectedMaterialSummary.Material);
                        ModelWithGroundDistanceFix(stlModel);
                    }

                    // stlModel.Triangles.ClearConnections();

                }
                catch (Exception exc)
                {
                    LoggingManager.WriteToLog("GroundSupportDistanceProcessing", "Exception", exc);
                }

            }
        }

        private void GroundSupportDistanceAsync_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            ProcessActionEnd();

            SceneControl.Render();
        }
        #endregion
        #region Rotation
        void ObjectView_RotationProcessing(object sender, RotationEventArgs e)
        {
            ProcessActionStart();

            //show wait dialog
            var t = new BackgroundWorker();
            t.DoWork += new DoWorkEventHandler(RotationProcessingAsync);
            t.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RotationProcessingAsync_Completed);
            t.RunWorkerAsync(new List<object>() { e });

        }

        private void RotationProcessingAsync(object sender, DoWorkEventArgs e)
        {
            var selectedModel = ObjectView.SelectedModel;

            ActionModel actionModel = new ActionModel();
            actionModel.BackgroundWorker = sender as BackgroundWorker;
            actionModel.Arguments = (List<object>)e.Argument;
            actionModel.IsUndo = true;
            actionModel.ModelIndex = selectedModel.Index;

            UndoRedoManager.GetInstance.PushReverseAction(x => RotationProcess(x, false), actionModel);

            RotationProcess(actionModel, true);

        }

        private void RotationProcessingAsync_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            ProcessActionEnd();

            SceneControl.Render();

            var selectedModel = ObjectView.SelectedModel;
            if (selectedModel != null)
            {
                //SceneActionControlManager.UpdateRotationValues(selectedModel.RotationAngleX, selectedModel.RotationAngleY, selectedModel.RotationAngleZ);
                SceneView.Rotation3DGizmo.UpdateControl(SceneViewSelectedRotationAxisType.None);
            }
        }

        #endregion

        void ProcessActionStart()
        {
            if (SceneControl != null) SceneControl.DisableRendering();
        }

        void ProcessActionEnd()
        {
            if (SceneControl != null) SceneControl.EnableRendering();
        }

        #region Scaling
        void ObjectView_ScaleProcessing(object sender, ScaleEventArgs e)
        {
            //ProcessActionStart();

            //var t = new BackgroundWorker();
            //t.DoWork += new DoWorkEventHandler(ScaleProcessingAsync);
            //t.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ScalingProcessingAsync_Completed);
            //t.RunWorkerAsync(new List<object>() { e });

        }

        //private void ScaleProcessingAsync(object sender, DoWorkEventArgs e)
        //{
        //    var selectedMaterialSummary = PrintJobManager.SelectedMaterialSummary;
        //    var t = sender as BackgroundWorker;
        //    var arguments = (List<object>)e.Argument;
        //    var scaleEvent = (ScaleEventArgs)arguments[0];

        //    var object3d = ObjectView.SelectedModel;
        //    if (object3d is STLModel3D)
        //    {
        //        var stlModel = object3d as STLModel3D;
        //        if (scaleEvent.Axis == ScaleEventArgs.TypeAxis.ALL)
        //        {
        //            stlModel.Scale(scaleEvent.ScaleFactor, scaleEvent.ScaleFactor, scaleEvent.ScaleFactor, ScaleEventArgs.TypeAxis.ALL, false);
        //        }
        //        else
        //        {
        //            switch (scaleEvent.Axis)
        //            {
        //                case ScaleEventArgs.TypeAxis.X:
        //                    stlModel.Scale(scaleEvent.ScaleFactor, stlModel.ScaleFactorY, stlModel.ScaleFactorZ, scaleEvent.Axis);
        //                    break;
        //                case ScaleEventArgs.TypeAxis.Y:
        //                    stlModel.Scale(stlModel.ScaleFactorX, scaleEvent.ScaleFactor, stlModel.ScaleFactorZ, scaleEvent.Axis);
        //                    break;
        //                case ScaleEventArgs.TypeAxis.Z:
        //                    stlModel.Scale(stlModel.ScaleFactorX, stlModel.ScaleFactorY, scaleEvent.ScaleFactor, scaleEvent.Axis);
        //                    break;
        //            }

        //        }

        //        stlModel.SupportStructure.Clear();
        //        stlModel.LiftModelOnSupport();
        //        //  if (stlModel.ZSupport && selectedMaterialSummary != null) SupportEngine.CreateZSupport(stlModel, selectedMaterialSummary.Material);
        //        if (stlModel.BottomPoint > 0 || stlModel.MoveTranslationZ > 0 && selectedMaterialSummary != null)
        //        {
        //            stlModel.Triangles.CalcHorizontalSurfaces(stlModel.MoveTranslationZ, PrintJobManager.SelectedMaterialSummary.Material.SupportProfiles.First().SupportTopRadius);
        //            SupportEngine.CreateSurfaceSupport(null, stlModel, selectedMaterialSummary.Material);
        //        }
        //        //    if (stlModel.ZSupport) ModelWithGroundDistanceFix(stlModel);
        //        stlModel.UpdateBoundries();
        //        //stlModel.UpdateSelectionboxText();
        //        stlModel.UpdateSupportBasement();
        //        if (ObjectView.BindingSupported)
        //        {

        //            stlModel.UpdateBinding();

        //        }

        //        //stlModel.Triangles.ClearConnections();
        //    }
        //}

        //private void ScalingProcessingAsync_Completed(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    ProcessActionEnd();

        //    SceneControl.Render();

        //}
        #endregion
        //#region InternalSupport
        //void ObjectView_InternalSupportProcessing(object sender, EventArgs e)
        //{
        //    ProcessActionStart();

        //    var t = new BackgroundWorker();
        //    t.DoWork += new DoWorkEventHandler(InternalSupportProcessingAsync);
        //    t.RunWorkerCompleted += new RunWorkerCompletedEventHandler(InternalSupportProcessingAsync_Completed);
        //    t.RunWorkerAsync();
        //}

        //private void InternalSupportProcessingAsync(object sender, DoWorkEventArgs e)
        //{
        //    var t = sender as BackgroundWorker;
        //    var selectedMaterialSummary = PrintJobManager.SelectedMaterialSummary;
        //    var object3d = ObjectView.SelectedModel;
        //    if (object3d is STLModel3D)
        //    {
        //        var stlModel = object3d as STLModel3D;
        //        stlModel.SupportStructure.Clear();
        //        //if (stlModel.ZSupport && selectedMaterialSummary != null) SupportEngine.CreateZSupport(stlModel, selectedMaterialSummary.Material);
        //        if (stlModel.ZSupport && selectedMaterialSummary != null) ModelWithGroundDistanceFix(stlModel);
        //    }
        //}

        //private void InternalSupportProcessingAsync_Completed(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    ProcessActionEnd();

        //    SceneControl.Render();
        //}

        #endregion
        //#region CrossSupport
        //void ObjectView_CrossSupportProcessing(object sender, EventArgs e)
        //{
        //    ProcessActionStart();

        //    var t = new BackgroundWorker();

        //    StatusbarManager.UpdateProgressStatus("Cross-support initializing", 1);

        //    t.DoWork += new DoWorkEventHandler(CrossSupportProcessingAsync);
        //    t.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CrossSupportProcessingAsync_Completed);
        //    t.RunWorkerAsync();
        //}

        //private void CrossSupportProcessingAsync(object sender, DoWorkEventArgs e)
        //{
        //    var t = sender as BackgroundWorker;
        //    var selectedMaterial = PRINTJOBPROPERTIESPANEL.SelectedMaterial;
        //    var object3d = ObjectView.SelectedModel;
        //    if (object3d is STLModel3D)
        //    {
        //        var stlModel = object3d as STLModel3D;
        //        StatusbarManager.UpdateProgressStatus("Clearing current support structure...", 10);
        //        stlModel.SupportStructure.Clear();
        //        StatusbarManager.UpdateProgressStatus("Updating model support...", 35);
        //        if (stlModel.ZSupport && selectedMaterial != null) SupportEngine.CreateZSupport(stlModel, selectedMaterial);
        //        StatusbarManager.UpdateProgressStatus("Updating model support...", 70);
        //        if (stlModel.ZSupport && selectedMaterial != null) SupportEngine.CreateSurfaceSupport(null, stlModel, selectedMaterial);
        //        if (stlModel.ZSupport && selectedMaterial != null) ModelWithGroundDistanceFix(stlModel);

        //        StatusbarManager.UpdateProgressStatus(string.Empty, 100);
        //    }
        //}

        //private void CrossSupportProcessingAsync_Completed(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    ProcessActionEnd();

        //    SceneControl.Render();

        //    NAVIGATIONPANEL.RefreshItems();

        //    MODELPROPERTIESPANEL.Focus();
        //}
        //#endregion

        //#region ZSupport
        //void ObjectView_ZSupportProcessing(object sender, EventArgs e)
        //{
        //    ProcessActionStart();

        //    var t = new BackgroundWorker();
        //    t.DoWork += new DoWorkEventHandler(ZSupportProcessingAsync);
        //    t.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ZSupportProcessingAsync_Completed);
        //    t.RunWorkerAsync();

        //}

        //private void ZSupportProcessingAsync(object sender, DoWorkEventArgs e)
        //{
        //    var t = sender as BackgroundWorker;

        //    var selectedMaterial = PrintJobManager.SelectedMaterial;
        //    var object3d = ObjectView.SelectedModel;
        //    if (object3d is STLModel3D)
        //    {
        //        var stlModel = object3d as STLModel3D;
        //        stlModel.SupportStructure.Clear();
        //        stlModel.Triangles.ClearSupportStructure();

        //        if (stlModel.ZSupport && selectedMaterial != null)
        //        {
        //            stlModel.Triangles.CalcHorizontalSurfaces(stlModel.MoveTranslationZ, SupportManager.DefaultSupportSettings.TopRadius);
        //            SupportEngine.CreateSurfaceSupport(null, stlModel, selectedMaterial);
        //            SupportEngine.CreateZSupport(stlModel, selectedMaterial);
        //            if ((stlModel.BottomPoint > 0 || stlModel.MoveTranslationZ > 0))
        //            {

        //                //stlModel.Triangles.Contours.CalcSupportPoints(stlModel);
        //                ///stlModel.Triangles.Contours.CreateContourSupport(stlModel);
        //                //stlModel.Triangles.CalcHorizontalSurfaces(stlModel.MoveTranslationZ);
        //                //foreach (var horizontalSurface in stlModel.Triangles.HorizontalSurfaces)
        //                //{
        //                //    horizontalSurface.SupportStructure.Clear();
        //                //    horizontalSurface.SupportDistance = 3;
        //                //}
        //            }

        //            if (stlModel.SupportBasement)
        //            {
        //                ModelWithGroundDistanceFix(stlModel);
        //            }
        //            //stlModel.UpdateSelectionboxText();
        //        }
        //        else
        //        {

        //        }

        //        stlModel.UpdateSupportBasement();

        //       // stlModel.Triangles.ClearConnections();
        //    }
        //}

        //private void ZSupportProcessingAsync_Completed(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    ProcessActionEnd();

        //    SceneControl.Render();
        //}
        //#endregion

        #region GeneratePrintJob

        private void GenerateExportAsync(object sender, DoWorkEventArgs e)
        {
            var t = sender as BackgroundWorker;
            try
            {

                RenderEngine.PrintJob = PrintJobManager.CurrentPrintJobSettings;
                RenderEngine.PreRender();
                RenderEngine.Render();

                while (RenderEngine.TotalAmountSlices != RenderEngine.TotalProcessedSlices || RenderEngine._cancelRendering)
                {
                    var progress = (((decimal)RenderEngine.TotalProcessedSlices / (decimal)RenderEngine.TotalAmountSlices) * 100);
                    SetExportProgressValue(Convert.ToInt32(progress));
                    Thread.Sleep(250);
                }
                SetExportProgressValue(100);
            }
            catch (Exception exc)
            {
                LoggingManager.WriteToLog("GenerateExportAsync", "Exc", exc);
                MessageBox.Show(exc.Message.Replace("Atum", string.Empty));

            }
        }

        private void GeneratePrintJobAsync_Completed(object sender, EventArgs e)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate { GeneratePrintJobAsync_Completed(sender, e); });
                    return;
                }
                SetExportProgressValue(100);
                if (ExportControl != null)
                {
                    new frmPrintPreview(RenderEngine.PrintJob, this);
                    ExportControl.InitPrintJob();
                }

            }
            catch (Exception exc)
            {
                // MessageBox.Show(exc.Message);
            }

            ProcessActionEnd();
        }

        #endregion


        private void ImportSTLFile(string stlPath, DAL.Materials.Material selectedMaterial)
        {
            if (!string.IsNullOrEmpty(stlPath))
            {
                ObjectView.Create(new[] { stlPath }, selectedMaterial.ModelColor, ObjectView.BindingSupported, this);

                SceneControl.Render();

                ProcessDroppedSTLFiles();
            }
        }

        private void ProcessDroppedAPFFiles()
        {
            if (MainFormManager.DroppedProjectFiles != null && MainFormManager.DroppedProjectFiles.Count > 0)
            {
                var projectFile = MainFormManager.DroppedProjectFiles[0];

                if (!this.Text.Contains("-"))
                {
                    var fileName = (new FileInfo(projectFile)).Name;
                    this.Text = this.Text + " - " + fileName;
                }

                PrintJobManager.ProjectSaveFilePath = projectFile;
                ImportEngine.ImportWorkspaceFileAsync(this, projectFile);

                MainFormManager.DroppedProjectFiles.RemoveAt(0);
            }
        }

        private void ProcessDroppedSTLFiles()
        {
            if (MainFormManager.DroppedSTLFiles != null && MainFormManager.DroppedSTLFiles.Count > 0)
            {
                var droppedSTLFile = MainFormManager.DroppedSTLFiles[0];
                MainFormManager.DroppedSTLFiles.Remove(droppedSTLFile);

                ImportSTLFile(droppedSTLFile, PrintJobManager.SelectedMaterialSummary.Material);
            }
        }



        //#endregion

        #region ContextMenu events

        #endregion

        void ObjectView_ExportObjectsAsSTL(object sender, string fileName)
        {
            ProcessActionStart();

            var t = new BackgroundWorker();
            t.DoWork += new DoWorkEventHandler(ObjectView_ExportObjectsAsSTLAsync);
            t.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ObjectView_ExportObjectsAsSTLAsync_Completed);
            t.RunWorkerAsync(fileName);

        }

        private void ObjectView_ExportObjectsAsSTLAsync(object sender, DoWorkEventArgs e)
        {
            var t = sender as BackgroundWorker;
            long numberOfTriangles = 0;
            int totalObjectsToExport = 0;
            var trianglesToExport = new List<Triangle>();
            foreach (var object3d in ObjectView.Objects3D)
            {
                if (object3d is STLModel3D && !(object3d is GroundPane))
                {
                    var stlModel = object3d as STLModel3D;
                    //stlModel.Triangles[0].AddRange(stlModel.BleedingCorrection.Triangles[0]);
                    numberOfTriangles += stlModel.TotalTriangles;
                    totalObjectsToExport++;
                }
            }

            int exportProgress = 50 / totalObjectsToExport;
            var exportProgressTotal = 5;
            foreach (var object3d in ObjectView.Objects3D)
            {
                if (object3d is STLModel3D && !(object3d is GroundPane))
                {
                    trianglesToExport.AddRange((object3d as STLModel3D).ExportToSTL());
                    exportProgressTotal += exportProgress;
                }
            }
            ExportEngine.SaveSTL_Binary((string)e.Argument, numberOfTriangles, trianglesToExport);
        }

        private void ObjectView_ExportObjectsAsSTLAsync_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            ProcessActionEnd();
        }

        #region Toolstrip events



        private void btnRedoAction_Click(object sender, EventArgs e)
        {
            ProcessRedoOperation();
        }

        #endregion

        #region Undo Handlers

        private static void ProcessUndoOperation()
        {
            UndoRedoManager.GetInstance.Undo();
        }

        private static void ProcessRedoOperation()
        {
            UndoRedoManager.GetInstance.Redo();
            ObjectView.RemoveSelection();
        }

        private void ModelMoveTranslate(TranslateModel translateModel, bool isUndoAction)
        {
            if (translateModel.ModelIndex > 0) ObjectView.SelectObjectByIndex(translateModel.ModelIndex);
            var selectedModel = ObjectView.SelectedModel;
            if (selectedModel != null && (!(selectedModel is SupportCone)))
            {
                TranslateModel undoModel = new TranslateModel();
                var stlModel = selectedModel as STLModel3D;
                if (stlModel != null && (translateModel.TotalMoveTranslation != stlModel.MoveTranslation) || ((translateModel.TotalMoveTranslation == stlModel.MoveTranslation && translateModel.IsUndo)))
                {
                    if (translateModel.IsUndo)
                    {
                        undoModel.ModelIndex = stlModel.Index;
                        undoModel.IsUndo = translateModel.IsUndo;
                        var oldValue = stlModel.MoveTranslation;
                        stlModel.PreviewMoveTranslation = new Vector3Class();
                        stlModel.MoveTranslation -= (Vector3Class)translateModel.TotalMoveTranslation;

                        undoModel.TotalMoveTranslation = stlModel.MoveTranslation - oldValue;

                        stlModel.LiftModelOnSupport();
                        //stlModel.UpdateSelectionboxText();

                        //update controls
                        var selectedLinkedClone = stlModel.LinkedClones.FirstOrDefault(s => s.Selected);
                        if (selectedLinkedClone != null)
                        {
                            SceneView.MoveTranslation3DGizmo.MoveTranslation = selectedLinkedClone.Translation + new Vector3Class(0, 0, ((stlModel.TopPoint - stlModel.BottomPoint) / 2) + stlModel.BottomPoint);
                        }
                        else
                        {
                            SceneView.MoveTranslation3DGizmo.MoveTranslation = stlModel.MoveTranslation + new Vector3Class(0, 0, (stlModel.TopPoint - stlModel.BottomPoint) / 2);
                        }

                        if (translateModel.TotalMoveTranslation.Z != 0)
                        {
                            foreach (var supportCone in stlModel.TotalObjectSupportCones)
                            {
                                supportCone.UpdateHeight(-translateModel.TotalMoveTranslation.Z);
                            }

                        }

                        UndoRedoManager.GetInstance.PushReverseAction(x => ModelMoveTranslate(x, true), undoModel);

                        frmStudioMain.SceneControl.Render();
                    }

                    stlModel.UpdateSupportBasement();
                }
            }

            frmStudioMain.SceneControl.Render();
        }

        private void SupportConeMoveTranslate(TranslateManualSupportCone translateSupportCone, bool isUndoAction)
        {
            if (translateSupportCone.ModelIndex > 0) ObjectView.SelectObjectByIndex(translateSupportCone.ModelIndex);
            var selectedModel = ObjectView.SelectedModel;

            if (selectedModel.SupportStructure.Count > translateSupportCone.SupportConeIndex)
            {
                var selectedSupportCone = selectedModel.SupportStructure[translateSupportCone.SupportConeIndex];
                if (selectedSupportCone != null)
                {
                    if (!isUndoAction)
                    {
                        var undoSupportCone = new TranslateManualSupportCone();
                        if (selectedSupportCone != null)
                        {
                            undoSupportCone.MoveTranslation = selectedSupportCone.PreviewMoveTranslation;
                            undoSupportCone.SupportConeIndex = translateSupportCone.SupportConeIndex;

                            UndoRedoManager.GetInstance.PushReverseAction(x => SupportConeMoveTranslate(x, true), undoSupportCone);

                            frmStudioMain.SceneControl.Render();
                        }
                    }
                    else
                    {
                        selectedSupportCone.MoveTranslation = translateSupportCone.MoveTranslation;
                        selectedSupportCone.UpdateSupportHeight();
                        selectedSupportCone.Selected = false;

                        frmStudioMain.SceneControl.Render();
                    }
                }

                selectedModel.UpdateSupportBasement();
            }
        }

        private void RevertRemoveModel(AddModel addModel, bool isUndoAction)
        {
            var undoModel = ImportEngine.ImportModelUndoFileAsync(addModel.UndoFilePath, addModel.ModelIndex);
            undoModel.BindModel();
            undoModel.UpdateBinding();
            ObjectView.AddModel(undoModel);
            frmStudioMain.SceneControl.Render();
        }

        private void RevertAddModel(RemoveModel removeModel, bool isUndoAction)
        {
            if (removeModel.ModelIndex > 0) ObjectView.SelectObjectByIndex(removeModel.ModelIndex);
            var selectedModel = ObjectView.SelectedModel;
            if (selectedModel != null && (!(selectedModel is SupportCone)))
            {
                selectedModel.Remove();
            }

            SceneControl.Render();
        }

        public void RevertRemoveManualSupportCone(AddManualSupportCone manualSupportCone, bool isUndoAction)
        {
            if (manualSupportCone.ModelIndex > 0) ObjectView.SelectObjectByIndex(manualSupportCone.ModelIndex);
            var selectedModel = ObjectView.SelectedModel;
            selectedModel.SupportStructure.Insert(manualSupportCone.SupportConeIndex, manualSupportCone.SupportCone);

            selectedModel.UpdateSupportBasement();

            frmStudioMain.SceneControl.Render();
        }

        private void RevertGridSupport(AddGridSupport gridSupport, bool isUndoAction)
        {
            if (gridSupport.ModelIndex > 0) ObjectView.SelectObjectByIndex(gridSupport.ModelIndex);
            var selectedModel = ObjectView.SelectedModel;

            if (gridSupport.IsFlatSurface)
            {
                var flatSurface = selectedModel.Triangles.FlatSurfaces[gridSupport.SelectedSurfaceIndex];
                var workspaceSurface = gridSupport.Surface as WorkspaceSTLModelFlatSurface;
                var flatSupportEvents = new List<ManualResetEvent>();

                foreach (var importSupportCone in workspaceSurface.SupportStructure)
                {
                    var groundSupport = (importSupportCone.Position.Z == 0 && !selectedModel.SupportBasement) || (importSupportCone.Position.Z == Core.Managers.UserProfileManager.UserProfile.SupportEngine_Basement_Thickness && selectedModel.SupportBasement) ? true : false;
                    var flatSupportEvent = new ManualResetEvent(false);
                    var surfaceClone = flatSurface;

                    ThreadPool.QueueUserWorkItem(arg =>
                    {
                        ImportEngine.AddGridSupportCone(importSupportCone.TotalHeight, importSupportCone.TopHeight, importSupportCone.TopRadius, importSupportCone.MiddleRadius, importSupportCone.BottomHeight, importSupportCone.BottomRadius,
                    importSupportCone.SlicesCount, importSupportCone.Position, new Vector3(), Color.FromArgb(selectedModel.Color.A + 100, ColorTranslator.FromHtml(importSupportCone.Color)), selectedModel, importSupportCone.MoveTranslation,
                    importSupportCone.RotationAngleX, importSupportCone.RotationAngleZ, groundSupport, surfaceClone, useSupportPenetration: importSupportCone.SupportPenetrationEnabled, supportBottomWidthCorrection: importSupportCone.BottomWidthCorrection);

                        flatSupportEvent.Set();
                    });

                    flatSupportEvents.Add(flatSupportEvent);
                }

                ThreadHelper.WaitForAll(flatSupportEvents.ToArray());
                flatSurface.UpdateBoundries(selectedModel.Triangles);
            }
            else if (gridSupport.IsHorizontalSurface)
            {
                var horizontalSurface = selectedModel.Triangles.HorizontalSurfaces[gridSupport.SelectedSurfaceIndex];
                var workspaceSurface = gridSupport.Surface as WorkspaceSTLModelHorizontalSurface;
                var horizontalSupportEvents = new List<ManualResetEvent>();

                foreach (var importSupportCone in workspaceSurface.SupportStructure)
                {
                    var groundSupport = (importSupportCone.Position.Z == 0 && !selectedModel.SupportBasement) || (importSupportCone.Position.Z == Core.Managers.UserProfileManager.UserProfile.SupportEngine_Basement_Thickness && selectedModel.SupportBasement) ? true : false;
                    var flatSupportEvent = new ManualResetEvent(false);
                    var surfaceClone = horizontalSurface;

                    ThreadPool.QueueUserWorkItem(arg =>
                    {
                        ImportEngine.AddGridSupportCone(importSupportCone.TotalHeight, importSupportCone.TopHeight, importSupportCone.TopRadius, importSupportCone.MiddleRadius, importSupportCone.BottomHeight, importSupportCone.BottomRadius,
                    importSupportCone.SlicesCount, importSupportCone.Position, new Vector3(), Color.FromArgb(selectedModel.Color.A + 100, ColorTranslator.FromHtml(importSupportCone.Color)), selectedModel, importSupportCone.MoveTranslation,
                    importSupportCone.RotationAngleX, importSupportCone.RotationAngleZ, groundSupport, surfaceClone, useSupportPenetration: importSupportCone.SupportPenetrationEnabled, supportBottomWidthCorrection: importSupportCone.BottomWidthCorrection);

                        flatSupportEvent.Set();
                    });

                    horizontalSupportEvents.Add(flatSupportEvent);
                }

                ThreadHelper.WaitForAll(horizontalSupportEvents.ToArray());
                horizontalSurface.UpdateBoundries(selectedModel.Triangles);
            }

            selectedModel.UpdateSupportBasement();

            frmStudioMain.SceneControl.Render();
        }

        #endregion

        private void btnInfo_Click(object sender, EventArgs e)
        {
            using (var contactDialog = new frmContact())
            {
                contactDialog.StartPosition = FormStartPosition.CenterParent;
                contactDialog.ShowDialog();
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            var currentSupportBasementSettings = UserProfileManager.UserProfile.Settings_Use_Support_Basement;
            var currentTouchscreenInterfaceEnabled = UserProfileManager.UserProfile.Settings_Enable_Touch_Interface_Mode;

            using (var preferenceDialog = new frmUserPreferences(UserProfileManager.UserProfile, PerformanceSettingsManager.Settings))
            {
                preferenceDialog.StartPosition = FormStartPosition.CenterParent;
                preferenceDialog.ShowDialog();
            }

            var newTouchscreenInterfaceEnabled = UserProfileManager.UserProfile.Settings_Enable_Touch_Interface_Mode;
            if (currentTouchscreenInterfaceEnabled != newTouchscreenInterfaceEnabled)
            {
                SceneView.MoveTranslation3DGizmo = new SceneMoveTranslationGizmo();
                SceneView.MoveTranslation3DGizmo.UpdateControl(SceneViewSelectedMoveTranslationAxisType.NoAxisSelected, false);

                SceneView.Rotation3DGizmo = new SceneRotationGizmo();
                SceneView.Rotation3DGizmo.UpdateControl(SceneViewSelectedRotationAxisType.None);
            }

            var newSupportBasementSettings = UserProfileManager.UserProfile.Settings_Use_Support_Basement;
            if (currentSupportBasementSettings != newSupportBasementSettings)
            {
                foreach (var object3d in ObjectView.Objects3D)
                {
                    if (object3d is STLModel3D && (!(object3d is GroundPane)))
                    {
                        var stlModel = object3d as STLModel3D;
                        if (stlModel.SupportBasement != newSupportBasementSettings)
                        {
                            stlModel.SupportBasement = newSupportBasementSettings;
                        }
                    }
                }

                SceneControl.Render();
            }
        }

        private void btnAddObject_Click(object sender, EventArgs e)
        {
            OpenModel();
        }

        private void OpenModel()
        {
            using (var popup = new OpenFileDialog())
            {
                popup.Filter = "Supported Types | *.stl; *.apf; *.3mf";
                popup.Multiselect = false;
                if (popup.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(popup.FileName))
                {
                    if (File.Exists(popup.FileName))
                    {
                        var fileName = popup.FileName;
                        List<string> arguments = new List<string>() { fileName };
                        if (arguments != null && arguments.Count() > 0)
                        {
                            MainFormManager.ProcesArguments(arguments.ToArray());
                        }

                        ProcessDroppedAPFFiles();
                        ProcessDroppedSTLFiles();
                    }
                }
            }
        }

        private void btnPrepare_Click(object sender, EventArgs e)
        {
            if (RenderEngine.TotalProcessedSlices != RenderEngine.TotalAmountSlices)
            {
                RenderEngine.Cancel();
            }

            if (MainFormManager.IsInExportMode)
            {
                ExportControl.UnloadControl();
                SceneControl.Controls.Remove(ExportControl);

                SceneControlToolbarManager.Reinitialize(SceneControl);
            }

            Color red = BrandingManager.Button_HighlightColor;
            Color black = BrandingManager.Button_BackgroundColor_LightDark;

            btnExport.BackColor = black;
            btnPrepare.BackColor = red;

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Color red = BrandingManager.Button_HighlightColor;
            Color black = BrandingManager.Button_BackgroundColor_LightDark;

            btnExport.BackColor = red;
            btnPrepare.BackColor = black;

            if (!MainFormManager.IsInExportMode)
            {
                ToolbarActionsManager.Update(MainFormToolStripActionType.btnSelectPressed, frmStudioMain.SceneControl, null);

                var printJobPanelMaterialSummary = PrintJobManager.SelectedMaterialSummary;
                if ((printJobPanelMaterialSummary.Material.InitialLayers > 0 && printJobPanelMaterialSummary.Material.LT1 == 0) || printJobPanelMaterialSummary.Material.LT2 == 0)
                {
                    new frmMessageBox("Change layer thickness", "Layer thickness must be greater than zero", MessageBoxButtons.OK, MessageBoxDefaultButton.Button2).ShowDialog();
                }
                else
                {
                    SceneControlToolbarManager.HideToolbars(frmStudioMain.SceneControl);
                    ExportControl = new ExportUserControl();
                    ExportControl.ExportCompleted += ExportControl_ExportCompleted;
                    ExportControl.Dock = DockStyle.Fill;
                    SceneControl.Controls.Add(ExportControl);

                    var t = new BackgroundWorker();
                    SetExportProgressValue(0);
                    t.DoWork += new DoWorkEventHandler(GenerateExportAsync);
                    t.RunWorkerAsync();
                }
            }
        }

        private void ExportControl_ExportCompleted(object sender, EventArgs e)
        {
            //return to prepare mode
            this.btnPrepare_Click(null, null);
        }

        private void SetExportProgressValue(int value)
        {
            if (ExportControl != null)
            {
                ExportControl.Invoke(new MethodInvoker(delegate
                {
                    ExportControl.CirclularProgressBar.Value = value;
                    if (value >= 100)
                    {
                        ExportControl.CirclularProgressBar.Visible = false;
                    }
                }));
            }
        }

        private void btnOpenFileMenu_Click(object sender, EventArgs e)
        {
            MainFormManager.ShowMenu();
        }

        private void frmMainMenuFlyout_OnSelectMenuItem(object sender, EventArgs e)
        {
            if (sender is MenuItemControl)
            {
                var item = sender as MenuItemControl;
                switch (item.ItemName)
                {
                    case "New Project":
                        clearProjectToolStripMenuItem_Click(null, null);
                        break;
                    case "Open...":
                        OpenModel();
                        break;
                    case "Save":
                        saveProjectToolStripMenuItem_Click(null, null);
                        break;
                    case "Save As...":
                        saveAsProjectToolStripMenuItem_Click(null, null);
                        break;
                    default:
                        break;
                }
            }

            MainFormManager.CloseMenu();
        }

        private void frmStudioMain_SizeChanged(object sender, EventArgs e)
        {
            SceneControlToolbarManager.Reinitialize(frmStudioMain.SceneControl);
            // SetButtonLocations();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var frmMaterialCatalogManager = new Controls.NewGui.MaterialCatalogEditor.frmMaterialCatalogManager())
            {
                frmMaterialCatalogManager.ShowDialog();
            }
        }

        private void SupportEngine_GridSupportAdded(STLModel3D selectedModel)
        {
            if (selectedModel != null)
            {
                if (UserProfileManager.UserProfile.Settings_Use_Support_Basement)
                {
                    selectedModel.UpdateSupportBasement();
                }
                else
                {
                    selectedModel.SupportBasement = false;
                }
            }
        }

        private void SceneControlToolbarManager_OnEscKeyPressed(object sender, EventArgs e)
        {
            SceneControlToolbarManager.ModelActionsToolbar.DeselectButtons();
            SceneActionControlManager.ResetPanels();
            
            var modelToolbar = SceneControlToolbarManager.Toolbars.OfType<SceneControlModelActionsToolbar>().First();
            modelToolbar.ResetBackgroundColors();
            
            SceneView.ChangeViewMode(SceneView.ViewMode.SelectObject, SceneControl, modelToolbar.ModelMoveButton);
        }

        private void frmStudioMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                SceneControlToolbarManager_OnEscKeyPressed(null, null);
            }
        }
    }
}
