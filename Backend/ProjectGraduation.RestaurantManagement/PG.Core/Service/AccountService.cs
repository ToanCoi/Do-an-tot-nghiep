using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using PG.Core.Dto;
using PG.Core.Entities;
using PG.Core.Interface.Repository;
using PG.Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Service
{
    public class AccountService : BaseService<Account>, IAccountService
    {
        public AccountService(IAccountRepository accountRepository, IServiceProvider serviceProvider) : base(accountRepository, serviceProvider)
        {

        }

        /// <summary>
        /// Check login
        /// </summary>
        public Account? VerifyLogin(string username, string password)
        {
            var account = _baseRepository.GetEntityByProperty(nameof(Account.username), username);

            if (VerifyPassword(password, account.password, account.salt))
                return account;

            return null;
        }

        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm mới</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        public override ServiceResult InsertEntity(Account account)
        {
            HashPassword(account);

            return base.InsertEntity(account);
        }

        /// <summary>
        /// Hash password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private void HashPassword(Account account) {

            using (var hmac = new HMACSHA512())
            {
                account.salt = Convert.ToBase64String(hmac.Key);
                account.password = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(account.password)));
            }
        }

        /// <summary>
        /// Kiểm tra đăng nhập
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hashedPassword"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        private bool VerifyPassword(string password, string hashedPassword, string saltOrg)
        {
            using (var hmac = new HMACSHA512(Convert.FromBase64String(saltOrg)))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                return computedHash.SequenceEqual(Convert.FromBase64String(hashedPassword));
            }
        }
    }
}
