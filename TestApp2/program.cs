using OpenTK.Platform;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace TestApp2
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

            try
            {
                Application.Run(new frmMain());

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

    }
}
