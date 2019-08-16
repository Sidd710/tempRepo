using Atum.Studio.Core.Models;
using Atum.Studio.Core.ModelView;
using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls.MagsAI
{
    public partial class frmMagsAIWizard2 : Form
    {
        private int _selectedIndex = 0;

        public frmMagsAIWizard2()
        {
            InitializeComponent();
        }

        private void frmMagsAIWizard2_Load(object sender, EventArgs e)
        {
            this.Icon = Core.Managers.BrandingManager.MainForm_Icon;

            this.btnBack.Visible = false;
            
            this.tbWizard.SelectedIndex = 0;
            this.trWizardSteps.Nodes[0].BackColor = this.trWizardSteps.BackColor;
            this.trWizardSteps.Nodes[0].ForeColor = Color.Black;
            this.btnBack.Visible = false;


            this.magsAIWelcomeTabPanel1.ModelLoaded += MagsAIWelcomeTabPanel1_ModelLoaded;

            for (var nodeIndex = 1; nodeIndex < this.trWizardSteps.Nodes.Count; nodeIndex++)
            {
                this.trWizardSteps.Nodes[nodeIndex].NodeFont = new Font(this.trWizardSteps.Font, FontStyle.Regular);
            }
        }

        private void MagsAIWelcomeTabPanel1_ModelLoaded(object sender, EventArgs e)
        {
            this.NextStep();
        }

        private void PreviousStep()
        {
            if (_selectedIndex > 0)
            {
                this.trWizardSteps.Nodes[_selectedIndex].NodeFont = new Font(this.trWizardSteps.Font, FontStyle.Regular);
                _selectedIndex--;
                this.trWizardSteps.Nodes[_selectedIndex].NodeFont = new Font(this.trWizardSteps.Font, FontStyle.Bold);

                this.tbWizard.SelectedIndex = _selectedIndex;

                this.btnNext.Text = "Next";
                this.btnBack.Visible = true;
            }

            if (_selectedIndex == 0)
            {
                this.btnBack.Visible = false;
            }
        }

        private void NextStep()
        {
            if (this.trWizardSteps.Nodes.Count > _selectedIndex - 1)
            {
                this.btnBack.Visible = true;
                this.trWizardSteps.Nodes[_selectedIndex].NodeFont = new Font(this.trWizardSteps.Font, FontStyle.Regular);
                _selectedIndex++;
                this.trWizardSteps.Nodes[_selectedIndex].NodeFont = new Font(this.trWizardSteps.Font, FontStyle.Bold);

                this.tbWizard.SelectedIndex = _selectedIndex;
            }

            if (this.trWizardSteps.Nodes.Count == _selectedIndex - 1)
            {
                this.btnNext.Text = "Finished";
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.NextStep();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.PreviousStep();
        }

        private void plFooter_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(plWizardSteps.BackColor, 1), 0, 0, 10000, 0);
        }

        private void trWizardSteps_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void tbWizard_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbWizard.SelectedTab == this.tbSelectOrientation)
            {
                this.magsAIOrientationTabPanel1.GLControl.Render();
            }
            else if (tbWizard.SelectedTab == this.tbPreview)
            {
                this.magsAIPreview1.GLControl.Render();

                var stlModel = ObjectView.Objects3D[1] as STLModel3D;
                Task.Run(() => Core.Engines.MagsAIEngine.Calculate(stlModel, this.magsAIMaterialTabPanel1.SelectedMaterial));

            }
        }

        private void tbWizard_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == this.tbPreview)
            {
                this.magsAIPreview1.GLControl = this.magsAIOrientationTabPanel1.GLControl;
                this.magsAIPreview1.GLControl.StopSelectionTimer();
            }
        }
    }
}
