using Atum.Studio.Core.Events;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Structs;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Atum.Studio.Core.Engines
{
    static class PrintjobSimulatorEngine
    {
        public static List<SlicePolyLine3D> SimulationPolyLines;

        internal static bool IsReadyForRendering;
        internal static long TotalProcessedSlices { get; set; }
        internal static long TotalAmountSlices { get; set; }
        internal static Atum.DAL.Print.PrintJob PrintJob { get; set; }

        internal static STLModel3D STLModel;

        internal static event EventHandler SimulationPrintJobCanceled;
        internal static event EventHandler SimulationPrintJobCompleted;

        readonly static object _totalProcessedSlicesLock = new object();

        internal static void CreateSimulationPrintJobAsync(Core.Slices.RenderSliceInfo sliceInfo)
        {
            SimulationPolyLines = new List<SlicePolyLine3D>();
            TotalProcessedSlices = 0;
            TotalAmountSlices = 1;

            var bwPrintJobSimulatorEngine = new System.ComponentModel.BackgroundWorker();
            bwPrintJobSimulatorEngine.DoWork += bwbwPrintJobSimulatorEngine_DoWork;
            bwPrintJobSimulatorEngine.RunWorkerAsync(sliceInfo);
        }

        static void bwbwPrintJobSimulatorEngine_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            CreateSimulationPrintJob(e.Argument as Core.Slices.RenderSliceInfo);
        }


        internal static void CreateSimulationPrintJob(Core.Slices.RenderSliceInfo sliceInfo)
        {
            var selectedPrinterResolutionX = PrinterManager.DefaultPrinter.ProjectorResolutionX;
            var selectedPrinterResolutionY = PrinterManager.DefaultPrinter.ProjectorResolutionY;

            try
            {

                TotalProcessedSlices = 0;
                //copy org vectors to undopoints
                foreach (var object3d in ObjectView.Objects3D)
                {
                    if (object3d is STLModel3D)
                    {
                        var stlModel = object3d as STLModel3D;
                        stlModel.CreateUndoPoints();
                    }
                }

                //update triangles with min/max z
                var topPoint = 0f;
                foreach (var object3d in ObjectView.Objects3D)
                {
                    if (object3d is STLModel3D)
                    {
                        var stlModel = (STLModel3D)object3d;
                        //if (UserProfileManager.UserProfiles[0].Settings_PrintJob_MirrorObjects) stlModel.Mirror(false, false);
                        stlModel.UpdateTrianglesMinMaxZ();
                        if (stlModel.TopPoint > topPoint) { topPoint = stlModel.TopPoint; }
                    }
                }

                foreach (var object3d in ObjectView.Objects3D)
                {
                    if (object3d is STLModel3D && !(object3d is GroundPane))
                    {
                        if (object3d is Models.Defaults.TrapezoidCorrection)
                        {
                            var stlModel = (Models.Defaults.TrapezoidCorrection)object3d;
                            stlModel.Scale(2.0025f, 2.0025f, 2, ScaleEventArgs.TypeAxis.ALL, false, false);

                            //move move 1px to left
                            for (var arrayIndex = 0; arrayIndex < stlModel.Triangles.Count; arrayIndex++)
                            {
                                for (var triangleIndex = 0; triangleIndex < stlModel.Triangles[arrayIndex].Count; triangleIndex++)
                                {
                                    if (triangleIndex == 0 && arrayIndex == 0)
                                    {
                                        Console.WriteLine(stlModel.Triangles[arrayIndex][triangleIndex].Vectors[0].Position);
                                    }
                                    stlModel.Triangles[arrayIndex][triangleIndex].Vectors[0].Position -= new Vector3(0.25f, 0.1f, 0);
                                    stlModel.Triangles[arrayIndex][triangleIndex].Vectors[1].Position -= new Vector3(0.25f, 0.1f, 0);
                                    stlModel.Triangles[arrayIndex][triangleIndex].Vectors[2].Position -= new Vector3(0.25f, 0.1f, 0);
                                }
                            }

                            if (stlModel.SupportBasement)
                            {
                                stlModel.SupportBasementStructure.CalcSliceIndexes(sliceInfo.Material);
                            }
                        }
                        else
                        {
                            var stlModel = (STLModel3D)object3d;
                            stlModel.PreviousScaleFactorX = stlModel.ScaleFactorX;
                            stlModel.PreviousScaleFactorY = stlModel.ScaleFactorY;
                            stlModel.PreviousScaleFactorZ = stlModel.ScaleFactorZ;

                            if (stlModel.SupportBasement)
                            {
                                stlModel.SupportBasementStructure._scaleFactorX = 1;
                                stlModel.SupportBasementStructure._scaleFactorY = 1;
                                stlModel.SupportBasementStructure._scaleFactorZ = 1;
                                stlModel.SupportBasementStructure.Scale((float)((stlModel.SupportBasementStructure.ScaleFactorX / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorX) * sliceInfo.Material.ShrinkFactor), 0, 0, Events.ScaleEventArgs.TypeAxis.X, false, false);
                                stlModel.SupportBasementStructure.Scale(0, (float)((stlModel.SupportBasementStructure.ScaleFactorY / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorY) * sliceInfo.Material.ShrinkFactor), 0, Events.ScaleEventArgs.TypeAxis.Y, false, false);
                                stlModel.SupportBasementStructure.Scale(0, 0, (float)(stlModel.SupportBasementStructure.ScaleFactorZ * sliceInfo.Material.ShrinkFactor), Events.ScaleEventArgs.TypeAxis.Z, false, false);

                                //combine movetranslation with scaled vector
                                // stlModel.SupportBasementStructure.MoveTranslation = stlModel.MoveTranslation;
                                stlModel.SupportBasementStructure.CombineMoveTranslationWithVectors(new OpenTK.Vector3(
                                    (stlModel.MoveTranslation.X / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorX) * (float)sliceInfo.Material.ShrinkFactor,
                                (stlModel.MoveTranslation.Y / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorY) * (float)sliceInfo.Material.ShrinkFactor,
                                stlModel.MoveTranslation.Z * (float)sliceInfo.Material.ShrinkFactor));

                                stlModel.SupportBasementStructure.CalcSliceIndexes(sliceInfo.Material);
                            }

                            stlModel.Scale((float)((stlModel.ScaleFactorX / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorX) * sliceInfo.Material.ShrinkFactor), 0, 0, Events.ScaleEventArgs.TypeAxis.X, false, false);
                            stlModel.Scale(0, (float)((stlModel.ScaleFactorY / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorY) * sliceInfo.Material.ShrinkFactor), 0, Events.ScaleEventArgs.TypeAxis.Y, false, false);
                            stlModel.Scale(0, 0, (float)(stlModel.ScaleFactorZ * sliceInfo.Material.ShrinkFactor), Events.ScaleEventArgs.TypeAxis.Z, false, false);

                            //combine movetranslation with scaled vector
                            stlModel.CombineMoveTranslationWithVectors(new OpenTK.Vector3(
                                    (stlModel.MoveTranslation.X / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorX) * (float)sliceInfo.Material.ShrinkFactor,
                                (stlModel.MoveTranslation.Y / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorY) * (float)sliceInfo.Material.ShrinkFactor,
                                stlModel.MoveTranslation.Z * (float)sliceInfo.Material.ShrinkFactor));


                            foreach (var supportCone in stlModel.SupportStructure)
                            {
                                supportCone._scaleFactorX = 1;
                                supportCone._scaleFactorY = 1;
                                supportCone._scaleFactorZ = 1;

                                supportCone.Scale((float)((supportCone.ScaleFactorX / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorX) * sliceInfo.Material.ShrinkFactor), 0, 0, Events.ScaleEventArgs.TypeAxis.X, false, false);
                                supportCone.Scale(0, (float)((supportCone.ScaleFactorY / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorY) * sliceInfo.Material.ShrinkFactor), 0, Events.ScaleEventArgs.TypeAxis.Y, false, false);
                                supportCone.Scale(0, 0, (float)(supportCone.ScaleFactorZ * sliceInfo.Material.ShrinkFactor), Events.ScaleEventArgs.TypeAxis.Z, false, false);

                                supportCone.CombineMoveTranslationWithVectors(new OpenTK.Vector3(
                                    (supportCone.MoveTranslation.X / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorX +
                                    stlModel.MoveTranslation.X / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorX) * (float)sliceInfo.Material.ShrinkFactor,
                                    (supportCone.MoveTranslation.Y / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorY +
                              stlModel.MoveTranslation.Y / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorY) * (float)sliceInfo.Material.ShrinkFactor,
                              stlModel.MoveTranslation.Z * (float)sliceInfo.Material.ShrinkFactor));

                                supportCone.CalcSliceIndexes(sliceInfo.Material);
                            }

                            foreach (var supportCone in stlModel.Triangles.HorizontalSurfaces.SupportStructure)
                            {
                                supportCone._scaleFactorX = 1;
                                supportCone._scaleFactorY = 1;
                                supportCone._scaleFactorZ = 1;

                                supportCone.Scale((float)((supportCone.ScaleFactorX / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorX) * sliceInfo.Material.ShrinkFactor), 0, 0, Events.ScaleEventArgs.TypeAxis.X, false, false);
                                supportCone.Scale(0, (float)((supportCone.ScaleFactorY / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorY) * sliceInfo.Material.ShrinkFactor), 0, Events.ScaleEventArgs.TypeAxis.Y, false, false);
                                supportCone.Scale(0, 0, (float)(supportCone.ScaleFactorZ * sliceInfo.Material.ShrinkFactor), Events.ScaleEventArgs.TypeAxis.Z, false, false);

                                supportCone.CombineMoveTranslationWithVectors(new OpenTK.Vector3(
                                    (supportCone.MoveTranslation.X / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorX +
                                    stlModel.MoveTranslation.X / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorX) * (float)sliceInfo.Material.ShrinkFactor,
                                    (supportCone.MoveTranslation.Y / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorY +
                              stlModel.MoveTranslation.Y / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorY) * (float)sliceInfo.Material.ShrinkFactor,
                              stlModel.MoveTranslation.Z * (float)sliceInfo.Material.ShrinkFactor + (stlModel.SupportBasement ? stlModel.SupportBasementStructure.TopPoint : 0)));

                                supportCone.CalcSliceIndexes(sliceInfo.Material);
                            }

                            foreach (var supportCone in stlModel.Triangles.FlatSurfaces.SupportStructure)
                            {
                                supportCone._scaleFactorX = 1;
                                supportCone._scaleFactorY = 1;
                                supportCone._scaleFactorZ = 1;

                                supportCone.Scale((float)((supportCone.ScaleFactorX / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorX) * sliceInfo.Material.ShrinkFactor), 0, 0, Events.ScaleEventArgs.TypeAxis.X, false, false);
                                supportCone.Scale(0, (float)((supportCone.ScaleFactorY / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorY) * sliceInfo.Material.ShrinkFactor), 0, Events.ScaleEventArgs.TypeAxis.Y, false, false);
                                supportCone.Scale(0, 0, (float)(supportCone.ScaleFactorZ * sliceInfo.Material.ShrinkFactor), Events.ScaleEventArgs.TypeAxis.Z, false, false);

                                //combine movetranslation with scaled vector
                                supportCone.CombineMoveTranslationWithVectors(new OpenTK.Vector3(
                                                                    (supportCone.MoveTranslation.X / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorX +
                                                                    stlModel.MoveTranslation.X / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorX) * (float)sliceInfo.Material.ShrinkFactor,
                                                                    (supportCone.MoveTranslation.Y / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorY +
                                                              stlModel.MoveTranslation.Y / Managers.PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorY) * (float)sliceInfo.Material.ShrinkFactor,
                                                              stlModel.MoveTranslation.Z * (float)sliceInfo.Material.ShrinkFactor));

                                supportCone.CalcSliceIndexes(sliceInfo.Material);
                            }


                        }
                    }
                }

                foreach (var object3d in ObjectView.Objects3D)
                {
                    if (object3d is STLModel3D && (!(object3d is GroundPane)))
                    {
                        var stlModel = (STLModel3D)object3d;
                        stlModel.CalcSliceIndexes(sliceInfo.Material);
                    }
                }

                //var sliceIndex = 0;
                var sliceCount = 0;

                //
                //determine slicecount
                var initialLayersHeight = 0f;
                for (var initialLayerIndex = 1; initialLayerIndex < sliceInfo.Material.InitialLayers + 1; initialLayerIndex++)
                {
                    initialLayersHeight = (float)sliceInfo.Material.LT1 * initialLayerIndex;

                    sliceCount++;

                    if (initialLayersHeight > topPoint)
                    {
                        break;
                    }
                }

                if (initialLayersHeight < topPoint)
                {
                    var remainingLayersHeight = topPoint - initialLayersHeight;
                    var remainingLayersCount = (remainingLayersHeight / sliceInfo.Material.LT2) + 1;

                    for (var remainingLayerIndex = 0; remainingLayerIndex < remainingLayersCount; remainingLayerIndex++)
                    {
                        sliceCount++;
                    }
                }


                var highestModelIndex = 0;
                TotalAmountSlices = 0;
                foreach (var object3d in ObjectView.Objects3D)
                {
                    if (object3d is STLModel3D && (!(object3d is GroundPane)))
                    {
                        var stlModel = object3d as STLModel3D;
                        if (stlModel.SliceIndexes.Count >= TotalAmountSlices)
                        {
                            TotalAmountSlices = stlModel.SliceIndexes.Count;
                            highestModelIndex = stlModel.Index;
                        }
                    }
                }

                var currentSliceIndex = 0;
                if (highestModelIndex > 0 && TotalAmountSlices > 0)
                {
                    SortedDictionary<float, List<TriangleConnectionInfo>> stlModelIndexes = null;
                    foreach (var object3d in ObjectView.Objects3D)
                    {
                        if (object3d is STLModel3D && (!(object3d is GroundPane)))
                        {
                            var stlModel = object3d as STLModel3D;
                            if (stlModel.Index == highestModelIndex)
                            {
                                stlModelIndexes = stlModel.SliceIndexes;
                                break;
                            }
                        }
                    }

                    foreach (var sliceIndexKey in stlModelIndexes)
                    {
                        var renderSliceInfo = new Core.Slices.RenderSliceInfo();
                        renderSliceInfo.SliceIndex = currentSliceIndex;
                        renderSliceInfo.SliceHeight = sliceIndexKey.Key;
                        renderSliceInfo.SliceCount = stlModelIndexes.Count;
                        renderSliceInfo.PrinterResolutionX = selectedPrinterResolutionX;
                        renderSliceInfo.PrinterResolutionY = selectedPrinterResolutionY;
                        renderSliceInfo.Material = sliceInfo.Material;

                        if (Managers.PerformanceSettingsManager.Settings.PrintJobGenerationMultiThreading && renderSliceInfo.SliceIndex > 0)
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback((_) => { RenderSliceThread(renderSliceInfo); }));
                        }
                        else
                        {
                            currentSliceIndex = RenderSliceThread(renderSliceInfo);
                        }

                        currentSliceIndex++;
                    }
                }
                else
                {
                    //no printjob/objects found
                    if (SimulationPrintJobCanceled != null) SimulationPrintJobCanceled(null, null);
                }

                //cleanjob image
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }


        }

        internal static int RenderSliceThread(object renderSliceInfo)
        {
            if (Managers.PerformanceSettingsManager.Settings.PrintJobGenerationMaxMemoryLimitEnabled)
            {
                while ((Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024) > Managers.PerformanceSettingsManager.Settings.PrintJobGenerationMaxMemory)
                {
                    Thread.Sleep(1000);
                    GC.Collect();
                    GC.Collect();

                    Atum.DAL.Managers.LoggingManager.WriteToLog("Render Engine", "Memory management", "Waiting for free memory");
                }
            }
            var renderSlice = (Core.Slices.RenderSliceInfo)renderSliceInfo;
            Slices.Slice slice = new Slices.Slice(new List<SlicePolyLine3D>(), new List<List<SlicePolyLine3D>>(), new List<SlicePolyLine3D>(), renderSlice.PrinterResolutionX, renderSlice.PrinterResolutionY, renderSlice.SliceHeight);


            foreach (var object3d in ObjectView.Objects3D)
            {
                var supportZPolys = new List<List<SlicePolyLine3D>>();

                if (object3d is STLModel3D && (!(object3d is GroundPane)))
                {
                    var stlModel = (STLModel3D)object3d;

                    foreach (var supportCone in stlModel.SupportStructure)
                    {
                        if (!supportCone.Hidden)
                        {
                            var supportConeZPolys = GetZPolys(supportCone, renderSlice.SliceHeight);
                            supportZPolys.Add(GetZIntersections(supportConeZPolys, renderSlice.SliceHeight));
                        }
                    }

                    foreach (var supportCone in stlModel.Triangles.HorizontalSurfaces.SupportStructure)
                    {
                        if (!supportCone.Hidden)
                        {
                            var supportConeZPolys = GetZPolys(supportCone, renderSlice.SliceHeight);
                            supportZPolys.Add(GetZIntersections(supportConeZPolys, renderSlice.SliceHeight));
                        }
                    }

                    foreach (var supportCone in stlModel.Triangles.FlatSurfaces.SupportStructure)
                    {
                        if (!supportCone.Hidden)
                        {
                            var supportConeZPolys = GetZPolys(supportCone, renderSlice.SliceHeight);
                            supportZPolys.Add(GetZIntersections(supportConeZPolys, renderSlice.SliceHeight));
                        }
                    }

                    if (renderSlice.SliceHeight <= (0.2f * renderSlice.Material.ShrinkFactor) && stlModel.SupportBasement && stlModel.SupportBasementStructure != null)
                    {
                        var supportBase = (STLModel3D)stlModel.SupportBasementStructure.Clone();
                        supportBase.MoveTranslation = stlModel.MoveTranslation;

                        foreach (var triangle in supportBase.Triangles[0])
                        {
                            triangle.CalcMinMaxZ();
                        }
                        var supportConeZPolys = GetZPolys(supportBase, renderSlice.SliceHeight);
                        supportZPolys.Add(GetZIntersections(supportConeZPolys, renderSlice.SliceHeight));
                    }
                    var modelZPolys = GetZIntersections(GetZPolys(stlModel, renderSlice.SliceHeight), renderSlice.SliceHeight);

                    using (var modelSlice = new Core.Slices.Slice(modelZPolys, supportZPolys,null, renderSlice.PrinterResolutionX, renderSlice.PrinterResolutionY, renderSlice.SliceHeight))
                    {
                        modelSlice.RenderSliceAsByteArray();
                        slice.ModelPoints.AddRange(modelSlice.ModelPoints);
                        slice.SupportPoints.AddRange(modelSlice.SupportPoints);
                    }
                }
            }

            RenderSliceToPolygons(slice, renderSlice);

            return renderSlice.SliceIndex;
        }

        public static void RenderSliceToPolygons(Slices.Slice slice, Slices.RenderSliceInfo renderSlice)
        {
            var pixelValues = new byte[renderSlice.PrinterResolutionX * renderSlice.PrinterResolutionY];
            var pixelLength = pixelValues.Length;
        //        Array.Clear(pixelValues, 0, pixelLength);

                int beginPixel = 0;
                int beginPixelOfRow = 0;
                int endPixel = 0;
                int endPixelOfRow = 0;
                SlicePoint2D p1;
                SlicePoint2D p2;

                //model points
                if (slice.ModelPoints.Count % 2 == 0)
                {

                    for (int cnt = 0; cnt < slice.ModelPoints.Count; cnt += 2)  // increment by 2
                    {
                        p1 = slice.ModelPoints[cnt];
                        p2 = slice.ModelPoints[cnt + 1];
                        if (p1.X > p2.X) //flip over
                        {
                            var pTemp = p1;
                            p1 = p2;
                            p2 = pTemp;
                        }
                        p1.X++;

                        if (p1.X <= p2.X)
                        {
                            if (p2.Y >= 0 && p1.Y >= 0)
                            {
                                beginPixel = (p1.Y * renderSlice.PrinterResolutionX + p1.X);
                                beginPixelOfRow = (((p1.Y) * renderSlice.PrinterResolutionX));
                                if (p1.X < 0)
                                {
                                    beginPixel = (p1.Y * renderSlice.PrinterResolutionX);
                                }

                                //get end pixel of row
                                endPixel = ((p2.Y * renderSlice.PrinterResolutionX + p2.X));
                                endPixelOfRow = (((p1.Y + 1) * renderSlice.PrinterResolutionX)) - 1;

                                if (endPixel > endPixelOfRow)
                                {
                                    endPixel = endPixelOfRow;
                                }

                                if (pixelValues.Length - 1 >= beginPixel && beginPixel >= 0)
                                {
                                pixelValues[beginPixel] = 255;
                                }

                                if (p2.X < 0)
                                {
                                    endPixel = ((p2.Y * renderSlice.PrinterResolutionX));
                                }

                                //fill color between begin and endpoints
                                if (pixelValues.Length - 1 >= endPixel && endPixel >= 0)
                                {
                                pixelValues[endPixel] = 255;

                                    if (beginPixel < endPixel)
                                    {
                                        for (var betweenPixel = beginPixel + 1; betweenPixel < endPixel; betweenPixel++)
                                        {
                                            if (betweenPixel >= 0)
                                            {
                                            pixelValues[betweenPixel] = 255;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (slice.SupportPoints.Count % 2 == 0)  // is even
                {
                    for (int cnt = 0; cnt < slice.SupportPoints.Count; cnt += 2)  // increment by 2
                    {
                        p1 = slice.SupportPoints[cnt];
                        p2 = slice.SupportPoints[cnt + 1];
                        if (p1.X > p2.X) //flip over
                        {
                            var pTemp = p1;
                            p1 = p2;
                            p2 = pTemp;
                        }
                        p1.X++;

                        if (p1.X <= p2.X)
                        {
                            if (p2.Y >= 0 && p1.Y >= 0)
                            {
                                beginPixel = (p1.Y * renderSlice.PrinterResolutionX + p1.X);
                                beginPixelOfRow = (((p1.Y) * renderSlice.PrinterResolutionX));
                                if (p1.X < 0)
                                {
                                    beginPixel = (p1.Y * renderSlice.PrinterResolutionX);
                                }

                                //get end pixel of row
                                endPixel = ((p2.Y * renderSlice.PrinterResolutionX + p2.X));
                                endPixelOfRow = (((p1.Y + 1) * renderSlice.PrinterResolutionX));

                                if (endPixel > endPixelOfRow)
                                {
                                    endPixel = endPixelOfRow;
                                }

                                if (pixelValues.Length - 1 >= beginPixel && beginPixel >= 0)
                                {
                                pixelValues[beginPixel] = 255;
                                }

                                if (p2.X < 0)
                                {
                                    endPixel = ((p2.Y * renderSlice.PrinterResolutionX));
                                }

                                if (pixelValues.Length - 1 >= endPixel && endPixel >= 0)
                                {
                                pixelValues[endPixel] = 255;

                                    if (beginPixel < endPixel)
                                    {
                                        for (var betweenPixel = beginPixel + 1; betweenPixel < endPixel; betweenPixel++)
                                        {
                                            if (betweenPixel >= 0)
                                            {
                                            pixelValues[betweenPixel] = 255;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                else  // flag error
                {
                    Console.WriteLine("Row y=" + " odd # of points = " + slice.SupportPoints.Count + " - Model may have holes");
                }

                //convert pixel values to polylines
                for (var pixelRowIndex = 0; pixelRowIndex < renderSlice.PrinterResolutionY; pixelRowIndex++)
                {
                    var beginRowIndex = renderSlice.PrinterResolutionX * pixelRowIndex;
                    var startOfPolygon = false;
                    var startOfPolygonIndex = 0;
                    var endOfPolygonIndex = 0;

                    for (var pixelColumnIndex = 0; pixelColumnIndex < renderSlice.PrinterResolutionX; pixelColumnIndex++)
                    {
                        if (pixelValues[beginRowIndex + pixelColumnIndex] == 255)
                        {
                            if (!startOfPolygon)
                            {
                                startOfPolygon = true;
                                startOfPolygonIndex = pixelColumnIndex;
                            }
                        }
                        else if (pixelValues[beginRowIndex + pixelColumnIndex] == 0)
                            if (startOfPolygon)
                            {
                                endOfPolygonIndex = pixelColumnIndex;
                                startOfPolygon = false;

                                //add polygon to return list
                                var slicePolygon = new SlicePolyLine3D();
                                slicePolygon.Points.Add(new Vector3(startOfPolygonIndex, pixelRowIndex, renderSlice.SliceHeight));
                                slicePolygon.Points.Add(new Vector3(endOfPolygonIndex, pixelRowIndex, renderSlice.SliceHeight));
                                SimulationPolyLines.Add(slicePolygon);
                            }
                    }
                }
            


            lock (_totalProcessedSlicesLock)
            {
                TotalProcessedSlices++;
            }

            if (TotalProcessedSlices == TotalAmountSlices)
            {
                if (SimulationPrintJobCompleted != null)
                {
                    //convert polygons to vectors
                    STLModel = new STLModel3D(STLModel3D.TypeObject.Model, true);
                    STLModel.Triangles = new TriangleInfoList();

                    var triangleIndex = 0;
                    var arrayIndex = 0;
                    foreach(var polyLine in SimulationPolyLines)
                    {
                        if (polyLine != null)
                        {
                            var polygonCube = new Shapes.Cube(polyLine.Points[1].X - polyLine.Points[0].X + 1, polyLine.Points[1].Y - polyLine.Points[0].Y + 1, 1);
                            //Console.WriteLine(polygonCube.Center);
                            for (var cubeVertexIndex = 0; cubeVertexIndex < polygonCube.VertexArray.Length; cubeVertexIndex += 3)
                            {
                                var triangle = new Triangle();
                                triangle.Normal = polygonCube.VertexArray[cubeVertexIndex].Normal;

                                triangle.Vectors[0].Position = new Vector3(-48, -20, 0);
                                triangle.Vectors[0].Normal = triangle.Vectors[1].Normal = triangle.Vectors[2].Normal = triangle.Normal;
                                triangle.Vectors[0].Color = triangle.Vectors[1].Color = triangle.Vectors[2].Color = new Byte4() { A = 128, B = 255 };
                                triangle.Vectors[1].Position = new Vector3(-48, -20, 0);
                                triangle.Vectors[2].Position = new Vector3(-48, -20, 0);


                                triangle.Vectors[0].Position += polygonCube.VertexArray[cubeVertexIndex].Position / 20;
                                triangle.Vectors[1].Position += polygonCube.VertexArray[cubeVertexIndex + 1].Position / 20;
                                triangle.Vectors[2].Position += polygonCube.VertexArray[cubeVertexIndex + 2].Position / 20;

                                //add 0 point
                                triangle.Vectors[0].Position += new Vector3(polyLine.Points[0].X / 20, polyLine.Points[0].Y / 20, polyLine.Points[0].Z);
                                triangle.Vectors[1].Position += new Vector3(polyLine.Points[0].X / 20, polyLine.Points[0].Y / 20, polyLine.Points[0].Z);
                                triangle.Vectors[2].Position += new Vector3(polyLine.Points[0].X / 20, polyLine.Points[0].Y / 20, polyLine.Points[0].Z);

                                if (triangleIndex > 33333)
                                {
                                    IsReadyForRendering = true;
                                    break;
                                    triangleIndex = 0;
                                    arrayIndex++;
                                    STLModel.Triangles.Add(new List<Triangle>());
                                }

                                triangleIndex++;
                                STLModel.Triangles[arrayIndex].Add(triangle);

                            }
                        }
                    }

                    IsReadyForRendering = true;
                    SimulationPrintJobCompleted(null, null);
                }
            }
        }

        public static List<SlicePolyLine3D> GetZIntersections(List<Triangle> triangles, float zcur)
        {
            try
            {
                SlicePolyLine3D s3d = null;
                var lstlines = new List<SlicePolyLine3D>();
                foreach (var triangle in triangles)
                {
                    s3d = triangle.IntersectZPlane(zcur);
                    if (s3d != null)
                    {
                        lstlines.Add(s3d);
                    }
                }
                return lstlines;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /*
        Return a list of polygons that intersect at this zlevel
        */
        public static List<Triangle> GetZPolys(STLModel3D stlModel, float zlev)
        {
            var sliceHeight = (float)Math.Round(zlev, 3);
            var lst = new List<Triangle>();
            List<TriangleConnectionInfo> triangleConnections = null;
            if (stlModel.SliceIndexes != null && stlModel.SliceIndexes.ContainsKey(sliceHeight))
            {
                triangleConnections = stlModel.SliceIndexes[sliceHeight];
                for (var triangleConnectionIndex = 0; triangleConnectionIndex < triangleConnections.Count; triangleConnectionIndex++)
                {
                    lst.Add(stlModel.Triangles[triangleConnections[triangleConnectionIndex].ArrayIndex][triangleConnections[triangleConnectionIndex].TriangleIndex]);
                }
            }

            return lst;
        }
    }
}
