using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ptcent.Cloud.Drive.WebApi.Application.Interface.File;
using Ptcent.Cloud.Drive.WebApi.Domain.Dto.Common;
using Ptcent.Cloud.Drive.WebApi.Domain.Dto.Request;
using Ptcent.Cloud.Drive.WebApi.Domain.Dto.Response;
using Ptcent.Cloud.Drive.WebApi.Filters;

namespace Ptcent.Cloud.Drive.WebApi.Controllers
{
    /// <summary>
    /// item模块
    /// </summary>
    public class ItemController : BaseController
    {
        private readonly IConfiguration config;
        private readonly IFileService fileService;
        private readonly IHttpContextAccessor httpContextAccessor;
        public ItemController(IConfiguration config, IFileService fileService, IHttpContextAccessor httpContextAccessor) : base(config)
        {
            this.config = config;
            this.fileService = fileService;
            this.httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="upLoadFileRequestDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseMessageDto<bool>> UpLoadFile([FromForm] UpLoadFileRequestDto upLoadFileRequestDto)
        {
            return await fileService.UpLoadFile(upLoadFileRequestDto, httpContextAccessor.HttpContext.Request.Form.Files, CurrentUserLogintDto.UserId);
        }
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="createFolderRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidRequestParameterFilter]
        public async Task<ResponseMessageDto<bool>> CreateFolder(CreateFolderRequest createFolderRequest)
        {
            return await fileService.CreateFolder(createFolderRequest, CurrentUserLogintDto.UserId);
        }
        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="fileRequestDato"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseMessageDto<List<FileReponseDto>>> GetFiles(FileRequestDato fileRequestDato)
        {
            return await fileService.GetFiles(fileRequestDato, CurrentUserLogintDto.UserId);
        }
    }
}
