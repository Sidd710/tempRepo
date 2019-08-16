using Atum.Studio.Core.Structs;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Models
{
    /// <summary>
    /// A model can have a set of linked clones which have their own position on the buildplate and can be rotated.
    /// This way the model data is shared between all instances and upon rendering the linked clone information 
    /// is used to render the clones.
    /// </summary>
    public class LinkedClone
    {
        /// <summary>
        /// The translation for the linked clone item
        /// </summary>
        public Vector3Class Translation { get; set; }

        /// <summary>
        /// If true, rotate model 90 degrees on Z-axis
        /// </summary>
        public bool Rotate { get; set; }

        public float SizeX { get; set; }

        public float SizeY { get; set; }

        public bool Selected { get; set; }
    }
}
