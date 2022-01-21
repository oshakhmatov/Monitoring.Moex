using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Monitoring.Moex.Core.DataAccess.Repos;
using System.IO.Compression;

namespace Monitoring.Moex.Core.Proccesses.SecuritiesMonitoring
{
    public class SecuritiesMonitoring
    {
        private const string TEMP_DIR_NAME = "securities";

        private readonly IOptionsMonitor<SecuritiesMonitoringOptions> _securitiesMonitoringOptions;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SecuritiesMonitoring(
            IOptionsMonitor<SecuritiesMonitoringOptions> securitiesMonitoringOptions,
            IServiceScopeFactory serviceScopeFactory)
        {
            _securitiesMonitoringOptions = securitiesMonitoringOptions;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task RunAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var options = _securitiesMonitoringOptions.CurrentValue;

                await Task.Delay(options.Interval, stoppingToken);

                var tempFileName = Path.GetTempFileName();
                var tempDirPath = Path.Combine(Path.GetTempPath(), TEMP_DIR_NAME);

                var client = new HttpClient();
                var fileStream = new FileStream(tempFileName, FileMode.OpenOrCreate);
                var scope = _serviceScopeFactory.CreateScope();

                try
                {
                    client = new HttpClient();
                    var response = await client.GetAsync(options.SecuritiesUrl, stoppingToken);

                    await response.Content.CopyToAsync(fileStream, stoppingToken);
                    fileStream.Close();

                    ZipFile.ExtractToDirectory(tempFileName, tempDirPath);
                    var dirInfo = new DirectoryInfo(tempDirPath);
                    var extractedFileName = dirInfo.GetFiles().First().FullName;
                    var content = await File.ReadAllTextAsync(extractedFileName, stoppingToken);

                    var securitiesParser = scope.ServiceProvider.GetRequiredService<ISecuritiesParser>();
                    var securities = securitiesParser.Parse(content).Where(s => s.IsNotEmpty());

                    var securitiesRepo = scope.ServiceProvider.GetRequiredService<ISecuritiesRepo>();
                    var securityIds = await securitiesRepo.GetAllSecurityIdsAsync();

                    var newSecurities = securities.Where(s => !securityIds.Contains(s.SecurityId!));
                    await securitiesRepo.AddRangeAsync(newSecurities);
                    
                    var oldSecurities = securities.Where(s => securityIds.Contains(s.SecurityId!));
                    await securitiesRepo.UpdateRangeAsync(oldSecurities);
                }
                finally
                {
                    if (File.Exists(tempFileName)) File.Delete(tempFileName);
                    if (Directory.Exists(tempDirPath)) Directory.Delete(tempDirPath, recursive: true);

                    client?.Dispose();
                    fileStream?.Dispose();
                    scope?.Dispose();
                }
            }
        }
    }
}
