using System;
using System.Windows.Forms;
using OpenTK;
using System.Diagnostics;

namespace Atum.Studio.Controls.Correction
{
    public partial class Basic : UserControl
    {
       // internal event Action<bool> DirtyStateChanged;

        private bool _isDirty;
        
        public bool IsDirty
        {
            get { return this._isDirty; }
            set
            {
                if (this._initialized)
                {
                    this._isDirty = value;
                    this._selectedPrinter.IsDirty = true;
             //       this.DirtyStateChanged?.Invoke(value);
                }
            }
        }

        private DAL.Hardware.AtumPrinter _selectedPrinter;
        private bool _initialized;

        private float _defaultGroundPaneX;
        private float _defaultGroundPaneY;

        private float _calibrationModelSideX;
        private float _calibrationModelSideY;

        private float _previousTrapeziumCorrectionInputA;
        private float _previousTrapeziumCorrectionInputB;
        private float _previousTrapeziumCorrectionInputC;
        private float _previousTrapeziumCorrectionInputD;
        private float _previousTrapeziumCorrectionInputE;
        private float _previousTrapeziumCorrectionInputF;

        public bool AdvancedMode
        {
            get
            {
                return this.txtUpperLeftLowerRight.Visible;
            }
            set
            {
                this.txtLowerLeftUpperRight.Visible = this.txtUpperLeftLowerRight.Visible = value;
            }
        }

