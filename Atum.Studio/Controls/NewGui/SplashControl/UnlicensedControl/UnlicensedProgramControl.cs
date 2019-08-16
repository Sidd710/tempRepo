using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atum.Studio.Core.Managers;
using System.IO;
using Atum.Studio.Controls.NewGui.SplashControl.LicensedControl;

namespace Atum.Studio.Controls.NewGui.SplashControl.UnlicensedControl
{
    public partial class UnlicensedProgramControl : UserControl
    {
        private DAL.Licenses.OnlineCatalogLicenses _license;

        internal event EventHandler ControlClosed;

        internal event EventHandler LicensedProgramControlOpenNewProject;
        internal event EventHandler LicensedProgramControlOpenExistingProject;

        public DAL.Licenses.OnlineCatalogLicenses DataSource
        {
            get
            {
                return this._license;
            }
            set
            {
                this._license = value;

            }
        }
        private SplashFrm SplashForm = null;
        
        public UnlicensedProgramControl(SplashFrm splashFrm)
        {
            SplashForm = splashFrm;
            InitializeComponent();

            this.pbWarning.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.warning, pbWarning.Size);
            this.pbClose.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.cross_white, pbClose.Size);

            if (FontManager.Loaded)
            {
                this.lbl30DaysTrial.Font = FontManager.Montserrat14Regular;
            }
        }

        private void lbl30DaysTrial_Click(object sender, EventArgs e)
        {
             
        }

        internal void HideStartTrialLink()
        {
            this.lbl30DaysTrial.Visible = false;
        }

        internal void ShowStartTrialLink()
        {
            this.lbl30DaysTrial.Visible = false;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            using (var popup = new OpenFileDialog())
            {
                popup.Filter = "(*.lic)|*.lic|(*.LIC)|*.LIC";
                popup.Multiselect = false;
                Stream fileStream = null;
                if (popup.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(popup.FileName))
                {
                    if (File.Exists(popup.FileName))
                    {
                        try
                        {
                            if ((fileStream = popup.OpenFile()) != null)
                            {                                
                                using (fileStream)
                                {
                                    StreamReader reader = new StreamReader(fileStream);
                                    string fileContents = reader.ReadToEnd();
                                    this.DataSource = DAL.Licenses.OnlineCatalogLicenses.FromLicenseStream(fileContents);
                                    this.DataSource.ToLicenseFile(DAL.ApplicationSettings.Settings.LocalLicenseFilePath);
                                    if (this.DataSource[0].Activated)
                                    {
                                        if (SplashForm != null)
                                        {
                                            var spcSplashContainer = (SplitContainer)SplashForm.Controls["spcSplashContainer"];
                                            spcSplashContainer.Panel2.Controls.Clear();
                                            var licensedProgramControl = new LicensedProgramControl();
                                            licensedProgramControl.OpenNewProject += LicensedProgramControl_OpenNewProject;
                                            licensedProgramControl.OpenExistsProject += LicensedProgramControl_OpenExistsProject;
                                            licensedProgramControl.ControlClosed += pbClose_Click;
                                            spcSplashContainer.Panel2.Controls.Add(licensedProgramControl);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                           
                        }
                    }
                }
            }
   
        }

        private void LicensedProgramControl_OpenExistsProject(object sender, EventArgs e)
        {
            LicensedProgramControlOpenExistingProject?.Invoke(sender, e);
        }

        private void LicensedProgramControl_OpenNewProject(object sender, EventArgs e)
        {
            LicensedProgramControlOpenNewProject?.Invoke(sender, e);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var computerName = Environment.MachineName;
            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.AddExtension = true;
                saveDialog.FileName = !string.IsNullOrEmpty(computerName) ? computerName + ".lic" : string.Empty;
                saveDialog.DefaultExt = ".lic";
                #if LOCTITE
                saveDialog.Filter = "Loctite License File(*.lic)|*.lic";
                #else
                saveDialog.Filter = "Operator Station License File(*.lic)|*.lic";
                #endif
                if (saveDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(saveDialog.FileName))
                {
                   var licenseContents = DAL.Licenses.OnlineCatalogLicenses.FromLicenseFile().ToLicenseRequest();
                    File.WriteAllLines(saveDialog.FileName,new string[] { licenseContents }, Encoding.UTF8);
                }
            }
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            ControlClosed?.Invoke(null, null);
        }

        private void lbl30DaysTrial_LinkClicked(object sender, EventArgs e)
        {

        }
    }
}
