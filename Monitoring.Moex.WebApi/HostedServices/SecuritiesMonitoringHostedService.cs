using Monitoring.Moex.Core.Proccesses.SecuritiesMonitoring;

namespace Monitoring.Moex.WebApi.HostedServices
{
    public class SecuritiesMonitoringHostedService : BackgroundService
    {
        private readonly SecuritiesMonitoring _securitiesMonitoringProccess;

        public SecuritiesMonitoringHostedService(SecuritiesMonitoring securitiesMonitoringProccess)
        {
            _securitiesMonitoringProccess = securitiesMonitoringProccess;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _securitiesMonitoringProccess.RunAsync(stoppingToken);
        }
    }
}
