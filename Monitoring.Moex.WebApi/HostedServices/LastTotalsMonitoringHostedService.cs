using Monitoring.Moex.Core.Proccesses.LastTotalsMonitoring;

namespace Monitoring.Moex.WebApi.HostedServices
{
    public class LastTotalsMonitoringHostedService : BackgroundService
    {
        private readonly LastTotalsMonitoringProccess _lastTotalsMonitoringProccess;

        public LastTotalsMonitoringHostedService(LastTotalsMonitoringProccess lastTotalsMonitoringProccess)
        {
            _lastTotalsMonitoringProccess = lastTotalsMonitoringProccess;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _lastTotalsMonitoringProccess.RunAsync(stoppingToken);
        }
    }
}
