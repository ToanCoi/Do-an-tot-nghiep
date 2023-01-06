using PG.Core.Entities;
using PG.Core.Interface.Repository;
using PG.Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Service
{
    public class UserService : BaseService<User>, IUserService
    {
        #region Constructor
        public UserService(IUserRepository userRepository, IServiceProvider serviceProvider) : base(userRepository, serviceProvider)
        {

        }
        #endregion
    }
}
