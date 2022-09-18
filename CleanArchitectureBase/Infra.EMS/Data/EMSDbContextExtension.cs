using Infra.EMS.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra.EMS.Data
{
    public class EMSDbContextExtension : DbContext
    {
        public EMSDbContextExtension()
        {
        }

        public EMSDbContextExtension(DbContextOptions<EMSDbContext> options)
            : base(options)
        {
        }


    }
}
