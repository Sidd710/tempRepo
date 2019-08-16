using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Atum.Licenses.Activator
{
    partial class srvMain : ServiceBase
    {
        private string _serviceName = "Loctite License Activator";

        private Timer _servicePollTimer = new Timer();


        public srvMain()
        {
            InitializeComponent();

            this.ServiceName = this._serviceName;

            _servicePollTimer.Interval = 60000;
            _servicePollTimer.Elapsed += servicePollTimer_Elapsed;
            _servicePollTimer.Start();

        }

        private void servicePollTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _servicePollTimer.Stop();
            Program.Start(new string[0]);
            _servicePollTimer.Start();
        }

        protected override void OnStart(string[] args)
        {
            _servicePollTimer.Stop();
            Program.Start(args);
            _servicePollTimer.Start();
        }

        protected override void OnStop()
        {
            Program.Stop();
        }
    }
}