        public DAL.Hardware.AtumPrinter SelectedPrinter
        {
            get
            {
                if (this._selectedPrinter != null)
                {

                    if ((float)this.txtLeft.Value != this._previousTrapeziumCorrectionInputA)
                    {
                        this._selectedPrinter.TrapeziumCorrectionInputA = ((float)this.txtLeft.Value / this._calibrationModelSideY) * this._previousTrapeziumCorrectionInputA;
                        this._selectedPrinter.TrapeziumCorrectionSideA = (this._selectedPrinter.TrapeziumCorrectionInputA / this._calibrationModelSideY) * this._defaultGroundPaneY;
                    }

                    if ((float)this.txtLower.Value != this._previousTrapeziumCorrectionInputB)
                    {
                        this._selectedPrinter.TrapeziumCorrectionInputB = ((float)this.txtLower.Value / this._calibrationModelSideX) * this._previousTrapeziumCorrectionInputB;
                        this._selectedPrinter.TrapeziumCorrectionSideB = (this._selectedPrinter.TrapeziumCorrectionInputB / this._calibrationModelSideX) * this._defaultGroundPaneX;
                    }

                    if ((float)this.txtRight.Value != this._previousTrapeziumCorrectionInputC)
                    {
                        this._selectedPrinter.TrapeziumCorrectionInputC = ((float)this.txtRight.Value / this._calibrationModelSideY) * this._previousTrapeziumCorrectionInputC;
                        this._selectedPrinter.TrapeziumCorrectionSideC = (this._selectedPrinter.TrapeziumCorrectionInputC / this._calibrationModelSideY) * this._defaultGroundPaneY;
                    }

                    if ((float)this.txtUpper.Value != this._previousTrapeziumCorrectionInputD)
                    {
                        this._selectedPrinter.TrapeziumCorrectionInputD = ((float)this.txtUpper.Value / this._calibrationModelSideX) * this._previousTrapeziumCorrectionInputD;
                        this._selectedPrinter.TrapeziumCorrectionSideD = (this._selectedPrinter.TrapeziumCorrectionInputD / this._calibrationModelSideX) * this._defaultGroundPaneX;
                    }

                    if (this._previousTrapeziumCorrectionInputE != (float)this.txtUpperLeftLowerRight.Value)
                    {
                        //calc percentage difference in diagonals
                        var unScaledTrapeziumInputE = (float)this.txtUpperLeftLowerRight.Value;
                        var unScaledTrapeziumInputF = (float)this.txtLowerLeftUpperRight.Value;

                        //var previous equal diagonals 
                        this.txtUpperLeftLowerRight.ValueChanged -= txtUpperLeftLowerRight_ValueChanged;
                        CalcDiagonals((float)this.txtLeft.Value, (float)this.txtLower.Value, (float)this.txtRight.Value, (float)this.txtUpper.Value);

                        var previousInputEPercentage = unScaledTrapeziumInputE / (float)this.txtUpperLeftLowerRight.Value;
                        var previousInputFPercentage = unScaledTrapeziumInputF / (float)this.txtLowerLeftUpperRight.Value;

                        //calc new diagonals
                        CalcDiagonals(this._selectedPrinter.TrapeziumCorrectionInputA, this._selectedPrinter.TrapeziumCorrectionInputB, this._selectedPrinter.TrapeziumCorrectionInputC, this._selectedPrinter.TrapeziumCorrectionInputD);

                        this._selectedPrinter.TrapeziumCorrectionInputE = (float)this.txtUpperLeftLowerRight.Value * previousInputEPercentage;
                        this._selectedPrinter.TrapeziumCorrectionInputF = (float)this.txtLowerLeftUpperRight.Value * previousInputFPercentage;
                    }
                }
                return this._selectedPrinter;
            }
            set
            {
                if (value != null)
                {
                    this._initialized = false;
                    this._selectedPrinter = value;

                    this._previousTrapeziumCorrectionInputA = this._selectedPrinter.TrapeziumCorrectionInputA;
                    this._previousTrapeziumCorrectionInputB = this._selectedPrinter.TrapeziumCorrectionInputB;
                    this._previousTrapeziumCorrectionInputC = this._selectedPrinter.TrapeziumCorrectionInputC;
                    this._previousTrapeziumCorrectionInputD = this._selectedPrinter.TrapeziumCorrectionInputD;
                    this._previousTrapeziumCorrectionInputE = this._selectedPrinter.TrapeziumCorrectionInputE;
                    this._previousTrapeziumCorrectionInputF = this._selectedPrinter.TrapeziumCorrectionInputF;


                    if (this._selectedPrinter is DAL.Hardware.AtumDLPStation5 || this._selectedPrinter is DAL.Hardware.LoctiteV10)
                    {
                        this._defaultGroundPaneX = 96f;
                        this._defaultGroundPaneY = 54;

                        switch (this._selectedPrinter.PrinterXYResolution)
                        {
                            case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron50:
                                this._defaultGroundPaneX = 96f;
                                this._defaultGroundPaneY = 54f;

                                this._calibrationModelSideX = 80;
                                this._calibrationModelSideY = 40;

                                break;
                            case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron75:
                                this._defaultGroundPaneX = 144;
                                this._defaultGroundPaneY = 81f;

                                this._calibrationModelSideX = 120; //120
                                this._calibrationModelSideY = 60; //75

                                break;
                            case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron100:
                                this._defaultGroundPaneX = 192f;
                                this._defaultGroundPaneY = 108f;

                                this._calibrationModelSideX = 160;
                                this._calibrationModelSideY = 80;

                                break;
                        }
                    }
                    else
                    {
                        this._defaultGroundPaneX = 96f;
                        this._defaultGroundPaneY = 60f;

                        switch (this._selectedPrinter.PrinterXYResolution)
                        {
                            case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron40:
                                this._defaultGroundPaneX = 76.8f;
                                this._defaultGroundPaneY = 48f;

                                this._calibrationModelSideX = 50;
                                this._calibrationModelSideY = 30;
                                break;
                            case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron50:
                                this._defaultGroundPaneX = 96f;
                                this._defaultGroundPaneY = 60f;

                                this._calibrationModelSideX = 80;
                                this._calibrationModelSideY = 50;

                                break;
                            case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron75:
                                this._defaultGroundPaneX = 144;
                                this._defaultGroundPaneY = 90f;

                                this._calibrationModelSideX = 120; //120
                                this._calibrationModelSideY = 75; //75

                                break;
                            case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron100:
                                this._defaultGroundPaneX = 192f;
                                this._defaultGroundPaneY = 120f;

                                this._calibrationModelSideX = 160;
                                this._calibrationModelSideY = 100;

                                break;
                        }
                    }

                    this.txtLeft.Value = (decimal)Math.Round(this._selectedPrinter.TrapeziumCorrectionInputA, 3);
                    this.txtLower.Value = (decimal)Math.Round(this._selectedPrinter.TrapeziumCorrectionInputB, 3);
                    this.txtRight.Value = (decimal)Math.Round(this._selectedPrinter.TrapeziumCorrectionInputC, 3);
                    this.txtUpper.Value = (decimal)Math.Round(this._selectedPrinter.TrapeziumCorrectionInputD, 3);
                    this.txtUpperLeftLowerRight.Value = (decimal)Math.Round(this._selectedPrinter.TrapeziumCorrectionInputE, 3);
                    this.txtLowerLeftUpperRight.Value = (decimal)Math.Round(this._selectedPrinter.TrapeziumCorrectionInputF, 3);

                    this._initialized = true;

                }
            }
        }

