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
    public class TableRepository : BaseRepository<Table>, ITableRepository
    {
        #region Constructor
        public TableRepository(IConfiguration configuration) : base(configuration)
        {

        }
        #endregion
    }
}
