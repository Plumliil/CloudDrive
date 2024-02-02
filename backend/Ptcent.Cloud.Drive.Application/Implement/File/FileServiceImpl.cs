using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ptcent.Cloud.Drive.WebApi.Application.Interface.File;
using Ptcent.Cloud.Drive.WebApi.Application.Mapping;
using Ptcent.Cloud.Drive.WebApi.Domain.Dto.Common;
using Ptcent.Cloud.Drive.WebApi.Domain.Dto.Request;
using Ptcent.Cloud.Drive.WebApi.Domain.Dto.Response;
using Ptcent.Cloud.Drive.WebApi.Domain.Entities.Item;
using Ptcent.Cloud.Drive.WebApi.Infrastructure;
using Ptcent.Cloud.Drive.WebApi.Domain.IRepositories.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Yitter.IdGenerator;

namespace Ptcent.Cloud.Drive.WebApi.Application.Implement.File
{
    public class FileServiceImpl : BaseServiceImpl<FileEntity>, IFileService
    {
        private readonly IFileRepository fileRepository;
        private readonly IIdGenerator idGenerator;
        private readonly IConfiguration config;
        public FileServiceImpl(IFileRepository fileRepository, IConfiguration config, IIdGenerator idGenerator) : base(fileRepository)
        {
            this.fileRepository = fileRepository;
            this.config = config;
            this.idGenerator = idGenerator;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="upLoadFileRequestDto"></param>
        /// <param name="formFiles"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseMessageDto<bool>> UpLoadFile(UpLoadFileRequestDto upLoadFileRequestDto, IFormFileCollection formFiles, long userId)
        {
            var response = new ResponseMessageDto<bool>();
            var fileEntitys = new List<FileEntity>();
            List<string> itemPaths = new List<string>();
            //获取IFormFileCollection  集合中文件名 判断类型是否允许上传
            var allowUploadFileType = config["AllowUploadFileType"];
            if (string.IsNullOrEmpty(allowUploadFileType))
            {
                response.IsSuccess = false;
                response.Message = $"allowUploadFileType配置节点不存在";
                return response;
            }
            var allowUploadFileTypes = allowUploadFileType.Split(',').ToList();
            var fileTypes = formFiles.Select(a => Path.GetExtension(a.FileName)).ToList();
            var excludeFileTypes = allowUploadFileTypes.Intersect(fileTypes).ToList(); ;
            if (!excludeFileTypes.IsNull())
            {
                string excludeFileType = string.Join(",", excludeFileTypes);
                response.IsSuccess = false;
                response.Message = $"{excludeFileType}类型文件不允许上传";
                return response;
            }
            foreach (var file in formFiles)
            {
                //获取文件的Hash值
                var fileName = Path.GetFileName(file.FileName);
                var exName = Path.GetExtension(fileName);
                FileTypeEnum fileType = CommonExtension.JudgmentFileType(exName);
                long fileId = idGenerator.NewLong();
                long versionId = idGenerator.NewLong();
                //根据Hash 判断 数据库有没有 Hash值一样的文件 如果有 则不需要重新上传
                var fileIsExit = await fileRepository.FirstOrDefaultAsync(a => a.ItemHash == upLoadFileRequestDto.FileHash);
                FileEntity fileEntity = new FileEntity();
                fileEntity.Id = fileId;
                fileEntity.VersionId = versionId;
                fileEntity.LeafName = fileName;
                fileEntity.FileType = (int)Enum.Parse(typeof(FileTypeEnum), fileType.ToString());
                fileEntity.Extension = exName;
                fileEntity.ParentFolderId = upLoadFileRequestDto.ParentFloderId == null ? null : upLoadFileRequestDto.ParentFloderId;
                fileEntity.IsFolder = (int)EnumItemType.ItemFile;
                fileEntity.FileSize = file.Length;
                //存在 PhysicalDirectory 字段下 如果编辑该文件了 则需要更新 ItemFileMapUrl字段
                if (fileEntity.ParentFolderId == null)
                {
                    //证明是在最顶层创建的
                    fileEntity.Path = $"/{fileName}";
                    fileEntity.Idpath = $"/{fileId}";
                }
                else
                {
                    //证明是在某个文件夹下方 则需要查询 该文件的Path IdPath
                    var parentFileEntity = await fileRepository.FirstOrDefaultAsync(a => a.Id == fileEntity.ParentFolderId);
                    fileEntity.Path = parentFileEntity.Path + $"/{fileName}";
                    fileEntity.Idpath = parentFileEntity.Idpath + $"/{fileId}";
                }
                if (fileIsExit != null)
                {
                    fileEntity.ItemHash = fileIsExit.ItemHash;
                    fileEntity.PhysicalDirectory = fileIsExit.PhysicalDirectory;
                    fileEntity = await SaveFileEntity(fileEntity, userId, false);
                    fileEntitys.Add(fileEntity);
                }
                else
                {
                    //没有则需要上传
                    fileEntity.ItemHash = upLoadFileRequestDto.FileHash;
                    //文件存储路径 Id+VersionId+Id.extension
                    string filePath = "SourceFiles" + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyy-MM-dd") + Path.DirectorySeparatorChar + fileEntity.Id.ToString() + Path.DirectorySeparatorChar + fileEntity.VersionId.ToString() + Path.DirectorySeparatorChar + fileEntity.Id.ToString() + fileEntity.Extension;
                    fileEntity.PhysicalDirectory = filePath;
                    fileEntity = await SaveFileEntity(fileEntity, userId, false);
                    fileEntitys.Add(fileEntity);
                    Stream stream = file.OpenReadStream();
                    string aimPath = Path.Combine(FileRootPath, filePath);
                    //上传MinIo
                    using (FileStream fs = new(aimPath, FileMode.Create, FileAccess.Write))
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.CopyTo(fs);
                    }
                    itemPaths.Add(aimPath);
                }
            }

            //事务
            using (TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await fileRepository.AddBatch(fileEntitys);
                }
                catch (Exception ex)
                {

                    LogUtil.Info("文件上传失败:" + ex.Message);
                    foreach (var itemPath in itemPaths)
                    {
                        //删除文件
                        System.IO.File.Delete(itemPath);
                        response.IsSuccess = false;
                    }
                }
                scope.Complete();
            }
            response.IsSuccess = true;
            response.Message = "上传成功!";
            return response;
        }

