using Monitoring.Moex.Core.Proccesses.LastTotalsMonitoring;

namespace Monitoring.Moex.WebApi.HostedServices
{
    public class LastTotalsMonitoringHostedService : BackgroundService
    {
        private readonly LastTotalsMonitoring _lastTotalsMonitoringProccess;

        public LastTotalsMonitoringHostedService(LastTotalsMonitoring lastTotalsMonitoringProccess)
        {
            _lastTotalsMonitoringProccess = lastTotalsMonitoringProccess;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _lastTotalsMonitoringProccess.RunAsync(stoppingToken);
        }
    }
}
