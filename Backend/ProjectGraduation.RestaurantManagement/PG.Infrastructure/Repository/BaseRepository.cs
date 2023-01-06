using Dapper;
using Microsoft.Extensions.Configuration;
using PG.Core.Entities;
using PG.Core.Interface.Repository;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using PG.Library.Service;
using Microsoft.Extensions.DependencyInjection;
using PG.Core.Enum;

namespace PG.Infrastructure.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        #region Declare
        IConfiguration _configuration;
        string _connectionString = string.Empty;
        protected IDbConnection _dbConnection = null;
        string _tableName = string.Empty;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITypeService _typeService;
        #endregion

        #region Constructor
        public BaseRepository(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            this._configuration = configuration;
            _connectionString = _configuration.GetConnectionString("Development");
            _dbConnection = new MySqlConnection(_connectionString);
            _dbConnection.Open();
            _tableName = ((TableAttribute)Attribute.GetCustomAttribute(typeof(TEntity), typeof(TableAttribute))).Name;
            _serviceProvider = serviceProvider;
            _typeService = _serviceProvider.GetRequiredService<ITypeService>();
        }
        #endregion

        #region Method
        /// <summary>
        /// Xóa một bản ghi theo Id
        /// </summary>
        /// <param name="Id">Id của bản ghi cần xóa</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        public int DeleteEntity(Guid Id)
        {

            int rowAffects = 0;

            var idParam = new DynamicParameters();
            idParam.Add($"{_tableName}Id", Id);

            string sql = $"DELETE FROM `{_tableName}` WHERE `{_tableName}_id` = @{_tableName}Id";

            //Xóa 
            rowAffects = _dbConnection.Execute(sql, idParam, commandType: CommandType.Text);

            return rowAffects;
                    
        }

        /// <summary>
        /// Kiểm tra xem có sự xung đột dữ liệu khi xoá hay không
        /// </summary>
        /// <param name="Id">Id bản ghi</param>
        /// <returns>Có xung đột hay không</returns>
        public bool CheckConflictData(Guid Id)
        {
            //Kiểm tra xem có cần check conflict không
            var mustCheckConflict = _dbConnection.QueryFirstOrDefault<string>($"SELECT Fn_ProcExist('Proc_CheckConflict{_tableName}Data') as exist;");

            if(Boolean.Parse(mustCheckConflict))
            {
                //Check conflict
                var idParam = new DynamicParameters();
                idParam.Add($"{_tableName}Id", Id);

                var rowAffects = _dbConnection.QueryFirstOrDefault<string>($"Proc_CheckConflict{_tableName}Data", param: idParam, commandType: CommandType.StoredProcedure);

                if (rowAffects == "true" || int.Parse(rowAffects) > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                //Không thì thôi
                return false;
            }
        }

        /// <summary>
        /// Xóa nhiều bản ghi theo Id
        /// </summary>
        /// <param name="listId">Danh sách id bản ghi cần xóa</param>
        /// <returns>Số bản ghi xóa được</returns>
        public virtual int MultipleDelete(IEnumerable<Guid> listId)
        {
            int rowAffects = 0;

            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    foreach(var id in listId)
                    {
                        var idParam = new DynamicParameters();
                        idParam.Add($"{_tableName}Id", id);

                        //Xóa 
                        rowAffects += _dbConnection.Execute($"Proc_Delete{_tableName}ById", idParam, commandType: CommandType.StoredProcedure, transaction: transaction);
                    }

                    //Nếu xóa tất cả được thì commit
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }

            return rowAffects;
        }

        /// <summary>
        /// Lấy toàn bộ bản ghi
        /// </summary>
        /// <returns>List bản ghi lấy được</returns>
        public IEnumerable<TEntity> GetEntities()
        {
            var entities = _dbConnection.Query<TEntity>($"Select * from `{_tableName}`", commandType: CommandType.Text);

            return entities;
        }

        /// <summary>
        /// Lấy bản ghi theo filter
        /// </summary>
        /// <returns>List bản ghi lấy được</returns>
        public IEnumerable<TEntity> GetEntitiesFilter(PagingParam pagingParam)
        {
            var sql = new StringBuilder();
            var where = this.ParseWhere(pagingParam);
            var sort = this.ParseSort(pagingParam.Sort);

            sql.Append($"SELECT * FROM `{_tableName}`");
            if(where != null)
            {
                sql.Append(where);
            }

            if(sort != null)
            {
                sql.Append(sort);
            }

            if(pagingParam.PageSize > 0)
            {
                sql.Append($" LIMIT {pagingParam.PageSize}");

                if(pagingParam.CurrentPage > 1)
                {
                    sql.Append($" OFFSET {(pagingParam.CurrentPage - 1) * pagingParam.PageSize}");
                }
            }

            var entities = _dbConnection.QueryAsync<TEntity>(sql.ToString(), commandType: CommandType.Text).GetAwaiter().GetResult();

            return entities;
        }

        /// <summary>
        /// Lấy tổng số bản ghi theo filter
        /// </summary>
        /// <returns></returns>
        public int GetTotalFilters(PagingParam pagingParam)
        {
            var where = this.ParseWhere(pagingParam);
            string sql = $"Select Count(*) from `{_tableName}` {where}";

            var total = _dbConnection.QueryFirstOrDefaultAsync<int>(sql, commandType: CommandType.Text).GetAwaiter().GetResult();

            return total;
        }

        /// <summary>
        /// Hàm build câu where
        /// </summary>
        /// <param name="pagingParam"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected string ParseWhere(PagingParam pagingParam)
        {
            var where = new StringBuilder();
            var columnsFilter = pagingParam.FilterColumn?.Split(",");

            if(columnsFilter != null && columnsFilter.Length > 0)
            {
                where.Append(" WHERE ");

                for(int i = 0; i < columnsFilter.Length; i++)
                {
                    where.Append($" `{columnsFilter[i].Trim()}` like '%{pagingParam.FilterValue ?? ""}%'");

                    if(i < columnsFilter.Length - 1)
                    {
                        where.Append(" OR ");
                    }
                }
            }

            return where.ToString();
        }

        /// <summary>
        /// Build câu order by
        /// </summary>
        /// <param name="pagingParam"></param>
        /// <returns></returns>
        protected string ParseSort(string sort)
        {
            if(String.IsNullOrWhiteSpace(sort))
            {
                return null;
            }

            return $" ORDER BY {sort}";
        }

        /// <summary>
        /// Lấy bản ghi theo Id
        /// </summary>
        /// <param name="Id">Id của đối tượng cần lấy</param>
        /// <returns>Một bản ghi lấy được theo Id</returns>
        public virtual TEntity GetEntityById(Guid Id)
        {
            var idParam = new DynamicParameters();
            idParam.Add($"{_tableName}Id", Id);

            string sql = $"SELECT * FROM `{_tableName}` WHERE {_tableName}_id = @{_tableName}Id";

            var entity = _dbConnection.QueryFirstOrDefault<TEntity>(sql, idParam, commandType: CommandType.Text);

            return entity;
        }

        /// <summary>
        /// Lấy bản ghi theo property
        /// </summary>
        /// <param name="entity">Object cần kiểm tra</param>
        /// <param name="propName">Tên của trường cần kiểm tra</param>
        /// <returns>Một bản ghi có property với value truyền vào</returns>
        public TEntity GetEntityByProperty(TEntity entity, string propName)
        {
            var propvalue = entity.GetType().GetProperty(propName).GetValue(entity);

            string query = $"select * FROM {_tableName} where {propName} = '{propvalue}'";
            var entitySearch = _dbConnection.QueryFirstOrDefault<TEntity>(query);

            return entitySearch;
        }

        /// <summary>
        /// Hàm lấy bản ghi theo property - cho frontend dùng
        /// </summary>
        /// <param name="propName">Tên trường cần kiểm tra</param>
        /// <param name="value">Giá trị của thuộc tính</param>
        /// <returns>Một bản ghi có property và value truyền vào</returns>
        public TEntity GetEntityByProperty(string propName, string propValue)
        {
            //Query string
            string query = $"select * FROM {_tableName} where {propName} = '{propValue}'";

            //Lấy entity
            var entitySearch = _dbConnection.QueryFirstOrDefault<TEntity>(query);

            return entitySearch;
        }

        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm mới</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        public virtual object InsertEntity(TEntity entity)
        {

            var columns = _typeService.GetTableColumns(entity.GetType());
            var keyField = _typeService.GetKeyField(entity.GetType());

            if(keyField != null && keyField.GetValue(entity).ToString() == Guid.Empty.ToString())
            {
                keyField.SetValue(entity, Guid.NewGuid());
            }
            entity.created_date = DateTime.Now;

            var sql = new StringBuilder();
            sql.Append($"INSERT INTO `{_tableName}` (`{string.Join("`,`", columns)}`) VALUES (@{string.Join(",@", columns)});");

            var res = _dbConnection.ExecuteScalarAsync(sql.ToString(), entity, commandType: CommandType.Text).GetAwaiter().GetResult();

            return res;
        }

        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm mới</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        public virtual object InsertEntity(TEntity entity, Type type)
        {

            var columns = _typeService.GetTableColumns(type);
            var keyField = _typeService.GetKeyField(type);

            if (keyField != null && keyField.GetValue(entity).ToString() == Guid.Empty.ToString())
            {
                keyField.SetValue(entity, Guid.NewGuid());
            }
            entity.created_date = DateTime.Now;

            var sql = new StringBuilder();
            sql.Append($"INSERT INTO `{_tableName}` (`{string.Join("`,`", columns)}`) VALUES (@{string.Join(",@", columns)});");

            var res = _dbConnection.ExecuteScalarAsync(sql.ToString(), entity, commandType: CommandType.Text).GetAwaiter().GetResult();

            return res;
        }

        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm mới</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        public virtual object InsertEntity(TEntity entity, string tableName)
        {
            var columns = _typeService.GetTableColumns(entity.GetType());
            var keyField = _typeService.GetKeyField(entity.GetType());

            if (keyField.GetValue(entity).ToString() == Guid.Empty.ToString())
            {
                keyField.SetValue(entity, Guid.NewGuid());
            }
            entity.created_date = DateTime.Now;

            var sql = new StringBuilder();
            sql.Append($"INSERT INTO `{tableName}` (`{string.Join("`,`", columns)}`) VALUES (@{string.Join(",@", columns)});");

            var res = _dbConnection.ExecuteScalarAsync(sql.ToString(), entity, commandType: CommandType.Text).GetAwaiter().GetResult();

            return res;
        }

        /// <summary>
        /// Sửa thông tin một bản ghi
        /// </summary>
        /// <param name="Id">Id của bản ghi cần sửa</param>
        /// <param name="entity">Đối tượng có những thông tin cần sửa</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        public virtual int UpdateEntity(TEntity entity)
        {
            int rowAffects = 0;

            var keyFields = _typeService.GetKeyFields(entity.GetType()).Select(x => x.Name).ToList();
            string keyName = keyFields?.FirstOrDefault();
            var columns = _typeService.GetTableColumns(entity.GetType()).Where(x => !keyFields.Contains(x));
            entity.update_date = DateTime.Now;


            var sql = new StringBuilder();
            sql.Append($"UPDATE `{_tableName}` SET {string.Join(", ", columns.Select(n => $"`{n}` = @{n}"))} WHERE `{keyName}` = @{keyName}");


            //Insert
            rowAffects = _dbConnection.ExecuteAsync(sql.ToString(), entity, commandType: CommandType.Text).GetAwaiter().GetResult();

            return rowAffects;
        }

        /// <summary>
        /// Kiểm tra xem bản ghi có tồn tại không
        /// </summary>
        /// <param name="Id">Id bản ghi</param>
        /// <returns>Bản ghi có tồn tại hay không</returns>
        public int CheckExist(Guid Id)
        {
            int rowAffects = 0;

            var param = new DynamicParameters();
            param.Add($"{_tableName}Id", Id);

            rowAffects = _dbConnection.Execute($"Proc_CheckExist{_tableName}", param, commandType: CommandType.StoredProcedure);

            return rowAffects;
        }

        /// <summary>
        /// Hàm mapping dữ liệu
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">Object cần mapping</param>
        /// <returns>Object chứa các data mapping</returns>
        protected DynamicParameters MappingData(TEntity entity)
        {
            var dynamicParam = new DynamicParameters();

            foreach (var prop in entity.GetType().GetProperties())
            {
                var propName = prop.Name;
                var propValue = prop.GetValue(entity);
                var propType = prop.PropertyType;

                if (propType == typeof(Guid) || propType == typeof(Guid?))
                {
                    dynamicParam.Add($"@{propName}", propValue, DbType.String);
                }
                else
                {
                    dynamicParam.Add($"@{propName}", propValue);
                }
            }

            return dynamicParam;
        }

        #endregion

        #region Ultility
        public IDbConnection GetConnection()
        {
            return null;
        }
        #endregion
    }
}
