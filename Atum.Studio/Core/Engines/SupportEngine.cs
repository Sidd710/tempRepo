using Atum.Studio.Core.Models;
using OpenTK;
using System;
using System.Collections.Generic;
using Atum.Studio.Core.Events;
using Atum.Studio.Core.Shapes;
using System.Drawing;
using Atum.Studio.Core.Managers;
using System.Threading;
using Atum.Studio.Core.Utils;
using System.Diagnostics;
using static Atum.Studio.Core.Models.SupportCone;
using System.Windows.Forms;
using Atum.DAL.Materials;
using Atum.Studio.Core.Managers.UndoRedo;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Structs;
using System.Linq;

namespace Atum.Studio.Core.Engines
{
    internal class SupportEngine
    {
        internal static Action<STLModel3D> GridSupportAdded;
        internal static Action<STLModel3D> ManualSupportConeAdded;

        internal static SupportCone AddMagsAISupportCone(STLModel3D stlModel, TriangleIntersection modelIntersection, Color supportConeColor, Material selectedMaterial, STLModel3D trianglesWithinXYRange, float lastLayerSupportedHeight)
        {
            SupportCone supportCone = new SupportCone();
            //}

            // SupportCone supportCone = null;
            var minSupportHeight = 0.1f;
            var defaultSupportCatalogItem = selectedMaterial.SupportProfiles[0];
            var supportConeTopRadius = defaultSupportCatalogItem.SupportTopRadius;
            var supportConeTopHeight = defaultSupportCatalogItem.SupportTopHeight;
            var supportConeMiddleRadius = defaultSupportCatalogItem.SupportMiddleRadius;
            var supportConeBottomHeight = defaultSupportCatalogItem.SupportBottomHeight;
            var supportConeBottomRadius = defaultSupportCatalogItem.SupportBottomRadius;
            var supportConeBottomWidthCorrection = defaultSupportCatalogItem.SupportBottomWidthCorrection;

            try
            {
                if (modelIntersection != null)
                {
                    var triangleIntersectedBelowTriangle = false;
                    var bottomSupportPoint = new Vector3Class(modelIntersection.IntersectionPoint.X, modelIntersection.IntersectionPoint.Y, 0);

                    TriangleIntersection[] trianglesIntersected = null;

                    var triangleInRangeBelow = new STLModel3D() { Triangles = new TriangleInfoList() };

                    triangleInRangeBelow.Triangles[0] = trianglesWithinXYRange.Triangles[0].Where(s => s.Bottom < modelIntersection.IntersectionPoint.Z && s.Normal.Z > 0).ToList();
                    IntersectionProvider.IntersectTriangle(new Vector3Class(modelIntersection.IntersectionPoint.X, modelIntersection.IntersectionPoint.Y, 1000), new Vector3Class(0, 0, -1), triangleInRangeBelow, IntersectionProvider.typeDirection.OneWay, false, new Vector3Class(), out trianglesIntersected);

                    var nearestPointBelow = bottomSupportPoint;
                    var nearestDistanceBelow = float.MaxValue;
                    if (trianglesIntersected != null)
                    {
                        foreach (var triangleIntersected in trianglesIntersected)
                        {
                            if (triangleIntersected != null)
                            {
                                var intersectionPoint = new Vector3(triangleIntersected.IntersectionPoint.X, triangleIntersected.IntersectionPoint.Y, triangleIntersected.IntersectionPoint.Z);

                                var distanceBelow = modelIntersection.IntersectionPoint.Z - triangleIntersected.IntersectionPoint.Z;
                                if (distanceBelow < nearestDistanceBelow)
                                {
                                    nearestPointBelow = triangleIntersected.IntersectionPoint;
                                    nearestDistanceBelow = distanceBelow;
                                    triangleIntersectedBelowTriangle = true;
                                }
                            }
                        }
                    }

                    if (triangleIntersectedBelowTriangle)
                    {
                        var supportHeight = modelIntersection.IntersectionPoint.Z - nearestPointBelow.Z;
                        if (supportHeight > minSupportHeight)
                        {
                            supportCone = new SupportCone(supportHeight,
                            supportConeTopHeight,
                            supportConeTopRadius,
                            supportConeMiddleRadius,
                            supportConeTopHeight,
                            supportConeTopRadius,
                            16, new Vector3Class(modelIntersection.IntersectionPoint.X, modelIntersection.IntersectionPoint.Y, nearestPointBelow.Z), Color.FromArgb(stlModel.Color.A + 100, supportConeColor),
                            model: stlModel, groundSupport: false, useSupportPenetration: true,
                            supportConeType: TypeSupportCone.Normal,
                            bottomWidthCorrection: supportConeBottomWidthCorrection);
                        }
                    }
                    else if (!triangleIntersectedBelowTriangle)
                    {
                        var supportHeight = modelIntersection.IntersectionPoint.Z;
                        //if (stlModel.SupportBasement) supportHeight -= UserProfileManager.UserProfile.SupportEngine_Basement_Thickness;
                        if (supportHeight > minSupportHeight)
                        {
                            //if (FeatureManager.EnableExtendedSupportConeV2)
                            //{
                            //    if (defaultSupportCatalogItem.ASegmentRadius == 0 && defaultSupportCatalogItem.BSegmentRadius == 0 && defaultSupportCatalogItem.CSegmentRadius == 0)
                            //    {
                            //        selectedMaterial.SupportProfiles.Clear();
                            //        selectedMaterial.SupportProfiles.Add(SupportProfile.CreateDefault());
                            //        defaultSupportCatalogItem = selectedMaterial.SupportProfiles.First();
                            //    }

                            //    var ARadius = defaultSupportCatalogItem.ASegmentRadius; //1f;
                            //    var AHeight = defaultSupportCatalogItem.ASegmentHeight; //1f;
                            //    var BRadius = defaultSupportCatalogItem.BSegmentRadius; //0.35f;
                            //    var BHeight = defaultSupportCatalogItem.BSegmentHeight; //1.5f;
                            //    var CRadius = defaultSupportCatalogItem.CSegmentRadius; // 1.5f;
                            //    var DRadius = defaultSupportCatalogItem.DSegmentRadius; // 0.5f;
                            //    var DHeight = defaultSupportCatalogItem.DSegmentHeight; // 1.5f;

                            //    supportCone = new SupportConeV2(
                            //        supportConeTopHeight,
                            //        supportConeTopRadius,
                            //    supportConeMiddleRadius,
                            //    supportConeBottomHeight,
                            //    supportConeBottomRadius,
                            //16,
                            //modelIntersection,
                            // Color.FromArgb(stlModel.Color.A + 100, supportConeColor),
                            //stlModel,
                            //true,
                            //false,
                            //trianglesWithinXYRange,
                            //TypeSupportCone.Normal,
                            //supportConeBottomWidthCorrection,
                            //aSegmentRadius: ARadius,
                            //aSegmentHeight: AHeight,
                            //bSegmentRadius: BRadius,
                            //bSegmentHeight: BHeight,
                            //cSegmentRadius: CRadius,
                            //dSegmentRadius: DRadius,
                            //dSegmentHeight: DHeight,
                            //calcSliceContours: true);
                            //}
                            //else
                            //{
                            supportCone = new SupportCone(supportHeight,
                        supportConeTopHeight,
                            supportConeTopRadius,
                            supportConeMiddleRadius,
                            supportConeBottomHeight,
                            supportConeBottomRadius,
                        16, new Vector3Class(modelIntersection.IntersectionPoint.X, modelIntersection.IntersectionPoint.Y, stlModel.SupportBasement ? Managers.UserProfileManager.UserProfile.SupportEngine_Basement_Thickness : 0), Color.FromArgb(stlModel.Color.A + 100, supportConeColor),
                        useSupportPenetration: true,
                        model: stlModel,
                        trianglesXYInRange: trianglesWithinXYRange,
                        supportConeType: TypeSupportCone.Normal,
                        bottomWidthCorrection: supportConeBottomWidthCorrection);
                            //}
                        }
                    }



                    if (supportCone != null)
                    {
                        supportCone.UpdateBoundries();
                        supportCone._color = Color.FromArgb(Convert.ToInt16(stlModel.Color.A) + 100, supportConeColor);
                        supportCone.CreationBy = SupportCone.typeCreationBy.Auto;
                        if (lastLayerSupportedHeight != float.MinValue)
                        {
                            supportCone.LastSupportedLayerHeight = lastLayerSupportedHeight;
                        }

                        lock (stlModel.SupportStructure)
                        {
                            stlModel.SupportStructure.Add(supportCone);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
            return supportCone;
        }

        internal static SupportCone AddManualSupport(STLModel3D stlModel, TriangleIntersection modelIntersection, Material selectedMaterial, bool addToStructure = true)
        {
            var supportCone = AddManualSupport(stlModel, modelIntersection, Properties.Settings.Default.DefaultSupportColor, selectedMaterial, addToStructure);
            if (UserProfileManager.UserProfile.Settings_Use_Support_Basement)
            {
                stlModel.SupportBasement = false;
                stlModel.SupportBasement = true;
            }
            return supportCone;
        }

        internal static SupportCone AddManualSupport(STLModel3D stlModel, TriangleIntersection modelIntersection, Color supportConeColor, Material selectedMaterial, bool addToStructure = true, float lastLayerSupportedHeight = float.MinValue)
        {
            //minSupportHeight is lowest layer heigt
            // topPosition -= new Vector3(this.MoveTranslationX, this.MoveTranslationY, 0);
            if (selectedMaterial != null)
            {
                var minSupportHeight = selectedMaterial.LT2;

                if (selectedMaterial.LT2 > selectedMaterial.LT1 && selectedMaterial.InitialLayers > 0)
                {
                    minSupportHeight = selectedMaterial.LT1;
                }

                //remove all previous selection
                //foreach (var supportCone in stlModel.TotalObjectSupportCones)
                //{
                //    supportCone.Selected = false;
                //}

                if (stlModel.Triangles.HorizontalSurfaces != null)
                {
                    foreach (var surface in stlModel.Triangles.HorizontalSurfaces)
                    {
                        surface.Selected = false;
                    }
                }

                if (stlModel.Triangles.FlatSurfaces != null)
                {
                    foreach (var surface in stlModel.Triangles.FlatSurfaces)
                    {
                        surface.Selected = false;
                    }
                }

                var stlModelClone = stlModel;
                var createdSupportConeTask = System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    return CreateSupportConeThread(modelIntersection, selectedMaterial, SupportCone.typeCreationBy.Manual, 0, stlModelClone, supportConeColor, addToStructure: addToStructure,
 selectSupportConeAfterCreation: false, lastLayerSupportedHeight: lastLayerSupportedHeight, calcSliceContours: false);
                });

                if (createdSupportConeTask.Result != null)
                {
                    var createdSupportCone = createdSupportConeTask.Result;

                    return createdSupportCone;
                }
            }

            return null;
        }

        internal static void AddManualSurfaceSupport(STLModel3D stlModel, TriangleIntersection modelIntersection, Color supportConeColor, Material selectedMaterial, TriangleSurfaceInfo surface, float lastLayerSupportedHeight = float.MinValue)
        {
            //minSupportHeight is lowest layer heigt
            // topPosition -= new Vector3(this.MoveTranslationX, this.MoveTranslationY, 0);
            if (selectedMaterial != null)
            {
                var minSupportHeight = selectedMaterial.LT2;

                if (selectedMaterial.LT2 > selectedMaterial.LT1 && selectedMaterial.InitialLayers > 0)
                {
                    minSupportHeight = selectedMaterial.LT1;
                }



                var stlModelClone = stlModel;
                var supportPointEvents = new List<ManualResetEvent>();
                var supportPointEvent = new ManualResetEvent(false);
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    CreateSupportConeThread(modelIntersection, selectedMaterial, SupportCone.typeCreationBy.Manual, 0, stlModelClone, supportConeColor, surfaceTriangles: surface.AsStlModel(stlModel),
    selectSupportConeAfterCreation: false, lastLayerSupportedHeight: lastLayerSupportedHeight);
                    supportPointEvent.Set();
                });
                supportPointEvents.Add(supportPointEvent);
                Helpers.ThreadHelper.WaitForAll(supportPointEvents.ToArray());

                if (UserProfileManager.UserProfile.Settings_Use_Support_Basement)
                {
                    stlModel.SupportBasement = false;
                    stlModel.SupportBasement = true;
                }
            }



            //ManualSupportConeAdded?.Invoke(stlModel);
        }

