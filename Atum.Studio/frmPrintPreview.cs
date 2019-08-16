using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Atum.DAL.Managers;
using System.Net;
using Atum.Studio.Controls;
using Atum.Studio.Core.ModelView;
using Atum.DAL.Compression.Zip;
using System.Net.NetworkInformation;
using System.IO;
using Atum.DAL.Hardware;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Xml.Serialization;
using System.Diagnostics;
using Atum.Studio.Core.Managers;
using Atum.DAL.Print;
using Atum.Studio.Core.Engines;

namespace Atum.Studio
{
    public partial class frmPrintPreview : Form
    {
        internal Controls.OpenGL.GLControl GLCONTROL;
        internal Panel _plWorkspace;

        private float _cameraZoom = 1f;
        private float _cameraPanX = 0f;
        private float _cameraPanY;
        private float _cameraRotationX;
        private float _cameraRotationZ;

        internal float ModelVolume { get; set; }

        public DAL.Print.PrintJob PrintJob { get; set; }
        
        public frmPrintPreview()
        {
            InitializeComponent();
        }

        public frmPrintPreview(PrintJob prj, frmStudioMain mainForm)
        {
            this.PrintJob = prj;
            this.GLCONTROL = frmStudioMain.SceneControl;
            this._plWorkspace = mainForm.plWorkSpace;
            GenerateThumbnails(null, null);
            UpdatePrintTimeStamp();
        }
        
        void UpdatePrintTimeStamp()
        {
            var printingTime = this.PrintJob.PrintingTimeRemaining(0, RenderEngine.TotalProcessedSlices);
         
            this.PrintJob.JobTimeInSeconds = printingTime.TotalSeconds;
         
        }

