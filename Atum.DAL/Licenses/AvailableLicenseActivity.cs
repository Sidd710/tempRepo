using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.DAL.Licenses
{
    public class AvailableLicenseActivity
    {
        public string Username { get; set; }
        public string Computername { get; set; }
        public DateTime EndsOn { get; set; }

        public AvailableLicenseActivity()
        {

        }
    }
}
