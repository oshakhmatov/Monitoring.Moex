using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Monitoring.Moex.Core.DataAccess.Repos;
using System.IO.Compression;

namespace Monitoring.Moex.Core.Proccesses.SecuritiesMonitoring
{
    public class SecuritiesMonitoringProccess
    {
        private readonly SecuritiesMonitoringOptions _securitiesMonitoringOptions;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SecuritiesMonitoringProccess(
            IOptionsMonitor<SecuritiesMonitoringOptions> securitiesMonitoringOptions,
            IServiceScopeFactory serviceScopeFactory)
        {
            _securitiesMonitoringOptions = securitiesMonitoringOptions.CurrentValue;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task RunAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var client = new HttpClient();
                var response = await client.GetAsync(_securitiesMonitoringOptions.SecuritiesUrl, stoppingToken);

                var tempFileName = Path.GetTempFileName();
                var tempDirPath = Path.Combine(Path.GetTempPath(), "securities");
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    using var fs = new FileStream(tempFileName, FileMode.OpenOrCreate);
                    await response.Content.CopyToAsync(fs, stoppingToken);
                    fs.Close();

                    ZipFile.ExtractToDirectory(tempFileName, tempDirPath);
                    var dirInfo = new DirectoryInfo(tempDirPath);
                    var extractedFileName = dirInfo.GetFiles().First().FullName;
                    var content = await File.ReadAllTextAsync(extractedFileName, stoppingToken);

                    var parseSecuritiesHelper = scope.ServiceProvider.GetRequiredService<IParseSecuritiesHelper>();
                    var securities = parseSecuritiesHelper.Parse(content).Where(s => s.IsNotEmpty()).ToArray();

                    var securitiesRepo = scope.ServiceProvider.GetRequiredService<ISecuritiesRepo>();
                    var securityIds = await securitiesRepo.GetAllSecurityIdsAsync();

                    var newSecurities = securities.Where(s => !securityIds.Contains(s.SecurityId));
                    await securitiesRepo.AddRangeAsync(newSecurities);
                    
                    var oldSecurities = securities.Where(s => securityIds.Contains(s.SecurityId));
                    await securitiesRepo.UpdateRangeAsync(oldSecurities);
                }
                finally
                {
                    if (File.Exists(tempFileName)) File.Delete(tempFileName);
                    if (Directory.Exists(tempDirPath)) Directory.Delete(tempDirPath, true);
                }

                await Task.Delay(_securitiesMonitoringOptions.Interval, stoppingToken);
            }
        }
    }
}