        public Basic()
        {
            InitializeComponent();
        }


        float CalcY(float sideAC, float sideAB, float sideBC)
        {
            float x = ((sideAC * sideAC + sideAB * sideAB - sideBC * sideBC) / (2 * sideAB));
            float h = (float)Math.Sqrt((sideAC * sideAC) - (x * x));
            float angleA = (float)(Math.Atan(h / x));
            if (angleA < 0) { angleA = (float)(angleA + Math.PI); }
            if (angleA > Math.PI) { angleA = (float)(angleA - Math.PI); }
            angleA = (float)Math.Round(MathHelper.RadiansToDegrees(angleA), 3);
            float pointB_Y = (float)((float)sideAB * Math.Sin(MathHelper.DegreesToRadians(angleA - 90)));
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

            return (float)Math.Round(pointB_X, 4);

        }

        private void txtLeft_ValueChanged(object sender, EventArgs e)
        {
            if (this._initialized)
            {
                CalcDiagonals((float)this.txtLeft.Value, (float)this.txtLower.Value, (float)this.txtRight.Value, (float)this.txtUpper.Value);

                this.IsDirty = true;
            }
        }

        private void txtLower_ValueChanged(object sender, EventArgs e)
        {
            if (this._initialized)
            {
                CalcDiagonals((float)this.txtLeft.Value, (float)this.txtLower.Value, (float)this.txtRight.Value, (float)this.txtUpper.Value);

                this.IsDirty = true;
            }
        }

