using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using System.Net.Sockets;
using System.IO;
using System.Net;
using Poly2Tri;
using System.Drawing.Drawing2D;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Controls;
using OpenTK;
using System.Drawing.Design;
using System.Reflection;
using System.Collections;
using Atum.Studio.Core.Models;
using Atum.Studio;
using System.Security.Cryptography;
using Atum.DAL.Network;
using Atum.DAL.Hardware;

namespace TestApp
{


    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var test = new Atum.Studio.Core.Managers.LicenseManager();
            test.Test(Guid.NewGuid().ToString());
        }

        //void AtumPrinterRemoteControlDisplayReceived(Atum.DAL.Remoting.RemoteControlDisplay remoteDisplay)
        //{
        //    if (remoteDisplay != null)
        //    {
        //        Console.WriteLine(remoteDisplay.ToString());
        //    }
        //}

        private void Form1_Load(object sender, EventArgs e)
        {
            ConnectionManager.Start(11000, false);
         //   ConnectionManager.AtumPrinterRemoteControlDisplayReceived += AtumPrinterRemoteControlDisplayReceived;
         //   var actions = new Atum.DAL.Remoting.RemoteControlActions();
         //   actions.Add(new Atum.DAL.Remoting.RemoteControlAction() { NavigationButtonAction = Atum.DAL.Remoting.RemoteControlAction.NavigationButtonActionType.CurrentLCDMenu, RemoteHostIP = "127.0.0.1" });
         //   ConnectionManager.Send(actions, IPAddress.Parse("127.0.0.1"), 11002);

            //var test = new Atum.Studio.Core.Managers.LicenseManager();
            //RSA rsa = new RSACryptoServiceProvider(2048); // Generate a new 2048 bit RSA key

            //string publicPrivateKeyXML = rsa.ToXmlString(true);
            //string publicOnlyKeyXML = rsa.ToXmlString(false);
            //test.Test(publicPrivateKeyXML);
        }

        private void txtUpper_ValueChanged(object sender, EventArgs e)
        {

        }

        private void cloneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ConnectionManager.Send(new PrinterFirmware(), IPAddress.Parse("127.0.0.1"), 11000);

        }





        //void Lines_ListChanged(object sender, ListChangedEventArgs e)
        //{
        //    if (LogReader.LastStreamOffset > 0)
        //    {
        //        //this.dataGridView1.FirstDisplayedScrollingRowIndex = this.dataGridView1.RowCount -1;
        //    }
        //}

    }


}
