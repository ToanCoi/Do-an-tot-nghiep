using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Enum
{
    /// <summary>
    /// Mã code trả về
    /// </summary>
    public enum PGCode
    {
        ValidData = 100,
        InvalidData = 900,
        Exception = 500,
        Success = 200,
        ConflictData = 999,
    }

    /// <summary>
    /// Trạng thái entity
    /// </summary>
    public enum EntityState
    {
        Add = 1,
        Update = 2,
        Delete = 3
    }
}
