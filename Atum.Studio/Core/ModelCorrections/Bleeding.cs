using Atum.Studio.Core.Models;
using Atum.Studio.Core.Shapes;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Studio.Core.ModelCorrections
{
    [Serializable]
    public class BleedingModel : Models.STLModel3D
    {

        public BleedingModel(STLModel3D stlModel, double bleedingDistance)
        {
            this.Triangles = new TriangleInfoList();
            for (var arrayIndex = 0; arrayIndex < stlModel.Triangles.Count; arrayIndex++)
            {
                if (arrayIndex > 0)
                {
                    this.Triangles.Add(new List<Triangle>());
                }

                for (var triangleIndex = 0; triangleIndex < stlModel.Triangles[arrayIndex].Count; triangleIndex++)
                {
                    var fBleedingOffset = (float)bleedingDistance;
                    this.Triangles[arrayIndex].Add((Triangle)stlModel.Triangles[arrayIndex][triangleIndex].Clone());
                    this.Triangles[arrayIndex][triangleIndex].Vectors[0].Color = new Structs.Byte4(new byte[] { 255, 255, 0, 0 });
                    this.Triangles[arrayIndex][triangleIndex].Vectors[0].Position.Z -= fBleedingOffset;
                    this.Triangles[arrayIndex][triangleIndex].Vectors[1].Color = new Structs.Byte4(new byte[] { 255, 255, 0, 0 });
                    this.Triangles[arrayIndex][triangleIndex].Vectors[1].Position.Z -= fBleedingOffset;
                    this.Triangles[arrayIndex][triangleIndex].Vectors[2].Color = new Structs.Byte4(new byte[] { 255, 255, 0, 0 });
                    this.Triangles[arrayIndex][triangleIndex].Vectors[2].Position.Z -= fBleedingOffset;
                    this.Triangles[arrayIndex][triangleIndex].CalcMinMaxZ();
                }
            }

            this.BindModel();
        }

        //internal void CalcSides(Models.STLModel3D model)
        //{
        //    for (var arrayIndex = 0; arrayIndex < model.Triangles.Count; arrayIndex++)
        //    {
        //        for (var triangleIndex = 0; triangleIndex < model.Triangles[arrayIndex].Count; triangleIndex++)
        //        {
        //            if (model.Triangles[arrayIndex][triangleIndex].Normal.Z > 0 && model.Triangles[arrayIndex][triangleIndex].Bottom > 0.5f)
        //            {
        //                var triangle = model.Triangles[arrayIndex][triangleIndex];

        //                //check if connected triangles have a +normal. If so this is an bleedingedge triangle
        //                foreach (var connectedTriangle in model.Triangles.Connections[arrayIndex][triangleIndex])
        //                {
        //                    if (model.Triangles[connectedTriangle.ArrayIndex][connectedTriangle.TriangleIndex].Normal.Z <= 0)
        //                    {
        //                        //get connected sides
        //                        var connectedSides = new List<Vector3>();
        //                        foreach (var trianglePoint in triangle.Points)
        //                        {
        //                            foreach (var connectedTrianglePoint in model.Triangles[connectedTriangle.ArrayIndex][connectedTriangle.TriangleIndex].Points)
        //                            {
        //                                if (connectedTrianglePoint == trianglePoint)
        //                                {
        //                                    connectedSides.Add(trianglePoint);
        //                                }
        //                            }
        //                        }


        //                        //check direction
        //                        if (connectedSides.Count == 2)
        //                        {
        //                            //Console.WriteLine(triangleIndex);
        //                            if (!(connectedSides[0] == triangle.Points[0] && connectedSides[1] == triangle.Points[2]))
        //                            {
        //                                connectedSides.Add(connectedSides[0]);
        //                                connectedSides.RemoveAt(0);
        //                            }

        //                            connectedSides[0] -= new Vector3(0, 0, offset);
        //                            connectedSides[1] -= new Vector3(0, 0, offset);

        //                            //connectedSides[0] += new Vector3(model.MoveTranslationX, model.MoveTranslationY, 0);
        //                            //connectedSides[1] += new Vector3(model.MoveTranslationX, model.MoveTranslationY, 0);

        //                            this.BleedingSides.Add(connectedSides);

        //                            //create triangles
        //                            var leftTriangle = new Triangle();
        //                            leftTriangle.Vectors[0].Position = new Vector3(connectedSides[0]);
        //                            leftTriangle.Vectors[0].Color = new Structs.Byte4(new byte[] { 255, 255, 0, 0 });
        //                            leftTriangle.Vectors[1].Position = new Vector3(connectedSides[1]);
        //                            leftTriangle.Vectors[1].Color = new Structs.Byte4(new byte[] { 255, 255, 0, 0 });
        //                            leftTriangle.Vectors[2].Position = new Vector3(connectedSides[1] + new Vector3(0, 0, offset));
        //                            leftTriangle.Vectors[2].Color = new Structs.Byte4(new byte[] { 255, 255, 0, 0 });
        //                            leftTriangle.CalcNormal();
        //                            leftTriangle.CalcAngleZ();
        //                            leftTriangle.CalcMinMaxZ();
        //                            leftTriangle.UpdateNormalType(true);
        //                            //                         model.Triangles[0].Add(leftTriangle);
        //                            this.Triangles[0].Add(leftTriangle);

        //                            var rightTriangle = new Triangle();
        //                            rightTriangle.Vectors[0].Color = new Structs.Byte4(new byte[] { 255, 255, 0, 0 });
        //                            rightTriangle.Vectors[0].Position = new Vector3(connectedSides[1] + new Vector3(0, 0, offset));
        //                            rightTriangle.Vectors[1].Color = new Structs.Byte4(new byte[] { 255, 255, 0, 0 });
        //                            rightTriangle.Vectors[1].Position = new Vector3(connectedSides[0] + new Vector3(0, 0, offset));
        //                            rightTriangle.Vectors[2].Color = new Structs.Byte4(new byte[] { 255, 255, 0, 0 });
        //                            rightTriangle.Vectors[2].Position = new Vector3(connectedSides[0]);
        //                            rightTriangle.CalcNormal();
        //                            rightTriangle.CalcAngleZ();
        //                            rightTriangle.CalcMinMaxZ();
        //                            rightTriangle.UpdateNormalType(true);
        //                            //                           model.Triangles[0].Add(rightTriangle);
        //                            this.Triangles[0].Add(rightTriangle);


        //                            var triangleConnection = new TriangleConnectionInfo() { ArrayIndex = arrayIndex, TriangleIndex = triangleIndex };
        //                            if (!this.bleedingEdgeTriangles.ContainsKey(triangleConnection))
        //                            {
        //                                this.bleedingEdgeTriangles.Add(triangleConnection, triangleConnection);
        //                                // break;
        //                            }
        //                        }
        //                    }
        //                }

        //                //move bleeding part down
        //                triangle.Vectors[0].Color = new Structs.Byte4(new byte[] { 255, 255, 0, 0 });
        //                triangle.Vectors[0].Position -= new Vector3(0,0, offset);
        //                triangle.Vectors[1].Color = new Structs.Byte4(new byte[] { 255, 255, 0, 0 });
        //                triangle.Vectors[1].Position -= new Vector3(0, 0, offset);
        //                triangle.Vectors[2].Color = new Structs.Byte4(new byte[] { 255, 255, 0, 0 });
        //                triangle.Vectors[2].Position -= new Vector3(0, 0, offset);
        //                triangle.CalcMinMaxZ();

        //                //set normaltype to bleeding
        //             //   triangle.UpdateNormalType(true);
        //            }
        //        }
        //    }

            
    

    }
}
