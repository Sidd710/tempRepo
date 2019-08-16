using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace Atum.Studio.Controls
{
    public class FloatTrackBar: TrackBar 
    { 
        [Browsable(true)]
        public float Precision { get;set; }

        public FloatTrackBar()
        {
            this.Precision = 0.1f;
        }

        public new float LargeChange { get { return base.LargeChange * this.Precision; } set { base.LargeChange = (int)(value / this.Precision); } }
        public new float Maximum { get { return base.Maximum * this.Precision; } set { base.Maximum = (int)(value / this.Precision); } }
        public new float Minimum { get { return base.Minimum * this.Precision; } set { base.Minimum = (int)(value / this.Precision); } }
        public new float SmallChange { get { return base.SmallChange * this.Precision; } set { base.SmallChange = (int)(value / this.Precision); } }
        public new float Value { get { return base.Value * this.Precision; } set { base.Value = (int)(value / this.Precision); } }
    }
}
