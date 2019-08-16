using System;
using System.Collections.Generic;

namespace Atum.Studio.Core.Shapes
{
    class ConnectedIndexValueItem : Dictionary<ushort, ushort>, IDisposable //outside vectorindex
    {
        public ConnectedIndexValueItem()
        {
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        ~ConnectedIndexValueItem()
        {
            this.Dispose();
        }
    }

    class ConnectedIndexItemList : Dictionary<int, ConnectedIndexValueItem>, IDisposable
    {
        public ConnectedIndexItemList()
        {
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        ~ConnectedIndexItemList()
        {
            this.Dispose();
        }
    }
}
