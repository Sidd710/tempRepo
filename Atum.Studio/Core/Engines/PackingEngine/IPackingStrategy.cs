using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Engines.PackingEngine
{
    /// <summary>
    /// A packing strategy can provide a packing solution for a pack models request by using a specific approach. Some approaches will
    /// work better for a certain set of models than others.
    /// </summary>
    public interface IPackingStrategy
    {
        /// <summary>
        /// Determine the packing solution for a pack models request
        /// </summary>
        /// <param name="packModelsRequest"></param>
        /// <returns></returns>
        PackingSolutions CreateSolutions(PackModelsRequest packModelsRequest);
    }
}
