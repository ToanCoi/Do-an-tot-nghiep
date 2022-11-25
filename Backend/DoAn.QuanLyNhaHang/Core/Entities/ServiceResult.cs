using PG.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Entities
{
    public class ServiceResult
    {
        public Object Data { get; set; }
        public string Message { get; set; }
        public PGCode Code { get; set; }
    }
}
