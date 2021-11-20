using Microsoft.Extensions.Hosting;
using Monitoring.Moex.Core.Rules.Services.SecuritiesMonitoring;

namespace Monitoring.Moex.Infrastructure.HostedServices
{
    public class SecuritiesMonitoringHostedService : BackgroundService
    {
        private readonly SecuritiesMonitoringService _securitiesMonitoringService;

        public SecuritiesMonitoringHostedService(SecuritiesMonitoringService securitiesMonitoringService)
        {
            _securitiesMonitoringService = securitiesMonitoringService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _securitiesMonitoringService.RunAsync(stoppingToken);
        }
    }
}
