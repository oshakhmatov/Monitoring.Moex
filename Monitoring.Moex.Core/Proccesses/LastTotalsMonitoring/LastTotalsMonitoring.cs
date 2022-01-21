using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Monitoring.Moex.Core.DataAccess.Repos;
using Monitoring.Moex.Core.Models;

namespace Monitoring.Moex.Core.Proccesses.LastTotalsMonitoring
{
    public class LastTotalsMonitoring
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IOptionsMonitor<TotalsMonitoringOptions> _totalsMonitorOptions;

        public LastTotalsMonitoring(
            IServiceScopeFactory serviceScopeFactory,
            IOptionsMonitor<TotalsMonitoringOptions> totalsMonitorOptions)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _totalsMonitorOptions = totalsMonitorOptions;
        }

        public async Task RunAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var options = _totalsMonitorOptions.CurrentValue;

                await Task.Delay(options.Interval, stoppingToken);

                using var client = new HttpClient();
                var content = await client.GetStringAsync(options.LastTotalsUrl, stoppingToken);

                using var scope = _serviceScopeFactory.CreateScope();
                var totalsParser = scope.ServiceProvider.GetRequiredService<ITotalsParser>();
                var totals = totalsParser.Parse(content).Where(s => s.IsNotEmpty());

                var totalsRepo = scope.ServiceProvider.GetRequiredService<ITotalsRepo>();
                var lastTradeClock = await totalsRepo.GetMaxTradeClockAsync();

                if (DataIsNotActual(lastTradeClock, totals))
                    continue;

                var securitiesRepo = scope.ServiceProvider.GetRequiredService<ISecuritiesRepo>();
                var securityIds = await securitiesRepo.GetAllSecurityIdsAsync();

                var totalsToSave = totals.Where(t => securityIds.Contains(t.SecurityId!)).ToArray();

                Parallel.ForEach(totalsToSave, (total) =>
                {
                    total.OpenCloseDelta = GetPercentageDelta(total.Open!.Value, total.Close!.Value, digitsAfterPoint: 2);
                });

                await totalsRepo.AddRangeAsync(totalsToSave);
            }
        }

        private static double GetPercentageDelta(double first, double second, int digitsAfterPoint)
        {
            var absPercentage = Math.Round(100 * (Math.Abs(first - second) / first), digitsAfterPoint);

            return first > second ? absPercentage * (-1) : absPercentage;
        }

        private static bool DataIsNotActual(long? lastTradeClock, IEnumerable<SecurityTotal> totals)
        {
            return lastTradeClock == totals.First().TradeClock;
        }
    }
}
