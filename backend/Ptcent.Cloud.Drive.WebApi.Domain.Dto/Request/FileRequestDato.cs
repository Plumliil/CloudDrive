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
    /// 文件入参
    /// </summary>
    public class FileRequestDato : IValidatableObject
    {
        /// <summary>
        /// 要查询的类型
        /// </summary>
        public string FileType { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string? LeafName { get; set; }
        /// <summary>
        /// 当前页数
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var fileRequestDato = validationContext.ObjectInstance as FileRequestDato;
            if (fileRequestDato == null || fileRequestDato.FileType.IsNullOrWhiteSpace())
            {
                yield return new ValidationResult("参数或文件类型不能为空!");
            }
        }
    }
}
