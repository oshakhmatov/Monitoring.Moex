using Microsoft.EntityFrameworkCore;
using Monitoring.Moex.Core.DataAccess.Repos;
using Monitoring.Moex.Core.Models;

namespace Monitoring.Moex.Infrastructure.Data.Repos
{
    public class SecuritiesRepo : EfRepo<Security>, ISecuritiesRepo
    {
        public SecuritiesRepo(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<string>> GetAllSecurityIdsAsync()
        {
            return await _dbContext.Set<Security>()
                .Select(s => s.SecurityId)
                .ToListAsync();
        }
    }
}
