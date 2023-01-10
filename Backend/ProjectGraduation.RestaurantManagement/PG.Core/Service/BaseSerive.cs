using PG.Core.Entities;
using PG.Core.Enum;
using PG.Core.Interface.Repository;
using PG.Core.Interface.Service;
using PG.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PG.Core.Service
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : BaseEntity
    {
        #region Declare
        protected IBaseRepository<TEntity> _baseRepository;
        protected ServiceResult _serviceResult;
        List<string> _errorMsg;
        #endregion

        #region Constructor
        public BaseService(IBaseRepository<TEntity> baseRepository, IServiceProvider serviceProvider)
        {
            _baseRepository = baseRepository;
            _serviceResult = new ServiceResult() { Code = PGCode.ValidData };
            _errorMsg = new List<string>();
        }
        #endregion

        #region Method
        /// <summary>
        /// Xóa một bản ghi theo Id
        /// </summary>
        /// <param name="Id">Id của bản ghi cần xóa</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        public ServiceResult DeleteEntity(Guid Id)
        {
            var rowAffect = _baseRepository.DeleteEntity(Id);

            if (rowAffect > 0)
            {
                _serviceResult.Data = rowAffect;
                _serviceResult.Code = PGCode.Success;
                _serviceResult.Message = Properties.Resources.Msg_SuccessDelete;
            }
            else
            {
                _serviceResult.Code = PGCode.InvalidData;
                _serviceResult.Message = Properties.Resources.Msg_FailedDelete;
            }

            return _serviceResult;
        }

        /// <summary>
        /// Xóa nhiều bản ghi theo Id
        /// </summary>
        /// <param name="listId">Danh sách id bản ghi cần xóa</param>
        /// <returns>Số bản ghi xóa được</returns>
        public ServiceResult MultipleDelete(IEnumerable<Guid> listId)
        {
            var rowAffect = _baseRepository.MultipleDelete(listId);

            if (rowAffect > 0)
            {
                _serviceResult.Data = rowAffect;
                _serviceResult.Code = PGCode.Success;
                _serviceResult.Message = Properties.Resources.Msg_SuccessDelete;
            }
            else
            {
                _serviceResult.Code = PGCode.InvalidData;
                _serviceResult.Message = Properties.Resources.Msg_FailedDelete;
            }

            return _serviceResult;
        }

        /// <summary>
        /// Kiểm tra xem có sự xung đột dữ liệu khi xoá hay không
        /// </summary>
        /// <param name="Id">Id bản ghi</param>
        /// <returns>Có xung đột hay không</returns>
        public ServiceResult CheckConflictData(Guid Id)
        {
            bool conflict = _baseRepository.CheckConflictData(Id);

            if(conflict)
            {
                _serviceResult.Code = PGCode.ConflictData;
                _serviceResult.Message = Properties.Resources.Msg_ConflictData;
                _serviceResult.Data = _baseRepository.GetEntityById(Id);
            }
            else
            {
                _serviceResult.Code = PGCode.Success;
            }

            return _serviceResult;
        }

        /// <summary>
        /// Lấy toàn bộ bản ghi
        /// </summary>
        /// <returns>List bản ghi lấy được</returns>
        public virtual IEnumerable<TEntity> GetEntities()
        {
            return _baseRepository.GetEntities();
        }

        /// <summary>
        /// Lấy dữ liệu paging
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public virtual object GetPagingData(PagingParam param)
        {
            //Lấy dữ liệu về
            var entities = _baseRepository.GetEntitiesFilter(param);

            //Lấy tổng số bản ghi
            var totalRecord = _baseRepository.GetTotalFilters(param);

            //Dữ liệu trả về
            var data = new
            {
                TotalRecord = totalRecord,
                TotalPage = param.PageSize != 0 ? Math.Ceiling((decimal)((decimal)totalRecord / param.PageSize)) : 1,
                data = entities
            };

            return data;
        }

        /// <summary>
        /// Lấy bản ghi theo Id
        /// </summary>
        /// <param name="Id">Id của đối tượng cần lấy</param>
        /// <returns>Một bản ghi lấy được theo Id</returns>
        public virtual TEntity GetEntityById(Guid Id)
        {
            return _baseRepository.GetEntityById(Id);
        }

        /// <summary>
        /// Lấy bản ghi theo property
        /// </summary>
        /// <param name="entity">Object cần kiểm tra</param>
        /// <param name="propName">Tên của trường cần kiểm tra</param>
        /// <returns>Một bản ghi có property với value truyền vào</returns>
        public TEntity GetEntityByProperty(TEntity entity, string propName)
        {
            return _baseRepository.GetEntityByProperty(entity, propName);
        }

        /// <summary>
        /// Hàm lấy bản ghi theo property
        /// </summary>
        /// <param name="propName">Tên trường cần kiểm tra</param>
        /// <param name="value">Giá trị của thuộc tính</param>
        /// <returns>Một bản ghi có property và value truyền vào</returns>
        public TEntity GetEntityByProperty(string propName, string propValue)
        {
            return _baseRepository.GetEntityByProperty(propName, propValue);
        }



        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm mới</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        public virtual ServiceResult InsertEntity(TEntity entity)
        {
            entity.EntityState = (int)EntityState.Add;
            //validate dữ liệu
            this.Validate(entity);

            //Dữ liệu trả về
            _serviceResult.Data = entity;

            //Nếu qua validate mà oke thì lưu
            if (_serviceResult.Code == PGCode.ValidData)
            {
                try
                {
                    _baseRepository.InsertEntity(entity);

                    _serviceResult.Code = PGCode.Success;
                    _serviceResult.Message = Properties.Resources.Msg_SuccessAdd;
                }
                catch(Exception ex)
                {
                    _serviceResult.Code = PGCode.Exception;
                    _serviceResult.Message = Properties.Resources.Msg_ServerError;
                }
            }

            return _serviceResult;
        }

        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm mới</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        public virtual ServiceResult InsertEntity(TEntity entity, Type type)
        {
            entity.EntityState = (int)EntityState.Add;
            //validate dữ liệu
            this.Validate(entity);

            //Dữ liệu trả về
            _serviceResult.Data = entity;

            //Nếu qua validate mà oke thì lưu
            if (_serviceResult.Code == PGCode.ValidData)
            {
                try
                {
                    _baseRepository.InsertEntity(entity, type);

                    _serviceResult.Code = PGCode.Success;
                    _serviceResult.Message = Properties.Resources.Msg_SuccessAdd;
                }
                catch (Exception ex)
                {
                    _serviceResult.Code = PGCode.Exception;
                    _serviceResult.Message = Properties.Resources.Msg_ServerError;
                }
            }

            return _serviceResult;
        }

        /// <summary>
        /// Sửa thông tin một bản ghi
        /// </summary>
        /// <param name="Id">Id của bản ghi cần sửa</param>
        /// <param name="entity">Đối tượng có những thông tin cần sửa</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        public virtual ServiceResult UpdateEntity(TEntity entity)
        {
            entity.EntityState = (int)EntityState.Update;
            //validate dữ liệu
            this.Validate(entity);

            //Dữ liệu trả về
            _serviceResult.Data = entity;

            //Nếu qua validate mà oke thì lưu
            if (_serviceResult.Code == PGCode.ValidData)
            {
                var rowAffect = _baseRepository.UpdateEntity(entity);

                //Trả về code tương ứng
                if (rowAffect > 0)
                {
                    _serviceResult.Code = PGCode.Success;
                    _serviceResult.Message = Properties.Resources.Msg_SuccessUpdate;
                }
                else
                {
                    _serviceResult.Code = PGCode.Exception;
                    _serviceResult.Message = Properties.Resources.Msg_ServerError;
                }
            }

            return _serviceResult;
        }

        #region Validate
        /// <summary>
        /// Validate dữ liệu
        /// </summary>
        /// <param name="entity">Đối tượng cần validate</param>
        /// <returns>Dữ liệu đã đúng hay chưa</returns>
        protected bool Validate(TEntity entity, IEnumerable<TEntity> entities = null, IDictionary<object, List<string>> uniqueProp = null)
        {
            var isValid = true;
            if (entities == null && uniqueProp == null)
            {
                entity.Status = new List<string>();
            }

            foreach (var prop in entity.GetType().GetProperties())
            {
                var displayName = "";
                //Tên hiển thị của property
                if (prop.IsDefined(typeof(DisplayNameAttribute), false))
                {
                    displayName = prop.GetCustomAttributes(typeof(DisplayNameAttribute), false)
                                        .Cast<DisplayNameAttribute>().Single().DisplayName;
                }


                //Validate required
                if (prop.IsDefined(typeof(Required), false))
                {
                    isValid = validateRequired(prop.GetValue(entity), displayName);
                }

                //Validate Unique
                if (prop.IsDefined(typeof(Unique), false) && (isValid || entities != null))
                {
                    isValid = ValidateUnique(entity, prop.Name, displayName, entities, uniqueProp);
                }
            }

            //Validate riêng
            isValid = CustomValidate(entity, _errorMsg) == true ? isValid : false;

            entity.Status.AddRange(_errorMsg);
            _errorMsg.Clear();

            return isValid;
        }

        /// <summary>
        /// Validate không được để trống
        /// </summary>
        /// <param name="value">Dữ liệu cần kiểm tra</param>
        /// <param name="propName">Display name của trường dữ liệu</param>
        /// <returns>Dữ liệu có hợp lệ hay không</returns>
        private bool validateRequired(object value, object displayName)
        {
            if (value == null || value.ToString().Length == 0)
            {
                _errorMsg.Add(String.Format(Properties.Resources.Msg_DataRequired, displayName));
                _serviceResult.Code = PGCode.InvalidData;
                _serviceResult.Message = Properties.Resources.Msg_InvalidData;

                return false;
            }

            return true;
        }

        /// <summary>
        /// Kiểm tra dữ liệu trùng lặp
        /// </summary>
        /// <param name="entity">Dữ liệu cần kiểm tra</param>
        /// <param name="propName">Field name của dữ liệu cần kiểm tra</param>
        /// <returns>Dữ liệu có hợp lệ hay không</returns>
        private bool ValidateUnique(TEntity entity, string propName, object displayName, IEnumerable<TEntity> entities, IDictionary<object, List<string>> uniqueProp)
        {
            var isUnique = true;
            //insert thông thường
            if (entities == null && uniqueProp == null)
            {
                isUnique = this.ValidateUniqueInsert(entity, propName, displayName);
            }
            //import
            else
            {
                //Lấy dữ liệu cần kiểm tra
                var value = entity.GetType().GetProperty(propName).GetValue(entity);

                if (value != null)
                {
                    //Validate với dữ liệu trên hệ thống
                    isUnique = this.ValidateUniqueImportDb(entities, propName, value, displayName);

                    //Validate với dữ liệu trong excel
                    isUnique = this.ValidateUniqueImportExcel(uniqueProp, value, propName, displayName);
                }
            }

            if (!isUnique)
            {
                _serviceResult.Code = PGCode.InvalidData;
                _serviceResult.Message = Properties.Resources.Msg_InvalidData;
            }

            return isUnique;
        }

        /// <summary>
        /// Hàm validate Unique khi insert một bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng cần kiểm tra</param>
        /// <param name="propName">Tên trường dữ liệu cần kiểm tra</param>
        /// <param name="displayName">Tên hiển thị của trường dữ liệu cần kiểm tra</param>
        /// <returns>Dữ liệu có hợp lệ hay không</returns>
        private bool ValidateUniqueInsert(TEntity entity, string propName, object displayName)
        {
            var entitySearch = _baseRepository.GetEntityByProperty(entity, propName);

            if (entitySearch != null)
            {
                //Nếu là form thêm hoặc là form sửa nhưng id không giống nhau
                if (entity.EntityState == (int)EntityState.Add ||
                    (entity.EntityState == (int)EntityState.Update && this.GetKeyProperty(entity).GetValue(entity).ToString() != this.GetKeyProperty(entitySearch).GetValue(entitySearch).ToString()))
                {
                    _errorMsg.Add(String.Format(Properties.Resources.Msg_DataNotUnique, displayName));
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Hàm kiểm tra dữ liệu đã tồn tại trong database chưa khi import
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="propName">Tên trường dữ liệu cần kiểm tra</param>
        /// <param name="value">Dữ liệu cần kiểm tra</param>
        /// <param name="displayName">Tên hiển thị của trường dữ liệu cần kiểm tra</param>
        /// <returns>Dữ liệu có hợp lệ hay không</returns>
        private bool ValidateUniqueImportDb(IEnumerable<TEntity> entities, string propName, object value, object displayName)
        {
            var entitySearch = entities.Where(e => e.GetType().GetProperty(propName).GetValue(e).ToString() == value.ToString()).FirstOrDefault();

            if (entitySearch != null)
            {
                _errorMsg.Add(String.Format(Properties.Resources.Msg_DataNotUnique, displayName));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Hàm kiểm tra dữ liệu có trùng trong list dữ liệu import không
        /// </summary>
        /// <param name="uniqueProp">Một map check giá trị trùng lặp</param>
        /// <param name="value">Dữ liệu cần kiểm tra</param>
        /// <param name="propName">Tên trường dữ liệu cần kiểm tra</param>
        /// <param name="displayName">Tên hiển thị của trường dữ liệu cần kiểm tra</param>
        /// <returns>Dữ liệu có hợp lệ hay không</returns>
        private bool ValidateUniqueImportExcel(IDictionary<object, List<string>> uniqueProp, object value, string propName, object displayName)
        {
            //Nếu chưa từng có trong map
            if (!uniqueProp.ContainsKey(value))
            {
                var list = new List<string>();
                list.Add(propName);

                uniqueProp.Add(value, list);
            }
            else
            {
                //Nếu dữ liệu đã từng xuất hiện
                if (uniqueProp[value].Contains(propName))
                {
                    _errorMsg.Add($"{displayName} đã trùng với {displayName} khác nhập khẩu");

                    return false;
                }
                else
                {
                    uniqueProp[value].Add(propName);
                }
            }

            return true;
        }

        /// <summary>
        /// Hàm cho lớp con kế thừa để validate riêng
        /// </summary>
        /// <param name="entity">Đối tượng cần validate</param>
        /// <param name="errorMsg">List lỗi trả về</param>
        /// <returns></returns>
        protected virtual bool CustomValidate(TEntity entity, List<string> errorMsg)
        {
            return true;
        }

        /// <summary>
        /// Hàm lấy ra giá trị key của entity
        /// </summary>
        /// <param name="entity">Đối tượng cần lấy</param>
        /// <returns>Giá trị key của entity</returns>
        private PropertyInfo GetKeyProperty(TEntity entity)
        {
            var keyProperty = entity.GetType()
                .GetProperties()
                .Where(p => p.IsDefined(typeof(PrimaryKey), false))
                .FirstOrDefault();
            return keyProperty;
        }
        #endregion
        #endregion
    }
}
