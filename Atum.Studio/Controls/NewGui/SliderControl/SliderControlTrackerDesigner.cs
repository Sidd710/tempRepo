using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Atum.Studio.Controls.NewGui.SliderControl.SliderControlTracker
{
    class SliderControlTrackerDesigner : ControlDesigner
    {

    
            public SliderControlTrackerDesigner()
            { }

            //Overrides
            /// <summary>
            /// Remove Button and Control properties that are 
            /// not supported by the <see cref="MACTrackBar"/>.
            /// </summary>
            protected override void PostFilterProperties(IDictionary Properties)
            {
                Properties.Remove("AllowDrop");
                Properties.Remove("BackgroundImage");
                Properties.Remove("ContextMenu");

                Properties.Remove("Text");
                Properties.Remove("TextAlign");
                Properties.Remove("RightToLeft");
            }


            //Overrides
            /// <summary>
            /// Remove Button and Control events that are 
            /// not supported by the <see cref="MACTrackBar"/>.
            /// </summary>
            protected override void PostFilterEvents(IDictionary events)
            {
                //Actions
                events.Remove("Click");
                events.Remove("DoubleClick");

                //Appearence
                events.Remove("Paint");

                //Behavior
                events.Remove("ChangeUICues");
                events.Remove("ImeModeChanged");
                events.Remove("QueryAccessibilityHelp");
                events.Remove("StyleChanged");
                events.Remove("SystemColorsChanged");

                //Drag Drop
                events.Remove("DragDrop");
                events.Remove("DragEnter");
                events.Remove("DragLeave");
                events.Remove("DragOver");
                events.Remove("GiveFeedback");
                events.Remove("QueryContinueDrag");
                events.Remove("DragDrop");

                //Layout
                events.Remove("Layout");
                events.Remove("Move");
                events.Remove("Resize");

                //Property Changed
                events.Remove("BackColorChanged");
                events.Remove("BackgroundImageChanged");
                events.Remove("BindingContextChanged");
                events.Remove("CausesValidationChanged");
                events.Remove("CursorChanged");
                events.Remove("FontChanged");
                events.Remove("ForeColorChanged");
                events.Remove("RightToLeftChanged");
                events.Remove("SizeChanged");
                events.Remove("TextChanged");

                base.PostFilterEvents(events);
            }

        }
    }

