
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ptcent.Cloud.Drive.Application.Dto.ReponseModels;
using Ptcent.Cloud.Drive.Application.Dto.RequestModels;

namespace Ptcent.Cloud.Drive.Web.Controllers
{
    /// <summary>
    /// Item模块
    /// </summary>
    public class ItemController : BaseController
    {
        private readonly IConfiguration config;
        private readonly IMediator mediator;
        private readonly IHttpContextAccessor httpContextAccessor;
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="config"></param>
        /// <param name="mediator"></param>
        /// <param name="httpContextAccessor"></param>
        public ItemController(IConfiguration config, IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(config)
        {
            this.config = config;
            this.mediator = mediator;
            this.httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="createFolderRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseMessageDto<bool>> CreateFolder(CreateFolderRequestDto createFolderRequest)
        {
            return await mediator.Send(createFolderRequest);
        }
        /// <summary>
        /// 检查文件Hash
        /// </summary>
        /// <param name="fileHash"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseMessageDto<bool>> CheckFileHash(string fileHash)
        {
            return await mediator.Send(new CheckFileHashRequestDto { FileHash=fileHash});
        }
        /// <summary>
        /// 分块上传
        /// </summary>
        /// <param name="upLoadFileRequestDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseMessageDto<bool>> BlockUpload([FromForm] UpLoadFileRequestDto upLoadFileRequestDto)
        {
            return await mediator.Send(upLoadFileRequestDto);
        }
        /// <summary>
        /// 合并
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseMessageDto<bool>> MergeChunkFile(MergeChunkFileRequestDto mergeChunkFileRequestDto)
        {
            return await mediator.Send(mergeChunkFileRequestDto);
        }
    }
}
