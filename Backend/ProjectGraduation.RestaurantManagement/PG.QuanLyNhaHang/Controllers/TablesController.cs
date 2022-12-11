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
    public class TablesController : BaseController<Table>
    {
        #region Constructor
        public TablesController(ITableService tableService) : base(tableService)
        {

        }
        #endregion
    }
}
