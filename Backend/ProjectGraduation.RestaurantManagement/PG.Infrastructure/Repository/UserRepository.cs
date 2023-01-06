using Microsoft.Extensions.Configuration;
using PG.Core.Entities;
using PG.Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Infrastructure.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        #region Constructor
        public UserRepository(IConfiguration configuration, IServiceProvider serviceProvider) : base(configuration, serviceProvider)
        {

        }
        #endregion
    }
}
