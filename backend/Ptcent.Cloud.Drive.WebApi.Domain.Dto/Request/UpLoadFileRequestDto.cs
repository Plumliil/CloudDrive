using Ptcent.Cloud.Drive.WebApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ptcent.Cloud.Drive.WebApi.Domain.Dto.Request
{
    public class UpLoadFileRequestDto
    {
        /// <summary>
        /// 父文件Id
        /// </summary>
        public long? ParentFloderId { get; set; }
        /// <summary>
        /// 文件哈希值
        /// </summary>
        public string? FileHash { get; set; }
    }
    /// <summary>
    /// 创建文件夹Dto
    /// </summary>
    public class CreateFolderRequest : IValidatableObject
    {
        /// <summary>
        /// 文件夹名称
        /// </summary>
        public string FolderName { get; set; }
        /// <summary>
        /// 上级文件夹Id 最顶层 传空
        /// </summary>
        public long? ParentFolderId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var createFolderRequest = validationContext.ObjectInstance as CreateFolderRequest;
            if (createFolderRequest == null || createFolderRequest.FolderName.IsNullOrWhiteSpace())
            {
                yield return new ValidationResult("文件夹名称不能为空");
            }
        }
    }
}
