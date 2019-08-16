using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Engines.PackingEngine
{
    /// <summary>
    /// An overview of all the packing solutions found by the packing engine
    /// </summary>
    public class PackingSolutions
    {
        /// <summary>
        /// All the found solutions
        /// </summary>
        public List<PackingSolution> Solutions { get; set; }

        /// <summary>
        /// The solution that yielded the most amound of clones with the biggest area in one piece left
        /// </summary>
        public PackingSolution BestSolution { get; set; }

        /// <summary>
        /// The total duration for generating all possible solutions
        /// </summary>
        public long TotalDurationMs { get; set; }

        public PackingSolutions()
        {
            Solutions = new List<PackingSolution>();
        }
    }
}
