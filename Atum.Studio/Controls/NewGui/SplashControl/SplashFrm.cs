using Atum.DAL.Licenses;
using Atum.DAL.Managers;
using Atum.Studio.Controls.NewGui.SplashControl.UnlicensedControl;
using Atum.Studio.Core.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls.NewGui.SplashControl
{
    public partial class SplashFrm : Form
    {
        internal event EventHandler ControlClosed;

        internal event EventHandler UnLicensedProgramControlLicensedProgramControlOpenNewProject;
        internal event EventHandler UnLicensedProgramControlLicensedProgramControlOpenExistingProject;

        internal event EventHandler RecentFilesSelectedRecentFileChanged;
        internal event EventHandler RecentFilesOpenExistsProject;
        internal event EventHandler RecentFilesOpenNewProject;

        internal event EventHandler SplashSkipWelcomClicked;

        private DAL.Licenses.OnlineCatalogLicenses _license;
        public int DaysLeft { get; set; }
        public bool IsTrailLicense { get; set; }
        public bool IsStandardLicense { get; set; }
        public DAL.Licenses.OnlineCatalogLicenses DataSource
        {
            get
            {
                return this._license;
            }
            set
            {
                this._license = value;

                //this.UpdateControl();
            }
        }


        public SplashFrm()
        {
            InitializeComponent();

            //license request
            LicenseClientManager.Start();

            this.StartPosition = FormStartPosition.CenterScreen;

            this.ShowIcon = false;
            this.Icon = BrandingManager.MainForm_Icon;
            this.ShowIcon = true;

            bool licenseActivated = true;
            IsTrailLicense = false;
            DaysLeft = 0;
            if (LicenseClientManager.CurrentLicenses != null &&
                LicenseClientManager.CurrentLicenses.Count > 0 &&
                (LicenseClientManager.CurrentLicenses[0].ExpirationDate < DateTime.Now ||
                    LicenseClientManager.CurrentLicenses[0].SystemID != SystemManager.GetUUID()))
            {
                licenseActivated = false;
            }
            else
            {
                if (LicenseClientManager.CurrentLicenses != null && LicenseClientManager.CurrentLicenses.Count > 0)
                {
                    var license = LicenseClientManager.CurrentLicenses.First();
                    if (license.LicenseType == DAL.Licenses.AvailableLicense.TypeOfLicense.Trial)
                    {
                        IsTrailLicense = true;
                        DaysLeft = (license.ExpirationDate - DateTime.Now).Days;
                        if (DaysLeft < 0)
                        {
                            licenseActivated = false;
                        }
                    }
                    else if (license.LicenseType == DAL.Licenses.AvailableLicense.TypeOfLicense.StudioStandard)
                    {
                        IsStandardLicense = true;
                        DaysLeft = (license.ExpirationDate - DateTime.Now).Days;
                        if (DaysLeft < 1)
                        {
                            licenseActivated = false;
                        }
                    }
                }
            }

            var splashControl = new SplashControl(this);
            splashControl.SplashSkipWelcomClicked += SplashControl_SkipWelcomClicked;
            var plTrail = (Panel)splashControl.Controls["plTrail"];
            if (plTrail != null)
            {
                var lblLinkAuthorize = (LinkLabel)plTrail.Controls["lblLinkAuthorize"];
                lblLinkAuthorize.Click += LblLinkAuthorize_Click;
            }

            spcSplashContainer.Panel1.Controls.Add(splashControl);

            if (!licenseActivated)
            {
                splashControl.HideSkipWelcomeScreen();
                LoadUnAuthorisedControl();
            }
            else
            {
                LoadRecentOpenedFileControl();
            }
        }

        private void SplashControl_SkipWelcomClicked(object sender, EventArgs e)
        {
            SplashSkipWelcomClicked?.Invoke(null, null);
        }

        private void LoadUnAuthorisedControl()
        {
            spcSplashContainer.Panel2.Controls.Clear();
            var unLicensedProgramControl = new UnlicensedProgramControl(this);
            unLicensedProgramControl.ControlClosed += ChildControl_ControlClosed;
            unLicensedProgramControl.LicensedProgramControlOpenExistingProject += UnLicensedProgramControl_LicensedProgramControlOpenExistingProject;
            unLicensedProgramControl.LicensedProgramControlOpenNewProject += UnLicensedProgramControl_LicensedProgramControlOpenNewProject;
            var lbl30DaysTrial = (LinkLabel)unLicensedProgramControl.Controls["lbl30DaysTrial"];
            lbl30DaysTrial.Click += Lbl30DaysTrial_Click;
            spcSplashContainer.Panel2.Controls.Add(unLicensedProgramControl);

            if (!IsTrailLicense)
            {
                unLicensedProgramControl.HideStartTrialLink();
            }
        }

        private void ChildControl_ControlClosed(object sender, EventArgs e)
        {
            ControlClosed?.Invoke(null, null);
        }

        private void UnLicensedProgramControl_LicensedProgramControlOpenNewProject(object sender, EventArgs e)
        {
            UnLicensedProgramControlLicensedProgramControlOpenNewProject?.Invoke(sender, e);
        }

        private void UnLicensedProgramControl_LicensedProgramControlOpenExistingProject(object sender, EventArgs e)
        {
            UnLicensedProgramControlLicensedProgramControlOpenExistingProject?.Invoke(sender, e);
        }

        private void LblLinkAuthorize_Click(object sender, EventArgs e)
        {
            LoadUnAuthorisedControl();
        }

        private void Lbl30DaysTrial_Click(object sender, EventArgs e)
        {
            LoadRecentOpenedFileControl();
        }

        private void LoadRecentOpenedFileControl()
        {
            spcSplashContainer.Panel2.Controls.Clear();
            var recentFilesControl = new RecentFilesControl();
            recentFilesControl.OpenNewProject += RecentFilesControl_OpenNewProject;
            recentFilesControl.OpenExistsProject += RecentFilesControl_OpenExistsProject;
            recentFilesControl.SelectedRecentFileChanged += RecentFilesControl_SelectedRecentFileChanged;
            recentFilesControl.ControlClosed += ChildControl_ControlClosed;
            spcSplashContainer.Panel2.Controls.Add(recentFilesControl);
        }

        private void RecentFilesControl_SelectedRecentFileChanged(object sender, EventArgs e)
        {
            RecentFilesSelectedRecentFileChanged?.Invoke(sender, e);
        }

        private void RecentFilesControl_OpenExistsProject(object sender, EventArgs e)
        {
            RecentFilesOpenExistsProject?.Invoke(sender, e);
        }

        private void RecentFilesControl_OpenNewProject(object sender, EventArgs e)
        {
            RecentFilesOpenNewProject?.Invoke(sender, e);
        }

    }
}
