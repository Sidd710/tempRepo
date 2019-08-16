using System;
using System.Collections.Generic;
using System.Linq;

namespace Atum.Licenses.Activator.Objects
{
    public class Office365User
    {
        internal string Account { get; set; }
        internal string Pwd { get; set; }

        public Office365User()
        {
            this.Account = "license@atum3d.com";
            this.Pwd = "Atum2W0rk";
        }
    }
}
