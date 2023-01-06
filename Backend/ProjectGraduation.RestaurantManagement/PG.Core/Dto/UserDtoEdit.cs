using PG.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Dto
{
    public class UserDtoEdit : User
    {
        public string? username { get; set; }

        public string? password { get; set; }

        public string? token { get; set; }
    }
}
