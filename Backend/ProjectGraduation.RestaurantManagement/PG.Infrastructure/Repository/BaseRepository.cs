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

namespace PG.Infrastructure.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        #region Declare
        IConfiguration _configuration;
        string _connectionString = string.Empty;
        protected IDbConnection _dbConnection = null;
        string _tableName = string.Empty;
        #endregion

        #region Constructor
        public BaseRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
            _connectionString = _configuration.GetConnectionString("Development");
            _dbConnection = new MySqlConnection(_connectionString);
            _dbConnection.Open();
            _tableName = typeof(TEntity).Name;
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

            //Xóa 
            rowAffects = _dbConnection.Execute($"Proc_Delete{_tableName}ById", param: idParam, commandType: CommandType.StoredProcedure);

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
            var entities = _dbConnection.Query<TEntity>($"Proc_Get{_tableName}s", commandType: CommandType.StoredProcedure);

            return entities;
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
            var customer = _dbConnection.QueryFirstOrDefault<TEntity>($"Proc_Get{_tableName}ById", idParam, commandType: CommandType.StoredProcedure);

            return customer;
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
        public virtual int InsertEntity(TEntity entity)
        {
            int rowAffects = 0;

            //Insert
            rowAffects = _dbConnection.Execute($"Proc_Insert{_tableName}", entity, commandType: CommandType.StoredProcedure);

            return rowAffects;
        }

        /// <summary>
        /// Sửa thông tin một bản ghi
        /// </summary>
        /// <param name="Id">Id của bản ghi cần sửa</param>
        /// <param name="entity">Đối tượng có những thông tin cần sửa</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        public virtual int UpdateEntity(Guid Id, TEntity entity)
        {
            int rowAffects = 0;

            //Mapping dữ liệu
            var dynamicParam = MappingData(entity);

            //Insert
            rowAffects = _dbConnection.Execute($"Proc_Update{_tableName}", dynamicParam, commandType: CommandType.StoredProcedure);

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
    }
}
