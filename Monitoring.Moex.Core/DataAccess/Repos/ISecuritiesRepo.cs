using Monitoring.Moex.Core.Models;

namespace Monitoring.Moex.Core.DataAccess.Repos
{
    public interface ISecuritiesRepo : IRepo<Security>
    {
        public Task<List<string>> GetAllSecurityIdsAsync();
    }
}
