using Atum.Studio.Core.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Engines.PackingEngine
{
    /// <summary>
    /// Contains the model footprint information used for creating packing solution
    /// </summary>
    public class ModelFootprint : Rectangle
    {
        /// <summary>
        /// An optional reference to the model for this footprint information
        /// </summary>
        public STLModel3D Model { get; set; }

        /// <summary>
        /// An optional color to visualize this footprint
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// If true, rotate model 90 degrees over Z-axis for a better orientation
        /// </summary>
        public bool RotateModel { get; set; }

        /// <summary>
        /// The requested clone count for this particular footprint
        /// </summary>
        public int RequestedCloneCount { get; set; } = int.MaxValue;

        /// <summary>
        /// The actual clone count when a packing solution is generated
        /// </summary>
        public int CloneCount { get; set; }

        private float _sizeX;

        public override float SizeX
        {
            get
            {
                if (!RotateModel)
                    return _sizeX;
                else
                    return _sizeY;
            }
            set
            {
                if (!RotateModel)
                    _sizeX = value;
                else
                    _sizeY = value;
            }
        }

        private float _sizeY;
        public override float SizeY
        {
            get
            {
                if (!RotateModel)
                    return _sizeY;
                else
                    return _sizeX;
            }
            set
            {
                if (!RotateModel)
                    _sizeY = value;
                else
                    _sizeX = value;
            }
        }

        public ModelFootprint()
        {
        }

        public ModelFootprint(float sizeX, float sizeY)
        {
            _sizeX = sizeX;
            _sizeY = sizeY;
            Color = Color.LightGray;
        }

        public ModelFootprint(float sizeX, float sizeY, Color color) : this(sizeX, sizeY)
        {
            Color = color;
        }

        public ModelFootprint Clone()
        {
            ModelFootprint clonedFootprint = new ModelFootprint(this.SizeX, this.SizeY, this.Color);
            clonedFootprint.Model = this.Model;
            clonedFootprint.PositionX = this.PositionX;
            clonedFootprint.PositionY = this.PositionY;
            clonedFootprint.RequestedCloneCount = this.RequestedCloneCount;
            clonedFootprint.RotateModel = this.RotateModel;
            clonedFootprint.CloneCount = this.CloneCount;
            return clonedFootprint;
        }

        /// <summary>
        /// Create a model footprint for a STL model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ModelFootprint FromModel(STLModel3D model)
        {
            ModelFootprint modelFootprint = new ModelFootprint();
            modelFootprint.Model = model;
            if (model.SupportBasement)
            {
                if (model.SupportBasementStructure != null)
                {
                    model.SupportBasementStructure.UpdateBoundries();
                }
            }
            
            modelFootprint.SizeX = Math.Abs(model.FootprintRightPoint - model.FootprintLeftPoint);
            modelFootprint.SizeY = Math.Abs(model.FootprintBackPoint - model.FootprintFrontPoint);

            modelFootprint.Color = model.Color;

            return modelFootprint;
        }
    }
}
