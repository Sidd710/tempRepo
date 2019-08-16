using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.DAL.Remoting
{
    [Serializable]
    public class PrinterAction
    {
        public string PrinterType;
        public int PrinterActionId;
        public List<object> Values;

        public PrinterAction()
        {
            this.Values = new List<object>();
        }

    }
}
