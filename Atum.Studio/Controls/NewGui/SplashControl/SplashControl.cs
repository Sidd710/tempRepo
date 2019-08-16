using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using Atum.Studio.Core.Managers;
using OpenTK;
using Atum.Studio.Controls.NewGui.SupportContact;

namespace Atum.Studio.Controls.NewGui.SplashControl
{
    public partial class SplashControl : UserControl
    {
        internal event EventHandler SplashSkipWelcomClicked;

        private SplashFrm _splashFrm = null;
        public SplashControl(SplashFrm splashFrm)
        {
            _splashFrm = splashFrm;
            InitializeComponent();

            this.pbContact.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.contact_white, this.pbContact.Size);
            lblOperatorStation.Text = string.Format(BrandingManager.MainForm_Title, string.Empty, string.Empty, string.Empty).Replace(".", string.Empty);

            if (FontManager.Loaded)
            {
                this.lblOperatorStation.Font = new Font(FontManager.MontserratRegular, 24, FontStyle.Regular, GraphicsUnit.Pixel);

                this.lblDaysTrailLeft.Top = (this.plTrail.Height / 2) - (this.lblDaysTrailLeft.Height / 2);
                this.lblDaysTrailLeft.Font = this.plDaysLeft.Font = this.lblLinkAuthorize.Font = this.lblDaysTrailRight.Font = FontManager.Montserrat14Regular;
                this.lblDaysTrailLeft.Width = TextRenderer.MeasureText(this.lblDaysTrailLeft.Text, this.lblDaysTrailLeft.Font).Width + 2;
                this.plDaysLeft.Left = this.lblDaysTrailLeft.Width + this.lblDaysTrailLeft.Left;
                this.lblDaysTrailRight.Top = (this.plTrail.Height / 2) - (this.lblDaysTrailRight.Height / 2);
                this.lblDaysTrailRight.Left = this.plDaysLeft.Left + this.plDaysLeft.Width + 2;
                this.lblDaysTrailRight.Width = TextRenderer.MeasureText(this.lblDaysTrailRight.Text, this.lblDaysTrailRight.Font).Width + 2;
                this.lblLinkAuthorize.Left = this.lblDaysTrailRight.Width + this.lblDaysTrailRight.Left;
                this.lblLinkAuthorize.Top = (this.plTrail.Height /2)  - (this.lblLinkAuthorize.Height / 2);
            }

            this.plDaysLeft.BackColor = BrandingManager.Button_HighlightColor;
            this.pbLogo.BackColor = Color.White;
            this.pbLogo.BackgroundImage = BrandingManager.Splash_Logo;

#if LOCTITE
            this.pbLogo.Left -= 10;
            this.pbLogo.Top -= 20;
            this.pbLogo.Width = this.pbLogo.Width;
            this.pbLogo.Height = this.pbLogo.Height;
#else
            this.pbLogo.Left -= 10;
            this.pbLogo.Top -= 7;
            this.pbLogo.Width = this.pbLogo.Width;
            this.pbLogo.Height = this.pbLogo.Height;
#endif

            SetDaysLeft();
        }

