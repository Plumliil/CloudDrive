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
    /// 注册账号
    /// </summary>
    public class RegistrationAccountRequestDto : IValidatableObject
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int? Sex { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var registrationAccountRequestDto = validationContext.ObjectInstance as RegistrationAccountRequestDto;
            if (registrationAccountRequestDto == null)
            {
                string msg = "注册信息不能为空";
                yield return new ValidationResult(msg);
            }
            if (registrationAccountRequestDto.Phone.IsNullOrWhiteSpace() || !registrationAccountRequestDto.Phone.IsMobile())
            {

                yield return new ValidationResult("手机号不能为空或者格式不正确!");
            }
            if (registrationAccountRequestDto.Email != null && !registrationAccountRequestDto.Email.IsEmail())
            {
                yield return new ValidationResult("邮件格式不正确!");
            }
            if (registrationAccountRequestDto.PassWord.IsNullOrWhiteSpace())
            {
                yield return new ValidationResult("密码不能为空!");
            }
        }
    }
}
