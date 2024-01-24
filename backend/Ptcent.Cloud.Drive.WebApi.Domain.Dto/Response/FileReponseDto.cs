using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ptcent.Cloud.Drive.WebApi.Domain.Dto.Response
{
    public class FileReponseDto
    {
        public long Id { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string LeafName { get; set; }
        /// <summary>
        /// 扩展名
        /// </summary>
        public string? Extension { get; set; }
        /// <summary>
        /// 中文路径
        /// </summary>
        public string? Path { get; set; }
        /// <summary>
        /// 父级文件Id
        /// </summary>
        public long? ParentFolderId { get; set; }
        /// <summary>
        /// Idpath
        /// </summary>
        public string Idpath { get; set; }
        /// <summary>
        /// 是否是文件夹 1、是 0、否
        /// </summary>
        public int IsFolder { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public int? FileType { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public int? IsDel { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long? FileSize { get; set; }
    }
}
