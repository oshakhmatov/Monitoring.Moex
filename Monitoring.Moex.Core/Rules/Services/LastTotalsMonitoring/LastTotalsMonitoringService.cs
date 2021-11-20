using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Monitoring.Moex.Core.DataAccess.Repos;
using Monitoring.Moex.Core.Rules.Services.LastTotalsMonitoring.Helpers;
using Monitoring.Moex.Core.SharedOptions;

namespace Monitoring.Moex.Core.Rules.Services.LastTotalsMonitoring
{
    public class LastTotalsMonitoringService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly TotalsMonitoringOptions _totalsMonitorOptions;
        private readonly MoexAuthOptions _moexAuthOptions;

        public LastTotalsMonitoringService(
            IServiceScopeFactory serviceScopeFactory,
            IOptionsMonitor<MoexAuthOptions> moexAuthOptions,
            IOptionsMonitor<TotalsMonitoringOptions> totalsMonitorOptions)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _totalsMonitorOptions = totalsMonitorOptions.CurrentValue;
            _moexAuthOptions = moexAuthOptions.CurrentValue;
        }

        public async Task RunAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_totalsMonitorOptions.Interval, stoppingToken);
                //var cookies = new CookieContainer();
                //var handler = new HttpClientHandler
                //{
                //    CookieContainer = cookies
                //};

                //using var client = new HttpClient(handler);
                //client.SetCredentials(_moexAuthOptions.Login, _moexAuthOptions.Password);

                //var uri = new Uri(_moexAuthOptions.Url);
                //var result = await client.GetAsync(uri, stoppingToken);
                //var responseCookies = cookies.GetCookies(uri).Cast<Cookie>();

                //var token = await result.Content.ReadAsStringAsync(stoppingToken);
                //cookies.Add(uri, new Cookie("MicexPassportCert", token));
                using var scope = _serviceScopeFactory.CreateScope();
                using var client = new HttpClient();
                var content = await client.GetStringAsync(_totalsMonitorOptions.LastTotalsUrl, stoppingToken);

                var parseTotalsHelper = scope.ServiceProvider.GetRequiredService<IParseTotalsHelper>();
                var totals = parseTotalsHelper.Parse(content).Where(s => s.IsNotEmpty()).ToArray();

                var totalsRepo = scope.ServiceProvider.GetRequiredService<ITotalsRepo>();
                var lastTradeClock = await totalsRepo.GetMaxTradeClockAsync();

                if (lastTradeClock == totals.First().TradeClock)
                    continue;

                var securitiesRepo = scope.ServiceProvider.GetRequiredService<ISecuritiesRepo>();
                var securityIds = await securitiesRepo.GetAllSecurityIdsAsync();

                var totalsToSave = totals.Where(t => securityIds.Contains(t.SecurityId));
                Parallel.ForEach(totalsToSave, (t) => t.OpenCloseDelta = GetPercentageDelta(t.Open!.Value, t.Close!.Value, digitsAfterPoint: 2));

                await totalsRepo.AddRangeAsync(totalsToSave);
            }
        }

        private double GetPercentageDelta(double first, double second, int digitsAfterPoint)
        {
            var absPercentage = Math.Round(100 * (Math.Abs(first - second) / first), digitsAfterPoint);
            return first > second ? absPercentage * (-1) : absPercentage;
        }
    }
}