        private void CalcDiagonals(float sideA, float sideB, float sideC, float sideD)
        {
            if (this._initialized)
            {
                //first calc side upper left -> right down
                var diagonal = (float)Math.Round(Math.Sqrt((sideA * sideA) +
                    (float)(sideD * sideD)), 4);


                var pointB = new Vector2(CalcX(sideA, sideB, diagonal),
                                    CalcY(sideA, sideB, diagonal));
                var pointC = new Vector2(CalcX(sideA, sideD, diagonal),
                    CalcY(sideA, sideD, diagonal));

                pointB.Y = -pointB.Y;

                if (pointC.X != Single.NaN)
                {
                    pointC.Y += sideA;

                    if (!Single.IsNaN(pointB.X))
                    {
                        var approxSideC = (pointC - pointB).Length;
                        if (approxSideC < sideC)
                        {
                            while (approxSideC < sideC)
                            {
                                diagonal += 0.0001f;
                                pointB = new Vector2(CalcX(sideA, sideB, diagonal),
                                           CalcY(sideA, sideB, diagonal));
                                pointC = new Vector2(CalcX(sideA, sideD, diagonal),
                                    CalcY(sideA, sideD, diagonal));

                                pointB.Y = -pointB.Y;

                                if (pointC.X != Single.NaN)
                                {
                                    pointC.Y += sideA;

                                    if (!Single.IsNaN(pointB.X))
                                    {
                                        approxSideC = (pointC - pointB).Length;
                                        // Debug.WriteLine(sideC);
                                    }
                                }
                            }
                        }
                        else if (approxSideC > sideC)
                        {
                            while (approxSideC > sideC)
                            {
                                diagonal -= 0.0001f;
                                pointB = new Vector2(CalcX(sideA, sideB, diagonal),
                                           CalcY(sideA, sideB, diagonal));
                                pointC = new Vector2(CalcX(sideA, sideD, diagonal),
                                    CalcY(sideA, sideD, diagonal));

                                pointB.Y = -pointB.Y;

                                if (pointC.X != Single.NaN)
                                {
                                    pointC.Y += sideA;

                                    if (!Single.IsNaN(pointB.X))
                                    {
                                        approxSideC = (pointC - pointB).Length;
                                        //Debug.WriteLine(sideC);
                                    }
                                }
                            }
                        }

                        this.txtLowerLeftUpperRight.Value = this.txtUpperLeftLowerRight.Value = (decimal)diagonal;
                    }
                }
            }
        }

        private void txtUpper_ValueChanged(object sender, EventArgs e)
        {
            CalcDiagonals((float)this.txtLeft.Value, (float)this.txtLower.Value, (float)this.txtRight.Value, (float)this.txtUpper.Value);

            this.IsDirty = true;
        }

        private void txtRight_ValueChanged(object sender, EventArgs e)
        {
            CalcDiagonals((float)this.txtLeft.Value, (float)this.txtLower.Value, (float)this.txtRight.Value, (float)this.txtUpper.Value);

            this.IsDirty = true;
        }

        internal void CalcSideF(float sideA, float sideB, float sideC, float sideD, float sideE)
        {
            try
            {
                if (sideA > 0 && sideB > 0 && sideC > 0 && sideD > 0)
                {
                    var sideABAngle = MathHelper.RadiansToDegrees(Math.Acos((Math.Pow(sideA, 2) + Math.Pow(sideB, 2) - Math.Pow(sideE, 2)) / (2 * sideA * sideB)));
                    var sideAD1Angle = MathHelper.RadiansToDegrees(Math.Acos((Math.Pow(sideE, 2) + Math.Pow(sideA, 2) - Math.Pow(sideB, 2)) / (2 * sideE * sideA)));
                    var sideAD2Angle = MathHelper.RadiansToDegrees(Math.Acos((Math.Pow(sideE, 2) + Math.Pow(sideD, 2) - Math.Pow(sideC, 2)) / (2 * sideE * sideD)));

                    var sideADAngle = sideAD1Angle + sideAD2Angle;
                    var sideADAngleInRadians = MathHelper.DegreesToRadians(sideADAngle);
                    var sideFFactor = Math.Cos(sideADAngleInRadians) * 2 * (double)this.txtLeft.Value * (double)this.txtUpper.Value;
                    sideFFactor -= Math.Pow((double)this.txtUpper.Value, 2) + Math.Pow((double)this.txtLeft.Value, 2);
                    sideFFactor = sideFFactor < 0 ? -sideFFactor : sideFFactor;
                    this.txtLowerLeftUpperRight.Value = (decimal)Math.Sqrt(sideFFactor);
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
        }

        private void txtUpperLeftLowerRight_ValueChanged(object sender, EventArgs e)
        {
            CalcSideF((float)this.txtLeft.Value, (float)this.txtLower.Value, (float)this.txtRight.Value, (float)this.txtUpper.Value, (float)this.txtUpperLeftLowerRight.Value);

            this.IsDirty = true;
        }
    }
}
