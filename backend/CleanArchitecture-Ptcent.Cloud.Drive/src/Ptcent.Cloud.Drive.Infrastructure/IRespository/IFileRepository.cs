using Ptcent.Cloud.Drive.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ptcent.Cloud.Drive.Infrastructure.IRespository
{
    public interface IFileRepository : IBaseRepository<FileEntity>
    {
        Task<FileEntity> SaveFileEntity(FileEntity fileEntity, long userId, bool isSave = false);
    }
}
