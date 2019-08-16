using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Engines.PackingEngine
{
    /// <summary>
    /// Describes the outcome of cloning a set of models
    /// </summary>
    public class PackingSolution
    {
        /// <summary>
        /// The resulting packed items for this solution
        /// </summary>
        public List<PackedItem> PackedItems { get; set; }

        /// <summary>
        /// The packing strategy that created this solution
        /// </summary>
        public IPackingStrategy PackingStrategy { get; set; }

        /// <summary>
        /// The amount of clones that could be packed for this solution
        /// </summary>
        public int CloneCount { get; set; }

        /// <summary>
        /// The time it took to create the packing solution. Can be used to determine the efficiency of the algorithm
        /// </summary>
        public long DurationMs { get; set; }

        /// <summary>
        /// The unused space areas for this solution that cannot be used by any model
        /// </summary>
        public List<Container> UnusedSpaces { get; set; }

        public int[] FootprintCloneCount { get; set; }

        public List<ModelFootprint> Footprints { get; set; }

        public PackingSolution()
        {
            PackedItems = new List<PackedItem>();
            UnusedSpaces = new List<Container>();
        }

        /// <summary>
        /// Creates a bitmap of the solution to visualize the packing, used for debugging and unittesting to test the outcome of a packing strategy
        /// </summary>
        /// <returns></returns>
        public Bitmap CreateImage(Rectangle buildPlatform)
        {
            Bitmap bitmap = new Bitmap((int)buildPlatform.SizeX, (int)buildPlatform.SizeY);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                // Create background for buildplate
                graphics.FillRectangle(new SolidBrush(Color.DarkSlateGray), new RectangleF(0, 0, (int)buildPlatform.SizeX, (int)buildPlatform.SizeY));

                // Add footprints for all packed items
                foreach (var packedItem in PackedItems)
                {
                    var rectangle = new RectangleF(packedItem.PositionX, packedItem.PositionY, packedItem.SizeX, packedItem.SizeY);
                    SolidBrush brush = new SolidBrush(packedItem.ModelFootprint.Color);
                    graphics.FillRectangle(brush, rectangle);
                }
            }

            return bitmap;
        }

        public void SaveImage(Rectangle buildPlatform, Stream imageStream)
        {
            Bitmap solutionImage = CreateImage(buildPlatform);
            solutionImage.Save(imageStream, ImageFormat.Bmp);
        }

        public void SaveImage(Rectangle buildPlatform, string fileName)
        {
            using (FileStream imageFileStream = File.OpenWrite(fileName))
            {
                SaveImage(buildPlatform, imageFileStream);
            }
        }
    }
}
