using PG.Core.Dto;
using PG.Core.Entities;
using PG.Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Interface.Service
{
    public interface IAccountService : IBaseService<Account>
    {
        /// <summary>
        /// Check login
        /// </summary>
        Account? VerifyLogin(string username, string password);
    }
}
