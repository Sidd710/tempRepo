using Atum.DAL.Hardware;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.Shapes;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Atum.Studio.Core.Plugins.PostSlicer
{
    internal class AdvancedPostSlicer : IPlugin
    {
        public event Action<IPlugin> PluginLoaded;

        public bool HasInitializeMethod { get; set; }
        public bool HasPostSliceMethod { get; set; }
        public bool HasMenuStripIcon { get; set; }
        public bool HasStatusStripIcon { get; set; }

        public PluginTypes.PostSliceActionType PostSliceActionType { get; set; }

        public AdvancedPostSlicer()
        {
            this.HasInitializeMethod = false;
            this.HasPostSliceMethod = true;
            this.HasMenuStripIcon = false;
            this.HasStatusStripIcon = false;
            this.PostSliceActionType = PluginTypes.PostSliceActionType.Bleeding | PluginTypes.PostSliceActionType.AntiAliasOffset;
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
                case PluginTypes.PostSliceActionType.Bleeding:
                    return Engines.RenderEngine.GetZIntersections(Engines.RenderEngine.GetZPolys(stlModel, stlModel.ModelBleedingSliceIndexes, sliceHeight), sliceHeight);
            }

            return new List<SlicePolyLine3D>();
        }

        public void PostSlice(byte[] pixelValues, int antiAliasColor, int materialSmoothingOffset, DAL.Print.PrintJob.AntiAliasType antiAliasSide, PluginTypes.PostSliceActionType postSliceAction, int printerResolutionX)
        {
            var pixelLength = pixelValues.Length;
            switch (postSliceAction)
            {
                case PluginTypes.PostSliceActionType.AntiAliasOffset:
                    var antiAliasFactor = antiAliasColor;
                    var antiAliasType = antiAliasSide;
                    var smoothingOffset = materialSmoothingOffset;

                    if (antiAliasType != DAL.Print.PrintJob.AntiAliasType.None && antiAliasFactor > 1 && antiAliasFactor <= 255 && smoothingOffset > 0)
                    {
                        for (var pixelIndex = 0; pixelIndex < pixelLength; pixelIndex += 3)
                        {
                            var currentPixelWhite = pixelValues[pixelIndex] == 255 && pixelValues[pixelIndex + 1] == 255 && pixelValues[pixelIndex + 2] == 255;
                            if (currentPixelWhite)
                            {
                                DrawSmoothing(pixelValues, pixelIndex, pixelLength, currentPixelWhite, printerResolutionX, antiAliasFactor, antiAliasType, smoothingOffset);

                                if (smoothingOffset == 2)
                                {
                                    //above
                                    var rowAbovePixelIndex = pixelIndex - (printerResolutionX * 3);
                                    if (rowAbovePixelIndex > 0)
                                    {
                                        DrawSmoothing(pixelValues, rowAbovePixelIndex, pixelLength, currentPixelWhite, printerResolutionX, antiAliasFactor, antiAliasType, smoothingOffset);
                                    }

                                    //below
                                    var rowBelowPixelIndex = pixelIndex + (printerResolutionX * 3);
                                    if (rowBelowPixelIndex > 0)
                                    {
                                        DrawSmoothing(pixelValues, rowBelowPixelIndex, pixelLength, currentPixelWhite, printerResolutionX, antiAliasFactor, antiAliasType, smoothingOffset);
                                    }

                                    //left
                                    var pixelLeft = pixelIndex - 3;
                                    if (pixelLeft > 0)
                                    {
                                        DrawSmoothing(pixelValues, pixelLeft, pixelLength, currentPixelWhite, printerResolutionX, antiAliasFactor, antiAliasType, smoothingOffset);
                                    }

                                    //Right
                                    var pixelRight = pixelIndex + 3;
                                    if (pixelRight < pixelLength)
                                    {
                                        DrawSmoothing(pixelValues, pixelRight, pixelLength, currentPixelWhite, printerResolutionX, antiAliasFactor, antiAliasType, smoothingOffset);
                                    }
                                }
                            }
                        }
                    }

                    break;
            }
        }

        private void DrawSmoothing(byte[] pixelValues, int pixelIndex, int pixelLength, bool currentPixelWhite, int selectedPrinterResolutionX, int antiAliasFactor, DAL.Print.PrintJob.AntiAliasType antiAliasType, int antiAliasOffset)
        {
            int rowAboveBeginPixel = 0;
            int rowBelowBeginPixel = 0;

            //vertical query
            //above
            rowAboveBeginPixel = pixelIndex - (selectedPrinterResolutionX * 3);
            if (rowAboveBeginPixel > 0)
            {
                if (currentPixelWhite &&
                    pixelValues[rowAboveBeginPixel] == 0 &&
                    pixelValues[rowAboveBeginPixel + 1] == 0 &&
                    pixelValues[rowAboveBeginPixel + 2] == 0)
                {
                    if (antiAliasType == DAL.Print.PrintJob.AntiAliasType.Outside)
                    {
                        pixelValues[rowAboveBeginPixel] = (byte)antiAliasFactor; //BLUE
                        pixelValues[rowAboveBeginPixel + 1] = (byte)antiAliasFactor; //GREEN
                        pixelValues[rowAboveBeginPixel + 2] = (byte)antiAliasFactor; //RED

                    }
                    else
                    {
                        pixelValues[pixelIndex] = (byte)antiAliasFactor; //BLUE
                        pixelValues[pixelIndex + 1] = (byte)antiAliasFactor; //GREEN
                        pixelValues[pixelIndex + 2] = (byte)antiAliasFactor; //RED
                    }
                }
            }

            //below
            rowBelowBeginPixel = pixelIndex + (selectedPrinterResolutionX * 3);
            if (rowBelowBeginPixel + 2 < pixelLength)
            {
                if (currentPixelWhite &&
                    pixelValues[rowBelowBeginPixel] == 0 &&
                    pixelValues[rowBelowBeginPixel + 1] == 0 &&
                    pixelValues[rowBelowBeginPixel + 2] == 0)
                {
                    if (antiAliasType == DAL.Print.PrintJob.AntiAliasType.Outside)
                    {
                        pixelValues[rowBelowBeginPixel] = (byte)antiAliasFactor; //BLUE
                        pixelValues[rowBelowBeginPixel + 1] = (byte)antiAliasFactor; //GREEN
                        pixelValues[rowBelowBeginPixel + 2] = (byte)antiAliasFactor; //RED

                    }
                    else
                    {
                        pixelValues[pixelIndex] = (byte)antiAliasFactor; //BLUE
                        pixelValues[pixelIndex + 1] = (byte)antiAliasFactor; //GREEN
                        pixelValues[pixelIndex + 2] = (byte)antiAliasFactor; //RED

                    }
                }
            }

            //horizontal query
            if (pixelIndex + 6 < pixelLength)
            {
                if (currentPixelWhite &&
                    pixelValues[pixelIndex + 3] == 0 &&
                    pixelValues[pixelIndex + 4] == 0 &&
                    pixelValues[pixelIndex + 5] == 0)
                {
                    if (antiAliasType == DAL.Print.PrintJob.AntiAliasType.Outside)
                    {
                        pixelValues[pixelIndex + 3] = (byte)antiAliasFactor; //BLUE
                        pixelValues[pixelIndex + 4] = (byte)antiAliasFactor; //GREEN
                        pixelValues[pixelIndex + 5] = (byte)antiAliasFactor; //RED

                    }
                    else
                    {
                        pixelValues[pixelIndex] = (byte)antiAliasFactor; //BLUE
                        pixelValues[pixelIndex + 1] = (byte)antiAliasFactor; //GREEN
                        pixelValues[pixelIndex + 2] = (byte)antiAliasFactor; //RED
                    }
                }
                else if (pixelIndex - 3 > 0 &&
                    currentPixelWhite &&
                    pixelValues[pixelIndex - 3] == 0 &&
                    pixelValues[pixelIndex - 2] == 0 &&
                    pixelValues[pixelIndex - 1] == 0)
                {
                    if (antiAliasType == DAL.Print.PrintJob.AntiAliasType.Outside)
                    {
                        pixelValues[pixelIndex - 3] = (byte)antiAliasFactor; //BLUE
                        pixelValues[pixelIndex - 2] = (byte)antiAliasFactor; //GREEN
                        pixelValues[pixelIndex - 1] = (byte)antiAliasFactor; //RED
                    }
                    else
                    {
                        pixelValues[pixelIndex] = (byte)antiAliasFactor; //BLUE
                        pixelValues[pixelIndex + 1] = (byte)antiAliasFactor; //GREEN
                        pixelValues[pixelIndex + 2] = (byte)antiAliasFactor; //RED
                    }
                }
            }
        }

        public void PostSlice(Slices.Slice renderSlice, DAL.Hardware.AtumPrinter selectedPrinter, PluginTypes.PostSliceActionType postSliceActions)
        {
        }
    }
}