        #region OPENGL screenshot
        private void GenerateThumbnails(object sender, EventArgs e)
        {
            //preview thumbnail
            //this.Width = this.Height = 128;
            GLCONTROL.Dock = DockStyle.None;
            GLCONTROL.Width = GLCONTROL.Height = 128;
            this.Controls.Add(GLCONTROL);

            GL.Enable(EnableCap.CullFace);
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.LineSmooth);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.PolygonSmooth);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
            GL.Enable(EnableCap.Light1);
            GL.Light(LightName.Light1, LightParameter.Position, new Vector4(0.0f, 0.0f, 500f, 1f));
            GL.Light(LightName.Light1, LightParameter.Ambient, new Color4()
            {
                R = 0.35f,
                G = 0.35f,
                B = 0.35f,
                A = 1f
            });
            GL.Light(LightName.Light1, LightParameter.Diffuse, new Color4()
            {
                R = 0.2f,
                G = 0.2f,
                B = 0.2f,
                A = 1f
            });
            GL.Light(LightName.Light1, LightParameter.Specular, new Color4()
            {
                R = 0.25f,
                G = 0.25f,
                B = 0.25f,
                A = 1f
            });
            float[] params1 = new float[4] { 1f, 1f, 1f, 1f };
            float[] params2 = new float[4]
            {
        1f,
        1f,
        1f,
        0.1f
            };
            float[] params3 = new float[4] { 5f, 5f, 5f, 1f };
            GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, params1);
            GL.Material(MaterialFace.Front, MaterialParameter.Specular, params2);
            GL.Material(MaterialFace.Front, MaterialParameter.Shininess, params3);
            this.FitModelToScreen();
            this.Render(false);
            Bitmap screenshot = this.CreateScreenshot();
            this.PrintJob.Thumbnail = (byte[])new ImageConverter().ConvertTo((object)screenshot, typeof(byte[]));
            //this.picPrintjobThumbnail.BackgroundImage = (Image)screenshot;

            //large thumbnail
            GLCONTROL.Width = 800;
            GLCONTROL.Height = 480;
            this.FitModelToLargeScreen();

            this.Render(true);
            screenshot = this.CreateScreenshot();
            this.PrintJob.ThumbnailLarge = (byte[])new ImageConverter().ConvertTo((object)screenshot, typeof(byte[]));

            this.Controls.Remove(this.GLCONTROL);
            this._plWorkspace.Controls.Add(this.GLCONTROL);
            this.GLCONTROL.Dock = DockStyle.Fill;

        }

        internal void Render(bool largeThumbnail)
        {
            lock (this)
            {
                if (GLCONTROL.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate { GLCONTROL.MakeCurrent(); }));
                    this._cameraPanY = 200; //default 50 micron

                    switch (this.PrintJob.SelectedPrinter.PrinterXYResolution)
                    {
                        case AtumPrinter.PrinterXYResolutionType.Micron75:
                            break;
                        case AtumPrinter.PrinterXYResolutionType.Micron100:
                            this._cameraZoom = 100; //change zoom for 100 micron
                            break;
                    }

                    SceneView.RenderAsModelView(GLCONTROL, this._cameraRotationX, this._cameraRotationZ, this._cameraZoom, this._cameraPanX, this._cameraPanY, true, largeThumbnail);
                    ObjectView.DrawObjects(GLCONTROL, this.PrintJob.Material.SupportProfiles[0], true);
                    GLCONTROL.SwapBuffers();
                }
                else
                {
                    GLCONTROL.MakeCurrent();
                    this._cameraPanY = 200; //default 50 micron

                    switch (this.PrintJob.SelectedPrinter.PrinterXYResolution)
                    {
                        case AtumPrinter.PrinterXYResolutionType.Micron75:
                            break;
                        case AtumPrinter.PrinterXYResolutionType.Micron100:
                            this._cameraZoom = 100; //change zoom for 100 micron
                            break;
                    }

                    SceneView.RenderAsModelView(GLCONTROL, this._cameraRotationX, this._cameraRotationZ, this._cameraZoom, this._cameraPanX, this._cameraPanY, true, largeThumbnail);
                    ObjectView.DrawObjects(GLCONTROL, this.PrintJob.Material.SupportProfiles[0], true);
                   // GLCONTROL.SwapBuffers();
                }

            }
        }

        internal void FitModelToScreen()
        {
            this.MoveCameraToPosition("0,270,0");
        }

        internal void FitModelToLargeScreen()
        {
            this.MoveCameraToPosition("135, 270, 0");
        }

        internal void MoveCameraToPosition(string positionTag)
        {
            int num1 = 50;
            string str = positionTag;
            float num2 = float.Parse(str.Split(',')[0]);
            float num3 = float.Parse(str.Split(',')[1]);
            if ((double)num2 - (double)this._cameraRotationX > 180.0)
                num2 = (float)-((double)num2 - 180.0);
            if ((double)num3 - (double)this._cameraRotationZ > 180.0)
                num3 = (float)-((double)num3 - 180.0);
            float num4 = num2 - this._cameraRotationX;
            float num5 = num3 - this._cameraRotationZ;
            if ((double)num4 < -180.0)
                num4 = 360f + num4;
            if ((double)num5 < -180.0)
                num5 = 360f + num5;
            float num6 = num4 / (float)num1;
            float num7 = num5 / (float)num1;
            if ((double)num6 == 0.0 && (double)num7 == 0.0)
                return;
            for (int index = 0; index < num1; ++index)
            {
                this._cameraRotationX = this._cameraRotationX + num6;
                this._cameraRotationZ = this._cameraRotationZ + num7;
            }
        }

       
        internal Bitmap CreateScreenshot()
        {
            Size clientSize = GLCONTROL.ClientSize;
            int width = clientSize.Width;
            clientSize = GLCONTROL.ClientSize;
            int height = clientSize.Height;
            Bitmap bitmap = new Bitmap(width, height);
            GLCONTROL.Invalidate();
            this.Render(false);
            GLCONTROL.Invalidate();
            this.Render(false);
            Rectangle clientRectangle = GLCONTROL.ClientRectangle;
            int num1 = 2;
            int num2 = 137224;
            BitmapData bitmapData = bitmap.LockBits(clientRectangle, (ImageLockMode)num1, (System.Drawing.Imaging.PixelFormat)num2);
            GL.ReadPixels(0, 0, GLCONTROL.Width, GLCONTROL.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, bitmapData.Scan0);
            BitmapData bitmapdata = bitmapData;
            bitmap.UnlockBits(bitmapdata);
            int num3 = 6;
            bitmap.RotateFlip((RotateFlipType)num3);
            return bitmap;
        }
        #endregion

    }
}
