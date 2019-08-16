using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Engines.PackingEngine
{
    /// <summary>
    /// A simple rectangular area.
    /// </summary>
    public class Rectangle
    {
        /// <summary>
        /// Amount of mm in X direction for the area
        /// </summary>
        public virtual float SizeX { get; set; }

        /// <summary>
        /// Amount of mm in Y direction for the area
        /// </summary>
        public virtual float SizeY { get; set; }

        /// <summary>
        /// The X position of the rectangle
        /// </summary>
        public float PositionX { get; set; }

        /// <summary>
        /// The Y position of the rectangle
        /// </summary>
        public float PositionY { get; set; }

        /// <summary>
        /// The area of the rectangle in mm2
        /// </summary>
        public float Area
        {
            get
            {
                return SizeX * SizeY;
            }
        }

        public Rectangle()
        {
        }

        public Rectangle(float sizeX, float sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
        }

        public bool Fits(Rectangle other, float clearanceX, float clearanceY)
        {
            return Fits(new Rectangle()
            {
                SizeX = other.SizeX + clearanceX,
                SizeY = other.SizeY + clearanceY
            });
        }

        public bool Fits(Rectangle other)
        {
            return other.SizeX <= SizeX && other.SizeY <= SizeY;
        }
    }
}
