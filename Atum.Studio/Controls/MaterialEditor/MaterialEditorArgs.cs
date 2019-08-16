using Atum.DAL.Materials;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Atum.Studio.Controls.MaterialEditor
{
    public class MaterialDisplayNameArgs: EventArgs
    {
        public string DisplayName { get; set; }
        public Material Material { get; set; }
    }

    public class MaterialModelColorArgs : EventArgs
    {
        public Color ModelColor { get; set; }
        public Material Material { get; set; }
    }

    public class MaterialSupplierDisplayNameArgs : EventArgs
    {
        public string DisplayName { get; set; }
    }
}
