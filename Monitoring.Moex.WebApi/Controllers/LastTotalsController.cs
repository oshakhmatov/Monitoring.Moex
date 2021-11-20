using Microsoft.AspNetCore.Mvc;
using Monitoring.Moex.Core.Dto.SecurityTotals;
using Monitoring.Moex.Core.Rules.QueryHandlers.LastTotals;
using Monitoring.Moex.Core.Rules.QueryHandlers.LastTotals.ViewModels;

namespace Monitoring.Moex.WebApi.Controllers
{
    public class LastTotalsController : AppController
    {
        [HttpGet]
        public async Task<LastTotalsVm?> GetLastTotals([FromServices] GetLastTotalsQh getLastTotalsQh) =>
            await getLastTotalsQh.HandleAsync();

        [HttpGet]
        public async Task<SecurityTotalShortDto?> GetHighestUp([FromServices] GetHighestUpQh getHighestUpQh) =>
            await getHighestUpQh.HandleAsync();

        [HttpGet]
        public async Task<SecurityTotalShortDto?> GetHighestDown([FromServices] GetHighestDownQh getHighestDownQh) =>
            await getHighestDownQh.HandleAsync();
    }
}
