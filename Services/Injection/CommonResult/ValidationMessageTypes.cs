using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Injection.CommonResult
{
    public enum ValidationMessageTypes
    {
        Success,
        Warning,
        Error,
        Exception,
        Exist,
        NotFound,
        NotAuthorized
    }
}
