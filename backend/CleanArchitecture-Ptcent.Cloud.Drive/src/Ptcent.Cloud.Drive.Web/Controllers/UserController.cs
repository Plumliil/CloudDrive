using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ptcent.Cloud.Drive.Application.Dto.ReponseModels;
using Ptcent.Cloud.Drive.Application.Dto.RequestModels;

namespace Ptcent.Cloud.Drive.Web.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserController : BaseController
    {
        private readonly IConfiguration config;
        private readonly IMediator mediator;
        public UserController(IConfiguration config, IMediator mediator) : base(config)
        {
            this.config = config;
            this.mediator = mediator;
        }
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="registrationAccountRequestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseMessageDto<bool>> RegistrationAccount(RegistrationAccountRequestDto registrationAccountRequestDto)
        {
            return await mediator.Send(registrationAccountRequestDto);
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginUserRequestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseMessageDto<string>> Login(LoginUserRequestDto loginUserRequestDto)
        {
            return await mediator.Send(loginUserRequestDto);
        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ResponseMessageDto<bool>> SignOut(long userId)=>await mediator.Send(new UserQueryRequestDto { UserId = userId });
    }
}
