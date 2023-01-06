using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PG.Core.Dto;
using PG.Core.Entities;
using PG.Core.Enum;
using PG.Core.Interface.Service;
using PG.Core.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Dapper.SqlMapper;

namespace PG.API.Auth.Controllers
{
    [Route("api/v1/Accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        IAccountService _accountService;
        IUserService _userService;
        IConfiguration _configuration;

        public AccountsController(IConfiguration configuration, IAccountService accountService, IUserService userService)
        {
            _accountService = accountService;
            _userService = userService;
            _configuration = configuration;
        }

        /// <summary>
        /// Register
        /// </summary>
        [EnableCors("AllowCROSPolicy")]
        [HttpPost("register")]
        public IActionResult Register([FromBody]UserDtoEdit userDto)
        {
            var serviceResult = new ServiceResult();
            try
            {
                _userService.InsertEntity(userDto, typeof(User));

                _accountService.InsertEntity(new Account()
                {
                    account_id = Guid.NewGuid(),
                    username = userDto.username,
                    password = userDto.password,
                    user_id = userDto.user_id
                });

                serviceResult.Code = PGCode.Success;
            }
            catch (Exception ex)
            {
                serviceResult.Code = PGCode.Exception;
            }

            switch (serviceResult.Code)
            {
                case PGCode.Exception:
                    return StatusCode(500, serviceResult);
                case PGCode.InvalidData:
                    return BadRequest(serviceResult);
                default:
                    return StatusCode(201, serviceResult);
            }
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        [EnableCors("AllowCROSPolicy")]
        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {
            var serviceResult = new ServiceResult();
            var account = _accountService.VerifyLogin(username, password);

            if (account != null)
            {
                var user = _userService.GetEntityById(account.user_id);

                var token = GenerateToken(username, user);

                serviceResult.Code = PGCode.Success;
                serviceResult.Data = new
                {
                    user = user,
                    token = token
                };
            }

            switch (serviceResult.Code)
            {
                case PGCode.Exception:
                    return StatusCode(500, serviceResult);
                case PGCode.InvalidData:
                    return BadRequest(serviceResult);
                default:
                    return StatusCode(200, serviceResult);
            }
        }

        /// <summary>
        /// Sinh token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string GenerateToken(string username, User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, user.role.ToString(), ClaimValueTypes.Integer64)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Appsettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
