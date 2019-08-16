using Atum.Studio.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Engines.PackingEngine
{
    /// <summary>
    /// A request to pack the most amount of models onto the build platform, optionally providing a maximum number of clones.
    /// </summary>
    public enum TypeOfPacking
    {
        DefinedNumbersOfClones = 0,
        MaxAmountOfClones = 1
    }

    public class PackModelsRequest
    {
        public TypeOfPacking PackingType { get; set; }
        /// <summary>
        /// The models to pack
        /// </summary>
        public List<ModelFootprint> ModelFootprints { get; set; }

        /// <summary>
        /// The total maximum clones to create if set
        /// </summary>
        public int? RequestedCloneCount { get; set; }

        /// <summary>
        /// The clearance to keep between models (in mm) to prevent the models from attaching to each other
        /// </summary>
        public float Clearance { get; set; }

        /// <summary>
        /// The buildplatform size
        /// </summary>
        public Rectangle BuildPlatform { get; set; }
        
        /// <summary>
        /// Determine the theorethical maximum clone count based on the surface area, 
        /// this does not take into account the actual footprint packing.
        /// </summary>
        /// <returns></returns>
        public int DetermineMaximumCloneCountBasedOnSurfaceArea()
        {
            float totalModelsArea = ModelFootprints.Sum(footprint => footprint.Area);
            float buildPlatformArea = BuildPlatform.SizeX * BuildPlatform.SizeY;
            return (int)Math.Floor(buildPlatformArea / totalModelsArea);
        }

        public PackModelsRequest()
        {
            ModelFootprints = new List<ModelFootprint>();
        }
    }
}
