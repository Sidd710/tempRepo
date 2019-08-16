using System;
using System.Collections.Generic;
using System.Linq;

namespace Loctite.Licenses.Activator.Objects
{
    public class Office365User
    {
        internal string Account { get; set; }
        internal string Pwd { get; set; }

        public Office365User()
        {
            this.Account = "activate-lic@loctite-pr10.com";
            this.Pwd = "Loctite2W0rk";
        }
    }
}
