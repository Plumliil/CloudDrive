using Ptcent.Cloud.Drive.Domain.Entities;
using Ptcent.Cloud.Drive.Infrastructure.Context;
using Ptcent.Cloud.Drive.Infrastructure.IRespository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ptcent.Cloud.Drive.Infrastructure.Respository
{
    public class FileRepository : BaseRepository<FileEntity>, IFileRepository
    {
        public FileRepository(EFDbContext db) : base(db)
        {
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
                await Add(fileEntity);
                return fileEntity;
            }
            else
            {
                return fileEntity;
            }
        }
    }
}
