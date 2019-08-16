using Atum.DAL.Compression.Zip;
using Atum.DAL.Materials;
using Atum.Studio.Controls;
using Atum.Studio.Controls.NewGui.MaterialEditor;
using Atum.Studio.Core.Engines.MagsAI;
using Atum.Studio.Core.Events;
using Atum.Studio.Core.Helpers;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Structs;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace Atum.Studio.Core.ModelView
{
    public class ObjectView
    {
        internal static event EventHandler GroundSupportDistanceProcessing;
        internal static event EventHandler SupportXYDistanceProcessing;
        internal static event EventHandler SupportSurfaceProcessing;
        //internal static event EventHandler InternalSupportProcessing;
        internal static event EventHandler CrossSupportProcessing;
        //internal static event EventHandler ZSupportProcessing;
        internal static event EventHandler<RotationEventArgs> RotationProcessing;
        internal static event EventHandler<ScaleEventArgs> ScaleProcessing;
        internal static event EventHandler ModelAdded;
        public static event EventHandler FileProcessed;
        internal static event EventHandler ModelRemoved;
        internal static event EventHandler SelectedModelChanged;


        public static bool BindingSupported { get; set; }

        public static STLModel3D GroundPaneInflated { get; set; }
        public static List<object> Objects3D = new List<object>();

        public static int NextObjectIndex
        {
            get
            {
                var nextObjectIndex = 1;

                if (Objects3D.Count > 0)
                {
                    var orderedModelIndex = new SortedList<int, bool>();
                    foreach (var objectViewObject in ObjectView.Objects3D)
                    {
                        if (!(objectViewObject is GroundPane))
                        {
                            var stlModel = objectViewObject as STLModel3D;
                            if (!orderedModelIndex.ContainsKey(stlModel.Index))
                            {
                                orderedModelIndex.Add(stlModel.Index, false);
                            }
                        }
                    }

                    //first check last id
                    if (orderedModelIndex.Count > 0 && orderedModelIndex.Count < 254)
                    {
                        nextObjectIndex = orderedModelIndex.Last().Key + 1;
                        return nextObjectIndex;
                    }
                    else if (orderedModelIndex.Count >= 254)
                    {
                        //find empty id's
                        for (var objectIndex = nextObjectIndex; objectIndex < orderedModelIndex.Count; objectIndex++)
                        {
                            if (!orderedModelIndex.ContainsKey(objectIndex))
                            {
                                return objectIndex;
                            }
                        }
                    }
                }

                return nextObjectIndex;
            }
        }

        internal static void Init()
        {
            SceneControlToolbarManager.PrintJobProperties.SelectedMaterialChanged += PrintJobProperties_SelectedMaterialChanged;

            GroundPaneInflated = new GroundPane(4000, 4000, 0.02f);

        }

        private static void PrintJobProperties_SelectedMaterialChanged(object sender, MaterialSummary e)
        {
            try
            {
                ProgressBarManager.UpdateMainPercentage(2);
                UpdateModelColors(e.Material.ModelColor);

                if (SceneView.CurrentViewMode == SceneView.ViewMode.MagsAI)
                {
                    var selectedModel = ObjectView.SelectedModel;
                    if (selectedModel != null)
                    {
                        selectedModel.ChangeTrianglesToBlendViewMode(true);
                    }
                }

                //update support cones
                ProgressBarManager.UpdateMainPercentage(10);
                var supportChangeEvent = new Events.SupportEventArgs();
                var supportProfile = e.Material.SupportProfiles.FirstOrDefault();
                if (supportProfile == null)
                {
                    supportProfile = DAL.Materials.SupportProfile.CreateDefault();
                }
                supportChangeEvent.TopRadius = supportProfile.SupportTopRadius;
                supportChangeEvent.TopHeight = supportProfile.SupportTopHeight;
                supportChangeEvent.MiddleRadius = supportProfile.SupportMiddleRadius;
                supportChangeEvent.BottomRadius = supportProfile.SupportBottomRadius;
                supportChangeEvent.BottomHeight = supportProfile.SupportBottomHeight;
                supportChangeEvent.BottomWidthCorrection = supportProfile.SupportBottomWidthCorrection;
                foreach (var model in ObjectView.Objects3D)
                {
                    if (model != GroundPane)
                    {
                        var stlModel = model as STLModel3D;
                        Engines.SupportEngine.UpdateModelSingleSupportCones(supportChangeEvent, stlModel);
                    }
                }

                ProgressBarManager.UpdateMainPercentage(30);
                foreach (var model in ObjectView.Objects3D)
                {
                    if (model != GroundPane)
                    {
                        var stlModel = model as STLModel3D;
                        lock (stlModel.Triangles.FlatSurfaces)
                        {
                            foreach (var surface in stlModel.Triangles.FlatSurfaces)
                            {
                                if (surface.SupportStructure != null && surface.SupportStructure.Count > 0)
                                {
                                    Engines.SupportEngine.UpdateSurfaceSupport(surface, stlModel, e.Material);
                                }
                            }
                        }
                    }
                }

                ProgressBarManager.UpdateMainPercentage(50);
                foreach (var model in ObjectView.Objects3D)
                {
                    if (model != GroundPane)
                    {
                        var stlModel = model as STLModel3D;
                        lock (stlModel.Triangles.HorizontalSurfaces)
                        {
                            foreach (var surface in stlModel.Triangles.HorizontalSurfaces)
                            {
                                if (surface.SupportStructure != null && surface.SupportStructure.Count > 0)
                                {
                                    Engines.SupportEngine.UpdateSurfaceSupport(surface, stlModel, e.Material);
                                }
                            }
                        }
                    }
                }


                ProgressBarManager.UpdateMainPercentage(99);
                UserProfileManager.UserProfile.SelectedMaterial = e.Material;
                UserProfileManager.Save();

                ProgressBarManager.UpdateMainPercentage(100);
            }
            catch (Exception exc)
            {
                //new frmMessageBox("PrintJobProperties_SelectedMaterialChanged", exc.Message, MessageBoxButtons.OK, MessageBoxDefaultButton.Button2).ShowDialog();
            }
        }

        internal static void RemoveSelection()
        {
            foreach (var object3d in Objects3D)
            {
                if (object3d is STLModel3D)
                {
                    var stlModel = (STLModel3D)object3d;
                    if (stlModel.Selected)
                    {
                        stlModel.Selected = false;
                    }
                    lock (stlModel.SupportStructure)
                    {
                        foreach (var supportCone in stlModel.SupportStructure)
                        {
                            if (supportCone.Selected)
                            {
                                supportCone.Selected = false;
                            }
                        }
                    }

                    lock (stlModel.Triangles.HorizontalSurfaces.SupportStructure)
                    {
                        foreach (var surface in stlModel.Triangles.HorizontalSurfaces)
                        {
                            if (surface.Selected)
                            {
                                surface.Selected = false;
                            }
                        }
                    }
                }
            }
        }

        internal static STLModel3D SelectedModel
        {
            get
            {
                lock (Objects3D)
                {
                    foreach (var object3d in Objects3D)
                    {
                        if (object3d is STLModel3D && !(object3d is GroundPane))
                        {
                            var stlModel = (STLModel3D)object3d;
                            if (stlModel.SupportStructure != null)
                            {
                                lock (stlModel.TotalObjectSupportCones)
                                {
                                    foreach (var supportCone in stlModel.TotalObjectSupportCones)
                                    {
                                        if (supportCone != null)
                                        {
                                            if (supportCone.Selected)
                                            {
                                                return stlModel;
                                            }
                                        }
                                    }
                                }
                            }


                            foreach (var surface in stlModel.Triangles.HorizontalSurfaces)
                            {
                                if (surface.Selected)
                                {
                                    return stlModel;
                                }
                            }



                            foreach (var surface in stlModel.Triangles.FlatSurfaces)
                            {
                                if (surface.Selected)
                                {
                                    return stlModel;
                                }
                            }


                            if (stlModel.Selected)
                            {
                                return stlModel;
                            }
                        }
                    }
                }

                return null;
            }
        }

        internal static void SelectObjectByIndex(int index)
        {
            foreach (var object3d in Objects3D)
            {
                if (object3d is STLModel3D)
                {
                    var stlModel = (STLModel3D)object3d;
                    if (stlModel.Index == index)
                    {
                        stlModel.Selected = true;
                    }
                    else
                    {
                        stlModel.Selected = false;
                    }
                }
            }
        }

        internal static object SelectedObject
        {
            get
            {

                foreach (var object3d in Objects3D)
                {
                    if (object3d is STLModel3D && !(object3d is GroundPane))
                    {
                        var stlModel = (STLModel3D)object3d;

                        foreach (var supportCone in stlModel.TotalObjectSupportCones)
                        {
                            if (!(supportCone is null))
                            {
                                if (supportCone.Selected)
                                {
                                    return supportCone;
                                }
                            }

                        }

                        foreach (var flatSurface in stlModel.Triangles.FlatSurfaces)
                        {
                            if (flatSurface.Selected)
                            {
                                return flatSurface;
                            }
                        }

                        foreach (var horizontalSurface in stlModel.Triangles.HorizontalSurfaces)
                        {
                            if (horizontalSurface.Selected)
                            {
                                return horizontalSurface;
                            }
                        }

                        if (stlModel.Selected)
                        {
                            return stlModel;
                        }
                    }

                }
                return null;
            }
        }

        internal static GroundPane GroundPane
        {
            get
            {
                foreach (var object3d in Objects3D)
                {
                    if (object3d is GroundPane)
                    {
                        return (GroundPane)object3d;
                    }
                }
                return null;
            }
        }

        //internal static object HighlightedModel
        //{
        //    get
        //    {
        //        foreach (var object3d in Objects3D)
        //        {
        //            if (object3d is STLModel3D)
        //            {
        //                var stlModel = (STLModel3D)object3d;
        //                if (stlModel.Highlighted)
        //                {
        //                    return stlModel;
        //                }
        //                else
        //                {
        //                    if (stlModel.SupportStructure != null)
        //                    {
        //                        lock (stlModel.SupportStructure)
        //                        {
        //                            foreach (var supportCone in stlModel.SupportStructure)
        //                            {
        //                                if (supportCone.Highlighted)
        //                                {
        //                                    return supportCone;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //            }
        //        }
        //        return null;
        //    }
        //}

        public static void AddModel(object object3d, bool raiseEvent = true, bool raiseMAGSEvent = true, bool raiseSelectedEvent = true)
        {
            if (frmStudioMain.SceneControl != null && frmStudioMain.SceneControl.InvokeRequired && (raiseSelectedEvent || raiseMAGSEvent || raiseEvent))
                {
                    frmStudioMain.SceneControl.Invoke(new MethodInvoker(delegate
                    {
                        AddModel(object3d, raiseEvent, raiseMAGSEvent, raiseSelectedEvent);
                        return;
                    }));
                }
                else
                {
                    lock (Objects3D)
                    {
                        foreach (var currentObject3d in Objects3D)
                        {
                            if (currentObject3d is STLModel3D)
                            {
                                ((STLModel3D)currentObject3d).Selected = false;
                            }
                        }

                        if (object3d is STLModel3D)
                        {
                            var stlModel = object3d as STLModel3D;

                            Objects3D.Add(stlModel);

                            if (raiseEvent)
                            {
                                if (raiseSelectedEvent) stlModel.Selected = true;
                                stlModel.SupportXYDistanceProcessing += new EventHandler(stlModel_SupportXYDistanceProcessing);
                                stlModel.SupportSurfaceProcessing += new EventHandler(stlModel_SupportSurfaceProcessing);
                                //stlModel.InternalSupportProcessing += new EventHandler(stlModel_InternalSupportProcessing);
                                stlModel.CrossSupportProcessing += new EventHandler(stlModel_CrossSupportProcessing);
                                //stlModel.ZSupportProcessing += new EventHandler(stlModel_ZSupportProcessing);
                                stlModel.RotationProcessing += new EventHandler<RotationEventArgs>(stlModel_RotationProcessing);
                                stlModel.ScaleProcessing += new EventHandler<ScaleEventArgs>(stlModel_ScaleProcessing);
                                stlModel.ModelRemoved += stlModel_ModelRemoved;
                            }

                            if (raiseMAGSEvent)
                            {
                                ModelAdded?.Invoke(stlModel, null);
                            }
                        }
                    }
                }
        }

        private static void stlModel_ModelRemoved(object sender, EventArgs e)
        {
            ModelRemoved?.Invoke(null, null);
        }

        //static void stlModel_ZSupportProcessing(object sender, EventArgs e)
        //{
        //    ZSupportProcessing?.Invoke(null, null);
        //}

        static void stlModel_CrossSupportProcessing(object sender, EventArgs e)
        {
            CrossSupportProcessing?.Invoke(null, null);
        }

        //static void stlModel_InternalSupportProcessing(object sender, EventArgs e)
        //{
        //    InternalSupportProcessing?.Invoke(null, null);
        //}

        public static void Create(string[] files, Color color, bool bindingSupported, Form parent)
        {
            if (frmStudioMain.SceneControl != null) frmStudioMain.SceneControl.DisableRendering();

            foreach (var file in files)
            {
                if (file.ToLower().EndsWith("stl") || file.ToLower().EndsWith("3mf"))
                {
                    var t = new BackgroundWorker();

                    var arguments = new List<object>();
                    arguments.Add(parent);
                    arguments.Add(files);
                    arguments.Add(color);
                    arguments.Add(bindingSupported);

                    t.DoWork += new DoWorkEventHandler(CreateAsync);
                    t.RunWorkerAsync(arguments);
                }
            }
        }

        static void CreateAsync(object sender, DoWorkEventArgs e)
        {
            var arguments = (List<object>)e.Argument;
            var parent = (frmStudioMain)arguments[0];
            var files = (string[])arguments[1];
            var color = (Color)arguments[2];
            var bindingSupported = (bool)arguments[3];

            DAL.Managers.LoggingManager.WriteToLog("DEBUG", "CreateAsync", "Params started");

            foreach (var file in files)
            {
                var selectedObject = ObjectView.SelectedObject;
                if (selectedObject != null)
                {
                    if (selectedObject is STLModel3D)
                    {
                        var stlModelSelected = selectedObject as STLModel3D;
                        stlModelSelected.Selected = false;
                    }
                    else if (selectedObject is SupportCone)
                    {
                        var supportCone = selectedObject as SupportCone;
                        supportCone.Selected = false;
                    }
                    else if (selectedObject is TriangleSurfaceInfo)
                    {
                        var surface = selectedObject as TriangleSurfaceInfo;
                        surface.Selected = false;
                        foreach (var supportCone in surface.SupportStructure)
                        {
                            supportCone.Selected = false;
                        }
                    }
                }

                var stlModel = new STLModel3D(STLModel3D.TypeObject.Model, bindingSupported);
                stlModel.ModelSelected += StlModel_ModelSelected;


                if (file.ToLower().EndsWith("3mf"))
                {
                    var fileName = file.Substring(0, file.LastIndexOf('.'));
                    fileName = fileName.Substring(file.LastIndexOf('\\') + 1);
                    List<TriangleInfoList> objectComponentTriangles = new List<TriangleInfoList>();
                    objectComponentTriangles = GenerateResources(file);

                    if (objectComponentTriangles.Count > 1) // contains multiple model parts
                    {
                        var partId = 1;
                        foreach (var objectComponentTriangle in objectComponentTriangles)
                        {
                            stlModel = new STLModel3D(STLModel3D.TypeObject.Model, bindingSupported);
                            stlModel.ModelSelected += StlModel_ModelSelected;
                            stlModel.Open(null, false, color, ObjectView.NextObjectIndex, objectComponentTriangle, objectName: fileName + "-part" + partId.ToString(), disableCentering: true);

                            stlModel.UpdateBoundries();
                            stlModel.UpdateDefaultCenter(false, useEdgeAsOrigin:true);

                            stlModel.BindModel();

                            AddModel(stlModel);
                            partId++;
                        }

                        stlModel.Selected = true;
                    }
                    else
                    {
                        stlModel = new STLModel3D(STLModel3D.TypeObject.Model, bindingSupported);
                        stlModel.ModelSelected += StlModel_ModelSelected;
                        stlModel.Open(null, false, color, ObjectView.NextObjectIndex, objectComponentTriangles[0], objectName: fileName, disableCentering: true);
                        stlModel.UpdateBoundries();
                        stlModel.UpdateDefaultCenter(false, useEdgeAsOrigin: true);
                        stlModel.BindModel();

                        AddModel(stlModel);

                    }
                }
                else
                {
                    stlModel.Open(file, false, color, ObjectView.NextObjectIndex);
                    stlModel.BindModel();

                    AddModel(stlModel);

                }

            }

            if (frmStudioMain.SceneControl != null)
            {
                frmStudioMain.SceneControl.EnableRendering();
            }

            FileProcessed?.Invoke(null, null);
            // ModelAdded?.Invoke(null, null);
        }

        internal static void UpdateModelColors(Color color)
        {
            if (SceneView.CurrentViewMode == SceneView.ViewMode.MagsAI)
            {
                //don't update the
                foreach (var model in Objects3D)
                {
                    if (model != GroundPane)
                    {
                        var stlModel = model as STLModel3D;
                        stlModel.Triangles.UpdateFaceColors(new Byte4Class(stlModel.Color.A, color.R, color.G, color.B), true);
                        stlModel.UpdateBinding();
                        ((STLModel3D)model).ChangeTrianglesToBlendViewMode();
                    }
                }
            }
            else
            {
                foreach (var model in Objects3D)
                {
                    if (model != GroundPane)
                    {

                        var stlModel = model as STLModel3D;
                        stlModel.Triangles.UpdateFaceColors(new Byte4Class(stlModel.Color.A, color.R, color.G, color.B), true);
                        stlModel.UpdateBinding();
                    }
                }
            }
        }

        private static void StlModel_ModelSelected(object sender, STLModel3D e)
        {
            SelectedModelChanged?.Invoke(sender as STLModel3D, null);
        }

        static void stlModel_ScaleProcessing(object sender, ScaleEventArgs e)
        {
            ScaleProcessing?.Invoke(null, e);
        }

        static void stlModel_RotationProcessing(object sender, RotationEventArgs e)
        {
            RotationProcessing?.Invoke(null, e);
        }

        static void stlModel_SupportXYDistanceProcessing(object sender, EventArgs e)
        {
            SupportXYDistanceProcessing?.Invoke(null, null);
        }

        static void stlModel_SupportSurfaceProcessing(object sender, EventArgs e)
        {
            SupportSurfaceProcessing?.Invoke(sender, null);
        }

        static void stlModel_GroundSupportDistanceProcessing(object sender, EventArgs e)
        {
            GroundSupportDistanceProcessing?.Invoke(null, null);
        }

        private static List<TriangleInfoList> GenerateResources(string filePath)
        {
            Models.Models3MF.Model3MF model3MF = new Models.Models3MF.Model3MF();

            if (File.Exists(filePath))
            {
                using (ZipFile _fileZip = new ZipFile(new StreamReader(filePath).BaseStream))
                {
                    if (_fileZip != null)
                    {
                        for (int i = 0; i < _fileZip.Count; i++)
                        {
                            var zipEntryFile = _fileZip.GetEntry(string.Format("{0}", _fileZip[i].Name));
                            if (zipEntryFile != null && zipEntryFile.Name.EndsWith(".model"))
                            {
                                using (var streamReader = _fileZip.GetInputStream(zipEntryFile))
                                {
                                    XmlDocument xmlDoc = new XmlDocument();
                                    xmlDoc.Load(streamReader);
                                    var resourcesNode = xmlDoc.DocumentElement.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "resources").FirstOrDefault();
                                    if (resourcesNode != null)
                                    {
                                        model3MF.Resources = new Models.Models3MF.Resources();
                                        var objectNodes = resourcesNode.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "object").ToList();
                                        if (objectNodes != null && objectNodes.Count > 0)
                                        {
                                            var objects3MF = new List<Models.Models3MF.Object3MF>();
                                            foreach (var objectNode in objectNodes)
                                            {
                                                var objectModel3MF = new Models.Models3MF.Object3MF();
                                                objectModel3MF.Id = Convert.ToInt32(objectNode.Attributes["id"].Value);
                                                objectModel3MF.Type = Convert.ToString(objectNode.Attributes["type"].Value);

                                                var meshNode = objectNode.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "mesh").FirstOrDefault();
                                                if (meshNode != null)
                                                {
                                                    var mesh = new Models.Models3MF.Mesh();

                                                    var verticesNode = meshNode.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "vertices").FirstOrDefault();
                                                    if (verticesNode != null)
                                                    {
                                                        var vertices = new List<Models.Models3MF.Vertex>();
                                                        var vertexNodes = verticesNode.Cast<XmlNode>().Where(x => x.Name == "vertex").ToList();
                                                        foreach (var vertexNode in vertexNodes)
                                                        {
                                                            float x = (float)Convert.ToDouble(vertexNode.Attributes["x"].Value);
                                                            float y = (float)Convert.ToDouble(vertexNode.Attributes["y"].Value);
                                                            float z = (float)Convert.ToDouble(vertexNode.Attributes["z"].Value);
                                                            vertices.Add(new Models.Models3MF.Vertex
                                                            {
                                                                X = x,
                                                                Y = y,
                                                                Z = z
                                                            });
                                                            //verticesList.Add(new { X = x, Y = y, Z = z });
                                                        }
                                                        mesh.Vertices = vertices;
                                                    }
                                                    var trianglesNode = meshNode.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "triangles").FirstOrDefault();
                                                    if (trianglesNode != null)
                                                    {
                                                        var triangles = new List<Models.Models3MF.Triangle>();
                                                        var triangleNodes = trianglesNode.Cast<XmlNode>().Where(x => x.Name == "triangle").ToList();
                                                        foreach (var triangleNode in triangleNodes)
                                                        {
                                                            int v1 = triangleNode.Attributes["v1"] != null ? Convert.ToInt32(triangleNode.Attributes["v1"].Value) : 0;
                                                            int v2 = triangleNode.Attributes["v2"] != null ? Convert.ToInt32(triangleNode.Attributes["v2"].Value) : 0;
                                                            int v3 = triangleNode.Attributes["v3"] != null ? Convert.ToInt32(triangleNode.Attributes["v3"].Value) : 0;

                                                            int pid = 0;
                                                            if (triangleNode.Attributes.GetNamedItem("pid") != null)
                                                            {
                                                                pid = Convert.ToInt32(triangleNode.Attributes["pid"].Value);
                                                            }

                                                            int p1 = triangleNode.Attributes["p1"] != null ? Convert.ToInt32(triangleNode.Attributes["p1"].Value) : 0;
                                                            int p2 = triangleNode.Attributes["p2"] != null ? Convert.ToInt32(triangleNode.Attributes["p2"].Value) : 0;
                                                            int p3 = triangleNode.Attributes["p3"] != null ? Convert.ToInt32(triangleNode.Attributes["p3"].Value) : 0;

                                                            triangles.Add(new Models.Models3MF.Triangle
                                                            {
                                                                V1 = v1,
                                                                V2 = v2,
                                                                V3 = v3,
                                                                PId = pid,
                                                                P1 = p1,
                                                                P2 = p2,
                                                                P3 = p3
                                                            });
                                                            //trianglesList.Add(new { V1 = v1, V2 = v2, V3 = v3, PID = pid, P1 = p1, P2 = p2, P3 = p3 });
                                                        }
                                                        mesh.Triangles = triangles;
                                                    }
                                                    objectModel3MF.Mesh = mesh;
                                                }

                                                //Components
                                                var componentsNode = objectNode.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "components").FirstOrDefault();
                                                if (componentsNode != null)
                                                {
                                                    var components = new List<Models.Models3MF.Component>();
                                                    var componentNodes = componentsNode.Cast<XmlNode>().Where(x => x.Name == "component").ToList();
                                                    foreach (var componentNode in componentNodes)
                                                    {
                                                        int objectid = componentNode.Attributes["objectid"] != null ? Convert.ToInt32(componentNode.Attributes["objectid"].Value) : 0;
                                                        string transformNodeValue = componentNode.Attributes["transform"] != null ? Convert.ToString(componentNode.Attributes["transform"].Value) : string.Empty;
                                                        var component = new Models.Models3MF.Component();
                                                        component.ObjectId = objectid;
                                                        if (!string.IsNullOrEmpty(transformNodeValue))
                                                        {
                                                            component.Transform = Array.ConvertAll(transformNodeValue.Split(' '), float.Parse);
                                                        }
                                                        components.Add(component);
                                                    }
                                                    objectModel3MF.Components = components;
                                                }
                                                objects3MF.Add(objectModel3MF);
                                            }
                                            model3MF.Resources.Objects3MF = objects3MF;
                                        }
                                    }
                                    var buildNode = xmlDoc.DocumentElement.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "build").FirstOrDefault();
                                    if (buildNode != null)
                                    {
                                        var itemNodes = buildNode.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "item").ToList();
                                        if (itemNodes != null && itemNodes.Count > 0)
                                        {
                                            var buildItems = new List<Models.Models3MF.BuildItem>();
                                            foreach (var itemNode in itemNodes)
                                            {
                                                int objectid = itemNode.Attributes["objectid"] != null ? Convert.ToInt32(itemNode.Attributes["objectid"].Value) : 0;
                                                string transformNodeValue = itemNode.Attributes["transform"] != null ? Convert.ToString(itemNode.Attributes["transform"].Value) : string.Empty;
                                                var buildItem = new Models.Models3MF.BuildItem();
                                                buildItem.ObjectId = objectid;
                                                if (!string.IsNullOrEmpty(transformNodeValue))
                                                {
                                                    buildItem.Transform = Array.ConvertAll(transformNodeValue.Split(' '), float.Parse);
                                                }
                                                buildItems.Add(buildItem);
                                            }

                                            model3MF.BuildItems = buildItems;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var totalTriangleInfoList = new List<TriangleInfoList>();
            //

            var emptyChildFound = true;
            while (emptyChildFound)
            {
                emptyChildFound = false;
                foreach (var buildItem in model3MF.BuildItems)
                {
                    var childObjects = model3MF.Resources.Objects3MF.Where(s => s.Id == buildItem.ObjectId);
                    foreach (var childObject in childObjects)
                    {
                        if (childObject.Mesh == null)
                        {
                            foreach (var childObjectComponent in childObject.Components)
                            {
                                var newBuildItem = new Models.Models3MF.BuildItem() { ObjectId = childObjectComponent.ObjectId, Transform = childObjectComponent.Transform };
                                if (buildItem.Transform != null)
                                {
                                    if (newBuildItem.Transform == null)
                                    {
                                        newBuildItem.Transform = buildItem.Transform;
                                    }
                                    else
                                    {
                                        newBuildItem.Transform[9] += buildItem.Transform[9];
                                        newBuildItem.Transform[10] += buildItem.Transform[10];
                                        newBuildItem.Transform[11] += buildItem.Transform[11];
                                    }
                                    
                                }
                                model3MF.BuildItems.Add(newBuildItem);
                            }

                            model3MF.BuildItems.Remove(buildItem);

                            emptyChildFound = true;
                        }
                    }

                    if (emptyChildFound)
                    {
                        break;
                    }
                }
            }
            

            foreach (var buildItem in model3MF.BuildItems)
            {
                var transform = buildItem.Transform;
                var buildObject = model3MF.Resources.Objects3MF.Where(x => x.Id == buildItem.ObjectId).FirstOrDefault();    //14
                if (buildObject != null)
                {
                    FindVerticesAndTriangles(totalTriangleInfoList, model3MF, buildItem, buildObject, transform);
                }
            }

            return totalTriangleInfoList;
        }

        private static void FindVerticesAndTriangles(List<TriangleInfoList> totalTriangleInfoList, Models.Models3MF.Model3MF model3MF, Models.Models3MF.BuildItem buildItem, Models.Models3MF.Object3MF buildObject, float[] transform)
        {
            var itemObject = buildObject;
            var objectVertices = new List<Models.Models3MF.Vertex>();

            //find built object component mesh (recursive) 2-levels

            if (buildObject.Components != null)
            {
                foreach (var component in buildObject.Components)
                {
                    var componentObjectMesh = model3MF.Resources.Objects3MF.Where(x => x.Id == component.ObjectId).FirstOrDefault();
                    if (componentObjectMesh.Mesh == null)
                    {
                        foreach (var childComponent in componentObjectMesh.Components)
                        {
                            componentObjectMesh = model3MF.Resources.Objects3MF.Where(x => x.Id == childComponent.ObjectId && x.Mesh != null).FirstOrDefault();

                            if (componentObjectMesh != null)
                            {
                                break;
                            }
                        }
                    }

                    if (componentObjectMesh != null)
                    {
                        var componentTriangles = PrepareTriangleInfoList(componentObjectMesh.Mesh.Vertices, componentObjectMesh.Mesh.Triangles);
                        if (buildItem.Transform != null)
                        {
                            //convert vertices to triangles
                            var matrixTranslation = new Vector3Class(buildItem.Transform[9], buildItem.Transform[10], buildItem.Transform[11]);
                            componentTriangles.InitialTranslation = matrixTranslation;
                        }
                        totalTriangleInfoList.Add(componentTriangles);
                    }
                }
            }
            else
            {
                var componentObjectMesh = buildObject;

                if (componentObjectMesh.Mesh != null)
                {
                    //convert vertices to triangles
                    var componentTriangles = PrepareTriangleInfoList(componentObjectMesh.Mesh.Vertices, componentObjectMesh.Mesh.Triangles);
                    var matrixTranslation = buildItem.Transform == null? new Vector3Class(): new Vector3Class(buildItem.Transform[9], buildItem.Transform[10], buildItem.Transform[11]);
                    componentTriangles.InitialTranslation = matrixTranslation;
                    totalTriangleInfoList.Add(componentTriangles);
                }
            }
        }


        private static TriangleInfoList PrepareTriangleInfoList(List<Models.Models3MF.Vertex> vertices, List<Models.Models3MF.Triangle> triangles)
        {
            TriangleInfoList triangleInfoList = new TriangleInfoList();
            ushort triangleIndex = 0;
            ushort triangleArrayIndex = 0;
            try
            {
                float x = 0;
                float y = 0;
                float z = 0;
                //var normal = new Vector3Class();
                for (int i = 0; i < triangles.Count; i++)
                {
                    if (triangleIndex > 33333)
                    {
                        triangleIndex = 0;
                        triangleArrayIndex++;
                        triangleInfoList.Add(new List<Triangle>());
                    }
                    var triangle = new Triangle();
                    triangle.Index = new TriangleConnectionInfo() { ArrayIndex = triangleArrayIndex, TriangleIndex = triangleIndex };
                    var triangleValue = triangles[i];
                    for (int vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                    { //iterate through the points       
                        var vIndex = 0;
                        switch (vectorIndex)
                        {
                            case 0:
                                vIndex = triangleValue.V1;//(int)triangleValue.GetType().GetProperty("V1").GetValue(triangleValue, null);
                                break;
                            case 1:
                                vIndex = triangleValue.V2;//(int)triangleValue.GetType().GetProperty("V2").GetValue(triangleValue, null);
                                break;
                            case 2:
                                vIndex = triangleValue.V3;//(int)triangleValue.GetType().GetProperty("V3").GetValue(triangleValue, null);
                                break;
                        }
                        var vertex = vertices.ElementAt(vIndex);// [vIndex];
                        x = vertex.X;//(float)vertex.GetType().GetProperty("X").GetValue(vertex, null);
                        y = vertex.Y;// (float)vertex.GetType().GetProperty("Y").GetValue(vertex, null);
                        z = vertex.Z;//(float)vertex.GetType().GetProperty("Z").GetValue(vertex, null);


                        triangle.Vectors[vectorIndex] = new VertexClass() { Position = new Vector3Class(x, y, z) };
                    }

                    triangle.CalcCenter();
                    triangle.CalcNormal();
                    triangle.CalcAngleZ();
                    triangle.CalcVolume();

                    triangleInfoList[triangleArrayIndex].Add(triangle);

                    triangleIndex++;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
            return triangleInfoList;
        }

        internal static void DrawObjects(Controls.OpenGL.GLControl glControl, SupportProfile supportProfile, bool thumbnailRendering = false)
        {
            var defaultSupportColor = Properties.Settings.Default.DefaultSupportColor;
            var defaultSurfaceSupportColor = Properties.Settings.Default.DefaultSurfaceSupportColor;
            var selectedModel = ObjectView.SelectedModel;
            var notInMAGSAIMode = SceneView.CurrentViewMode != SceneView.ViewMode.MagsAI && SceneView.CurrentViewMode != SceneView.ViewMode.MagsAIGridSupport && SceneView.CurrentViewMode != SceneView.ViewMode.MagsAIManualSupport;

            try
            {
                if (!MainFormManager.IsInExportMode || thumbnailRendering)
                {

                    var modelIndex = 0;
                    if (glControl.Context.IsCurrent)
                    {
                        foreach (var object3d in Objects3D)
                        {
                            if (object3d is STLModel3D)
                            {
                                if (glControl.Context != null)
                                {
                                    var stlModel = (STLModel3D)object3d;
                                    if (stlModel.Loaded)
                                    {
                                        //do not render all other models when MAGS AI is selected
                                        if ((SceneView.CurrentViewMode == SceneView.ViewMode.MagsAI || SceneView.CurrentViewMode == SceneView.ViewMode.MagsAIGridSupport || SceneView.CurrentViewMode == SceneView.ViewMode.MagsAIManualSupport)
                                            && selectedModel != stlModel
                                            )
                                        {
                                            continue;
                                        }


                                        GL.PushMatrix();

                                        if (stlModel.PreviewMoveTranslation != new Vector3Class())
                                        {
                                            GL.Translate((stlModel.PreviewMoveTranslation - new Vector3Class(0, 0, stlModel.MoveTranslationZ)).ToStruct());
                                        }
                                        else
                                        {
                                            GL.Translate(new Vector3(stlModel.MoveTranslation.X, stlModel.MoveTranslation.Y, 0));
                                        }


                                        if (stlModel.PreviewRotationX != 0)
                                        {
                                            GL.Translate(new Vector3(0, 0, stlModel.Center.Z));
                                            GL.Rotate(stlModel.PreviewRotationX, Vector3.UnitX);
                                            GL.Translate(new Vector3(0, 0, -stlModel.Center.Z));
                                        }
                                        if (stlModel.PreviewRotationY != 0)
                                        {
                                            GL.Translate(new Vector3(0, 0, stlModel.Center.Z));
                                            GL.Rotate(stlModel.PreviewRotationY, Vector3.UnitY);
                                            GL.Translate(new Vector3(0, 0, -stlModel.Center.Z));
                                        }
                                        else if (stlModel.PreviewRotationZ != 0)
                                        {
                                            GL.Rotate(stlModel.PreviewRotationZ, Vector3.UnitZ);
                                        }

                                        if (selectedModel == stlModel)
                                        {
                                            SceneView.DrawMAGSAISelectionOverlay(selectedModel);
                                        }

                                        //selected?
                                        if (SceneView.CurrentViewMode != SceneView.ViewMode.ModelRotation)
                                        {
                                            if (!thumbnailRendering && stlModel.Selected && SceneView.CurrentViewMode != SceneView.ViewMode.MagsAI)
                                            {
                                                if (stlModel.LinkedClones.Count > 0 && notInMAGSAIMode)
                                                {
                                                    foreach (var linkedClone in stlModel.LinkedClones)
                                                    {
                                                        if (linkedClone.Selected)
                                                        {
                                                            GL.PushMatrix();
                                                            GL.Translate(linkedClone.Translation.ToStruct());
                                                            if (linkedClone.Rotate)
                                                                GL.Rotate(90, Vector3d.UnitZ);

                                                            GL.Color4(Color.FromArgb(stlModel.Color.A, Color.WhiteSmoke));
                                                            GL.LineWidth(1.3f);
                                                            GL.LineStipple(6, 0xAAAA);
                                                            GL.Enable(EnableCap.LineStipple);
                                                            GL.Begin(PrimitiveType.Lines);
                                                            foreach (var point in stlModel.SelectionBox)
                                                            {
                                                                if (point != null)
                                                                {
                                                                    GL.Vertex3(point.ToStruct());
                                                                }
                                                            }
                                                            GL.End();
                                                            GL.Disable(EnableCap.LineStipple);
                                                            GL.LineWidth(1);

                                                            // Undo translation and rotation
                                                            GL.Translate(-linkedClone.Translation.ToStruct());
                                                            if (linkedClone.Rotate)
                                                                GL.Rotate(-90, Vector3d.UnitZ);
                                                            GL.PopMatrix();
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    GL.Color4(Color.FromArgb(stlModel.Color.A, Color.WhiteSmoke));
                                                    GL.LineWidth(1.3f);
                                                    GL.LineStipple(6, 0xAAAA);
                                                    GL.Enable(EnableCap.LineStipple);
                                                    GL.Begin(PrimitiveType.Lines);
                                                    foreach (var point in stlModel.SelectionBox)
                                                    {
                                                        if (point != null)
                                                        {
                                                            GL.Vertex3(point.ToStruct());
                                                        }
                                                    }
                                                    GL.End();
                                                    GL.Disable(EnableCap.LineStipple);
                                                    GL.LineWidth(1);
                                                }

                                            }
                                        }

                                        //check view settings
                                        if (SceneView.ModelRenderMode == SceneView.SceneViewRenderModeType.Wireframe)
                                        {
                                            GL.Color4(Color.FromArgb(stlModel.Color.A, Color.White));
                                            GL.PolygonMode(MaterialFace.Front, PolygonMode.Line);
                                        }
                                        else
                                        {
                                            GL.Color4(stlModel.Selected ? Color.FromArgb(stlModel.Color.A, Color.White) : stlModel.Color);
                                            GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
                                        }

                                        if (SceneView.CurrentViewMode == SceneView.ViewMode.MagsAI)
                                        {
                                            GL.Enable(EnableCap.Blend);
                                            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                                        }

                                        if (SceneView.ModelRenderMode != SceneView.SceneViewRenderModeType.Hidden)
                                        {

                                            GL.EnableClientState(ArrayCap.ColorArray);
                                            GL.EnableClientState(ArrayCap.VertexArray);
                                            GL.EnableClientState(ArrayCap.NormalArray);


                                            try
                                            {
                                                if (stlModel.VBOIndexes != null)
                                                {
                                                    if (stlModel.LinkedClones.Count > 0 && notInMAGSAIMode)
                                                    {
                                                        foreach (var linkedClone in stlModel.LinkedClones)
                                                        {
                                                            GL.PushMatrix();
                                                            GL.Translate(linkedClone.Translation.ToStruct());
                                                            if (linkedClone.Rotate)
                                                                GL.Rotate(90, Vector3d.UnitZ);

                                                            var triangleIndex = 0;
                                                            foreach (var vboIndex in stlModel.VBOIndexes)
                                                            {
                                                                GL.BindBuffer(BufferTarget.ArrayBuffer, vboIndex);
                                                                GL.VertexPointer(3, VertexPointerType.Float, Vertex.Stride, 4);
                                                                GL.NormalPointer(NormalPointerType.Float, Vertex.Stride, new IntPtr(16));
                                                                GL.ColorPointer(4, ColorPointerType.UnsignedByte, Vertex.Stride, new IntPtr(0));
                                                                GL.DrawArrays(PrimitiveType.Triangles, 0, stlModel.Triangles[triangleIndex].Count * 3);

                                                                triangleIndex++;
                                                            }

                                                            // Undo translation and rotation
                                                            //GL.Translate(-translation.ToStruct());
                                                            //if (linkedClone.Rotate)
                                                            //    GL.Rotate(-90, Vector3d.UnitZ);

                                                            GL.Translate(-linkedClone.Translation.ToStruct());
                                                            if (linkedClone.Rotate)
                                                                GL.Rotate(-90, Vector3d.UnitZ);

                                                            GL.PopMatrix();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        var triangleIndex = 0;
                                                        foreach (var vboIndex in stlModel.VBOIndexes)
                                                        {
                                                            GL.BindBuffer(BufferTarget.ArrayBuffer, vboIndex);
                                                            GL.ColorPointer(4, ColorPointerType.UnsignedByte, Vertex.Stride, new IntPtr(0));
                                                            GL.VertexPointer(3, VertexPointerType.Float, Vertex.Stride, 4);
                                                            GL.NormalPointer(NormalPointerType.Float, Vertex.Stride, new IntPtr(16));
                                                            GL.DrawArrays(PrimitiveType.Triangles, 0, stlModel.Triangles[triangleIndex].Count * 3);

                                                            triangleIndex++;
                                                        }
                                                    }

                                                }
                                            }
                                            catch (Exception exc)
                                            {
                                                Debug.WriteLine(exc.Message);
                                            }

                                            GL.DisableClientState(ArrayCap.ColorArray);
                                            GL.DisableClientState(ArrayCap.VertexArray);
                                            GL.DisableClientState(ArrayCap.NormalArray);
                                        }

                                        if (stlModel.PreviewMoveTranslation.ToStruct() != new Vector3())
                                        {
                                            GL.Translate(-stlModel.PreviewMoveTranslation.ToStruct());
                                        }
                                        else
                                        {
                                            GL.Translate(-stlModel.MoveTranslation.ToStruct());
                                        }

                                        GL.Disable(EnableCap.Blend);
                                        GL.PopMatrix();


                                        if (SceneView.SupportRenderMode != SceneView.SceneViewRenderModeType.Hidden)
                                        {
                                            if (!stlModel.DisableSupportDrawing)
                                            {
                                                if (SceneView.SupportRenderMode == SceneView.SceneViewRenderModeType.Solid)
                                                {
                                                    GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
                                                }
                                                else
                                                {
                                                    GL.PolygonMode(MaterialFace.Front, PolygonMode.Line);
                                                }

                                                if (stlModel.LinkedClones.Count > 0 && notInMAGSAIMode)
                                                {
                                                    foreach (var linkedClone in stlModel.LinkedClones)
                                                    {
                                                        GL.PushMatrix();
                                                        GL.Translate(linkedClone.Translation.ToStruct());
                                                        if (linkedClone.Rotate)
                                                            GL.Rotate(90, Vector3d.UnitZ);

                                                        DrawSupportStructures(stlModel, thumbnailRendering, defaultSupportColor, defaultSurfaceSupportColor, supportProfile);
                                                        //Console.WriteLine("Draw Objects 2: " + stopwatch.ElapsedMilliseconds + "ms");

                                                        // Undo translation and rotation
                                                        GL.Translate(-linkedClone.Translation.ToStruct());
                                                        if (linkedClone.Rotate)
                                                            GL.Rotate(-90, Vector3d.UnitZ);
                                                        GL.PopMatrix();
                                                    }
                                                }
                                                else
                                                {
                                                    GL.PushMatrix();
                                                    DrawSupportStructures(stlModel, thumbnailRendering, defaultSupportColor, defaultSurfaceSupportColor, supportProfile);
                                                    GL.PopMatrix();
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            modelIndex++;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                DAL.Managers.LoggingManager.WriteToLog("XX", "exc", exc);
            }
        }

        static void DrawSupportStructures(STLModel3D stlModel, bool thumbnailRendering, Color defaultSupportColor, Color defaultSurfaceSupportFaceColor, SupportProfile supportProfile)
        {
            var selectedSupportConeColor = BrandingManager.Button_HighlightColor;
            var modelIntersectedModelColor = Color.PaleVioletRed;

            try
            {
                var modelMoveTranslation = new Vector3(stlModel.MoveTranslationX, stlModel.MoveTranslationY, 0);

                if (stlModel.PreviewMoveTranslation != new Vector3Class())
                {
                    GL.Translate((stlModel.PreviewMoveTranslation - new Vector3Class(0, 0, stlModel.MoveTranslationZ)).ToStruct());
                }
                else
                {
                    GL.Translate(new Vector3(stlModel.MoveTranslation.X, stlModel.MoveTranslation.Y, 0));
                }

                GL.Rotate(stlModel.PreviewRotationZ, Vector3.UnitZ);

                //support basement
                if ((SceneView.CameraPosition.Z > 0 || thumbnailRendering) && stlModel.SupportBasementStructure != null)
                {
                    GL.Begin(PrimitiveType.Triangles);
                    GL.Color4(Color.SteelBlue);
                    if (stlModel.SupportBasementStructure != null)
                    {
                        foreach (var triangle in stlModel.SupportBasementStructure.Triangles[0])
                        {
                            GL.Normal3(triangle.Normal.ToStruct());
                            GL.Vertex3(triangle.Vectors[0].Position.ToStruct());
                            GL.Vertex3(triangle.Vectors[1].Position.ToStruct());
                            GL.Vertex3(triangle.Vectors[2].Position.ToStruct());
                        }
                    }

                    GL.End();
                }


                try
                {

                    SupportCone selectedSupportCone = null;
                    if (stlModel.SupportStructure != null)
                    {
                        foreach (var supportCone in stlModel.SupportStructure)
                        {
                            if (supportCone != null)
                            {
                                supportCone.Draw(supportCone._color, selectedSupportConeColor, modelIntersectedModelColor, !thumbnailRendering, false);

                                if (supportCone.Selected)
                                {
                                    selectedSupportCone = supportCone;
                                }
                            }
                        }
                    }

                    if (stlModel.SupportHelperStructure != null)
                    {
                        foreach (var supportCone in stlModel.SupportHelperStructure)
                        {
                            if (supportCone != null)
                            {
                                supportCone.Draw(Color.Beige, selectedSupportConeColor, modelIntersectedModelColor, !thumbnailRendering, false);

                                if (supportCone.Selected)
                                {
                                    selectedSupportCone = supportCone;
                                }
                            }
                        }
                    }


                    //#if DEBUG
                    //                    if (stlModel.SupportStructure != null)
                    //                    {

                    //                        foreach (var supportCone in stlModel.SupportStructure)
                    //                        {
                    //                            if (supportCone != null)
                    //                            {
                    //                                supportCone.DrawDebugTopPointMarker(supportProfile);
                    //                            }
                    //                        }

                    //                    }
                    //#endif

                }
                catch
                {
                    GL.End();
                }

                try
                {
                    GL.Begin(PrimitiveType.Triangles);
                    if (stlModel.SupportHelperStructure != null)
                    {
                        foreach (var supportCone in stlModel.SupportHelperStructure)
                        {
                            if (supportCone != null)
                            {
                                supportCone.Draw(supportCone._color, selectedSupportConeColor, modelIntersectedModelColor, !thumbnailRendering, false);
                            }
                        }
                    }

                    GL.End();
                }
                catch
                {
                    GL.End();
                }

                //horizontal surface
                //surface support
                try
                {
                    GL.Begin(PrimitiveType.Triangles);
                    foreach (var horizontalSurface in stlModel.Triangles.HorizontalSurfaces)
                    {
                        var hasSelectedSupportCone = horizontalSurface.SupportStructure.Any(s => s.Selected);
                        foreach (var supportCone in horizontalSurface.SupportStructure)
                        {
                            if (supportCone != null)
                            {
                                if (hasSelectedSupportCone)
                                {
                                    supportCone.Draw(defaultSurfaceSupportFaceColor, selectedSupportConeColor, modelIntersectedModelColor, !thumbnailRendering, true);
                                }
                                else
                                {
                                    supportCone.Draw(defaultSupportColor, selectedSupportConeColor, modelIntersectedModelColor, !thumbnailRendering, false);
                                }
                            }
                        }
                    }

                    GL.End();
                }
                catch
                {
                    GL.End();
                }


                //flat surface
                //surface support
                try
                {

                    GL.Begin(PrimitiveType.Triangles);
                    foreach (var flatSurface in stlModel.Triangles.FlatSurfaces)
                    {
                        var hasSelectedSupportCone = flatSurface.SupportStructure.Any(s => s.Selected);
                        foreach (var supportCone in flatSurface.SupportStructure)
                        {
                            if (supportCone != null)
                            {
                                if (hasSelectedSupportCone)
                                {
                                    supportCone.Draw(defaultSurfaceSupportFaceColor, selectedSupportConeColor, modelIntersectedModelColor, !thumbnailRendering, true);
                                }
                                else
                                {
                                    supportCone.Draw(defaultSupportColor, selectedSupportConeColor, modelIntersectedModelColor, !thumbnailRendering, false);
                                }
                            }
                        }
                    }

                    GL.End();

                    if (RegistryManager.RegistryProfile.DebugMode)
                    {
                        foreach (var flatSurface in stlModel.Triangles.FlatSurfaces)
                        {
                            foreach (var supportCone in flatSurface.SupportStructure)
                            {
                                if (supportCone != null)
                                {
                                    supportCone.DrawDebugTopPointMarker(supportProfile);
                                }
                            }
                        }
                    }

                    if (MagsAIEngineFilters.DebugVectors != null)
                    {

                        GL.Begin(PrimitiveType.Lines);
                        GL.Color3(Color.White);
                        foreach (var debugVectors in MagsAIEngineFilters.DebugVectors)
                        {
                            if (debugVectors != null)
                            {
                                GL.Vertex3(debugVectors[0].ToStruct());
                                GL.Vertex3((debugVectors[0] + debugVectors[1]).ToStruct());
                            }
                        }

                        GL.End();
                    }


                    //if (selectedFlatSurface != null)
                    //{
                    //    //draw selected horizontal surface grid
                    //    if (SceneView.CurrentViewMode == SceneView.ViewMode.MagsAIGridSupport || SceneView.CurrentViewMode == SceneView.ViewMode.LayFlat)
                    //    {
                    //        GL.Begin(PrimitiveType.Triangles);
                    //        GL.Normal3(new Vector3(0, 0, -1));
                    //        GL.Color4(Color.FromArgb(stlModel.Index + 100, Properties.Settings.Default.DefaultSurfaceSupportFaceSelectedColor));

                    //        foreach (var flatSurfaceIndex in selectedFlatSurface.Keys)
                    //        {
                    //            GL.Normal3(stlModel.Triangles[flatSurfaceIndex.ArrayIndex][flatSurfaceIndex.TriangleIndex].Normal.ToStruct());
                    //            foreach (var vector in stlModel.Triangles[flatSurfaceIndex.ArrayIndex][flatSurfaceIndex.TriangleIndex].Vectors)
                    //            {
                    //                GL.Vertex3((vector.Position - new Vector3Class(0, 0, 0.01f)).ToStruct());
                    //            }
                    //        }

                    //        GL.End();
                    //    }

                    //    //GL.Begin(PrimitiveType.Lines);
                    //    //    GL.Color4(Color.Blue);
                    //    //    foreach (var vertex in selectedFlatSurface.SelectionBox)
                    //    //    {
                    //    //        GL.Vertex3(vertex);
                    //    //    }

                    //    //GL.End();
                    //}
                }
                catch
                {
                    GL.End();
                }



                GL.Translate(new Vector3(-(modelMoveTranslation.X), -(modelMoveTranslation.Y), 0));

            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
        }

        internal static void DeselectModels()
        {
            //deselect all models
            foreach (var model in Objects3D)
            {
                if (model is STLModel3D && !(model is GroundPane))
                {
                    var stlModel = model as STLModel3D;
                    stlModel.Selected = false;
                }
            }
        }

        internal static void DrawNewSupportConeGhost()
        {
            var selectedTriangle = frmStudioMain.SceneControl.SelectedTriangle;
            if (SceneView.CurrentViewMode == SceneView.ViewMode.MagsAIManualSupport && selectedTriangle != null)
            {
                Console.WriteLine(selectedTriangle.Index.ArrayIndex + ":" + selectedTriangle.Index.TriangleIndex);

                //ghost support cone
                GL.PushMatrix();
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One);
                GL.Enable(EnableCap.Blend);
                GL.Color4(0, 0.2, 1, 0.9);
                GL.Begin(PrimitiveType.Triangles);
                var stlModelMoveTransation = new Vector3Class();
                if (ObjectView.SelectedModel is STLModel3D)
                {

                    var stlModel = ObjectView.SelectedModel;
                    stlModelMoveTransation = new Vector3Class(stlModel.MoveTranslation.X, stlModel.MoveTranslation.Y, 0);
                    var defaultSupportCatalogItem = PrintJobManager.SelectedMaterialSummary.Material.SupportProfiles.First();
                    var supportCone = new SupportCone(selectedTriangle.IntersectionPoint.Z,
                defaultSupportCatalogItem.SupportTopRadius,
                0,
                defaultSupportCatalogItem.SupportMiddleRadius,
                selectedTriangle.IntersectionPoint.Z,
                defaultSupportCatalogItem.SupportBottomRadius,
                        16, new Vector3Class(selectedTriangle.IntersectionPoint.X, selectedTriangle.IntersectionPoint.Y, 0) + stlModelMoveTransation,
                        Color.FromArgb(stlModel.Color.A + 100,
                        Properties.Settings.Default.DefaultSupportColor),
                        bottomWidthCorrection: 0f
                        );

                    foreach (var triange in supportCone.Triangles[0])
                    {
                        GL.Normal3(triange.Normal.ToStruct());
                        foreach (var point in triange.Points)
                        {
                            GL.Vertex3(point.ToStruct());
                        }
                    }

                }
                GL.End();
                GL.Disable(EnableCap.Blend);
                GL.PopMatrix();

            }
        }
    }
}

