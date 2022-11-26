using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PG.Core.Entities;
using PG.Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PG.API.Controllers
{
    public class DepartmentsController : BaseController<Department>
    {
        #region Constructor
        public DepartmentsController(IDepartmentService departmentService) : base(departmentService)
        {

        }
        #endregion
    }
}
