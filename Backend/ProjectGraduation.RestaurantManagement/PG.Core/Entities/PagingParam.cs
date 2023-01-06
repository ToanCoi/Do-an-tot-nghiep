using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Entities
{
    public class PagingParam
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public string? FilterValue { get; set; }
        public string? FilterColumn { get; set; }
        public string? Sort { get; set; }
    }
}
