using Minio;
using Minio.DataModel.Args;
using Ptcent.Cloud.Drive.WebApi.Application.Interface.File;
using Ptcent.Cloud.Drive.WebApi.Domain.Dto.Common;
using Ptcent.Cloud.Drive.WebApi.Infrastructure;

namespace Ptcent.Cloud.Drive.WebApi.Application.Implement.File
{
    public class MinIoServiceImpl : IMinIOService
    {
        private readonly IMinioClient minioClient;
        public MinIoServiceImpl(IMinioClient minioClient) {
            this.minioClient = minioClient;
        }
        /// <summary>
        /// 检查存储桶是否存在
        /// </summary>
        /// <param name="bucketName">存储桶名称</param>
        /// <returns></returns>
        public async Task<bool> CheckBucket(string bucketName)
        {
            BucketExistsArgs args = new BucketExistsArgs().WithBucket(bucketName);
            return await minioClient.BucketExistsAsync(args);
        }
        /// <summary>
        /// 创建桶
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="loc"></param>
        /// <returns></returns>
        public async Task<ResponseMessageDto<bool>> MakeBucket(string bucketName, string loc = "us-east-1")
        {
            var response=new ResponseMessageDto<bool>() { IsSuccess=true};
            try
            {
                //先检测桶是否存在
                var found = await CheckBucket(bucketName);
                if (!found)
                {
                    response.IsSuccess = false;
                    response.Message = $"桶{bucketName}已经存在,不可重复创建";
                    return response;
                }
                MakeBucketArgs args = new MakeBucketArgs().WithBucket(bucketName).WithLocation(loc);
                await minioClient.MakeBucketAsync(args);
                response.IsSuccess = true;
                response.Message = "创建成功";
                return response;
            }
            catch (Exception ex)
            {
                LogUtil.Error("MakeBucket:" + ex.Message);
                response.IsSuccess = false;
                response.Message = "桶创建失败";
                return response;
            }
        }
        /// <summary>
        /// 删除桶
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResponseMessageDto<bool>> RemoveBucket(string bucketName, CancellationToken cancellationToken = default)
        {
            var response = new ResponseMessageDto<bool>() { IsSuccess = true };
            try
            {
                var exsistFlag = await CheckBucket(bucketName);
                if (!exsistFlag)
                {
                    response.IsSuccess = false;
                    response.Message = "桶不存在";
                    return response;
                }
                RemoveBucketArgs args = new RemoveBucketArgs().WithBucket(bucketName);
                await minioClient.RemoveBucketAsync(args);
                response.Message = "操作成功";
                return response;
            }
            catch (Exception ex)
            {
                LogUtil.Error("RemoveBucket:" + ex.Message);
                response.IsSuccess= false;
                response.Message = "桶删除失败";
                return response;    
            }
        }
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
        public async Task<ResponseMessageDto<bool>> PutObjectAsync(string bucketName, string objectName, Stream data, long size, string contentType = "application/octet-stream", Dictionary<string, string> metaData = null)
        {
           var response=new ResponseMessageDto<bool>() { IsSuccess=true };
            try
            {
                var existFlag = await CheckBucket(bucketName);
                if (existFlag)
                {
                    PutObjectArgs args = new PutObjectArgs().WithBucket(bucketName)
                                            .WithObject(objectName)
                                            .WithStreamData(data)
                                            .WithObjectSize(size)
                                            .WithContentType(GetContentTypeUtil.GetContentType(objectName))
                                            .WithHeaders(metaData)
                                            .WithServerSideEncryption(null);
                  await minioClient.PutObjectAsync(args);
                    response.Message= "OK";
                    return response;
                }
                response.IsSuccess = false;
                response.Message = "桶不存在";
                return response;
            }
            catch (Exception ex)
            {
                LogUtil.Error("PutObjectAsync:" + ex.Message);
                response.IsSuccess = false;
                response.Message = "上传文件失败";
                return response;
            }
        }
    }
}
