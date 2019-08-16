using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Atum.Studio.Core.Managers.UndoRedo
{
    public class ActionModel : BaseModel
    {
        public BackgroundWorker BackgroundWorker { get; set; }
        public List<object> Arguments { get; set; }
    }
    
    public class AddModel : BaseModel
    {
        public string UndoFilePath { get; set; }
    }

    public class RemoveModel : BaseModel
    {

    }
}
