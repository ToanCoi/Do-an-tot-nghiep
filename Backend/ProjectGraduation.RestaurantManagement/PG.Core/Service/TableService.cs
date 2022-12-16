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
    public class TableService : BaseService<Table>, ITableService
    {
        #region Constructor
        public TableService(ITableRepository tableRepository, IServiceProvider serviceProvider) : base(tableRepository, serviceProvider)
        {

        }
        #endregion
    }
}
