using Ptcent.Cloud.Drive.WebApi.Domain.Entities.Item;
using Ptcent.Cloud.Drive.WebApi.Domain.IRepositories.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ptcent.Cloud.Drive.WebApi.Domain.Repositories.File
{
    public class FileRepository : BaseRepository<FileEntity>, IFileRepository
    {
        public FileRepository(EFDbContext db) : base(db)
        {
        }
    }
}
