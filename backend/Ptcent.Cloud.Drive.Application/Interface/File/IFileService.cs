using Microsoft.AspNetCore.Http;
using Ptcent.Cloud.Drive.WebApi.Domain.Dto.Common;
using Ptcent.Cloud.Drive.WebApi.Domain.Dto.Request;
using Ptcent.Cloud.Drive.WebApi.Domain.Dto.Response;
using Ptcent.Cloud.Drive.WebApi.Domain.Entities.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ptcent.Cloud.Drive.WebApi.Application.Interface.File
{
    public interface IFileService : IBaseService<FileEntity>
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="upLoadFileRequestDto"></param>
        /// <param name="formFiles"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResponseMessageDto<bool>> UpLoadFile(UpLoadFileRequestDto upLoadFileRequestDto, IFormFileCollection formFiles, long userId);
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="createFolderRequest">文件夹入参</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        Task<ResponseMessageDto<bool>> CreateFolder(CreateFolderRequest createFolderRequest, long userId);
        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="fileRequestDato"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResponseMessageDto<List<FileReponseDto>>> GetFiles(FileRequestDato fileRequestDato, long userId);
    }
}
