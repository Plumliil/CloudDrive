using Ptcent.Cloud.Drive.WebApi.Domain.Entities;
using Ptcent.Cloud.Drive.WebApi.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ptcent.Cloud.Drive.WebApi.Domain.Repositories
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        public UserRepository(EFDbContext dbContext) : base(dbContext)
        {

        }
    }
}
