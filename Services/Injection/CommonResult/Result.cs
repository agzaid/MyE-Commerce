using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Injection.CommonResult
{
    public class Result<TResult>
    {
        public Result()
        {
            Errors = new List<string>();
        }

        public bool Succeeded { get; set; }
        public List<string> Errors { get; set; }
        public string ExceptionMessage { get; set; }
        public ValidationMessageTypes MessageType { get; set; }
        public TResult GetModelResult { get; set; }
    }
}
