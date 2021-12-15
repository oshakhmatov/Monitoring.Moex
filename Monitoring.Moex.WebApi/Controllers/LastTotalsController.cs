using Microsoft.AspNetCore.Mvc;
using Monitoring.Moex.Core.Dto.SecurityTotals;
using Monitoring.Moex.Core.Services.SecurityTotals;

namespace Monitoring.Moex.WebApi.Controllers
{
    public class LastTotalsController : AppController
    {
        private readonly ISecurityTotalService _securityTotalService;

        public LastTotalsController(ISecurityTotalService securityTotalService)
        {
            _securityTotalService = securityTotalService;
        }

        [HttpGet]
        public async Task<LastTotalsViewModel?> GetLastTotals()
        {
            return await _securityTotalService.GetLastTotalsAsync();
        }

        [HttpGet]
        public async Task<SecurityTotalShortDto?> GetHighestUp()
        {
            return await _securityTotalService.GetHighestUpAsync();
        }

        [HttpGet]
        public async Task<SecurityTotalShortDto?> GetHighestDown()
        {
            return await _securityTotalService.GetHighestDownAsync();
        }
    }
}
