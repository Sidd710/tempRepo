using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.DAL.Remoting
{
    [Serializable]
    public class RemoteControlAction
    {
        public NavigationButtonActionType NavigationButtonAction { get; set; }
        public string RemoteHostIP { get; set; }

        public enum NavigationButtonActionType
        {
            Left = 0,
            Right = 1,
            Up = 2,
            Down = 3,
            Select = 5,
            CurrentLCDMenu = 10
        }

    }

    [Serializable]
    public class RemoteControlActions : List<RemoteControlAction>
    {
        public RemoteControlActions()
        {
            
        }
    }
}