using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls.NewGui
{
    public partial class NewGUIContentSplitContainerBase : UserControl
    {
     
        public Panel LeftPanel
        {
            get
            {
                return this.splitContainer1.Panel1;
            }
        }

        public Panel RightPanel
        {
            get
            {
                return this.splitContainer1.Panel2;
            }
        }
        //public PictureBox btnpicAdd
        // {
        //     get
        //     {
        //         return this.picAdd;
        //     }
        // }

        // public PictureBox btnremove
        // {
        //     get
        //     {
        //         return this.btnRemove;
        //     }
        // }
        public NewGUIContentSplitContainerBase()
        {
            InitializeComponent();
        }
    }
}