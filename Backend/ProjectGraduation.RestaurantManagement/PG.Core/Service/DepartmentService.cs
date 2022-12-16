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
    public class DepartmentService : BaseService<Department>, IDepartmentService
    {
        #region Constructor
        public DepartmentService(IDepartmentRepository departmentRepository, IServiceProvider serviceProvider) : base(departmentRepository, serviceProvider)
        {

        }
        #endregion
    }
}
