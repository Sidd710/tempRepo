using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Atum.LicenseManager
{
    partial class srvLicenseManager : ServiceBase
    {
        public srvLicenseManager()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            DAL.Managers.LoggingManager.Start();
            DAL.Managers.EventLogManager.Start();
            Managers.LicenseServerManager.Start();


            Managers.LicenseServerOnlineCatalogManager.Start();
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
    }
}
