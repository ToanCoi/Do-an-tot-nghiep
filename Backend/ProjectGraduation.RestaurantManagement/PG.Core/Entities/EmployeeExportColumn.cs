using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Entities
{
    public class EmployeeExportColumn
    {
        /// <summary>
        /// Stt cột
        /// </summary>
        public int ColumnId { get; set; }

        /// <summary>
        /// Tên trường
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Tên hiển thị
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Độ rộng cột
        /// </summary>
        public int Width { get; set; }
    }
}
