using Dapper;
using Microsoft.Extensions.Configuration;
using PG.Core.Entities;
using PG.Core.Enum;
using PG.Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Infrastructure.Repository
{
    public class TaxRepository : BaseRepository<Tax>, ITaxRepository
    {
        #region Constructor
        public TaxRepository(IConfiguration configuration, IServiceProvider serviceProvider) : base(configuration, serviceProvider)
        {

        }
        #endregion

        #region Method
        /// <summary>
        /// Lấy mã kỳ thuế lớn nhất trong db
        /// </summary>
        /// <returns>Mã nhân viên lớn</returns>
        public string GetMaxTaxCode()
        {
            var maxTaxCode = _dbConnection.QueryFirstOrDefault<string>("Proc_GetMaxTaxCode", commandType: CommandType.StoredProcedure);

            return maxTaxCode;
        }
        /// <summary>
        /// Lấy danh sách kỳ thuế theo năm và mã hoặc tên
        /// </summary>
        /// <param name="year">Năm cần tìm</param>
        /// <param name="filterValue">Giá trị filter</param>
        /// <param name="pageSize">Số bản ghi/trang</param>
        /// <param name="pageNum">Số trang</param>
        /// <returns>Danh sách kỳ thuế theo giá trị truyền vào</returns>
        public IEnumerable<Tax> GetTaxsFilter(int year, int? pageSize = null, int? pageNum = null, string filterValue = null)
        {
            DynamicParameters param = new DynamicParameters();

            param.Add("@Year", year);
            param.Add("@FilterValue", filterValue);
            param.Add("@PageSize", pageSize);
            param.Add("@PageNum", pageNum);

            //Lấy các đợt thuế
            var taxs = _dbConnection.Query<Tax>("Proc_GetTaxsFilter", param, commandType: CommandType.StoredProcedure);

            //Lấy tax detail
            foreach (Tax tax in taxs)
            {
                param = new DynamicParameters();
                param.Add("@TaxId", tax.TaxId);

                //Danh sách nhân viên trong đợt thuế
                tax.TaxDetail = _dbConnection.Query<EmployeeTax>("Proc_GetTaxDetail", param, commandType: CommandType.StoredProcedure);

                //Tổng số tiền thuế
                var totalMoney = _dbConnection.QueryFirstOrDefault<string>("Proc_GetTotalTaxMoney", param, commandType: CommandType.StoredProcedure);
                if(totalMoney != null)
                {
                    tax.TotalTaxMoney = Double.Parse(totalMoney);
                }
                else
                {
                    tax.TotalTaxMoney = null;
                }

                //Tổng số nhân viên
                tax.TotalEmployee = tax.TaxDetail.ToList().Count;
            }

            return taxs;
        }

        /// <summary>
        /// Lấy tổng số bản ghi theo filter và năm
        /// </summary>
        /// <returns>Tổng số bản ghi theo filter và năm</returns>
        public string GetTotalRecord(int year, string filterValue)
        {
            var param = new DynamicParameters();

            param.Add("@Year", year);
            param.Add("@FilterValue", filterValue);

            var totalRecord = _dbConnection.QueryFirstOrDefault<string>("Proc_GetTotalTaxsFilter", param, commandType: CommandType.StoredProcedure);

            return totalRecord;
        }

        /// <summary>
        /// Lấy bản ghi theo Id
        /// </summary>
        /// <param name="Id">Id của đối tượng cần lấy</param>
        /// <returns>Một bản ghi lấy được theo Id</returns>
        public override Tax GetEntityById(Guid Id)
        {
            var idParam = new DynamicParameters();
            idParam.Add($"TaxId", Id);
            var tax = _dbConnection.QueryFirstOrDefault<Tax>($"Proc_GetTaxById", idParam, commandType: CommandType.StoredProcedure);

            //Danh sách nhân viên trong đợt thuế
            tax.TaxDetail = _dbConnection.Query<EmployeeTax>("Proc_GetTaxDetail", idParam, commandType: CommandType.StoredProcedure);

            //Tổng số tiền thuế
            tax.TotalTaxMoney = Double.Parse(_dbConnection.QueryFirstOrDefault<string>("Proc_GetTotalTaxMoney", idParam, commandType: CommandType.StoredProcedure));

            //Tổng số nhân viên
            tax.TotalEmployee = tax.TaxDetail.ToList().Count;

            return tax;
        }

        /// <summary>
        /// Hàm thêm mới một kỳ thuế
        /// </summary>
        /// <param name="tax">Kỳ thuế</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        public override object InsertEntity(Tax tax)
        {
            int rowAffects = 0;

            //Tạo mã taxid mới
            tax.TaxId = Guid.NewGuid();

            var param = new DynamicParameters();

            param.Add($"TaxId", tax.TaxId);
            param.Add($"TaxCode", tax.TaxCode);
            param.Add($"TaxName", tax.TaxName);
            param.Add($"StartDate", tax.StartDate);
            param.Add($"EndDate", tax.EndDate);
            param.Add($"Note", tax.Note);

            rowAffects = _dbConnection.Execute($"Proc_InsertTax", param, commandType: CommandType.StoredProcedure);

            //Nếu thêm được tax rồi thì mới thêm taxdetail
            if (rowAffects > 0)
            {
                rowAffects = 0;
                foreach (var et in tax.TaxDetail)
                {
                    //Gán mã kỳ thuế
                    et.TaxId = tax.TaxId;

                    param = new DynamicParameters();

                    param.Add($"EmployeeTaxId", et.EmployeeTaxId);
                    param.Add($"EmployeeId", et.EmployeeId);
                    param.Add($"TaxId", et.TaxId);
                    param.Add($"EmployeeSalary", et.EmployeeSalary);
                    param.Add($"InsuranceMoney", et.InsuranceMoney);

                    rowAffects += _dbConnection.Execute($"Proc_InsertEmployeeTax", param, commandType: CommandType.StoredProcedure);
                }
            }

            return rowAffects;
        }

        /// <summary>
        /// Sửa thông tin một bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng có những thông tin cần sửa</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        public override int UpdateEntity(Tax tax)
        {
            int rowAffects = 0;

            var param = new DynamicParameters();

            param.Add($"TaxId", tax.TaxId);
            param.Add($"TaxCode", tax.TaxCode);
            param.Add($"TaxName", tax.TaxName);
            param.Add($"StartDate", tax.StartDate);
            param.Add($"EndDate", tax.EndDate);
            param.Add($"Note", tax.Note);

            rowAffects = _dbConnection.Execute($"Proc_UpdateTax", param, commandType: CommandType.StoredProcedure);

            foreach (var et in tax.TaxDetail)
            {

                param = new DynamicParameters();

                param.Add($"EmployeeTaxId", et.EmployeeTaxId);
                param.Add($"EmployeeId", et.EmployeeId);
                param.Add($"TaxId", tax.TaxId);
                param.Add($"EmployeeSalary", et.EmployeeSalary);
                param.Add($"InsuranceMoney", et.InsuranceMoney);

                switch (et.Mode)
                {
                    case EntityState.Add:
                        rowAffects += _dbConnection.Execute($"Proc_InsertEmployeeTax", param, commandType: CommandType.StoredProcedure);
                        break;
                    case EntityState.Update:
                        param = new DynamicParameters();
                        param.Add($"EmployeeTaxId", et.EmployeeTaxId);
                        param.Add($"EmployeeSalary", et.EmployeeSalary);
                        param.Add($"InsuranceMoney", et.InsuranceMoney);

                        rowAffects += _dbConnection.Execute($"Proc_UpdateEmployeeTax", param, commandType: CommandType.StoredProcedure);
                        break;
                    case EntityState.Delete:
                        param = new DynamicParameters();
                        param.Add($"EmployeeTaxId", et.EmployeeTaxId);

                        rowAffects += _dbConnection.Execute($"Proc_DeleteEmployeeTaxById", param, commandType: CommandType.StoredProcedure);
                        break;
                }
            }

            return rowAffects;
        }

        /// <summary>
        /// Xóa nhiều bản ghi theo Id
        /// </summary>
        /// <param name="listId">Danh sách id bản ghi cần xóa</param>
        /// <returns>Số bản ghi xóa được</returns>
        public override int MultipleDelete(IEnumerable<Guid> listId)
        {
            var rowAffects = 0;

            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    foreach (var id in listId)
                    {
                        var param = new DynamicParameters();
                        param.Add($"TaxId", id);

                        //Xoá bản ghi taxdetail trước
                        var temp = _dbConnection.Execute($"Proc_DeleteEmployeeTaxByTaxId", param, commandType: CommandType.StoredProcedure, transaction: transaction);

                        //Xoá tax
                        rowAffects += _dbConnection.Execute($"Proc_DeleteTaxById", param, commandType: CommandType.StoredProcedure, transaction: transaction);
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
    }
    #endregion
}
