using Atum.Studio.Core.Structs;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Shapes
{
    internal class OuterPathNormal
    {
        internal Vector3Class EdgeNormalStartPoint { get; set; }
        internal Vector3Class EdgeNormal { get; set; }

        internal Vector3Class EdgeStartPoint { get; set; }
        internal Vector3Class EdgeEndPoint { get; set; }


        internal Vector3Class FirstPoint { get; set; }
        internal Vector3Class SecondPoint { get; set; }
        internal Vector3Class RemainingPoint { get; set; }


        internal Triangle EdgeTriangle { get; set; }
        //internal int ArrayIndex { get; set; }
        //internal int TriangleIndex { get; set; }

            internal TriangleConnectionInfo FirstTriangleConnectionIndex { get; set; }
        internal TriangleConnectionInfo SecondTriangleConnectionIndex { get; set; }
        //internal int FirstVectorIndex { get; set; }
        //internal int SecondVectorIndex { get; set; }


    }
}
