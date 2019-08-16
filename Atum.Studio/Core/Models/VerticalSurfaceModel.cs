using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Shapes;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Atum.Studio.Core.Helpers.ContourHelper;
using Atum.Studio.Core.Engines.MagsAI;
using Atum.DAL.Materials;
using Atum.Studio.Core.Helpers;
using System.Collections.Concurrent;
using Atum.DAL.Hardware;

namespace Atum.Studio.Core.Models
{
    internal class VerticalSurfaceModel : STLModel3D
    {
        internal ConcurrentDictionary<float, PolyTree> ModelContours = new ConcurrentDictionary<float, PolyTree>();

        internal VerticalSurfaceModel()
        {
        }
            
        internal void CalcSlicesIndexes(Material selectedMaterial, AtumPrinter selectedPrinter, float bottomPoint, float topPoint, int sliceIndexModulus = 1)
        {
            this.CalcSliceIndexes(selectedMaterial, true);

            var firstSliceIndex = 0;
            var firstSliceHeight = 0f;
            foreach (var sliceHeight in this.SliceIndexes.Keys)
            {
                if (sliceHeight > bottomPoint)
                {
                    firstSliceHeight = sliceHeight;
                    break;
                }

                firstSliceIndex++;
            }

            var lastSliceHeight = 0f;
            foreach (var sliceHeight in this.SliceIndexes.Keys)
            {
                if (sliceHeight > topPoint)
                {
                    lastSliceHeight = sliceHeight;
                    break;
                }
            }


            Parallel.ForEach(MagsAIEngine.SliceHeightsWithModulus.Keys, sliceHeightAsync =>
            {
                var sliceHeight = sliceHeightAsync;
                var sliceIndex = firstSliceIndex;
                if (sliceHeight >= firstSliceHeight && sliceHeight <= lastSliceHeight)
                {
                    PolyTree sliceAngledContours = null;
                    var sliceContours = GetSliceContours(sliceIndex, sliceHeight, selectedPrinter, selectedMaterial, out sliceAngledContours);
                    ModelContours.TryAdd(sliceHeight, sliceContours);
                    sliceIndex++;
                }
                
            });

        }
    }
}
