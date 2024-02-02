using Minio;
using Ptcent.Cloud.Drive.WebApi.Domain.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ptcent.Cloud.Drive.WebApi.Application.Interface.File
{
    public interface IMinIOService
    {
        /// <summary>
        /// 校验是否存在
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public Task<bool> CheckBucket(string bucketName);
        /// <summary>
        /// 删除一个桶
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ResponseMessageDto<bool>> RemoveBucket(string bucketName, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// 创建桶的路径
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="loc"></param>
        /// <returns></returns>
        public Task<ResponseMessageDto<bool>> MakeBucket( string bucketName, string loc = "us-east-1");
        /// <summary>
        /// 通过Stream上传对象
        /// </summary>
        /// <param name="bucketName">存储桶名称</param>
        /// <param name="objectName">存储桶里的对象名称</param>
        /// <param name="data">要上传的Stream对象</param>
        /// <param name="size">流的大小</param>
        /// <param name="contentType">文件的Content type</param>
        /// <param name="metaData">元数据头信息的Dictionary对象，默认是null</param>
        /// <returns></returns>
        public Task<ResponseMessageDto<bool>> PutObjectAsync(string bucketName, string objectName, Stream data, long size, string contentType = "application/octet-stream", Dictionary<string, string> metaData = null);
    }
}