        internal static void UpdateSurfaceSupport(TriangleSurfaceInfo surface, STLModel3D stlModel, Material selectedMaterial)
        {
            CreateSurfaceSupport(surface, stlModel, selectedMaterial);
        }

        internal static void CreateSurfaceSupport(TriangleSurfaceInfo surface, STLModel3D stlModel, DAL.Materials.Material selectedMaterial, bool skipEdgeCheck = false)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            if (selectedMaterial != null)
            {
                if (surface == null)
                {
                    stlModel.Triangles.CalcHorizontalSurfaceSupportPoints(stlModel._supportDistance, stlModel._crossSupport, stlModel, selectedMaterial, skipEdgeCheck);
                }
                else
                {
                    surface.SupportPoints.AddRange(stlModel.Triangles.CalcHorizontalSurfaceSupportPoints(surface, surface.SupportDistance, surface.CrossSupport, true).Keys);
                }

                //minSupportHeight is lowest layer heigt
                var minSupportHeight = selectedMaterial.LT2;
                if (selectedMaterial.LT2 > selectedMaterial.LT1 && selectedMaterial.InitialLayers > 0)
                {
                    minSupportHeight = selectedMaterial.LT1;
                }
                var supportPointEvents = new List<ManualResetEvent>();
                if (surface == null)
                {
                    //add support cone to all surfaces
                    foreach (var hsurface in stlModel.Triangles.HorizontalSurfaces)
                    {
                        var surfaceAsSTL = new STLModel3D();
                        surfaceAsSTL.Triangles = new TriangleInfoList();
                        foreach (var surfaceArrayTriangleIndex in hsurface.Keys)
                        {
                            surfaceAsSTL.Triangles[0].Add(stlModel.Triangles[surfaceArrayTriangleIndex.ArrayIndex][surfaceArrayTriangleIndex.TriangleIndex]);
                        }

                        hsurface.SupportStructure.Clear();
                        foreach (var supportPoint in hsurface.SupportPoints)
                        {
                            var supportPointClone = supportPoint;
                            var supportPointEvent = new ManualResetEvent(false);
                            ThreadPool.QueueUserWorkItem(arg => { SupportEngine.CreateSurfaceSupportConeThread(hsurface, selectedMaterial, surfaceAsSTL, supportPointClone, SupportCone.typeCreationBy.Auto, minSupportHeight, stlModel, true); supportPointEvent.Set(); });
                            supportPointEvents.Add(supportPointEvent);
                        }
                    }
                }
                else if (surface.SupportPoints.Count > 0)
                {
                    //add support cone to selected surface
                    var surfaceAsSTL = new STLModel3D();
                    surfaceAsSTL.Triangles = new TriangleInfoList();
                    foreach (var surfaceArrayTriangleIndex in surface.Keys)
                    {
                        surfaceAsSTL.Triangles[0].Add(stlModel.Triangles[surfaceArrayTriangleIndex.ArrayIndex][surfaceArrayTriangleIndex.TriangleIndex]);
                    }
                    surface.SupportStructure.Clear();
                    foreach (var supportPoint in surface.SupportPoints)
                    {
                        var supportPointClone = supportPoint;
                        var supportPointEvent = new ManualResetEvent(false);
                        ThreadPool.QueueUserWorkItem(arg => { SupportEngine.CreateSurfaceSupportConeThread(surface, selectedMaterial, surfaceAsSTL, supportPointClone, SupportCone.typeCreationBy.Auto, minSupportHeight, stlModel, false); supportPointEvent.Set(); });
                        supportPointEvents.Add(supportPointEvent);
                    }
                }
                Helpers.ThreadHelper.WaitForAll(supportPointEvents.ToArray());

                Console.WriteLine("Surface Support: " + stopwatch.ElapsedMilliseconds + "ms");

                stlModel.Triangles.HorizontalSurfaces.UpdateBoundries(stlModel.Triangles);
                stlModel.Triangles.FlatSurfaces.UpdateBoundries(stlModel.Triangles);

                if (surface != null)
                {

                    for (var supportConeIndex = surface.SupportStructure.Count - 1; supportConeIndex > 0; supportConeIndex--)
                    {
                        var supportCone = surface.SupportStructure[supportConeIndex];
                        if (supportCone.InvalidPenetrationPoints)
                        {
                            surface.SupportStructure.RemoveAt(supportConeIndex);
                        }
                    }
                }

                if (UserProfileManager.UserProfile.Settings_Use_Support_Basement)
                {
                    stlModel.SupportBasement = false;
                    stlModel.SupportBasement = true;
                }

                GridSupportAdded?.Invoke(stlModel);
            }

        }

