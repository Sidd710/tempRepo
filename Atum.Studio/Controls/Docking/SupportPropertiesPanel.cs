using Atum.Studio.Core.Engines;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.Managers.UndoRedo;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.ModelView;
using OpenTK;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Atum.Studio.Controls.Docking
{
    public partial class SupportPropertiesPanel : DockPanelBase
    {
        internal event Action<object> ValueChanged;
        internal event EventHandler btnRemovedSupportCone_Clicked;

        private SupportCone _datasource;


        public SupportPropertiesPanel()
        {
            InitializeComponent();

            this.ToolstripIconMouseOver = BrandingManager.DockPanelSupportPropertiesMouseOver;

            this.pgSupport.MouseMove += pgSupport_MouseMove;
            this.pgSupport.SetParent(this);
            this.pgSupport.SelectedGridItemChanged += PgSupport_SelectedGridItemChanged;

            foreach (ToolStripItem button in this.toolStrip1.Items)
            {
                button.MouseMove += button_MouseMove;
            }
        }

        private void PgSupport_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            this.btnSupportApplyToAll.InvokeEnabled = false;

            if (e.NewSelection != null)
            {
                if (e.NewSelection.Label != null && (e.NewSelection.Label == "Bottom Radius" || e.NewSelection.Label == "Top Radius" || e.NewSelection.Label == "Middle Radius" || e.NewSelection.Label == "Top Height" || e.NewSelection.Label == "Bottom Height"))
                {
                    this.btnSupportApplyToAll.InvokeEnabled = true;
                }
            }
        }

        void button_MouseMove(object sender, MouseEventArgs e)
        {
            base.SetFocusToDockPanel();
        }

        void pgSupport_MouseMove(object sender, MouseEventArgs e)
        {
            base.SetFocusToDockPanel();
        }


        public SupportCone DataSource
        {
            get
            {
                return this._datasource;
            }
            set
            {
                this._datasource = value;

                //disable some categories
                if (this._datasource.SupportConeType == SupportCone.TypeSupportCone.Contour)
                {
                    this.pgSupport.HiddenProperties = new string[] {"Location", "AxisLocked", "ScaleFactorX", "ScaleFactorY", "ScaleFactorZ",
                    "GroundDistance", "InternalSupport", "ZSupport", "SupportDistance","CrossSupport",
                "RotationAngleX", "RotationAngleY", "RotationAngleZ", "SupportBasement", "MoveTranslationX", "MoveTranslationY","MoveTranslationZ"};

                }
                else if (!this._datasource.IsSurfaceSupport)
                {
                    this.pgSupport.HiddenProperties = new string[] {"Location", "AxisLocked", "ScaleFactorX", "ScaleFactorY", "ScaleFactorZ",
                    "GroundDistance",  "SupportDistance", "InternalSupport", "ZSupport", "CrossSupport",
                "RotationAngleX", "RotationAngleY", "RotationAngleZ", "SupportBasement", "OutlineOffsetDistance", "OutlineDistanceFactor", "InfillOffsetDistance", "InfillDistanceFactor"};
                }
                else if (this.DataSource.SupportConeVersion == SupportCone.TypeSupportConeVersion.Version1)
                {
                    this._datasource.SupportSurface.Selected = true;
                    this.pgSupport.HiddenProperties = new string[] {"Location", "AxisLocked", "ScaleFactorX", "ScaleFactorY", "ScaleFactorZ",
                    "GroundDistance", "InternalSupport", "ZSupport",
                "RotationAngleX", "RotationAngleY", "RotationAngleZ", "SupportBasement", "MoveTranslationZ", "OutlineOffsetDistance", "OutlineDistanceFactor", "InfillOffsetDistance", "InfillDistanceFactor"};

                }
                else if (this.DataSource.SupportConeVersion == SupportCone.TypeSupportConeVersion.Version2)
                {
                    this._datasource.SupportSurface.Selected = true;
         //           this.pgSupport.HiddenProperties = new string[] {"Location", "AxisLocked", "ScaleFactorX", "ScaleFactorY", "ScaleFactorZ",
          //          "GroundDistance", "InternalSupport", "ZSupport",
           //     "RotationAngleX", "RotationAngleY", "RotationAngleZ", "SupportBasement", "MoveTranslationZ", "OutlineOffsetDistance", "OutlineDistanceFactor", "InfillOffsetDistance", "InfillDistanceFactor"};

                }
                this.pgSupport.RefreshProperties();
                this.pgSupport.SelectedObject = this._datasource;
            }
        }

        private void pgSupport_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {

            //UNDO
            var selectedModel = this._datasource.Model;
            if (this.DataSource is SupportConeV2)
            {
                var supportConeV2 = this.DataSource as SupportConeV2;
                supportConeV2.Update();

                var manualSupportCone = this._datasource;
                var manualSupportEvent = new ManualSupportConePropertyChange();
                manualSupportEvent.PropertyName = e.ChangedItem.Label;
                manualSupportEvent.PropertyValueNew = (float)e.ChangedItem.Value;
                manualSupportEvent.PropertyValueOld = (float)e.OldValue;
                manualSupportEvent.SupportCone = this._datasource;
                manualSupportEvent.ModelIndex = selectedModel.Index;

                UndoRedoManager.GetInstance.PushReverseAction(x => SupportEngine.UndoManualSupportConeProperty(x), manualSupportEvent);

                this.ValueChanged?.Invoke(this);
            }
            else
            {
                if (!this._datasource.IsSurfaceSupport)
                {
                    //manual
                    if (e.OldValue != e.ChangedItem.Value)
                    {
                        var manualSupportCone = this._datasource;
                        var manualSupportEvent = new ManualSupportConePropertyChange();
                        manualSupportEvent.PropertyName = e.ChangedItem.Label;
                        manualSupportEvent.PropertyValueNew = (float)e.ChangedItem.Value;
                        manualSupportEvent.PropertyValueOld = (float)e.OldValue;
                        manualSupportEvent.SupportCone = this._datasource;
                        manualSupportEvent.ModelIndex = selectedModel.Index;

                        UndoRedoManager.GetInstance.PushReverseAction(x => Core.Engines.SupportEngine.UndoManualSupportConeProperty(x), manualSupportEvent);
                    }
                }
                else
                {
                    //grid support
                    var gridSupportCone = this._datasource;
                    var gridSupportEvent = new GridSupportConePropertyChange();
                    gridSupportEvent.PropertyName = e.ChangedItem.Label;

                    if (gridSupportEvent.PropertyName == "Cross Support")
                    {
                        gridSupportEvent.PropertyValueNew = ((bool)e.ChangedItem.Value) == true ? 1f : 0f;
                        gridSupportEvent.PropertyValueOld = ((bool)e.OldValue) == true ? 1f : 0f;
                    }
                    else
                    {
                        gridSupportEvent.PropertyValueNew = (float)e.ChangedItem.Value;
                        gridSupportEvent.PropertyValueOld = (float)e.OldValue;
                    }
                    gridSupportEvent.ModelIndex = selectedModel.Index;

                    var selectedSurface = this._datasource.SupportSurface;
                    if (selectedModel != null)
                    {
                        var surfaceIndex = 0;
                        foreach (var flatSurface in selectedModel.Triangles.FlatSurfaces)
                        {
                            if (flatSurface == selectedSurface)
                            {
                                gridSupportEvent.IsFlatSurface = true;
                                gridSupportEvent.SelectedSurfaceIndex = surfaceIndex;
                                break;
                            }

                            surfaceIndex++;
                        }

                        surfaceIndex = 0;
                        foreach (var horizontalSurface in selectedModel.Triangles.HorizontalSurfaces)
                        {
                            if (horizontalSurface == selectedSurface)
                            {
                                gridSupportEvent.IsHorizontalSurface = true;
                                gridSupportEvent.SelectedSurfaceIndex = surfaceIndex;
                                break;
                            }

                            surfaceIndex++;
                        }
                    }

                    UndoRedoManager.GetInstance.PushReverseAction(x => Core.Engines.SupportEngine.UndoGridSupportConeProperty(x), gridSupportEvent);
                }

                if (e.ChangedItem.Label == "Translation-X")
                {
                    if (this._datasource.IsSurfaceSupport)
                    {
                        foreach (var supportCone in this._datasource.SupportSurface.SupportStructure)
                        {
                            supportCone.MoveTranslation = new Vector3((float)e.ChangedItem.Value, supportCone.MoveTranslationY, supportCone.MoveTranslationZ);
                        }

                        this._datasource.SupportSurface.UpdateBoundries(this._datasource.Model.Triangles);
                    }
                }
                else if (e.ChangedItem.Label == "Translation-Y")
                {
                    if (this._datasource.IsSurfaceSupport)
                    {
                        foreach (var supportCone in this._datasource.SupportSurface.SupportStructure)
                        {
                            supportCone.MoveTranslation = new Vector3(supportCone.MoveTranslationX, (float)e.ChangedItem.Value, supportCone.MoveTranslationZ);
                        }
                        this._datasource.SupportSurface.UpdateBoundries(this._datasource.Model.Triangles);
                    }
                    else
                    {
                        this._datasource.MoveTranslation = new Vector3(this._datasource.MoveTranslationX, (float)e.ChangedItem.Value, this._datasource.MoveTranslationZ);
                    }
                }
                else if (e.ChangedItem.Label == "Bottom Radius" && this._datasource.IsSurfaceSupport)
                {

                    var changingEvents = new Atum.Studio.Core.Events.SupportEventArgs();
                    changingEvents.BottomHeight = this._datasource.BottomHeight;
                    changingEvents.BottomRadius = (float)e.ChangedItem.Value;
                    changingEvents.MiddleRadius = this._datasource.MiddleRadius;
                    changingEvents.TopHeight = this._datasource.TopHeight;
                    changingEvents.TopRadius = this._datasource.TopRadius;

                    this._datasource.SupportSurface.UpdateGridSupport(changingEvents);
                    //this._datasource.SupportSurface.UpdateGridSupport(changingEvents);
                }
                else if (e.ChangedItem.Label == "Bottom Height" && this._datasource.IsSurfaceSupport)
                {

                    var changingEvents = new Atum.Studio.Core.Events.SupportEventArgs();
                    changingEvents.BottomRadius = this._datasource.BottomRadius;
                    changingEvents.BottomHeight = (float)e.ChangedItem.Value;
                    changingEvents.MiddleRadius = this._datasource.MiddleRadius;
                    changingEvents.TopHeight = this._datasource.TopHeight;
                    changingEvents.TopRadius = this._datasource.TopRadius;

                    this._datasource.SupportSurface.UpdateGridSupport(changingEvents);
                }
                else if (e.ChangedItem.Label == "Middle Radius" && this._datasource.IsSurfaceSupport)
                {

                    var changingEvents = new Core.Events.SupportEventArgs();
                    changingEvents.BottomRadius = this._datasource.BottomRadius;
                    changingEvents.BottomHeight = this._datasource.BottomHeight;
                    changingEvents.MiddleRadius = (float)e.ChangedItem.Value;
                    changingEvents.TopHeight = this._datasource.TopHeight;
                    changingEvents.TopRadius = this._datasource.TopRadius;

                    this._datasource.SupportSurface.UpdateGridSupport(changingEvents);
                }
                else if (e.ChangedItem.Label == "Top Radius" && this._datasource.IsSurfaceSupport)
                {
                    var changingEvents = new Core.Events.SupportEventArgs();
                    changingEvents.BottomRadius = this._datasource.BottomRadius;
                    changingEvents.BottomHeight = this._datasource.BottomHeight;
                    changingEvents.MiddleRadius = this._datasource.MiddleRadius;
                    changingEvents.TopHeight = this._datasource.TopHeight;
                    changingEvents.TopRadius = (float)e.ChangedItem.Value;

                    this._datasource.SupportSurface.UpdateGridSupport(changingEvents);

                }
                else if (e.ChangedItem.Label == "Top Height" && this._datasource.IsSurfaceSupport)
                {

                    var changingEvents = new Core.Events.SupportEventArgs();
                    changingEvents.BottomRadius = this._datasource.BottomRadius;
                    changingEvents.BottomHeight = this._datasource.BottomHeight;
                    changingEvents.MiddleRadius = this._datasource.MiddleRadius;
                    changingEvents.TopHeight = (float)e.ChangedItem.Value;
                    changingEvents.TopRadius = this._datasource.TopRadius;

                    this._datasource.SupportSurface.UpdateGridSupport(changingEvents);

                }
                else if (e.ChangedItem.Label == "Translation-Z")
                {
                }
                else if (e.ChangedItem.Label == "Cross Support")
                {
                    //change support cones in selected horizontal surface
                    foreach (var surface in this._datasource.Model.Triangles.HorizontalSurfaces)
                    {
                        if (surface.Selected)
                        {
                            surface.SupportStructure.Clear();
                            if (selectedModel is STLModel3D)
                            {
                                var stlModel = selectedModel as STLModel3D;
                                stlModel.UpdateSurfaceSupportASync(surface);
                            }

                            break;
                        }
                    }

                    //change support cones in selected horizontal surface
                    foreach (var surface in this._datasource.Model.Triangles.FlatSurfaces)
                    {
                        if (surface.Selected)
                        {
                            surface.SupportStructure.Clear();
                            if (selectedModel is STLModel3D)
                            {
                                var stlModel = selectedModel as STLModel3D;
                                stlModel.UpdateSurfaceSupportASync(surface);
                            }

                            break;
                        }
                    }
                }
                else if (e.ChangedItem.Label == "Cone Distance")
                {
                    //change support cones in selected horizontal surface
                    foreach (var surface in this._datasource.Model.Triangles.HorizontalSurfaces)
                    {
                        if (surface.Selected)
                        {
                            surface.SupportStructure.Clear();
                            if (selectedModel is STLModel3D)
                            {
                                var stlModel = selectedModel as STLModel3D;
                                stlModel.UpdateSurfaceSupportASync(surface);
                            }

                            break;
                        }
                    }

                    //change support cones in selected flat surface
                    foreach (var surface in this._datasource.Model.Triangles.FlatSurfaces)
                    {
                        if (surface.Selected)
                        {
                            surface.SupportStructure.Clear();
                            if (selectedModel is STLModel3D)
                            {
                                var stlModel = selectedModel as STLModel3D;
                                stlModel.UpdateSurfaceSupportASync(surface);
                            }

                            break;
                        }
                    }
                }

                this.ValueChanged?.Invoke(this);
            }
        }

        private void btnSupportRemove_Click(object sender, EventArgs e)
        {
            this.btnRemovedSupportCone_Clicked?.Invoke(sender, e);
        }

        private void btnSupportApplyToAll_Click(object sender, EventArgs e)
        {
            if (this._datasource != null)
            {
                STLModel3D model = null;
                if (ObjectView.SelectedModel is SupportCone)
                {
                    model = (ObjectView.SelectedModel as SupportCone).Model;
                }
                else if (ObjectView.SelectedModel is STLModel3D)
                {
                    model = ObjectView.SelectedModel as STLModel3D;
                }
                else if (this._datasource is SupportCone)
                {
                    model = (this._datasource as SupportCone).Model;
                }

                if (model != null)
                {
                    var supportUpdateArgs = new Core.Events.SupportEventArgs();
                    var selectedPGLabel = this.pgSupport.SelectedGridItem.Label;
                    if (selectedPGLabel != null)
                    {
                        if (selectedPGLabel == "Top Radius")
                        {
                            supportUpdateArgs.TopRadius = this._datasource.TopRadius;
                        }
                        else if (selectedPGLabel == "Top Height")
                        {
                            supportUpdateArgs.TopHeight = this._datasource.TopHeight;
                        }
                        else if (selectedPGLabel == "Middle Radius")
                        {
                            supportUpdateArgs.MiddleRadius = this._datasource.MiddleRadius;
                        }
                        else if (selectedPGLabel == "Bottom Height")
                        {
                            supportUpdateArgs.BottomHeight = this._datasource.BottomHeight;
                        }
                        else if (selectedPGLabel == "Bottom Radius")
                        {
                            supportUpdateArgs.BottomRadius = this._datasource.BottomRadius;
                        }
                        SupportEngine.UpdateModelSingleSupportCones(supportUpdateArgs, model);
                    }


                    //surface support
                    foreach (var supportCone in model.Triangles.HorizontalSurfaces.SupportStructure)
                    {
                        supportCone.Update(supportUpdateArgs, model);
                    }

                    //flat support
                    foreach (var supportCone in model.Triangles.FlatSurfaces.SupportStructure)
                    {
                        supportCone.Update(supportUpdateArgs, model);
                    }
                }

                //update main scene
                this.ValueChanged?.Invoke(this);
            }
        }

        private void btnSaveAsDefaults_Click(object sender, EventArgs e)
        {
            SupportManager.DefaultSupportSettings = new DAL.Catalogs.SupportCatalogItem()
            {
                BottomHeight = this._datasource.BottomHeight,
                BottomRadius = this._datasource.BottomRadius,
                IsDefault = true,
                MiddleRadius = this._datasource.MiddleRadius,
                TopHeight = this._datasource.TopHeight,
                TopRadius = this._datasource.TopRadius
            };

            SupportManager.Save();
        }
    }
}
