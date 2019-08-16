using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Engines.PackingEngine
{
    /// <summary>
    /// A shelf which contains packedItems in a shelf packing strategy
    /// </summary>
    public class Shelf : Rectangle
    {
        /// <summary>
        /// Items on this shelf
        /// </summary>
        public List<PackedItem> Items { get; set; }

        /// <summary>
        /// The unused areas that remains after filling the shelf
        /// </summary>
        public List<Rectangle> UnusedAreas { get; set; }

        public Shelf()
        {
            Items = new List<PackedItem>();
            UnusedAreas = new List<Rectangle>();
        }
    }
}
