using Atum.DAL.Hardware;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.Shapes;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Atum.Studio.Core.Plugins.PostSlicer
{
    internal class WarpCorrectionPostSlicer : IPlugin
    {
        public event Action<IPlugin> PluginLoaded;

        public bool HasInitializeMethod { get; set; }
        public bool HasPostSliceMethod { get; set; }
        public bool HasMenuStripIcon { get; set; }
        public bool HasStatusStripIcon { get; set; }

        public PluginTypes.PostSliceActionType PostSliceActionType { get; set; }

        public WarpCorrectionPostSlicer()
        {
            this.HasInitializeMethod = false;
            this.HasPostSliceMethod = true;
            this.HasMenuStripIcon = false;
            this.HasStatusStripIcon = false;
            this.PostSliceActionType = PluginTypes.PostSliceActionType.WarpCorrection;
        }

        public void Initialize(frmStudioMain mainForm)
        {

        }

        public void StatusStripIcon(StatusStrip menuStrip)
        {
            
        }

        public void MenuStripIcon(MenuStrip menuStrip)
        {

        }

        public List<SlicePolyLine3D> PostSlice(STLModel3D stlModel, float sliceHeight, PluginTypes.PostSliceActionType postSliceAction)
        {
            switch (postSliceAction)
            {
                case PluginTypes.PostSliceActionType.WarpCorrection:
                    return Engines.RenderEngine.GetZIntersections(Engines.RenderEngine.GetZPolys(stlModel, stlModel.SliceIndexes, sliceHeight), sliceHeight);
            }

            return new List<SlicePolyLine3D>();
        }

        public void PostSlice(byte[] pixelValues, int antiAliasColor, int materialSmoothingOffset, DAL.Print.PrintJob.AntiAliasType antiAliasSide, PluginTypes.PostSliceActionType postSliceAction, int printerResolutionX)
        {
           
        }

        public void PostSlice(Slices.Slice renderSlice, AtumPrinter selectedPrinter, PluginTypes.PostSliceActionType postSliceActions)
        {

            ////apply warp correction to polynode
            //if (renderSlice.ModelPolyTrees.Length > 0 && renderSlice.ModelPolyTrees[0].Count > 0)
            //{
            //    foreach (var modelPolynode in renderSlice.ModelPolyTrees[0][0]._allPolys)
            //    {
            //        LensWarpCorrection.CalculateByContour(modelPolynode, selectedPrinter);
            //    }
            //}
            
        }
    }
}
