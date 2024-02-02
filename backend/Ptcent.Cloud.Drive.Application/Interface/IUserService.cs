using Ptcent.Cloud.Drive.WebApi.Domain.Dto.Common;
using Ptcent.Cloud.Drive.WebApi.Domain.Dto.Request;
using Ptcent.Cloud.Drive.WebApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ptcent.Cloud.Drive.WebApi.Application.Interface
{
    public interface IUserService : IBaseService<UserEntity>
    {
        /// <summary>
        /// 注册账号
        /// </summary>
        /// <param name="registrationAccountRequestDto"></param>
        /// <returns></returns>
        Task<ResponseMessageDto<bool>> RegistrationAccount(RegistrationAccountRequestDto registrationAccountRequestDto);
        /// <summary>
        /// 登录账号
        /// </summary>
        /// <param name="loginUserRequestDto"></param>
        /// <returns></returns>
        Task<ResponseMessageDto<string>> Login(LoginUserRequestDto loginUserRequestDto);
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResponseMessageDto<bool>> SignOut(long userId);
    }
}
