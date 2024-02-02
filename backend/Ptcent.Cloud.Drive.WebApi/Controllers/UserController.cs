using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ptcent.Cloud.Drive.WebApi.Application.Interface;
using Ptcent.Cloud.Drive.WebApi.Components;
using Ptcent.Cloud.Drive.WebApi.Domain.Dto.Common;
using Ptcent.Cloud.Drive.WebApi.Domain.Dto.Request;
using Ptcent.Cloud.Drive.WebApi.Filters;

namespace Ptcent.Cloud.Drive.WebApi.Controllers
{
    /// <summary>
    /// 用户模块
    /// </summary>
    public class UserController : BaseController
    {
        private readonly IConfiguration configuration;
        private readonly IUserService userService;
        public UserController(IConfiguration config, IUserService userService) : base(config)
        {
            this.configuration = config;
            this.userService = userService;
        }
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="registrationAccountRequestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidRequestParameterFilter]
        [AllowAnonymous]
        public async Task<ResponseMessageDto<bool>> RegistrationAccount(RegistrationAccountRequestDto registrationAccountRequestDto)
        {
            try
            {
                return await userService.RegistrationAccount(registrationAccountRequestDto);
            }
            catch (Exception ex)
            {

                return UtilResponseMessage.CaptureException<bool>(ex, "RegistrationAccount");
            }
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginUserRequestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidRequestParameterFilter]
        public async Task<ResponseMessageDto<string>> Login(LoginUserRequestDto loginUserRequestDto)
        {
            return await userService.Login(loginUserRequestDto);
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ResponseMessageDto<bool>> SignOut(long userId)
        {
            ;
            return await userService.SignOut(userId);
        }
    }
}
