using Monitoring.Moex.Core.Proccesses.SecuritiesMonitoring;

namespace Monitoring.Moex.WebApi.HostedServices
{
    public class SecuritiesMonitoringHostedService : BackgroundService
    {
        private readonly SecuritiesMonitoringProccess _securitiesMonitoringProccess;

        public SecuritiesMonitoringHostedService(SecuritiesMonitoringProccess securitiesMonitoringProccess)
        {
            _securitiesMonitoringProccess = securitiesMonitoringProccess;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _securitiesMonitoringProccess.RunAsync(stoppingToken);
        }
    }
}
