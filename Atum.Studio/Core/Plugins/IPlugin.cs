using Atum.Studio.Core.Models;
using Atum.Studio.Core.Shapes;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Atum.Studio.Core.Plugins
{
    internal interface IPlugin
    {
        event Action<IPlugin> PluginLoaded;

        bool HasInitializeMethod { get; set; }
        bool HasPostSliceMethod { get; set; }
        bool HasMenuStripIcon { get; set; }
        bool HasStatusStripIcon { get; set; }

        PluginTypes.PostSliceActionType PostSliceActionType { get; set; }

        void Initialize(frmStudioMain mainForm);
        void MenuStripIcon(MenuStrip menuStrip);
        void StatusStripIcon(StatusStrip menuStrip);

        List<SlicePolyLine3D> PostSlice(STLModel3D stlModel, float bleedingCorrection, PluginTypes.PostSliceActionType postSliceActions);
        void PostSlice(Slices.Slice renderSlice, DAL.Hardware.AtumPrinter selectedPrinter, PluginTypes.PostSliceActionType postSliceActions);
        void PostSlice(byte[] pixelValues, int antiAliasColor, int materialSmoothingOffset, Atum.DAL.Print.PrintJob.AntiAliasType antiAliasSide, PluginTypes.PostSliceActionType postSliceAction, int printerResolutionX);
    }
}
