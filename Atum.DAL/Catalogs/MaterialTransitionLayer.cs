using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.DAL.Catalogs
{
    [Serializable]
    public class MaterialTransitionLayer
    {
        public float CT { get; set; }
        public float LI { get; set; }
        public float RTAC { get; set; }
    }
}
