using Atum.Studio.Core.Models;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Engines.PackingEngine
{
    /// <summary>
    /// Describes a placement of one of the models clones.
    /// </summary>
    public class PackedItem : Rectangle
    {
        /// <summary>
        /// Reference to the model being packed
        /// </summary>
        public ModelFootprint ModelFootprint { get; set; }

        /// <summary>
        /// The clone number for this model
        /// </summary>
        public int CloneNumber { get; set; }

        /// <summary>
        /// The amount of clearance on X axis after the item
        /// </summary>
        public float ClearanceX { get; set; }

        /// <summary>
        /// The amount of clearance on Y axis after the item
        /// </summary>
        public float ClearanceY { get; set; }

        /// <summary>
        /// Determines if the model clone should be rotated
        /// </summary>
        public bool RotateModel { get; set; }

        /// <summary>
        /// The total size including the clearance on X axis
        /// </summary>
        public float TotalSizeX
        {
            get
            {
                return SizeX + ClearanceX;
            }
        }

        /// <summary>
        /// The total size including the clearance on Y axis
        /// </summary>
        public float TotalSizeY
        {
            get
            {
                return SizeY + ClearanceY;
            }
        }

        public float TotalPositionX
        {
            get
            {
                return PositionX + TotalSizeX;
            }
        }

        public float TotalPositionY
        {
            get
            {
                return PositionY + TotalSizeY;
            }
        }

        /// <summary>
        /// The translation vector for placing the model on the buildplate
        /// </summary>
        public Vector3 GetTranslation()
        {
            return new Vector3(PositionX, PositionY, 0);
        }

        public static PackedItem Create(ModelFootprint modelFootprint)
        {
            PackedItem packedItem = new PackedItem()
            {
                ModelFootprint = modelFootprint,
                SizeX = modelFootprint.SizeX,
                SizeY = modelFootprint.SizeY,
                RotateModel = modelFootprint.RotateModel,
            };
            return packedItem;
        }
    }
}
