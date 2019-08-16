using Atum.Studio.Core.Models;
using Atum.Studio.Core.Structs;
using OpenTK;
using System;
using System.Collections.Generic;

namespace Atum.Studio.Core.Shapes
{
    internal class Torus : STLModel3D
    {
        internal Torus(float innerRadius,
                     float outerRadius,
                     int torusSegmentCount,
                     int torusSegmentBandCount,
                     float angle = 360)
        {
            this.Triangles = new TriangleInfoList();

            this.InnerRadius = innerRadius;
            this.OuterRadius = outerRadius;
            this.TorusSegmentCount = torusSegmentCount;
            this.TorusSegmentBandCount = torusSegmentBandCount;

            var torusVertexList = new List<VertexClass>();
            var torusIndicesList = new List<int>();

            //Loop rounds the circumference creating all of the individual segments of the torus
            for (int i = 0; i < this.TorusSegmentCount; i++)
            {
                //Calculate the starting angle of the torus segment
                float torusAngle = (float)(System.Math.PI * 2f / (360f / angle)) * ((float)i / this.TorusSegmentCount);

                //Create a vector that is pointing in the current direction of the
                //segment we want to create, this is a unit vector
                var currentSegmentDirection = new Vector3Class((float)System.Math.Cos(torusAngle), (float)System.Math.Sin(torusAngle), 0);

                //For each segment we want to create a number of bands going around
                //the segment
                for (int j = 0; j < this.TorusSegmentBandCount; j++)
                {
                    float bandAngle = (float)((System.Math.PI * 2)) * ((float)j / (float)this.TorusSegmentBandCount);
                    Vector3Class currentBandDirection = (float)System.Math.Cos(bandAngle) * currentSegmentDirection + new Vector3Class(0, 0, (float)System.Math.Sin(bandAngle));

                    var p = new Vector3Class(0, 0, 0) + (this.InnerRadius * currentSegmentDirection) + this.OuterRadius * currentBandDirection;
                    torusVertexList.Add(new VertexClass() { Position = p });
                    int a = torusVertexList.Count - 1;
                    int b = torusVertexList.Count - 2;
                    int c = torusVertexList.Count - 2 - this.TorusSegmentBandCount;
                    int d = torusVertexList.Count - 1 - this.TorusSegmentBandCount;

                    if (j == 0)
                    {
                        b += this.TorusSegmentBandCount;
                        c += this.TorusSegmentBandCount;
                    }

                    if (i == 0)
                    {
                        c += this.TorusSegmentBandCount * this.TorusSegmentCount;
                        d += this.TorusSegmentBandCount * this.TorusSegmentCount;
                    }

                    torusIndicesList.Add(a);
                    torusIndicesList.Add(b);
                    torusIndicesList.Add(c);
                    torusIndicesList.Add(a);
                    torusIndicesList.Add(c);
                    torusIndicesList.Add(d);
                }
            }

            for (var torusVertexIndice = 0; torusVertexIndice < torusIndicesList.Count; torusVertexIndice += 3)
            {
                try
                {
                    var triangle = new Triangle();
                    triangle.Vectors[1] = torusVertexList[torusIndicesList[torusVertexIndice]];
                    triangle.Vectors[0] = torusVertexList[torusIndicesList[torusVertexIndice + 1]];
                    triangle.Vectors[2] = torusVertexList[torusIndicesList[torusVertexIndice + 2]];
                    triangle.CalcNormal();
                    this.Triangles[0].Add(triangle);
                }
                catch (Exception exc)
                {

                }
            }
        }

        public float InnerRadius
        {
            get;
            set;
        }

        public float OuterRadius
        {
            get;
            set;
        }

        /// <summary>
        /// The number of segments that create a single torus segment
        /// </summary>
        public int TorusSegmentBandCount
        {
            get;
            set;
        }

        /// <summary>
        /// The number of segments to break the torus circumference into.
        /// </summary>
        public int TorusSegmentCount
        {
            get;
            set;
        }

    }
}
