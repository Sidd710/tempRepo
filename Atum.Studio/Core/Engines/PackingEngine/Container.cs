using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Engines.PackingEngine
{
    /// <summary>
    /// A container is a rectangle which can store items or other containers.
    /// </summary>
    public class Container : Rectangle
    {
        public Container Parent { get; set; }

        /// <summary>
        /// Items in this container
        /// </summary>
        public List<PackedItem> Items { get; set; }

        /// <summary>
        /// Container areas within this container
        /// </summary>
        public List<Container> Children { get; set; }

        public Container()
        {
            Items = new List<PackedItem>();
            Children = new List<Container>();
        }

        public void AddChild(Container childContainer)
        {
            childContainer.Parent = this;
            Children.Add(childContainer);
        }

        public List<Container> GetAll()
        {
            List<Container> containers = new List<Container>();
            containers.Add(this);
            foreach (var child in this.Children)
            {
                containers.AddRange(child.GetAll());
            }

            return containers;
        }

        public static Container Create(Rectangle rectangle)
        {
            Container container = new Container()
            {
                SizeX = rectangle.SizeX,
                SizeY = rectangle.SizeY,
                PositionX = rectangle.PositionX,
                PositionY = rectangle.PositionY
            };

            return container;
        }
    }
}
