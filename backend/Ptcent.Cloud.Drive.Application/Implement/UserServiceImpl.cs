using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Ptcent.Cloud.Drive.WebApi.Application.Interface;
using Ptcent.Cloud.Drive.WebApi.Domain.Dto.Common;
using Ptcent.Cloud.Drive.WebApi.Domain.Dto.Request;
using Ptcent.Cloud.Drive.WebApi.Domain.Dto.Response;
using Ptcent.Cloud.Drive.WebApi.Domain.Entities;
using Ptcent.Cloud.Drive.WebApi.Infrastructure.Cache;
using Ptcent.Cloud.Drive.WebApi.Infrastructure.Security;
using Ptcent.Cloud.Drive.WebApi.Infrastructure;
using Ptcent.Cloud.Drive.WebApi.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Yitter.IdGenerator;
using Microsoft.Extensions.Configuration;
using Ptcent.Cloud.Drive.WebApi.Application.Mapping;

namespace Ptcent.Cloud.Drive.WebApi.Application.Implement
{
    public class UserServiceImpl : BaseServiceImpl<UserEntity>, IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration config;
        private readonly IIdGenerator idGenerator;
        public UserServiceImpl(IUserRepository userRepository, IConfiguration config, IIdGenerator idGenerator) : base(userRepository)
        {
            this.userRepository = userRepository;
            this.config = config;
            this.idGenerator = idGenerator;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginUserRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseMessageDto<string>> Login(LoginUserRequestDto loginUserRequestDto)
        {
            var respone = new ResponseMessageDto<string>();
            var userEntity = (await userRepository.WhereAsync(a => a.Phone == loginUserRequestDto.Phone)).FirstOrDefault();
            if (userEntity != null)
            {
                //查询用户状态
                if (userEntity.IsDel == 1)
                {
                    respone.IsSuccess = false;
                    respone.Message = "用户已被禁用";
                    return respone;
                }
                string pwd = MD5Util.ComputeMD5(loginUserRequestDto.PassWord, loginUserRequestDto.Phone);
                if (!string.Equals(userEntity.Password, pwd, StringComparison.InvariantCultureIgnoreCase))
                {
                    respone.IsSuccess = false;
                    respone.Message = "密码错误!";
                    return respone;
                }
                var loginInfoResult = CreateToken(userEntity);
                if (loginInfoResult.IsSuccess)
                {
                    //将信息记录在Redis
                    respone.Data = loginInfoResult.Data;
                    RedisClient.Insert<string>(CacheKey.Ptcent_YiDoc_User_Login_Status + userEntity.Id, UserLoginStatus.Login.GetHashCode().ToString(), new TimeSpan(Convert.ToInt32(config["UserLoingExpires"] ?? "30"), 0, 0, 0));
                }
                else
                {
                    respone.IsSuccess = false;
                    respone.Message = "登录失败";
                    return respone;
                }
            }
            if (userEntity == null)
            {
                respone.IsSuccess = false;
                respone.Message = "用户不存在!";
                return respone;
            }
            respone.IsSuccess = true;
            return respone;
        }

        /// <summary>
        /// 注册账号
        /// </summary>
        /// <param name="registrationAccountRequestDto"></param>
        /// <returns></returns>
        public async Task<ResponseMessageDto<bool>> RegistrationAccount(RegistrationAccountRequestDto registrationAccountRequestDto)
        {
            var response = new ResponseMessageDto<bool>();

            //判断注册手机号是否存在 手机号唯一

            var phoneIsExit = await userRepository.Any(a => a.Phone == registrationAccountRequestDto.Phone);
            if (phoneIsExit)
            {
                response.IsSuccess = false;
                response.Message = "手机号不能重复";
                return response;
            }
            var userEntity = AutoMapperConfig.Map<RegistrationAccountRequestDto, UserEntity>(registrationAccountRequestDto);
            //userEntity.Id = Snowflake.Instance().GetId();
            userEntity.Id = idGenerator.NewLong();
            userEntity.UpdateDate = DateTime.Now;
            userEntity.CreateDate = DateTime.Now;
            userEntity.CreateBy = userEntity.Id;
            userEntity.UpdateBy = userEntity.Id;
            userEntity.Sex = 0;
            userEntity.Password = MD5Util.ComputeMD5(registrationAccountRequestDto.PassWord, registrationAccountRequestDto.Phone);
            userEntity.IsDel = 0;
            userEntity.RegisterTime = DateTime.Now;
            userEntity.Salt = registrationAccountRequestDto.Phone;
            userEntity.UserName = registrationAccountRequestDto.UserName;
            response.IsSuccess = await userRepository.Add(userEntity);
            response.Message = "注册成功";
            return response;
        }
        public ResponseMessageDto<string> CreateToken(UserEntity userEntity)
        {
            var response = new ResponseMessageDto<string>();
            try
            {

                var loginUserDto = new LoginUserDto();
                loginUserDto.UserId = userEntity.Id;
                loginUserDto.Phone = userEntity.Phone;
                loginUserDto.UserName = userEntity.UserName;
                loginUserDto.UserMail = userEntity.Email;
                var claims = new List<Claim>();
                claims.Add(new Claim("jwt", JsonConvert.SerializeObject(loginUserDto)));
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Authentication:SecretKey"]));
                var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(Convert.ToDouble(config["UserLoingExpires"] ?? "15")),
                    signingCredentials: signingCredentials);
                var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
                response.IsSuccess = true;
                response.Data = tokenStr;
                return response;
            }
            catch (Exception ex)
            {
                LogUtil.Error("Token生成错误!" + ex.Message);
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }


        }


        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseMessageDto<bool>> SignOut(long userId)
        {
            var response = new ResponseMessageDto<bool>() { IsSuccess = true };
            string cacheKey = CacheKey.Ptcent_YiDoc_User_Login_Status + userId;
            RedisClient.Remove(cacheKey);
            response.Message = "退出成功";
            return response;
        }
    }
}
