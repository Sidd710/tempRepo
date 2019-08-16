using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace Atum.Studio.Core.Models
{
    public class MAGSAIMarkSelectionGizmo : STLModel3D
    {
        public enum TypeOfSelectionBox {
            Circle = 0,
            Square = 1
            }


        //internal int Percentage { get; set; }
        internal int Diameter { get; set; }
        internal Point MousePosition { get; set; }
        internal List<Vector2> CrossPoints = new List<Vector2>();
        internal List<Vector2> OutlinePoints = new List<Vector2>();
        internal TypeOfSelectionBox SelectionBoxType { get; set; }

        internal MAGSAIMarkSelectionGizmo()
        {

        }

        internal void UpdateMousePosition(Point mousePosition, int sceneControlWidth, int sceneControlHeight)
        {
            this.MousePosition = mousePosition;
            this.CalcPoints(sceneControlWidth, sceneControlHeight);
        }

        private void CalcPoints(int sceneControlWidth, int sceneControlHeight)
        {
            OutlinePoints.Clear();

            if (this.SelectionBoxType == TypeOfSelectionBox.Circle)
            {
                var circlePoints = Helpers.VectorHelper.CreateCircle(0, this.Diameter / 2, 255, false);
                foreach (var circlePoint in circlePoints)
                {
                    OutlinePoints.Add(new Vector2(this.MousePosition.X, sceneControlHeight - this.MousePosition.Y) + circlePoint.Xy);
                }
            }
            else
            {
                OutlinePoints.Add(new Vector2(this.MousePosition.X - (this.Diameter / 2), sceneControlHeight - this.MousePosition.Y - (this.Diameter / 2))); //left top
                OutlinePoints.Add(new Vector2(this.MousePosition.X - (this.Diameter / 2), sceneControlHeight - this.MousePosition.Y + (this.Diameter / 2))); //left bottom
                OutlinePoints.Add(new Vector2(this.MousePosition.X + (this.Diameter / 2), sceneControlHeight - this.MousePosition.Y + (this.Diameter / 2))); //right top
                OutlinePoints.Add(new Vector2(this.MousePosition.X + (this.Diameter / 2), sceneControlHeight - this.MousePosition.Y - (this.Diameter / 2))); //right bottom
            }

            //selectionbox cross
            var innerCircleSize = 10f;
            var innerCircleCenterPointOffset = 2;
            CrossPoints.Clear();
            
            CrossPoints.Add(new Vector2(this.MousePosition.X, sceneControlHeight - this.MousePosition.Y) + new Vector2(0, innerCircleCenterPointOffset));
            CrossPoints.Add(new Vector2(this.MousePosition.X, sceneControlHeight - this.MousePosition.Y) + new Vector2(0, innerCircleCenterPointOffset) + new Vector2(0, innerCircleSize));

            CrossPoints.Add(new Vector2(this.MousePosition.X, sceneControlHeight - this.MousePosition.Y) + new Vector2(0, -innerCircleCenterPointOffset));
            CrossPoints.Add(new Vector2(this.MousePosition.X, sceneControlHeight - this.MousePosition.Y) + new Vector2(0, -innerCircleCenterPointOffset) + new Vector2(0, -innerCircleSize));

            CrossPoints.Add(new Vector2(this.MousePosition.X, sceneControlHeight - this.MousePosition.Y) + new Vector2(innerCircleCenterPointOffset, 0));
            CrossPoints.Add(new Vector2(this.MousePosition.X, sceneControlHeight - this.MousePosition.Y) + new Vector2(innerCircleCenterPointOffset, 0) + new Vector2(innerCircleSize, 0));

            CrossPoints.Add(new Vector2(this.MousePosition.X, sceneControlHeight - this.MousePosition.Y) + new Vector2(-innerCircleCenterPointOffset, 0));
            CrossPoints.Add(new Vector2(this.MousePosition.X, sceneControlHeight - this.MousePosition.Y) + new Vector2(-innerCircleCenterPointOffset, 0) + new Vector2(-innerCircleSize, 0));
        }

        internal void UpdateSelectionBox(Events.SelectionBoxSizeEventArgs e, int sceneControlWidth, int sceneControlHeight, Point mousePosition)
        {
            this.MousePosition = mousePosition;
            this.SelectionBoxType = e.SelectionBoxType;

            CalcPoints(sceneControlWidth, sceneControlHeight);
        }
    }
}