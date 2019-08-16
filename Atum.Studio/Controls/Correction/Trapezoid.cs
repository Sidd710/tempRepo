using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OpenTK;

namespace Atum.Studio.Controls.Correction
{
    public partial class Trapezoid : UserControl
    {
        private DAL.Hardware.AtumPrinter _selectedPrinter;
        private bool _initialized;

        public DAL.Hardware.AtumPrinter SelectedPrinter
        {
            get
            {
                if (this._selectedPrinter != null)
                {
                    this._selectedPrinter.TrapeziumCorrectionSideA = (float)this.txtLeft.Value;
                    this._selectedPrinter.TrapeziumCorrectionSideB = (float)this.txtLower.Value;
                    this._selectedPrinter.TrapeziumCorrectionSideC = (float)this.txtRight.Value;
                    this._selectedPrinter.TrapeziumCorrectionSideD = (float)this.txtUpper.Value;
                    this._selectedPrinter.TrapeziumCorrectionSideE = (float)this.txtUpperLeftLowerRight.Value;
                    this._selectedPrinter.TrapeziumCorrectionSideF = (float)this.txtLowerLeftUpperRight.Value;
                }
                return this._selectedPrinter;
            }
            set
            {
                if (value != null)
                {
                    this._selectedPrinter = value;

                    this.txtLeft.Value = (decimal)Math.Round(this._selectedPrinter.TrapeziumCorrectionSideA, 3);
                    this.txtLower.Value = (decimal)Math.Round(this._selectedPrinter.TrapeziumCorrectionSideB, 3);
                    this.txtRight.Value = (decimal)Math.Round(this._selectedPrinter.TrapeziumCorrectionSideC, 3);
                    this.txtUpper.Value = (decimal)Math.Round(this._selectedPrinter.TrapeziumCorrectionSideD, 3);
                    this.txtUpperLeftLowerRight.Value = (decimal)Math.Round(this._selectedPrinter.TrapeziumCorrectionSideE, 3);
                    this.txtLowerLeftUpperRight.Value = (decimal)Math.Round(this._selectedPrinter.TrapeziumCorrectionSideF, 3);

                    this._initialized = true;
                }
            }
        }

        public Trapezoid()
        {
            InitializeComponent();
        }

        private void txtLowerLeftUpperRight_ValueChanged(object sender, EventArgs e)
        {
            CalcSideC();
        }

        void CalcSideC()
        {
            if (this._initialized)
            {
                var pointB = new Vector2(CalcX(SelectedPrinter.TrapeziumCorrectionSideA, SelectedPrinter.TrapeziumCorrectionSideB, SelectedPrinter.TrapeziumCorrectionSideE),
                    CalcY(SelectedPrinter.TrapeziumCorrectionSideA, SelectedPrinter.TrapeziumCorrectionSideB, SelectedPrinter.TrapeziumCorrectionSideE));
                var pointC = new Vector2(CalcX(SelectedPrinter.TrapeziumCorrectionSideA, SelectedPrinter.TrapeziumCorrectionSideD, SelectedPrinter.TrapeziumCorrectionSideF),
                    CalcY(SelectedPrinter.TrapeziumCorrectionSideA, SelectedPrinter.TrapeziumCorrectionSideD, SelectedPrinter.TrapeziumCorrectionSideF));

                pointB.Y = -pointB.Y;

                if (pointC.X != float.NaN)
                {
                    pointC.Y += this.SelectedPrinter.TrapeziumCorrectionSideA;

                    if (!float.IsNaN(pointB.X))
                    {
                        this.txtRight.Value = (decimal)(pointC - pointB).Length;
                    }
                }
            }

        }

        float CalcY(float sideAC, float sideAB, float sideBC)
        {
            float x = ((sideAC * sideAC + sideAB * sideAB - sideBC * sideBC) / (2 * sideAB));
            float h = (float)Math.Sqrt((sideAC * sideAC) - (x * x));
            float angleA = (float)(Math.Atan(h / x));
            if (angleA < 0) { angleA = (float)(angleA + Math.PI); }
            if (angleA > Math.PI) { angleA = (float)(angleA - Math.PI); }
            angleA = (float)Math.Round(MathHelper.RadiansToDegrees(angleA), 3);
            float pointB_Y = (float)(sideAB * Math.Sin(MathHelper.DegreesToRadians(angleA - 90)));
            return (float)Math.Round(pointB_Y, 3);

        }

        static float CalcX(float sideAC, float sideAB, float sideBC)
        {
            var x = ((sideAC * sideAC + sideAB * sideAB - sideBC * sideBC) / (2 * sideAB));
            var h = Math.Sqrt((sideAC * sideAC) - (x * x));
            var angleA = (Math.Atan(h / x));
            if (angleA < 0) { angleA = angleA + Math.PI; }
            if (angleA > Math.PI) { angleA = angleA - Math.PI; }
            angleA = MathHelper.RadiansToDegrees(angleA);
            var pointB_X = sideAB * Math.Cos(MathHelper.DegreesToRadians(angleA - 90));

            return (float)Math.Round(pointB_X, 3);

        }

        private void txtUpper_ValueChanged(object sender, EventArgs e)
        {
            CalcSideC();
        }

        private void txtLeft_ValueChanged(object sender, EventArgs e)
        {
            CalcSideC();
        }

        private void txtLower_ValueChanged(object sender, EventArgs e)
        {
            CalcSideC();
        }

        private void txtUpperLeftLowerRight_ValueChanged(object sender, EventArgs e)
        {
            CalcSideC();
        }

        internal void CalcUpperRight()
        {
            this.txtLowerLeftUpperRight.Value = (decimal)Math.Round(Math.Sqrt((this.SelectedPrinter.TrapeziumCorrectionSideA * this.SelectedPrinter.TrapeziumCorrectionSideA) +
                (this.SelectedPrinter.TrapeziumCorrectionSideD * this.SelectedPrinter.TrapeziumCorrectionSideD)), 3);
        }

        internal void CalcLowerRight()
        {
            this.txtUpperLeftLowerRight.Value = (decimal)Math.Round(Math.Sqrt((this.SelectedPrinter.TrapeziumCorrectionSideA * this.SelectedPrinter.TrapeziumCorrectionSideA) +
                (this.SelectedPrinter.TrapeziumCorrectionSideB * this.SelectedPrinter.TrapeziumCorrectionSideB)), 3);
        }

       

    }
}