        /// <summary>
        /// 保存文件实体
        /// </summary>
        /// <param name="fileEntity">实体</param>
        /// <param name="userId">用户Id</param>
        /// <param name="isSave">是否保存</param>
        /// <returns></returns>
        public async Task<FileEntity> SaveFileEntity(FileEntity fileEntity, long userId, bool isSave = false)
        {

            fileEntity.CreatedDate = DateTime.Now;
            fileEntity.CreatedBy = userId;
            if (isSave)
            {
                await fileRepository.Add(fileEntity);
                return fileEntity;
            }
            else
            {
                return fileEntity;
            }
        }
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="createFolderRequest">文件夹入参</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public async Task<ResponseMessageDto<bool>> CreateFolder(CreateFolderRequest createFolderRequest, long userId)
        {
            var response = new ResponseMessageDto<bool>();
            long? folderId = createFolderRequest.ParentFolderId == null ? null : createFolderRequest.ParentFolderId;
            //判断要创建的文件夹名称是否存在
            var folderExit = await fileRepository.Any(a => a.LeafName == createFolderRequest.FolderName && a.ParentFolderId == folderId && a.IsDel == (int)FileStatsType.NoDel);
            if (folderExit)
            {
                response.IsSuccess = false;
                response.Message = $"存在相同名称{createFolderRequest.FolderName}的文件夹!";
                return response;
            }
            //
            FileEntity fileEntity = new FileEntity();
            fileEntity.Id = idGenerator.NewLong();
            fileEntity.ParentFolderId = folderId;
            fileEntity.VersionId = idGenerator.NewLong();
            fileEntity.LeafName = createFolderRequest.FolderName;
            fileEntity.FileSize = 0;
            fileEntity.Extension = string.Empty;
            fileEntity.FileType = (int)FileTypeEnum.Folder;
            fileEntity.IsDel = (int)FileStatsType.NoDel;
            if (folderId == null)
            {
                //代表在最顶成创建
                fileEntity.Path = $"/{fileEntity.LeafName}";
                fileEntity.Idpath = $"/{fileEntity.Idpath}";
            }
            else
            {
                var parentFileEntity = (await fileRepository.WhereAsync(a => a.ParentFolderId == createFolderRequest.ParentFolderId)).Select(a => new
                {
                    a.Path,
                    a.Idpath
                }).FirstOrDefault();
                fileEntity.Path = parentFileEntity?.Path + $"/{fileEntity.LeafName}";
                fileEntity.Idpath = parentFileEntity?.Idpath + $"/{fileEntity.Idpath}";
            }
            await SaveFileEntity(fileEntity, userId, true);
            response.IsSuccess = true;
            response.Message = "创建成功";
            return response;
        }

        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="fileRequestDato"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseMessageDto<List<FileReponseDto>>> GetFiles(FileRequestDato fileRequestDato, long userId)
        {
            var response = new ResponseMessageDto<List<FileReponseDto>>();
            var fileReponseDtos = new List<FileReponseDto>();
            var fileEntitys = await fileRepository.WhereAsync(a => a.CreatedBy == userId && a.IsDel == (int)FileStatsType.NoDel);
            if (!fileRequestDato.LeafName.IsNullOrWhiteSpace())
            {
                fileEntitys.Where(a => EF.Functions.Like(a.LeafName, $"%{fileRequestDato.LeafName}%"));
            }
            object fileType;
            try
            {
                fileType = Enum.Parse(typeof(FileTypeEnum), fileRequestDato.FileType);
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = "文件类型有误!";
                return response;
            }
            if ((int)FileTypeEnum.All == (int)fileType)
            {

            }
            else
            {
                fileEntitys.Where(a => a.FileType == (int)fileType);
            }
            var fileEntityList = fileEntitys.Skip((fileRequestDato.PageIndex - 1) * fileRequestDato.PageSize).Take(fileRequestDato.PageSize).OrderByDescending(a => a.CreatedDate).ToList();
            var tempData = AutoMapperConfig.Map<List<FileEntity>, List<FileReponseDto>>(fileEntityList);
            response.IsSuccess = true;
            response.Data = tempData;
            return response;
        }
    }
}
