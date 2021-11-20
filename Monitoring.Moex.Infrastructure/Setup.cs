using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monitoring.Moex.Core.DataAccess;
using Monitoring.Moex.Core.Rules.Services.LastTotalsMonitoring;
using Monitoring.Moex.Core.Rules.Services.SecuritiesMonitoring;
using Monitoring.Moex.Core.SharedOptions;
using Monitoring.Moex.Infrastructure.Data;
using Monitoring.Moex.Infrastructure.HostedServices;
using StackExchange.Redis;
using System.Reflection;

namespace Monitoring.Moex.Infrastructure
{
    public static class Setup
    {
        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<DbConnectionOptions>(config.GetSection(nameof(DbConnectionOptions)));
            services.Configure<MoexAuthOptions>(config.GetSection(nameof(MoexAuthOptions)));
            services.Configure<TotalsMonitoringOptions>(config.GetSection(nameof(TotalsMonitoringOptions)));
            services.Configure<SecuritiesMonitoringOptions>(config.GetSection(nameof(SecuritiesMonitoringOptions)));

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
        {
            var dbConnOptions = new DbConnectionOptions();
            config.GetSection(nameof(DbConnectionOptions)).Bind(dbConnOptions);

            var dbConnOptsBuilder = new DbContextOptionsBuilder();
            dbConnOptsBuilder.UseNpgsql(dbConnOptions.ConnectionString);

            var dbContext = new AppDbContext(dbConnOptsBuilder.Options);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            return services.AddDbContext<AppDbContext>(o => o.UseNpgsql(dbConnOptions.ConnectionString));
        }

        public static IServiceCollection AddRedisCache(this IServiceCollection services)
        {
            var multiplexer = ConnectionMultiplexer.Connect("localhost,allowAdmin=true");
            var redis = multiplexer.GetServer("localhost:6379");
            redis.FlushAllDatabases();
            
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);
            return services;
        }

        public static IServiceCollection AddRepos(this IServiceCollection services)
        {
            var typesFromAssemblies = Assembly.GetAssembly(typeof(Setup)).DefinedTypes.Where(x => x.Name.EndsWith("Repo"));
            foreach (var type in typesFromAssemblies)
                services.Add(new ServiceDescriptor(type.GetInterface("I" + type.Name), type, ServiceLifetime.Scoped));

            return services;
        }

        public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
        {
            var typesFromAssemblies = Assembly.GetAssembly(typeof(IRepo<>)).DefinedTypes.Where(x => x.Name.EndsWith("Qh"));
            foreach (var type in typesFromAssemblies)
                services.Add(new ServiceDescriptor(type, type, ServiceLifetime.Scoped));

            return services;
        }

        public static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<LastTotalsMonitoringHostedService>();
            services.AddHostedService<SecuritiesMonitoringHostedService>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            var typesFromAssemblies = Assembly.GetAssembly(typeof(IRepo<>)).DefinedTypes.Where(x => x.Name.Contains("Service"));
            foreach (var type in typesFromAssemblies)
                services.Add(new ServiceDescriptor(type, type, ServiceLifetime.Singleton));

            return services;
        }

        public static IServiceCollection AddHelpers(this IServiceCollection services)
        {
            var typesFromAssemblies = Assembly.GetAssembly(typeof(Setup)).DefinedTypes.Where(x => x.Name.Contains("Helper"));
            foreach (var type in typesFromAssemblies)
                services.Add(new ServiceDescriptor(type.GetInterface("I" + type.Name), type, ServiceLifetime.Scoped));

            return services;
        }
    }
}
