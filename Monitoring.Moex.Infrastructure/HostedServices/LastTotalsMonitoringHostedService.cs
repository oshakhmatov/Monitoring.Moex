using Microsoft.Extensions.Hosting;
using Monitoring.Moex.Core.Rules.Services.LastTotalsMonitoring;

namespace Monitoring.Moex.Infrastructure.HostedServices
{
    public class LastTotalsMonitoringHostedService : BackgroundService
    {
        private readonly LastTotalsMonitoringService _lastTotalsMonitoring;

        public LastTotalsMonitoringHostedService(LastTotalsMonitoringService lastTotalsMonitoring)
        {
            _lastTotalsMonitoring = lastTotalsMonitoring;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _lastTotalsMonitoring.RunAsync(stoppingToken);
        }
    }
}
