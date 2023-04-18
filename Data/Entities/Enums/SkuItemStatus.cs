using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.Enums
{
    public enum SkuItemStatus
    {
        Published = 1,
        Inactive = 2,
        Deleted = 3,
        available = 4,
        unavailable = 5,
        sold = 6,
        damaged = 7,
        returned = 8
    }
}