        private void SetDaysLeft()
        {
            if (_splashFrm.IsTrailLicense)
            {
                this.lblDaysTrailLeft.Width = TextRenderer.MeasureText(this.lblDaysTrailLeft.Text, this.lblDaysTrailLeft.Font).Width;
                this.plDaysLeft.Left = this.lblDaysTrailLeft.Left + this.lblDaysTrailLeft.Width;
                this.lblDaysTrailRight.Left = this.plDaysLeft.Left + this.plDaysLeft.Width;
                this.lblLinkAuthorize.Left = this.lblDaysTrailRight.Left + this.lblDaysTrailRight.Width;

                SetDaysLeftLabel();
                if (_splashFrm.DaysLeft < 0)
                {
                    this.plDaysLeft.Text = "0";
                    lblskipwelcome.Visible = false;
                }
            }
            else if (_splashFrm.IsStandardLicense && _splashFrm.DaysLeft < 60)
            {
                this.lblDaysTrailLeft.Text = "License will expire in ";
                this.lblDaysTrailLeft.Width = TextRenderer.MeasureText(this.lblDaysTrailLeft.Text, this.lblDaysTrailLeft.Font).Width;
                this.plDaysLeft.Left = this.lblDaysTrailLeft.Left + this.lblDaysTrailLeft.Width;
                this.lblDaysTrailRight.Left = this.plDaysLeft.Left + this.plDaysLeft.Width;
                this.lblLinkAuthorize.Text = "Renew now?";
                this.lblLinkAuthorize.Left = this.lblDaysTrailRight.Left + this.lblDaysTrailRight.Width;

                SetDaysLeftLabel();
                if (_splashFrm.DaysLeft < 0)
                {
                    this.plDaysLeft.Text = "0";
                    lblskipwelcome.Visible = false;
                }
            }
            else
            {
                plTrail.Visible = false;
            }
        }

        private void SetDaysLeftLabel()
        {
            if (_splashFrm.DaysLeft < 0)
            {
                
            }
            else
            {
                
            }
            this.plDaysLeft.Text = _splashFrm.DaysLeft.ToString();
        }

        internal void HideSkipWelcomeScreen()
        {
            this.lblskipwelcome.Visible = false;
        }

        public string AssemblyVersion
        {
            get
            {
                Assembly entryAssembly = Assembly.GetEntryAssembly();
                AssemblyFileVersionAttribute informationalVersionAttribute = entryAssembly.GetCustomAttribute<AssemblyFileVersionAttribute>();

                return informationalVersionAttribute.Version;
            }
        }
        
        private void lblContact_Click(object sender, EventArgs e)
        {
            using (var supportAboutPopup = new frmContact())
            {
                supportAboutPopup.ShowDialog();
            }
        }

        private void pbContact_Click(object sender, EventArgs e)
        {
            using (var supportAboutPopup = new frmContact())
            {
                supportAboutPopup.ShowDialog();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Graphics graphics = e.Graphics;
            System.Drawing.Rectangle gradient_rectangle = new System.Drawing.Rectangle(0, 0, plGradient.Width, plGradient.Height);
            
            System.Drawing.Brush b = new System.Drawing.Drawing2D.LinearGradientBrush(gradient_rectangle, Color.FromArgb(0,0,0,0), Color.Black, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
            
            graphics.FillRectangle(b, gradient_rectangle);
        }

        private void lblskipwelcome_Click(object sender, EventArgs e)
        {
            UserProfileManager.UserProfile.Settings_Skip_Welcome_Screen_On_Next_Start = true;
            UserProfileManager.Save();
            SplashSkipWelcomClicked?.Invoke(null, null);
        }

        //private void lblskipwelcome_Click_1(object sender, EventArgs e)
        //{
        //    //Core.Managers.UserProfileManager.UserProfile.Settings_Skip_Welcome_Screen_On_Next_Start = true;
        //    //InitializeMainForm();
        //}

        //private void InitializeMainForm()
        //{
        //    var mainForm = new frmStudioMain();
        //    mainForm.Load += new EventHandler(mainForm_Load);
        //    MainFormManager.Start(mainForm);
        //    MainFormManager.ProcesArguments(Environment.GetCommandLineArgs());

        //    var toolkitOptions = new ToolkitOptions();
        //    toolkitOptions.Backend = PlatformBackend.PreferNative;
        //    Toolkit.Init(toolkitOptions);

        //    mainForm.ShowDialog();
        //}

        //private void mainForm_Load(object sender, EventArgs e)
        //{
        //    if (SplashFrm == null)
        //    {
        //        return;
        //    }
        //    SplashFrm.Hide();
        //}
    }
}
