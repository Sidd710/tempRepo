﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Atum.Studio.Controls.ToolstripItems
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripTrackBarItem : ToolStripControlHost
    {
        public ToolStripTrackBarItem()
        : base(CreateControlInstance())
    {
        }
        /// <summary>
        /// Create a strongly typed property called TrackBar - handy to prevent casting everywhere.
        /// </summary>
        public TrackBar TrackBar
        {
            get
            {
                return Control as TrackBar;
            }
        }
        /// <summary>
        /// Create the actual control, note this is static so it can be called from the
        /// constructor.
        ///
        /// </summary>
        /// <returns></returns>
        private static Control CreateControlInstance()
        {
            TrackBar t = new TrackBar();
            t.AutoSize = false;
            // Add other initialization code here.
            return t;
        }

        [DefaultValue(0)]
        public int Value
        {
            get { return TrackBar.Value; }
            set { TrackBar.Value = value; }
        }
        /// <summary>
        /// Attach to events we want to re-wrap
        /// </summary>
        /// <param name="control"></param>
        protected override void OnSubscribeControlEvents(Control control)
        {
            base.OnSubscribeControlEvents(control);
            TrackBar trackBar = control as TrackBar;
            trackBar.ValueChanged += new EventHandler(trackBar_ValueChanged);
        }
        /// <summary>
        /// Detach from events.
        /// </summary>
        /// <param name="control"></param>
        protected override void OnUnsubscribeControlEvents(Control control)
        {
            base.OnUnsubscribeControlEvents(control);
            TrackBar trackBar = control as TrackBar;
            trackBar.ValueChanged -= new EventHandler(trackBar_ValueChanged);
        }
        /// <summary>
        /// Routing for event
        /// TrackBar.ValueChanged -> ToolStripTrackBar.ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void trackBar_ValueChanged(object sender, EventArgs e)
        {
            // when the trackbar value changes, fire an event.
            if (this.ValueChanged != null)
            {
                ValueChanged(sender, e);
            }
        }
        // add an event that is subscribable from the designer.
        public event EventHandler ValueChanged;
        // set other defaults that are interesting
        protected override Size DefaultSize
        {
            get
            {
                return new Size(200, 16);
            }
        }
    }
}
