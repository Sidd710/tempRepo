using Atum.Studio.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace atum3D.OperatorStation.CatalogManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MaterialManager.Start(false);
            FontManager.LoadDefaultFonts();

            Application.Run(new Atum.Studio.Controls.NewGui.MaterialCatalogEditor.frmMaterialCatalogManager());
            
        }
    }
}
