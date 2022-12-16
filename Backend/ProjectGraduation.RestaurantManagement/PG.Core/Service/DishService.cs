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
    public class DishService : BaseService<Dish>, IDishService
    {
        #region Constructor
        public DishService(IDishRepository dishRepository, IServiceProvider serviceProvider) : base(dishRepository, serviceProvider)
        {

        }
        #endregion

        #region Method
        #endregion
    }
}
