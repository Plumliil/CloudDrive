using Ptcent.Cloud.Drive.WebApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ptcent.Cloud.Drive.WebApi.Domain.Dto.Request
{
    /// <summary>
    /// 登录账号入参
    /// </summary>
    public class LoginUserRequestDto : IValidatableObject
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var loginUserRequestDto = validationContext.ObjectInstance as LoginUserRequestDto;
            if (loginUserRequestDto == null)
            {
                yield return new ValidationResult("参数不能为空");
            }
            if (!loginUserRequestDto.Phone.IsMobile())
            {
                yield return new ValidationResult("手机号格式有误");
            }
        }
    }
}
