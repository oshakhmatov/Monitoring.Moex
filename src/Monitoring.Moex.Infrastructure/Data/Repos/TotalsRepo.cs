using Microsoft.EntityFrameworkCore;
using Monitoring.Moex.Core.DataAccess.Repos;
using Monitoring.Moex.Core.Dto.SecurityTotals;
using Monitoring.Moex.Core.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Monitoring.Moex.Infrastructure.Data.Repos
{
    public class TotalsRepo : EfRepo<SecurityTotal>, ITotalsRepo
    {
        private const string HighestUpKey = "HighestUp";
        private const int HighestUpExpHours = 6;

        private const string HighestDownKey = "HighestDown";
        private const int HighestDownExpHours = 6;

        private const string ListTotalsKey = "ListTotals";
        private const int ListTotalsExpHours = 6;

        private const string MaxTradeDateKey = "MaxTradeDate";
        private const int MaxTradeDateExpHours = 6;

        private readonly DbSet<SecurityTotal> _securityTotals;
        private readonly IDatabase _redis;

        public TotalsRepo(AppDbContext dbContext, IConnectionMultiplexer redis) : base(dbContext)
        {
            _securityTotals = dbContext.Set<SecurityTotal>();

            _redis = redis.GetDatabase();
        }

        public async Task<List<SecurityTotalShortDto>> ListByClockAsync(long clock)
        {
            var redisKey = $"{ListTotalsKey}:{clock}";
            var cachedTotals = await _redis.StringGetAsync(redisKey);

            if (cachedTotals.HasValue)
            {
                return JsonConvert.DeserializeObject<List<SecurityTotalShortDto>>(cachedTotals);
            }

            var totals = await _securityTotals
                .Where(st => st.TradeClock == clock)
                .Include(st => st.Security)
                .Select(st => st.AsShortDto())
                .ToListAsync();

            await _redis.StringSetAsync(redisKey, JsonConvert.SerializeObject(totals), TimeSpan.FromHours(ListTotalsExpHours));

            return totals;
        }

        public async Task<long?> GetMaxTradeClockAsync()
        {
            var cachedDate = await _redis.StringGetAsync(MaxTradeDateKey);

            if (cachedDate.HasValue)
            {
                return long.Parse(cachedDate);
            }

            var clock = await _securityTotals.MaxAsync(s => s.TradeClock);

            if (clock is not null)
            {
                await _redis.StringSetAsync(MaxTradeDateKey, clock.ToString(), TimeSpan.FromHours(MaxTradeDateExpHours));
            }

            return clock;
        }

        public async Task<SecurityTotalShortDto?> GetHighestUpByClockAsync(long clock)
        {
            var redisKey = $"{HighestUpKey}:{clock}";
            var cachedHighestUp = await _redis.StringGetAsync(redisKey);

            if (cachedHighestUp.HasValue)
            {
                return JsonConvert.DeserializeObject<SecurityTotalShortDto>(cachedHighestUp);
            }

            var highestUp = await _dbContext.Set<SecurityTotal>()
                .Where(s => s.TradeClock == clock)
                .OrderByDescending(s => s.OpenCloseDelta)
                .Include(s => s.Security)
                .Select(s => s.AsShortDto())
                .FirstOrDefaultAsync();

            if (highestUp is not null)
            {
                await _redis.StringSetAsync(redisKey, JsonConvert.SerializeObject(highestUp), TimeSpan.FromHours(HighestUpExpHours));
            }

            return highestUp;
        }

        public async Task<SecurityTotalShortDto?> GetHighestDownByClockAsync(long clock)
        {
            var redisKey = $"{HighestDownKey}:{clock}";
            var cachedHighestDown = await _redis.StringGetAsync(redisKey);

            if (cachedHighestDown.HasValue)
            {
                return JsonConvert.DeserializeObject<SecurityTotalShortDto>(cachedHighestDown);
            }

            var highestDown = await _dbContext.Set<SecurityTotal>()
                .Where(t => t.TradeClock == clock)
                .OrderBy(t => t.OpenCloseDelta)
                .Include(t => t.Security)
                .Select(t => t.AsShortDto())
                .FirstOrDefaultAsync();

            if (highestDown is not null)
            {
                await _redis.StringSetAsync(redisKey, JsonConvert.SerializeObject(highestDown), TimeSpan.FromHours(HighestDownExpHours));
            }

            return highestDown;
        }
    }
}