        internal static void CreateSurfaceSupportConeThread(TriangleSurfaceInfo surface, Material selectedMaterial, STLModel3D surfaceTriangles, TriangleIntersection modelIntersection, SupportCone.typeCreationBy supportType, double minSupportHeight, STLModel3D stlModel, bool addToStructure = true)
        {
            if (selectedMaterial != null)
            {
                var supportCone = CreateSupportConeThread(modelIntersection, selectedMaterial, supportType, minSupportHeight, stlModel, Properties.Settings.Default.DefaultSurfaceSupportColor, false, surfaceTriangles);
                if (supportCone != null)
                {
                    if (supportCone.TotalHeight > minSupportHeight)
                    {
                        //get surface by supportpoint
                        if (surface != null)
                        {
                            lock (surface.SupportStructure)
                            {
                                if (supportCone.Triangles != null)
                                {
                                    surface.SupportStructure.Add(supportCone);
                                }
                            }
                        }
                    }
                }
            }
        }

        internal static SupportCone CreateSupportConeThread(TriangleIntersection modelIntersection, Material selectedMaterial, SupportCone.typeCreationBy supportType, double minSupportHeight, STLModel3D stlModel, Color supportConeColor, bool addToStructure = true, STLModel3D surfaceTriangles = null, bool selectSupportConeAfterCreation = false, bool findIntersectionPointBelow = false, List<SupportCone> targetSupportStructure = null, TypeSupportCone supportConeType = TypeSupportCone.Normal,
            float newSupportConeTopRadius = -1f, float newSupportConeTopHeight = -1f, float newSupportConeMiddleRadius = -1f, float newSupportConeBottomHeight = -1f, float newSupportConeBottomRadius = -1f, float lastLayerSupportedHeight = float.MinValue, Tuple<string, float> extendedProperties = null, bool calcSliceContours = true)
        {
            if (modelIntersection != null)
            {
                if (modelIntersection.Normal.Z < 0)
                {
                    if (selectedMaterial != null)
                    {
                        var defaultSupportCatalogItem = selectedMaterial.SupportProfiles[0];
                        var supportConeTopRadius = defaultSupportCatalogItem.SupportTopRadius;
                        var supportConeTopHeight = defaultSupportCatalogItem.SupportTopHeight;
                        var supportConeMiddleRadius = defaultSupportCatalogItem.SupportMiddleRadius;
                        var supportConeBottomHeight = defaultSupportCatalogItem.SupportBottomHeight;
                        var supportConeBottomRadius = defaultSupportCatalogItem.SupportBottomRadius;
                        var supportConeBottomWidthCorrection = defaultSupportCatalogItem.SupportBottomWidthCorrection;

                        if (newSupportConeBottomHeight != -1f) supportConeBottomHeight = newSupportConeBottomHeight;
                        if (newSupportConeBottomRadius != -1f) supportConeBottomRadius = newSupportConeBottomRadius;
                        if (newSupportConeMiddleRadius != -1f) supportConeMiddleRadius = newSupportConeMiddleRadius;
                        if (newSupportConeTopHeight != -1f) supportConeTopHeight = newSupportConeTopHeight;
                        if (newSupportConeTopRadius != -1f) supportConeTopRadius = newSupportConeTopRadius;


                        try
                        {
                            var triangleIntersectedBelowTriangle = false;
                            var nearestPointBelow = new Vector3Class();
                            TriangleIntersection[] trianglesIntersected = null;

                            var bottomSupportPoint = new Vector3Class(modelIntersection.IntersectionPoint.X, modelIntersection.IntersectionPoint.Y, 0);
                            if (findIntersectionPointBelow)
                            {
                                TriangleIntersection[] belowIntersectionPoints = null;
                                if (surfaceTriangles != null)
                                {
                                    IntersectionProvider.IntersectTriangle(bottomSupportPoint, new Vector3Class(0, 0, 1), surfaceTriangles, IntersectionProvider.typeDirection.OneWay, false, new Vector3Class(), out belowIntersectionPoints);
                                }
                                else
                                {
                                    IntersectionProvider.IntersectTriangle(bottomSupportPoint, new Vector3Class(0, 0, 1), stlModel, IntersectionProvider.typeDirection.OneWay, false, new Vector3Class(), out belowIntersectionPoints);
                                }
                                var intersectionPointFound = false;
                                var supportDistance = 5000f;
                                var nearestIntersectionPoint = new Vector3Class();
                                if (belowIntersectionPoints != null && belowIntersectionPoints.Length >= 1 && belowIntersectionPoints.Length > 0)
                                {
                                    foreach (var belowIntersectionPoint in belowIntersectionPoints)
                                    {
                                        if (belowIntersectionPoint.IntersectionPoint != new Vector3Class())
                                        {
                                            if (belowIntersectionPoint.IntersectionPoint.Z >= 0 && belowIntersectionPoint.IntersectionPoint.Z <= modelIntersection.IntersectionPoint.Z)
                                            {
                                                intersectionPointFound = true;

                                                var supportOffset = (new Vector3(0, 0, belowIntersectionPoint.IntersectionPoint.Z)).Length;
                                                if (supportOffset < supportDistance && belowIntersectionPoint.IntersectionPoint.Z <= modelIntersection.IntersectionPoint.Z)
                                                {
                                                    supportDistance = supportOffset;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (intersectionPointFound)
                                {
                                    modelIntersection.IntersectionPoint = nearestIntersectionPoint;
                                }
                            }

                            if (surfaceTriangles != null)
                            {
                                var trianglesBelow = new List<Triangle>();
                                for (var arrayIndex = 0; arrayIndex < stlModel.Triangles.Count; arrayIndex++)
                                {
                                    for (var triangleIndex = 0; triangleIndex < stlModel.Triangles[arrayIndex].Count; triangleIndex++)
                                    {
                                        if (stlModel.Triangles[arrayIndex][triangleIndex].Top < modelIntersection.IntersectionPoint.Z && stlModel.Triangles[arrayIndex][triangleIndex].HPointInside(modelIntersection.IntersectionPoint.X, modelIntersection.IntersectionPoint.Y))
                                        {
                                            trianglesBelow.Add(stlModel.Triangles[arrayIndex][triangleIndex]);
                                        }
                                    }
                                }

                                if (trianglesBelow.Count > 0)
                                {
                                    var trianglesBelowAsStl = new STLModel3D();
                                    trianglesBelowAsStl.Triangles = new TriangleInfoList();
                                    trianglesBelowAsStl.Triangles[0].AddRange(trianglesBelow);
                                    IntersectionProvider.IntersectTriangle(new Vector3Class(modelIntersection.IntersectionPoint.X, modelIntersection.IntersectionPoint.Y, modelIntersection.IntersectionPoint.Z), new Vector3Class(0, 0, -1), trianglesBelowAsStl, IntersectionProvider.typeDirection.OneWay, false, new Vector3Class(), out trianglesIntersected);
                                }
                            }
                            else
                            {
                                IntersectionProvider.IntersectTriangle(new Vector3Class(modelIntersection.IntersectionPoint.X, modelIntersection.IntersectionPoint.Y, 1000), new Vector3Class(0, 0, -1), stlModel, IntersectionProvider.typeDirection.OneWay, false, new Vector3Class(), out trianglesIntersected);
                            }


                            if (trianglesIntersected != null && trianglesIntersected.Length > 0)
                            {
                                foreach (var triangleIntersected in trianglesIntersected)
                                {
                                    if (triangleIntersected != null)
                                    {
                                        var intersectionPoint = new Vector3(triangleIntersected.IntersectionPoint.X, triangleIntersected.IntersectionPoint.Y, triangleIntersected.IntersectionPoint.Z);

                                        if (triangleIntersected.IntersectionPoint.Z > 0)
                                        {
                                            if (triangleIntersected.IntersectionPoint.Z != 0 && intersectionPoint.Z < modelIntersection.IntersectionPoint.Z)
                                            {
                                                if (nearestPointBelow != new Vector3Class() && triangleIntersected.IntersectionPoint.Z > (nearestPointBelow.Z + 0.5f))
                                                {
                                                    nearestPointBelow = new Vector3Class(modelIntersection.IntersectionPoint.X, modelIntersection.IntersectionPoint.Y, triangleIntersected.IntersectionPoint.Z);
                                                }
                                                else if (triangleIntersected.IntersectionPoint.Z < modelIntersection.IntersectionPoint.Z && nearestPointBelow == new Vector3Class())
                                                {
                                                    nearestPointBelow = new Vector3Class(modelIntersection.IntersectionPoint.X, modelIntersection.IntersectionPoint.Y, intersectionPoint.Z); ;
                                                    triangleIntersectedBelowTriangle = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            SupportCone supportCone = null;

                            if (triangleIntersectedBelowTriangle && (supportType == SupportCone.typeCreationBy.Manual))
                            {
                                var supportHeight = modelIntersection.IntersectionPoint.Z - nearestPointBelow.Z;
                                if (supportHeight > minSupportHeight)
                                {
                                    supportCone = new SupportCone(supportHeight,
                                    supportConeTopHeight,
                                    supportConeTopRadius,
                                    supportConeMiddleRadius,
                                    supportConeTopHeight,
                                    supportConeTopRadius,
                                    16, new Vector3Class(modelIntersection.IntersectionPoint.X, modelIntersection.IntersectionPoint.Y, nearestPointBelow.Z), Color.FromArgb(stlModel.Color.A + 100, supportConeColor),
                                    model: stlModel, groundSupport: false, useSupportPenetration: true,
                                    supportConeType: supportConeType,
                                    bottomWidthCorrection: supportConeBottomWidthCorrection);
                                }
                            }
                            else if (!triangleIntersectedBelowTriangle)
                            {
                                var supportHeight = modelIntersection.IntersectionPoint.Z;
                                //if (stlModel.SupportBasement) supportHeight -= UserProfileManager.UserProfile.SupportEngine_Basement_Thickness;
                                if (supportHeight > minSupportHeight)
                                {
                                    if (FeatureManager.EnableExtendedSupportConeV2)
                                    {
                                        if (defaultSupportCatalogItem.ASegmentRadius == 0 && defaultSupportCatalogItem.BSegmentRadius == 0 && defaultSupportCatalogItem.CSegmentRadius == 0)
                                        {
                                            selectedMaterial.SupportProfiles.Clear();
                                            selectedMaterial.SupportProfiles.Add(SupportProfile.CreateDefault());
                                            defaultSupportCatalogItem = selectedMaterial.SupportProfiles.First();
                                        }

                                        var ARadius = defaultSupportCatalogItem.ASegmentRadius; //1f;
                                        var AHeight = defaultSupportCatalogItem.ASegmentHeight; //1f;
                                        var BRadius = defaultSupportCatalogItem.BSegmentRadius; //0.35f;
                                        var BHeight = defaultSupportCatalogItem.BSegmentHeight; //1.5f;
                                        var CRadius = defaultSupportCatalogItem.CSegmentRadius; // 1.5f;
                                        var DRadius = defaultSupportCatalogItem.DSegmentRadius; // 0.5f;
                                        var DHeight = defaultSupportCatalogItem.DSegmentHeight; // 1.5f;

                                        supportCone = new SupportConeV2(
                                            supportConeTopHeight,
                                            supportConeTopRadius,
                                        supportConeMiddleRadius,
                                        supportConeBottomHeight,
                                        supportConeBottomRadius,
                                    16,
                                    modelIntersection,
                                     Color.FromArgb(stlModel.Color.A + 100, supportConeColor),
                                    stlModel,
                                    true,
                                    false,
                                    surfaceTriangles,
                                    supportConeType,
                                    supportConeBottomWidthCorrection,
                                    aSegmentRadius: ARadius,
                                    aSegmentHeight: AHeight,
                                    bSegmentRadius: BRadius,
                                    bSegmentHeight: BHeight,
                                    cSegmentRadius: CRadius,
                                    dSegmentRadius: DRadius,
                                    dSegmentHeight: DHeight,
                                    calcSliceContours: calcSliceContours);
                                    }
                                    else
                                    {
                                        supportCone = new SupportCone(supportHeight,
                                    supportConeTopHeight,
                                        supportConeTopRadius,
                                        supportConeMiddleRadius,
                                        supportConeBottomHeight,
                                        supportConeBottomRadius,
                                    16, new Vector3Class(modelIntersection.IntersectionPoint.X, modelIntersection.IntersectionPoint.Y, stlModel.SupportBasement ? Managers.UserProfileManager.UserProfile.SupportEngine_Basement_Thickness : 0), Color.FromArgb(stlModel.Color.A + 100, supportConeColor),
                                    useSupportPenetration: true,
                                    model: stlModel,
                                    surfaceTriangles: surfaceTriangles,
                                    supportConeType: supportConeType,
                                    bottomWidthCorrection: supportConeBottomWidthCorrection);
                                    }
                                }
                            }



                            if (supportCone != null)
                            {
                                supportCone.UpdateBoundries();
                                supportCone._color = Color.FromArgb(Convert.ToInt16(stlModel.Color.A) + 100, supportConeColor);
                                supportCone.CreationBy = SupportCone.typeCreationBy.Auto;
                                supportCone.Selected = selectSupportConeAfterCreation;
                                if (lastLayerSupportedHeight != float.MinValue)
                                {
                                    supportCone.LastSupportedLayerHeight = lastLayerSupportedHeight;
                                }

                                lock (stlModel.SupportStructure)
                                {
                                    if (supportCone.BottomPoint == UserProfileManager.UserProfile.SupportEngine_Basement_Thickness && stlModel.SupportBasement && supportType == SupportCone.typeCreationBy.Auto)
                                    {
                                        if (addToStructure && surfaceTriangles == null)
                                        {
                                            if (targetSupportStructure == null)
                                            {
                                                stlModel.SupportStructure.Add(supportCone);
                                            }
                                            else
                                            {
                                                targetSupportStructure.Add(supportCone);
                                            }
                                        }
                                        return supportCone;
                                    }
                                    else if (supportCone.BottomPoint == -UserProfileManager.UserProfile.SupportEngine_Penetration_Depth && !stlModel.SupportBasement && supportType == SupportCone.typeCreationBy.Auto)
                                    {
                                        if (addToStructure)
                                        {
                                            if (targetSupportStructure == null)
                                            {
                                                stlModel.SupportStructure.Add(supportCone);
                                            }
                                            else
                                            {
                                                targetSupportStructure.Add(supportCone);
                                            }
                                        }
                                        return supportCone;
                                    }

                                    else if (supportCone.BottomPoint != 0 && (supportType == SupportCone.typeCreationBy.Manual || supportType == SupportCone.typeCreationBy.ManualCrossSupport))
                                    {
                                        if (addToStructure)
                                        {
                                            if (targetSupportStructure == null)
                                            {
                                                //if (supportCone.TotalHeight >= 1f)
                                                //{
                                                stlModel.SupportStructure.Add(supportCone);
                                                //}
                                            }
                                            else
                                            {
                                                targetSupportStructure.Add(supportCone);
                                            }
                                        }
                                        return supportCone;
                                    }
                                    else if (supportCone.BottomPoint == 0 || supportCone.BottomPoint == UserProfileManager.UserProfile.SupportEngine_Penetration_Depth)
                                    {
                                        if (addToStructure)
                                        {
                                            if (targetSupportStructure == null)
                                            {
                                                stlModel.SupportStructure.Add(supportCone);
                                            }
                                            else
                                            {
                                                targetSupportStructure.Add(supportCone);
                                            }
                                        }
                                        return supportCone;
                                    }
                                }
                            }


                        }
                        catch (Exception exc)
                        {
                            Debug.WriteLine(exc.Message);
                        }
                    }
                }
            }

            return null;
        }

        internal static void UndoManualSupportConeProperty(ManualSupportConePropertyChange manualSupportConePropertyChangeModel)
        {
            if (manualSupportConePropertyChangeModel.SupportCone is SupportConeV2)
            {
                var supportConeV2 = manualSupportConePropertyChangeModel.SupportCone as SupportConeV2;
                switch (manualSupportConePropertyChangeModel.PropertyName)
                {
                    case "A-Segment Height":
                        supportConeV2.ASegmentHeight = manualSupportConePropertyChangeModel.PropertyValueOld;
                        break;
                    case "A-Segment Radius":
                        supportConeV2.ASegmentRadius = manualSupportConePropertyChangeModel.PropertyValueOld;
                        break;
                    case "B-Segment Height":
                        supportConeV2.BSegmentHeight = manualSupportConePropertyChangeModel.PropertyValueOld;
                        break;
                    case "B-Segment Radius":
                        supportConeV2.BSegmentRadius = manualSupportConePropertyChangeModel.PropertyValueOld;
                        break;
                    case "C-Segment Radius":
                        supportConeV2.CSegmentRadius = manualSupportConePropertyChangeModel.PropertyValueOld;
                        break;
                    case "D-Segment Height":
                        supportConeV2.DSegmentHeight = manualSupportConePropertyChangeModel.PropertyValueOld;
                        break;
                    case "D-Segment Radius":
                        supportConeV2.DSegmentRadius = manualSupportConePropertyChangeModel.PropertyValueOld;
                        break;
                    case "E-Segment Height":
                        supportConeV2.TopHeight = manualSupportConePropertyChangeModel.PropertyValueOld;
                        break;
                    case "E-Segment Radius":
                        supportConeV2.TopRadius = manualSupportConePropertyChangeModel.PropertyValueOld;
                        break;
                    case "F-Segment Radius":
                        supportConeV2.MiddleRadius = manualSupportConePropertyChangeModel.PropertyValueOld;
                        break;
                    case "G-Segment Height":
                        supportConeV2.BottomHeight = manualSupportConePropertyChangeModel.PropertyValueOld;
                        break;
                    case "G-Segment Radius":
                        supportConeV2.BottomRadius = manualSupportConePropertyChangeModel.PropertyValueOld;
                        break;
                }
            }

            switch (manualSupportConePropertyChangeModel.PropertyName)
            {
                case "Bottom Height":
                    manualSupportConePropertyChangeModel.SupportCone.BottomHeight = manualSupportConePropertyChangeModel.PropertyValueOld;
                    break;
                case "Bottom Radius":
                    manualSupportConePropertyChangeModel.SupportCone.BottomRadius = manualSupportConePropertyChangeModel.PropertyValueOld;
                    break;
                case "Middle Radius":
                    manualSupportConePropertyChangeModel.SupportCone.MiddleRadius = manualSupportConePropertyChangeModel.PropertyValueOld;
                    break;
                case "Top Height":
                    manualSupportConePropertyChangeModel.SupportCone.TopHeight = manualSupportConePropertyChangeModel.PropertyValueOld;
                    break;
                case "Top Radius":
                    manualSupportConePropertyChangeModel.SupportCone.TopRadius = manualSupportConePropertyChangeModel.PropertyValueOld;
                    break;
            }

            if (manualSupportConePropertyChangeModel.SupportCone is SupportConeV2)
            {
                (manualSupportConePropertyChangeModel.SupportCone as SupportConeV2).Update();
            }


        }

        internal static void UndoGridSupportConeProperty(GridSupportConePropertyChange gridSupportConePropertyChangeModel)
        {
            ObjectView.SelectObjectByIndex(gridSupportConePropertyChangeModel.ModelIndex);
            var selectedModel = ObjectView.SelectedModel;

            if (gridSupportConePropertyChangeModel.IsFlatSurface)
            {
                if (selectedModel.Triangles.FlatSurfaces[gridSupportConePropertyChangeModel.SelectedSurfaceIndex].SupportStructure.Count > 0)
                {
                    var currentSupportCone = selectedModel.Triangles.FlatSurfaces[gridSupportConePropertyChangeModel.SelectedSurfaceIndex].SupportStructure[0];
                    var gridSupportEvent = new SupportEventArgs()
                    {
                        BottomHeight = currentSupportCone.BottomHeight,
                        MiddleRadius = currentSupportCone.MiddleRadius,
                        BottomRadius = currentSupportCone.BottomRadius,
                        TopHeight = currentSupportCone.TopHeight,
                        TopRadius = currentSupportCone.TopRadius
                    };

                    var selectedSurface = selectedModel.Triangles.FlatSurfaces[gridSupportConePropertyChangeModel.SelectedSurfaceIndex];

                    switch (gridSupportConePropertyChangeModel.PropertyName)
                    {
                        case "Bottom Height":
                            gridSupportEvent.BottomHeight = gridSupportConePropertyChangeModel.PropertyValueOld;
                            selectedSurface.UpdateGridSupport(gridSupportEvent);
                            break;
                        case "Bottom Radius":
                            gridSupportEvent.BottomRadius = gridSupportConePropertyChangeModel.PropertyValueOld;
                            selectedSurface.UpdateGridSupport(gridSupportEvent);
                            break;
                        case "Middle Radius":
                            gridSupportEvent.MiddleRadius = gridSupportConePropertyChangeModel.PropertyValueOld;
                            selectedSurface.UpdateGridSupport(gridSupportEvent);
                            break;
                        case "Top Height":
                            gridSupportEvent.TopHeight = gridSupportConePropertyChangeModel.PropertyValueOld;
                            selectedSurface.UpdateGridSupport(gridSupportEvent);
                            break;
                        case "Top Radius":
                            gridSupportEvent.TopRadius = gridSupportConePropertyChangeModel.PropertyValueOld;
                            selectedSurface.UpdateGridSupport(gridSupportEvent);
                            break;
                        case "Cone Distance":
                            selectedSurface.SupportDistance = gridSupportConePropertyChangeModel.PropertyValueOld;
                            selectedModel.UpdateSurfaceSupportASync(selectedSurface);
                            break;
                        case "Cross Support":
                            selectedSurface.CrossSupport = gridSupportConePropertyChangeModel.PropertyValueOld == 0f ? false : true;
                            selectedModel.UpdateSurfaceSupportASync(selectedSurface);
                            break;
                    }


                }
            }
            else if (gridSupportConePropertyChangeModel.IsHorizontalSurface)
            {

                var selectedSurface = selectedModel.Triangles.HorizontalSurfaces[gridSupportConePropertyChangeModel.SelectedSurfaceIndex];

                if (selectedSurface.SupportStructure.Count > 0)
                {
                    var currentSupportCone = selectedSurface.SupportStructure[0];
                    var gridSupportEvent = new SupportEventArgs()
                    {
                        BottomHeight = currentSupportCone.BottomHeight,
                        MiddleRadius = currentSupportCone.MiddleRadius,
                        BottomRadius = currentSupportCone.BottomRadius,
                        TopHeight = currentSupportCone.TopHeight,
                        TopRadius = currentSupportCone.TopRadius
                    };

                    switch (gridSupportConePropertyChangeModel.PropertyName)
                    {
                        case "Bottom Height":
                            gridSupportEvent.BottomHeight = gridSupportConePropertyChangeModel.PropertyValueOld;
                            selectedSurface.UpdateGridSupport(gridSupportEvent);
                            break;
                        case "Bottom Radius":
                            gridSupportEvent.BottomRadius = gridSupportConePropertyChangeModel.PropertyValueOld;
                            selectedSurface.UpdateGridSupport(gridSupportEvent);
                            break;
                        case "Middle Radius":
                            gridSupportEvent.MiddleRadius = gridSupportConePropertyChangeModel.PropertyValueOld;
                            selectedSurface.UpdateGridSupport(gridSupportEvent);
                            break;
                        case "Top Height":
                            gridSupportEvent.TopHeight = gridSupportConePropertyChangeModel.PropertyValueOld;
                            selectedSurface.UpdateGridSupport(gridSupportEvent);
                            break;
                        case "Top Radius":
                            gridSupportEvent.TopRadius = gridSupportConePropertyChangeModel.PropertyValueOld;
                            selectedSurface.UpdateGridSupport(gridSupportEvent);
                            break;
                        case "Cone Distance":
                            selectedSurface.SupportDistance = gridSupportConePropertyChangeModel.PropertyValueOld;
                            selectedModel.UpdateSurfaceSupportASync(selectedSurface);
                            break;
                        case "Cross Support":
                            selectedSurface.CrossSupport = gridSupportConePropertyChangeModel.PropertyValueOld == 0f ? false : true;
                            selectedModel.UpdateSurfaceSupportASync(selectedSurface);
                            break;
                    }


                }
            }

        }

        internal static void UpdateModelSingleSupportCones(SupportEventArgs changingEvents, STLModel3D stlModel)
        {
            var supportConeThreadEvents = new List<ManualResetEvent>();
            foreach (var supportCone in stlModel.SupportStructure)
            {
                var supportConeThreadEvent = new ManualResetEvent(false);
                supportConeThreadEvents.Add(supportConeThreadEvent);
                ThreadPool.QueueUserWorkItem(new WaitCallback((_) =>
                {
                    supportCone.Hidden = false;

                    supportCone.Update(changingEvents, stlModel);

                    supportConeThreadEvent.Set();

                }));

            }

            Helpers.ThreadHelper.WaitForAll(supportConeThreadEvents.ToArray());
        }


    }
}
