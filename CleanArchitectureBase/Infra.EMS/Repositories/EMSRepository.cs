using Core.Utils.Entities;
using Infra.EMS.Data.Models;
using Infra.Utils.Repositories;

namespace Infra.MIS.Repositories
{
    public class EMSRepository<T> : AsyncRepository<T> where T : BaseEntity
    {
        public EMSRepository(EMSDbContext dbContext) : base(dbContext)
        {
        }
    }
}
