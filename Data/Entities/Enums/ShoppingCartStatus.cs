using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.Enums
{
    public enum ShoppingCartStatus
    {
        PendingForPreview = 1,
        InProgress = 2,
        OnTheWay = 3,
        Received = 4,
        Returned = 5,
        Damaged = 6,
        MissingItems= 7,
        OnHold= 8,
        Cancelled= 9,
    }
}
